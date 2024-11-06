using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaDoc.AccesoDatos;
using ViaDoc.Configuraciones;
using ViaDocFirma.LogicaNegocios;
using ViaDoc.Utilitarios;
using ViaDoc.Logs;

namespace FirmaViadoc.WinServ
{
    public class MetodosWinServ
    {
        DocumentoAD _metodosDocumentos = new DocumentoAD();
        private int codigoRetorno = 0;
        private string descripcionRetorno = string.Empty;
        private int cantidadDocumento = 0;
        private int reprocesoFirma = 0;
        ValidacionCertificado _procesoValidacion = new ValidacionCertificado();
        ConexionBDMongo objLogs = new ConexionBDMongo();

        public void ProbadorServicio()
        {
            DataSet dsTipoDocumento = _metodosDocumentos.ObtenerTipoDocumentos("1", CatalogoViaDoc.DocumentoFactura, "", 0, ref codigoRetorno, ref descripcionRetorno);

            if (codigoRetorno.Equals(0))
            {
                DataTable dtTipoDocumento = dsTipoDocumento.Tables[0];
                DataTable dtContadorDocumentos = dsTipoDocumento.Tables[1];

                foreach (DataRow listaDocumentos in dtTipoDocumento.Rows)
                {

                    if (listaDocumentos["tipoDocumento"].ToString() == CatalogoViaDoc.DocumentoCompRetencion)
                    {
                        int contador = int.Parse(dtContadorDocumentos.Rows[0]["ContCompRetencion"].ToString());
                        if (contador > 0)
                        {
                            GenerarFirmaElectronicaCompRetencion();
                        }
                    }
                    if (listaDocumentos["tipoDocumento"].ToString() == CatalogoViaDoc.DocumentoGuiaRemision)
                    {
                        int contador = int.Parse(dtContadorDocumentos.Rows[0]["ContGuiaRemision"].ToString());
                        if (contador > 0)
                        {
                            GenerarFirmaElectronicaGuiaRemision();
                        }
                    }
                }
            }
        }

        public void GenerarFirmaElectronicaFactura()
        {
            DataSet dsTipoDocumento = _metodosDocumentos.ObtenerTipoDocumentos("2", CatalogoViaDoc.DocumentoFactura, "", 0, ref codigoRetorno, ref descripcionRetorno);
            if (codigoRetorno.Equals(0))
            {
                if (dsTipoDocumento.Tables.Count > 0)
                {
                    string ciTipoDocumento = dsTipoDocumento.Tables[0].Rows[0]["tipoDocumento"].ToString();
                    cantidadDocumento = int.Parse(dsTipoDocumento.Tables[0].Rows[0]["cantidadFirma"].ToString());
                    _procesoValidacion.ValidarGenerarFirmaDocumentos("", ciTipoDocumento, cantidadDocumento, ref codigoRetorno, ref descripcionRetorno);
                }
                else
                {
                    //objLogs.creaLogMongo("", "0000", "", "", "GenerarFirmaElectronicaFactura", "No hay Facturas para firmar", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), "FirmaViadoc.WinServ");
                }
            }
            else
            {
                //objLogs.creaLogMongo("", "0000", "", "", "GenerarFirmaElectronicaFactura", descripcionRetorno, DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), "FirmaViadoc.WinServ");
            }
        }

        public void GenerarFirmaElectronicaCompRetencion()
        {
            DataSet dsTipoDocumento = _metodosDocumentos.ObtenerTipoDocumentos("2", CatalogoViaDoc.DocumentoCompRetencion, "", 0, ref codigoRetorno, ref descripcionRetorno);

            if (codigoRetorno.Equals(0))
            {

                if (dsTipoDocumento.Tables.Count > 0)
                {
                    string ciTipoDocumento = dsTipoDocumento.Tables[0].Rows[0]["tipoDocumento"].ToString();
                    cantidadDocumento = int.Parse(dsTipoDocumento.Tables[0].Rows[0]["cantidadFirma"].ToString());
                    _procesoValidacion.ValidarGenerarFirmaDocumentos("", ciTipoDocumento, cantidadDocumento, ref codigoRetorno, ref descripcionRetorno);
                }
                else
                {
                    //objLogs.creaLogMongo("", "0000", "", "", "GenerarFirmaElectronicaCompRetencion", "No hay Comprobantes de Retención para firmar", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), "FirmaViadoc.WinServ");
                }
            }
            else
            {
                //objLogs.creaLogMongo("", "0000", "", "", "GenerarFirmaElectronicaCompRetencion", descripcionRetorno, DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), "FirmaViadoc.WinServ");
            }
        }

