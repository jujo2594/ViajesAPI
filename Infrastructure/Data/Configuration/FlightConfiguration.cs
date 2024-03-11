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
    public class FlightConfiguration : IEntityTypeConfiguration<Flight>
    {
        public void Configure(EntityTypeBuilder<Flight> builder)
        {
            builder.ToTable("Flight");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id);

            builder.Property(p => p.DepartureStation)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.ArrivalStation)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Price)
                .HasColumnType("double")
                .IsRequired();

            builder.HasOne(p => p.Transport)
                .WithMany(p => p.Flights)
                .HasForeignKey(p => p.IdTransportFk)
                .IsRequired();

            builder.HasOne(p => p.Transport)
                .WithMany(p => p.Flights)
                .HasForeignKey(p => p.IdTransportFk)
                .IsRequired();
        }
    }
}
