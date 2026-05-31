using ERP.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP.Api.Data;

public class ErpDbContext(DbContextOptions<ErpDbContext> options) : DbContext(options)
{
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<TipoDescuento> TiposDescuento => Set<TipoDescuento>();
    public DbSet<Empleado> Empleados => Set<Empleado>();
    public DbSet<Planilla> Planillas => Set<Planilla>();
    public DbSet<DetallePlanilla> DetallesPlanilla => Set<DetallePlanilla>();
    public DbSet<TipoComprobante> TiposComprobante => Set<TipoComprobante>();
    public DbSet<Comprobante> Comprobantes => Set<Comprobante>();
    public DbSet<LibroContable> LibrosContables => Set<LibroContable>();
    public DbSet<ComprobanteLIbro> ComprobantesLibro => Set<ComprobanteLIbro>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rol>(e =>
        {
            e.ToTable("rol");
            e.HasKey(r => r.IdRol);
            e.Property(r => r.IdRol).HasColumnName("id_rol");
            e.Property(r => r.NombreRol).HasColumnName("nombre_rol").HasMaxLength(30).IsRequired();
            e.Property(r => r.Descripcion).HasColumnName("descripcion").HasMaxLength(200);
        });

        modelBuilder.Entity<Usuario>(e =>
        {
            e.ToTable("usuario");
            e.HasKey(u => u.IdUsuario);
            e.Property(u => u.IdUsuario).HasColumnName("id_usuario");
            e.Property(u => u.Username).HasColumnName("username").HasMaxLength(50).IsRequired();
            e.Property(u => u.PasswordHash).HasColumnName("password_hash").HasMaxLength(128).IsRequired();
            e.Property(u => u.NombreCompleto).HasColumnName("nombre_completo").HasMaxLength(100).IsRequired();
            e.Property(u => u.IdRol).HasColumnName("id_rol");
            e.Property(u => u.Activo).HasColumnName("activo");
            e.Property(u => u.UltimoAcceso).HasColumnName("ultimo_acceso");
            e.Property(u => u.RefreshToken).HasColumnName("refresh_token");
            e.Property(u => u.RefreshTokenExpiry).HasColumnName("refresh_token_expiry");
            e.HasOne(u => u.Rol).WithMany(r => r.Usuarios).HasForeignKey(u => u.IdRol);
        });

        modelBuilder.Entity<TipoDescuento>(e =>
        {
            e.ToTable("tipo_descuento");
            e.HasKey(t => t.IdTipo);
            e.Property(t => t.IdTipo).HasColumnName("id_tipo");
            e.Property(t => t.Nombre).HasColumnName("nombre").HasMaxLength(20).IsRequired();
            e.Property(t => t.Porcentaje).HasColumnName("porcentaje").HasPrecision(5, 4);
        });

        modelBuilder.Entity<Empleado>(e =>
        {
            e.ToTable("empleado");
            e.HasKey(x => x.IdEmpleado);
            e.Property(x => x.IdEmpleado).HasColumnName("id_empleado");
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(80).IsRequired();
            e.Property(x => x.Apellido).HasColumnName("apellido").HasMaxLength(80).IsRequired();
            e.Property(x => x.Dni).HasColumnName("dni").HasMaxLength(8).IsRequired();
            e.Property(x => x.Cargo).HasColumnName("cargo").HasMaxLength(80);
            e.Property(x => x.SalarioBase).HasColumnName("salario_base").HasPrecision(10, 2);
            e.Property(x => x.IdTipoDescuento).HasColumnName("id_tipo_descuento");
            e.Property(x => x.FechaIngreso).HasColumnName("fecha_ingreso");
            e.Property(x => x.Activo).HasColumnName("activo");
            e.HasOne(x => x.TipoDescuento).WithMany(t => t.Empleados).HasForeignKey(x => x.IdTipoDescuento);
        });

        modelBuilder.Entity<Planilla>(e =>
        {
            e.ToTable("planilla");
            e.HasKey(p => p.IdPlanilla);
            e.Property(p => p.IdPlanilla).HasColumnName("id_planilla");
            e.Property(p => p.IdEmpleado).HasColumnName("id_empleado");
            e.Property(p => p.Periodo).HasColumnName("periodo").HasMaxLength(7);
            e.Property(p => p.TotalBruto).HasColumnName("total_bruto").HasPrecision(10, 2);
            e.Property(p => p.TotalDescuentos).HasColumnName("total_descuentos").HasPrecision(10, 2);
            e.Property(p => p.TotalNeto).HasColumnName("total_neto").HasPrecision(10, 2);
            e.Property(p => p.FechaCalculo).HasColumnName("fecha_calculo");
            e.HasOne(p => p.Empleado).WithMany(em => em.Planillas).HasForeignKey(p => p.IdEmpleado);
            e.HasOne(p => p.Detalle).WithOne(d => d.Planilla).HasForeignKey<DetallePlanilla>(d => d.IdPlanilla);
        });