        public void GenerarFirmaElectronicaNotaCredito()
        {
            DataSet dsTipoDocumento = _metodosDocumentos.ObtenerTipoDocumentos("2", CatalogoViaDoc.DocumentoNotaCredito, "", 0, ref codigoRetorno, ref descripcionRetorno);

            if (codigoRetorno.Equals(0))
            {
                if (dsTipoDocumento.Tables.Count > 0)
                {
                    string ciTipoDocumento = dsTipoDocumento.Tables[0].Rows[0]["tipoDocumento"].ToString();
                    cantidadDocumento = int.Parse(dsTipoDocumento.Tables[0].Rows[0]["cantidadFirma"].ToString());
                    _procesoValidacion.ValidarGenerarFirmaDocumentos("", ciTipoDocumento, cantidadDocumento, ref codigoRetorno, ref descripcionRetorno);
                }
                else
                {
                    //objLogs.creaLogMongo("", "0000", "", "", "GenerarFirmaElectronicaNotaCredito", "No hay Notas de Credito para firmar", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), "FirmaViadoc.WinServ");
                }
            }
            else
            {
                //objLogs.creaLogMongo("", "0000", "", "", "GenerarFirmaElectronicaNotaCredito", descripcionRetorno, DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), "FirmaViadoc.WinServ");
            }
        }

        public void GenerarFirmaElectronicaNotaDebito()
        {
            DataSet dsTipoDocumento = _metodosDocumentos.ObtenerTipoDocumentos("2", CatalogoViaDoc.DocumentoNotaDebito, "", 0, ref codigoRetorno, ref descripcionRetorno);
            if (codigoRetorno.Equals(0))
            {
                if (dsTipoDocumento.Tables.Count > 0)
                {
                    string ciTipoDocumento = dsTipoDocumento.Tables[0].Rows[0]["tipoDocumento"].ToString();
                    cantidadDocumento = int.Parse(dsTipoDocumento.Tables[0].Rows[0]["cantidadFirma"].ToString());
                    _procesoValidacion.ValidarGenerarFirmaDocumentos("", ciTipoDocumento, cantidadDocumento, ref codigoRetorno, ref descripcionRetorno);
                }
                else
                {
                    //objLogs.creaLogMongo("", "0000", "", "", "GenerarFirmaElectronicaNotaDebito", "No hay Notas de Debito para firmar", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), "FirmaViadoc.WinServ");
                }
            }
            else
            {
                //objLogs.creaLogMongo("", "0000", "", "", "GenerarFirmaElectronicaNotaDebito", descripcionRetorno, DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), "FirmaViadoc.WinServ");
            }
        }

        public void GenerarFirmaElectronicaGuiaRemision()
        {
            DataSet dsTipoDocumento = _metodosDocumentos.ObtenerTipoDocumentos("2", CatalogoViaDoc.DocumentoGuiaRemision, "", 0, ref codigoRetorno, ref descripcionRetorno);

            if (codigoRetorno.Equals(0))
            {
                if (dsTipoDocumento.Tables.Count > 0)
                {
                    string ciTipoDocumento = dsTipoDocumento.Tables[0].Rows[0]["tipoDocumento"].ToString();
                    cantidadDocumento = int.Parse(dsTipoDocumento.Tables[0].Rows[0]["cantidadFirma"].ToString());
                    _procesoValidacion.ValidarGenerarFirmaDocumentos("", ciTipoDocumento, cantidadDocumento, ref codigoRetorno, ref descripcionRetorno);
                }
                else
                {
                    //objLogs.creaLogMongo("", "0000", "", "", "GenerarFirmaElectronicaGuiaRemision", "No hay Guia de Remisión para firmar", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), "FirmaViadoc.WinServ");
                }
            }
            else
            {
                //objLogs.creaLogMongo("", "0000", "", "", "GenerarFirmaElectronicaGuiaRemision", descripcionRetorno, DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), "FirmaViadoc.WinServ");
            }
        }

        public void GenerarFirmaElectronicaLiquidacion()
        {
            DataSet dsTipoDocumento = _metodosDocumentos.ObtenerTipoDocumentos("2", CatalogoViaDoc.DocumentoLiquidacion, "", 0, ref codigoRetorno, ref descripcionRetorno);
            if (codigoRetorno.Equals(0))
            {
                if (dsTipoDocumento.Tables.Count > 0)
                {
                    string ciTipoDocumento = dsTipoDocumento.Tables[0].Rows[0]["tipoDocumento"].ToString();
                    cantidadDocumento = int.Parse(dsTipoDocumento.Tables[0].Rows[0]["cantidadFirma"].ToString());

                    _procesoValidacion.ValidarGenerarFirmaDocumentos("", ciTipoDocumento, cantidadDocumento, ref codigoRetorno, ref descripcionRetorno);
                }
                else
                {
                    //objLogs.creaLogMongo("", "0000", "", "", "GenerarFirmaElectronicaFactura", "No hay Facturas para firmar", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), "FirmaViadoc.WinServ");
                }
            }
            else
            {
                //objLogs.creaLogMongo("", "0000", "", "", "GenerarFirmaElectronicaFactura", descripcionRetorno, DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), "FirmaViadoc.WinServ");
            }

        }
    }
}
