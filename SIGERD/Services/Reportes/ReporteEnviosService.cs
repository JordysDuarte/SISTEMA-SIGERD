using SIGERD.DTOs.Reportes;
using SIGERD.Interfaces.IRespositories.Reportes;
using SIGERD.Interfaces.IServices.Reportes;
using SIGERD.Models.Envios;
using SIGERD.ViewModels.Reportes.Envios;
using ClosedXML.Excel;

namespace SIGERD.Services.Reportes
{
    public class ReporteEnviosService : IReporteEnviosService
    {
        private readonly IReporteEnviosRepository _reporteEnviosRepository;

        public ReporteEnviosService(IReporteEnviosRepository reporteEnviosRepository)
        {
            _reporteEnviosRepository = reporteEnviosRepository;
        }

        public async Task<ReporteEnviosIndexViewModel> ObtenerReporteAsync(ReporteEnviosFiltroDto filtro)
        {
            ValidarFiltro(filtro);

            var envios = (await _reporteEnviosRepository.ObtenerEnviosAsync(filtro)).ToList();

            var resultados = envios
                .Select(ToResultadoViewModel)
                .ToList();

            return new ReporteEnviosIndexViewModel
            {
                Resultados = resultados,
                TotalEnvios = resultados.Count,
                TotalPendientes = resultados.Count(e => EsEstado(e.EstadoEnvio, "Pendiente")),
                TotalEnTransito = resultados.Count(e =>
                    EsEstado(e.EstadoEnvio, "En tránsito") ||
                    EsEstado(e.EstadoEnvio, "En transito")),
                TotalRecibidos = resultados.Count(e => EsEstado(e.EstadoEnvio, "Recibido"))
            };
        }

        private static void ValidarFiltro(ReporteEnviosFiltroDto filtro)
        {
            if (filtro is null)
            {
                throw new InvalidOperationException("La información del filtro no es válida.");
            }

            if (filtro.FechaInicio.HasValue &&
                filtro.FechaFin.HasValue &&
                filtro.FechaInicio.Value.Date > filtro.FechaFin.Value.Date)
            {
                throw new InvalidOperationException("La fecha inicio no puede ser mayor que la fecha fin.");
            }

            if (!filtro.EsSuperAdministrador && filtro.IdDelegacionUsuario <= 0)
            {
                throw new InvalidOperationException("No fue posible identificar la delegación del usuario.");
            }
        }

        private static ReporteEnviosResultadoViewModel ToResultadoViewModel(Envio envio)
        {
            return new ReporteEnviosResultadoViewModel
            {
                IdEnvio = envio.idEnvio,
                CodigoEnvio = envio.codigoEnvio ?? string.Empty,
                FechaEnvio = envio.fechaEnvio,
                DelegacionOrigen = envio.DelegacionOrigen?.nombreDelegacion ?? "Sin origen",
                DelegacionDestino = envio.DelegacionDestino?.nombreDelegacion ?? "Sin destino",
                UsuarioEnvio = envio.Usuario?.nombreCompleto ?? "Sin usuario",
                EstadoEnvio = envio.EstadoEnvio?.nombreEstadoEnvio ?? "Sin estado",
                FechaDespacho = envio.fechaDespacho,
                UsuarioDespacho = envio.UsuarioDespacho?.nombreCompleto,
                FechaRecepcion = envio.Recepcion?.fechaRecepcion,
                UsuarioRecepcion = envio.Recepcion?.Usuario?.nombreCompleto,
                TotalArticulos = envio.DetallesEnvio?.Count ?? 0
            };
        }

        private static bool EsEstado(string estadoActual, string estadoEsperado)
        {
            return estadoActual.Trim().Equals(
                estadoEsperado,
                StringComparison.OrdinalIgnoreCase
            );
        }

