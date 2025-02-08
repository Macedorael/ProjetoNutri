using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoNutri.Models
{
    public class Projeto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do projeto é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do projeto deve ter no máximo 100 caracteres.")]
        public string NomeProjeto { get; set; }

        // Chave estrangeira para o Paciente
        [ForeignKey("Pacientes")]
        public int PacienteId { get; set; }

        // Propriedade de navegação para a entidade Pacientes
        public Paciente Paciente { get; set; }
        // Data de criação do projeto
        [DataType(DataType.Date)]
        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now; // Valor padrão é a data atual

        // Indica se o projeto está ativo
        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true; // Valor padrão é true
    }
}