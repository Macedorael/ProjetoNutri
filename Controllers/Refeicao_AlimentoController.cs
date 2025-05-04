using System;
using System.Collections.Generic;
using System.Linq;
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

        public IActionResult CriarRefeicao_Alimento(int? RefeicaoId = null, int? AlimentoId = null, int? IdProjeto = null)
        {   
            if (!IdProjeto.HasValue)
                return BadRequest("Projeto não fornecido.");

            ViewBag.IdProjeto = IdProjeto.Value;
            ViewBag.RefeicaoId = RefeicaoId;

            ViewBag.Refeicoes = _context.Refeicoes.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.Nome
            }).ToList();

            ViewBag.Alimentos = _context.Alimentos.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Nome
            }).ToList();

            if (RefeicaoId.HasValue)
            {
                var refeicao = _context.Refeicoes.Find(RefeicaoId.Value);
                if (refeicao != null)
                    ViewBag.RefeicaoSelecionada = refeicao;

                var alimentosDaRefeicao = _context.Refeicoes_Alimentos
                    .Where(ra => ra.IdRefeicao == RefeicaoId.Value)
                    .Select(ra => new
                    {
                        ra.Id,
                        Alimento = _context.Alimentos.FirstOrDefault(a => a.Id == ra.IdAlimento),
                        ra.Quantidade
                    }).ToList();

                ViewBag.AlimentosDaRefeicao = alimentosDaRefeicao;

                var dados = (from ra in _context.Refeicoes_Alimentos
                             join a in _context.Alimentos on ra.IdAlimento equals a.Id
                             where ra.IdRefeicao == RefeicaoId.Value
                             select new
                             {
                                 Quantidade = ra.Quantidade,
                                 Proteina = a.Proteina,
                                 Lipidio = a.Lipidio,
                                 Carboidrato = a.Carboidrato,
                                 Energia_Kcal = a.Energia_Kcal,
                                 Energia_KJ = a.Energia_KJ
                             }).ToList();

                ViewBag.TotalProteina = dados.Sum(x => (x.Quantidade * x.Proteina) / 100);
                ViewBag.TotalLipidio = dados.Sum(x => (x.Quantidade * x.Lipidio) / 100);
                ViewBag.TotalCarboidrato = dados.Sum(x => (x.Quantidade * x.Carboidrato) / 100);
                ViewBag.TotalKcal = dados.Sum(x => (x.Quantidade * x.Energia_Kcal) / 100);
                ViewBag.TotalKj = dados.Sum(x => (x.Quantidade * x.Energia_KJ) / 100);
            }

            if (AlimentoId.HasValue)
            {
                var alimento = _context.Alimentos.Find(AlimentoId.Value);
                if (alimento != null)
                    ViewBag.AlimentoSelecionado = alimento;
            }

            return View();
        }

        [HttpPost]
        public IActionResult CriarRefeicao_Alimento(Refeicao_Alimento refeicao_Alimento, int IdProjeto)
        {
            if (refeicao_Alimento.IdRefeicao == 0 || refeicao_Alimento.IdAlimento == 0)
                return BadRequest("Refeição ou Alimento não especificados.");

            refeicao_Alimento.IdProjeto = IdProjeto;
            _context.Refeicoes_Alimentos.Add(refeicao_Alimento);
            _context.SaveChanges();

            return RedirectToAction("CriarRefeicao_Alimento", new
            {
                RefeicaoId = refeicao_Alimento.IdRefeicao,
                IdProjeto = IdProjeto
            });
        }

        [HttpPost]
        public IActionResult DeletarRefeicao_Alimento(int idRefeicao, int idAlimento, int IdProjeto)
        {
            var refeicao_Alimento = _context.Refeicoes_Alimentos
                .FirstOrDefault(ra => ra.IdRefeicao == idRefeicao && ra.IdAlimento == idAlimento);

            if (refeicao_Alimento != null)
            {
                _context.Refeicoes_Alimentos.Remove(refeicao_Alimento);
                _context.SaveChanges();

                return RedirectToAction("CriarRefeicao_Alimento", "Refeicao_Alimento", new { refeicaoId = idRefeicao, idProjeto = IdProjeto });
            }

            return NotFound();
        }


        [HttpPost]
        public IActionResult AtualizarQuantidade(int idRefeicao, int idAlimento, float novaQuantidade, int IdProjeto)
        {
            if (novaQuantidade <= 0)
                return BadRequest("Quantidade inválida.");

            var relacao = _context.Refeicoes_Alimentos
                .FirstOrDefault(r => r.IdRefeicao == idRefeicao && r.IdAlimento == idAlimento);

            if (relacao != null)
            {
                relacao.Quantidade = novaQuantidade;
                _context.SaveChanges();
            }

            return RedirectToAction("CriarRefeicao_Alimento", new
            {
                RefeicaoId = idRefeicao,
                IdProjeto = IdProjeto
            });
        }

        [HttpGet]
        public JsonResult BuscarAlimentos(string termo)
        {
            var alimentos = _context.Alimentos
                .Where(a => a.Nome.Contains(termo))
                .Select(a => new { a.Id, a.Nome })
                .Take(10)
                .ToList();

            return Json(alimentos);
        }

        [HttpGet]
        public IActionResult ObterDadosAlimento(int id)
        {
            var alimento = _context.Alimentos.FirstOrDefault(a => a.Id == id);

            if (alimento == null)
                return NotFound();

            return Json(new
            {
                proteina = alimento.Proteina,
                energia_Kcal = alimento.Energia_Kcal,
                energia_KJ = alimento.Energia_KJ,
                lipidio = alimento.Lipidio,
                carboidrato = alimento.Carboidrato
            });
        }
    }
}
