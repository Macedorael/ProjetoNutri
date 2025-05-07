using Microsoft.AspNetCore.Mvc;
using ProjetoNutri.Models;
using ProjetoNutri.Context;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ProjetoNutri.Controllers
{
    public class AgendamentoController : Controller
    {
        private readonly ClienteContext _context;
        private readonly GoogleAgendaService _googleAgendaService;

        public AgendamentoController(ClienteContext context)
        {
            _context = context;
            _googleAgendaService = new GoogleAgendaService();
        }

        public IActionResult CriarAgendamento()
        {
            ViewBag.Pacientes = _context.Pacientes
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Nome
                }).ToList();

            return View();
        }

        [HttpPost]
        public IActionResult CriarAgendamento(Agendamento agendamento)
        {
            if (ModelState.IsValid)
            {
                _context.Agendamentos.Add(agendamento);
                _context.SaveChanges();

                var pacienteNome = _context.Pacientes
                    .FirstOrDefault(p => p.Id == agendamento.IdPaciente)?.Nome;

                if (pacienteNome != null)
                {
                    var googleEventId = _googleAgendaService.CriarEvento(
                        pacienteNome,
                        agendamento.Data,
                        agendamento.Hora,
                        agendamento.Observacao
                    );

                    agendamento.GoogleEventId = googleEventId;

                    _context.Update(agendamento);
                    _context.SaveChanges();
                }

                return RedirectToAction("Detalhe", "Paciente", new { id = agendamento.IdPaciente });
            }

            ViewBag.Pacientes = _context.Pacientes
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Nome
                }).ToList();

            return View(agendamento);
        }

        [HttpGet]
        public IActionResult DeletarAgendamento(int id)
        {
            var agendamento = _context.Agendamentos.Find(id);

            if (agendamento == null)
                return NotFound();

            int pacienteId = agendamento.IdPaciente;

            if (!string.IsNullOrEmpty(agendamento.GoogleEventId))
            {
                _googleAgendaService.ExcluirEvento(agendamento.GoogleEventId);
            }

            _context.Agendamentos.Remove(agendamento);
            _context.SaveChanges();

            return RedirectToAction("Detalhe", "Paciente", new { id = pacienteId });
        }

        public IActionResult Index()
        {
            var agendamentos = _context.Agendamentos.Include(a => a.Paciente).ToList();
            return View(agendamentos);
        }
    }
}
