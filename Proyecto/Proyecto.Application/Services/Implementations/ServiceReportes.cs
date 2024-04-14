using AutoMapper;
using Proyecto.Application.DTOs;
using Proyecto.Application.Services.Interfaces;
using Proyecto.Infraestructure.Models;
using Proyecto.Infraestructure.Repository.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Proyecto.Application.Services.Implementations;

public class ServiceReportes : IServiceReportes
{
    private readonly IRepositoryCliente _repositoryCliente;
    private readonly IRepositoryNFT _repositoryeNFT;
    private readonly IMapper _mapper;

    public ServiceReportes(IRepositoryCliente repositoryCliente, IRepositoryNFT repositoryeNFT, IMapper mapper)
    {
        _repositoryCliente = repositoryCliente;
        _repositoryeNFT = repositoryeNFT;
        _mapper = mapper;
    }

    public async Task<byte[]> ClientesReportPDF()
    {
        var collection = await _repositoryCliente.ListAsync();
        QuestPDF.Settings.License = LicenseType.Community;

        var pdfByteArray = Document.Create(document =>
        {
            document.Page(page =>
            {
                page.Size(PageSizes.Letter);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.Margin(30);

                page.Header().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().AlignLeft().Text("NFTSHOP S.A. ").Bold().FontSize(14).Bold();
                        col.Item().AlignLeft().Text($"Fecha: {DateTime.Now} ").FontSize(9);
                        col.Item().LineHorizontal(1f);
                    });

                });

                page.Content().PaddingVertical(10).Column(col1 =>
                {
                    col1.Item().AlignCenter().Text("Reporte de clientes").FontSize(14).Bold();
                    col1.Item().Text("");
                    col1.Item().LineHorizontal(0.5f);

                    col1.Item().Table(tabla =>
                    {
                        tabla.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2); // Código
                            columns.RelativeColumn(3); // Nombre
                            columns.RelativeColumn(3); // Primer Apellido
                            columns.RelativeColumn(3); // Segundo Apellido
                            columns.RelativeColumn(4); // Correo electrónico
                            columns.RelativeColumn(2); // País
                        });

                        tabla.Header(header =>
                        {
                            header.Cell().Background("#4666FF").Padding(2).AlignCenter().Text("Código").FontColor("#fff");
                            header.Cell().Background("#4666FF").Padding(2).AlignCenter().Text("Nombre").FontColor("#fff");
                            header.Cell().Background("#4666FF").Padding(2).AlignCenter().Text("Primer Apellido").FontColor("#fff");
                            header.Cell().Background("#4666FF").Padding(2).AlignCenter().Text("Segundo apellido").FontColor("#fff");
                            header.Cell().Background("#4666FF").Padding(2).AlignCenter().Text("Correo electrónico").FontColor("#fff");
                            header.Cell().Background("#4666FF").Padding(2).AlignCenter().Text("País").FontColor("#fff");
                        });

                        foreach (var item in collection)
                        {
                            // Column 1
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).AlignLeft().Text(item.IdCliente.ToString()).FontSize(10);

                            // Column 2
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).AlignCenter().Text(item.Nombre).FontSize(10);

                            // Column 3
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).AlignCenter().Text(item.Apellido1).FontSize(10);

                            // Column 4
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).AlignCenter().Text(item.Apellido2).FontSize(10);

                            // Column 5
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).AlignCenter().Text(item.Email).FontSize(10);

                            // Column 6
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).AlignCenter().Text(item.IdPais).FontSize(10);
                        }

                    });
                });

                page.Footer().AlignRight().Text(txt =>
                {
                    txt.Span("Página ").FontSize(10);
                    txt.CurrentPageNumber().FontSize(10);
                    txt.Span(" de ").FontSize(10);
                    txt.TotalPages().FontSize(10);
                });
            });
        }).GeneratePdf();

        return pdfByteArray;
    }

    public async Task<byte[]> NFTsReportPDF()
    {
        var collection = await _repositoryeNFT.ListAsync();
        QuestPDF.Settings.License = LicenseType.Community;

        var pdfByteArray = Document.Create(document =>
        {
            document.Page(page =>
            {
                page.Size(PageSizes.Letter);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.Margin(30);

                page.Header().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().AlignLeft().Text("NFTSHOP S.A. ").Bold().FontSize(14).Bold();
                        col.Item().AlignLeft().Text($"Fecha: {DateTime.Now} ").FontSize(9);
                        col.Item().LineHorizontal(1f);
                    });

                });

                page.Content().PaddingVertical(10).Column(col1 =>
                {
                    col1.Item().AlignCenter().Text("Reporte de NFTs").FontSize(14).Bold();
                    col1.Item().Text("");
                    col1.Item().LineHorizontal(0.5f);

                    col1.Item().Table(tabla =>
                    {
                        tabla.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();

                        });

                        tabla.Header(header =>
                        {
                            header.Cell().Background("#4666FF")
                            .Padding(2).AlignCenter().Text("NFT").FontColor("#fff");

                            header.Cell().Background("#4666FF")
                           .Padding(2).AlignCenter().Text("Foto").FontColor("#fff");

                            header.Cell().Background("#4666FF")
                           .Padding(2).AlignCenter().Text("Inventario").FontColor("#fff");

                            header.Cell().Background("#4666FF")
                           .Padding(2).AlignCenter().Text("Precio").FontColor("#fff");

                            header.Cell().Background("#4666FF")
                           .Padding(2).AlignCenter().Text("Total").FontColor("#fff");
                        });

                        foreach (var item in collection)
                        {
                            var total = item.Inventario * item.Precio;

                            // Column 1
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                            .Padding(2).Text(item.IdNft.ToString() + "\n\n" + item.Nombre).FontSize(10);

                            // Column 2
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                            .Padding(2).Image(item.Imagen).UseOriginalImage();

                            // Column 3
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                                .Padding(2).AlignCenter().Text(item.Inventario.ToString()).FontSize(10);
                            // Column 4
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                                   .Padding(2).AlignCenter().Text(item.Precio.ToString("###,###.00")).FontSize(10);
                            // Column 5
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                                 .Padding(2).AlignCenter().Text(total.ToString("###,###.00")).FontSize(10);
                        }
                    });

                    var granTotal = collection.Sum(p => p.Inventario * p.Precio);

                    col1.Item().AlignRight().Text("Total " + granTotal.ToString("###,###.00")).FontSize(12).Bold();
                });

                page.Footer()
                .AlignRight()
                .Text(txt =>
                {
                    txt.Span("Página ").FontSize(10);
                    txt.CurrentPageNumber().FontSize(10);
                    txt.Span(" de ").FontSize(10);
                    txt.TotalPages().FontSize(10);
                });
            });
        }).GeneratePdf();

        return pdfByteArray;
    }
}
