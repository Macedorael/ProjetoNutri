using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjetoNutri.Models;


namespace ProjetoNutri.Context
{
    public class ClienteContext : DbContext
    {
        public ClienteContext(DbContextOptions<ClienteContext> options) : base(options) 
        {
            
        }

        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Projeto> Projetos { get; set; }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Projeto>().HasOne(p => p.Paciente).WithMany().HasForeignKey(p => p.PacienteId)
            .OnDelete(DeleteBehavior.Cascade);

        }
    }
}