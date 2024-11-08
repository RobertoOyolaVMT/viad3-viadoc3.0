﻿using eSign;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using ViaDoc.AccesoDatos.winServFirmas;
using ViaDoc.Configuraciones;
using ViaDoc.EntidadNegocios;
using ViaDoc.EntidadNegocios.usuario;
using ViaDoc.Logs;
using ViaDoc.Utilitarios;
using ViaDocFirma.LogicaNegocios.procesos;


namespace ViaDocFirma.LogicaNegocios
{
    public class ValidacionCertificado
    {
        ConexionBDMongo objLogs = new ConexionBDMongo();
        public void ValidarGenerarFirmaDocumentos(string esquemaVersion, string ciTipoDocumento, int numeroRegistro,
                                                  ref int codigoRetorno, ref string mensajeRetorno)
        {
            string idCompania = string.Empty;

            //esquemaVersion = string.Empty;
            DataTable dtEsquemas = new DataTable();
            DataTable dt = new DataTable();

            try
            {
                Compania companias = new Compania();
                DataSet dsCompaniaCertificado = companias.ConsultaCompanias_y_Certificados(ref codigoRetorno, ref mensajeRetorno);
                if (codigoRetorno.Equals(0))
                {
                    dtEsquemas = companias.ConsultarEsquemasDocumentos(ciTipoDocumento, esquemaVersion, ref codigoRetorno, ref mensajeRetorno);

                    dsCompaniaCertificado.Relations.Add("COMPAÑIAS", dsCompaniaCertificado.Tables[0].Columns["ciCompania"], dsCompaniaCertificado.Tables[1].Columns["ciCompania"]);
                    idCompania = dsCompaniaCertificado.Tables[0].Columns["ciCompania"].ToString();
                    esquemaVersion = dtEsquemas.Rows[0]["txVersion"].ToString();
                }

                if (codigoRetorno.Equals(0))
                {
                    foreach (DataRow companiasCertificadas in dsCompaniaCertificado.Tables[0].Rows)
                    {
                        int band = 0;
                        var compania = ObtenerDatosCompania(companiasCertificadas);
                        DataRow[] companiasCertificados = companiasCertificadas.GetChildRows("COMPAÑIAS");
                        if (companiasCertificados.LongLength > 0)
                        {
                            Certificado certificado = new Certificado();
                            DateTime fechaMenor = new DateTime(01, 01, 0001, 12, 00, 00);
                            DateTime fechaAuxiliar = new DateTime();
                            DateTime objFecha = VerificaActualizaEstadoCertificado(companiasCertificados);
                            #region OBTENGO_CERTIFICADOS
                            foreach (DataRow CertificadosDataRow in companiasCertificados)
                            {
                                fechaAuxiliar = (DateTime)CertificadosDataRow["fcHasta"];
                                if (band == 0)
                                {
                                    band = 1;
                                    fechaMenor = (DateTime)CertificadosDataRow["fcHasta"];
                                    certificado = ObtenerDatosCertificados(CertificadosDataRow);
                                }
                                else
                                {
                                    if ((DateTime.Compare(fechaAuxiliar, fechaMenor) < 0) && (DateTime.Compare(fechaAuxiliar, DateTime.Now) == 0 || DateTime.Compare(fechaAuxiliar, DateTime.Now) > 0))
                                    {
                                        fechaMenor = fechaAuxiliar;
                                        certificado = ObtenerDatosCertificados(CertificadosDataRow);
                                    }
                                }
                            }
                            #endregion OBTENGO_CERTIFICADOS
                            if (fechaMenor.CompareTo(Convert.ToDateTime("01/01/0001 12:00:00")) != 0)
                            {
                                string ClaveCertificado = "";
                                bool banderaConectividad = true;
                                int codigoCompania = Convert.ToInt32(companiasCertificadas["ciCompania"].ToString());
                                string razonSocial = companiasCertificadas["txRazonSocial"].ToString().Trim();
                                ClaveCertificado = DescencriptaClaveCertificado(ref banderaConectividad, compania.TxRuc, compania.UiCompania, certificado.TxClave, certificado.UiSemilla, certificado.txKey);
                                Sign eSign = new Sign();

                                if (ClaveCertificado.Trim().Length != 0)
                                {
                                    eSign.SeteaCertificado(certificado.ObCertificado, ClaveCertificado);
                                    foreach (DataRow Esquema in dtEsquemas.Rows)
                                    {
                                        if (ciTipoDocumento == CatalogoViaDoc.DocumentoFactura)
                                            ConsultarDocumentosAFirmarFactura(eSign, compania, Esquema["xmlEsquema"].ToString().Trim(), Esquema["txVersion"].ToString().Trim(), numeroRegistro);

                                        if (ciTipoDocumento == CatalogoViaDoc.DocumentoCompRetencion)
                                            ConsultarDocumentosAFirmarCompRetencion(eSign, compania, Esquema["xmlEsquema"].ToString(), Esquema["txVersion"].ToString().Trim(), numeroRegistro);

                                        if (ciTipoDocumento == CatalogoViaDoc.DocumentoNotaCredito)
                                            ConsultarDocumentosAFirmarNotaCredito(eSign, compania, Esquema["xmlEsquema"].ToString(), Esquema["txVersion"].ToString().Trim(), numeroRegistro);

                                        if (ciTipoDocumento == CatalogoViaDoc.DocumentoNotaDebito)
                                            ConsultarDocumentosAFirmarNotaDebito(eSign, compania, Esquema["xmlEsquema"].ToString(), Esquema["txVersion"].ToString().Trim(), numeroRegistro);

                                        if (ciTipoDocumento == CatalogoViaDoc.DocumentoGuiaRemision)
                                            ConsultarDocumentosAFirmarGuiaRemision(eSign, compania, Esquema["xmlEsquema"].ToString(), Esquema["txVersion"].ToString().Trim(), numeroRegistro);

                                        if (ciTipoDocumento == CatalogoViaDoc.DocumentoLiquidacion)
                                            ConsultaDocumentoAFirmarLiquidacion(eSign, compania, Esquema["xmlEsquema"].ToString(), Esquema["txVersion"].ToString().Trim(), numeroRegistro);
                                    }
                                }
                                else
                                {
                                    codigoRetorno = 1;
                                    mensajeRetorno = "Certificado no se obtuvo clave de Firma.";
                                }
                            }
                            else
                            {
                                codigoRetorno = 1;
                                mensajeRetorno = "Certificado digital Caducado..";
                                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(mensajeRetorno);
                            }
                        }
                        else
                        {
                            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("No hay certificados vigentes");
                        }
                    }
                }
                else
                {
                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Error al verificar certificado");
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
            }
        }

        protected void ConsultarDocumentosAFirmarFactura(Sign eSign, Compania compania, String EsquemaXSD, string Version, int numeroRegistro)
        {
            List<XmlGenerados> xmlGenerados = new List<XmlGenerados>();
            FirmaDocumentos _metodosFirmas = new FirmaDocumentos();
            ProcesoFacturas procesoFacturas = new ProcesoFacturas();
            xmlGenerados = procesoFacturas.ProcesaXmlFacturas(compania, Version, numeroRegistro);

            if (xmlGenerados.Count > 0)
            {
                _metodosFirmas.ProcesosFirmarDocumentos(ref xmlGenerados, eSign, compania, EsquemaXSD);
            }
            else
            {
                //objLogs.creaLogMongo("ValidacionCertificado", "9999", compania.CiCompania.ToString(), "", "ConsultarDocumentosAFirmarFactura", "No existe Xml para firmar", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), "ViaDocFirma.LogicaNegocios");
            }
        }

        protected void ConsultarDocumentosAFirmarCompRetencion(Sign eSign, Compania compania, String EsquemaXSD, string Version, int numeroRegistro)
        {
            //
            List<XmlGenerados> xmlGenerados = new List<XmlGenerados>();
            FirmaDocumentos _metodosFirmas = new FirmaDocumentos();
            ProcesoCompRetencion procesoCompRetencion = new ProcesoCompRetencion();
            xmlGenerados = procesoCompRetencion.ProcesarXmlCompRetencion(compania, Version, numeroRegistro);

            if (xmlGenerados.Count > 0)
            {
                _metodosFirmas.ProcesosFirmarDocumentos(ref xmlGenerados, eSign, compania, EsquemaXSD);
            }
            else
            {

            }
        }

        protected void ConsultarDocumentosAFirmarNotaCredito(Sign eSign, Compania compania, String EsquemaXSD, string Version, int numeroRegistro)
        {
            List<XmlGenerados> xmlGenerados = new List<XmlGenerados>();
            FirmaDocumentos _metodosFirmas = new FirmaDocumentos();
            ProcesoNotaCredito procesoNotaCredito = new ProcesoNotaCredito();

            xmlGenerados = procesoNotaCredito.ProcesarXmlNotaCredito(compania, Version, numeroRegistro);

            if (xmlGenerados.Count > 0)
            {
                _metodosFirmas.ProcesosFirmarDocumentos(ref xmlGenerados, eSign, compania, EsquemaXSD);
            }
            else
            {

            }
        }

        protected void ConsultarDocumentosAFirmarNotaDebito(Sign eSign, Compania compania, String EsquemaXSD, string Version, int numeroRegistro)
        {
            List<XmlGenerados> xmlGenerados = new List<XmlGenerados>();
            FirmaDocumentos _metodosFirmas = new FirmaDocumentos();
            ProcesoNotaDebito procesoNotaDebito = new ProcesoNotaDebito();
            xmlGenerados = procesoNotaDebito.ProcesarXmlNotaDebito(compania, Version, numeroRegistro);

            if (xmlGenerados.Count > 0)
            {
                _metodosFirmas.ProcesosFirmarDocumentos(ref xmlGenerados, eSign, compania, EsquemaXSD);
            }
            else
            {
            }
        }

        protected void ConsultarDocumentosAFirmarGuiaRemision(Sign eSign, Compania compania, String EsquemaXSD, string Version, int numeroRegistro)
        {
            List<XmlGenerados> xmlGenerados = new List<XmlGenerados>();
            FirmaDocumentos _metodosFirmas = new FirmaDocumentos();
            ProcesoGuiaRemision procesoGuiaRemision = new ProcesoGuiaRemision();
            xmlGenerados = procesoGuiaRemision.ProcesarXmlGuiaRemision(compania, Version, numeroRegistro);

            if (xmlGenerados.Count > 0)
            {
                _metodosFirmas.ProcesosFirmarDocumentos(ref xmlGenerados, eSign, compania, EsquemaXSD);
            }
            else
            {
            }
        }

        protected void ConsultaDocumentoAFirmarLiquidacion(Sign eSign, Compania compania, string EsquemaXSD, string Version, int numeroRegistro)
        {
            List<XmlGenerados> xmlGenerados = new List<XmlGenerados>();
            FirmaDocumentos _metodosFirmas = new FirmaDocumentos();
            ProcesoLiquidaacion ProceLiqui = new ProcesoLiquidaacion();
            xmlGenerados = ProceLiqui.ProcesaXmlLiquidacion(compania, Version, numeroRegistro);
            if (xmlGenerados.Count > 0)
            {
                _metodosFirmas.ProcesosFirmarDocumentos(ref xmlGenerados, eSign, compania, EsquemaXSD);
            }
            else
            {

            }

        }

        protected string DescencriptaClaveCertificado(ref bool respuestaConectividad, string txRuc, string uiCompania, string txClave, Guid uiSemilla, string txKey)
        {
            string txtclaveDes = "";
            try
            {
                AlgoritmoRijndael algoritmo = new AlgoritmoRijndael();
                string keyUisemilla = RetornaUiSemilla(uiSemilla);
                txtclaveDes = algoritmo.DescencriptartxtClaveEncrypt(txKey, txClave, keyUisemilla);

                if (txtclaveDes != "" && txtclaveDes.Length > 0)
                    respuestaConectividad = false;
            }
            catch (Exception ex)
            {

            }
            return txtclaveDes;
        }

        protected Compania ObtenerDatosCompania(DataRow requestCompania)
        {
            Compania responseCompania = new Compania();
            responseCompania.CiCompania = Convert.ToInt32(requestCompania["ciCompania"].ToString());
            responseCompania.UiCompania = requestCompania["uiCompania"].ToString();
            responseCompania.TxRuc = requestCompania["txRuc"].ToString();
            responseCompania.CiTipoAmbiente = Convert.ToInt32(requestCompania["ciTipoAmbiente"].ToString());
            responseCompania.TxNombreComercial = requestCompania["txNombreComercial"].ToString();
            responseCompania.TxDireccionMatriz = requestCompania["txDireccionMatriz"].ToString();
            responseCompania.TxRazonSocial = requestCompania["txRazonSocial"].ToString();
            responseCompania.TxObligadoContabilidad = requestCompania["txObligadoContabilidad"].ToString();
            responseCompania.TxContribuyenteEspecial = requestCompania["txContribuyenteEspecial"].ToString();
            responseCompania.CiTipoAmbiente = Convert.ToInt32(requestCompania["ciTipoAmbiente"].ToString());

            return responseCompania;
        }

        protected DateTime VerificaActualizaEstadoCertificado(DataRow[] companiasCertificados)
        {
            int codigoRetorno = 0;
            string mensajeRetorno = string.Empty;
            DateTime fechaValida = new DateTime();
            CompaniasCertificadosAD _metodosCertificados = new CompaniasCertificadosAD();
            foreach (DataRow dtrCertificado in companiasCertificados)
            {
                fechaValida = (DateTime)dtrCertificado["fcHasta"];
                if (DateTime.Compare(fechaValida, DateTime.Now) < 0)
                {
                    //FALTAN AGG ESTA
                    _metodosCertificados.MantenimientoCertificado(3, Convert.ToInt32(dtrCertificado["ciCompania"].ToString()), 
                            0, "", "", "",dtrCertificado["obCertificado"].ToString(), fechaValida, fechaValida, "I", ref codigoRetorno, ref mensajeRetorno);

                }
            }
            return fechaValida;
        }

        protected Certificado ObtenerDatosCertificados(DataRow CertificadoDataRow)
        {
            Certificado responseCertificados = new Certificado();
            try
            {
                string xmlFirmaElectronica = CertificadoDataRow["obCertificado"].ToString();
                String[] tempAry = xmlFirmaElectronica.Split('-');
                byte[] decBytes2 = new byte[tempAry.Length];
                for (int i = 0; i < tempAry.Length; i++)
                    decBytes2[i] = Convert.ToByte(tempAry[i], 16);

                responseCertificados.CiCompania = Convert.ToInt32(CertificadoDataRow["ciCompania"].ToString());
                responseCertificados.UiSemilla = (Guid)CertificadoDataRow["uiSemilla"];
                responseCertificados.TxClave = CertificadoDataRow["txClave"].ToString();
                responseCertificados.txKey = CertificadoDataRow["txKey"].ToString();
                responseCertificados.ObCertificado = (byte[])decBytes2;
                responseCertificados.FcDesde = (DateTime)CertificadoDataRow["fcDesde"];
                responseCertificados.FcHasta = (DateTime)CertificadoDataRow["fcHasta"];
            }
            catch (Exception ex)
            {
                string descripcion = ex.Message;
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
            }
            return responseCertificados;
        }

        protected bool ConsultarInformacionDelCertificado(string ruta, string pass, ref Certificado certificado)
        {
            bool Valida = false;
            try
            {
                X509Certificate2 objCert = new X509Certificate2(ruta, pass); //Acá tenemos que poner el certificado
                StringBuilder objSB = new StringBuilder("Detalle del certificado: \n\n");

                certificado.FcDesde = Convert.ToDateTime(objCert.NotBefore.ToString());
                certificado.FcHasta = Convert.ToDateTime(objCert.NotAfter.ToString());
                Valida = true;
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                Valida = false;
            }
            return Valida;
        }

        protected string RetornaUiSemilla(Guid keytmp)
        {
            string key = keytmp.ToString().Replace("-", "");
            try
            {
                key = key.Substring(0, key.Length / 2);
            }
            catch (Exception ex)
            {

            }
            return key;
        }
    }
}
