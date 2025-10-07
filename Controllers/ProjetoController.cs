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

                return RedirectToAction("Detalhe", "Paciente", new { id = projeto.PacienteId });
            }

            return View(projeto);
        }



        public IActionResult DetalheProjeto(int id)
        {
            var projeto = _context.Projetos
                .Include(p => p.Paciente)
                .FirstOrDefault(p => p.Id == id);

            var imc = _context.Imcs.Include(i => i.Projeto).ThenInclude(p => p.Paciente).FirstOrDefault(i => i.IdProjeto == id);
            // Carregar dados das tabelas associadas (Imcs, Circunferencias, Pregas)
            var imcs = _context.Imcs.Where(i => i.IdProjeto == id).ToList();
            var circunferencias = _context.Circunferencias.Where(i => i.IdProjeto == id).ToList();
             var circunferencia = _context.Circunferencias
            .Include(c => c.Projeto)       // Inclui o Projeto relacionado
            .ThenInclude(p => p.Paciente)  // Inclui o Paciente relacionado ao Projeto
            .FirstOrDefault(c => c.IdProjeto == id);

            if (circunferencia != null)
            {
                var (rcq, classificacao) = _calculosCircunferencia.CalcularRCQ(circunferencia.Cintura, circunferencia.Quadril, projeto.Paciente.Sexo);
                ViewBag.RCQ = rcq;
                ViewBag.ClassificacaoRCQ = classificacao;
            }      
            // Calcula RCQ
            if (imc != null)
            {
                ViewBag.Peso = imc.Peso;
                ViewBag.Altura = imc.Altura;

                var (imcNovo, classificacaoImc, pesoIdeal) = _calculoImc.CalcularImc(imc.Peso, imc.Altura, projeto.Paciente.Sexo);
                ViewBag.ValorImc = imcNovo;
                ViewBag.ClassificacaoImc = classificacaoImc;
                ViewBag.PesoIdeal = pesoIdeal;
                if (circunferencia != null)
                {

                    var (cmb, precentagemCmb, classificacaoCmb) = _calculoImc.CalcularCMBCompleto(circunferencia.Bracodireito, imc.Altura, projeto.Paciente.Sexo);
                    ViewBag.CMB = cmb;
                    ViewBag.PorcentagemCMB = precentagemCmb;
                    ViewBag.ClassificacaoCMB = classificacaoCmb;
                }
            }
            else
            {
                ViewBag.MensagemErro = "Dados de IMC ou paciente não encontrados.";
            }

            if (projeto == null)
                return NotFound();

            // Recupera todas as refeições do projeto
            var refeicoes = _context.Refeicoes
                        .Include(r => r.Refeicao_Alimentos)
                        .ThenInclude(ra => ra.Alimento)
                        .Where(r => r.IdProjeto == id) // Filtra as refeições pelo ProjetoId
                        .OrderByDescending(r => r.DataCriacao)
                        .ToList();

                    // Criando dicionários para armazenar as somas por refeição
                    var totalProteinaPorRefeicao = refeicoes.ToDictionary(
                        refeicao => refeicao.Id,
                        refeicao => refeicao.Refeicao_Alimentos?.Sum(ra => ra.Alimento.Proteina) ?? 0
                    );

                    var totalLipidioPorRefeicao = refeicoes.ToDictionary(
                        refeicao => refeicao.Id,
                        refeicao => refeicao.Refeicao_Alimentos?.Sum(ra => ra.Alimento.Lipidio) ?? 0
                    );

                    var totalCarboidratoPorRefeicao = refeicoes.ToDictionary(
                        refeicao => refeicao.Id,
                        refeicao => refeicao.Refeicao_Alimentos?.Sum(ra => ra.Alimento.Carboidrato) ?? 0
                    );

                    var totalKcalPorRefeicao = refeicoes.ToDictionary(
                        refeicao => refeicao.Id,
                        refeicao => refeicao.Refeicao_Alimentos?.Sum(ra => ra.Alimento.Energia_Kcal) ?? 0
                    );

                    var totalKjPorRefeicao = refeicoes.ToDictionary(
                        refeicao => refeicao.Id,
                        refeicao => refeicao.Refeicao_Alimentos?.Sum(ra => ra.Alimento.Energia_KJ) ?? 0
                    );

                    // Calculando os totais gerais
                    double totalProteinaGeral = totalProteinaPorRefeicao.Values.Sum();
                    double totalLipidioGeral = totalLipidioPorRefeicao.Values.Sum();
                    double totalCarboidratoGeral = totalCarboidratoPorRefeicao.Values.Sum();
                    double totalKcalGeral = totalKcalPorRefeicao.Values.Sum();
                    double totalKjGeral = totalKjPorRefeicao.Values.Sum();

                    // Criando o ViewModel para a View
                // Criando o ViewModel para a View
        var viewModel = new ProjetoDietaViewModel
        {
            Refeicaos = refeicoes,
            TotalProteinaPorRefeicao = totalProteinaPorRefeicao,
            TotalLipidioPorRefeicao = totalLipidioPorRefeicao,
            TotalCarboidratoPorRefeicao = totalCarboidratoPorRefeicao,
            TotalKcalPorRefeicao = totalKcalPorRefeicao,
            TotalKjPorRefeicao = totalKjPorRefeicao,
            TotalProteinaGeral = totalProteinaGeral,
            TotalLipidioGeral = totalLipidioGeral,
            TotalCarboidratoGeral = totalCarboidratoGeral,
            TotalKcalGeral = totalKcalGeral,
            TotalKjGeral = totalKjGeral,
            Projeto = projeto
        };

        // ✅ Corrigido aqui:
        return View(viewModel);

        }


        [HttpGet]
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
        [ValidateAntiForgeryToken]
        public IActionResult EditarProjeto(int id, string NomeProjeto)
        {
            var projetoExistente = _context.Projetos.Find(id);
            if (projetoExistente == null)
            {
                return NotFound();
            }

            projetoExistente.NomeProjeto = NomeProjeto;
            _context.SaveChanges();

            return Ok(); // retorno simplificado para AJAX
        }


        

        [HttpPost]
        public IActionResult DeletarProjeto(int id, int pacienteId)
        {
            var projetoBanco = _context.Projetos.Find(id);
            if (projetoBanco == null)
            {
                // Se o projeto não for encontrado, redireciona para a página de projetos
                return RedirectToAction("IndexProjeto", new { pacienteId });
            }

            // Remove o projeto do banco de dados
            _context.Projetos.Remove(projetoBanco);
            _context.SaveChanges();

            // Redireciona para a página de detalhes do paciente
            return RedirectToAction("Detalhe", "Paciente", new { id = pacienteId });
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
            var PercentualGorduraGuedes = _calculosDobras.CalculoGuedes(prega);
            var PercentualGorduraDurnin = _calculosDobras.CalculoDurnin(prega);
            var PercentualGorduraFaulkner = _calculosDobras.CalculoFaulkner(prega);
            // Verifica se os objetos não são nulo
            if (circunferencia != null)
            {
                var (rcq, classificacao) = _calculosCircunferencia.CalcularRCQ(circunferencia.Cintura, circunferencia.Quadril, projeto.Paciente.Sexo);
                ViewBag.RCQ = rcq;
                ViewBag.ClassificacaoRCQ = classificacao;
            }      
            // Calcula RCQ
            if (imc != null)
            {
                ViewBag.Peso = imc.Peso;
                ViewBag.Altura = imc.Altura;

                var (imcNovo, classificacaoImc, pesoIdeal) = _calculoImc.CalcularImc(imc.Peso, imc.Altura, projeto.Paciente.Sexo);
                ViewBag.ValorImc = imcNovo;
                ViewBag.ClassificacaoImc = classificacaoImc;
                ViewBag.PesoIdeal = pesoIdeal;
                if (circunferencia != null)
                {

                    var (cmb, precentagemCmb, classificacaoCmb) = _calculoImc.CalcularCMBCompleto(circunferencia.Bracodireito, imc.Altura, projeto.Paciente.Sexo);
                    ViewBag.CMB = cmb;
                    ViewBag.PorcentagemCMB = precentagemCmb;
                    ViewBag.ClassificacaoCMB = classificacaoCmb;
                }
            }
            else
            {
                ViewBag.MensagemErro = "Dados de IMC ou paciente não encontrados.";
            }
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
                            ViewBag.PesoMassaMagraPollock7 = pesoMassaMagra;
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
                            var classificacaoGordura = _calculosDobras.ClassificarGordura(PercentualGorduraPetroski.PercentualGordura.Value, projeto.Paciente.Sexo);


                            ViewBag.ClassificacaoGorduraPetroski = classificacaoGordura;
                            ViewBag.PercentualGorduraPetroski = PercentualGorduraPetroski.PercentualGordura.Value;
                            ViewBag.PercentualDensidadeCorporalPetroski = PercentualGorduraPetroski.DensidadeCorporal.Value;
                            ViewBag.SomaPregasPetroski = PercentualGorduraPetroski.SomaPrega.Value;
                            ViewBag.PesoResidualPetroski = pesoResidual;
                            ViewBag.PesoGorduraPetroski = pesoGordura;
                            ViewBag.PesoMassaMagraPetroski = pesoMassaMagra;
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
                    ViewBag.MensagemErroPetroski = "Não foi possível calcular o percentual de gordura usando o método Petroski. Faltam dados necessários.";
                }


                if (PercentualGorduraGuedes != default && PercentualGorduraGuedes.PercentualGordura.HasValue && PercentualGorduraGuedes.SomaPrega.HasValue &&
                    PercentualGorduraGuedes.DensidadeCorporal.HasValue)
                {
                    Console.WriteLine($"Peso: {imc?.Peso}, Altura: {imc?.Altura}"); // Depuração
                    try
                    {
                        // Verifique se imc e projeto não são null
                        if (imc == null || projeto == null || projeto.Paciente == null)
                        {
                            ViewBag.MensagemErroGuedes = "Dados de IMC ou paciente não encontrados.";
                        }
                        else
                        {
                            DateTime dataNascimento = prega.Projeto.Paciente.DataNascimento;
                            int idade = ProjetoNutri.Services.CalculoIdade.Calcular(dataNascimento);
                            var (pesoGordura, pesoMassaMagra) = _calculosDobras.CalcularPesoGorduraEMassaMagra(imc.Peso, PercentualGorduraGuedes.PercentualGordura.Value);
                            double pesoResidual = _calculosDobras.CalcularPesoResidual(imc.Peso, imc.Altura, idade, projeto.Paciente.Sexo);
                            var classificacaoGordura = _calculosDobras.ClassificarGordura(PercentualGorduraGuedes.PercentualGordura.Value, projeto.Paciente.Sexo);


                            ViewBag.ClassificacaoGorduraGuedes = classificacaoGordura;
                            ViewBag.PercentualGorduraGuedes = PercentualGorduraGuedes.PercentualGordura.Value;
                            ViewBag.PercentualDensidadeCorporalGuedes = PercentualGorduraGuedes.DensidadeCorporal.Value;
                            ViewBag.SomaPregasGuedes = PercentualGorduraGuedes.SomaPrega.Value;
                            ViewBag.PesoResidualGuedes = pesoResidual;
                            ViewBag.PesoGorduraGuedes = pesoGordura;
                            ViewBag.PesoMassaMagraGuedes = pesoMassaMagra;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log do erro (opcional)
                        Console.WriteLine("Erro ao calcular Guedes: " + ex.Message);

                        // Exiba uma mensagem de erro genérica
                        ViewBag.MensagemErroGuedes = "Ocorreu um erro ao calcular os valores usando o método Guedes.";
                    }
                }
                else
                {
                    // Se o cálculo do Pollock3 falhar, exibe uma mensagem de erro
                    ViewBag.MensagemErroGuedes = "Não foi possível calcular o percentual de gordura usando o método Guedes. Faltam dados necessários.";
                }

                if (PercentualGorduraDurnin != default && PercentualGorduraDurnin.PercentualGordura.HasValue && PercentualGorduraDurnin.SomaPrega.HasValue &&
                    PercentualGorduraDurnin.DensidadeCorporal.HasValue)
                {
                    Console.WriteLine($"Peso: {imc?.Peso}, Altura: {imc?.Altura}"); // Depuração
                    try
                    {
                        // Verifique se imc e projeto não são null
                        if (imc == null || projeto == null || projeto.Paciente == null)
                        {
                            ViewBag.MensagemErroDurnin = "Dados de IMC ou paciente não encontrados.";
                        }
                        else
                        {
                            DateTime dataNascimento = prega.Projeto.Paciente.DataNascimento;
                            int idade = ProjetoNutri.Services.CalculoIdade.Calcular(dataNascimento);
                            var (pesoGordura, pesoMassaMagra) = _calculosDobras.CalcularPesoGorduraEMassaMagra(imc.Peso, PercentualGorduraDurnin.PercentualGordura.Value);
                            double pesoResidual = _calculosDobras.CalcularPesoResidual(imc.Peso, imc.Altura, idade, projeto.Paciente.Sexo);
                            var classificacaoGordura = _calculosDobras.ClassificarGordura(PercentualGorduraDurnin.PercentualGordura.Value, projeto.Paciente.Sexo);


                            ViewBag.ClassificacaoGorduraDurnin = classificacaoGordura;
                            ViewBag.PercentualGorduraDurnin = PercentualGorduraDurnin.PercentualGordura.Value;
                            ViewBag.PercentualDensidadeCorporalDurnin = PercentualGorduraDurnin.DensidadeCorporal.Value;
                            ViewBag.SomaPregasDurnin = PercentualGorduraDurnin.SomaPrega.Value;
                            ViewBag.PesoResidualDurnin = pesoResidual;
                            ViewBag.PesoGorduraDurnin = pesoGordura;
                            ViewBag.PesoMassaMagraDurnin = pesoMassaMagra;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log do erro (opcional)
                        Console.WriteLine("Erro ao calcular Durnin: " + ex.Message);

                        // Exiba uma mensagem de erro genérica
                        ViewBag.MensagemErroDurnin = "Ocorreu um erro ao calcular os valores usando o método Durnin.";
                    }
                }
                else
                {
                    // Se o cálculo do Pollock3 falhar, exibe uma mensagem de erro
                    ViewBag.MensagemErroDurnin = "Não foi possível calcular o percentual de gordura usando o método Durnin. Faltam dados necessários.";
                }

                if (PercentualGorduraFaulkner != default && PercentualGorduraFaulkner.PercentualGordura.HasValue && PercentualGorduraFaulkner.SomaPrega.HasValue &&
                    PercentualGorduraFaulkner.DensidadeCorporal.HasValue)
                {
                    Console.WriteLine($"Peso: {imc?.Peso}, Altura: {imc?.Altura}"); // Depuração
                    try
                    {
                        // Verifique se imc e projeto não são null
                        if (imc == null || projeto == null || projeto.Paciente == null)
                        {
                            ViewBag.MensagemErroFaulkner = "Dados de IMC ou paciente não encontrados.";
                        }
                        else
                        {
                            DateTime dataNascimento = prega.Projeto.Paciente.DataNascimento;
                            int idade = ProjetoNutri.Services.CalculoIdade.Calcular(dataNascimento);
                            var (pesoGordura, pesoMassaMagra) = _calculosDobras.CalcularPesoGorduraEMassaMagra(imc.Peso, PercentualGorduraFaulkner.PercentualGordura.Value);
                            double pesoResidual = _calculosDobras.CalcularPesoResidual(imc.Peso, imc.Altura, idade, projeto.Paciente.Sexo);
                            var classificacaoGordura = _calculosDobras.ClassificarGordura(PercentualGorduraFaulkner.PercentualGordura.Value, projeto.Paciente.Sexo);


                            ViewBag.ClassificacaoGorduraFaulkner = classificacaoGordura;
                            ViewBag.PercentualGorduraFaulkner = PercentualGorduraFaulkner.PercentualGordura.Value;
                            ViewBag.PercentualDensidadeCorporalFaulkner = PercentualGorduraFaulkner.DensidadeCorporal.Value;
                            ViewBag.SomaPregasFaulkner = PercentualGorduraFaulkner.SomaPrega.Value;
                            ViewBag.PesoResidualFaulkner = pesoResidual;
                            ViewBag.PesoGorduraFaulkner = pesoGordura;
                            ViewBag.PesoMassaMagraFaulkner = pesoMassaMagra;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log do erro (opcional)
                        Console.WriteLine("Erro ao calcular Faulkner: " + ex.Message);

                        // Exiba uma mensagem de erro genérica
                        ViewBag.MensagemErroFaulkner = "Ocorreu um erro ao calcular os valores usando o método Faulkner.";
                    }
                }
                else
                {
                    // Se o cálculo do Pollock3 falhar, exibe uma mensagem de erro
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
