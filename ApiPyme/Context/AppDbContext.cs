

using ApiPyme.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiPyme.Context
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<UsuarioRol> UsuarioRoles { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Inventario> Inventarios { get; set; }
        public DbSet<Rol> Rols { get; set; }
        public DbSet<UsuarioRol> UsuarioRol { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<DetalleCompra> DetalleCompras { get; set; }
        public DbSet<Comprobante> Comprobantes { get; set; }
        public DbSet<DetalleComprobante> DetalleComprobantes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Establecer el nombre de la tabla al nombre de la entidad (sin pluralizar)
                modelBuilder.Entity(entity.Name).ToTable(entity.ClrType.Name);                
            }

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasColumnType("decimal(10,2)");
            // relacion usuario tabla usuariorol
            modelBuilder.Entity<UsuarioRol>()
            .HasOne(ur => ur.usuario)
            .WithMany(u => u.UsuarioRoles)
            .HasForeignKey(ur => ur.IdUsuario);
            // relacion rol tabla usuariorol
            modelBuilder.Entity<UsuarioRol>()
            .HasOne(ur => ur.rol)
            .WithMany(r => r.UsuarioRoles)
            .HasForeignKey(ur => ur.IdRol);
            // realcion producto usuario proveedor
            modelBuilder.Entity<Producto>()
            .HasOne(p => p.usuarioProveedor)
            .WithMany(pro => pro.Productos )
            .HasForeignKey(p => p.IdUsuarioProveedor);
            // realacion pedido comerciante
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.usuarioComerciante)
                .WithMany(u => u.PedidosComerciante)
                .HasForeignKey(p => p.IdUsuarioComerciante);
            // relacion pedido proveedor
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.usuarioProveedor)
                .WithMany(u => u.PedidosProveedor)
                .HasForeignKey(p => p.IdUsuarioProveedor);
            // realacion compra comerciante
            modelBuilder.Entity<Compra>()
                .HasOne(c => c.usuarioComerciante)
                .WithMany(u => u.ComprasComerciante)
                .HasForeignKey(c => c.IdUsuarioComerciante);
            // relacion compra proveedor
            modelBuilder.Entity<Compra>()
                .HasOne(c => c.usuarioProveedor)
                .WithMany(u => u.ComprasProveedor)
                .HasForeignKey(c => c.IdUsuarioProveedor);
            modelBuilder.Entity<DetalleCompra>()
                .HasOne(d => d.compra)
                .WithMany(c => c.detallesCompra)
                .HasForeignKey(d => d.IdCompra);
            modelBuilder.Entity<DetalleCompra>()
                .HasOne(d => d.producto)
                .WithMany(p => p.detallesCompra)
                .HasForeignKey(d => d.IdProducto);

            // realacion comprobante comerciante
            modelBuilder.Entity<Comprobante>()
                .HasOne(c => c.usuarioComerciante)
                .WithMany(u => u.ComprobantesComerciante)
                .HasForeignKey(c => c.IdUsuarioComerciante);
            // relacion comprobante cliente
            modelBuilder.Entity<Comprobante>()
                .HasOne(c => c.usuarioCliente)
                .WithMany(u => u.ComprobantesCliente)
                .HasForeignKey(c => c.IdUsuarioCliente);
            modelBuilder.Entity<DetalleComprobante>()
                .HasOne(d => d.comprobante)
                .WithMany(c => c.detallesComprobante)
                .HasForeignKey(d => d.IdComprobante);
            modelBuilder.Entity<DetalleComprobante>()
                .HasOne(d => d.producto)
                .WithMany(p => p.detallesComprobante)
                .HasForeignKey(d => d.IdProducto);

        }
    }
}