        public async Task<byte[]> ExportarExcelAsync(ReporteEnviosFiltroDto filtro)
        {
            var reporte = await ObtenerReporteAsync(filtro);

            using var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Reporte de envíos");

            worksheet.Cell(1, 1).Value = "Sistema de Trazabilidad de Envíos PUCPA";
            worksheet.Cell(2, 1).Value = "Reporte general de envíos";
            worksheet.Cell(3, 1).Value = $"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}";

            worksheet.Range(1, 1, 1, 12).Merge();
            worksheet.Range(2, 1, 2, 12).Merge();
            worksheet.Range(3, 1, 3, 12).Merge();

            worksheet.Cell(1, 1).Style.Font.Bold = true;
            worksheet.Cell(1, 1).Style.Font.FontSize = 16;
            worksheet.Cell(2, 1).Style.Font.Bold = true;
            worksheet.Cell(2, 1).Style.Font.FontSize = 13;
            worksheet.Cell(3, 1).Style.Font.Italic = true;

            worksheet.Range(1, 1, 3, 12).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            worksheet.Cell(5, 1).Value = "Resumen";
            worksheet.Cell(5, 1).Style.Font.Bold = true;

            worksheet.Cell(6, 1).Value = "Total envíos";
            worksheet.Cell(6, 2).Value = reporte.TotalEnvios;

            worksheet.Cell(7, 1).Value = "Pendientes";
            worksheet.Cell(7, 2).Value = reporte.TotalPendientes;

            worksheet.Cell(8, 1).Value = "En tránsito";
            worksheet.Cell(8, 2).Value = reporte.TotalEnTransito;

            worksheet.Cell(9, 1).Value = "Recibidos";
            worksheet.Cell(9, 2).Value = reporte.TotalRecibidos;

            worksheet.Range(6, 1, 9, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Range(6, 1, 9, 2).Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            int headerRow = 11;

            worksheet.Cell(headerRow, 1).Value = "Código";
            worksheet.Cell(headerRow, 2).Value = "Fecha envío";
            worksheet.Cell(headerRow, 3).Value = "Origen";
            worksheet.Cell(headerRow, 4).Value = "Destino";
            worksheet.Cell(headerRow, 5).Value = "Usuario registro";
            worksheet.Cell(headerRow, 6).Value = "Estado";
            worksheet.Cell(headerRow, 7).Value = "Fecha despacho";
            worksheet.Cell(headerRow, 8).Value = "Usuario despacho";
            worksheet.Cell(headerRow, 9).Value = "Fecha recepción";
            worksheet.Cell(headerRow, 10).Value = "Usuario recepción";
            worksheet.Cell(headerRow, 11).Value = "Total artículos";
            worksheet.Cell(headerRow, 12).Value = "ID envío";

            var headerRange = worksheet.Range(headerRow, 1, headerRow, 12);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
            headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            int row = headerRow + 1;

            foreach (var envio in reporte.Resultados)
            {
                worksheet.Cell(row, 1).Value = envio.CodigoEnvio;
                worksheet.Cell(row, 2).Value = envio.FechaEnvio;
                worksheet.Cell(row, 3).Value = envio.DelegacionOrigen;
                worksheet.Cell(row, 4).Value = envio.DelegacionDestino;
                worksheet.Cell(row, 5).Value = envio.UsuarioEnvio;
                worksheet.Cell(row, 6).Value = envio.EstadoEnvio;
                worksheet.Cell(row, 7).Value = envio.FechaDespacho;
                worksheet.Cell(row, 8).Value = string.IsNullOrWhiteSpace(envio.UsuarioDespacho)
                    ? "Sin despacho"
                    : envio.UsuarioDespacho;
                worksheet.Cell(row, 9).Value = envio.FechaRecepcion;
                worksheet.Cell(row, 10).Value = string.IsNullOrWhiteSpace(envio.UsuarioRecepcion)
                    ? "Sin recepción"
                    : envio.UsuarioRecepcion;
                worksheet.Cell(row, 11).Value = envio.TotalArticulos;
                worksheet.Cell(row, 12).Value = envio.IdEnvio;

                row++;
            }

            if (reporte.Resultados.Any())
            {
                var dataRange = worksheet.Range(headerRow, 1, row - 1, 12);
                dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                worksheet.Range(headerRow + 1, 2, row - 1, 2)
                    .Style.DateFormat.Format = "dd/MM/yyyy";

                worksheet.Range(headerRow + 1, 7, row - 1, 7)
                    .Style.DateFormat.Format = "dd/MM/yyyy HH:mm";

                worksheet.Range(headerRow + 1, 9, row - 1, 9)
                    .Style.DateFormat.Format = "dd/MM/yyyy HH:mm";
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }
    }
}