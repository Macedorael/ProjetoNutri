using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoNutri.Context;
using ProjetoNutri.Models;
using ProjetoNutri.Services;

namespace ProjetoNutri.Controllers
{
    public class CircunferenciaController : Controller
    {
        private readonly ClienteContext _context;
        private readonly CalculosCircunferencia _calculosCircunferencia;

        public CircunferenciaController(ClienteContext context, CalculosCircunferencia calculosCircunferencia)
        {
            _context = context;
            _calculosCircunferencia = calculosCircunferencia;
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


                // Adicionar à base de dados
                _context.Circunferencias.Add(circunferencia);
                _context.SaveChanges();

                // Redireciona para a listagem de Circuferencias do Projeto
                return RedirectToAction("AntropometriaProjeto", "Projeto", new { projetoId = circunferencia.IdProjeto });
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
            var (rcq, classificacao) = _calculosCircunferencia.CalcularRCQ(circunferencia.Cintura, circunferencia.Quadril, circunferencia.Projeto.Paciente.Sexo);

        // Passa os resultados para a View
            ViewBag.RCQ = rcq;
            ViewBag.ClassificacaoRCQ = classificacao;


            return View(circunferencia);
        }

        // GET: Editar Circunferência
        public IActionResult EditarCircunferencia(int id)
        {
            var circunferencia = _context.Circunferencias
                .Include(c => c.Projeto)  // Inclui o Projeto associado
                .ThenInclude(projeto => projeto.Paciente) // Inclui o Paciente associado ao Projeto
                .FirstOrDefault(c => c.Id == id);

            if (circunferencia == null)
            {
                return NotFound(); // Se a Circunferência não for encontrada, retorna erro 404
            }

            return View(circunferencia); // Exibe o formulário de edição
        }

        // POST: Editar Circunferência
        [HttpPost]
        public IActionResult EditarCircunferencia(int id, Circunferencia circunferencia)
        {
            if (id != circunferencia.Id)
            {
                return NotFound(); // Verifica se o ID passado no URL é o mesmo que o ID do objeto
            }

            if (ModelState.IsValid)
            {
                // Verifica se a Circunferência existe no banco de dados
                var circunferenciaBanco = _context.Circunferencias
                    .Include(c => c.Projeto)  // Inclui o projeto para acessar dados relacionados
                    .ThenInclude(projeto => projeto.Paciente) // Inclui o paciente associado ao projeto
                    .FirstOrDefault(c => c.Id == id);

                if (circunferenciaBanco == null)
                {
                    return NotFound(); // Se a circunferência não for encontrada, retorna erro 404
                }

                // Atualiza todos os campos que podem ser modificados (você pode escolher os campos a atualizar)
                circunferenciaBanco.Pescoco = circunferencia.Pescoco;
                circunferenciaBanco.Ombro = circunferencia.Ombro;
                circunferenciaBanco.Torax = circunferencia.Torax;
                circunferenciaBanco.Bracodireito = circunferencia.Bracodireito;
                circunferenciaBanco.Bracoesquerdo = circunferencia.Bracoesquerdo;
                circunferenciaBanco.Bracodireitocontraido = circunferencia.Bracodireitocontraido;
                circunferenciaBanco.Bracoesquerdocontraido = circunferencia.Bracoesquerdocontraido;
                circunferenciaBanco.Antebracodireito = circunferencia.Antebracodireito;
                circunferenciaBanco.Antebracoesquerdo = circunferencia.Antebracoesquerdo;
                circunferenciaBanco.Cintura = circunferencia.Cintura;
                circunferenciaBanco.Abdome = circunferencia.Abdome;
                circunferenciaBanco.Quadril = circunferencia.Quadril;
                circunferenciaBanco.Coxadistaldireita = circunferencia.Coxadistaldireita;
                circunferenciaBanco.Coxadistalesquerda = circunferencia.Coxadistalesquerda;
                circunferenciaBanco.Coxamedialdireita = circunferencia.Coxamedialdireita;
                circunferenciaBanco.Coxamedialesquerda = circunferencia.Coxamedialesquerda;
                circunferenciaBanco.Coxaproximaldireita = circunferencia.Coxaproximaldireita;
                circunferenciaBanco.Coxaproximalesquerda = circunferencia.Coxaproximalesquerda;
                circunferenciaBanco.Panturrilhadireita = circunferencia.Panturrilhadireita;
                circunferenciaBanco.Panturrilhaesquerda = circunferencia.Panturrilhaesquerda;

                // Desanexa a circunferência do contexto se estiver sendo rastreada
                _context.Entry(circunferenciaBanco).State = EntityState.Detached;

                // Re-anexa a entidade com o novo estado para evitar conflitos
                _context.Circunferencias.Update(circunferenciaBanco);

                // Salva as alterações no banco de dados
                _context.SaveChanges();

                // Redireciona para a página de listagem de circunferências após salvar
                return RedirectToAction("AntropometriaProjeto", "Projeto", new { projetoId = circunferenciaBanco.IdProjeto });
            }

            // Se o modelo for inválido, retorna o formulário de edição com erros
            return View(circunferencia);
        }

        [HttpPost]
        public IActionResult DeletarCircunferencia(int id)
        {
            var circunferencia = _context.Circunferencias.Find(id);

            if (circunferencia == null)
            {
                return NotFound();
            }

            _context.Circunferencias.Remove(circunferencia);
            _context.SaveChanges();

            return RedirectToAction("AntropometriaProjeto","Projeto", new { projetoId = circunferencia.IdProjeto });
        }
    }
}