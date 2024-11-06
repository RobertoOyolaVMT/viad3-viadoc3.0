using Microsoft.EntityFrameworkCore;
using ReportesViaDocNetCore.EntidadesReporte;
using ReportesViaDocNetCore.Interfaces;
using ReportesViaDocNetCore.Models;

namespace ReportesViaDocNetCore.Services
{
    public class CatalogoServices : ICatalogos
    {
        private readonly FacturacionElectronicaQaContext _context;

        public CatalogoServices(FacturacionElectronicaQaContext context)
        {
            this._context = context;
        }

        public async Task<DatosDocumento> DatosDocuemntosFactura(string claveAcceso)
        {
            var res = new DatosDocumento();

            try
            {
                res = await (from comp in _context.Facturas1
                             where comp.TxClaveAcceso == claveAcceso
                             select new DatosDocumento
                             {
                                 NumDoc = comp.TxEstablecimiento + "-" + comp.TxPuntoEmision + "-" + comp.TxSecuencial,
                                 IdCompania = comp.CiCompania
                             }).FirstOrDefaultAsync();

                return res;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<DatosDocumento> DatosDocuemntosCompRetecion(string claveAcceso)
        {
            var res = new DatosDocumento();

            try
            {
                res = await (from comp in _context.CompRetencions
                             where comp.TxClaveAcceso == claveAcceso
                             select new DatosDocumento
                             {
                                 NumDoc = comp.TxEstablecimiento + "-" + comp.TxPuntoEmision + "-" + comp.TxSecuencial,
                                 IdCompania = comp.CiCompania
                             }).FirstOrDefaultAsync();

                return res;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<DatosDocumento> DatosDocuemntosNotaCredito(string claveAcceso)
        {
            var res = new DatosDocumento();

            try
            {
                res = await (from comp in _context.NotaCreditos
                             where comp.TxClaveAcceso == claveAcceso
                             select new DatosDocumento
                             {
                                 NumDoc = comp.TxEstablecimiento + "-" + comp.TxPuntoEmision + "-" + comp.TxSecuencial,
                                 IdCompania = comp.CiCompania
                             }).FirstOrDefaultAsync();

                return res;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<DatosDocumento> DatosDocuemntosNotaDebito(string claveAcceso)
        {
            var res = new DatosDocumento();

            try
            {
                res = await (from comp in _context.NotaDebitos
                             where comp.TxClaveAcceso == claveAcceso
                             select new DatosDocumento
                             {
                                 NumDoc = comp.TxEstablecimiento + "-" + comp.TxPuntoEmision + "-" + comp.TxSecuencial,
                                 IdCompania = comp.CiCompania
                             }).FirstOrDefaultAsync();

                return res;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<DatosDocumento> DatosDocuemntosLiquidacion(string claveAcceso)
        {
            var res = new DatosDocumento();

            try
            {
                res = await (from comp in _context.Liquidacions
                             where comp.TxClaveAcceso == claveAcceso
                             select new DatosDocumento
                             {
                                 NumDoc = comp.TxEstablecimiento + "-" + comp.TxPuntoEmision + "-" + comp.TxSecuencial,
                                 IdCompania = comp.CiCompania
                             }).FirstOrDefaultAsync();

                return res;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<Companium> Compania(int idCompania)
        {
            var res = new Companium();
            try
            {
                res = await (from com in _context.Compania
                             where com.CiCompania == idCompania
                             select com).FirstOrDefaultAsync();

                res.Sucursals = await (from com in _context.Sucursals
                                       where com.CiCompania == idCompania
                                       select com).ToListAsync();

                return res;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<ConfiguracionReporte>> Configuraciones(int idCompania)
        {
            var res = new List<ConfiguracionReporte>();
            try
            {
                res = await (from c in _context.Configuracions
                             join dc in _context.ConfiguracionCompania on c.IdConfiguracion equals dc.IdConfiguracion
                             join com in _context.Compania on dc.CiCompania equals com.CiCompania
                             where dc.Estado == "A" && com.CiCompania == idCompania
                             select new ConfiguracionReporte
                             {
                                 IdConfiguracion = c.IdConfiguracion,
                                 CodigoReferencia = c.CodReferencia,
                                 Descripcion = c.Descripcion,
                                 IdCompania = com.CiCompania,
                                 RucCompania = com.TxRuc,
                                 Configuracion1 = dc.Param1!,
                                 Configuracion2 = dc.Param2!,
                                 Configuracion3 = dc.Param3!,
                                 Configuracion4 = dc.Param4!,
                                 Configuracion5 = dc.Param5!
                             }).ToListAsync();

                return res;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<CatalogoReporte>> CatalogoReportes()
        {
            var res = new List<CatalogoReporte>();
            try
            {
                res = await (from c in _context.CabeceraCatalogos
                             join dt in _context.DetalleCatalogos on c.CiCatalogo equals dt.CiCatalogo
                             where new[] { 1, 4, 5, 8, 9, 13, 14, 15 }.Contains(c.CiCatalogo) && dt.Ciestado == "A"
                             select new CatalogoReporte
                             {
                                 CodigoReferencia = c.CodReferencia!,
                                 Descripcion = c.Descripcion!,
                                 Codigo = dt.Param1!,
                                 Valor = dt.Param2!
                             }).ToListAsync();

                return res;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<DetalleCatalogo>> DetalleCatalogo()
        {
            var res = new List<DetalleCatalogo>();
            try
            {
                res = await _context.DetalleCatalogos.ToListAsync();

                return res;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
