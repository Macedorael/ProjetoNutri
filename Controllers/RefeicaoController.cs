using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoNutri.Context;
using ProjetoNutri.Models;

namespace ProjetoNutri.Controllers
{
    public class RefeicaoController : Controller
    {
        private readonly ClienteContext _context;

        public RefeicaoController(ClienteContext context)
        {
            _context = context;
        }

       public IActionResult IndexRefeicao()
        {
            var refeicoes = _context.Refeicoes
                .Include(r => r.Refeicao_Alimentos)
                .ThenInclude(ra => ra.Alimento)
                .OrderByDescending(r => r.DataCriacao)
                .ToList();

            // Criando um dicionário para armazenar a soma das proteínas por refeição
            var totalProteinaPorRefeicao = refeicoes.ToDictionary(
                refeicao => refeicao.Id,
                refeicao => refeicao.Refeicao_Alimentos?.Sum(ra => ra.Alimento.Proteina) ?? 0
            );
            var totalKcalPorRefeicao = refeicoes.ToDictionary(
                refeicao => refeicao.Id,
                refeicao => refeicao.Refeicao_Alimentos?.Sum(ra => ra.Alimento.Energia_Kcal) ?? 0
            );
            var totalKjPorRefeicao = refeicoes.ToDictionary(
                refeicao => refeicao.Id,
                refeicao => refeicao.Refeicao_Alimentos?.Sum(ra => ra.Alimento.Energia_KJ) ?? 0
            );

            var viewModel = new ProjetoDietaViewModel
            {
                Refeicaos = refeicoes,
                TotalProteinaPorRefeicao = totalProteinaPorRefeicao,
                TotalKcalPorRefeicao = totalKcalPorRefeicao,
                TotalKjPorRefeicao = totalKjPorRefeicao
            };

            return View(viewModel);
        }
        


        public IActionResult CriarRefeicao()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CriarRefeicao(Refeicao refeicao)
        {
            
            _context.Refeicoes.Add(refeicao);
            _context.SaveChanges();
            return RedirectToAction("IndexRefeicao");
        }

        public IActionResult EditarRefeicao(int id)
        {
            var refeicao = _context.Refeicoes.Find(id);

            if (refeicao == null)
            {
                return NotFound();
            }

            return View(refeicao);
        }

        [HttpPost]
        public IActionResult EditarRefeicao(Refeicao refeicao)
        {
            if (ModelState.IsValid)
            {
                _context.Refeicoes.Update(refeicao);
                _context.SaveChanges();
                return RedirectToAction("IndexRefeicao");
            }
            return View(refeicao);
        }

        public IActionResult DeletarRefeicao(int id)
        {
            var refeicao = _context.Refeicoes.Find(id);

            if (refeicao == null)
            {
                return NotFound();
            }

            return View(refeicao);
        }

        [HttpPost]
        public IActionResult Deletar_Refeicao(int id)
        {
            var refeicao = _context.Refeicoes.Find(id);
            if (refeicao == null)
            {
                return NotFound();
            }

            _context.Refeicoes.Remove(refeicao);
            _context.SaveChanges();
            
            return RedirectToAction("IndexRefeicao");
        }

    }
}