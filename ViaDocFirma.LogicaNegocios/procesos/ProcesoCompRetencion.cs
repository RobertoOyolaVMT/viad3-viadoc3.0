using eSign;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaDoc.AccesoDatos.winServFirmas;
using ViaDoc.Configuraciones;
using ViaDoc.EntidadNegocios;
using ViaDoc.Utilitarios;

namespace ViaDocFirma.LogicaNegocios.procesos
{
    public class ProcesoCompRetencion
    {
        public List<XmlGenerados> ProcesarXmlCompRetencion(Compania compania, string Version, int numeroRegistro)
        {
            var TarifaSustento = string.Empty;
            var ValorImpuestoSustento = string.Empty;
            var baseImponibleSustento = string.Empty;
            var codImpuestoDocSustento = string.Empty;
            var codigoPorcentajeSustento = string.Empty;
            var numDocSustento = string.Empty;

            int contInicial = 0;
            int codigoRetorno = 0;
            string descripcionRetorno = string.Empty;
            List<XmlGenerados> xmlCompRetencion = new List<XmlGenerados>();
            int numeroIntentosCompRetencion = 0;
            FirmaDocumentos actualizarEstado = new FirmaDocumentos();
            XmlGenerados xmlGenerado = null;
            ViaDoc.AccesoDatos.DocumentoAD _consultaDocumentos = new ViaDoc.AccesoDatos.DocumentoAD();

            try
            {
                DataSet dsComprobantesRetencion = _consultaDocumentos.DocumentosElectronicos(3, compania.CiCompania, "", "", "", "",
                                                                                             numeroRegistro, "", ref codigoRetorno, ref descripcionRetorno);

                if (codigoRetorno.Equals(0))
                {
                    contInicial = dsComprobantesRetencion.Tables[0].Select("ciEstado='" + CatalogoViaDoc.DocEstadoInicial + "'").ToList().Count;
                    if (contInicial == 0)
                        contInicial = dsComprobantesRetencion.Tables[0].Select("ciEstado='" + CatalogoViaDoc.DocEstadoEFirmado + "'").ToList().Count;

                    dsComprobantesRetencion.Tables[0].TableName = "CompRetencion";
                    dsComprobantesRetencion.Tables[1].TableName = "CompRetencionDetalle";
                    dsComprobantesRetencion.Tables[2].TableName = "CompRetencionInfoAdicional";
                    dsComprobantesRetencion.Tables[3].TableName = "CompRetencionDocSustento";
                    dsComprobantesRetencion.Tables[4].TableName = "CompRetencionFormaPago";

                    #region RELACIONO_TABLAS_COMPETENCION
                    DataColumn[] parentColumnsComprobantesRet = new DataColumn[4];
                    DataColumn[] childColumnsCompRetencionDetalle = new DataColumn[4];
                    DataColumn[] childColumnsCompRetencionInfoAdicional = new DataColumn[4];
                    DataColumn[] childColumnsCompRetencionDocSustento = new DataColumn[4];
                    DataColumn[] childColumnsCompRetencionFormaPago = new DataColumn[4];

                    //COMPROBANTE DE RETENCION
                    parentColumnsComprobantesRet[0] = dsComprobantesRetencion.Tables["CompRetencion"].Columns["ciCompania"];
                    parentColumnsComprobantesRet[1] = dsComprobantesRetencion.Tables["CompRetencion"].Columns["txEstablecimiento"];
                    parentColumnsComprobantesRet[2] = dsComprobantesRetencion.Tables["CompRetencion"].Columns["txPuntoEmision"];
                    parentColumnsComprobantesRet[3] = dsComprobantesRetencion.Tables["CompRetencion"].Columns["txSecuencial"];
                    //DETALLE DEL COMPROBANTE
                    childColumnsCompRetencionDetalle[0] = dsComprobantesRetencion.Tables["CompRetencionDetalle"].Columns["ciCompania"];
                    childColumnsCompRetencionDetalle[1] = dsComprobantesRetencion.Tables["CompRetencionDetalle"].Columns["txEstablecimiento"];
                    childColumnsCompRetencionDetalle[2] = dsComprobantesRetencion.Tables["CompRetencionDetalle"].Columns["txPuntoEmision"];
                    childColumnsCompRetencionDetalle[3] = dsComprobantesRetencion.Tables["CompRetencionDetalle"].Columns["txSecuencial"];
                    //INFORMACION ADICIONAL DEL COMPROBANTE
                    childColumnsCompRetencionInfoAdicional[0] = dsComprobantesRetencion.Tables["CompRetencionInfoAdicional"].Columns["ciCompania"];
                    childColumnsCompRetencionInfoAdicional[1] = dsComprobantesRetencion.Tables["CompRetencionInfoAdicional"].Columns["txEstablecimiento"];
                    childColumnsCompRetencionInfoAdicional[2] = dsComprobantesRetencion.Tables["CompRetencionInfoAdicional"].Columns["txPuntoEmision"];
                    childColumnsCompRetencionInfoAdicional[3] = dsComprobantesRetencion.Tables["CompRetencionInfoAdicional"].Columns["txSecuencial"];
                    //DOC SUSTENTO DEL COMMPROBANTE
                    childColumnsCompRetencionDocSustento[0] = dsComprobantesRetencion.Tables["CompRetencionDocSustento"].Columns["ciCompania"];
                    childColumnsCompRetencionDocSustento[1] = dsComprobantesRetencion.Tables["CompRetencionDocSustento"].Columns["txEstablecimiento"];
                    childColumnsCompRetencionDocSustento[2] = dsComprobantesRetencion.Tables["CompRetencionDocSustento"].Columns["txPuntoEmision"];
                    childColumnsCompRetencionDocSustento[3] = dsComprobantesRetencion.Tables["CompRetencionDocSustento"].Columns["txSecuencial"];
                    //FORMA PAGO DEL COMPROBANTE
                    childColumnsCompRetencionFormaPago[0] = dsComprobantesRetencion.Tables["CompRetencionFormaPago"].Columns["ciCompania"];
                    childColumnsCompRetencionFormaPago[1] = dsComprobantesRetencion.Tables["CompRetencionFormaPago"].Columns["txEstablecimiento"];
                    childColumnsCompRetencionFormaPago[2] = dsComprobantesRetencion.Tables["CompRetencionFormaPago"].Columns["txPuntoEmision"];
                    childColumnsCompRetencionFormaPago[3] = dsComprobantesRetencion.Tables["CompRetencionFormaPago"].Columns["txSecuencial"];
                    #endregion RELACIONO_TABLAS_COMPETENCION

                    dsComprobantesRetencion.Relations.Add("COMPROBANTES", parentColumnsComprobantesRet, childColumnsCompRetencionDetalle);
                    dsComprobantesRetencion.Relations.Add("COMPROBANTESINF", parentColumnsComprobantesRet, childColumnsCompRetencionInfoAdicional);
                    dsComprobantesRetencion.Relations.Add("COMPROBANTESDOCS", parentColumnsComprobantesRet, childColumnsCompRetencionDocSustento);
                    dsComprobantesRetencion.Relations.Add("COMPROBANTESPAGO", parentColumnsComprobantesRet, childColumnsCompRetencionFormaPago);


                    comprobanteRetencion comprobanteReten = new comprobanteRetencion();
                    comprobanteRetencionDocsSustento docsSustento = new comprobanteRetencionDocsSustento();
                    comprobanteRetencionDocSustento docSustento = new comprobanteRetencionDocSustento();
                    #region INFORMACION_TRIBUTARIA
                    comprobanteReten.id = comprobanteRetencionID.comprobante;
                    comprobanteReten.idSpecified = true;
                    comprobanteReten.version = Version;

                    infoTributaria_CR infoTributariaCR = new infoTributaria_CR();
                    infoTributariaCR.ambiente = compania.CiTipoAmbiente.ToString();
                    infoTributariaCR.dirMatriz = compania.TxDireccionMatriz; //proviene de Compania

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
                                            infoTributariaCR.agenteRetencion = CatalogoViaDoc.LeyendaAgente.Trim().Replace("Agente de Retención Resolución Nro ", "");
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
                                                infoTributariaCR.regimenMicroempresas = CatalogoViaDoc.LeyendaRegimen.Trim();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            infoTributariaCR.regimenMicroempresas = string.Empty;
                        }

                    }

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
                                                infoTributariaCR.contribuyenteRimpe = CatalogoViaDoc.LeyendaRegimenRimpe.Trim();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            infoTributariaCR.contribuyenteRimpe = string.Empty;
                        }

                    }
                    #endregion

