using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoNutri.Models
{
   public class Imc
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Projetos")]
        public int IdProjeto { get; set; }

        public Projeto Projeto { get; set; }

        [Required]
        public double Peso { get; set; }

        [Required]
        public double Altura { get; set; }

        public double ValorImc { get; set; } // Para armazenar o valor do IMC

        public string Classificacao { get; set; } // Para armazenar a classificação do IMC

        [DataType(DataType.Date)]
        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }

}