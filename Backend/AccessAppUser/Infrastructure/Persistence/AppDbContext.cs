using Microsoft.EntityFrameworkCore;
using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Infrastructure.Persistence
{
    /// <summary>
    /// Contexto de base de datos principal para la aplicación.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Definición de DbSet para las entidades
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Permission> Permissions { get; set; } = null!;
        public DbSet<Area> Areas { get; set; } = null!;
        public DbSet<Profile> Profiles { get; set; } = null!;
        public DbSet<AreaProfile> AreaProfiles { get; set; } = null!;
        public DbSet<GesPass> GesPasses { get; set; } = null!; 
        public DbSet<RolePermission> RolePermissions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de UserProfile uno a uno
            modelBuilder.Entity<Profile>()
                .HasOne(p => p.User)
                .WithOne(u => u.Profile)
                .HasForeignKey<Profile>(p => p.Id)
                .OnDelete(DeleteBehavior.Cascade);            

            // Configuración de la tabla intermedia AreaProfile muchos a muchos
            modelBuilder.Entity<AreaProfile>()
                .HasKey(ap => new { ap.AreaId, ap.ProfileId }); //Llave compuesta

            modelBuilder.Entity<AreaProfile>()
                .HasOne(ap => ap.Area)
                .WithMany(a => a.AreaProfiles)
                .HasForeignKey(ap => ap.AreaId);

            modelBuilder.Entity<AreaProfile>()
                .HasOne(ap => ap.Profile)
                .WithMany(p => p.AreaProfiles)
                .HasForeignKey(ap => ap.ProfileId);

            // Relaciones muchos a muchos entre Rol y Area
            modelBuilder.Entity<Role>()
                .HasMany(r => r.Areas)
                .WithMany(a => a.Roles)
                .UsingEntity(j => j.ToTable("RoleArea"));

            // Relación muchos a muchos entre User y Role
            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity(j => j.ToTable("UserRole"));

            modelBuilder.Entity<User>()
                .Property(u =>u.IsActive)
                .HasDefaultValue(true)
                .IsRequired();

            // Configuración de la tabla intermedia RolePermission
            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);    

            // Relación uno a uno entre User y GesPass para cambio de contraseña
            modelBuilder.Entity<GesPass>()
                .HasKey(gp => gp.Id); 

            modelBuilder.Entity<GesPass>()
                .HasOne(gp => gp.User)
                .WithOne(u => u.GesPass)
                .HasForeignKey<GesPass>(gp => gp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GesPass>()
                .Property(gp => gp.ResetToken)
                .HasMaxLength(20)
                .IsRequired();

            modelBuilder.Entity<GesPass>()
                .Property(gp => gp.TokenExpiration)
                .IsRequired();

            // Otras configuraciones adicionales personalizadas
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(r => r.Name)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Permission>()
                .Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Area>()
                .Property(a => a.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<GesPass>()
                .Property(gp => gp.ResetToken)
                .HasMaxLength(20)
                .IsRequired();
        }
    }
}
