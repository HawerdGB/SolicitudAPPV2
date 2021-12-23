using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SolicitudesAppV2.Models;

namespace SolicitudesAppV2.Data
{
    public class SolicitudesAppV2Context : DbContext
    {
        public SolicitudesAppV2Context (DbContextOptions<SolicitudesAppV2Context> options)
            : base(options)
        {
        }

        public DbSet<SolicitudesAppV2.Models.Persona> Persona { get; set; }

        public DbSet<SolicitudesAppV2.Models.Estado> Estado { get; set; }

        public DbSet<SolicitudesAppV2.Models.Solicitud> Solicitud { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Persona>().ToTable("Persona");
            modelBuilder.Entity<Estado>().ToTable("Estado");
            modelBuilder.Entity<Solicitud>().ToTable("Solicitud");
        }

    }
}
