using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoNutri.Models
{
    public class Circunferencia
    {
        [Key]
        public int Id { get; set; }

        // Relacionamento com a tabela Projetos
        [ForeignKey("Projetos")]
        public int IdProjeto { get; set; }

        public Projeto Projeto { get; set; }

        [Column("Pescoço")]
        public double Pescoco { get; set; }

        [Column("Ombro")]
        public double Ombro { get; set; }

        [Column("Torax")]
        public double Torax { get; set; }

        [Column("Braçodireito")]
        public double Bracodireito { get; set; }

        [Column("Braçoesquerdo")]
        public double Bracoesquerdo { get; set; }

        [Column("Braçodireitocontraido")]
        public double Bracodireitocontraido { get; set; }

        [Column("Braçoesquerdocontraido")]
        public double Bracoesquerdocontraido { get; set; }

        [Column("Antebraçodireito")]
        public double Antebracodireito { get; set; }

        [Column("Antebraçoesquerdo")]
        public double Antebracoesquerdo { get; set; }

        [Column("Cintura")]
        public double Cintura { get; set; }

        [Column("Abdome")]
        public double Abdome { get; set; }

        [Column("Quadril")]
        public double Quadril { get; set; }

        [Column("Coxadistaldireita")]
        public double Coxadistaldireita { get; set; }

        [Column("Coxadistalesquerda")]
        public double Coxadistalesquerda { get; set; }

        [Column("Coxamedialdireita")]
        public double Coxamedialdireita { get; set; }

        [Column("Coxamedialesquerda")]
        public double Coxamedialesquerda { get; set; }

        [Column("Coxaproximaldireita")]
        public double Coxaproximaldireita { get; set; }

        [Column("Coxaproximalesquerda")]
        public double Coxaproximalesquerda { get; set; }

        [Column("Panturrilhadireita")]
        public double Panturrilhadireita { get; set; }

        [Column("Panturrilhaesquerda")]
        public double Panturrilhaesquerda { get; set; }

        [Column("Classificação")]
        public string Classificacao { get; set; }

        [Column("Rcq")]
        public double Rcq { get; set; }
    }
}