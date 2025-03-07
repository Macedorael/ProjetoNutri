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


            DateTime dataNascimento = prega.Projeto.Paciente.DataNascimento;
            int idade = ProjetoNutri.Services.CalculoIdade.Calcular(dataNascimento);

            double densidadeCorporal = 1.112 - (0.00043499 * somaPregas) +
                               (0.00000055 * Math.Pow(somaPregas, 2)) -
                               (0.00028826 * idade);

            double percentualGordura = (495 / densidadeCorporal) - 450;

            return percentualGordura;
        }
        public double? CalculoPetroski(Pregas prega)
        {
            // Verifica se o sexo está definido
            if (prega.Projeto.Paciente.Sexo == null)
            {
                return null;
            }

            // Verifica se todas as pregas estão presentes
            if (prega.Toracica == null || prega.Abdominal == null || prega.Coxa == null ||
                prega.Tricipital == null || prega.Subescapular == null)
            {
                return null;
            }

            // Soma todas as pregas
            double somaPregas = prega.Toracica.Value + prega.Abdominal.Value + prega.Coxa.Value +
                                prega.Tricipital.Value + prega.Subescapular.Value;

            double densidadeCorporal;

            // Calcular a densidade corporal para o sexo masculino ou feminino
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

            // Calcular o percentual de gordura com a fórmula de Petroski
            double percentualGordura = (495 / densidadeCorporal) - 450;

            return percentualGordura;
        }

        public double? CalculoGuedes(Pregas prega)
        {
            // Verifica se o sexo está definido
            if (prega.Projeto.Paciente.Sexo == null)
            {
                return null;
            }

            // Verifica se todas as pregas necessárias estão presentes
            if (prega.Subescapular == null || prega.SupraIliaca == null || prega.Coxa == null)
            {
                return null;
            }

            // Soma das pregas
            double somaPregas = prega.Subescapular.Value + prega.SupraIliaca.Value + prega.Coxa.Value;

            // Cálculo da densidade corporal usando a fórmula de Guedes
            double densidadeCorporal;

            if (prega.Projeto.Paciente.Sexo == "Masculino")
            {
                // Fórmula de densidade corporal para homens
                densidadeCorporal = 1.112 - (0.00043499 * somaPregas);
            }
            else
            {
                // Fórmula de densidade corporal para mulheres
                densidadeCorporal = 1.097 - (0.00042399 * somaPregas);
            }

            // Cálculo do percentual de gordura usando a fórmula de Brozek (1963)
            double percentualGordura = (4.57 / densidadeCorporal) - 4.142;
            percentualGordura = Math.Round(percentualGordura * 100, 2);  // Multiplica por 100 para obter o percentual

            // Adicionando logs de depuração
            Console.WriteLine("Soma das pregas: " + somaPregas);
            Console.WriteLine("Densidade corporal: " + densidadeCorporal);
            Console.WriteLine("Percentual de gordura: " + percentualGordura);

            return percentualGordura;
        }

        public double? CalculoDurnin(Pregas prega)
        {
            // Verifica se o sexo está definido
            if (prega.Projeto.Paciente.Sexo == null)
            {
                return null;
            }

            // Verifica se todas as pregas necessárias estão presentes
            if (prega.Tricipital == null || prega.Bicipital == null || prega.Subescapular == null || prega.SupraIliaca == null)
            {
                return null;
            }

            // Soma das pregas
            double somaPregas = prega.Tricipital.Value + prega.Bicipital.Value + prega.Subescapular.Value + prega.SupraIliaca.Value;

            // Cálculo da densidade corporal usando a fórmula de Durnin e Womersley
            double densidadeCorporal;

            if (prega.Projeto.Paciente.Sexo == "Masculino")
            {
                // Fórmula de densidade corporal para homens
                densidadeCorporal = 1.1631 - (0.0632 * Math.Log10(somaPregas));
            }
            else
            {
                // Fórmula de densidade corporal para mulheres
                densidadeCorporal = 1.1599 - (0.0717 * Math.Log10(somaPregas));
            }

            // Cálculo do percentual de gordura usando a fórmula de Siri (1961)
            double percentualGordura = (4.95 / densidadeCorporal) - 4.5;
            percentualGordura = Math.Round(percentualGordura * 100, 2);  // Multiplica por 100 para obter o percentual

            // Adicionando logs de depuração
            Console.WriteLine("Soma das pregas: " + somaPregas);
            Console.WriteLine("Densidade corporal: " + densidadeCorporal);
            Console.WriteLine("Percentual de gordura: " + percentualGordura);

            return percentualGordura;
        }

        public double? CalculoFaulkner(Pregas prega)
        {
            // Verifica se o sexo está definido
            if (prega.Projeto.Paciente.Sexo == null)
            {
                return null;
            }

            // Verifica se todas as pregas necessárias estão presentes
            if (prega.Tricipital == null || prega.Subescapular == null || prega.SupraIliaca == null || prega.Abdominal == null)
            {
                return null;
            }

            // Soma das pregas
            double somaPregas = prega.Tricipital.Value + prega.Subescapular.Value + prega.SupraIliaca.Value + prega.Abdominal.Value;

            int idade = ProjetoNutri.Services.CalculoIdade.Calcular(prega.Projeto.Paciente.DataNascimento);
            // Cálculo da densidade corporal usando o protocolo de Faulkner
            double densidadeCorporal;

            if (prega.Projeto.Paciente.Sexo == "Masculino")
            {
                // Fórmula de densidade corporal para homens (Faulkner)
                densidadeCorporal = 1.112 - (0.00043499 * somaPregas) + (0.00000055 * Math.Pow(somaPregas, 2)) - (0.00028826 * idade);
            }
            else
            {
                // Fórmula de densidade corporal para mulheres (Faulkner)
                densidadeCorporal = 1.097 - (0.00042399 * somaPregas) + (0.00000056 * Math.Pow(somaPregas, 2)) - (0.0001392 *idade);
            }

            // Cálculo do percentual de gordura usando a fórmula de Siri (1961)
            double percentualGordura = (4.95 / densidadeCorporal) - 4.5;
            percentualGordura = Math.Round(percentualGordura * 100, 2);  // Multiplica por 100 para obter o percentual

            // Adicionando logs de depuração
            Console.WriteLine("Soma das pregas: " + somaPregas);
            Console.WriteLine("Densidade corporal: " + densidadeCorporal);
            Console.WriteLine("Percentual de gordura: " + percentualGordura);

            return percentualGordura;
        }
    }
}
