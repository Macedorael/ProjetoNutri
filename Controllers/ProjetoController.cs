using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjetoNutri.Models;
using ProjetoNutriC_.Context;
using Microsoft.EntityFrameworkCore;
using ProjetoNutri.Context;

namespace ProjetoNutri.Controllers
{
    public class ProjetoController : Controller
    {
        private readonly FocoContext _context;

        public ProjetoController(FocoContext context)
        {
            _context = context;
        }

        // Ação que lista os projetos de um paciente
        public IActionResult IndexProjeto(int pacienteId)
        {
            // Se pacienteId for fornecido, filtra os projetos para esse paciente específico
            var projetos = _context.Projetos.Where(p => p.PacienteId == pacienteId).ToList();
            ViewData["PacienteId"] = pacienteId;
            return View(projetos);
        }

        // Ação que exibe a página para criar um novo projeto
        public IActionResult CriarProjeto(int pacienteId)
        {
            // Verifica se o paciente existe
            var paciente = _context.Pacientes.Find(pacienteId);
            if (paciente == null)
            {
                return NotFound($"Paciente com ID {pacienteId} não encontrado.");
            }

            // Cria um novo projeto com o PacienteId
            var projeto = new Projeto
            {
                PacienteId = pacienteId // Passa o pacienteId para o projeto
            };

            return View(projeto);
        }

        // Ação que salva o novo projeto
        [HttpPost]
        public IActionResult CriarProjeto(Projeto projeto)
        {
            if (ModelState.IsValid)
            {
                var paciente = _context.Pacientes.Find(projeto.PacienteId);
                if (paciente == null)
                {
                    ModelState.AddModelError("PacienteId", "O paciente informado não existe.");
                    return View(projeto);
                }

                projeto.DataCriacao = DateTime.Now.Date;
                _context.Projetos.Add(projeto);
                _context.SaveChanges();

                return RedirectToAction("IndexProjeto", new { pacienteId = projeto.PacienteId });
            }

            return View(projeto);
        }

    }
}
