namespace ProjetoNutri.Services
{
    public class CalculoIdade
    {
        public static int Calcular(DateTime dataNascimento)
        {
            DateTime dataAtual = DateTime.Today;
            int idade = dataAtual.Year - dataNascimento.Year;

            // Ajusta a idade caso o aniversário ainda não tenha ocorrido este ano
            if (dataNascimento.Date > dataAtual.AddYears(-idade))
            {
                idade--;
            }

            return idade;
        }
    }
}
