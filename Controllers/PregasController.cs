using Microsoft.AspNetCore.Mvc;
using ProjetoNutri.Context;
using ProjetoNutri.Models;
using Microsoft.EntityFrameworkCore;
using ProjetoNutri.Services;

namespace ProjetoNutri.Controllers
{
    public class PregasController : Controller
    {
        private readonly ClienteContext _context;
        private readonly CalculosDobras _calculosDobras;

        public PregasController(ClienteContext context, CalculosDobras calculosDobras)
        {
            _context = context;
            _calculosDobras = calculosDobras;
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

            // Chama o método de cálculo do percentual de gordura e armazena o valor na variável
            
            
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
