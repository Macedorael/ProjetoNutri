using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoNutri.Models
{
    public class Pregas
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Projetos")]
        public int IdProjeto { get; set; }

        public Projeto Projeto { get; set; }

        [Column("Tricipital")]
        public double? Tricipital { get; set; } 
        [Column("Bicipital")]
        public double? Bicipital { get; set; } 

        [Column("Abdominal")]
        public double? Abdominal { get; set; }

        [Column("Subescapular")]
        public double? Subescapular { get; set; } 

        [Column("AxilarMedia")]
        public double? AxilarMedia { get; set; } 

        [Column("Coxa")]
        public double? Coxa { get; set; }

        [Column("Toracica")]
        public double? Toracica { get; set; }

        [Column("SupraIliaca")]
        public double? SupraIliaca { get; set; } 
        [Column("SupraEspinal")]
        public double? SupraEspinal { get; set; } 

        [Column("Panturrilha")]
        public double? Panturrilha { get; set; }
        
        [DataType(DataType.Date)]
        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}