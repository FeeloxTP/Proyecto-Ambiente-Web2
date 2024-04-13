using AutoMapper;
using Proyecto.Application.DTOs;
using Proyecto.Application.Services.Interfaces;
using Proyecto.Infraestructure.Repository.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Application.Services.Implementations;

public class ServiceReportes : IServiceReportes
{
    private readonly IRepositoryNFT _repositoryNFT;
    private readonly IRepositoryFactura _repositoryFactura;
    private readonly IRepositoryCliente _repositoryCliente;
    private readonly IRepositoryMovimientosCompras _repositoryMovimientosCompras;
    private readonly IMapper _mapper;

    public ServiceReportes(IRepositoryNFT repositoryNFT,
                          IRepositoryFactura repositoryFactura,
                          IRepositoryCliente repositoryCliente,
                          IRepositoryMovimientosCompras repositoryMovimientosCompras,
                          IMapper mapper)
    {
        _repositoryNFT = repositoryNFT;
        _repositoryFactura = repositoryFactura;
        _repositoryCliente = repositoryCliente;
        _repositoryMovimientosCompras = repositoryMovimientosCompras;
        _mapper = mapper;

    }

    public async Task<byte[]> ClienteReportByNFT(string description)
    {
        // Get Data
        NFTDTO nft = new NFTDTO();
          nft =  await _repositoryNFT.FindByNameAsync(description);  
        // Get Data  of cliente
        var collectionMovimientos = await _repositoryMovimientosCompras.FindByIdNFT();  

        var cliente 

        //leemos el viewbag con los clientes y despues hacemos un diccionario con el id y nombre
        // que es mucho mejor que usar un for para recorrer cliente por cliente
        var clientesList = ((IEnumerable<ClienteDTO>)collection).ToList();
        var clienteDict = clientesList.ToDictionary(c => c.IdCliente, c => c.Nombre);

        //preguntamos si el diccionario lo tiene
        if (clienteDict.ContainsKey(item.IdCliente))
        {
            @clienteDict[item.IdCliente]
        }

        // License config ******  IMPORTANT ******
        QuestPDF.Settings.License = LicenseType.Community;

        // return ByteArrays
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
                        col.Item().AlignLeft().Text("Galeria del Arte NFT S.A. ").Bold().FontSize(14).Bold();
                        col.Item().AlignLeft().Text($"Fecha: {DateTime.Now} ").FontSize(9);
                        col.Item().LineHorizontal(1f);
                    });

                });


                page.Content().PaddingVertical(10).Column(col1 =>
                {
                    col1.Item().AlignCenter().Text("Reporte de Cliente").FontSize(14).Bold();
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
                            .Padding(2).AlignCenter().Text("Producto").FontColor("#fff");

                            header.Cell().Background("#4666FF")
                           .Padding(2).AlignCenter().Text("Foto").FontColor("#fff");

                            header.Cell().Background("#4666FF")
                           .Padding(2).AlignCenter().Text("Cantidad").FontColor("#fff");

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
                            .Padding(2).Text(item.IdNft.ToString() + "-" + item.Nombre.PadRight(50, '.').Substring(0, 15)).FontSize(10);

                            // Column 2
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                            .Padding(2).Image(item.Imagen).UseOriginalImage();

                            // Column 3
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                                .Padding(2).AlignRight().Text(item.Inventario.ToString()).FontSize(10);
                            // Column 4
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                                   .Padding(2).AlignRight().Text(item.Precio.ToString("###,###.00")).FontSize(10);
                            // Column 5
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                                 .Padding(2).AlignRight().Text(total.ToString("###,###.00")).FontSize(10);
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

        //File.WriteAllBytes(@"C:\temp\ProductReport.pdf", pdfByteArray);
        return pdfByteArray;
    }

    public async Task<byte[]> ProductReport()
    {
        // Get Data
        var collection = await _repositoryNFT.ListAsync();

        // License config ******  IMPORTANT ******
        QuestPDF.Settings.License = LicenseType.Community;

        // return ByteArrays
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
                        col.Item().AlignLeft().Text("Electronics S.A. ").Bold().FontSize(14).Bold();
                        col.Item().AlignLeft().Text($"Fecha: {DateTime.Now} ").FontSize(9);
                        col.Item().LineHorizontal(1f);
                    });

                });


                page.Content().PaddingVertical(10).Column(col1 =>
                {
                    col1.Item().AlignCenter().Text("Reporte de Productos").FontSize(14).Bold();
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
                            .Padding(2).AlignCenter().Text("Producto").FontColor("#fff");

                            header.Cell().Background("#4666FF")
                           .Padding(2).AlignCenter().Text("Foto").FontColor("#fff");

                            header.Cell().Background("#4666FF")
                           .Padding(2).AlignCenter().Text("Cantidad").FontColor("#fff");

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
                            .Padding(2).Text(item.IdNft.ToString() + "-" + item.Nombre.PadRight(50, '.').Substring(0, 15)).FontSize(10);

                            // Column 2
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                            .Padding(2).Image(item.Imagen).UseOriginalImage();

                            // Column 3
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                                .Padding(2).AlignRight().Text(item.Inventario.ToString()).FontSize(10);
                            // Column 4
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                                   .Padding(2).AlignRight().Text(item.Precio.ToString("###,###.00")).FontSize(10);
                            // Column 5
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                                 .Padding(2).AlignRight().Text(total.ToString("###,###.00")).FontSize(10);
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

        //File.WriteAllBytes(@"C:\temp\ProductReport.pdf", pdfByteArray);
        return pdfByteArray;

    }


    public async Task<ICollection<FacturaDTO>> BillsByClientIdAsync(Guid id)
    {
        var collection = await _repositoryFactura.BillsByClientIdAsync(id);
        var collectionMapped = _mapper.Map<ICollection<FacturaDTO>>(collection);
        return collectionMapped;
    }
}
