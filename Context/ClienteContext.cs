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
        public DbSet<Circunferencia> Circunferencias { get; set; }
        public DbSet<Pregas> Pregas { get; set; }
        public DbSet<Alimento> Alimentos { get; set; }
        public DbSet<Categoria_Alimento> Categorias_Alimentos { get; set; }
        public DbSet<Refeicao> Refeicoes { get; set; }

        public DbSet<Refeicao_Alimento> Refeicoes_Alimentos { get; set; }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Projeto>()
                .HasOne(p => p.Paciente)
                .WithMany(p => p.Projetos)
                .HasForeignKey(p => p.PacienteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Imc>()
                .HasOne(i => i.Projeto)
                .WithMany(p => p.Imcs)
                .HasForeignKey(i => i.IdProjeto)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Circunferencia>()
                .HasOne(c => c.Projeto)
                .WithMany(p => p.Circunferencias)
                .HasForeignKey(c => c.IdProjeto)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Pregas>()
                .HasOne(c => c.Projeto)
                .WithMany(p => p.Pregas)
                .HasForeignKey(c => c.IdProjeto)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Alimento>()
                .HasOne(a => a.Categoria)
                .WithMany(c => c.Alimentos)
                .HasForeignKey(a => a.IdCategoria_Alimento)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Refeicao_Alimento>()
                .HasKey(ra => new { ra.IdRefeicao, ra.IdAlimento });

            modelBuilder.Entity<Refeicao_Alimento>()
                .HasOne(ra => ra.Refeicao)
                .WithMany(r => r.Refeicao_Alimentos)
                .HasForeignKey(ra => ra.IdRefeicao)
                .OnDelete(DeleteBehavior.Restrict); // Alterado para evitar ciclo de cascata

            modelBuilder.Entity<Refeicao_Alimento>()
                .HasOne(ra => ra.Alimento)
                .WithMany(a => a.Refeicao_Alimentos)
                .HasForeignKey(ra => ra.IdAlimento)
                .OnDelete(DeleteBehavior.Restrict); // Alterado para evitar ciclo de cascata

            modelBuilder.Entity<Refeicao>()
                .HasOne(r => r.Projeto)
                .WithMany(p => p.Refeicoes)
                .HasForeignKey(r => r.IdProjeto)
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
