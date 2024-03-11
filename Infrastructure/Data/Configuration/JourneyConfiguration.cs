using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
