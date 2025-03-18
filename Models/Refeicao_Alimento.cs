using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoNutri.Models
{
    public class Refeicao_Alimento
    {
        public int Id { get; set; }
        [ForeignKey("Refeicao")]
        public int IdRefeicao { get; set; }
        public Refeicao Refeicao { get; set; }
        
        [ForeignKey("Alimento")]
        public int IdAlimento { get; set; }
        public Alimento Alimento { get; set; }
        public double Quantidade { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}