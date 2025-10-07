using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjetoNutri.Models;
using Microsoft.EntityFrameworkCore;
using ProjetoNutri.Context;
using ProjetoNutri.Services;

namespace ProjetoNutri.Controllers
{
    public class PregasController : Controller
    {
        private readonly ClienteContext _context;

        public PregasController(ClienteContext context)
        {
            _context = context;
        }

        public IActionResult IndexPregas(int projetoId)
        {
            var pregas = _context.Pregas.Include(p => p.Projeto).ThenInclude(projeto => projeto.Paciente).Where(p => p.IdProjeto == projetoId).ToList();
            ViewData["ProjetoId"] = projetoId;
            pregas.ForEach(p => p.Projeto = _context.Projetos.Find(p.IdProjeto));

            return View(pregas);
        }

        public IActionResult CriarPregas(int projetoId)
        {
            var pregas = new Pregas { IdProjeto = projetoId };
            return View(pregas);
        }

        // Ação IndexPrega
        public IActionResult DetalhePrega(int id)
        {
            var prega = _context.Pregas
                .Include(p => p.Projeto)
                .ThenInclude(projeto => projeto.Paciente)
                .FirstOrDefault(p => p.Id == id);

            if (prega == null)
            {
                return NotFound();
            }

            return View(prega);
        }


        public IActionResult CriarPrega(int projetoId)
        {
            var prega = new Pregas { IdProjeto = projetoId };
            return View(prega);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CriarPrega(Pregas prega)
        {
            if (ModelState.IsValid)
            {
                // Adiciona a nova prega ao contexto
                _context.Pregas.Add(prega);

                // Salva as alterações no banco de dados
                _context.SaveChanges();

                // Redireciona para a página IndexPrega com o ID do projeto atual
                return RedirectToAction("AntropometriaProjeto","Projeto", new { projetoId = prega.IdProjeto });
            }

            // Se o modelo não for válido, retorna a mesma view com o erro
            return View(prega);
        }

        public IActionResult EditarPregas(int id, int projetoId)
        {
            var pregas = _context.Pregas.Find(id);
            if (pregas == null)
            {
                return NotFound();
            }
            ViewData["ProjetoId"] = projetoId;
            return View(pregas);
        }

        public IActionResult EditarPrega(int id)
        {
            var prega = _context.Pregas
                .Include(p => p.Projeto)
                .ThenInclude(projeto => projeto.Paciente)
                .FirstOrDefault(p => p.Id == id);

            if (prega == null)
            {
                return NotFound();
            }

            return View(prega);
        }

        [HttpPost]
        public IActionResult EditarPrega(int id, Pregas prega)
        {
            if (id != prega.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var pregaBanco = _context.Pregas.Find(id);
                if (pregaBanco == null)
                {
                    return NotFound();
                }
                pregaBanco.Tricipital = prega.Tricipital;
                pregaBanco.Bicipital = prega.Bicipital;
                pregaBanco.Abdominal = prega.Abdominal;
                pregaBanco.AxilarMedia = prega.AxilarMedia;
                pregaBanco.Subescapular = prega.Subescapular;
                pregaBanco.Coxa = prega.Coxa;
                pregaBanco.Toracica = prega.Toracica;
                pregaBanco.SupraIliaca = prega.SupraIliaca;
                pregaBanco.SupraEspinal = prega.SupraEspinal;
                pregaBanco.Panturrilha = prega.Panturrilha;

                _context.Entry(pregaBanco).State = EntityState.Detached;
                _context.Pregas.Update(pregaBanco);
                _context.SaveChanges();

                return RedirectToAction("AntropometriaProjeto", "Projeto", new { projetoId = prega.IdProjeto });
            }

            return View(prega);
        }

        public IActionResult DeletarPregas(int id)
        {
            var pregas = _context.Pregas
                .Include(p => p.Projeto)
                .ThenInclude(p => p.Paciente)
                .FirstOrDefault(p => p.Id == id);

            if (pregas == null)
            {
                return NotFound();
            }

            return View(pregas);
        }

        [HttpPost]
        public IActionResult DeletarPregas(int id, bool confirmacao)
        {
            var pregas = _context.Pregas.Find(id);
            if (pregas == null)
            {
                return NotFound();
            }

            _context.Pregas.Remove(pregas);
            _context.SaveChanges();

            return RedirectToAction("AntropometriaProjeto", "Projeto", new { projetoId = pregas.IdProjeto });
        }

        [HttpPost]
        public IActionResult DeletarPrega(int id)
        {
            var prega = _context.Pregas.Find(id);

            if (prega == null)
            {
                return NotFound();
            }

            _context.Pregas.Remove(prega);
            _context.SaveChanges();

            return RedirectToAction("AntropometriaProjeto", "Projeto", new { projetoId = prega.IdProjeto });
        }

    }
}
