using AutoMapper;
using Proyecto.Application.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Proyecto.Application.Services.Interfaces;
using Proyecto.Infraestructure.Models;
using Proyecto.Infraestructure.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Reflection.Metadata;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Infrastructure;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using Proyecto.Application.DTOs;


namespace Proyecto.Application.Services.Implementations;

public class ServiceFactura : IServiceFactura
{
    private readonly IRepositoryFactura _repositoryFactura;
    private readonly IRepositoryCliente _repositoryCliente;
    private readonly IRepositoryNFT _repositoryNFT;
    private readonly IMapper _mapper;
    private readonly IOptions<AppConfig> _options;
    private readonly ILogger<ServiceFactura> _logger;

    public ServiceFactura(IRepositoryFactura repositoryFactura,
                          IRepositoryCliente repositoryCliente,
                          IRepositoryNFT repositoryNFT,
                          IMapper mapper,
                          IOptions<AppConfig> options,
                          ILogger<ServiceFactura> logger)
    {
        _repositoryFactura = repositoryFactura;
        _repositoryCliente = repositoryCliente;
        _repositoryNFT = repositoryNFT;
        _mapper = mapper;
        _options = options;
        _logger = logger;
    }

    public async Task<int> AddAsync(FacturaDTO dto)
    {
        // Validate Stock availability
        foreach (var item in dto.ListFacturaDetalle)
        {
            var producto = await _repositoryNFT.FindByIdAsync(item.IdNFT);

            if (producto.Inventario - item.Cantidad < 0)
            {
                throw new BadHttpRequestException($"No hay stock para el producto {producto.Nombre}, cantidad en stock {producto.Inventario} ");
            }
        }

        var @object = _mapper.Map<FacturaEncabezado>(dto);
        // Find Customer
        var cliente = await _repositoryCliente.FindByIdAsync(dto.IdCliente);
        // Save Bill
        dto.IdFactura = await _repositoryFactura.AddAsync(@object);
        // Create PDF Array
        var pdfBytes = await CreatePDFBill(dto.IdFactura);

        // Directory exist?        
        if (!Directory.Exists("c:\temp"))
            Directory.CreateDirectory(@"C:\temp");
        // Save it locally
        await File.WriteAllBytesAsync(@"c:\temp\" + dto.IdFactura.ToString().Trim() + ".pdf", pdfBytes);

        // Send email with PDF as Attachment
        await SendEmail(cliente!.Email!, pdfBytes);
        return dto.IdFactura;
    }
    public async Task<int> GetNextReceiptNumber()
    {
        int nextReceipt = await _repositoryFactura.GetNextReceiptNumber();
        return nextReceipt + 1;
    }

    /// <summary>
    /// Send email 
    /// </summary>
    /// <param name="email"></param>
    private async Task<bool> SendEmail(string email, byte[] pdf)
    {
        if (string.IsNullOrEmpty(_options.Value.SmtpConfiguration.Server) || string.IsNullOrEmpty(_options.Value.SmtpConfiguration.PortNumber.ToString()))
        {
            _logger.LogError($"No se encuentra configurado ningun valor para SMPT en {MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}");
            return false;
        }
        if (string.IsNullOrEmpty(_options.Value.SmtpConfiguration.UserName) || string.IsNullOrEmpty(_options.Value.SmtpConfiguration.FromName))
        {
            _logger.LogError($"No se encuentra configurado UserName o FromName en appSettings.json (Dev | Prod) {MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}");
            return false;
        }
        var mailMessage = new MailMessage(
                new MailAddress(_options.Value.SmtpConfiguration.UserName, _options.Value.SmtpConfiguration.FromName),
                new MailAddress(email))
        {
            Subject = "Factura Electrónica para " + email,
            Body = "Adjunto Factura Electronica Galeria del Arte NFT S.A.",
            IsBodyHtml = true
        };
        // attach it as a File
        //Attachment attachment = new Attachment(@"c:\\temp\\factura.pdf");
        // Bytes 
        Attachment attachment = new Attachment(new MemoryStream(pdf), "factura.pdf");
        mailMessage.Attachments.Add(attachment);

        using var smtpClient = new SmtpClient(_options.Value.SmtpConfiguration.Server,
                                              _options.Value.SmtpConfiguration.PortNumber)
        {
            Credentials = new NetworkCredential(_options.Value.SmtpConfiguration.UserName,
                                                _options.Value.SmtpConfiguration.Password),
            EnableSsl = _options.Value.SmtpConfiguration.EnableSsl,
        };
        await smtpClient.SendMailAsync(mailMessage);
        return true;

    }

    private async Task<byte[]> CreatePDFBill(int id)
    {
        var factura = await _repositoryFactura.FindByIdAsync(id);

        // License config ******  IMPORTANT ******
        QuestPDF.Settings.License = LicenseType.Community;

        var pdfByteArray = QuestPDF.Fluent.Document.Create(document =>
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
                    col1.Item().AlignLeft().Text($"Factura : {factura.IdFactura}").FontSize(12);
                    col1.Item().AlignLeft().Text($"Cliente : {factura.IdCliente.ToString()}- {factura.IdClienteNavigation.Nombre} {factura.IdClienteNavigation.Apellido1}").FontSize(12);
                    col1.Item().AlignLeft().Text($"Fecha   : {factura.FechaFacturacion}").FontSize(12);
                    col1.Item().LineHorizontal(0.5f);
                    col1.Item().Text("");
                    col1.Item().Table(tabla =>
                    {
                        tabla.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(10);
                            columns.RelativeColumn(5);
                            columns.RelativeColumn(10);
                            //columns.RelativeColumn(10);
                            columns.RelativeColumn(10);
                        });

                        tabla.Header(header =>
                        {
                            header.Cell().Background("#4666FF")
                            .Padding(2).AlignCenter().Text("No").FontColor("#fff");

                            header.Cell().Background("#4666FF")
                           .Padding(2).AlignCenter().Text("Producto").FontColor("#fff");

                            header.Cell().Background("#4666FF")
                           .Padding(2).AlignCenter().Text("Cantidad").FontColor("#fff");

                            header.Cell().Background("#4666FF")
                           .Padding(2).AlignCenter().Text("Precio").FontColor("#fff");

                            // header.Cell().Background("#4666FF")
                            //.Padding(2).AlignCenter().Text("Impuesto").FontColor("#fff");

                            header.Cell().Background("#4666FF")
                           .Padding(2).AlignCenter().Text("Total").FontColor("#fff");

                        });


                        foreach (var item in factura.FacturaDetalle)
                        {
                            // Column 1
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                            .Padding(2).AlignCenter().Text(item!.Secuencia.ToString()).FontSize(10);

                            // Column 2
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                           .Padding(2).AlignCenter().Text(item.IdNftNavigation.Nombre.ToString()).FontSize(10);

                            // Column 3
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                           .Padding(2).AlignCenter().Text(item.Cantidad.ToString()).FontSize(10);

                            // Column 4
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                           .Padding(2).AlignCenter().Text(item.Precio.ToString("###,###.00")).FontSize(10);

                            // // Column 5
                            // tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                            //.Padding(2).AlignRight().Text(item.Impuesto.ToString("###,###.00")).FontSize(10);

                            // Column 6
                            var totalLinea = (item.Cantidad * item.Precio);
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                           .Padding(2).AlignCenter().Text(totalLinea.ToString("###,###.00")).FontSize(10);
                        }

                    });


                    var granTotal = factura.FacturaDetalle.Sum(p => (p.Cantidad * p.Precio));

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

        return pdfByteArray!;
    }

