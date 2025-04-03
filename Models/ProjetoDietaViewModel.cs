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
        public Dictionary<int, double> TotalProteinaPorRefeicao { get; set; } = new Dictionary<int, double>();
        public Dictionary<int, double> TotalKcalPorRefeicao { get; set; } = new Dictionary<int, double>();
        public Dictionary<int, double> TotalKjPorRefeicao { get; set; } = new Dictionary<int, double>();
        public double TotalProteinaGeral { get; set; }
        public double TotalKcalGeral { get; set; }
        public double TotalKjGeral { get; set; }
    }
}
