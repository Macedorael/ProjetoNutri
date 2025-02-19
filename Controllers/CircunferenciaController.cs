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
    public class CircunferenciaController : Controller
    {
        private readonly ClienteContext _context;

        public CircunferenciaController(ClienteContext context)
        {
            _context = context;
        }

        public IActionResult IndexCircunferencia(int projetoId)
        {
        var circunferencia = _context.Circunferencias
            .Include(c => c.Projeto)  // Inclui o Projeto associado à Circuferencia
            .ThenInclude(projeto => projeto.Paciente)  // Inclui o Paciente associado ao Projeto
            .Where(c => c.IdProjeto == projetoId)  // Filtra pelo ID do Projeto
            .ToList();
        ViewData["ProjetoId"] = projetoId;
        circunferencia.ForEach(p => p.Projeto = _context.Projetos.Find(p.IdProjeto));

            return View(circunferencia);
        }

        public IActionResult CriarCircunferencia(int projetoId)
        {
            var circulferencia = new Circunferencia { IdProjeto = projetoId };
            return View(circulferencia);
        }

        [HttpPost]
        public IActionResult CriarCircunferencia(Circunferencia circunferencia)
        {
            if (ModelState.IsValid)
            {
                var projeto = _context.Projetos.Include(p => p.Paciente).FirstOrDefault(p => p.Id == circunferencia.IdProjeto);
                var paciente = projeto.Paciente;

                // Calcular o RCQ
                circunferencia.Rcq = circunferencia.Cintura / circunferencia.Quadril;

                // Classificação do RCQ
                if (paciente.Sexo.ToLower() == "masculino")
                {
                    if (circunferencia.Rcq < 0.90)
                    {
                        circunferencia.Classificacao = "Normal";
                    }
                    else
                    {
                        circunferencia.Classificacao = "Risco aumentado";
                    }
                }
                else if (paciente.Sexo.ToLower() == "feminino")
                {
                    if (circunferencia.Rcq < 0.85)
                    {
                        circunferencia.Classificacao = "Normal";
                    }
                    else
                    {
                        circunferencia.Classificacao = "Risco aumentado";
                    }
                }

                // Adicionar à base de dados
                _context.Circunferencias.Add(circunferencia);
                _context.SaveChanges();

                // Redireciona para a listagem de Circuferencias do Projeto
                return RedirectToAction("IndexCircunferencia", new { projetoId = circunferencia.IdProjeto });
            }

            return View(circunferencia);
        }

        public IActionResult DetalheCircunferencia(int id)
        {
            var circunferencia = _context.Circunferencias
            .Include(c => c.Projeto)       // Inclui o Projeto relacionado
            .ThenInclude(p => p.Paciente)  // Inclui o Paciente relacionado ao Projeto
            .FirstOrDefault(c => c.Id == id);

            if (circunferencia == null)
            {
                return RedirectToAction(nameof(Index)); // Redireciona para a listagem
            }

            return View(circunferencia);
        }

    }
}