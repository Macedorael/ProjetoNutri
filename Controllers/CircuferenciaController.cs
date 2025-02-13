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
    public class CircuferenciaController : Controller
    {
        private readonly ClienteContext _context;

        public CircuferenciaController(ClienteContext context)
        {
            _context = context;
        }

        public IActionResult IndexCircuferencia(int projetoId)
        {
        var circulferencia = _context.Circuferencias
            .Include(c => c.Projeto)  // Inclui o Projeto associado à Circuferencia
            .ThenInclude(projeto => projeto.Paciente)  // Inclui o Paciente associado ao Projeto
            .Where(c => c.IdProjeto == projetoId)  // Filtra pelo ID do Projeto
            .ToList();
        ViewData["ProjetoId"] = projetoId;
        circulferencia.ForEach(p => p.Projeto = _context.Projetos.Find(p.IdProjeto));

            return View(circulferencia);
        }

        public IActionResult CriarCircuferencia(int projetoId)
        {
            var circulferencia = new Circuferencia { IdProjeto = projetoId };
            return View(circulferencia);
        }

        [HttpPost]
        public IActionResult CriarCircuferencia(Circuferencia circulferencia)
        {
            if (ModelState.IsValid)
            {
                var projeto = _context.Projetos.Include(p => p.Paciente).FirstOrDefault(p => p.Id == circulferencia.IdProjeto);
                var paciente = projeto.Paciente;

                // Calcular o RCQ
                circulferencia.Rcq = circulferencia.Cintura / circulferencia.Quadril;

                // Classificação do RCQ
                if (paciente.Sexo.ToLower() == "masculino")
                {
                    if (circulferencia.Rcq < 0.90)
                    {
                        circulferencia.Classificacao = "Normal";
                    }
                    else
                    {
                        circulferencia.Classificacao = "Risco aumentado";
                    }
                }
                else if (paciente.Sexo.ToLower() == "feminino")
                {
                    if (circulferencia.Rcq < 0.85)
                    {
                        circulferencia.Classificacao = "Normal";
                    }
                    else
                    {
                        circulferencia.Classificacao = "Risco aumentado";
                    }
                }

                // Adicionar à base de dados
                _context.Circuferencias.Add(circulferencia);
                _context.SaveChanges();

                // Redireciona para a listagem de Circuferencias do Projeto
                return RedirectToAction("IndexCircuferencia", new { projetoId = circulferencia.IdProjeto });
            }

            return View(circulferencia);
        }

    }
}