    public async Task<ICollection<FacturaDTO>> ListAsync()
    {
        // Get data from Repository
        var list = await _repositoryFactura.ListAsync();
        // Map List<Entity> to ICollection<XXXXXXDTO>
        var collection = _mapper.Map<ICollection<FacturaDTO>>(list);
        // Return Data
        return collection;
    }
    public async Task UpdateAsync(int id, FacturaDTO dto)
    {
        var objectMapped = _mapper.Map<FacturaEncabezado>(dto);
        await _repositoryFactura.UpdateAsync(id, objectMapped);
    }

    public async Task<FacturaDTO> FindByIdAsync(int id)
    {
        var @object = await _repositoryFactura.FindByIdAsync(id);
        var objectMapped = _mapper.Map<FacturaDTO>(@object);
        return objectMapped;
    }

    public async Task<ICollection<FacturaDTO>> FindByReporteXFechas(DateTime fechaInicial, DateTime fechaFinal)
    {
        var list = await _repositoryFactura.FindByReporteXFechas(fechaInicial, fechaFinal);
        var collection = _mapper.Map<ICollection<FacturaDTO>>(list);
        return collection;
    }

    public async Task<ICollection<FacturaDTO>> FindByVentasByFechasAsync(DateTime fechaInicial, DateTime fechaFinal)
    {
        var list = await _repositoryFactura.FindByVentasByFechasAsync(fechaInicial, fechaFinal);
        var collection = _mapper.Map<ICollection<FacturaDTO>>(list);
        return collection;
    }

    public async Task<ICollection<FacturaDTO>> FindByClientNameAsync(string name)
    {
        var list = await _repositoryFactura.FindByClientNameAsync(name);
        var collection = _mapper.Map<ICollection<FacturaDTO>>(list);
        return collection;
    }


}
