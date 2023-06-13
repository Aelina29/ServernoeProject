using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Floristic;

public partial class FloristicsContext : DbContext
{
    public FloristicsContext()
    {
    }

    public FloristicsContext(DbContextOptions<FloristicsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bouquet> Bouquets { get; set; }

    public virtual DbSet<Florist> Florists { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=floristic;Username=postgres;Password=postgres");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bouquet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_bouquet");

            entity.ToTable("bouquet", "floristic");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Florist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_florist");

            entity.ToTable("florist", "floristic");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("full_name");
            entity.Property(e => e.ShortName)
                .HasMaxLength(100)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("short_name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_order");

            entity.ToTable("order", "floristic");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("address");
            entity.Property(e => e.BouquetId).HasColumnName("bouquet_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.FloristId).HasColumnName("florist_id");
            entity.Property(e => e.Time).HasColumnName("time");

            entity.HasOne(d => d.Bouquet).WithMany(p => p.Orders)
                .HasForeignKey(d => d.BouquetId)
                .HasConstraintName("fk_fl_bouq");

            entity.HasOne(d => d.Florist).WithMany(p => p.Orders)
                .HasForeignKey(d => d.FloristId)
                .HasConstraintName("fk_fl_flor");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
