using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace appRestauranteDSW_WebApi.Data.Entities;

public partial class RestauranteContext : DbContext
{
    public RestauranteContext()
    {
    }

    public RestauranteContext(DbContextOptions<RestauranteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<caja> caja { get; set; }

    public virtual DbSet<cargo> cargo { get; set; }

    public virtual DbSet<categoria_plato> categoria_plato { get; set; }

    public virtual DbSet<cliente> cliente { get; set; }

    public virtual DbSet<comanda> comanda { get; set; }

    public virtual DbSet<comprobante> comprobante { get; set; }

    public virtual DbSet<detalle_comanda> detalle_comanda { get; set; }

    public virtual DbSet<detalle_comprobante> detalle_comprobante { get; set; }

    public virtual DbSet<empleado> empleado { get; set; }

    public virtual DbSet<establecimiento> establecimiento { get; set; }

    public virtual DbSet<estado_comanda> estado_comanda { get; set; }

    public virtual DbSet<mesa> mesa { get; set; }

    public virtual DbSet<metodo_pago> metodo_pago { get; set; }

    public virtual DbSet<plato> plato { get; set; }

    public virtual DbSet<tipo_comprobante> tipo_comprobante { get; set; }

    public virtual DbSet<usuario> usuario { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-NCJQ5QE;Database=RestauranteDSW;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<caja>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__caja__3213E83F7AA34C3A");

            entity.HasOne(d => d.establecimiento).WithMany(p => p.caja).HasConstraintName("FK__caja__establecim__5535A963");
        });

        modelBuilder.Entity<cargo>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__cargo__3213E83FF69F9D2C");
        });

        modelBuilder.Entity<categoria_plato>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__categori__3213E83F312E8BF6");
        });

        modelBuilder.Entity<cliente>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__cliente__3213E83FFF7D77B5");
        });

        modelBuilder.Entity<comanda>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__comanda__3213E83FEF9A57CA");

            entity.HasOne(d => d.empleado).WithMany(p => p.comanda).HasConstraintName("FK__comanda__emplead__48CFD27E");

            entity.HasOne(d => d.estado_comanda).WithMany(p => p.comanda).HasConstraintName("FK__comanda__estado___49C3F6B7");

            entity.HasOne(d => d.mesa).WithMany(p => p.comanda).HasConstraintName("FK__comanda__mesa_id__4AB81AF0");
        });

        modelBuilder.Entity<comprobante>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__comproba__3213E83F80079D57");

            entity.HasOne(d => d.caja).WithMany(p => p.comprobante).HasConstraintName("FK__comproban__caja___59FA5E80");

            entity.HasOne(d => d.cliente).WithMany(p => p.comprobante).HasConstraintName("FK__comproban__clien__5AEE82B9");

            entity.HasOne(d => d.comanda).WithMany(p => p.comprobante).HasConstraintName("FK__comproban__coman__5BE2A6F2");

            entity.HasOne(d => d.empleado).WithMany(p => p.comprobante).HasConstraintName("FK__comproban__emple__5CD6CB2B");

            entity.HasOne(d => d.tipo_comprobante).WithMany(p => p.comprobante).HasConstraintName("FK__comproban__tipo___5DCAEF64");
        });

        modelBuilder.Entity<detalle_comanda>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__detalle___3213E83FD9EE1FF0");

            entity.HasOne(d => d.comanda).WithMany(p => p.detalle_comanda).HasConstraintName("FK__detalle_c__coman__4D94879B");

            entity.HasOne(d => d.plato).WithMany(p => p.detalle_comanda).HasConstraintName("FK__detalle_c__plato__4E88ABD4");
        });

        modelBuilder.Entity<detalle_comprobante>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__detalle___3213E83F086335D7");

            entity.HasOne(d => d.comprobante).WithMany(p => p.detalle_comprobante).HasConstraintName("FK__detalle_c__compr__628FA481");

            entity.HasOne(d => d.metodo_pago).WithMany(p => p.detalle_comprobante).HasConstraintName("FK__detalle_c__metod__6383C8BA");
        });

        modelBuilder.Entity<empleado>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__empleado__3213E83F49AAF447");

            entity.HasOne(d => d.cargo).WithMany(p => p.empleado).HasConstraintName("FK__empleado__cargo___44FF419A");

            entity.HasOne(d => d.usuario).WithMany(p => p.empleado).HasConstraintName("FK__empleado__usuari__45F365D3");
        });

        modelBuilder.Entity<establecimiento>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__establec__3213E83F509C3A57");
        });

        modelBuilder.Entity<estado_comanda>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__estado_c__3213E83F1159492E");
        });

        modelBuilder.Entity<mesa>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__mesa__3213E83F6D606F32");
        });

        modelBuilder.Entity<metodo_pago>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__metodo_p__3213E83F56F88190");
        });

        modelBuilder.Entity<plato>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__plato__3213E83F69E37421");

            entity.HasOne(d => d.categoria_plato).WithMany(p => p.plato).HasConstraintName("FK__plato__categoria__3B75D760");
        });

        modelBuilder.Entity<tipo_comprobante>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__tipo_com__3213E83F3DF89D8C");
        });

        modelBuilder.Entity<usuario>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__usuario__3213E83F866A739E");

            entity.Property(e => e.verificado).HasDefaultValue(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
