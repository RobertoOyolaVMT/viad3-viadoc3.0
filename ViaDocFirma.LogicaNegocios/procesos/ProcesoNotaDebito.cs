using eSign;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using ViaDoc.Configuraciones;
using ViaDoc.EntidadNegocios;
using ViaDoc.Utilitarios;

namespace ViaDocFirma.LogicaNegocios.procesos
{
    public class ProcesoNotaDebito
    {
        public List<XmlGenerados> ProcesarXmlNotaDebito(Compania compania, string Version, int numeroRegistro)
        {
            int contInicial = 0;
            int codigoRetorno = 0;
            int numeroIntentosNotaDebito = 0;
            string descripcionRetorno = string.Empty;
            List<XmlGenerados> xmlNotaDebito = new List<XmlGenerados>();
            XmlGenerados xmlGenerado = null;
            ViaDoc.AccesoDatos.DocumentoAD _consultaDocumentos = new ViaDoc.AccesoDatos.DocumentoAD();

            try
            {
                DataSet dsNotaDebito = _consultaDocumentos.DocumentosElectronicos(7, compania.CiCompania, "", "", "", "", numeroRegistro, "", ref codigoRetorno, ref descripcionRetorno);

                if (codigoRetorno.Equals(0))
                {
                    contInicial = dsNotaDebito.Tables[0].Select("ciEstado='" + CatalogoViaDoc.DocEstadoInicial + "'").ToList().Count;
                    if (contInicial == 0)
                        contInicial = dsNotaDebito.Tables[0].Select("ciEstado='" + CatalogoViaDoc.DocEstadoEFirmado + "'").ToList().Count;

                    #region RELACIONO_TABLAS_NOTA_DEBITO
                    dsNotaDebito.Tables[0].TableName = "NotaDebito";
                    dsNotaDebito.Tables[1].TableName = "NotaDebitoInfoAdicional";
                    dsNotaDebito.Tables[2].TableName = "NotaDebitoMotivo";
                    dsNotaDebito.Tables[3].TableName = "NotaDebitoTotalImpuesto";

                    DataColumn[] PkColumnsNotaDebito = new DataColumn[4];
                    DataColumn[] FkColumnsNotaDebitoInfoAdicional = new DataColumn[4];
                    DataColumn[] FkColumnsNotaDebitoMotivo = new DataColumn[4];
                    DataColumn[] FkColumnsNotaDebitoImpuesto = new DataColumn[4];
                    
                    //NOTA_DE_DEBITO
                    PkColumnsNotaDebito[0] = dsNotaDebito.Tables["NotaDebito"].Columns["ciCompania"];
                    PkColumnsNotaDebito[1] = dsNotaDebito.Tables["NotaDebito"].Columns["txEstablecimiento"];
                    PkColumnsNotaDebito[2] = dsNotaDebito.Tables["NotaDebito"].Columns["txPuntoEmision"];
                    PkColumnsNotaDebito[3] = dsNotaDebito.Tables["NotaDebito"].Columns["txSecuencial"];
                    
                    // NOTA_DEBITO_INFO_ADICIONAL
                    FkColumnsNotaDebitoInfoAdicional[0] = dsNotaDebito.Tables["NotaDebitoInfoAdicional"].Columns["ciCompania"];
                    FkColumnsNotaDebitoInfoAdicional[1] = dsNotaDebito.Tables["NotaDebitoInfoAdicional"].Columns["txEstablecimiento"];
                    FkColumnsNotaDebitoInfoAdicional[2] = dsNotaDebito.Tables["NotaDebitoInfoAdicional"].Columns["txPuntoEmision"];
                    FkColumnsNotaDebitoInfoAdicional[3] = dsNotaDebito.Tables["NotaDebitoInfoAdicional"].Columns["txSecuencial"];
                    
                    // NOTA_DEBITO_MOTIVO
                    FkColumnsNotaDebitoMotivo[0] = dsNotaDebito.Tables["NotaDebitoMotivo"].Columns["ciCompania"];
                    FkColumnsNotaDebitoMotivo[1] = dsNotaDebito.Tables["NotaDebitoMotivo"].Columns["txEstablecimiento"];
                    FkColumnsNotaDebitoMotivo[2] = dsNotaDebito.Tables["NotaDebitoMotivo"].Columns["txPuntoEmision"];
                    FkColumnsNotaDebitoMotivo[3] = dsNotaDebito.Tables["NotaDebitoMotivo"].Columns["txSecuencial"];
                    
                    // NOTA_DEBITO_IMPUESTO
                    FkColumnsNotaDebitoImpuesto[0] = dsNotaDebito.Tables["NotaDebitoTotalImpuesto"].Columns["ciCompania"];
                    FkColumnsNotaDebitoImpuesto[1] = dsNotaDebito.Tables["NotaDebitoTotalImpuesto"].Columns["txEstablecimiento"];
                    FkColumnsNotaDebitoImpuesto[2] = dsNotaDebito.Tables["NotaDebitoTotalImpuesto"].Columns["txPuntoEmision"];
                    FkColumnsNotaDebitoImpuesto[3] = dsNotaDebito.Tables["NotaDebitoTotalImpuesto"].Columns["txSecuencial"];
                    

                    dsNotaDebito.Relations.Add("NotaDebitoInfoAdicional", PkColumnsNotaDebito, FkColumnsNotaDebitoInfoAdicional);
                    dsNotaDebito.Relations.Add("NotaDebitoMotivo", PkColumnsNotaDebito, FkColumnsNotaDebitoMotivo);
                    dsNotaDebito.Relations.Add("NotaDebitoImpuesto", PkColumnsNotaDebito, FkColumnsNotaDebitoImpuesto);
                    #endregion RELACIONO_TABLAS_NOTA_DEBITO

                    notaDebito NotaDebito = new notaDebito();
                    #region INFORMACION_TRIBUTARIA
                    NotaDebito.id = notaDebitoID.comprobante;
                    NotaDebito.idSpecified = true;
                    NotaDebito.version = Version;

                    infoTributaria_ND objinfoTributariaND = new infoTributaria_ND();
                    objinfoTributariaND.ambiente = compania.CiTipoAmbiente.ToString();
                    objinfoTributariaND.dirMatriz = compania.TxDireccionMatriz; //proviene de Compania
                    objinfoTributariaND.nombreComercial = compania.TxNombreComercial;  //proviene de Compania 
                    objinfoTributariaND.razonSocial = compania.TxRazonSocial;  //proviene de Compania
                    objinfoTributariaND.ruc = compania.TxRuc;   //proviene de Compania

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
                                            objinfoTributariaND.agenteRetencion = CatalogoViaDoc.LeyendaAgente.Trim().Replace("Agente de Retención Resolución Nro ", "");
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
                            string[] Cadena = ConfigurationManager.AppSettings.Get("contribuyente_Microempresa").Split('|');
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
                                                objinfoTributariaND.regimenMicroempresas = CatalogoViaDoc.LeyendaRegimen.Trim();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            objinfoTributariaND.regimenMicroempresas = string.Empty; 
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
                                                objinfoTributariaND.contribuyenteRimpe = CatalogoViaDoc.LeyendaRegimenRimpe.Trim();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            objinfoTributariaND.contribuyenteRimpe = string.Empty;
                        }

                    }
                    #endregion

                    notaDebitoInfoNotaDebito objinfoNotaDebitoND = new notaDebitoInfoNotaDebito();

                    if (compania.TxContribuyenteEspecial.Trim().Length != 0)
                        objinfoNotaDebitoND.contribuyenteEspecial = compania.TxContribuyenteEspecial; //proviene de Compania

                    if (compania.TxObligadoContabilidad == "N")
                        objinfoNotaDebitoND.obligadoContabilidad = "NO"; //proviene de Compania
                    #endregion INFORMACION_TRIBUTARIA

                    foreach (DataRow dtrNotaDebito in dsNotaDebito.Tables["NotaDebito"].Rows)
                    {
                        
                        int cont = 0;
                        string claveAccesoGenerada = "";
                        string xmlNotaDebitoGenerado = "";
                        string tipoDocumento = "";
                        try
                        {
                            if (dtrNotaDebito["ciEstado"].ToString().Trim() == CatalogoViaDoc.DocEstadoInicial
                                || dtrNotaDebito["ciEstado"].ToString().Trim() == CatalogoViaDoc.DocEstadoEFirmado )
                            {
                                
                                numeroIntentosNotaDebito = int.Parse(dtrNotaDebito["ciNumeroIntento"].ToString().Trim());
                                #region NOTA_DEBITO
                                objinfoTributariaND.claveAcceso = dtrNotaDebito["txClaveAcceso"].ToString();
                                objinfoTributariaND.estab = dtrNotaDebito["txEstablecimiento"].ToString();
                                objinfoTributariaND.ptoEmi = dtrNotaDebito["txPuntoEmision"].ToString();
                                objinfoTributariaND.secuencial = dtrNotaDebito["txSecuencial"].ToString();
                                objinfoTributariaND.tipoEmision = dtrNotaDebito["ciTipoEmision"].ToString();
                                objinfoTributariaND.codDoc = dtrNotaDebito["ciTipoDocumento"].ToString();
                                NotaDebito.infoTributaria = objinfoTributariaND;

                                objinfoNotaDebitoND.dirEstablecimiento = dtrNotaDebito["txDireccion"].ToString();
                                objinfoNotaDebitoND.fechaEmision = dtrNotaDebito["txFechaEmision"].ToString();
                                objinfoNotaDebitoND.tipoIdentificacionComprador = dtrNotaDebito["ciTipoIdentificacionComprador"].ToString();
                                objinfoNotaDebitoND.razonSocialComprador = dtrNotaDebito["txRazonSocialComprador"].ToString().Trim().Contains("&amp;") ?
                                    //dtrNotaDebito["txRazonSocialComprador"].ToString().Trim().Replace("&amp:", "Y") : dtrNotaDebito["txRazonSocialComprador"].ToString().Trim().Contains("&") ?
                                    dtrNotaDebito["txRazonSocialComprador"].ToString().Trim().Replace("&", "&amp;") : dtrNotaDebito["txRazonSocialComprador"].ToString().Trim();
                                objinfoNotaDebitoND.identificacionComprador = dtrNotaDebito["txIdentificacionComprador"].ToString();
                                if (dtrNotaDebito["txRise"].ToString() != "" && dtrNotaDebito["txRise"].ToString() != null)
                                    objinfoNotaDebitoND.rise = dtrNotaDebito["txRise"].ToString();
                                objinfoNotaDebitoND.codDocModificado = dtrNotaDebito["ciTipoDocumentoModificado"].ToString();
                                objinfoNotaDebitoND.numDocModificado = dtrNotaDebito["txNumeroDocumentoModificado"].ToString();
                                objinfoNotaDebitoND.fechaEmisionDocSustento = dtrNotaDebito["txFechaEmisionDocumentoModificado"].ToString();
                                objinfoNotaDebitoND.totalSinImpuestos = Convert.ToDecimal(dtrNotaDebito["qnTotalSinImpuestos"].ToString());
                                objinfoNotaDebitoND.valorTotal = Convert.ToDecimal(dtrNotaDebito["qnValorTotal"].ToString());
                                NotaDebito.infoNotaDebito = objinfoNotaDebitoND;
                                #endregion NOTA_DEBITO

                                #region IMPUESTO
                                DataRow[] dtrArryImpuesto = dtrNotaDebito.GetChildRows("NotaDebitoImpuesto");
                                if (dtrArryImpuesto.LongLength > 0)
                                {
                                    impuesto_ND[] objImpuestos = new impuesto_ND[dtrArryImpuesto.LongLength];
                                    cont = 0;
                                    foreach (DataRow DataRowDetalleImpuesto in dtrArryImpuesto)
                                    {
                                        impuesto_ND objImpuest = new impuesto_ND();
                                        objImpuest.codigo = DataRowDetalleImpuesto["txCodigo"].ToString();
                                        objImpuest.codigoPorcentaje = DataRowDetalleImpuesto["txCodigoPorcentaje"].ToString();
                                        objImpuest.tarifa = Convert.ToDecimal(DataRowDetalleImpuesto["txTarifa"].ToString());
                                        objImpuest.baseImponible = Convert.ToDecimal(DataRowDetalleImpuesto["qnBaseImponible"].ToString());
                                        objImpuest.valor = Convert.ToDecimal(DataRowDetalleImpuesto["qnValor"].ToString());
                                        objImpuestos[cont] = objImpuest;
                                        cont++;
                                    }
                                    NotaDebito.infoNotaDebito.impuestos = objImpuestos;
                                }
                                cont = 0;
                                #endregion IMPUESTO

                                #region MOTIVO
                                DataRow[] dtrArryMotivo = dtrNotaDebito.GetChildRows("NotaDebitoMotivo");
                                if (dtrArryMotivo.LongLength > 0)
                                {

                                    notaDebitoMotivosMotivo[] objMotivosMotivos = new notaDebitoMotivosMotivo[dtrArryMotivo.LongLength];
                                    notaDebitoMotivos objMotivos = new notaDebitoMotivos();
                                    cont = 0;
                                    foreach (DataRow dtrNotaDebitoMotivo in dtrArryMotivo)
                                    {

                                        notaDebitoMotivosMotivo objMotivosMotivo = new notaDebitoMotivosMotivo();
                                        objMotivosMotivo.razon = dtrNotaDebitoMotivo["txRazon"].ToString().Trim().Contains("&amp;") ?
                                            dtrNotaDebitoMotivo["txRazon"].ToString().Trim().Replace("&amp:", "Y") : dtrNotaDebitoMotivo["txRazon"].ToString().Trim().Contains("&") ?
                                            dtrNotaDebitoMotivo["txRazon"].ToString().Trim().Replace("&", "Y") : dtrNotaDebitoMotivo["txRazon"].ToString().Trim();
                                        objMotivosMotivo.valor = Convert.ToDecimal(dtrNotaDebitoMotivo["qnValor"].ToString());
                                        objMotivosMotivos[cont] = objMotivosMotivo;
                                        cont++;
                                    }
                                    objMotivos.motivo = objMotivosMotivos;
                                    NotaDebito.motivos = objMotivos;
                                }
                                cont = 0;
                                #endregion MOTIVO


                                #region INFORMACION_ADICIONAL
                                DataRow[] dtrArrayInfoAdicional = dtrNotaDebito.GetChildRows("NotaDebitoInfoAdicional");
                                if (dtrArrayInfoAdicional.LongLength > 0)
                                {
                                    notaDebitoCampoAdicional[] objInfoAdicional = new notaDebitoCampoAdicional[dtrArrayInfoAdicional.LongLength];
                                    cont = 0;
                                    foreach (DataRow dtrNotaDebitoINF in dtrArrayInfoAdicional)
                                    {
                                        notaDebitoCampoAdicional campoAdicionalND = new notaDebitoCampoAdicional();
                                        campoAdicionalND.nombre = dtrNotaDebitoINF["txNombre"].ToString();
                                        campoAdicionalND.Value = dtrNotaDebitoINF["txValor"].ToString().Trim().Contains("&amp;") ?
                                            dtrNotaDebitoINF["txValor"].ToString().Trim().Replace("&amp:", "Y") : dtrNotaDebitoINF["txValor"].ToString().Trim().Contains("&") ?
                                            dtrNotaDebitoINF["txValor"].ToString().Trim().Replace("&", "Y") : dtrNotaDebitoINF["txValor"].ToString().Trim();

                                        objInfoAdicional[cont] = campoAdicionalND;
                                        cont++;
                                    }
                                    NotaDebito.infoAdicional = objInfoAdicional;
                                }
                                cont = 0;
                                #endregion INFORMACION_ADICIONAL

                                #region CLAVE_ACCESO
                                ClaveAcceso claveAcceso = new ClaveAcceso();
                                if (dtrNotaDebito["txClaveAcceso"].ToString() == "" || dtrNotaDebito["txClaveAcceso"].ToString() == null)
                                {
                                    #region Nuevo documento generacion de su clave                               
                                    string ClaveAccesoNormal = claveAcceso.GenerarClaveAccesoDocumento(NotaDebito.infoTributaria.codDoc,
                                                                                                       NotaDebito.infoTributaria.secuencial.ToString().Trim(),
                                                                                                       NotaDebito.infoTributaria.ptoEmi,
                                                                                                       NotaDebito.infoTributaria.estab,
                                                                                                       NotaDebito.infoNotaDebito.fechaEmision,
                                                                                                       NotaDebito.infoTributaria.ruc,
                                                                                                       NotaDebito.infoTributaria.ambiente);

                                    if (ClaveAccesoNormal.Length == 49)
                                    {
                                        NotaDebito.infoTributaria.claveAcceso = ClaveAccesoNormal;
                                    }
                                    else
                                    {
                                        NotaDebito.infoTributaria.claveAcceso = claveAcceso.GenerarClaveAccesoDocumento(NotaDebito.infoTributaria.codDoc,
                                                                                                                        NotaDebito.infoTributaria.secuencial.ToString().Trim(),
                                                                                                                        NotaDebito.infoTributaria.ptoEmi,
                                                                                                                        NotaDebito.infoTributaria.estab,
                                                                                                                        NotaDebito.infoNotaDebito.fechaEmision,
                                                                                                                        NotaDebito.infoTributaria.ruc,
                                                                                                                        NotaDebito.infoTributaria.ambiente);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Documento ya contiene una clave de Acceso
                                    if (NotaDebito.infoNotaDebito.fechaEmision.Replace("/", "") != dtrNotaDebito["txClaveAcceso"].ToString().Substring(0, 8))
                                    {
                                        #region Nuevo documento generacion de su clave                               
                                        string ClaveAccesoNormal = claveAcceso.GenerarClaveAccesoDocumento(NotaDebito.infoTributaria.codDoc,
                                                                                                           NotaDebito.infoTributaria.secuencial.ToString().Trim(),
                                                                                                           NotaDebito.infoTributaria.ptoEmi,
                                                                                                           NotaDebito.infoTributaria.estab,
                                                                                                           NotaDebito.infoNotaDebito.fechaEmision,
                                                                                                           NotaDebito.infoTributaria.ruc,
                                                                                                           NotaDebito.infoTributaria.ambiente);

                                        if (ClaveAccesoNormal.Length == 49)
                                        {
                                            NotaDebito.infoTributaria.claveAcceso = ClaveAccesoNormal;
                                        }
                                        else
                                        {
                                            NotaDebito.infoTributaria.claveAcceso = claveAcceso.GenerarClaveAccesoDocumento(NotaDebito.infoTributaria.codDoc,
                                                                                                                            NotaDebito.infoTributaria.secuencial.ToString().Trim(),
                                                                                                                            NotaDebito.infoTributaria.ptoEmi,
                                                                                                                            NotaDebito.infoTributaria.estab,
                                                                                                                            NotaDebito.infoNotaDebito.fechaEmision,
                                                                                                                            NotaDebito.infoTributaria.ruc,
                                                                                                                            NotaDebito.infoTributaria.ambiente);
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                                #endregion CLAVE_ACCESO

                                #region AGREGA_EL_DOCUMENTO_GENERADO_A_LA_LISTA
                                xmlGenerado = new XmlGenerados();
                                xmlGenerado.Identity = Convert.ToInt32(dtrNotaDebito["ciNotaDebito"].ToString());
                                xmlGenerado.CiCompania = compania.CiCompania;
                                xmlGenerado.XmlComprobante = Serializacion.serializar(NotaDebito);
                                xmlGenerado.ClaveAcceso = NotaDebito.infoTributaria.claveAcceso;
                                xmlGenerado.NameXml = NotaDebito.infoTributaria.claveAcceso;
                                xmlGenerado.CiTipoEmision = NotaDebito.infoTributaria.tipoEmision;
                                xmlGenerado.CiTipoDocumento = NotaDebito.infoTributaria.codDoc;
                                xmlGenerado.XmlEstado = CatalogoViaDoc.DocEstadoGenerado;
                                xmlGenerado.ciNumeroIntento = numeroIntentosNotaDebito;


                                xmlNotaDebito.Add(xmlGenerado);
                                #endregion AGREGA_EL_DOCUMENTO_GENERADO_A_LA_LISTA
                            }
                            else
                            {
                                #region AGREGA_EL_DOCUMENTO_GENERADO_POR_CONTINGENCIA_A_LA_LISTA
                                NotaDebito = (notaDebito)Serializacion.desSerializar(dtrNotaDebito["xmlDocumentoAutorizado"].ToString(), NotaDebito.GetType());
                                xmlGenerado = new XmlGenerados();
                                xmlGenerado.Identity = Convert.ToInt32(dtrNotaDebito["ciNotaDebito"].ToString());
                                xmlGenerado.CiCompania = compania.CiCompania;
                                xmlGenerado.XmlComprobante = Serializacion.serializar(NotaDebito);
                                xmlGenerado.ClaveAcceso = NotaDebito.infoTributaria.claveAcceso;
                                xmlGenerado.NameXml = NotaDebito.infoTributaria.claveAcceso;
                                xmlGenerado.CiTipoEmision = NotaDebito.infoTributaria.tipoEmision;
                                xmlGenerado.CiTipoDocumento = NotaDebito.infoTributaria.codDoc;
                                xmlGenerado.CiContingenciaDet = Convert.ToInt32(dtrNotaDebito["ciContingenciaDet"].ToString());
                                xmlGenerado.XmlEstado = CatalogoViaDoc.DocEstadoGenerado;
                                xmlGenerado.ciNumeroIntento = numeroIntentosNotaDebito;
                                xmlNotaDebito.Add(xmlGenerado);
                                #endregion AGREGA_EL_DOCUMENTO_GENERADO_POR_CONTINGENCIA_A_LA_LISTA
                            }
                        }
                        catch (Exception ex)
                        {
                            xmlGenerado = new XmlGenerados();
                            xmlGenerado.Identity = Convert.ToInt32(dtrNotaDebito["ciNotaDebito"].ToString());
                            xmlGenerado.MensajeError = ex.Message + "||||" + ex.StackTrace;
                            xmlGenerado.XmlEstado = CatalogoViaDoc.DocEstadoEFirmado;
                            xmlGenerado.txCodError = "101";
                            xmlGenerado.CiTipoDocumento = CatalogoViaDoc.DocumentoFactura;
                            xmlGenerado.CiCompania = compania.CiCompania;
                            xmlGenerado.CiContingenciaDet = 1;
                            xmlGenerado.XmlComprobante = Serializacion.serializar(NotaDebito);
                            xmlGenerado.NameXml = NotaDebito.infoTributaria.claveAcceso;
                            xmlGenerado.ClaveAcceso = NotaDebito.infoTributaria.claveAcceso;
                            xmlGenerado.ciNumeroIntento = numeroIntentosNotaDebito + 1;
                            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("ProcesosFirma - Facturas: " + ex.Message);
                            FirmaDocumentos actualizarEstado = new FirmaDocumentos();
                            actualizarEstado.ActualizarXmlComprobantes(xmlGenerado);
                        }
                    }
                    dsNotaDebito.Dispose();
                }
            }
            catch (Exception ex)
            {

            }
            return xmlNotaDebito;
        }
    }
}
