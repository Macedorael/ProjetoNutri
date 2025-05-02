using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoNutri.Models
{
    public class Alimento
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        [ForeignKey("Categoria_Alimento")]
        public int IdCategoria_Alimento { get; set; }
        public Categoria_Alimento Categoria { get; set; }

        public double Energia_Kcal { get; set; }
        public double Energia_KJ { get; set; }
        public double Proteina { get; set; }
        public double Lipidio { get; set; }
        public double Colesterol { get; set; }
        public double Carboidrato { get; set; }
        public double Fibra_Alimentar { get; set; }
        public double Cinzas { get; set; }
        public double Calcio { get; set; }
        public double Magnesio { get; set; }
        public double Manganes { get; set; }
        public double Fosforo { get; set; }
        public double Ferro { get; set; }
        public double Sodio { get; set; }
        public double Potassio { get; set; }
        public double Cobre { get; set; }
        public double Zinco { get; set; }
        public double Retinol { get; set; }
        public double RE { get; set; }
        public double RAE { get; set; }
        public double Tiamina { get; set; }
        public double Riboflavina { get; set; }
        public double Piridoxina { get; set; }
        public double Niacina { get; set; }
        public double Vitamina_C { get; set; }

        public ICollection<Refeicao_Alimento> Refeicao_Alimentos { get; set; }
    }
}
