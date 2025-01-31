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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de UserProfile
            modelBuilder.Entity<Profile>()
                .HasOne(p => p.User)
                .WithOne(u => u.Profile)
                .HasForeignKey<Profile>(p => p.Id);

            // Configuración de la tabla intermedia AreaProfile
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
            // Cambio de contraseñas
            modelBuilder.Entity<GesPass>()
                .HasOne(gp => gp.User)
                .WithOne(u => u.GesPass)
                .HasForeignKey<GesPass>(gp => gp.UserId);
            modelBuilder.Entity<GesPass>()
                .Property(gp => gp.ResetToken)
                .HasMaxLength(20)
                .IsRequired();
            modelBuilder.Entity<GesPass>()
                .Property(gp => gp.TokenExpiration)
                .IsRequired();

            // Otras configuraciones personalizadas (opcional)
            // Ejemplo: Longitud de cadenas
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(r => r.Name)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
