using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoNutri.Models
{
    public class ProjetoImcViewModel
    {
        public Projeto Projeto { get; set; }
        public Imc Imc { get; set; }
        public List<Imc> Imcs { get; set; }
        public Circunferencia Circunferencia { get; set; }
        public List<Circunferencia> Circunferencias { get; set; }
        public List<Pregas> Prega { get; set; }
    }
}