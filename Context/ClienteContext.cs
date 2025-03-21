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
                .HasOne(p => p.Paciente) // Relacionamento de um Projeto com um Paciente
                .WithMany(p => p.Projetos) // Um Paciente pode ter muitos Projetos
                .HasForeignKey(p => p.PacienteId) // Chave estrangeira
                .OnDelete(DeleteBehavior.Cascade); // Comportamento de exclusão em cascata

            modelBuilder.Entity<Imc>()
                .HasOne(i => i.Projeto)  // Relacionamento de um IMC com um Projeto
                .WithMany(p => p.Imcs)  // Um Projeto pode ter muitos IMCs
                .HasForeignKey(i => i.IdProjeto)  // Definindo a chave estrangeira
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Circunferencia>()
                .HasOne(c => c.Projeto) // Relacionamento de uma Circuferencia com um Projeto
                .WithMany(p => p.Circunferencias) // Um Projeto pode ter muitas Circuferencias
                .HasForeignKey(c => c.IdProjeto) // Chave estrangeira
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Pregas>()
                .HasOne(c => c.Projeto) // Relacionamento de uma Circuferencia com um Projeto
                .WithMany(p => p.Pregas) // Um Projeto pode ter muitas Circuferencias
                .HasForeignKey(c => c.IdProjeto) // Chave estrangeira
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Alimento>()
                .HasOne(a => a.Categoria) // Relacionamento de um Alimento com uma Categoria
                .WithMany(c => c.Alimentos) // Uma Categoria pode ter muitos Alimentos
                .HasForeignKey(a => a.IdCategoria_Alimento) // Chave estrangeira
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Refeicao_Alimento>()
                .HasKey(ra => new { ra.IdRefeicao, ra.IdAlimento }); // Definindo chave composta

            modelBuilder.Entity<Refeicao_Alimento>()
                .HasOne(ra => ra.Refeicao) // Relacionamento de uma Refeicao_Alimento com uma Refeicao
                .WithMany(r => r.Refeicao_Alimentos) // Uma Refeicao pode ter muitos Refeicao_Alimentos
                .HasForeignKey(ra => ra.IdRefeicao) // Chave estrangeira
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Refeicao_Alimento>()
                .HasOne(ra => ra.Alimento) // Relacionamento de uma Refeicao_Alimento com um Alimento
                .WithMany(a => a.Refeicao_Alimentos) // Um Alimento pode ter muitos Refeicao_Alimentos
                .HasForeignKey(ra => ra.IdAlimento) // Chave estrangeira
                .OnDelete(DeleteBehavior.Cascade);

            }
        }
    }
