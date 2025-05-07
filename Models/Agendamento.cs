using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoNutri.Models
{
    public class Agendamento
    {
        public int Id { get; set; }

        [Required]
        public int IdPaciente { get; set; }

        [ForeignKey("Pacientes")]
        public Paciente Paciente { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan Hora { get; set; }

        public string? Observacao { get; set; }

        public string? GoogleEventId { get; set; }

    }
}