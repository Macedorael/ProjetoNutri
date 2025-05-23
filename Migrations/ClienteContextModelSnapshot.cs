﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjetoNutri.Context;

#nullable disable

namespace ProjetoNutri.Migrations
{
    [DbContext(typeof(ClienteContext))]
    partial class ClienteContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ProjetoNutri.Models.Agendamento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime2");

                    b.Property<string>("GoogleEventId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("Hora")
                        .HasColumnType("time");

                    b.Property<int>("IdPaciente")
                        .HasColumnType("int");

                    b.Property<string>("Modalidade")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Observacao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Pacientes")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdPaciente");

                    b.ToTable("Agendamentos");
                });

            modelBuilder.Entity("ProjetoNutri.Models.Alimento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Calcio")
                        .HasColumnType("float");

                    b.Property<double>("Carboidrato")
                        .HasColumnType("float");

                    b.Property<double>("Cinzas")
                        .HasColumnType("float");

                    b.Property<double>("Cobre")
                        .HasColumnType("float");

                    b.Property<double>("Colesterol")
                        .HasColumnType("float");

                    b.Property<double>("Energia_KJ")
                        .HasColumnType("float");

                    b.Property<double>("Energia_Kcal")
                        .HasColumnType("float");

                    b.Property<double>("Ferro")
                        .HasColumnType("float");

                    b.Property<double>("Fibra_Alimentar")
                        .HasColumnType("float");

                    b.Property<double>("Fosforo")
                        .HasColumnType("float");

                    b.Property<int>("IdCategoria_Alimento")
                        .HasColumnType("int");

                    b.Property<double>("Lipidio")
                        .HasColumnType("float");

                    b.Property<double>("Magnesio")
                        .HasColumnType("float");

                    b.Property<double>("Manganes")
                        .HasColumnType("float");

                    b.Property<double>("Niacina")
                        .HasColumnType("float");

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Piridoxina")
                        .HasColumnType("float");

                    b.Property<double>("Potassio")
                        .HasColumnType("float");

                    b.Property<double>("Proteina")
                        .HasColumnType("float");

                    b.Property<double>("RAE")
                        .HasColumnType("float");

                    b.Property<double>("RE")
                        .HasColumnType("float");

                    b.Property<double>("Retinol")
                        .HasColumnType("float");

                    b.Property<double>("Riboflavina")
                        .HasColumnType("float");

                    b.Property<double>("Sodio")
                        .HasColumnType("float");

                    b.Property<double>("Tiamina")
                        .HasColumnType("float");

                    b.Property<double>("Vitamina_C")
                        .HasColumnType("float");

                    b.Property<double>("Zinco")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("IdCategoria_Alimento");

                    b.ToTable("Alimentos");
                });

            modelBuilder.Entity("ProjetoNutri.Models.Categoria_Alimento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categorias_Alimentos");
                });

            modelBuilder.Entity("ProjetoNutri.Models.Circunferencia", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Abdome")
                        .HasColumnType("float")
                        .HasColumnName("Abdome");

                    b.Property<double>("Antebracodireito")
                        .HasColumnType("float")
                        .HasColumnName("Antebraçodireito");

                    b.Property<double>("Antebracoesquerdo")
                        .HasColumnType("float")
                        .HasColumnName("Antebraçoesquerdo");

                    b.Property<double>("Bracodireito")
                        .HasColumnType("float")
                        .HasColumnName("Braçodireito");

                    b.Property<double>("Bracodireitocontraido")
                        .HasColumnType("float")
                        .HasColumnName("Braçodireitocontraido");

                    b.Property<double>("Bracoesquerdo")
                        .HasColumnType("float")
                        .HasColumnName("Braçoesquerdo");

                    b.Property<double>("Bracoesquerdocontraido")
                        .HasColumnType("float")
                        .HasColumnName("Braçoesquerdocontraido");

                    b.Property<double>("Cintura")
                        .HasColumnType("float")
                        .HasColumnName("Cintura");

                    b.Property<double>("Coxadistaldireita")
                        .HasColumnType("float")
                        .HasColumnName("Coxadistaldireita");

                    b.Property<double>("Coxadistalesquerda")
                        .HasColumnType("float")
                        .HasColumnName("Coxadistalesquerda");

                    b.Property<double>("Coxamedialdireita")
                        .HasColumnType("float")
                        .HasColumnName("Coxamedialdireita");

                    b.Property<double>("Coxamedialesquerda")
                        .HasColumnType("float")
                        .HasColumnName("Coxamedialesquerda");

                    b.Property<double>("Coxaproximaldireita")
                        .HasColumnType("float")
                        .HasColumnName("Coxaproximaldireita");

                    b.Property<double>("Coxaproximalesquerda")
                        .HasColumnType("float")
                        .HasColumnName("Coxaproximalesquerda");

                    b.Property<int>("IdProjeto")
                        .HasColumnType("int");

                    b.Property<double>("Ombro")
                        .HasColumnType("float")
                        .HasColumnName("Ombro");

                    b.Property<double>("Panturrilhadireita")
                        .HasColumnType("float")
                        .HasColumnName("Panturrilhadireita");

                    b.Property<double>("Panturrilhaesquerda")
                        .HasColumnType("float")
                        .HasColumnName("Panturrilhaesquerda");

                    b.Property<double>("Pescoco")
                        .HasColumnType("float")
                        .HasColumnName("Pescoço");

                    b.Property<double>("Quadril")
                        .HasColumnType("float")
                        .HasColumnName("Quadril");

                    b.Property<double>("Torax")
                        .HasColumnType("float")
                        .HasColumnName("Torax");

                    b.HasKey("Id");

                    b.HasIndex("IdProjeto");

                    b.ToTable("Circunferencias");
                });

            modelBuilder.Entity("ProjetoNutri.Models.Imc", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Altura")
                        .HasColumnType("float");

                    b.Property<string>("Classificacao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdProjeto")
                        .HasColumnType("int");

                    b.Property<double>("Peso")
                        .HasColumnType("float");

                    b.Property<double>("ValorImc")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("IdProjeto");

                    b.ToTable("Imcs");
                });

            modelBuilder.Entity("ProjetoNutri.Models.Paciente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataNascimento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Instagram")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sexo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sobrenome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Telefone")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Pacientes");
                });

            modelBuilder.Entity("ProjetoNutri.Models.Pregas", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double?>("Abdominal")
                        .HasColumnType("float")
                        .HasColumnName("Abdominal");

                    b.Property<double?>("AxilarMedia")
                        .HasColumnType("float")
                        .HasColumnName("AxilarMedia");

                    b.Property<double?>("Bicipital")
                        .HasColumnType("float")
                        .HasColumnName("Bicipital");

                    b.Property<double?>("Coxa")
                        .HasColumnType("float")
                        .HasColumnName("Coxa");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdProjeto")
                        .HasColumnType("int");

                    b.Property<double?>("Panturrilha")
                        .HasColumnType("float")
                        .HasColumnName("Panturrilha");

                    b.Property<double?>("Subescapular")
                        .HasColumnType("float")
                        .HasColumnName("Subescapular");

                    b.Property<double?>("SupraEspinal")
                        .HasColumnType("float")
                        .HasColumnName("SupraEspinal");

                    b.Property<double?>("SupraIliaca")
                        .HasColumnType("float")
                        .HasColumnName("SupraIliaca");

                    b.Property<double?>("Toracica")
                        .HasColumnType("float")
                        .HasColumnName("Toracica");

                    b.Property<double?>("Tricipital")
                        .HasColumnType("float")
                        .HasColumnName("Tricipital");

                    b.HasKey("Id");

                    b.HasIndex("IdProjeto");

                    b.ToTable("Pregas");
                });

            modelBuilder.Entity("ProjetoNutri.Models.Projeto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("datetime2");

                    b.Property<string>("NomeProjeto")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("PacienteId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PacienteId");

                    b.ToTable("Projetos");
                });

            modelBuilder.Entity("ProjetoNutri.Models.Refeicao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataEdicao")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdProjeto")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdProjeto");

                    b.ToTable("Refeicoes");
                });

            modelBuilder.Entity("ProjetoNutri.Models.Refeicao_Alimento", b =>
                {
                    b.Property<int>("IdRefeicao")
                        .HasColumnType("int");

                    b.Property<int>("IdAlimento")
                        .HasColumnType("int");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("datetime2");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("IdProjeto")
                        .HasColumnType("int");

                    b.Property<double>("Quantidade")
                        .HasColumnType("float");

                    b.HasKey("IdRefeicao", "IdAlimento");

                    b.HasIndex("IdAlimento");

                    b.HasIndex("IdProjeto");

                    b.ToTable("Refeicoes_Alimentos");
                });

            modelBuilder.Entity("ProjetoNutri.Models.Agendamento", b =>
                {
                    b.HasOne("ProjetoNutri.Models.Paciente", "Paciente")
                        .WithMany("Agendamentos")
                        .HasForeignKey("IdPaciente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Paciente");
                });

            modelBuilder.Entity("ProjetoNutri.Models.Alimento", b =>
                {
                    b.HasOne("ProjetoNutri.Models.Categoria_Alimento", "Categoria")
                        .WithMany("Alimentos")
                        .HasForeignKey("IdCategoria_Alimento")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categoria");
                });

            modelBuilder.Entity("ProjetoNutri.Models.Circunferencia", b =>
                {
                    b.HasOne("ProjetoNutri.Models.Projeto", "Projeto")
                        .WithMany("Circunferencias")
                        .HasForeignKey("IdProjeto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Projeto");
                });

            modelBuilder.Entity("ProjetoNutri.Models.Imc", b =>
                {
                    b.HasOne("ProjetoNutri.Models.Projeto", "Projeto")
                        .WithMany("Imcs")
                        .HasForeignKey("IdProjeto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Projeto");
                });

            modelBuilder.Entity("ProjetoNutri.Models.Pregas", b =>
                {
                    b.HasOne("ProjetoNutri.Models.Projeto", "Projeto")
                        .WithMany("Pregas")
                        .HasForeignKey("IdProjeto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Projeto");
                });

            modelBuilder.Entity("ProjetoNutri.Models.Projeto", b =>
                {
                    b.HasOne("ProjetoNutri.Models.Paciente", "Paciente")
                        .WithMany("Projetos")
                        .HasForeignKey("PacienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Paciente");
                });

            modelBuilder.Entity("ProjetoNutri.Models.Refeicao", b =>
                {
                    b.HasOne("ProjetoNutri.Models.Projeto", "Projeto")
                        .WithMany("Refeicoes")
                        .HasForeignKey("IdProjeto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Projeto");
                });

            modelBuilder.Entity("ProjetoNutri.Models.Refeicao_Alimento", b =>
                {
                    b.HasOne("ProjetoNutri.Models.Alimento", "Alimento")
                        .WithMany("Refeicao_Alimentos")
                        .HasForeignKey("IdAlimento")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjetoNutri.Models.Projeto", "Projeto")
                        .WithMany("Refeicoes_Alimentos")
                        .HasForeignKey("IdProjeto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjetoNutri.Models.Refeicao", "Refeicao")
                        .WithMany("Refeicao_Alimentos")
                        .HasForeignKey("IdRefeicao")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Alimento");

                    b.Navigation("Projeto");

                    b.Navigation("Refeicao");
                });

            modelBuilder.Entity("ProjetoNutri.Models.Alimento", b =>
                {
                    b.Navigation("Refeicao_Alimentos");
                });

            modelBuilder.Entity("ProjetoNutri.Models.Categoria_Alimento", b =>
                {
                    b.Navigation("Alimentos");
                });

            modelBuilder.Entity("ProjetoNutri.Models.Paciente", b =>
                {
                    b.Navigation("Agendamentos");

                    b.Navigation("Projetos");
                });

            modelBuilder.Entity("ProjetoNutri.Models.Projeto", b =>
                {
                    b.Navigation("Circunferencias");

                    b.Navigation("Imcs");

                    b.Navigation("Pregas");

                    b.Navigation("Refeicoes");

                    b.Navigation("Refeicoes_Alimentos");
                });

            modelBuilder.Entity("ProjetoNutri.Models.Refeicao", b =>
                {
                    b.Navigation("Refeicao_Alimentos");
                });
#pragma warning restore 612, 618
        }
    }
}
