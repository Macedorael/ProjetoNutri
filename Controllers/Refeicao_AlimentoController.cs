using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjetoNutri.Context;
using ProjetoNutri.Models;

namespace ProjetoNutri.Controllers
{
    public class Refeicao_AlimentoController : Controller
    {
         private readonly ClienteContext _context;

        public Refeicao_AlimentoController(ClienteContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult CriarRefeicao_Alimento(int? RefeicaoId = null, int? AlimentoId = null)
        {
            var refeicoes = _context.Refeicoes.ToList();
            var alimentos = _context.Alimentos.ToList();

            ViewBag.RefeicaoId = RefeicaoId;

            Console.WriteLine("RefeicaoId: " + RefeicaoId);

            ViewBag.Refeicoes = refeicoes.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.Nome
            }).ToList();

            ViewBag.Alimentos = _context.Alimentos?
    .Select(a => new SelectListItem
    {
        Value = a.Id.ToString(),
        Text = a.Nome
    }).ToList() ?? new List<SelectListItem>(); // Garante que não será null



            if (RefeicaoId.HasValue)
            {
                var refeicao = _context.Refeicoes.Find(RefeicaoId.Value);
                if (refeicao != null)
                {
                    ViewBag.RefeicaoSelecionada = refeicao;
                }
            }

            if (AlimentoId.HasValue)
            {
                var alimento = _context.Alimentos.Find(AlimentoId.Value);
                if (alimento != null)
                {
                    ViewBag.AlimentoSelecionado = alimento; // Agora é um objeto Alimento real
                }
            }
            else
            {
                ViewBag.AlimentoSelecionado = null; // Garante que a View sabe que não há alimento selecionado
            }


            // Retorna a View
            return View();
        }




        [HttpPost]  
        public IActionResult CriarRefeicao_Alimento(Refeicao_Alimento refeicao_Alimento)
        {
            _context.Refeicoes_Alimentos.Add(refeicao_Alimento);
            _context.SaveChanges();
            return RedirectToAction("IndexRefeicao", "Refeicao");
        }

    }
}