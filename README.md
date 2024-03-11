# PRUEBA Biinteli:

e solicito crear una solución que permitiera recibir parametros origen "origin" y destino "destination" para proceder a consultar los vuelos que se disponen en la base de datos para de está manera calcular la ruta llegar al destino.

Se solicito que se implemente una solucion con una division lógica de 3 capas. Teniendo en cuenta las recomendaciones añadidas en el PDF de la prueba.

Para la solución del proyecto se usaron las siguientes herramientas tecnológicas:

- C#
- .NET
- SQL SERVER
- Visual Studio Community Edition 2022
- Git

## Estructura del Proyecto:

Se opto por implementar una lógica de 3 capas en el proyecto, se tomo está decisión ya que no se requería de una gran cantidad de entidades por modelar, ni tampoco se requeria de procesos de autenticación y verificación.

#### CORE:

Dentro de esta solución estoy almacenando mis entidades e interfaces que sin el modelo para estructurar mi base de datos y definir métodos propios y funcionalidad de mis entidades. A continuación, enumero las entidades de mi solución

- **Entities**: Carpeta en la que modelo cada una de las entidades de  mi proyecto.

```c#
namespace Core.Entities
{
    public class Journey : BaseEntity
    {
        [Required]
        public string Origin { get; set; }
        [Required]
        public string Destination { get; set; }
        [Required]
        public double Price { get; set; }
        public ICollection<Flight> Flights{ get; set; } = new List<Flight>();
    }
}
```



- **Interfaces**: Defino las interfaces relacionadas a mis entidades, adicionalmente creo las interfaces para mi unidad de trabajo "IUnitOfWork" y mi "IGenericRepository", que me permite agrupar una o mas operaciones en una única transacción:

```c#
namespace Core.Interfaces
{
    public interface IJourney : IGenericRepository<Journey>
    {
        Task<JArray> GetJsonFromApi();
        Task<IEnumerable<Journey>> MapToJourneys();
        Task SaveMappInfo();
        Task<IEnumerable<Object>> GetStepsInTrip(string origin, string destination);
    }
}
```



#### INFRASTRUCTURE:

Dentro de la librería de clases (Infrastructure), se crean las carpetas:

- **Data:** Se define toda la lógica que permite almacenar los datos en mi base de datos. Se crea una carpeta llamada "Configuration", en la que defino las relaciones y los constraints que tendran mi entidades y como se modelararan dentro de mi base de datos. 

​	Adicionalmente, defino mi archivo DbContext una capa de abstracción que permite la interacción con mi base de datos:

​	A continuación, se presenta el archivo de configuración para la entidad "Journey".

```c#
namespace Infrastructure.Data.Configuration
{
    public class JourneyConfiguration : IEntityTypeConfiguration<Journey>
    {
        public void Configure(EntityTypeBuilder<Journey> builder)
        {
            builder.ToTable("Journey");

            builder.HasKey( e => e.Id );
            builder.Property(e => e.Id);

            builder.Property(p => p.Origin)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property( p => p.Destination)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property( p => p.Price)
                .HasColumnType("double")
                .IsRequired();

            builder.HasMany(j => j.Flights)
                .WithMany(f => f.Journeys)
                .UsingEntity<Dictionary<string, object>>(
                    "JourneyFlight",
                    jf => jf.HasOne<Flight>().WithMany().HasForeignKey("FlightId"),
                    jf => jf.HasOne<Journey>().WithMany().HasForeignKey("JourneyId"),
                    jf =>
                    {
                        jf.HasKey("FlightId", "JourneyId");
                        jf.ToTable("JourneyFlights");
                    });
        }
    }
}
```

Se muestra el archivo de contexto "DbContext"

```c#
namespace Infrastructure.Data
{
    public class ViajesAPIContext : DbContext
    {
        public ViajesAPIContext(DbContextOptions<ViajesAPIContext> options) : base(options) { } 
        public DbSet<Journey> Journeys { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Transport> Transports { get; set; }
        
    }
}
```

- **Repositories:**

​	Dentro de la carpeta "Repositories" defino los repositorios de cada una de mis entidades, que me permiten implementar 	lógica definida en mis interfaces, también defino la lógica que me permitirá realizar operaciones básica de un API (CRUD).



​	Se define el método que permite obtener los vuelos y sus respectivas escalas, para luego ser mostradas en mi EDNPOINT

