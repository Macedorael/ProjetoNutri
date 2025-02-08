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

        public DbSet<Imc> Imcs { get; set; }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
        modelBuilder.Entity<Projeto>()
            .HasOne(p => p.Paciente) // Relacionamento de um Projeto com um Paciente
            .WithMany(p => p.Projetos) // Um Paciente pode ter muitos Projetos
            .HasForeignKey(p => p.PacienteId) // Chave estrangeira
            .OnDelete(DeleteBehavior.Cascade); // Comportamento de exclusão em cascata

        modelBuilder.Entity<Imc>()
            .HasOne(i => i.Projeto)  // Relacionamento de um IMC com um Projeto
            .WithMany(p => p.Imcs)  // Um Projeto pode ter muitos IMCs
            .HasForeignKey(i => i.IdProjeto)  // Definindo a chave estrangeira
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}