                    if (compania.TxNombreComercial.Trim().Length != 0)
                        infoTributariaCR.nombreComercial = compania.TxNombreComercial;  //proviene de Compania     

                    infoTributariaCR.razonSocial = compania.TxRazonSocial;  //proviene de Compania
                    infoTributariaCR.ruc = compania.TxRuc;   //proviene de Compania

                    comprobanteRetencionInfoCompRetencion InfoCompRetencionCR = new comprobanteRetencionInfoCompRetencion();

                    if (compania.TxContribuyenteEspecial.Trim().Length != 0)
                        InfoCompRetencionCR.contribuyenteEspecial = compania.TxContribuyenteEspecial; //proviene de Compania    

                    if (compania.TxObligadoContabilidad == "S")
                        InfoCompRetencionCR.obligadoContabilidad = "SI"; //proviene de Compania

                    if (compania.TxObligadoContabilidad == "N")
                        InfoCompRetencionCR.obligadoContabilidad = "NO"; //proviene de Compania

                    foreach (DataRow DataRowComprobanteRetencion in dsComprobantesRetencion.Tables[0].Rows)
                    {
                        try
                        {
                            if (DataRowComprobanteRetencion["ciEstado"].ToString().Trim() == CatalogoViaDoc.DocEstadoInicial
                                || DataRowComprobanteRetencion["ciEstado"].ToString().Trim() == CatalogoViaDoc.DocEstadoEFirmado)
                            {
                                #region COMPROBANTE_RETENCION
                                infoTributariaCR.claveAcceso = DataRowComprobanteRetencion["txClaveAcceso"].ToString().Trim();  //proviene de CompRetencion
                                infoTributariaCR.estab = DataRowComprobanteRetencion["txEstablecimiento"].ToString().Trim();     //proviene de CompRetencion ojo
                                infoTributariaCR.ptoEmi = DataRowComprobanteRetencion["txPuntoEmision"].ToString().Trim();    //proviene de CompRetencion          
                                infoTributariaCR.secuencial = DataRowComprobanteRetencion["txSecuencial"].ToString().Trim();    //proviene de CompRetencion
                                infoTributariaCR.tipoEmision = DataRowComprobanteRetencion["ciTipoEmision"].ToString().Trim();   //proviene de CompRetencion
                                infoTributariaCR.codDoc = DataRowComprobanteRetencion["ciTipoDocumento"].ToString().Trim();
                                comprobanteReten.infoTributaria = infoTributariaCR;

                                InfoCompRetencionCR.dirEstablecimiento = DataRowComprobanteRetencion["txDireccion"].ToString().Trim();//viene de tabla sucursal
                                InfoCompRetencionCR.fechaEmision = DataRowComprobanteRetencion["txFechaEmision"].ToString().Trim();
                                InfoCompRetencionCR.tipoIdentificacionSujetoRetenido = DataRowComprobanteRetencion["ciTipoIdentificacionSujetoRetenido"].ToString().Trim();
                                InfoCompRetencionCR.parteRel = "NO";
                                InfoCompRetencionCR.identificacionSujetoRetenido = DataRowComprobanteRetencion["txIdentificacionSujetoRetenido"].ToString().Trim();
                                InfoCompRetencionCR.razonSocialSujetoRetenido = DataRowComprobanteRetencion["txRazonSocialSujetoRetenido"].ToString().Trim().Contains("&amp;") ? DataRowComprobanteRetencion["txRazonSocialSujetoRetenido"].ToString().Trim().Replace("&amp:", "Y") : DataRowComprobanteRetencion["txRazonSocialSujetoRetenido"].ToString().Trim().Contains("&") ? DataRowComprobanteRetencion["txRazonSocialSujetoRetenido"].ToString().Trim().Replace("&", "&amp;") : DataRowComprobanteRetencion["txRazonSocialSujetoRetenido"].ToString().Trim();
                                InfoCompRetencionCR.periodoFiscal = DataRowComprobanteRetencion["txPeriodoFiscal"].ToString().Trim();
                                comprobanteReten.infoCompRetencion = InfoCompRetencionCR;
                                #endregion COMPROBANTE_RETENCION

                                #region DETALLE_COMPROBANTE
                                DataRow[] DataRows = DataRowComprobanteRetencion.GetChildRows("COMPROBANTES");
                                int cont = 0;
                                if (DataRows.LongLength > 0)
                                {
                                    impuesto_CR[] impuestos = new impuesto_CR[DataRows.LongLength];

                                    foreach (DataRow DataRowCompRetencionDetalle in DataRows)
                                    {
                                        codImpuestoDocSustento = DataRowCompRetencionDetalle["ciImpuesto"].ToString().Trim();
                                        baseImponibleSustento = DataRowCompRetencionDetalle["qnBaseImponible"].ToString().Trim();
                                        codigoPorcentajeSustento = DataRowCompRetencionDetalle["txCodRetencion"].ToString().Trim();
                                        numDocSustento = DataRowCompRetencionDetalle["txNumDocSustento"].ToString().Trim();
                                        #region INFORMACION_TABLA_COMPROBANTE_RETENCION_DETALLE
                                        string com = DataRowCompRetencionDetalle["ciCompania"].ToString();
                                        string secuencialDet = DataRowCompRetencionDetalle["txSecuencial"].ToString();
                                        #endregion INFORMACION_TABLA_COMPROBANTE_RETENCION_DETALLE

                                        impuesto_CR impuest = new impuesto_CR();
                                        impuest.codigo = DataRowCompRetencionDetalle["ciImpuesto"].ToString().Trim();
                                        impuest.codigoRetencion = DataRowCompRetencionDetalle["txCodRetencion"].ToString().Trim();
                                        impuest.baseImponible = (Decimal)DataRowCompRetencionDetalle["qnBaseImponible"];
                                        impuest.porcentajeRetener = (Decimal)DataRowCompRetencionDetalle["qnPorcentajeRetener"];
                                        impuest.valorRetenido = (Decimal)DataRowCompRetencionDetalle["qnValorRetenido"];
                                        impuest.codDocSustento = DataRowCompRetencionDetalle["txCodDocSustento"].ToString().Trim();
                                        impuest.numDocSustento = DataRowCompRetencionDetalle["txNumDocSustento"].ToString().Trim();
                                        if (DataRowCompRetencionDetalle["txFechaEmisionDocSustento"].ToString().Trim().Length != 0)
                                            impuest.fechaEmisionDocSustento = DataRowCompRetencionDetalle["txFechaEmisionDocSustento"].ToString().Trim();
                                        impuestos[cont] = impuest;
                                        cont++;
                                    }
                                }
                                #endregion DETALLE_COMPROBANTE

                                #region INFORMACION_ADICIONAL_COMPROBANTE
                                DataRow[] DataRowsDET = DataRowComprobanteRetencion.GetChildRows("COMPROBANTESINF");
                                if (DataRowsDET.LongLength > 0)
                                {
                                    comprobanteRetencionCampoAdicional[] camposAdicionalCR = new comprobanteRetencionCampoAdicional[DataRowsDET.LongLength];
                                    cont = 0;
                                    foreach (DataRow DataRowCompRetINF in DataRowsDET)
                                    {
                                        #region INFORMACION_TABLA_COMPROBANTE_RETENCION_INFORMACION_ADICIONAL
                                        #endregion INFORMACION_TABLA_COMPROBANTE_RETENCION_INFORMACION_ADICIONAL

                                        if (DataRowCompRetINF["txNombre"].ToString().Trim().Length != 0 && DataRowCompRetINF["txNombre"].ToString().Trim().Length != 0)
                                        {
                                            comprobanteRetencionCampoAdicional campoAdicionalCR = new comprobanteRetencionCampoAdicional();
                                            campoAdicionalCR.nombre = DataRowCompRetINF["txNombre"].ToString().Trim();
                                            campoAdicionalCR.Value = DataRowCompRetINF["txValor"].ToString().Trim().Contains("&amp;") ? DataRowCompRetINF["txValor"].ToString().Trim().Replace("&amp:", "Y") : DataRowCompRetINF["txValor"].ToString().Trim().Contains("&") ? DataRowCompRetINF["txValor"].ToString().Trim().Replace("&", "Y") : DataRowCompRetINF["txValor"].ToString().Trim();
                                            camposAdicionalCR[cont] = campoAdicionalCR;
                                            cont++;
                                        }
                                    }

                                    if (camposAdicionalCR.Count() != 0)
                                        comprobanteReten.infoAdicional = camposAdicionalCR;
                                }

                                cont = 0;
                                #endregion INFORMACION_ADICIONAL_COMPROBANTE


                                #region INFORMACION_TABLA_COMPROBANTE_RETENCION_DOCSSUSTENTO
                                DataRow[] DataRowsDOCS = DataRowComprobanteRetencion.GetChildRows("COMPROBANTESDOCS");
                                if (DataRowsDET.LongLength > 0)
                                {

                                    comprobanteRetencionImpuestosDocSustento[] impuestosDocSustento = new comprobanteRetencionImpuestosDocSustento[DataRowsDOCS.LongLength];
                                    comprobanteRetencionRetenciones[] retenciones = new comprobanteRetencionRetenciones[DataRowsDOCS.LongLength];
                                    comprobanteRetencionPagos[] pagos = new comprobanteRetencionPagos[DataRowsDOCS.LongLength];
                                    cont = 0;
                                    foreach (DataRow DataRowCompRetINF in DataRowsDOCS)
                                    {
                                        #region INFORMACION_TABLA_COMPROBANTE_RETENCION_DOCSSUSTENTO
                                        #endregion INFORMACION_TABLA_COMPROBANTE_RETENCION_DOCSSUSTENTO

                                        if (DataRowCompRetINF["txCodSustento"].ToString().Trim().Length != 0 && DataRowCompRetINF["txCodSustento"].ToString().Trim().Length != 0)
                                        {
                                            comprobanteRetencionDocSustento docSustentos = new comprobanteRetencionDocSustento();
                                            string codSustento = DataRowCompRetINF["txCodSustento"].ToString().Trim();
                                            if (codSustento.Length < 2)
                                            {
                                                codSustento = $"0{codSustento}";
                                            }
                                            docSustento.codSustento = codSustento;
                                            //docSustento.codSustento = DataRowCompRetINF["txCodSustento"].ToString().Trim();
                                            docSustento.codDocSustento = DataRowCompRetINF["txCodDocSustento"].ToString().Trim();
                                            docSustento.numDocSustento = numDocSustento;
                                            docSustento.pagoLocExt = "01";
                                            docSustento.fechaEmisionDocSustento = DataRowCompRetINF["txFechaEmision"].ToString().Trim();
                                            docSustento.fechaRegistroContable = DataRowCompRetINF["txFechaRegistroContable"].ToString().Trim();
                                            docSustento.totalSinImpuestos = DataRowCompRetINF["txTotalSinImpuesto"].ToString().Trim();
                                            docSustento.importeTotal = DataRowCompRetINF["txImporteTotal"].ToString().Trim();

                                            docsSustento.docSustento = docSustento;
                                            cont++;
                                        }
                                    }

                                    if (docSustento != null)
                                    {
                                        comprobanteReten.docsSustento = docsSustento;
                                    }

                                }

                                cont = 0;
                                #endregion
                                #region INFORMACION_TABLA_COMPROBANTE_RETENCION_DOCSUSTENTO_IMPUESTOS
                                DataRow[] DataRowsIMP = DataRowComprobanteRetencion.GetChildRows("COMPROBANTESDOCS");
                                if (DataRowsIMP.LongLength > 0)
                                {
                                    comprobanteRetencionImpuestosDocSustento[] impuestos = new comprobanteRetencionImpuestosDocSustento[DataRowsIMP.LongLength];
                                    cont = 0;
                                    foreach (DataRow DataRowCompRetINF in DataRowsIMP)
                                    {

                                        #region INFORMACION_TABLA_COMPROBANTE_RETENCION_DOCSUSTENTO_IMPUESTOS
                                        #endregion INFORMACION_TABLA_COMPROBANTE_RETENCION_DOCSUSTENTO_IMPUESTOS

                                        comprobanteRetencionImpuestosDocSustento impuesto = new comprobanteRetencionImpuestosDocSustento();
                                        impuesto.codImpuestoDocSustento = DataRowCompRetINF["txCodImpuestoDocSustento"].ToString().Trim();
                                        impuesto.codigoPorcentaje = DataRowCompRetINF["txCodigoPorcentaje"].ToString().Trim();
                                        impuesto.baseImponible = DataRowCompRetINF["txBaseImponible"].ToString().Trim();
                                        impuesto.tarifa = DataRowCompRetINF["txTarifa"].ToString().Trim();
                                        impuesto.valorImpuesto = DataRowCompRetINF["txValorImpuesto"].ToString().Trim();
                                        impuestos[cont] = impuesto;
                                        cont++;

                                    }

                                    if (impuestos.Count() != 0)
                                    {
                                        comprobanteReten.docsSustento.docSustento.impuestosDocSustento = impuestos;
                                    }

                                }

                                #endregion
                                #region INFORMACION_TABLA_COMPROBANTE_RETENCION_DOCSUSTENTO_RETENCION
                                DataRow[] DataRowsRET = DataRowComprobanteRetencion.GetChildRows("COMPROBANTES");

                                if (DataRowsRET.LongLength > 0)
                                {
                                    comprobanteRetencionRetenciones[] retenciones = new comprobanteRetencionRetenciones[DataRowsRET.LongLength];
                                    cont = 0;
                                    foreach (DataRow DataRowCompRetINF in DataRowsRET)
                                    {
                                        #region INFORMACION_TABLA_COMPROBANTE_RETENCION_DOCSUSTENTO_RETENCION
                                        #endregion INFORMACION_TABLA_COMPROBANTE_RETENCION_DOCSUSTENTO_RETENCION

                                        comprobanteRetencionRetenciones retencion = new comprobanteRetencionRetenciones();
                                        retencion.codigo = DataRowCompRetINF["ciImpuesto"].ToString().Trim();
                                        retencion.codigoRetencion = DataRowCompRetINF["txCodRetencion"].ToString().Trim();
                                        retencion.baseImponible = Convert.ToDecimal(DataRowCompRetINF["qnBaseImponible"].ToString().Trim());
                                        retencion.porcentajeRetener = Convert.ToDecimal(DataRowCompRetINF["qnPorcentajeRetener"].ToString().Trim());
                                        retencion.valorRetenido = Convert.ToDecimal(DataRowCompRetINF["qnValorRetenido"].ToString().Trim());
                                        retenciones[cont] = retencion;
                                        cont++;
                                    }

                                    if (retenciones.Count() != 0)
                                    {
                                        comprobanteReten.docsSustento.docSustento.retenciones = retenciones;
                                    }

                                }
                                #endregion
                                #region INFORMACION_TABLA_COMPROBANTE_RETENCION_DOCSUSTENTO_PAGOS

                                DataRow[] DataRowsPAGO = DataRowComprobanteRetencion.GetChildRows("COMPROBANTESPAGO");
                                if (DataRowsPAGO.LongLength > 0)
                                {
                                    comprobanteRetencionPagos[] pagos = new comprobanteRetencionPagos[DataRowsPAGO.LongLength];
                                    cont = 0;
                                    foreach (DataRow DataRowCompRetINF in DataRowsPAGO)
                                    {
                                        #region INFORMACION_TABLA_COMPROBANTE_RETENCION_DOCSUSTENTO_PAGOS
                                        #endregion INFORMACION_TABLA_COMPROBANTE_RETENCION_DOCSUSTENTO_PAGOS

                                        if (DataRowCompRetINF["txFormaPago"].ToString().Trim().Length != 0 && DataRowCompRetINF["txFormaPago"].ToString().Trim().Length != 0)
                                        {
                                            comprobanteRetencionPagos pagosCR = new comprobanteRetencionPagos();
                                            pagosCR.formaPago = DataRowCompRetINF["txFormaPago"].ToString().Trim();
                                            pagosCR.total = Convert.ToDecimal(DataRowCompRetINF["qnTotal"].ToString().Trim());
                                            pagos[cont] = pagosCR;
                                            cont++;
                                        }
                                    }

                                    if (pagos.Count() != 0)
                                    {
                                        comprobanteReten.docsSustento.docSustento.pagos = pagos;
                                    }

                                }
                                cont = 0;
                                #endregion

                                #region CLAVE_ACCESO
                                ClaveAcceso claveAcceso = new ClaveAcceso();
                                if (DataRowComprobanteRetencion["txClaveAcceso"].ToString() == "" || DataRowComprobanteRetencion["txClaveAcceso"].ToString() == null)
                                {
                                    #region Nuevo documento generacion de clave de acceso
                                    string ClaveAccesoNormal = claveAcceso.GenerarClaveAccesoDocumento(comprobanteReten.infoTributaria.codDoc,
                                                                                                       comprobanteReten.infoTributaria.secuencial.ToString().Trim(),
                                                                                                       comprobanteReten.infoTributaria.ptoEmi,
                                                                                                       comprobanteReten.infoTributaria.estab,
                                                                                                       comprobanteReten.infoCompRetencion.fechaEmision,
                                                                                                       comprobanteReten.infoTributaria.ruc,
                                                                                                       comprobanteReten.infoTributaria.ambiente);

                                    if (ClaveAccesoNormal.Length == 49)
                                    {
                                        comprobanteReten.infoTributaria.claveAcceso = ClaveAccesoNormal;
                                    }
                                    else
                                    {
                                        comprobanteReten.infoTributaria.claveAcceso = claveAcceso.GenerarClaveAccesoDocumento(comprobanteReten.infoTributaria.codDoc,
                                                                                                       comprobanteReten.infoTributaria.secuencial.ToString().Trim(),
                                                                                                       comprobanteReten.infoTributaria.ptoEmi,
                                                                                                       comprobanteReten.infoTributaria.estab,
                                                                                                       comprobanteReten.infoCompRetencion.fechaEmision,
                                                                                                       comprobanteReten.infoTributaria.ruc,
                                                                                                       comprobanteReten.infoTributaria.ambiente);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Documento ya contiene una clave de Acceso verifica si fue cambiada su fecha de emision
                                    if (comprobanteReten.infoCompRetencion.fechaEmision.Replace("/", "") != DataRowComprobanteRetencion["txClaveAcceso"].ToString().Substring(0, 8))
                                    {
                                        #region Nuevo documento generacion de su clave                               
                                        string ClaveAccesoNormal = claveAcceso.GenerarClaveAccesoDocumento(comprobanteReten.infoTributaria.codDoc,
                                                                                                       comprobanteReten.infoTributaria.secuencial.ToString().Trim(),
                                                                                                       comprobanteReten.infoTributaria.ptoEmi,
                                                                                                       comprobanteReten.infoTributaria.estab,
                                                                                                       comprobanteReten.infoCompRetencion.fechaEmision,
                                                                                                       comprobanteReten.infoTributaria.ruc,
                                                                                                       comprobanteReten.infoTributaria.ambiente);

                                        if (ClaveAccesoNormal.Length == 49)
                                        {
                                            comprobanteReten.infoTributaria.claveAcceso = ClaveAccesoNormal;
                                        }
                                        else
                                        {
                                            comprobanteReten.infoTributaria.claveAcceso = claveAcceso.GenerarClaveAccesoDocumento(comprobanteReten.infoTributaria.codDoc,
                                                                                                       comprobanteReten.infoTributaria.secuencial.ToString().Trim(),
                                                                                                       comprobanteReten.infoTributaria.ptoEmi,
                                                                                                       comprobanteReten.infoTributaria.estab,
                                                                                                       comprobanteReten.infoCompRetencion.fechaEmision,
                                                                                                       comprobanteReten.infoTributaria.ruc,
                                                                                                       comprobanteReten.infoTributaria.ambiente);
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                                #endregion CLAVE_ACCESO

                                #region AGREGA_EL_DOCUMENTO_GENERADO_A_LA_LISTA
                                xmlGenerado = new XmlGenerados();
                                xmlGenerado.Identity = Convert.ToInt32(DataRowComprobanteRetencion["ciCompRetencion"].ToString());
                                xmlGenerado.CiCompania = compania.CiCompania;
                                xmlGenerado.XmlComprobante = Serializacion.serializar(comprobanteReten);
                                xmlGenerado.ClaveAcceso = comprobanteReten.infoTributaria.claveAcceso;
                                xmlGenerado.NameXml = comprobanteReten.infoTributaria.claveAcceso;
                                xmlGenerado.CiTipoEmision = comprobanteReten.infoTributaria.tipoEmision;
                                xmlGenerado.CiTipoDocumento = comprobanteReten.infoTributaria.codDoc;
                                xmlGenerado.XmlEstado = CatalogoViaDoc.DocEstadoGenerado;
                                xmlGenerado.ciNumeroIntento = numeroIntentosCompRetencion;
                                xmlCompRetencion.Add(xmlGenerado);
                                #endregion AGREGA_EL_DOCUMENTO_GENERADO_A_LA_LISTA
                            }
                            else
                            {
                                if (DataRowComprobanteRetencion["xmlDocumentoAutorizado"].ToString() != "" && DataRowComprobanteRetencion["xmlDocumentoAutorizado"].ToString() != null)
                                {
                                    comprobanteReten = (comprobanteRetencion)Serializacion.desSerializar(DataRowComprobanteRetencion["xmlDocumentoAutorizado"].ToString(), comprobanteReten.GetType());
                                    xmlGenerado = new XmlGenerados();
                                    xmlGenerado.Identity = Convert.ToInt32(DataRowComprobanteRetencion["ciCompRetencion"].ToString().Trim());
                                    xmlGenerado.CiCompania = compania.CiCompania;
                                    xmlGenerado.XmlComprobante = DataRowComprobanteRetencion["xmlDocumentoAutorizado"].ToString().Trim();
                                    xmlGenerado.ClaveAcceso = comprobanteReten.infoTributaria.claveAcceso;
                                    xmlGenerado.NameXml = comprobanteReten.infoTributaria.claveAcceso;
                                    xmlGenerado.CiTipoEmision = comprobanteReten.infoTributaria.tipoEmision;
                                    xmlGenerado.CiTipoDocumento = comprobanteReten.infoTributaria.codDoc;
                                    xmlGenerado.CiContingenciaDet = Convert.ToInt32(DataRowComprobanteRetencion["ciContingenciaDet"].ToString().Trim());
                                    xmlGenerado.XmlEstado = CatalogoViaDoc.DocEstadoGenerado;
                                    xmlCompRetencion.Add(xmlGenerado);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            xmlGenerado = new XmlGenerados();
                            xmlGenerado.Identity = Convert.ToInt32(DataRowComprobanteRetencion["ciCompRetencion"].ToString().Trim());
                            xmlGenerado.MensajeError = ex.Message + "||||" + ex.StackTrace;
                            xmlGenerado.XmlEstado = CatalogoViaDoc.DocEstadoEFirmado;
                            xmlGenerado.txCodError = "101";
                            xmlGenerado.CiTipoDocumento = CatalogoViaDoc.DocumentoFactura;
                            xmlGenerado.CiCompania = compania.CiCompania;
                            xmlGenerado.CiContingenciaDet = 1;
                            xmlGenerado.XmlComprobante = Serializacion.serializar(comprobanteReten);
                            xmlGenerado.NameXml = comprobanteReten.infoTributaria.claveAcceso;
                            xmlGenerado.ClaveAcceso = comprobanteReten.infoTributaria.claveAcceso;
                            xmlGenerado.ciNumeroIntento = numeroIntentosCompRetencion + 1;
                            actualizarEstado.ActualizarXmlComprobantes(xmlGenerado);

                            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Exception 2: " + ex.ToString());
                        }
                    }
                    #endregion INFORMACION_TRIBUTARIA
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Exception: " + ex.ToString());
            }
            return xmlCompRetencion;
        }
    }
}

