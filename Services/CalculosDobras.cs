using System;
using ProjetoNutri.Models;

namespace ProjetoNutri.Services
{
    public class CalculosDobras
    {
        public double? CalculoPollock3(Pregas prega)
        {
            // Verifica se o sexo está definido
            if (prega.Projeto.Paciente.Sexo == null)
            {
                return null; // Retorna null se o sexo não for definido
            }

            // Verifica se todas as pregas necessárias estão preenchidas
            if (prega.Tricipital == null || prega.Abdominal == null || prega.SupraIliaca == null)
            {
                return null; // Retorna null se faltar alguma prega
            }

            // Calcula o percentual de gordura dependendo do sexo
            double somaPregas = prega.Tricipital.Value + prega.Abdominal.Value + prega.SupraIliaca.Value;
            double densidadeCorporal;

            if (prega.Projeto.Paciente.Sexo == "Masculino")
            {
                densidadeCorporal = 1.098 - 0.0008 * somaPregas; // Fórmula masculina
            }
            else if (prega.Projeto.Paciente.Sexo == "Feminino")
            {
                densidadeCorporal = 1.097 - 0.0008 * somaPregas; // Fórmula feminina
            }
            else
            {
                return null; // Retorna null se o sexo for inválido
            }

            // Cálculo do percentual de gordura
            double percentualGordura = (495 / densidadeCorporal) - 450;

            return percentualGordura;
        }

        public double? CalculoPollock7(Pregas prega)
        {
            // Verifica se o sexo está definido
            if (prega.Projeto.Paciente.Sexo == null)
            {
                return null; 
            }

            if (prega.Tricipital == null || prega.Abdominal == null || prega.Subescapular == null || prega.AxilarMedia == null || 
                prega.SupraIliaca == null || prega.Toracica == null || prega.Coxa == null)
            {
                return null; 
            }
            
            double somaPregas = prega.Tricipital.Value + prega.Abdominal.Value + prega.Subescapular.Value +
                                prega.AxilarMedia.Value + prega.SupraIliaca.Value + prega.Toracica.Value + prega.Coxa.Value;

            double densidadeCorporal;

            
            if (prega.Projeto.Paciente.Sexo == "Masculino")
            {
                densidadeCorporal = 1.112 - 0.00043499 * somaPregas; 
            }
            else if (prega.Projeto.Paciente.Sexo == "Feminino")
            {
                densidadeCorporal = 1.097 - 0.00042399 * somaPregas; 
            }
            else
            {
                return null; 
            }

            double percentualGordura = (495 / densidadeCorporal) - 450;

            return percentualGordura;
        }

    }
}
