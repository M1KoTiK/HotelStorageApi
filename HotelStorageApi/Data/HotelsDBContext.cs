using System;
using System.Collections.Generic;
using HotelStorageApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelStorageApi.Data;

public partial class HotelsDBContext : DbContext
{
    public HotelsDBContext()
    {
    }

    public HotelsDBContext(DbContextOptions<HotelsDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Hotel> Hotels { get; set; }

    public virtual DbSet<HotelName> HotelNames { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){ }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cities__3214EC07B9C63ADC");

            entity.HasIndex(e => e.CityName, "UQ__Cities__886159E5727A6FE6").IsUnique();

            entity.Property(e => e.CityName).HasMaxLength(100);
        });

        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Hotels__3214EC074540D25E");

            entity.Property(e => e.StandardRoomPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.City).WithMany(p => p.Hotels)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK_Hotels_CityId");

            entity.HasOne(d => d.HotelName).WithMany(p => p.Hotels)
                .HasForeignKey(d => d.HotelNameId)
                .HasConstraintName("FK_Hotels_HotelNameId");
        });

        modelBuilder.Entity<HotelName>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HotelNam__3214EC07173A646E");

            entity.HasIndex(e => e.Name, "UQ__HotelNam__737584F6F439A814").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
