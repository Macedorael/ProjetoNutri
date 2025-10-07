using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoNutri.Context;
using ProjetoNutri.Models;
using System.IO;
using System.Linq;

namespace ProjetoNutri.Controllers
{
    public class PdfController : Controller
    {
        private readonly ClienteContext _context;

        public PdfController(ClienteContext context)
        {
            _context = context;
        }

        public IActionResult GerarPdf(int idProjeto)
        {
            var refeicoes = _context.Refeicoes
                .Include(r => r.Refeicao_Alimentos)
                .ThenInclude(ra => ra.Alimento)
                .Where(r => r.IdProjeto == idProjeto)
                .OrderBy(r => r.Ordem)
                .ThenByDescending(r => r.DataCriacao)
                .ToList();

            var projeto = _context.Projetos
                .Include(p => p.Paciente)
                .FirstOrDefault(p => p.Id == idProjeto);

            if (projeto == null)
            {
                return NotFound("Projeto não encontrado.");
            }

            using (var ms = new MemoryStream())
            {
                // **Aumentei a margem superior para 80 para dar espaço para a imagem**
                Document document = new Document(PageSize.A4, 25, 25, 150, 30);
                var writer = PdfWriter.GetInstance(document, ms);

                // Marca d’água personalizada - carimbo no topo
                string caminhoImagem = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagens", "adesivos.png");
                writer.PageEvent = new CarimboTopo(caminhoImagem);

                document.Open();

                var titleFont = FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLACK);
                var title = new Paragraph
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 1f
                };
                title.Add(new Phrase($"Planejamento alimentar", titleFont));
                document.Add(title);

                var linha = new LineSeparator(1f, 100f, BaseColor.GRAY, Element.ALIGN_CENTER, -4);
                document.Add(new Chunk(linha));

                foreach (var refeicao in refeicoes)
                {
                    var refeicaoFont = FontFactory.GetFont("Arial", 14, Font.BOLD);
                    PdfPTable tituloTable = new PdfPTable(1);
                    tituloTable.WidthPercentage = 100;

                    PdfPCell tituloCell = new PdfPCell(new Phrase(refeicao.Nome, refeicaoFont))
                    {
                        BackgroundColor = new BaseColor(241, 241, 241),
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        PaddingTop = 10f,
                        PaddingBottom = 10f,
                        Border = Rectangle.NO_BORDER
                    };

                    tituloTable.AddCell(tituloCell);
                    tituloTable.SpacingBefore = 10f;
                    tituloTable.SpacingAfter = 10f;
                    document.Add(tituloTable);

                    if (refeicao.Refeicao_Alimentos != null && refeicao.Refeicao_Alimentos.Any())
                    {
                        PdfPTable table = new PdfPTable(2);
                        table.WidthPercentage = 100;
                        table.SetWidths(new float[] { 3, 3 });

                        BaseColor corLinha = new BaseColor(240, 240, 240);
                        var fonteMenor = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK);
                        foreach (var ra in refeicao.Refeicao_Alimentos)
                        {
                            PdfPCell cellAlimento = new PdfPCell(new Phrase(ra.Alimento.Nome, fonteMenor))
                            {
                                BorderColor = corLinha,
                                PaddingLeft = 10f,
                                MinimumHeight = 25f,
                                VerticalAlignment = Element.ALIGN_MIDDLE,
                                PaddingTop = 6f,
                                PaddingBottom = 6f
                            };
                            table.AddCell(cellAlimento);

                            string quantidadeGramas = (ra.Quantidade / 1000).ToString("F3") + " g";

                            PdfPCell cellQuantidade = new PdfPCell(new Phrase(quantidadeGramas, fonteMenor))
                            {
                                BorderColor = corLinha,
                                PaddingLeft = 10f,
                                MinimumHeight = 25f,
                                VerticalAlignment = Element.ALIGN_MIDDLE,
                                PaddingTop = 6f,
                                PaddingBottom = 6f
                            };
                            table.AddCell(cellQuantidade);
                        }
                        document.Add(table);
                    }
                    else
                    {
                        var noFood = new Paragraph("Nenhum alimento adicionado.")
                        {
                            SpacingAfter = 10f
                        };
                        document.Add(noFood);
                    }
                }

                document.Close();

                byte[] bytes = ms.ToArray();
                return File(bytes, "application/pdf", $"Refeicoes_Projeto_{projeto.Paciente.Nome}.pdf");
            }
        }

        // Carimbo no topo - ajustado para ficar dentro da margem superior aumentada
        public class CarimboTopo : PdfPageEventHelper
        {
            private readonly string _caminhoImagem;
            private Image _imagem;

            public CarimboTopo(string caminhoImagem)
            {
                _caminhoImagem = caminhoImagem;
                if (System.IO.File.Exists(_caminhoImagem))
                    _imagem = Image.GetInstance(_caminhoImagem);
            }

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                if (_imagem == null)
                    return;

                _imagem.ScaleToFit(300f, 120f);
                
                float x = (document.PageSize.Width - _imagem.ScaledWidth) / 2; // centralizado horizontalmente

                // Ajustei o Y para ficar dentro da margem superior maior (80)
                // document.PageSize.Height é o topo da página
                // document.TopMargin é 80 agora, então imagem fica pouco abaixo do topo da página
                float y = document.PageSize.Height - _imagem.ScaledHeight - 20f; 

                _imagem.SetAbsolutePosition(x, y);

                PdfContentByte canvas = writer.DirectContent;
                canvas.AddImage(_imagem);
            }
        }
    }
}
