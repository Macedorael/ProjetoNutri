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
    public class ProjetoController : Controller
    {
        private readonly ClienteContext _context;
        private readonly CalculosCircunferencia _calculosCircunferencia;
        private readonly CalculosDobras _calculosDobras;
        private readonly CalculoImc _calculoImc;
    
        public ProjetoController(ClienteContext context, CalculosCircunferencia calculosCircunferencia, CalculosDobras calculosDobras, CalculoImc calculoImc)
        {
            _context = context;
            _calculosCircunferencia = calculosCircunferencia;
            _calculosDobras = calculosDobras;
            _calculoImc = calculoImc;
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

            // Buscar o projeto específico
            var projeto = _context.Projetos
                .Include(p => p.Paciente) // Carregar os dados do paciente
                .FirstOrDefault(p => p.Id == projetoId);
            var imc = _context.Imcs.Include(i => i.Projeto).ThenInclude(p => p.Paciente).FirstOrDefault(i => i.IdProjeto == projetoId);
            // Carregar dados das tabelas associadas (Imcs, Circunferencias, Pregas)
            var imcs = _context.Imcs.Where(i => i.IdProjeto == projetoId).ToList();
            var circunferencias = _context.Circunferencias.Where(i => i.IdProjeto == projetoId).ToList();
            var pregas = _context.Pregas.Where(i => i.IdProjeto == projetoId).ToList();
            var prega = _context.Pregas
            .Include(p => p.Projeto)
            .ThenInclude(projeto => projeto.Paciente)
            .FirstOrDefault(p => p.IdProjeto == projetoId);
            var circunferencia = _context.Circunferencias
            .Include(c => c.Projeto)       // Inclui o Projeto relacionado
            .ThenInclude(p => p.Paciente)  // Inclui o Paciente relacionado ao Projeto
            .FirstOrDefault(c => c.IdProjeto == projetoId);

        

            
            var PercentualGorduraPollock3 = _calculosDobras.CalculoPollock3(prega);
        
            var PercentualGorduraPollock7 = _calculosDobras.CalculoPollock7(prega);
            var PercentualGorduraPetroski = _calculosDobras.CalculoPetroski(prega);
            double? PercentualGorduraGuedes = _calculosDobras.CalculoGuedes(prega);
            double? PercentualGorduraDurnin = _calculosDobras.CalculoDurnin(prega);
            double? PercentualGorduraFaulkner = _calculosDobras.CalculoFaulkner(prega);
            var (rcq, classificacao) = _calculosCircunferencia.CalcularRCQ(circunferencia.Cintura, circunferencia.Quadril, circunferencia.Projeto.Paciente.Sexo); 
            var (imcNovo, classificacaoImc, pesoIdeal) = _calculoImc.CalcularImc(imc.Peso, imc.Altura, projeto.Paciente.Sexo);
            var (cmb, precentagemCmb, classificacaoCmb) = _calculoImc.CalcularCMBCompleto(circunferencia.Bracodireito, imc.Altura, projeto.Paciente.Sexo);


            ViewBag.Peso = imc?.Peso;
            ViewBag.Altura = imc?.Altura;
            ViewBag.ValorImc = imcNovo;
            ViewBag.ClassificacaoImc = classificacaoImc;
            ViewBag.RCQ = rcq;
            ViewBag.ClassificacaoRCQ = classificacao;
            ViewBag.PesoIdeal = pesoIdeal;
            ViewBag.CMB = cmb;
            ViewBag.PorcentagemCMB = precentagemCmb;
            ViewBag.ClassificacaoCMB = classificacaoCmb;
           
             
            // Se o cálculo foi bem-sucedido, exibe o resultado
           if (PercentualGorduraPollock3 != default && PercentualGorduraPollock3.PercentualGordura.HasValue && PercentualGorduraPollock3.SomaPrega.HasValue && 
                PercentualGorduraPollock3.DensidadeCorporal.HasValue)
            {
                
                try
                {
                    // Verifique se imc e projeto não são null
                    if (imc == null || projeto == null || projeto.Paciente == null)
                    {
                        ViewBag.MensagemErroPollock3 = "Dados de IMC ou paciente não encontrados.";
                    }
                    else
                    {   
                        DateTime dataNascimento = prega.Projeto.Paciente.DataNascimento;
                        int idade = ProjetoNutri.Services.CalculoIdade.Calcular(dataNascimento);
                        var (pesoGordura, pesoMassaMagra) = _calculosDobras.CalcularPesoGorduraEMassaMagra(imc.Peso, PercentualGorduraPollock3.PercentualGordura.Value);
                        double pesoResidual = _calculosDobras.CalcularPesoResidual(imc.Peso, imc.Altura, idade, projeto.Paciente.Sexo);
                        var classificacaoGordura = _calculosDobras.ClassificarGordura(PercentualGorduraPollock3.PercentualGordura.Value, projeto.Paciente.Sexo);
                    

                        ViewBag.ClassificacaoGorduraPollock3 = classificacaoGordura;
                        ViewBag.PercentualGorduraPollock3 = PercentualGorduraPollock3.PercentualGordura.Value;
                        ViewBag.PercentualDensidadeCorporal = PercentualGorduraPollock3.DensidadeCorporal.Value;
                        ViewBag.SomaPregas = PercentualGorduraPollock3.SomaPrega.Value;
                        ViewBag.PesoResidual = pesoResidual;
                        ViewBag.PesoGorduraPollock3 = pesoGordura;
                        ViewBag.PesoMassaMagraPollock3 = pesoMassaMagra;
                    }
                }
                catch (Exception ex)
                {
                    // Log do erro (opcional)
                    Console.WriteLine("Erro ao calcular Pollock3: " + ex.Message);

                    // Exiba uma mensagem de erro genérica
                    ViewBag.MensagemErroPollock3 = "Ocorreu um erro ao calcular os valores usando o método Pollock3.";
                }
            }
            else
            {
                // Se o cálculo do Pollock3 falhar, exibe uma mensagem de erro
                ViewBag.MensagemErroPollock3 = "Não foi possível calcular o percentual de gordura usando o método Pollock3. Faltam dados necessários.";
            }

            if (PercentualGorduraPollock7 != default && PercentualGorduraPollock7.PercentualGordura.HasValue && PercentualGorduraPollock7.SomaPrega.HasValue && 
                PercentualGorduraPollock7.DensidadeCorporal.HasValue)
            {
                Console.WriteLine($"Peso: {imc?.Peso}, Altura: {imc?.Altura}"); // Depuração
                try
                {
                    // Verifique se imc e projeto não são null
                    if (imc == null || projeto == null || projeto.Paciente == null)
                    {
                        ViewBag.MensagemErroPollock7 = "Dados de IMC ou paciente não encontrados.";
                    }
                    else
                    {   
                        DateTime dataNascimento = prega.Projeto.Paciente.DataNascimento;
                        int idade = ProjetoNutri.Services.CalculoIdade.Calcular(dataNascimento);
                        var (pesoGordura, pesoMassaMagra) = _calculosDobras.CalcularPesoGorduraEMassaMagra(imc.Peso, PercentualGorduraPollock7.PercentualGordura.Value);
                        double pesoResidual = _calculosDobras.CalcularPesoResidual(imc.Peso, imc.Altura, idade, projeto.Paciente.Sexo);
                        var classificacaoGordura = _calculosDobras.ClassificarGordura(PercentualGorduraPollock7.PercentualGordura.Value, projeto.Paciente.Sexo);
                        

                        ViewBag.ClassificacaoGorduraPollock7 = classificacaoGordura;
                        ViewBag.PercentualGorduraPollock7 = PercentualGorduraPollock7.PercentualGordura.Value;
                        ViewBag.PercentualDensidadeCorporalPollock7 = PercentualGorduraPollock7.DensidadeCorporal.Value;
                        ViewBag.SomaPregasPollock7 = PercentualGorduraPollock7.SomaPrega.Value;
                        ViewBag.PesoResidualPollock7 = pesoResidual;
                        ViewBag.PesoGorduraPollock7 = pesoGordura;
                        ViewBag.PesoMassaMagraPollock7= pesoMassaMagra;
                    }
                }
                catch (Exception ex)
                {
                    // Log do erro (opcional)
                    Console.WriteLine("Erro ao calcular Pollock3: " + ex.Message);

                    // Exiba uma mensagem de erro genérica
                    ViewBag.MensagemErroPollock7 = "Ocorreu um erro ao calcular os valores usando o método Pollock3.";
                }
            }
            else
            {
                // Se o cálculo do Pollock3 falhar, exibe uma mensagem de erro
                ViewBag.MensagemErroPollock7 = "Não foi possível calcular o percentual de gordura usando o método Pollock3. Faltam dados necessários.";
            }

            if (PercentualGorduraPetroski != default && PercentualGorduraPetroski.PercentualGordura.HasValue && PercentualGorduraPetroski.SomaPrega.HasValue && 
                PercentualGorduraPetroski.DensidadeCorporal.HasValue)
            {
                Console.WriteLine($"Peso: {imc?.Peso}, Altura: {imc?.Altura}"); // Depuração
                try
                {
                    // Verifique se imc e projeto não são null
                    if (imc == null || projeto == null || projeto.Paciente == null)
                    {
                        ViewBag.MensagemErroPetroski = "Dados de IMC ou paciente não encontrados.";
                    }
                    else
                    {   
                        DateTime dataNascimento = prega.Projeto.Paciente.DataNascimento;
                        int idade = ProjetoNutri.Services.CalculoIdade.Calcular(dataNascimento);
                        var (pesoGordura, pesoMassaMagra) = _calculosDobras.CalcularPesoGorduraEMassaMagra(imc.Peso, PercentualGorduraPetroski.PercentualGordura.Value);
                        double pesoResidual = _calculosDobras.CalcularPesoResidual(imc.Peso, imc.Altura, idade, projeto.Paciente.Sexo);


                        ViewBag.PercentualGorduraPetroski = PercentualGorduraPetroski.PercentualGordura.Value;
                        ViewBag.PercentualDensidadeCorporalPetroski = PercentualGorduraPetroski.DensidadeCorporal.Value;
                        ViewBag.SomaPregasPetroski = PercentualGorduraPetroski.SomaPrega.Value;
                        ViewBag.PesoResidualPetroski = pesoResidual;
                        ViewBag.PesoGorduraPetroski = pesoGordura;
                        ViewBag.PesoMassaMagraPetroski= pesoMassaMagra;
                    }
                }
                catch (Exception ex)
                {
                    // Log do erro (opcional)
                    Console.WriteLine("Erro ao calcular Petroski: " + ex.Message);

                    // Exiba uma mensagem de erro genérica
                    ViewBag.MensagemErroPetroski = "Ocorreu um erro ao calcular os valores usando o método Petroski.";
                }
            }
            else
            {
                // Se o cálculo do Pollock3 falhar, exibe uma mensagem de erro
                ViewBag.MensagemErroPollock7 = "Não foi possível calcular o percentual de gordura usando o método Pollock3. Faltam dados necessários.";
            }


            if (PercentualGorduraGuedes.HasValue)
            {
                ViewBag.PercentualGorduraGuedes = PercentualGorduraGuedes.Value;
                var (pesoGordura, pesoMassaMagra) = _calculosDobras.CalcularPesoGorduraEMassaMagra(imc.Peso, PercentualGorduraGuedes.Value);
                ViewBag.PesoGorduraGuedes = pesoGordura;
                ViewBag.PesoMassaMagraGuedes = pesoMassaMagra;
            }
            else
            {
                // Se o cálculo do Guedes falhar, exibe uma mensagem de erro
                ViewBag.MensagemErroGuedes = "Não foi possível calcular o percentual de gordura usando o método Guedes. Faltam dados necessários.";
            }   

            if (PercentualGorduraDurnin.HasValue)
            {
                ViewBag.PercentualGorduraDurnin = PercentualGorduraDurnin.Value;
                var (pesoGordura, pesoMassaMagra) = _calculosDobras.CalcularPesoGorduraEMassaMagra(imc.Peso, PercentualGorduraDurnin.Value);
                ViewBag.PesoGorduraDurnin = pesoGordura;
                ViewBag.PesoMassaMagraDurnin = pesoMassaMagra;
            }
            else
            {
                // Se o cálculo do Durnin falhar, exibe uma mensagem de erro
                ViewBag.MensagemErroDurnin = "Não foi possível calcular o percentual de gordura usando o método Durnin. Faltam dados necessários.";
            }

            if (PercentualGorduraFaulkner.HasValue)
            {
                ViewBag.PercentualGorduraFaulkner = PercentualGorduraFaulkner.Value;
                var (pesoGordura, pesoMassaMagra) = _calculosDobras.CalcularPesoGorduraEMassaMagra(imc.Peso, PercentualGorduraFaulkner.Value);
                ViewBag.PesoGorduraFaulkner = pesoGordura;
                ViewBag.PesoMassaMagraFaulkner = pesoMassaMagra;
            }
            else
            {
                // Se o cálculo do Faulkner falhar, exibe uma mensagem de erro
                ViewBag.MensagemErroFaulkner = "Não foi possível calcular o percentual de gordura usando o método Faulkner. Faltam dados necessários.";
            }
            // Criar o ViewModel com os dados carregados
            var viewModel = new ProjetoImcViewModel
            {
                Projeto = projeto,
                Imcs = imcs,
                Circunferencias = circunferencias,
                Prega = pregas  // Passando a lista de Pregas para o ViewModel
            };

            // Retorne a view com o ViewModel
            return View(viewModel);
        }


    }
}
