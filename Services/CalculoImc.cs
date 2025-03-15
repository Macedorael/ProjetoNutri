using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Console;

namespace ProjetoNutri.Services
{
    public class CalculoImc
    {
        public (double? imc, string classificacao, double? pesoIdeal) CalcularImc(double peso, double altura, string sexo)
        {
            // Calcula o IMC
            double imc = peso / Math.Pow(altura, 2);

            // Classifica o IMC com base no sexo
            string classificacao = ClassificarIMC(imc, sexo);

            // Calcula o peso ideal com base no sexo
            double pesoIdeal = CalcularPesoIdeal(altura, sexo);

            return (imc, classificacao, pesoIdeal);
        }

        // Método para classificar o IMC com base no sexo
        private string ClassificarIMC(double imc, string sexo)
        {
            if (sexo.ToLower() == "masculino")
            {
                if (imc < 20.7)
                    return "Abaixo do peso";
                else if (imc >= 20.7 && imc < 26.4)
                    return "Peso normal";
                else if (imc >= 26.4 && imc < 30.4)
                    return "Acima do peso";
                else
                    return "Obesidade";
            }
            else if (sexo.ToLower() == "feminino")
            {
                if (imc < 19.1)
                    return "Abaixo do peso";
                else if (imc >= 19.1 && imc < 25.8)
                    return "Peso normal";
                else if (imc >= 25.8 && imc < 30.8)
                    return "Acima do peso";
                else
                    return "Obesidade";
            }
            else
            {
                throw new ArgumentException("Sexo inválido. Use 'masculino' ou 'feminino'.", nameof(sexo));
            }
        }

        // Método para calcular o peso ideal com base no sexo
        private double CalcularPesoIdeal(double altura, string sexo)
        {
            double imcDesejado;

            if (sexo.ToLower() == "masculino")
            {
                imcDesejado = 22; // IMC desejado para homens
            }
            else if (sexo.ToLower() == "feminino")
            {
                imcDesejado = 21; // IMC desejado para mulheres
            }
            else
            {
                throw new ArgumentException("Sexo inválido. Use 'masculino' ou 'feminino'.", nameof(sexo));
            }

            return Math.Round(imcDesejado * Math.Pow(altura, 2), 2);
        }
            // Método para calcular a CMB, % do padrão e classificação
        public (double? cmb, double? porcentagemPadrao, string classificacao) CalcularCMBCompleto(double circunferenciaBraco, double pregaTricipitalMm, string sexo)
        {
            // Converte a PCT de mm para cm
            double pregaTricipitalCm = pregaTricipitalMm / 10;

            // Determina o CMB padrão com base no sexo
            double cmbPadrao = ObterCMBPadrao(sexo);

            // Calcula a CMB
            double cmb = CalcularCMB(circunferenciaBraco, pregaTricipitalCm);

            // Calcula a % do padrão
            double porcentagemPadrao = CalcularPorcentagemPadrao(cmb, cmbPadrao);
            
            // Classifica a CMB
            string classificacao = ClassificarCMB(porcentagemPadrao);

            // Retorna os resultados como uma tupla
            return (cmb, porcentagemPadrao, classificacao);
        }

        // Método para obter o CMB padrão com base no sexo
        private double ObterCMBPadrao(string sexo)
        {
            if (sexo.ToLower() == "masculino")
                return 25.3; // CMB padrão para homens
            else if (sexo.ToLower() == "feminino")
                return 23.2; // CMB padrão para mulheres
            else
                throw new ArgumentException("Sexo inválido. Use 'masculino' ou 'feminino'.", nameof(sexo));
        }

        // Método para calcular a CMB
        private double CalcularCMB(double circunferenciaBraco, double pregaTricipitalCm)
        {
            return circunferenciaBraco - (Math.PI * pregaTricipitalCm);
        }

        // Método para calcular a % do padrão
        private double CalcularPorcentagemPadrao(double cmbMedido, double cmbPadrao)
        {
            return (cmbMedido / cmbPadrao) * 100;
        }

        // Método para classificar a CMB
        private string ClassificarCMB(double porcentagemPadrao)
        {
            if (porcentagemPadrao < 70)
                return "Desnutrição grave";
            else if (porcentagemPadrao >= 70 && porcentagemPadrao < 80)
                return "Desnutrição moderada";
            else if (porcentagemPadrao >= 80 && porcentagemPadrao < 90)
                return "Desnutrição leve";
            else if (porcentagemPadrao >= 90 && porcentagemPadrao < 110)
                return "Eutrofia (normal)";
            else if (porcentagemPadrao >= 110 && porcentagemPadrao < 120)
                return "Sobrepeso";
            else
                return "Obesidade";
        }
    }
        
}
