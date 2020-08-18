using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PeliculasAPI
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PeliculasActores>().HasKey(x => new { x.ActorId, x.PeliculaId });

            modelBuilder.Entity<PeliculasGeneros>().HasKey(x => new { x.GeneroId, x.PeliculaId });

            modelBuilder.Entity<PeliculasSalasDeCine>().HasKey(x => new { x.PeliculaId, x.SalaDeCineId });

            SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<PeliculasActores> PeliculasActores { get; set; }
        public DbSet<PeliculasGeneros> PeliculasGeneros { get; set; }
        public DbSet<SalaDeCine> SalasDeCine { get; set; }
        public DbSet<PeliculasSalasDeCine> PeliculasSalasDeCines { get; set; }
        public DbSet<Review> Reviews { get; set; }

        private void SeedData(ModelBuilder modelBuilder)
        {
            //var rolAdminId = "1a40931e-15fa-4a30-9a2d-307525673e58";
            //var usuarioAdminId = "72a677a2-2384-4f39-86cb-3253bb9f65ad";
            //var username = "javiconlinea@gmail.com";
            //var pass = "polloyon123";

            //var rolAdmin = new IdentityRole
            //{
            //    Id = rolAdminId,
            //    Name = "Admin",
            //    NormalizedName = "Admin"
            //};

            //var passwordHasher = new PasswordHasher<IdentityUser>();
            //var usuarioAdmin = new IdentityUser()
            //{
            //    Id = usuarioAdminId,
            //    UserName = username,
            //    NormalizedUserName = username,
            //    Email = username,
            //    NormalizedEmail = username,
            //    PasswordHash = passwordHasher.HashPassword(null, pass)
            //};

            //modelBuilder.Entity<IdentityUser>().HasData(usuarioAdmin);
            //modelBuilder.Entity<IdentityRole>().HasData(rolAdmin);
            //modelBuilder.Entity<IdentityUserClaim<string>>().HasData(new IdentityUserClaim<string>()
            //{
            //    Id = 1,
            //    ClaimType = ClaimTypes.Role,
            //    UserId = usuarioAdminId,
            //    ClaimValue = "Admin"
            //});
        }
    }
}
