using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoNutri.Models
{
    public class Alimento
    {
    public int Id { get; set; }
    public string Nome { get; set; }
    [ForeignKey("Categoria_Alimento")]
    public int IdCategoria_Alimento { get; set; }
    public  Categoria_Alimento Categoria { get; set; }
    public double Energia_Kcal { get; set; }
    public double Energia_KJ { get; set; }
    public double Proteina { get; set; }

    public ICollection<Refeicao_Alimento> Refeicao_Alimentos { get; set; }
    }
}