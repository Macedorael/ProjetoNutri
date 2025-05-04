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

        // Exibe as refeições de um projeto específico
        public IActionResult IndexRefeicao(int IdProjeto)
        {
            // Passando o ID do projeto para a view
            ViewData["ProjetoId"] = IdProjeto;

            var refeicoes = _context.Refeicoes
                .Include(r => r.Refeicao_Alimentos)
                .ThenInclude(ra => ra.Alimento)
                .Where(r => r.IdProjeto == IdProjeto) // Filtra as refeições pelo ProjetoId
                .OrderByDescending(r => r.DataCriacao)
                .ToList();

            // Criando dicionários para armazenar as somas por refeição
            var totalProteinaPorRefeicao = refeicoes.ToDictionary(
                refeicao => refeicao.Id,
                refeicao => refeicao.Refeicao_Alimentos?.Sum(ra => ra.Alimento.Proteina) ?? 0
            );

            var totalLipidioPorRefeicao = refeicoes.ToDictionary(
                refeicao => refeicao.Id,
                refeicao => refeicao.Refeicao_Alimentos?.Sum(ra => ra.Alimento.Lipidio) ?? 0
            );

            var totalCarboidratoPorRefeicao = refeicoes.ToDictionary(
                refeicao => refeicao.Id,
                refeicao => refeicao.Refeicao_Alimentos?.Sum(ra => ra.Alimento.Carboidrato) ?? 0
            );

            var totalKcalPorRefeicao = refeicoes.ToDictionary(
                refeicao => refeicao.Id,
                refeicao => refeicao.Refeicao_Alimentos?.Sum(ra => ra.Alimento.Energia_Kcal) ?? 0
            );

            var totalKjPorRefeicao = refeicoes.ToDictionary(
                refeicao => refeicao.Id,
                refeicao => refeicao.Refeicao_Alimentos?.Sum(ra => ra.Alimento.Energia_KJ) ?? 0
            );

            // Calculando os totais gerais
            double totalProteinaGeral = totalProteinaPorRefeicao.Values.Sum();
            double totalLipidioGeral = totalLipidioPorRefeicao.Values.Sum();
            double totalCarboidratoGeral = totalCarboidratoPorRefeicao.Values.Sum();
            double totalKcalGeral = totalKcalPorRefeicao.Values.Sum();
            double totalKjGeral = totalKjPorRefeicao.Values.Sum();

            // Criando o ViewModel para a View
            var viewModel = new ProjetoDietaViewModel
            {
                Refeicaos = refeicoes,
                TotalProteinaPorRefeicao = totalProteinaPorRefeicao,
                TotalLipidioPorRefeicao = totalLipidioPorRefeicao,
                TotalCarboidratoPorRefeicao = totalCarboidratoPorRefeicao,
                TotalKcalPorRefeicao = totalKcalPorRefeicao,
                TotalKjPorRefeicao = totalKjPorRefeicao,
                TotalProteinaGeral = totalProteinaGeral,
                TotalLipidioGeral = totalLipidioGeral,
                TotalCarboidratoGeral = totalCarboidratoGeral,
                TotalKcalGeral = totalKcalGeral,
                TotalKjGeral = totalKjGeral,
                Projeto = _context.Projetos.FirstOrDefault(p => p.Id == IdProjeto) // Passando o ProjetoId para a View
            };

            return View(viewModel);
        }

        // Exibe o formulário para criar uma nova refeição
        public IActionResult CriarRefeicao(int IdProjeto)
        {
            var refeicao = new Refeicao { IdProjeto = IdProjeto };
            var viewModel = new ProjetoDietaViewModel
            {
                Refeicao = refeicao
            };
            return View(viewModel);
        }


        // Cria uma nova refeição e a associa ao projeto
        [HttpPost]
        public IActionResult CriarRefeicao(Refeicao refeicao)
        {
            if (refeicao.IdProjeto <= 0)
            {
                return BadRequest("Projeto não encontrado.");
            }

            _context.Refeicoes.Add(refeicao);
            _context.SaveChanges();

            return RedirectToAction("IndexRefeicao", new { IdProjeto = refeicao.IdProjeto });
        }


        // Exibe o formulário para editar uma refeição
        public IActionResult EditarRefeicao(int id)
        {
            var refeicao = _context.Refeicoes.FirstOrDefault(r => r.Id == id);

            if (refeicao == null)
            {
                return NotFound();
            }

            // aqui é importante garantir que refeicao.IdProjeto já está preenchido corretamente
            return View(refeicao);
        }


        // Edita a refeição e salva no banco de dados
        [HttpPost]
        public IActionResult EditarRefeicao(Refeicao refeicao)
        {
            if (ModelState.IsValid)
            {
                _context.Update(refeicao); // refeicao.IdProjeto precisa estar preenchido com um valor existente
                _context.SaveChanges();
                return RedirectToAction("IndexRefeicao", new { idProjeto = refeicao.IdProjeto });
            }

            return View(refeicao);
        }


        // Exibe a página para confirmar a exclusão de uma refeição
        public IActionResult DeletarRefeicao(int id)
        {
            var refeicao = _context.Refeicoes.Find(id);

            if (refeicao == null)
            {
                return NotFound();
            }

            return View(refeicao);
        }

        // Exclui uma refeição do banco de dados
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

            // Redireciona de volta para a página de refeições do projeto
            return RedirectToAction("IndexRefeicao", new { IdProjeto = refeicao.IdProjeto });
        }

        public IActionResult DuplicarRefeicao(int id)
{
    var refeicaoOriginal = _context.Refeicoes
        .Include(r => r.Refeicao_Alimentos)
        .FirstOrDefault(r => r.Id == id);

    if (refeicaoOriginal == null)
    {
        return NotFound();
    }

    // 1. Criar nova refeição com o mesmo projeto
    var novaRefeicao = new Refeicao
    {
        Nome = refeicaoOriginal.Nome + " (Cópia)",
        IdProjeto = refeicaoOriginal.IdProjeto,
        DataCriacao = DateTime.Now
    };

    _context.Refeicoes.Add(novaRefeicao);
    _context.SaveChanges(); // precisa salvar antes para gerar Id

    // 2. Copiar os alimentos vinculados à refeição original
    foreach (var item in refeicaoOriginal.Refeicao_Alimentos)
    {
        var novoItem = new Refeicao_Alimento
        {
            IdRefeicao = novaRefeicao.Id,
            IdAlimento = item.IdAlimento,
            Quantidade = item.Quantidade,
            // Se houver IdProjeto no model Refeicao_Alimento:
            IdProjeto = refeicaoOriginal.IdProjeto // adicione isso se necessário
        };

        _context.Refeicoes_Alimentos.Add(novoItem);
    }

    _context.SaveChanges();

    return RedirectToAction("IndexRefeicao", new { IdProjeto = refeicaoOriginal.IdProjeto });
}

    }
}
