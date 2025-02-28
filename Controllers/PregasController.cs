using Microsoft.AspNetCore.Mvc;
using ProjetoNutri.Context;
using ProjetoNutri.Models;
using Microsoft.EntityFrameworkCore;

namespace ProjetoNutri.Controllers
{
    public class PregasController : Controller
    {
        private readonly ClienteContext _context;

        public PregasController(ClienteContext context)
        {
            _context = context;
        }

        // Ação IndexPrega
        public IActionResult IndexPrega(int projetoId)
        {
            var pregas = _context.Pregas
                .Include(pregas => pregas.Projeto)
                .ThenInclude(projeto => projeto.Paciente)
                .Where(pregas => pregas.IdProjeto == projetoId)
                .ToList();

            ViewData["ProjetoId"] = projetoId;
            pregas.ForEach(p => p.Projeto = _context.Projetos.Find(p.IdProjeto));

            return View("IndexPrega", pregas);
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
                return RedirectToAction("IndexPrega", new { projetoId = prega.IdProjeto });
            }

            // Se o modelo não for válido, retorna a mesma view com o erro
            return View(prega);
        }

    }
}
