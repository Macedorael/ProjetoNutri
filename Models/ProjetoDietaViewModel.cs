using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoNutri.Models
{
    public class ProjetoDietaViewModel
    {
        public Projeto Projeto { get; set; }
        public Alimento Alimento { get; set; }
        public List<Alimento> Alimentos { get; set; }
        public Refeicao Refeicao { get; set; }
        public List<Refeicao> Refeicaos { get; set; }
        public List<Refeicao_Alimento> Refeicao_Alimentos { get; set; }
    }
}
