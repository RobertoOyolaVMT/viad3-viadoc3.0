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
    public class ProcesoNotaCredito
    {
        public List<XmlGenerados> ProcesarXmlNotaCredito(Compania compania, string Version, int numeroRegistro)
        {
            int contInicial = 0;
            int codigoRetorno = 0;
            string descripcionRetorno = string.Empty;
            List<XmlGenerados> xmlNotaCredito = new List<XmlGenerados>();
            XmlGenerados xmlGenerados = null;
            int numeroIntentosNotaCredito = 0;
            ViaDoc.AccesoDatos.DocumentoAD _consultaDocumentos = new ViaDoc.AccesoDatos.DocumentoAD();

            try
            {
                DataSet dsNotaCredito = _consultaDocumentos.DocumentosElectronicos(5, compania.CiCompania, "", "", "", "", numeroRegistro, "", ref codigoRetorno, ref descripcionRetorno);
                if (codigoRetorno.Equals(0))
                {
                    contInicial = dsNotaCredito.Tables[0].Select("ciEstado='" + CatalogoViaDoc.DocEstadoInicial + "'").ToList().Count;
                    if (contInicial == 0)
                        contInicial = dsNotaCredito.Tables[0].Select("ciEstado='" + CatalogoViaDoc.DocEstadoEFirmado + "'").ToList().Count;

                    dsNotaCredito.Tables[0].TableName = "NotaCredito";
                    dsNotaCredito.Tables[1].TableName = "NotaCreditoInfoAdicional";
                    dsNotaCredito.Tables[2].TableName = "NotaCreditoTotalImpuesto";
                    dsNotaCredito.Tables[3].TableName = "NotaCreditoDetalle";

                    #region RELACIONO_TABLAS_NotaCredito
                    DataColumn[] Pk_NotaCredito = new DataColumn[4];
                    DataColumn[] Fk_NotaCreditoInfoAdicional = new DataColumn[4];
                    DataColumn[] Fk_NotaCreditoTotalImpuesto = new DataColumn[4];
                    DataColumn[] Fk_NotaCreditoDetalle = new DataColumn[4];

                    //NotaCredito
                    Pk_NotaCredito[0] = dsNotaCredito.Tables["NotaCredito"].Columns["ciCompania"];
                    Pk_NotaCredito[1] = dsNotaCredito.Tables["NotaCredito"].Columns["txEstablecimiento"];
                    Pk_NotaCredito[2] = dsNotaCredito.Tables["NotaCredito"].Columns["txPuntoEmision"];
                    Pk_NotaCredito[3] = dsNotaCredito.Tables["NotaCredito"].Columns["txSecuencial"];

                    //NotaCreditoInfoAdicional
                    Fk_NotaCreditoInfoAdicional[0] = dsNotaCredito.Tables["NotaCreditoInfoAdicional"].Columns["ciCompania"];
                    Fk_NotaCreditoInfoAdicional[1] = dsNotaCredito.Tables["NotaCreditoInfoAdicional"].Columns["txEstablecimiento"];
                    Fk_NotaCreditoInfoAdicional[2] = dsNotaCredito.Tables["NotaCreditoInfoAdicional"].Columns["txPuntoEmision"];
                    Fk_NotaCreditoInfoAdicional[3] = dsNotaCredito.Tables["NotaCreditoInfoAdicional"].Columns["txSecuencial"];

                    //NotaCreditoTotalImpuesto
                    Fk_NotaCreditoTotalImpuesto[0] = dsNotaCredito.Tables["NotaCreditoTotalImpuesto"].Columns["ciCompania"];
                    Fk_NotaCreditoTotalImpuesto[1] = dsNotaCredito.Tables["NotaCreditoTotalImpuesto"].Columns["txEstablecimiento"];
                    Fk_NotaCreditoTotalImpuesto[2] = dsNotaCredito.Tables["NotaCreditoTotalImpuesto"].Columns["txPuntoEmision"];
                    Fk_NotaCreditoTotalImpuesto[3] = dsNotaCredito.Tables["NotaCreditoTotalImpuesto"].Columns["txSecuencial"];

                    //NotaCreditoDetalle fk
                    Fk_NotaCreditoDetalle[0] = dsNotaCredito.Tables["NotaCreditoDetalle"].Columns["ciCompania"];
                    Fk_NotaCreditoDetalle[1] = dsNotaCredito.Tables["NotaCreditoDetalle"].Columns["txEstablecimiento"];
                    Fk_NotaCreditoDetalle[2] = dsNotaCredito.Tables["NotaCreditoDetalle"].Columns["txPuntoEmision"];
                    Fk_NotaCreditoDetalle[3] = dsNotaCredito.Tables["NotaCreditoDetalle"].Columns["txSecuencial"];

                    dsNotaCredito.Relations.Add("NotaCredito_NotaCreditoInfoAdicional", Pk_NotaCredito, Fk_NotaCreditoInfoAdicional);
                    dsNotaCredito.Relations.Add("NotaCredito_NotaCreditoTotalImpuesto", Pk_NotaCredito, Fk_NotaCreditoTotalImpuesto);
                    dsNotaCredito.Relations.Add("NotaCredito_NotaCreditoDetalle", Pk_NotaCredito, Fk_NotaCreditoDetalle);
                    #endregion RELACIONO_TABLAS_NotaCredito

                    #region LLena clases NotaCreditoS

                    #region INFORMACION_TRIBUTARIA
                    notaCredito NotaCredito = new notaCredito();
                    NotaCredito.id = notaCreditoID.comprobante;
                    NotaCredito.version = Version;
                    infoTributaria_NC infoTributariaNC = new infoTributaria_NC();

                    infoTributariaNC.ambiente = compania.CiTipoAmbiente.ToString();
                    infoTributariaNC.dirMatriz = compania.TxDireccionMatriz; //proviene de Compania

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
                                            infoTributariaNC.agenteRetencion = CatalogoViaDoc.LeyendaAgente.Trim().Replace("Agente de Retención Resolución Nro ", "");
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
                                                infoTributariaNC.regimenMicroempresas = CatalogoViaDoc.LeyendaRegimen.Trim();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            infoTributariaNC.regimenMicroempresas = string.Empty;
                        }
                        
                    }
                    #endregion
                    #region Regimen Rimpe
                    //Regimen Rimpe
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
                                                infoTributariaNC.contribuyenteRimpe = CatalogoViaDoc.LeyendaRegimenRimpe.Trim();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            infoTributariaNC.contribuyenteRimpe = string.Empty;
                        }

                    }
                    #endregion
                    infoTributariaNC.nombreComercial = compania.TxNombreComercial;  //proviene de Compania 
                    infoTributariaNC.razonSocial = compania.TxRazonSocial;  //proviene de Compania
                    infoTributariaNC.ruc = compania.TxRuc;   //proviene de Compania

                    notaCreditoInfoNotaCredito NotaCreditoInfoNotaCredito = new notaCreditoInfoNotaCredito();
                    if (compania.TxObligadoContabilidad == "S")
                        NotaCreditoInfoNotaCredito.obligadoContabilidad = "SI"; //proviene de Compania

                    if (compania.TxObligadoContabilidad == "N")
                        NotaCreditoInfoNotaCredito.obligadoContabilidad = "NO"; //proviene de Compania
                    #endregion INFORMACION_TRIBUTARIA

                    foreach (DataRow DataRowNotaCredito in dsNotaCredito.Tables["NotaCredito"].Rows)
                    {
                        string claveAccesoGenerada = "";
                        string xmlNotaCreditoGenerado = "";
                        string tipoDocumento = "";
                        try
                        {
                            if (DataRowNotaCredito["ciEstado"].ToString().Trim() == CatalogoViaDoc.DocEstadoInicial ||
                                DataRowNotaCredito["ciEstado"].ToString().Trim() == CatalogoViaDoc.DocEstadoEFirmado)
                            {
                                #region NotaCredito
                                infoTributariaNC.claveAcceso = DataRowNotaCredito["txClaveAcceso"].ToString().Trim();
                                infoTributariaNC.estab = DataRowNotaCredito["txEstablecimiento"].ToString().Trim();
                                infoTributariaNC.ptoEmi = DataRowNotaCredito["txPuntoEmision"].ToString().Trim();
                                infoTributariaNC.secuencial = DataRowNotaCredito["txSecuencial"].ToString().Trim();
                                infoTributariaNC.tipoEmision = DataRowNotaCredito["ciTipoEmision"].ToString().Trim();
                                infoTributariaNC.codDoc = DataRowNotaCredito["ciTipoDocumento"].ToString().Trim();
                                NotaCredito.infoTributaria = infoTributariaNC;

                                NotaCreditoInfoNotaCredito.fechaEmision = DataRowNotaCredito["txFechaEmision"].ToString().Trim();
                                NotaCreditoInfoNotaCredito.dirEstablecimiento = DataRowNotaCredito["txDireccion"].ToString().Trim();

                                if (compania.TxContribuyenteEspecial.ToString().Trim() != "" && compania.TxContribuyenteEspecial.ToString().Trim() != null && compania.TxContribuyenteEspecial.Length > 0)
                                    NotaCreditoInfoNotaCredito.contribuyenteEspecial = compania.TxContribuyenteEspecial;

                                NotaCreditoInfoNotaCredito.tipoIdentificacionComprador = DataRowNotaCredito["ciTipoIdentificacionComprador"].ToString().Trim();
                                NotaCreditoInfoNotaCredito.razonSocialComprador = DataRowNotaCredito["txRazonSocialComprador"].ToString().Trim().Contains("&amp;") ?
                                    //DataRowNotaCredito["txRazonSocialComprador"].ToString().Trim().Replace("&amp:", "Y") : DataRowNotaCredito["txRazonSocialComprador"].ToString().Trim().Contains("&") ?
                                    DataRowNotaCredito["txRazonSocialComprador"].ToString().Trim().Replace("&", "&amp;") : DataRowNotaCredito["txRazonSocialComprador"].ToString().Trim();
                                NotaCreditoInfoNotaCredito.identificacionComprador = DataRowNotaCredito["txIdentificacionComprador"].ToString().Trim();

                                NotaCreditoInfoNotaCredito.totalSinImpuestos = Convert.ToDecimal(DataRowNotaCredito["qnTotalSinImpuestos"].ToString());
                                if (DataRowNotaCredito["txRise"].ToString().Trim().Length != 0)
                                    NotaCreditoInfoNotaCredito.rise = DataRowNotaCredito["txRise"].ToString().Trim();

                                NotaCreditoInfoNotaCredito.codDocModificado = DataRowNotaCredito["ciTipoDocumentoModificado"].ToString();
                                NotaCreditoInfoNotaCredito.numDocModificado = DataRowNotaCredito["txNumeroDocumentoModificado"].ToString().Trim();
                                NotaCreditoInfoNotaCredito.fechaEmisionDocSustento = DataRowNotaCredito["txFechaEmisionDocumentoModificado"].ToString();
                                NotaCreditoInfoNotaCredito.totalSinImpuestos = Convert.ToDecimal(DataRowNotaCredito["qnTotalSinImpuestos"].ToString());
                                NotaCreditoInfoNotaCredito.valorModificacion = Convert.ToDecimal(DataRowNotaCredito["qnValorModificacion"].ToString());
                                NotaCreditoInfoNotaCredito.moneda = DataRowNotaCredito["txMoneda"].ToString().Trim();
                                NotaCreditoInfoNotaCredito.motivo = DataRowNotaCredito["txMotivo"].ToString().Trim();
                                NotaCredito.infoNotaCredito = NotaCreditoInfoNotaCredito;

                                #region NotaCredito_TOTAL_IMPUESTO
                                int cont = 0;
                                DataRow[] DataRowsNotaCreditoTotalImpuesto = DataRowNotaCredito.GetChildRows("NotaCredito_NotaCreditoTotalImpuesto");
                                if (DataRowsNotaCreditoTotalImpuesto.LongLength > 0)
                                {
                                    totalConImpuestosTotalImpuesto[] TotalImpuestos = new totalConImpuestosTotalImpuesto[DataRowsNotaCreditoTotalImpuesto.LongLength];
                                    foreach (DataRow DataRowFactTotalImpuesto in DataRowsNotaCreditoTotalImpuesto)
                                    {
                                        totalConImpuestosTotalImpuesto TotalImpuesto = new totalConImpuestosTotalImpuesto();
                                        TotalImpuesto.codigo = DataRowFactTotalImpuesto["txCodigo"].ToString();
                                        TotalImpuesto.codigoPorcentaje = DataRowFactTotalImpuesto["txCodigoPorcentaje"].ToString();
                                        TotalImpuesto.baseImponible = Convert.ToDecimal(DataRowFactTotalImpuesto["qnBaseImponible"].ToString());
                                        TotalImpuesto.valor = Convert.ToDecimal(DataRowFactTotalImpuesto["qnValor"].ToString());
                                        TotalImpuestos[cont] = TotalImpuesto;
                                        cont++;
                                    }
                                    NotaCredito.infoNotaCredito.totalConImpuestos = TotalImpuestos;
                                }
                                cont = 0;
                                #endregion NotaCredito_TOTAL_IMPUESTO

                                #region NotaCredito_DETALLE
                                cont = 0;
                                DataRow[] DataRowsNotaCreditoDetalle = DataRowNotaCredito.GetChildRows("NotaCredito_NotaCreditoDetalle");
                                if (DataRowsNotaCreditoDetalle.LongLength > 0)
                                {
                                    notaCreditoDetalle[] Detalles = new notaCreditoDetalle[DataRowsNotaCreditoDetalle.LongLength];
                                    foreach (DataRow DataRowDetalle in DataRowsNotaCreditoDetalle)
                                    {
                                        notaCreditoDetalle Detalle = new notaCreditoDetalle();
                                        Detalle.descuentoSpecified = true;
                                        Detalle.codigoInterno = DataRowDetalle["txCodigoInterno"].ToString();
                                        if (DataRowDetalle["txCodigoAdicional"].ToString().Trim().Length != 0)
                                            Detalle.codigoAdicional = DataRowDetalle["txCodigoAdicional"].ToString();
                                        Detalle.descripcion = DataRowDetalle["txDescripcion"].ToString().Trim().Contains("&amp;") ?
                                            DataRowDetalle["txDescripcion"].ToString().Trim().Replace("&amp:", "Y") : DataRowDetalle["txDescripcion"].ToString().Trim().Contains("&") ?
                                            DataRowDetalle["txDescripcion"].ToString().Trim().Replace("&", "Y") : DataRowDetalle["txDescripcion"].ToString().Trim();
                                        Detalle.cantidad = Convert.ToInt32(DataRowDetalle["qnCantidad"].ToString());
                                        Detalle.precioUnitario = Convert.ToDecimal(DataRowDetalle["qnPrecioUnitario"].ToString());
                                        Detalle.descuento = Convert.ToDecimal(DataRowDetalle["qnDescuento"].ToString());
                                        Detalle.precioTotalSinImpuesto = Convert.ToDecimal(DataRowDetalle["qnPrecioTotalSinImpuesto"].ToString());

                                        #region CAPTURO_NotaCredito_DETALLE
                                        DataSet dsDetalleImpuestoAdicional = new DataSet();
                                        dsDetalleImpuestoAdicional = LnconsultarNotaCreditoDetalle(
                                         1, Convert.ToInt32(DataRowDetalle["ciCompania"].ToString()),
                                            DataRowDetalle["txEstablecimiento"].ToString(),
                                            DataRowDetalle["txPuntoEmision"].ToString(),
                                            DataRowDetalle["txSecuencial"].ToString(), "",
                                            DataRowDetalle["txCodigoInterno"].ToString(),
                                            ref codigoRetorno, ref descripcionRetorno);

                                        if (codigoRetorno.Equals(0))
                                        {
                                            dsDetalleImpuestoAdicional.Tables[0].TableName = "NotaCreditoDetalleImpuesto";
                                            dsDetalleImpuestoAdicional.Tables[1].TableName = "NotaCreditoDetalleAdicional";
                                            #endregion CAPTURO_NotaCredito_DETALLE

                                            #region NOTA_CREDITO_DETALLE_ADICIONAL
                                            if (dsDetalleImpuestoAdicional.Tables["NotaCreditoDetalleAdicional"].Rows.Count > 0)
                                            {
                                                notaCreditoDetalleDetAdicional[] DetDetalleAdicional = new notaCreditoDetalleDetAdicional[dsDetalleImpuestoAdicional.Tables["NotaCreditoDetalleAdicional"].Rows.Count];
                                                foreach (DataRow DataRowDetalleAdicional in dsDetalleImpuestoAdicional.Tables["NotaCreditoDetalleAdicional"].Rows)
                                                {
                                                    notaCreditoDetalleDetAdicional DetalleAdicional = new notaCreditoDetalleDetAdicional();
                                                    DetalleAdicional.nombre = DataRowDetalleAdicional["txNombre"].ToString();
                                                    DetalleAdicional.valor = DataRowDetalleAdicional["txValor"].ToString();
                                                    DetDetalleAdicional[dsDetalleImpuestoAdicional.Tables["NotaCreditoDetalleAdicional"].Rows.IndexOf(DataRowDetalleAdicional)] = DetalleAdicional;
                                                }
                                                Detalle.detallesAdicionales = DetDetalleAdicional;
                                            }
                                            #endregion NOTA_CREDITO_DETALLE_ADICIONAL

                                            #region NotaCredito_DETALLE_IMPUESTO

                                            if (dsDetalleImpuestoAdicional.Tables["NotaCreditoDetalleImpuesto"].Rows.Count > 0)
                                            {
                                                impuesto_NC[] detalleImpuestos = new impuesto_NC[dsDetalleImpuestoAdicional.Tables["NotaCreditoDetalleImpuesto"].Rows.Count];

                                                foreach (DataRow DataRowDetalleImpuesto in dsDetalleImpuestoAdicional.Tables["NotaCreditoDetalleImpuesto"].Rows)
                                                {
                                                    impuesto_NC detalleImpuesto = new impuesto_NC();
                                                    detalleImpuesto.codigo = DataRowDetalleImpuesto["txCodigo"].ToString();
                                                    detalleImpuesto.codigoPorcentaje = DataRowDetalleImpuesto["txCodigoPorcentaje"].ToString();
                                                    detalleImpuesto.baseImponible = Convert.ToDecimal(DataRowDetalleImpuesto["qnBaseImponible"].ToString());
                                                    if (DataRowDetalleImpuesto["txTarifa"].ToString().Trim() != "")
                                                    {
                                                        detalleImpuesto.tarifaSpecified = true;
                                                        detalleImpuesto.tarifa = Convert.ToDecimal(DataRowDetalleImpuesto["txTarifa"].ToString());
                                                    }
                                                    else
                                                    {
                                                        detalleImpuesto.tarifaSpecified = false;
                                                    }
                                                    detalleImpuesto.valor = Convert.ToDecimal(DataRowDetalleImpuesto["qnValor"].ToString());
                                                    detalleImpuestos[dsDetalleImpuestoAdicional.Tables["NotaCreditoDetalleImpuesto"].Rows.IndexOf(DataRowDetalleImpuesto)] = detalleImpuesto;
                                                }
                                                Detalle.impuestos = detalleImpuestos;
                                            }
                                            #endregion NotaCredito_DETALLE_IMPUESTO
                                        }
                                        Detalles[cont] = Detalle;
                                        cont++;
                                        dsDetalleImpuestoAdicional.Dispose();
                                    }
                                    NotaCredito.detalles = Detalles;
                                }
                                cont = 0;
                                #endregion NotaCredito_DETALLE

                                #region NotaCredito_INFO_ADICIONAL

                                DataRow[] DataRowsNotaCreditoInfoAdicional = DataRowNotaCredito.GetChildRows("NotaCredito_NotaCreditoInfoAdicional");
                                cont = 0;
                                if (DataRowsNotaCreditoInfoAdicional.LongLength > 0)
                                {
                                    notaCreditoCampoAdicional[] NotaCreditoInformacionAdicional = new notaCreditoCampoAdicional[DataRowsNotaCreditoInfoAdicional.LongLength];
                                    foreach (DataRow DataRowFactInfoAdicional in DataRowsNotaCreditoInfoAdicional)
                                    {
                                        notaCreditoCampoAdicional InfoAdicional = new notaCreditoCampoAdicional();
                                        InfoAdicional.nombre = DataRowFactInfoAdicional["txNombre"].ToString();
                                        InfoAdicional.Value = DataRowFactInfoAdicional["txValor"].ToString().Trim().Contains("&amp;") ?
                                            DataRowFactInfoAdicional["txValor"].ToString().Trim().Replace("&amp:", "Y") : DataRowFactInfoAdicional["txValor"].ToString().Trim().Contains("&") ?
                                            DataRowFactInfoAdicional["txValor"].ToString().Trim().Replace("&", "Y") : DataRowFactInfoAdicional["txValor"].ToString().Trim();
                                        NotaCreditoInformacionAdicional[cont] = InfoAdicional;
                                        cont++;
                                    }
                                    NotaCredito.infoAdicional = NotaCreditoInformacionAdicional;
                                }
                                cont = 0;
                                #endregion NotaCredito_INFO_ADICIONAL
                                #endregion NotaCredito

                                #region GENERA_CLAVE_ACCESO
                                ClaveAcceso claveAcceso = new ClaveAcceso();
                                if (DataRowNotaCredito["txClaveAcceso"].ToString().Trim() == "" || DataRowNotaCredito["txClaveAcceso"].ToString().Trim() == null)
                                {
                                    #region Nuevo documento generacion de su clave                               
                                    string ClaveAccesoNormal = claveAcceso.GenerarClaveAccesoDocumento(NotaCredito.infoTributaria.codDoc,
                                                                                                       NotaCredito.infoTributaria.secuencial.ToString().Trim(),
                                                                                                       NotaCredito.infoTributaria.ptoEmi,
                                                                                                       NotaCredito.infoTributaria.estab,
                                                                                                       NotaCredito.infoNotaCredito.fechaEmision,
                                                                                                       NotaCredito.infoTributaria.ruc,
                                                                                                       NotaCredito.infoTributaria.ambiente);

                                    if (ClaveAccesoNormal.Length == 49)
                                    {
                                        NotaCredito.infoTributaria.claveAcceso = ClaveAccesoNormal;
                                    }
                                    else
                                    {
                                        NotaCredito.infoTributaria.claveAcceso = claveAcceso.GenerarClaveAccesoDocumento(NotaCredito.infoTributaria.codDoc,
                                                                                                                         NotaCredito.infoTributaria.secuencial.ToString().Trim(),
                                                                                                                         NotaCredito.infoTributaria.ptoEmi,
                                                                                                                         NotaCredito.infoTributaria.estab,
                                                                                                                         NotaCredito.infoNotaCredito.fechaEmision,
                                                                                                                         NotaCredito.infoTributaria.ruc,
                                                                                                                         NotaCredito.infoTributaria.ambiente);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Documento ya contiene una clave de Acceso
                                    if (NotaCredito.infoNotaCredito.fechaEmision.Replace("/", "") != DataRowNotaCredito["txClaveAcceso"].ToString().Substring(0, 8))
                                    {
                                        #region Nuevo documento generacion de su clave                               
                                        string ClaveAccesoNormal = claveAcceso.GenerarClaveAccesoDocumento(NotaCredito.infoTributaria.codDoc,
                                                                                                           NotaCredito.infoTributaria.secuencial.ToString().Trim(),
                                                                                                           NotaCredito.infoTributaria.ptoEmi,
                                                                                                           NotaCredito.infoTributaria.estab,
                                                                                                           NotaCredito.infoNotaCredito.fechaEmision,
                                                                                                           NotaCredito.infoTributaria.ruc,
                                                                                                           NotaCredito.infoTributaria.ambiente);

                                        if (ClaveAccesoNormal.Length == 49)
                                        {
                                            NotaCredito.infoTributaria.claveAcceso = ClaveAccesoNormal;
                                        }
                                        else
                                        {
                                            NotaCredito.infoTributaria.claveAcceso = claveAcceso.GenerarClaveAccesoDocumento(NotaCredito.infoTributaria.codDoc,
                                                                                                                             NotaCredito.infoTributaria.secuencial.ToString().Trim(),
                                                                                                                             NotaCredito.infoTributaria.ptoEmi,
                                                                                                                             NotaCredito.infoTributaria.estab,
                                                                                                                             NotaCredito.infoNotaCredito.fechaEmision,
                                                                                                                             NotaCredito.infoTributaria.ruc,
                                                                                                                             NotaCredito.infoTributaria.ambiente);
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                                #endregion GENERA_CLAVE_ACCESO

                                Object obj = new Object();
                                obj = NotaCredito;
                                XmlGenerados xmlGenerado = new XmlGenerados();
                                xmlGenerado.Identity = Convert.ToInt32(DataRowNotaCredito["ciNotaCredito"].ToString());
                                xmlGenerado.CiCompania = compania.CiCompania;
                                xmlGenerado.XmlComprobante = Serializacion.serializar(obj);
                                xmlGenerado.ClaveAcceso = NotaCredito.infoTributaria.claveAcceso;
                                xmlGenerado.NameXml = NotaCredito.infoTributaria.claveAcceso;
                                xmlGenerado.CiTipoEmision = NotaCredito.infoTributaria.tipoEmision;
                                xmlGenerado.CiTipoDocumento = NotaCredito.infoTributaria.codDoc;
                                xmlGenerado.XmlEstado = CatalogoViaDoc.DocEstadoGenerado;
                                xmlGenerado.ciNumeroIntento = numeroIntentosNotaCredito;
                                xmlNotaCredito.Add(xmlGenerado);
                            }
                            else
                            {
                                NotaCredito = (notaCredito)Serializacion.desSerializar(DataRowNotaCredito["xmlDocumentoAutorizado"].ToString(), NotaCredito.GetType());
                                XmlGenerados xmlGenerado = new XmlGenerados();
                                xmlGenerado.Identity = Convert.ToInt32(DataRowNotaCredito["ciNotaCredito"].ToString());
                                xmlGenerado.CiCompania = compania.CiCompania;
                                xmlGenerado.XmlComprobante = Serializacion.serializar(NotaCredito);
                                xmlGenerado.ClaveAcceso = NotaCredito.infoTributaria.claveAcceso;
                                xmlGenerado.NameXml = NotaCredito.infoTributaria.claveAcceso;
                                xmlGenerado.CiTipoEmision = NotaCredito.infoTributaria.tipoEmision;
                                xmlGenerado.CiTipoDocumento = NotaCredito.infoTributaria.codDoc;
                                xmlGenerado.CiContingenciaDet = Convert.ToInt32(DataRowNotaCredito["ciContingenciaDet"].ToString());
                                xmlGenerado.XmlEstado = CatalogoViaDoc.DocEstadoGenerado;
                                claveAccesoGenerada = NotaCredito.infoTributaria.claveAcceso;
                                xmlNotaCreditoGenerado = xmlGenerado.XmlComprobante;
                                tipoDocumento = xmlGenerado.CiTipoDocumento;
                                xmlNotaCredito.Add(xmlGenerado);
                            }
                        }
                        catch (Exception ex)
                        {
                            XmlGenerados xmlGenerado = new XmlGenerados();
                            xmlGenerado.Identity = Convert.ToInt32(DataRowNotaCredito["ciNotaCredito"].ToString());
                            xmlGenerado.MensajeError = ex.Message + "||||" + ex.StackTrace;
                            xmlGenerado.XmlEstado = CatalogoViaDoc.DocEstadoEFirmado;
                            xmlGenerado.txCodError = "101";
                            xmlGenerado.CiTipoDocumento = CatalogoViaDoc.DocumentoFactura;
                            xmlGenerado.CiCompania = compania.CiCompania;
                            xmlGenerado.CiContingenciaDet = 1;
                            xmlGenerado.XmlComprobante = Serializacion.serializar(NotaCredito);
                            xmlGenerado.NameXml = NotaCredito.infoTributaria.claveAcceso;
                            xmlGenerado.ClaveAcceso = NotaCredito.infoTributaria.claveAcceso;
                            xmlGenerado.ciNumeroIntento = numeroIntentosNotaCredito + 1;
                            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("ProcesosFirma - Facturas: " + ex.Message);
                            FirmaDocumentos actualizarEstado = new FirmaDocumentos();
                            actualizarEstado.ActualizarXmlComprobantes(xmlGenerado);
                        }
                    }
                    dsNotaCredito.Dispose();
                    #endregion NotaCredito
                }
            }
            catch (Exception ex)
            {

            }
            return xmlNotaCredito;
        }

        protected DataSet LnconsultarNotaCreditoDetalle(int op, int ciCompañia, string txEstablecimiento, string txPuntoEmision, string txSecuencial,
                                                         string txCodigoPrincipal, string txCodigoInterno, ref int codigoRetorno, ref string descripcionRetorno)
        {
            DataSet ds = new DataSet();

            ViaDoc.AccesoDatos.DocumentoAD _consultaDocumentos = new ViaDoc.AccesoDatos.DocumentoAD();

            return ds = _consultaDocumentos.DocumentosElectronicos(9, ciCompañia, txEstablecimiento, txPuntoEmision, txSecuencial, txCodigoPrincipal,
                                                                   0, txCodigoInterno, ref codigoRetorno, ref descripcionRetorno);
        }

    }
}
