using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoNutri.Models
{
    public class Paciente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }
        public string Sexo { get; set; }
        public int Telefone { get; set; }
        public string Instagram { get; set; }
        [DataType(DataType.Date)]
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

         public ICollection<Projeto> Projetos { get; set; }
    }
}