```c#
public async Task<IEnumerable<Object>> GetStepsInTrip(string origin, string destination)
{
    var steps = new List<object>();
    // Obtener los vuelos directos desde el origen hasta el destino
    var directFlights = await _context.Flights
        .Include(f => f.Transport)
        .Where(f => f.DepartureStation == origin && f.ArrivalStation == destination)
        .ToListAsync();
    // Si hay vuelos directos, agregarlos al viaje
    if (directFlights.Any())
    {
        var directStep = new
        {
            Journey = new
            {
                Origin = origin.ToUpper(),
                Destination = destination.ToUpper(),
                Price = directFlights.Sum(f => f.Price),
                Flights = directFlights.Select(f => new
                {
                    DepartureStation = f.DepartureStation,
                    ArrivalStation = f.ArrivalStation,
                    Price = f.Price,
                    Transport = new
                    {
                        FlightCarrier = f.Transport.FlightCarrier,
                        FlightNumber = f.Transport.FlightNumber
                    }
                })
            }
        };
        steps.Add(directStep);
    }
    // Buscar vuelos de conexión para las escalas
    var connectingFlights = await _context.Flights
        .Include(f => f.Transport)
        .Where(f => f.DepartureStation == origin || f.ArrivalStation == destination)
        .ToListAsync();
    foreach (var connectingFlight in connectingFlights)
    {
        // Encontrar el segundo tramo del vuelo de conexión
        var secondLeg = await _context.Flights
            .Include(f => f.Transport)
            .FirstOrDefaultAsync(f => f.DepartureStation == connectingFlight.ArrivalStation && f.ArrivalStation == destination);
        if (secondLeg != null)
        {
            var connectingStep = new
            {
                Journey = new
                {
                    Origin = origin.ToUpper(),
                    Destination = destination.ToUpper(),
                    Price = connectingFlight.Price + secondLeg.Price,
                    Flights = new[]
                    {
                new
                {
                    DepartureStation = origin,
                    ArrivalStation = connectingFlight.ArrivalStation,
                    Price = connectingFlight.Price,
                    Transport = new
                    {
                        FlightCarrier = connectingFlight.Transport.FlightCarrier,
                        FlightNumber = connectingFlight.Transport.FlightNumber
                    }
                },
                new
                {
                    DepartureStation = connectingFlight.ArrivalStation,
                    ArrivalStation = destination,
                    Price = secondLeg.Price,
                    Transport = new
                    {
                        FlightCarrier = secondLeg.Transport.FlightCarrier,
                        FlightNumber = secondLeg.Transport.FlightNumber
                    }
                }
            }
                }
            };
            steps.Add(connectingStep);
        }
    }
    return steps;
}
```



#### ViajesAPI

Dentro de mi solucion  webapi "ViajesAPI " se crearon las siguientes carpetas:

- **Controllers:** En está carpeta se encuentra cada uno de los controladores de mi aplicación, lo que permite ejecutar y dar acceso a cada uno de uno de los endpoints de mi aplicación:

  **JourneyController:**

  

   - **GET/ Journey :**  " https://localhost:7256/Journey "

​		Respuesta: 

```json
[
  {
    "id": 1,
    "origin": "BGA",
    "destination": "BTA",
    "price": 1000
  },
  {
    "id": 2,
    "origin": "BTA",
    "destination": "CTG",
    "price": 2000
  },
  {
    "id": 3,
    "origin": "CAL",
    "destination": "MED",
    "price": 1500
  },
  {
    "id": 4,
    "origin": "MED",
    "destination": "STA",
    "price": 4000
  }...
]
```



- **POST /  Journey:**  "https://localhost:7256/Journey "

Se requiere agregar desde el "Body" los datos a ingresar:

```json
{
  "origin": "ARC",
  "destination": "MED",
  "price": 1000
}
```

Response: 

```
Journeys saved successfully.
```



- **GET / Journey / StepsInFlight :**  " https://localhost:7256/Journey/StepsInFlight?origin=CAL%20&destination=CTG "

  parámetros: origin = CAL , destination = CTG;

Response:

```json
[
  {
    "journey": {
      "origin": "CAL",
      "destination": "CTG",
      "price": 1000,
      "flights": [
        {
          "departureStation": "CAL",
          "arrivalStation": "CTG",
          "price": 1000,
          "transport": {
            "flightCarrier": "AV",
            "flightNumber": "8080"
          }
        }
      ]
    }
  },
  {
    "journey": {
      "origin": "CAL",
      "destination": "CTG",
      "price": 2500,
      "flights": [
        {
          "departureStation": "CAL",
          "arrivalStation": "MED",
          "price": 1500,
          "transport": {
            "flightCarrier": "AV",
            "flightNumber": "8040"
          }
        },
        {
          "departureStation": "MED",
          "arrivalStation": "CTG",
          "price": 1000,
          "transport": {
            "flightCarrier": "AV",
            "flightNumber": "8070"
          }
        }
      ]
    }
  }
]
```



- **GET / Journey / {id}** :  " https://localhost:7256/Journey/2 "

​	parámetro: 2

RESPONSE: 

```json
{
  "id": 2,
  "origin": "BTA",
  "destination": "CTG",
  "price": 2000
}
```

- PUT / Journey / {Id} : " https://localhost:7256/Journey/3 "

  parámetro : id = 3;

  Modificar el body: {
    "origin": "VLLC",
    "destination": "BTA",
    "price": 2000
  }

RESPONSE:

```json
{
  "id": 3,
  "origin": "VLLC",
  "destination": "BTA",
  "price": 2000
}
```

- **DELETE / Journey / {id} :** " https://localhost:7256/Journey/3 "

  parámetro : 3

#### Extension: 

Se define la clase : " ApplicationServiceExtension" , donde se crean los métodos de extension que permiten agregar nuevos métodos a tipos existentes sin necesidad de modificar el original. Permitiendo extender la funcionalidad de mis Clases. 

```C#
namespace ViajesAPI.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static void ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new HeaderApiVersionReader("X-Version");
            });
        }
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options => {
                options.AddPolicy(
                    "CorsPolicy", builder =>
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                );
            });
        }

        public static void AddAplicationService(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
        public static void ConfigureRateLimiting(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddInMemoryRateLimiting();
            services.Configure<IpRateLimitOptions>(options => {
                options.EnableEndpointRateLimiting = true;
                options.StackBlockedRequests = false;
                options.HttpStatusCode = 429;
                options.RealIpHeader = "X-Real-Ip";
                options.GeneralRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint = "*",
                    Period = "10s",
                    Limit = 2
                }
            };
            });
        }
    }
}
```



