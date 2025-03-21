using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoNutri.Models
{
    public class Refeicao
    {
        public int Id { get; set; }

       
        public string Nome { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        [DataType(DataType.Date)]
        [Display(Name = "Data de Edição")]
        public DateTime DataEdicao { get; set; } = DateTime.Now;
        public virtual ICollection<Refeicao_Alimento> Refeicao_Alimentos { get; set; }
        

    }
}