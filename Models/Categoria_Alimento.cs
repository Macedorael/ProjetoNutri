using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoNutri.Models
{
    public class Categoria_Alimento
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public ICollection<Alimento> Alimentos { get; set; }
    }
}