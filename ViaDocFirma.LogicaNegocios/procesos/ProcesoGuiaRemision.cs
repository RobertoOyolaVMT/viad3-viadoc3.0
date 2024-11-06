using eSign;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using ViaDoc.AccesoDatos.winServFirmas;
using ViaDoc.Configuraciones;
using ViaDoc.EntidadNegocios;
using ViaDoc.Utilitarios;

namespace ViaDocFirma.LogicaNegocios.procesos
{
    public class ProcesoGuiaRemision
    {
        public List<XmlGenerados> ProcesarXmlGuiaRemision(Compania compania, string Version, int numeroRegistro)
        {
            int contInicial = 0;
            int codigoRetorno = 0;
            string descripcionRetorno = string.Empty;
            int numeroIntentosGuiaRemision = 0;
            List<XmlGenerados> xmlGuiaRemision = new List<XmlGenerados>();
            XmlGenerados xmlGenerado = null;
            ViaDoc.AccesoDatos.DocumentoAD _consultaDocumentos = new ViaDoc.AccesoDatos.DocumentoAD();

            try
            {
                DataSet dsGuiaRemision = _consultaDocumentos.DocumentosElectronicos(6, compania.CiCompania, "", "", "", "", numeroRegistro, "", ref codigoRetorno, ref descripcionRetorno);
                if (codigoRetorno.Equals(0))
                {
                    contInicial = dsGuiaRemision.Tables[0].Select("ciEstado='" + CatalogoViaDoc.DocEstadoInicial + "'").ToList().Count;
                    if (contInicial == 0)
                        contInicial = dsGuiaRemision.Tables[0].Select("ciEstado='" + CatalogoViaDoc.DocEstadoEFirmado + "'").ToList().Count;

                    dsGuiaRemision.Tables[0].TableName = "GuiaRemision";
                    dsGuiaRemision.Tables[1].TableName = "GuiaRemisionInfoAdicional";
                    dsGuiaRemision.Tables[2].TableName = "GuiaRemisionDestinatario";
                    dsGuiaRemision.Tables[3].TableName = "GuiaRemisionDestinatarioDetalle";

                    #region RELACIONO_TABLAS_GuiaRemision
                    DataColumn[] Pk_GuiaRemision = new DataColumn[4];
                    DataColumn[] Fk_GuiaRemisionInfoAdicional = new DataColumn[4];
                    DataColumn[] Fk_GuiaRemisionDestinatario = new DataColumn[4];
                    DataColumn[] Fk_GuiaRemisionDestinatarioDetalle = new DataColumn[4];

                    //GuiaRemision
                    Pk_GuiaRemision[0] = dsGuiaRemision.Tables["GuiaRemision"].Columns["ciCompania"];
                    Pk_GuiaRemision[1] = dsGuiaRemision.Tables["GuiaRemision"].Columns["txEstablecimiento"];
                    Pk_GuiaRemision[2] = dsGuiaRemision.Tables["GuiaRemision"].Columns["txPuntoEmision"];
                    Pk_GuiaRemision[3] = dsGuiaRemision.Tables["GuiaRemision"].Columns["txSecuencial"];

                    //GuiaRemisionInfoAdicional
                    Fk_GuiaRemisionInfoAdicional[0] = dsGuiaRemision.Tables["GuiaRemisionInfoAdicional"].Columns["ciCompania"];
                    Fk_GuiaRemisionInfoAdicional[1] = dsGuiaRemision.Tables["GuiaRemisionInfoAdicional"].Columns["txEstablecimiento"];
                    Fk_GuiaRemisionInfoAdicional[2] = dsGuiaRemision.Tables["GuiaRemisionInfoAdicional"].Columns["txPuntoEmision"];
                    Fk_GuiaRemisionInfoAdicional[3] = dsGuiaRemision.Tables["GuiaRemisionInfoAdicional"].Columns["txSecuencial"];

                    //GuiaRemisionDetalle fk
                    Fk_GuiaRemisionDestinatario[0] = dsGuiaRemision.Tables["GuiaRemisionDestinatario"].Columns["ciCompania"];
                    Fk_GuiaRemisionDestinatario[1] = dsGuiaRemision.Tables["GuiaRemisionDestinatario"].Columns["txEstablecimiento"];
                    Fk_GuiaRemisionDestinatario[2] = dsGuiaRemision.Tables["GuiaRemisionDestinatario"].Columns["txPuntoEmision"];
                    Fk_GuiaRemisionDestinatario[3] = dsGuiaRemision.Tables["GuiaRemisionDestinatario"].Columns["txSecuencial"];

                    Fk_GuiaRemisionDestinatarioDetalle[0] = dsGuiaRemision.Tables["GuiaRemisionDestinatarioDetalle"].Columns["ciCompania"];
                    Fk_GuiaRemisionDestinatarioDetalle[1] = dsGuiaRemision.Tables["GuiaRemisionDestinatarioDetalle"].Columns["txEstablecimiento"];
                    Fk_GuiaRemisionDestinatarioDetalle[2] = dsGuiaRemision.Tables["GuiaRemisionDestinatarioDetalle"].Columns["txPuntoEmision"];
                    Fk_GuiaRemisionDestinatarioDetalle[3] = dsGuiaRemision.Tables["GuiaRemisionDestinatarioDetalle"].Columns["txSecuencial"];
                    //
                    dsGuiaRemision.Relations.Add("GuiaRemision_GuiaRemisionInfoAdicional", Pk_GuiaRemision, Fk_GuiaRemisionInfoAdicional);
                    dsGuiaRemision.Relations.Add("GuiaRemision_GuiaRemisionDestinatario", Pk_GuiaRemision, Fk_GuiaRemisionDestinatario);
                    dsGuiaRemision.Relations.Add("GuiaRemision_GuiaRemisionDestinatarioDetalle", Pk_GuiaRemision, Fk_GuiaRemisionDestinatarioDetalle);
                    #endregion RELACIONO_TABLAS_GuiaRemision

                    #region GuiaRemisionS

                    #region INFORMACION_TRIBUTARIA
                    guiaRemision GuiaRemision = new guiaRemision();
                    GuiaRemision.id = guiaRemisionID.comprobante;
                    GuiaRemision.version = Version;

                    infoTributaria_GR infoTributariaGR = new infoTributaria_GR();
                    infoTributariaGR.ambiente = compania.CiTipoAmbiente.ToString();
                    infoTributariaGR.dirMatriz = compania.TxDireccionMatriz;
                    infoTributariaGR.nombreComercial = compania.TxNombreComercial;
                    infoTributariaGR.razonSocial = compania.TxRazonSocial;
                    infoTributariaGR.ruc = compania.TxRuc;

                    #region Configuracion de Leyenda
                    //Agente de retención
                    if (!ConfigurationManager.AppSettings.Get("Agente_de_Retención").Equals(""))
                    {
                        string[] Cadena = ConfigurationManager.AppSettings.Get("Agente_de_Retención").Split('|');
                        foreach (string Ruc in Cadena)
                        {
                            if (!Ruc.Equals(""))
                            {
                                if (Convert.ToInt64(compania.TxRuc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                {
                                    bool validaLeyenda = !CatalogoViaDoc.LeyendaAgente.Trim().Equals("") ? true : false;
                                    if (CatalogoViaDoc.LeyendaAgente.Trim() != null)
                                    {
                                        if (validaLeyenda)
                                            infoTributariaGR.agenteRetencion = CatalogoViaDoc.LeyendaAgente.Trim().Replace("Agente de Retención Resolución Nro ", "");
                                    }
                                }
                            }
                        }
                    }

                    //Regimen Micrempresa
                    if (!ConfigurationManager.AppSettings.Get("regimen_Microempresas").Equals(""))
                    {
                        if (!ConfigurationManager.AppSettings.Get("regimen_Microempresas").ToString().Trim().Equals(""))
                        {
                            string[] Cadena = ConfigurationManager.AppSettings.Get("regimen_Microempresas").Split('|');
                            foreach (string Ruc in Cadena)
                            {
                                if (!Ruc.Equals(""))
                                {
                                    if (Convert.ToInt64(compania.TxRuc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                    {
                                        bool validaLeyenda = !CatalogoViaDoc.LeyendaRegimen.Trim().Equals("") ? true : false;
                                        if (CatalogoViaDoc.LeyendaRegimen.Trim() != null)
                                        {
                                            if (validaLeyenda)
                                                infoTributariaGR.regimenMicroempresas = CatalogoViaDoc.LeyendaRegimen.Trim();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            infoTributariaGR.regimenMicroempresas = string.Empty;
                        }

                    }
                    //regimen Rimpe
                    if (!ConfigurationManager.AppSettings.Get("contribuyente_Rimpe").Equals(""))
                    {
                        if (!ConfigurationManager.AppSettings.Get("contribuyente_Rimpe").ToString().Trim().Equals(""))
                        {
                            string[] Cadena = ConfigurationManager.AppSettings.Get("contribuyente_Rimpe").Split('|');
                            foreach (string Ruc in Cadena)
                            {
                                if (!Ruc.Equals(""))
                                {
                                    if (Convert.ToInt64(compania.TxRuc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                    {
                                        bool validaLeyenda = !CatalogoViaDoc.LeyendaRegimenRimpe.Trim().Equals("") ? true : false;
                                        if (CatalogoViaDoc.LeyendaRegimenRimpe.Trim() != null)
                                        {
                                            if (validaLeyenda)
                                                infoTributariaGR.contribuyenteRimpe = CatalogoViaDoc.LeyendaRegimenRimpe.Trim();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            infoTributariaGR.contribuyenteRimpe = string.Empty;
                        }

                    }
                    #endregion

                    guiaRemisionInfoGuiaRemision objInfoGuiaRemision = new guiaRemisionInfoGuiaRemision();

                    if (objInfoGuiaRemision.contribuyenteEspecial != null && objInfoGuiaRemision.contribuyenteEspecial.Trim().Length > 0)
                        objInfoGuiaRemision.contribuyenteEspecial = compania.TxContribuyenteEspecial;

                    if (compania.TxObligadoContabilidad == "S")
                        objInfoGuiaRemision.obligadoContabilidad = "SI";

                    if (compania.TxObligadoContabilidad == "N")
                        objInfoGuiaRemision.obligadoContabilidad = "NO";

                    #endregion INFORMACION_TRIBUTARIA
                    int cont = 0;
                    foreach (DataRow DataRowGuiaRemision in dsGuiaRemision.Tables["GuiaRemision"].Rows)
                    {
                        try
                        {
                            if (DataRowGuiaRemision["ciEstado"].ToString().Trim() == CatalogoViaDoc.DocEstadoInicial
                                || DataRowGuiaRemision["ciEstado"].ToString().Trim() == CatalogoViaDoc.DocEstadoEFirmado)
                            {
                                #region GuiaRemision
                                numeroIntentosGuiaRemision = int.Parse(DataRowGuiaRemision["ciNumeroIntento"].ToString().Trim());
                                #region INFORMACION_TRIBUTARIA_
                                infoTributariaGR.claveAcceso = DataRowGuiaRemision["txClaveAcceso"].ToString();
                                infoTributariaGR.estab = DataRowGuiaRemision["txEstablecimiento"].ToString();
                                infoTributariaGR.ptoEmi = DataRowGuiaRemision["txPuntoEmision"].ToString();
                                infoTributariaGR.secuencial = DataRowGuiaRemision["txSecuencial"].ToString();
                                infoTributariaGR.tipoEmision = DataRowGuiaRemision["ciTipoEmision"].ToString();
                                infoTributariaGR.codDoc = DataRowGuiaRemision["ciTipoDocumento"].ToString();
                                #endregion INFORMACION_TRIBUTARIA_
                                GuiaRemision.infoTributaria = infoTributariaGR;

                                #region INFO_GUIA_REMISION
                                objInfoGuiaRemision.dirEstablecimiento = DataRowGuiaRemision["txDireccion"].ToString();
                                objInfoGuiaRemision.dirPartida = DataRowGuiaRemision["txDireccionPartida"].ToString();
                                objInfoGuiaRemision.razonSocialTransportista = DataRowGuiaRemision["txRazonSocialTransportista"].ToString().Trim().Contains("&amp;") ?
                                    //DataRowGuiaRemision["txRazonSocialTransportista"].ToString().Trim().Replace("&amp:", "Y") : DataRowGuiaRemision["txRazonSocialTransportista"].ToString().Trim().Contains("&") ?
                                    DataRowGuiaRemision["txRazonSocialTransportista"].ToString().Trim().Replace("&", "&amp;") : DataRowGuiaRemision["txRazonSocialTransportista"].ToString().Trim();
                                objInfoGuiaRemision.tipoIdentificacionTransportista = DataRowGuiaRemision["ciTipoIdentificacionTransportista"].ToString();
                                objInfoGuiaRemision.rucTransportista = DataRowGuiaRemision["txRucTransportista"].ToString();

                                if (DataRowGuiaRemision["txRise"].ToString() != null && DataRowGuiaRemision["txRise"].ToString().Trim().Length > 0)
                                    objInfoGuiaRemision.rise = DataRowGuiaRemision["txRise"].ToString();

                                objInfoGuiaRemision.fechaIniTransporte = DataRowGuiaRemision["txFechaIniTransporte"].ToString();
                                objInfoGuiaRemision.fechaFinTransporte = DataRowGuiaRemision["txFechaFinTransporte"].ToString();
                                objInfoGuiaRemision.placa = DataRowGuiaRemision["txPlaca"].ToString();
                                #endregion INFO_GUIA_REMISION
                                GuiaRemision.infoGuiaRemision = objInfoGuiaRemision;

                                #region GuiaRemision_DETALLE
                                cont = 0;
                                DataRow[] drwGuiaRemisionDestinatario = DataRowGuiaRemision.GetChildRows("GuiaRemision_GuiaRemisionDestinatario");
                                if (drwGuiaRemisionDestinatario.LongLength > 0)
                                {
                                    guiaRemisionDestinatarios objGuiaRemisionDestinatario = new guiaRemisionDestinatarios();
                                    destinatario[] Destinatarios = new destinatario[drwGuiaRemisionDestinatario.LongLength];
                                    foreach (DataRow DrwDestinatario in drwGuiaRemisionDestinatario)
                                    {
                                        #region DESTINATARIO
                                        destinatario Destinatario = new destinatario();
                                        Destinatario.identificacionDestinatario = DrwDestinatario["txIdentificacionDestinatario"].ToString();
                                        Destinatario.razonSocialDestinatario = DrwDestinatario["txRazonSocialDestinatario"].ToString().Trim().Contains("&amp;") ?
                                            DrwDestinatario["txRazonSocialDestinatario"].ToString().Trim().Replace("&amp:", "Y") : DrwDestinatario["txRazonSocialDestinatario"].ToString().Trim().Contains("&") ?
                                            DrwDestinatario["txRazonSocialDestinatario"].ToString().Trim().Replace("&", "Y") : DrwDestinatario["txRazonSocialDestinatario"].ToString().Trim();
                                        Destinatario.dirDestinatario = DrwDestinatario["txDireccionDestinatario"].ToString();
                                        Destinatario.motivoTraslado = DrwDestinatario["txMotivoTraslado"].ToString();

                                        if (DrwDestinatario["txDocumentoAduaneroUnico"].ToString() != null && DrwDestinatario["txDocumentoAduaneroUnico"].ToString().Trim().Length > 0)
                                            Destinatario.docAduaneroUnico = DrwDestinatario["txDocumentoAduaneroUnico"].ToString();

                                        if (DrwDestinatario["txCodigoEstablecimientoDestino"].ToString() != null && DrwDestinatario["txCodigoEstablecimientoDestino"].ToString().Trim().Length > 0)
                                            Destinatario.codEstabDestino = DrwDestinatario["txCodigoEstablecimientoDestino"].ToString();

                                        if (DrwDestinatario["txRuta"].ToString() != null && DrwDestinatario["txRuta"].ToString() != "")
                                            Destinatario.ruta = DrwDestinatario["txRuta"].ToString();

                                        if (DrwDestinatario["ciTipoDocumentoSustento"].ToString() != null && DrwDestinatario["ciTipoDocumentoSustento"].ToString().Trim().Length > 0)
                                            Destinatario.codDocSustento = DrwDestinatario["ciTipoDocumentoSustento"].ToString();

                                        if (DrwDestinatario["txNumeroDocumentoSustento"].ToString() != null && DrwDestinatario["txNumeroDocumentoSustento"].ToString().Trim().Length > 0)
                                            Destinatario.numDocSustento = DrwDestinatario["txNumeroDocumentoSustento"].ToString();

                                        if (DrwDestinatario["txNumeroAutorizacionDocumentoSustento"].ToString() != null && DrwDestinatario["txNumeroAutorizacionDocumentoSustento"].ToString().Trim().Length > 0)
                                            Destinatario.numAutDocSustento = DrwDestinatario["txNumeroAutorizacionDocumentoSustento"].ToString();

                                        if (DrwDestinatario["txFechaEmisionDocumentoSustento"].ToString() != null && DrwDestinatario["txFechaEmisionDocumentoSustento"].ToString().Trim().Length > 0)
                                            Destinatario.fechaEmisionDocSustento = DrwDestinatario["txFechaEmisionDocumentoSustento"].ToString();
                                        #endregion DESTINATARIO

                                        #region CAPTURO_GUIA_REMISION_DESTINATARIO_DETALLE
                                        DataTable dtDestinatarioDetalle = new DataTable();
                                        dtDestinatarioDetalle = LnconsultarGuiaRemisionDestinatarioDetalle(
                                         1, Convert.ToInt32(DrwDestinatario["ciCompania"].ToString()),
                                            DrwDestinatario["txEstablecimiento"].ToString(),
                                            DrwDestinatario["txPuntoEmision"].ToString(),
                                            DrwDestinatario["txSecuencial"].ToString(),
                                            DrwDestinatario["txIdentificacionDestinatario"].ToString(), "",
                                            ref codigoRetorno, ref descripcionRetorno);
                                        dtDestinatarioDetalle.TableName = "GuiaRemisionDestinatarioDetalle";

                                        #endregion CAPTURO_GUIA_REMISION_DESTINATARIO_DETALLE

                                        #region DESTINATARIO_DETALLES

                                        if (dtDestinatarioDetalle.Rows.Count > 0)
                                        {
                                            destinatarioDetalles DestinatarioDetalles = new destinatarioDetalles();
                                            detalle[] detalles = new detalle[dtDestinatarioDetalle.Rows.Count];
                                            foreach (DataRow drwDestinatarioDetalle in dtDestinatarioDetalle.Rows)
                                            {
                                                detalle Detalle = new detalle();
                                                Detalle.codigoInterno = drwDestinatarioDetalle["txCodigoInterno"].ToString();

                                                if (drwDestinatarioDetalle["txCodigoAdicional"].ToString() != null && drwDestinatarioDetalle["txCodigoAdicional"].ToString().Trim().Length > 0)
                                                    Detalle.codigoAdicional = drwDestinatarioDetalle["txCodigoAdicional"].ToString();

                                                Detalle.descripcion = drwDestinatarioDetalle["txDescripcion"].ToString().Trim().Contains("&amp;") ?
                                                    drwDestinatarioDetalle["txDescripcion"].ToString().Trim().Replace("&amp:", "Y") : drwDestinatarioDetalle["txDescripcion"].ToString().Trim().Contains("&") ?
                                                    drwDestinatarioDetalle["txDescripcion"].ToString().Trim().Replace("&", "Y") : drwDestinatarioDetalle["txDescripcion"].ToString().Trim();
                                                Detalle.cantidad = Convert.ToDecimal(drwDestinatarioDetalle["qnCantidad"].ToString());

                                                #region CAPTURO_DETALLES_ADICIONALES
                                                DataTable dtdetallesAdicionales = new DataTable();
                                                dtdetallesAdicionales = LnconsultarGuiaRemisionDestinatarioDetalle(
                                                 2, Convert.ToInt32(drwDestinatarioDetalle["ciCompania"].ToString()),
                                                    drwDestinatarioDetalle["txEstablecimiento"].ToString(),
                                                    drwDestinatarioDetalle["txPuntoEmision"].ToString(),
                                                    drwDestinatarioDetalle["txSecuencial"].ToString(),
                                                    drwDestinatarioDetalle["txIdentificacionDestinatario"].ToString(),
                                                    drwDestinatarioDetalle["txCodigoInterno"].ToString(),
                                                    ref codigoRetorno, ref descripcionRetorno);
                                                dtdetallesAdicionales.TableName = "GuiaRemisionDestinatarioDetalleAdicional";

                                                #endregion CAPTURO_DETALLES_ADICIONALES
                                                if (dtdetallesAdicionales.Rows.Count > 0)
                                                {
                                                    detalleDetAdicional[] detallesAdicionales = new detalleDetAdicional[dtdetallesAdicionales.Rows.Count];
                                                    foreach (DataRow drwDetAdicional in dtdetallesAdicionales.Rows)
                                                    {
                                                        try
                                                        {
                                                            detalleDetAdicional DetAdicional = new detalleDetAdicional();
                                                            DetAdicional.nombre = drwDetAdicional["txNombre"].ToString();
                                                            DetAdicional.valor = drwDetAdicional["txValor"].ToString();
                                                            detallesAdicionales[dtdetallesAdicionales.Rows.IndexOf(drwDetAdicional)] = DetAdicional;
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            xmlGenerado = new XmlGenerados();
                                                            xmlGenerado.Identity = Convert.ToInt32(DataRowGuiaRemision["ciGuiaRemision"].ToString());
                                                            xmlGenerado.MensajeError = ex.Message + "||||" + ex.StackTrace;
                                                            xmlGenerado.XmlEstado = CatalogoViaDoc.DocEstadoEFirmado;
                                                            xmlGenerado.txCodError = "101";
                                                            xmlGenerado.CiTipoDocumento = CatalogoViaDoc.DocumentoFactura;
                                                            xmlGenerado.CiCompania = compania.CiCompania;
                                                            xmlGenerado.CiContingenciaDet = 1;
                                                            xmlGenerado.XmlComprobante = Serializacion.serializar(GuiaRemision);
                                                            xmlGenerado.NameXml = GuiaRemision.infoTributaria.claveAcceso;
                                                            xmlGenerado.ClaveAcceso = GuiaRemision.infoTributaria.claveAcceso;
                                                            xmlGenerado.ciNumeroIntento = numeroIntentosGuiaRemision + 1;

                                                            FirmaDocumentos actualizarEstado = new FirmaDocumentos();
                                                            actualizarEstado.ActualizarXmlComprobantes(xmlGenerado);

                                                            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Error Guia Detalle Adicional" + ex.ToString());
                                                        }
                                                    }
                                                    Detalle.detallesAdicionales = detallesAdicionales;
                                                }

                                                detalles[dtDestinatarioDetalle.Rows.IndexOf(drwDestinatarioDetalle)] = Detalle;
                                            }
                                            DestinatarioDetalles.detalle = detalles;
                                            Destinatario.detalles = DestinatarioDetalles;
                                        }
                                        #endregion DESTINATARIO_DETALLES
                                        Destinatarios[cont] = Destinatario;
                                        cont++;
                                        dtDestinatarioDetalle.Dispose();
                                    }
                                    cont = 0;
                                    objGuiaRemisionDestinatario.destinatario = Destinatarios;
                                    GuiaRemision.destinatarios = objGuiaRemisionDestinatario;
                                }
                                #endregion GuiaRemision_DETALLE

                                #region GUIA_REMISION_INFO_ADICIONAL
                                DataRow[] DataRowsGuiaRemisionInfoAdicional = DataRowGuiaRemision.GetChildRows("GuiaRemision_GuiaRemisionInfoAdicional");
                                cont = 0;
                                if (DataRowsGuiaRemisionInfoAdicional.LongLength > 0)
                                {
                                    guiaRemisionCampoAdicional[] GuiaRemisionInformacionAdicional = new guiaRemisionCampoAdicional[DataRowsGuiaRemisionInfoAdicional.LongLength];

                                    foreach (DataRow DataRowFactInfoAdicional in DataRowsGuiaRemisionInfoAdicional)
                                    {
                                        guiaRemisionCampoAdicional InfoAdicional = new guiaRemisionCampoAdicional();
                                        InfoAdicional.nombre = DataRowFactInfoAdicional["txNombre"].ToString();
                                        InfoAdicional.Value = DataRowFactInfoAdicional["txValor"].ToString().Trim().Contains("&amp;") ?
                                            DataRowFactInfoAdicional["txValor"].ToString().Trim().Replace("&amp:", "Y") : DataRowFactInfoAdicional["txValor"].ToString().Trim().Contains("&") ?
                                            DataRowFactInfoAdicional["txValor"].ToString().Trim().Replace("&", "Y") : DataRowFactInfoAdicional["txValor"].ToString().Trim();
                                        GuiaRemisionInformacionAdicional[cont] = InfoAdicional;
                                        cont++;
                                    }
                                    GuiaRemision.infoAdicional = GuiaRemisionInformacionAdicional;
                                }
                                cont = 0;
                                #endregion GUIA_REMISION_INFO_ADICIONAL
                                #endregion GuiaRemision

                                #region GENERA_CLAVE_ACCESO
                                ClaveAcceso claveAcceso = new ClaveAcceso();
                                if (DataRowGuiaRemision["txClaveAcceso"].ToString() == "" || DataRowGuiaRemision["txClaveAcceso"].ToString() == null)
                                {
                                    #region Nuevo documento generacion de su clave                               
                                    string ClaveAccesoNormal = claveAcceso.GenerarClaveAccesoDocumento(GuiaRemision.infoTributaria.codDoc,
                                                                                                       GuiaRemision.infoTributaria.secuencial.ToString().Trim(),
                                                                                                       GuiaRemision.infoTributaria.ptoEmi,
                                                                                                       GuiaRemision.infoTributaria.estab,
                                                                                                       GuiaRemision.infoGuiaRemision.fechaIniTransporte,
                                                                                                       GuiaRemision.infoTributaria.ruc,
                                                                                                       GuiaRemision.infoTributaria.ambiente);

                                    if (ClaveAccesoNormal.Length == 49)
                                    {
                                        GuiaRemision.infoTributaria.claveAcceso = ClaveAccesoNormal;
                                    }
                                    else
                                    {
                                        GuiaRemision.infoTributaria.claveAcceso = claveAcceso.GenerarClaveAccesoDocumento(GuiaRemision.infoTributaria.codDoc,
                                                                                                                          GuiaRemision.infoTributaria.secuencial.ToString().Trim(),
                                                                                                                          GuiaRemision.infoTributaria.ptoEmi,
                                                                                                                          GuiaRemision.infoTributaria.estab,
                                                                                                                          GuiaRemision.infoGuiaRemision.fechaIniTransporte,
                                                                                                                          GuiaRemision.infoTributaria.ruc,
                                                                                                                          GuiaRemision.infoTributaria.ambiente);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Documento ya contiene una clave de Acceso
                                    if (GuiaRemision.infoGuiaRemision.fechaIniTransporte.Replace("/", "") != DataRowGuiaRemision["txClaveAcceso"].ToString().Substring(0, 8))
                                    {
                                        #region Nuevo documento generacion de su clave                               
                                        string ClaveAccesoNormal = claveAcceso.GenerarClaveAccesoDocumento(GuiaRemision.infoTributaria.codDoc,
                                                                                                           GuiaRemision.infoTributaria.secuencial.ToString().Trim(),
                                                                                                           GuiaRemision.infoTributaria.ptoEmi,
                                                                                                           GuiaRemision.infoTributaria.estab,
                                                                                                           GuiaRemision.infoGuiaRemision.fechaIniTransporte,
                                                                                                           GuiaRemision.infoTributaria.ruc,
                                                                                                           GuiaRemision.infoTributaria.ambiente);

                                        if (ClaveAccesoNormal.Length == 49)
                                        {
                                            GuiaRemision.infoTributaria.claveAcceso = ClaveAccesoNormal;
                                        }
                                        else
                                        {
                                            GuiaRemision.infoTributaria.claveAcceso = claveAcceso.GenerarClaveAccesoDocumento(GuiaRemision.infoTributaria.codDoc,
                                                                                                                              GuiaRemision.infoTributaria.secuencial.ToString().Trim(),
                                                                                                                              GuiaRemision.infoTributaria.ptoEmi,
                                                                                                                              GuiaRemision.infoTributaria.estab,
                                                                                                                              GuiaRemision.infoGuiaRemision.fechaIniTransporte,
                                                                                                                              GuiaRemision.infoTributaria.ruc,
                                                                                                                              GuiaRemision.infoTributaria.ambiente);
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                                #endregion GENERA_CLAVE_ACCESO

                                xmlGenerado = new XmlGenerados();
                                xmlGenerado.Identity = Convert.ToInt32(DataRowGuiaRemision["ciGuiaRemision"].ToString());
                                xmlGenerado.CiCompania = compania.CiCompania;
                                xmlGenerado.NameXml = GuiaRemision.infoTributaria.codDoc + "_" + compania.CiCompania + "_" + infoTributariaGR.estab + "_" + infoTributariaGR.ptoEmi + "_" + infoTributariaGR.secuencial;
                                xmlGenerado.XmlComprobante = Serializacion.serializar(GuiaRemision);
                                xmlGenerado.ClaveAcceso = GuiaRemision.infoTributaria.claveAcceso;
                                xmlGenerado.CiTipoEmision = GuiaRemision.infoTributaria.tipoEmision;
                                xmlGenerado.CiTipoDocumento = GuiaRemision.infoTributaria.codDoc;
                                xmlGenerado.XmlEstado = CatalogoViaDoc.DocEstadoGenerado;
                                xmlGenerado.ciNumeroIntento = numeroIntentosGuiaRemision;
                                xmlGuiaRemision.Add(xmlGenerado);
                            }
                            else
                            {
                                GuiaRemision = (guiaRemision)Serializacion.desSerializar(DataRowGuiaRemision["xmlDocumentoAutorizado"].ToString(), GuiaRemision.GetType());
                                xmlGenerado = new XmlGenerados();
                                xmlGenerado.Identity = Convert.ToInt32(DataRowGuiaRemision["ciGuiaRemision"].ToString());
                                xmlGenerado.CiCompania = compania.CiCompania;
                                xmlGenerado.NameXml = GuiaRemision.infoTributaria.codDoc + "_" + compania.CiCompania + "_" + GuiaRemision.infoTributaria.estab + "_" + GuiaRemision.infoTributaria.ptoEmi + "_" + GuiaRemision.infoTributaria.secuencial;
                                xmlGenerado.XmlComprobante = Serializacion.serializar(GuiaRemision);
                                xmlGenerado.ClaveAcceso = GuiaRemision.infoTributaria.claveAcceso;
                                xmlGenerado.CiTipoEmision = GuiaRemision.infoTributaria.tipoEmision;
                                xmlGenerado.CiTipoDocumento = GuiaRemision.infoTributaria.codDoc;
                                xmlGenerado.CiContingenciaDet = Convert.ToInt32(DataRowGuiaRemision["ciContingenciaDet"].ToString());
                                xmlGenerado.XmlEstado = CatalogoViaDoc.DocEstadoGenerado;
                                xmlGenerado.ciNumeroIntento = numeroIntentosGuiaRemision;
                                xmlGuiaRemision.Add(xmlGenerado);
                            }
                        }
                        catch (Exception ex)
                        {
                            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Error Guia" + ex.ToString());

                            xmlGenerado = new XmlGenerados();
                            xmlGenerado.Identity = Convert.ToInt32(DataRowGuiaRemision["ciGuiaRemision"].ToString());
                            xmlGenerado.MensajeError = ex.Message + "||||" + ex.StackTrace;
                            xmlGenerado.XmlEstado = CatalogoViaDoc.DocEstadoEFirmado;
                            xmlGenerado.txCodError = "101";
                            xmlGenerado.CiTipoDocumento = CatalogoViaDoc.DocumentoFactura;
                            xmlGenerado.CiCompania = compania.CiCompania;
                            xmlGenerado.CiContingenciaDet = 1;
                            xmlGenerado.XmlComprobante = Serializacion.serializar(GuiaRemision);
                            xmlGenerado.NameXml = GuiaRemision.infoTributaria.claveAcceso;
                            xmlGenerado.ClaveAcceso = GuiaRemision.infoTributaria.claveAcceso;
                            xmlGenerado.ciNumeroIntento = numeroIntentosGuiaRemision + 1;

                            FirmaDocumentos actualizarEstado = new FirmaDocumentos();
                            actualizarEstado.ActualizarXmlComprobantes(xmlGenerado);
                        }
                    }
                    dsGuiaRemision.Dispose();
                    #endregion GuiaRemision
                }
            }
            catch (Exception ex)
            {

            }
            return xmlGuiaRemision;
        }



        protected DataTable LnconsultarGuiaRemisionDestinatarioDetalle(int op, int ciCompañia, string txEstablecimiento, string txPuntoEmision, string txSecuencial,
                                                                     string txIdentificacionDestinatario, string txCodigoInterno, ref int codigoRetorno, ref string descripcionRetorno)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            ViaDoc.AccesoDatos.DocumentoAD _consultaDocumentos = new ViaDoc.AccesoDatos.DocumentoAD();

            ds = _consultaDocumentos.DocumentosElectronicos(10, ciCompañia, txEstablecimiento, txPuntoEmision, txSecuencial, txIdentificacionDestinatario,
                                                                   0, txCodigoInterno, ref codigoRetorno, ref descripcionRetorno);

            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }

    }
}