        modelBuilder.Entity<DetallePlanilla>(e =>
        {
            e.ToTable("detalle_planilla");
            e.HasKey(d => d.IdDetalle);
            e.Property(d => d.IdDetalle).HasColumnName("id_detalle");
            e.Property(d => d.IdPlanilla).HasColumnName("id_planilla");
            e.Property(d => d.Afp).HasColumnName("afp").HasPrecision(8, 2);
            e.Property(d => d.Onp).HasColumnName("onp").HasPrecision(8, 2);
            e.Property(d => d.Cts).HasColumnName("cts").HasPrecision(8, 2);
            e.Property(d => d.Gratificacion).HasColumnName("gratificacion").HasPrecision(8, 2);
            e.Property(d => d.HorasExtras).HasColumnName("horas_extras").HasPrecision(8, 2);
        });

        modelBuilder.Entity<TipoComprobante>(e =>
        {
            e.ToTable("tipo_comprobante");
            e.HasKey(t => t.IdTipo);
            e.Property(t => t.IdTipo).HasColumnName("id_tipo");
            e.Property(t => t.Nombre).HasColumnName("nombre").HasMaxLength(30).IsRequired();
            e.Property(t => t.SeriePrefix).HasColumnName("serie_prefix").HasMaxLength(5);
        });

        modelBuilder.Entity<Comprobante>(e =>
        {
            e.ToTable("comprobante");
            e.HasKey(c => c.IdComprobante);
            e.Property(c => c.IdComprobante).HasColumnName("id_comprobante");
            e.Property(c => c.IdTipoComprobante).HasColumnName("id_tipo_comprobante");
            e.Property(c => c.Numero).HasColumnName("numero").HasMaxLength(20).IsRequired();
            e.Property(c => c.Serie).HasColumnName("serie").HasMaxLength(10);
            e.Property(c => c.Ruc).HasColumnName("ruc").HasMaxLength(11).IsRequired();
            e.Property(c => c.RazonSocial).HasColumnName("razon_social").HasMaxLength(150);
            e.Property(c => c.Fecha).HasColumnName("fecha");
            e.Property(c => c.Monto).HasColumnName("monto").HasPrecision(10, 2);
            e.Property(c => c.Igv).HasColumnName("igv").HasPrecision(10, 2);
            e.Property(c => c.Activo).HasColumnName("activo");
            e.HasOne(c => c.TipoComprobante).WithMany(t => t.Comprobantes).HasForeignKey(c => c.IdTipoComprobante);
        });

        modelBuilder.Entity<LibroContable>(e =>
        {
            e.ToTable("libro_contable");
            e.HasKey(l => l.IdLibro);
            e.Property(l => l.IdLibro).HasColumnName("id_libro");
            e.Property(l => l.Tipo).HasColumnName("tipo").HasMaxLength(10).IsRequired();
            e.Property(l => l.Periodo).HasColumnName("periodo").HasMaxLength(7).IsRequired();
            e.Property(l => l.FechaGeneracion).HasColumnName("fecha_generacion");
            e.Property(l => l.Estado).HasColumnName("estado").HasMaxLength(15);
            e.Property(l => l.IdUsuario).HasColumnName("id_usuario");
            e.HasOne(l => l.Usuario).WithMany().HasForeignKey(l => l.IdUsuario);
        });

        modelBuilder.Entity<ComprobanteLIbro>(e =>
        {
            e.ToTable("comprobante_libro");
            e.HasKey(cl => new { cl.IdComprobante, cl.IdLibro });
            e.Property(cl => cl.IdComprobante).HasColumnName("id_comprobante");
            e.Property(cl => cl.IdLibro).HasColumnName("id_libro");
            e.Property(cl => cl.BaseImponible).HasColumnName("base_imponible").HasPrecision(10, 2);
            e.HasOne(cl => cl.Comprobante).WithMany().HasForeignKey(cl => cl.IdComprobante);
            e.HasOne(cl => cl.Libro).WithMany(l => l.ComprobantesLibro).HasForeignKey(cl => cl.IdLibro);
        });
    }
}
