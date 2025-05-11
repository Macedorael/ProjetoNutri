using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjetoNutri.Models;
using ProjetoNutri.Context;


namespace ProjetoNutri.Controllers
{
    public class PacienteController : Controller
     {
        private readonly ClienteContext _context;

        public PacienteController(ClienteContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var pacientes = _context.Pacientes.ToList();
            var agendamentos = _context.Agendamentos.ToList();

            var viewModel = new PacienteComAgendamentosViewModel
            {
                Pacientes = pacientes,
                Agendamentos = agendamentos
            };

            return View(viewModel);
        }

        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Criar(Paciente paciente)
        {
           if (ModelState.IsValid)
            {
                paciente.Ativo = true;

                paciente.DataCadastro = DateTime.Now.Date;

                paciente.DataNascimento = paciente.DataNascimento.Date;

                _context.Pacientes.Add(paciente);
                _context.SaveChanges();

                TempData["MensagemSucesso"] = "Paciente criado com sucesso!";
                
                return RedirectToAction("Index");
            }
            return View(paciente);
        }
        
        public IActionResult Editar(int id)
        {
            var paciente = _context.Pacientes.Find(id);

            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        [HttpPost]
        public IActionResult Editar(Paciente paciente)
        {
            var pacienteBanco = _context.Pacientes.Find(paciente.Id);

            pacienteBanco.Nome = paciente.Nome;
            pacienteBanco.Sobrenome = paciente.Sobrenome;
            pacienteBanco.DataNascimento = paciente.DataNascimento;
            pacienteBanco.Sexo = paciente.Sexo;
            pacienteBanco.Telefone = paciente.Telefone;
            pacienteBanco.Instagram = paciente.Instagram;
            pacienteBanco.Ativo = paciente.Ativo;

            _context.Pacientes.Update(pacienteBanco);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Detalhe(int id)
{
    var paciente = _context.Pacientes.Find(id);
    if (paciente == null)
    {
        return RedirectToAction(nameof(Index));
    }

    // Calcula idade
    int idade = ProjetoNutri.Services.CalculoIdade.Calcular(paciente.DataNascimento);
    ViewBag.Idade = idade;

    // Busca os agendamentos desse paciente
    var agendamentos = _context.Agendamentos
        .Where(a => a.IdPaciente == id)
        .OrderByDescending(a => a.Data)
        .ToList();
    ViewBag.Agendamentos = agendamentos;

    // Busca os projetos desse paciente
    var projetos = _context.Projetos
        .Where(p => p.PacienteId == id)
        .ToList();
    ViewBag.Projetos = projetos;

    return View(paciente);
}


        public IActionResult Deletar(int id)
        {
            var paciente = _context.Pacientes.Find(id);
            if (paciente == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(paciente);
        }
        
        [HttpPost]
        public IActionResult Deletar(Paciente paciente)
        {
            var pacienteBanco = _context.Pacientes.Find(paciente.Id);
            if (pacienteBanco == null)
            {
                return RedirectToAction(nameof(Index));
            }
            _context.Pacientes.Remove(pacienteBanco);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}