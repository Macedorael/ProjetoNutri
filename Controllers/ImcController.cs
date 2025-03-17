using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoNutri.Context;
using ProjetoNutri.Models;

namespace ProjetoNutri.Controllers
{
    public class ImcController : Controller
    {
        private readonly ClienteContext _context;

        public ImcController(ClienteContext context)
        {
            _context = context;
        }

        public IActionResult IndexImc(int projetoId)
        {
            var imcs = _context.Imcs.Include(imc => imc.Projeto).ThenInclude(projeto => projeto.Paciente).Where(imc => imc.IdProjeto == projetoId).ToList();
            ViewData["ProjetoId"] = projetoId;
            imcs.ForEach(p => p.Projeto = _context.Projetos.Find(p.IdProjeto));

            return View(imcs);
        }

        public IActionResult CriarImc(int projetoId)
        {
            var imc = new Imc { IdProjeto = projetoId };
            return View(imc);
        }

        [HttpPost]
        public IActionResult CriarImc(Imc imc)
        {
            if (ModelState.IsValid)
            {
                var projeto = _context.Projetos.Include(p => p.Paciente).FirstOrDefault(p => p.Id == imc.IdProjeto);
                var paciente = projeto.Paciente;
                var sexoDoPaciente = paciente.Sexo.ToLower();
                

                Console.WriteLine($"Peso: {imc.Peso}, Altura: {imc.Altura}");
                Console.WriteLine($"Peso: {imc.Peso}, Altura: {imc.Altura}");

                imc.ValorImc = imc.Peso / (imc.Altura * imc.Altura);

                if (sexoDoPaciente == "masculino")
                {
                    if (imc.ValorImc < 20.7)
                    {
                        imc.Classificacao = "Abaixo do peso";
                    }
                    else if (imc.ValorImc >= 20.7 && imc.ValorImc < 26.4)
                    {
                        imc.Classificacao = "Peso normal";
                    }
                    else if (imc.ValorImc >= 26.4 && imc.ValorImc < 31.1)
                    {
                        imc.Classificacao = "Sobrepeso";
                    }
                    else
                    {
                        imc.Classificacao = "Obesidade";
                    }
                }
                else if (sexoDoPaciente == "feminino")
                {
                    if (imc.ValorImc < 19.1)
                    {
                        imc.Classificacao = "Abaixo do peso";
                    }
                    else if (imc.ValorImc >= 19.1 && imc.ValorImc < 25.8)
                    {
                        imc.Classificacao = "Peso normal";
                    }
                    else if (imc.ValorImc >= 25.8 && imc.ValorImc < 32.3)
                    {
                        imc.Classificacao = "Sobrepeso";
                    }
                    else
                    {
                        imc.Classificacao = "Obesidade";
                    }
                }
                Console.WriteLine($"Peso: {imc.Peso}, Altura: {imc.Altura}");
                _context.Imcs.Add(imc);
                _context.SaveChanges();

                return RedirectToAction("AntropometriaProjeto","Projeto", new { projetoId = imc.IdProjeto });
            }

            return View(imc);
        }

        public IActionResult EditarImc(int id, int projetoId)
        {
            var imc = _context.Imcs.Find(id);
            if (imc == null)
            {
                return NotFound();
            }
            ViewData["ProjetoId"] = projetoId;
            return View(imc);
        }

        [HttpPost]
        public IActionResult EditarImc(Imc imc)
        {
            if (ModelState.IsValid)
            {
                var imcBanco = _context.Imcs.Find(imc.Id);

                imcBanco.Altura = imc.Altura;
                imcBanco.Peso = imc.Peso;


                _context.Imcs.Update(imcBanco);
                _context.SaveChanges();

                return RedirectToAction("IndexImc", new { projetoId = imcBanco.IdProjeto });
            }

            return View(imc);
        }

        public IActionResult DetalheImc(int id)
        {
            var imc = _context.Imcs.Include(i => i.Projeto).ThenInclude(p => p.Paciente).FirstOrDefault(i => i.Id == id);

            if (imc == null)
            {
                return NotFound();
            }

            return View(imc);
        }

        public IActionResult DeletarImc(int id)
        {
            var imc = _context.Imcs
                            .Include(i => i.Projeto)
                            .ThenInclude(p => p.Paciente)
                            .FirstOrDefault(i => i.Id == id);

            if (imc == null)
            {
                return NotFound();
            }

            return View(imc);
        }

        [HttpPost]
        public IActionResult DeletarImc(Imc imc)
        {
            var imcs = _context.Imcs.Find(imc.Id);

            if (imc == null)
            {
                return NotFound();
            }

            _context.Imcs.Remove(imcs);
            _context.SaveChanges();

            return RedirectToAction("AntropometriaProjeto","Projeto", new { projetoId = imcs.IdProjeto });
        }

    }
}
