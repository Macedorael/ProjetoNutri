using Microsoft.EntityFrameworkCore;
using ProjetoNutri.Context;
using ProjetoNutri.Models;

namespace ProjetoNutri.Services
{
    public class AntropometriaService
    {
        private readonly ClienteContext _context;
        private readonly CalculosDobras _calculosDobras;

        public AntropometriaService(ClienteContext context, CalculosDobras calculosDobras)
        {
            _context = context;
            _calculosDobras = calculosDobras;
        }

        public Dictionary<string, object> CalcularAntropometria(int projetoId)
        {
            var projeto = _context.Projetos.Include(p => p.Paciente).FirstOrDefault(p => p.Id == projetoId);
            var imc = _context.Imcs.Include(i => i.Projeto).ThenInclude(p => p.Paciente).FirstOrDefault(i => i.Id == projetoId);
            var prega = _context.Pregas.Include(p => p.Projeto).ThenInclude(p => p.Paciente).FirstOrDefault(p => p.IdProjeto == projetoId);

            DateTime dataNascimento = prega.Projeto.Paciente.DataNascimento;
            int idade = ProjetoNutri.Services.CalculoIdade.Calcular(dataNascimento);

            if (projeto == null || imc == null || prega == null)
                return new Dictionary<string, object> { { "Erro", "Dados não encontrados" } };

            
            var PercentualGorduraPollock3 = _calculosDobras.CalculoPollock3(prega);
            
            var (pesoGordura, pesoMassaMagra) = _calculosDobras.CalcularPesoGorduraEMassaMagra(imc.Peso, PercentualGorduraPollock3.PercentualGordura.Value);
            double pesoResidual = _calculosDobras.CalcularPesoResidual(imc.Peso, imc.Altura, idade, projeto.Paciente.Sexo);

            return new Dictionary<string, object>
            {
                { "PercentualGorduraPollock3", PercentualGorduraPollock3.PercentualGordura },
                { "PesoGordura", pesoGordura },
                { "PesoMassaMagra", pesoMassaMagra },
                { "PesoResidual", pesoResidual }
            };
        }
    }
}
