using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjetoNutri.Models;
using Microsoft.EntityFrameworkCore;
using ProjetoNutri.Context;

namespace ProjetoNutri.Controllers
{
    public class ProjetoController : Controller
    {
        private readonly ClienteContext _context;

        public ProjetoController(ClienteContext context)
        {
            _context = context;
        }

        // Ação que lista os projetos de um paciente
        public IActionResult IndexProjeto(int pacienteId)
        {
            // Se pacienteId for fornecido, filtra os projetos para esse paciente específico
            var projetos = _context.Projetos.Where(p => p.PacienteId == pacienteId).ToList();
            ViewData["PacienteId"] = pacienteId;
            projetos.ForEach(p => p.Paciente = _context.Pacientes.Find(p.PacienteId));
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

        public IActionResult DetalheProjeto(int id)
        {
            var projeto = _context.Projetos
                .Include(p => p.Paciente) // Para carregar os dados do paciente associado ao projeto
                .FirstOrDefault(p => p.Id == id);

            if (projeto == null)
            {
                return RedirectToAction(nameof(IndexProjeto));
            }

            return View(projeto);
        }

        public IActionResult EditarProjeto(int id)
        {
            var projeto = _context.Projetos.Find(id);
            if (projeto == null)
            {
                return NotFound();
            }
            return View(projeto);
        }
        [HttpPost]
        public IActionResult EditarProjeto(int id, Projeto projeto)
        {
            if (id != projeto.Id)
            {
                return BadRequest("O ID do projeto não corresponde.");
            }

            if (ModelState.IsValid)
            {
                // Encontrando o projeto que será atualizado
                var projetoExistente = _context.Projetos.Find(id);
                if (projetoExistente == null)
                {
                    return NotFound();
                }

                // Atualizando apenas o nome
                projetoExistente.NomeProjeto = projeto.NomeProjeto;

                _context.Projetos.Update(projetoExistente);
                _context.SaveChanges();

                return RedirectToAction("IndexProjeto", new { pacienteId = projetoExistente.PacienteId });
            }

            return View(projeto);
        }

        public IActionResult DeletarProjeto(int id)
        {
            var projeto = _context.Projetos.Include(p => p.Paciente).FirstOrDefault(p => p.Id == id);
            if (projeto == null)
            {
                return RedirectToAction(nameof(IndexProjeto)); // Redireciona se o projeto não for encontrado
            }

            return View(projeto); // Exibe a confirmação de exclusão
        }

        [HttpPost]
        public IActionResult DeletarProjeto(Projeto projeto)
        {
            var projetoBanco = _context.Projetos.Find(projeto.Id);
            if (projetoBanco == null)
            {
                return RedirectToAction(nameof(IndexProjeto)); // Redireciona se o projeto não for encontrado
            }

            // Remove o projeto do banco de dados
            _context.Projetos.Remove(projetoBanco);
            _context.SaveChanges();

            // Redireciona para a lista de projetos, usando o PacienteId correto
            return RedirectToAction(nameof(IndexProjeto), new { pacienteId = projeto.PacienteId }); // Aqui você garante que o pacienteId correto seja passado
        }
       public IActionResult AntropometriaProjeto(int projetoId)
        {
            Console.WriteLine($"ID recebido: {projetoId}"); // Depuração

            var projeto = _context.Projetos
                .Include(p => p.Paciente) // Certifique-se de que os dados do paciente são carregados
                .FirstOrDefault(p => p.Id == projetoId);
 

            if (projeto == null)
            {
                Console.WriteLine("Projeto não encontrado!");
                return NotFound();
            }

            return View(projeto);
        }


    }
}
