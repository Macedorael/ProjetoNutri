using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoNutri.Models
{
    public class ProjetoImcViewModel
    {
        public Projeto Projeto { get; set; }
        public Imc Imc { get; set; } // IMC para ser criado
        public List<Imc> Imcs { get; set; } // Lista de IMCs associados ao projeto
    }
}