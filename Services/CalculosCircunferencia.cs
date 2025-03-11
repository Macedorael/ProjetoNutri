using System;

namespace ProjetoNutri.Services
{
    public class CalculosCircunferencia
    {
        // Método para calcular o RCQ e a classificação
        public (double Rcq, string Classificacao) CalcularRCQ(double cintura, double quadril, string sexo)
        {
            double rcq = cintura / quadril;  // Cálculo do RCQ

            string classificacao = AvaliarClassificacaoRCQ(rcq, sexo);  // Determina a classificação do RCQ

            return (rcq, classificacao);
        }

        // Método para avaliar a classificação do RCQ com base no sexo
        private string AvaliarClassificacaoRCQ(double rcq, string sexo)
        {
            if (sexo.ToLower() == "masculino")
            {
                if (rcq < 0.95)
                {
                    return "Baixo risco";
                }
                else if (rcq >= 0.96 && rcq < 1.0)
                {
                    return "Risco moderado";
                }
                else
                {
                    return "Alto risco";
                }
            }
            else if (sexo.ToLower() == "feminino")
            {
                if (rcq < 0.80)
                {
                    return "Baixo risco";
                }
                else if (rcq >= 0.81 && rcq < 0.85)
                {
                    return "Risco moderado";
                }
                else
                {
                    return "Alto risco";
                }
            }
            else
            {
                return "Sexo não reconhecido";
            }
        }
    }
}
