﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Entidades;
using System.Reflection;

namespace Data
{
    public class ApplicationDbContext : IdentityDbContext<UsuarioAplicacion, RolAplicacion, int, IdentityUserClaim<int>,
                                        RolUsuarioAplicacion, IdentityUserLogin<int>, IdentityRoleClaim<int>,
                                        IdentityUserToken<int>>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UsuarioAplicacion> UsuarioAplicacion { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Especialidad> Especialidades { get; set; }
        public DbSet<Medico> Medicos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
