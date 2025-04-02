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

            var alimentosDaRefeicao = _context.Refeicoes_Alimentos
                                    .Where(ra => ra.IdRefeicao == RefeicaoId.Value)
                                    .Select(ra => new
                                    {
                                        ra.Id,
                                        Alimento = _context.Alimentos.FirstOrDefault(a => a.Id == ra.IdAlimento),
                                        ra.Quantidade
                                    })
                                    .ToList();

                                ViewBag.AlimentosDaRefeicao = alimentosDaRefeicao;

            if (RefeicaoId.HasValue)
            {
                var totalProteina = _context.Refeicoes_Alimentos
                    .Where(ra => ra.IdRefeicao == RefeicaoId.Value)
                    .Sum(ra => (ra.Quantidade * _context.Alimentos.Where(a => a.Id == ra.IdAlimento).Select(a => a.Proteina).FirstOrDefault()) / 100);
                var totalKcal = _context.Refeicoes_Alimentos
                    .Where(ra => ra.IdRefeicao == RefeicaoId.Value)
                    .Sum(ra => (ra.Quantidade * _context.Alimentos.Where(a => a.Id == ra.IdAlimento).Select(a => a.Energia_Kcal).FirstOrDefault()) / 100);
                var totalKj = _context.Refeicoes_Alimentos
                    .Where(ra => ra.IdRefeicao == RefeicaoId.Value)
                    .Sum(ra => (ra.Quantidade * _context.Alimentos.Where(a => a.Id == ra.IdAlimento).Select(a => a.Energia_KJ).FirstOrDefault()) / 100);

                ViewBag.TotalProteina = totalProteina;
                ViewBag.TotalKcal = totalKcal;
                ViewBag.TotalKj = totalKj;
            }
            // Retorna a View
            return View();
        }




        [HttpPost]  
        public IActionResult CriarRefeicao_Alimento(Refeicao_Alimento refeicao_Alimento)
        {
            Console.WriteLine("RefeicaoId: " + refeicao_Alimento.IdRefeicao);
            Console.WriteLine("AlimentoId: " + refeicao_Alimento.IdAlimento);

            _context.Refeicoes_Alimentos.Add(refeicao_Alimento);
            _context.SaveChanges();

            // Redirecionar para o método GET, passando os IDs necessários
            return RedirectToAction("CriarRefeicao_Alimento", new { RefeicaoId = refeicao_Alimento.IdRefeicao });
        }




        [HttpPost]
        public IActionResult DeletarRefeicaoAlimento(int idRefeicao, int idAlimento)
        {
            Console.WriteLine($"Tentando excluir o item com IdRefeicao: {idRefeicao}, IdAlimento: {idAlimento}");

            var refeicao_Alimento = _context.Refeicoes_Alimentos.Find(idRefeicao, idAlimento);
            if (refeicao_Alimento != null)
            {
                _context.Refeicoes_Alimentos.Remove(refeicao_Alimento);
                _context.SaveChanges();
                
                // Após excluir, redireciona para a página sem incluir o IdAlimento
                return RedirectToAction("CriarRefeicaoAlimento", new { IdRefeicao = idRefeicao });
            }

            Console.WriteLine("Item não encontrado no banco de dados.");
            return NotFound();
        }


        [HttpPost]
        public IActionResult DeletarRefeicao_Alimento(int idRefeicao, int idAlimento)
        {
            Console.WriteLine($"Tentando excluir o item com IdRefeicao: {idRefeicao}, IdAlimento: {idAlimento}");

            var refeicao_Alimento = _context.Refeicoes_Alimentos
                                            .FirstOrDefault(ra => ra.IdRefeicao == idRefeicao && ra.IdAlimento == idAlimento);
            if (refeicao_Alimento != null)
            {
                _context.Refeicoes_Alimentos.Remove(refeicao_Alimento);
                _context.SaveChanges();
                
                return RedirectToAction("CriarRefeicao_Alimento", new { RefeicaoId = idRefeicao });
            }

            Console.WriteLine("Item não encontrado no banco de dados.");
            return NotFound();
        }




    }
}