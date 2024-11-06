using com.sun.mail.iap;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using ViaDoc.Configuraciones;
using ViaDoc.EntidadNegocios;
using ViaDoc.Utilitarios;

namespace ViaDocFirma.LogicaNegocios.procesos
{
    public class ProcesoLiquidaacion
    {
        FirmaDocumentos actualizarEstado = new FirmaDocumentos();

        public List<XmlGenerados> ProcesaXmlLiquidacion(Compania compania, string Version, int numeroRegistro)
        {
            List<XmlGenerados> xmlLiqui = new List<XmlGenerados>();
            int contInicial = 0;
            int codigoRetorno = 0;
            string descripcionRetorno = string.Empty;
            int numeroIntentosFactura = 0;
            XmlGenerados xmlGenerado = null;
            ViaDoc.AccesoDatos.DocumentoAD _consultaDocumentos = new ViaDoc.AccesoDatos.DocumentoAD();

            try
            {
                DataSet dsLiquidacion = _consultaDocumentos.DocumentosElectronicos(12, compania.CiCompania, "", "", "", "", numeroRegistro, "", ref codigoRetorno, ref descripcionRetorno);

                if (codigoRetorno.Equals(0))
                {
                    contInicial = dsLiquidacion.Tables[0].Select("ciEstado='" + CatalogoViaDoc.DocEstadoInicial + "'").ToList().Count();

                    if (contInicial.Equals(0)) contInicial = dsLiquidacion.Tables[0].Select("ciEstado='" + CatalogoViaDoc.DocEstadoEFirmado + "'").ToList().Count();

                    dsLiquidacion.Tables[0].TableName = "Liquidacion";
                    dsLiquidacion.Tables[1].TableName = "LiquidacionInfoAdicional";
                    dsLiquidacion.Tables[2].TableName = "LiquidacionTotalImpuesto";
                    dsLiquidacion.Tables[3].TableName = "LiquidacionDetalle";
                    dsLiquidacion.Tables[4].TableName = "LiquidacionDetalleFormaPago";

                    DataColumn[] Pk_Liquidacion = new DataColumn[4];
                    DataColumn[] Fk_LiquidacionInfoAdicional = new DataColumn[4];
                    DataColumn[] Fk_LiquidacionTotalImpuesto = new DataColumn[4];
                    DataColumn[] Fk_LiquidacionDetalle = new DataColumn[4];
                    DataColumn[] Fk_LiquidacionFormaPago = new DataColumn[4];

                    //Liquidacion
                    Pk_Liquidacion[0] = dsLiquidacion.Tables["Liquidacion"].Columns["ciCompania"];
                    Pk_Liquidacion[1] = dsLiquidacion.Tables["Liquidacion"].Columns["txEstablecimiento"];
                    Pk_Liquidacion[2] = dsLiquidacion.Tables["Liquidacion"].Columns["txPuntoEmision"];
                    Pk_Liquidacion[3] = dsLiquidacion.Tables["Liquidacion"].Columns["txSecuencial"];

                    //LiquidacionInfoAdicional
                    Fk_LiquidacionInfoAdicional[0] = dsLiquidacion.Tables["LiquidacionInfoAdicional"].Columns["ciCompania"];
                    Fk_LiquidacionInfoAdicional[1] = dsLiquidacion.Tables["LiquidacionInfoAdicional"].Columns["txEstablecimiento"];
                    Fk_LiquidacionInfoAdicional[2] = dsLiquidacion.Tables["LiquidacionInfoAdicional"].Columns["txPuntoEmision"];
                    Fk_LiquidacionInfoAdicional[3] = dsLiquidacion.Tables["LiquidacionInfoAdicional"].Columns["txSecuencial"];

                    //LiquidacionTotalImpuesto
                    Fk_LiquidacionTotalImpuesto[0] = dsLiquidacion.Tables["LiquidacionTotalImpuesto"].Columns["ciCompania"];
                    Fk_LiquidacionTotalImpuesto[1] = dsLiquidacion.Tables["LiquidacionTotalImpuesto"].Columns["txEstablecimiento"];
                    Fk_LiquidacionTotalImpuesto[2] = dsLiquidacion.Tables["LiquidacionTotalImpuesto"].Columns["txPuntoEmision"];
                    Fk_LiquidacionTotalImpuesto[3] = dsLiquidacion.Tables["LiquidacionTotalImpuesto"].Columns["txSecuencial"];

                    //LiquidacionDetalle fk
                    Fk_LiquidacionDetalle[0] = dsLiquidacion.Tables["LiquidacionDetalle"].Columns["ciCompania"];
                    Fk_LiquidacionDetalle[1] = dsLiquidacion.Tables["LiquidacionDetalle"].Columns["txEstablecimiento"];
                    Fk_LiquidacionDetalle[2] = dsLiquidacion.Tables["LiquidacionDetalle"].Columns["txPuntoEmision"];
                    Fk_LiquidacionDetalle[3] = dsLiquidacion.Tables["LiquidacionDetalle"].Columns["txSecuencial"];

                    //LiquidacionDetalleformaPago fk
                    //Nuevo TAB Forma Pago
                    Fk_LiquidacionFormaPago[0] = dsLiquidacion.Tables["LiquidacionDetalleFormaPago"].Columns["ciCompania"];
                    Fk_LiquidacionFormaPago[1] = dsLiquidacion.Tables["LiquidacionDetalleFormaPago"].Columns["txEstablecimiento"];
                    Fk_LiquidacionFormaPago[2] = dsLiquidacion.Tables["LiquidacionDetalleFormaPago"].Columns["txPuntoEmision"];
                    Fk_LiquidacionFormaPago[3] = dsLiquidacion.Tables["LiquidacionDetalleFormaPago"].Columns["txSecuencial"];

                    dsLiquidacion.Relations.Add("LIQUIDACIONES_LiquidacionInfoAdicional", Pk_Liquidacion, Fk_LiquidacionInfoAdicional);
                    dsLiquidacion.Relations.Add("LIQUIDACIONES_LiquidacionTotalImpuesto", Pk_Liquidacion, Fk_LiquidacionTotalImpuesto);
                    dsLiquidacion.Relations.Add("LIQUIDACIONES_LiquidacionDetalle", Pk_Liquidacion, Fk_LiquidacionDetalle);
                    dsLiquidacion.Relations.Add("LIQUIDACIONES_LiquidacionDetalleFormaPago", Pk_Liquidacion, Fk_LiquidacionFormaPago);//Nuevo TAB Forma Pago

                    liquidacionCompra liquidacion = new liquidacionCompra();
                    liquidacion.id = liquidacionID.comprobante;
                    liquidacion.idSpecified = true;
                    liquidacion.version = Version;

                    infoTributaria_LQ infoTributariaLQ = new infoTributaria_LQ();
                    infoTributariaLQ.ambiente = compania.CiTipoAmbiente.ToString().Trim();
                    infoTributariaLQ.dirMatriz = compania.TxDireccionMatriz; //proviene de Compania
                    if (compania.TxNombreComercial.Trim().Length != 0)
                        infoTributariaLQ.nombreComercial = compania.TxNombreComercial;  //proviene de Compania 

                    infoTributariaLQ.razonSocial = compania.TxRazonSocial;  //proviene de Compania
                    infoTributariaLQ.ruc = compania.TxRuc;   //proviene de Compania

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
                                            infoTributariaLQ.agenteRetencion = CatalogoViaDoc.LeyendaAgente.Trim().Replace("Agente de Retención Resolución Nro ", "");
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
                                                infoTributariaLQ.contribuyenteRimpe = CatalogoViaDoc.LeyendaRegimen.Trim();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            infoTributariaLQ.contribuyenteRimpe = string.Empty;
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
                                                infoTributariaLQ.contribuyenteRimpe = CatalogoViaDoc.LeyendaRegimenRimpe.Trim();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            infoTributariaLQ.contribuyenteRimpe = string.Empty;
                        }

                    }
                    #endregion
                    liquidacionInfoLiquidacion LiquidaiconInfoLiquidacion = new liquidacionInfoLiquidacion();
                    if (compania.TxContribuyenteEspecial.Trim().Length != 0)
                        LiquidaiconInfoLiquidacion.contribuyenteEspecial = compania.TxContribuyenteEspecial; //proviene de Compania

                    if (compania.TxObligadoContabilidad.Trim().Length != 0)
                    {
                        if (compania.TxObligadoContabilidad == "S")
                        {
                            LiquidaiconInfoLiquidacion.obligadoContabilidad = "SI"; //proviene de Compania
                        }
                        if (compania.TxObligadoContabilidad == "N")
                        {
                            LiquidaiconInfoLiquidacion.obligadoContabilidad = "NO"; //proviene de Compania
                        }
                    }
                    DataRow liquidacionprueba;
                    int cntCiclo = 0;
                        foreach (DataRow DataRowLiquidacion in dsLiquidacion.Tables["Liquidacion"].Rows)
                    {
                        int ciLiquidacion = Convert.ToInt32(DataRowLiquidacion["ciLiquidacion"].ToString().Trim());
                        string claveAccesoGenerada = "";
                        //cntCiclo++;
                        try
                        {
                            //if (DataRowLiquidacion["ciEstado"].ToString().Trim() != ConfigurationManager.AppSettings.Get("CONTINGENCIA").ToString().Trim())
                            if (DataRowLiquidacion["ciEstado"].ToString().Trim() == CatalogoViaDoc.DocEstadoInicial
                                || DataRowLiquidacion["ciEstado"].ToString().Trim() == CatalogoViaDoc.DocEstadoEFirmado)
                            {
                                #region LIQUIDACION
                                infoTributariaLQ.claveAcceso = DataRowLiquidacion["txClaveAcceso"].ToString().Trim();  //proviene de CompRetencion
                                infoTributariaLQ.estab = DataRowLiquidacion["txEstablecimiento"].ToString().Trim();     //proviene de CompRetencion
                                infoTributariaLQ.ptoEmi = DataRowLiquidacion["txPuntoEmision"].ToString().Trim();    //proviene de CompRetencion          
                                infoTributariaLQ.secuencial = DataRowLiquidacion["txSecuencial"].ToString().Trim();    //proviene de CompRetencion
                                infoTributariaLQ.tipoEmision = DataRowLiquidacion["ciTipoEmision"].ToString().Trim();   //proviene de CompRetencion
                                infoTributariaLQ.codDoc = DataRowLiquidacion["ciTipoDocumento"].ToString().Trim();
                                liquidacion.infoTributaria = infoTributariaLQ;
                                LiquidaiconInfoLiquidacion.fechaEmision = DataRowLiquidacion["txFechaEmision"].ToString().Trim();

                                numeroIntentosFactura = int.Parse(DataRowLiquidacion["ciNumeroIntento"].ToString().Trim());

                                if (DataRowLiquidacion["txDireccion"].ToString().Trim().Length != 0)
                                {
                                    LiquidaiconInfoLiquidacion.dirEstablecimiento = DataRowLiquidacion["txDireccion"].ToString().Trim();
                                }
                                LiquidaiconInfoLiquidacion.tipoIdentificacionProveedor = DataRowLiquidacion["ciTipoIdentificacionProvedor"].ToString().Trim();
                                LiquidaiconInfoLiquidacion.razonSocialProveedor = DataRowLiquidacion["txRazonSocialProvedor"].ToString().Trim().Contains("&amp;") ?
                                    DataRowLiquidacion["txRazonSocialProvedor"].ToString().Trim().Replace("&amp:", "Y") : DataRowLiquidacion["txRazonSocialProvedor"].ToString().Trim().Contains("&") ?
                                    DataRowLiquidacion["txRazonSocialProvedor"].ToString().Trim().Replace("&", "Y") : DataRowLiquidacion["txRazonSocialProvedor"].ToString().Trim();
                                LiquidaiconInfoLiquidacion.identificacionProveedor = DataRowLiquidacion["txIdentificacionProvedor"].ToString().Trim();
                                LiquidaiconInfoLiquidacion.totalSinImpuestos = Convert.ToDecimal(DataRowLiquidacion["qnTotalSinImpuestos"].ToString().Trim());
                                LiquidaiconInfoLiquidacion.totalDescuento = Convert.ToDecimal(DataRowLiquidacion["qnTotalDescuento"].ToString().Trim());

                                if (DataRowLiquidacion["CodDocReembolso"].ToString().Trim().Equals(ConfigurationManager.AppSettings["CodDocReembolso"].Trim()))
                                {
                                    LiquidaiconInfoLiquidacion.codDocReembolso = DataRowLiquidacion["CodDocReembolso"].ToString().Trim();
                                }
                                LiquidaiconInfoLiquidacion.totalComprobantesReembolso = Convert.ToDecimal(DataRowLiquidacion["totalComprobantesReembolso"].ToString().Trim());
                                LiquidaiconInfoLiquidacion.totalBaseImponibleReembolso = Convert.ToDecimal(DataRowLiquidacion["totalBaseImponibleReembolso"].ToString().Trim());
                                LiquidaiconInfoLiquidacion.totalImpuestoReembolso = Convert.ToDecimal(DataRowLiquidacion["totalImpuestoReembolso"].ToString().Trim());


                                LiquidaiconInfoLiquidacion.importeTotal = Convert.ToDecimal(DataRowLiquidacion["qnImporteTotal"].ToString().Trim());

                                if (DataRowLiquidacion["txMoneda"].ToString().Trim().Length != 0)
                                {
                                    LiquidaiconInfoLiquidacion.moneda = DataRowLiquidacion["txMoneda"].ToString().Trim();
                                }
                                liquidacion.infoLiquidacionCompra = LiquidaiconInfoLiquidacion;

                                #region LIQUIDACION_TOTAL_IMPUESTO
                                int cont = 0;
                                DataRow[] DataRowsLiqudiacionTotalImpuesto = DataRowLiquidacion.GetChildRows("LIQUIDACIONES_LiquidacionTotalImpuesto");
                                if (DataRowsLiqudiacionTotalImpuesto.LongLength > 0)
                                {
                                    liquidacionInfoLiquidacionTotalImpuesto[] LiquTotalImpuestos = new liquidacionInfoLiquidacionTotalImpuesto[DataRowsLiqudiacionTotalImpuesto.LongLength];
                                    foreach (DataRow DataRowLiquTotalImpuesto in DataRowsLiqudiacionTotalImpuesto)
                                    {
                                        liquidacionInfoLiquidacionTotalImpuesto LiquTotalImpuesto = new liquidacionInfoLiquidacionTotalImpuesto();

                                        LiquTotalImpuesto.codigo = DataRowLiquTotalImpuesto["txCodigo"].ToString().Trim();
                                        LiquTotalImpuesto.codigoPorcentaje = DataRowLiquTotalImpuesto["txCodigoPorcentaje"].ToString().Trim();
                                        LiquTotalImpuesto.baseImponible = Convert.ToDecimal(DataRowLiquTotalImpuesto["qnBaseImponible"].ToString().Trim());
                                        if (DataRowLiquTotalImpuesto["txTarifa"].ToString().Trim() != "")
                                        {
                                            LiquTotalImpuesto.tarifaSpecified = true;
                                            LiquTotalImpuesto.tarifa = Convert.ToDecimal(DataRowLiquTotalImpuesto["txTarifa"].ToString().Trim());
                                        }
                                        else
                                        {
                                            LiquTotalImpuesto.tarifaSpecified = false;
                                        }

                                        LiquTotalImpuesto.valor = Convert.ToDecimal(DataRowLiquTotalImpuesto["qnValor"].ToString().Trim());
                                        LiquTotalImpuestos[cont] = LiquTotalImpuesto;
                                        cont++;
                                    }

                                    liquidacion.infoLiquidacionCompra.totalConImpuestos = LiquTotalImpuestos;
                                }
                                cont = 0;
                                #endregion LIQUIDACION_TOTAL_IMPUESTO

                                #region LIQUIDACION_FORMA_PAGO
                                cont = 0;
                                DataRow[] DataRowsLiquidcionFormaPago = DataRowLiquidacion.GetChildRows("LIQUIDACIONES_LiquidacionDetalleFormaPago");
                                if (DataRowsLiquidcionFormaPago.LongLength > 0)
                                {
                                    liquidacionInfoLiquidacionFormaPago[] LiquFormaPagos = new liquidacionInfoLiquidacionFormaPago[DataRowsLiquidcionFormaPago.LongLength];
                                    foreach (DataRow DataRowLiquidacionFormaPago in DataRowsLiquidcionFormaPago)
                                    {
                                        liquidacionInfoLiquidacionFormaPago LiquFormaPago = new liquidacionInfoLiquidacionFormaPago();

                                        LiquFormaPago.formaPago = DataRowLiquidacionFormaPago["txFormaPago"].ToString().Trim();
                                        LiquFormaPago.plazo = int.Parse(DataRowLiquidacionFormaPago["txPlazo"].ToString().Trim());
                                        LiquFormaPago.total = Convert.ToDecimal(DataRowLiquidacionFormaPago["qnTotal"].ToString().Trim());
                                        LiquFormaPago.unidadTiempo = DataRowLiquidacionFormaPago["txUnidadTiempo"].ToString().Trim();

                                        LiquFormaPagos[cont] = LiquFormaPago;
                                        cont++;
                                    }
                                    liquidacion.infoLiquidacionCompra.pagos = LiquFormaPagos;
                                }
                                cont = 0;
                                #endregion LIQUIDACION_FORMA_PAGO

                                #region LIQUIDACION_DETALLE
                                cont = 0;
                                DataRow[] DataRowsLiquidacionDetalle = DataRowLiquidacion.GetChildRows("LIQUIDACIONES_LiquidacionDetalle");
                                if (DataRowsLiquidacionDetalle.LongLength > 0)
                                {
                                    liquidcionDetalle[] liquDetalles = new liquidcionDetalle[DataRowsLiquidacionDetalle.LongLength];
                                    foreach (DataRow DataRowLiqutDetalle in DataRowsLiquidacionDetalle)
                                    {
                                        int contLiquiDetImpuesto = 0;
                                        liquidcionDetalle liquDetalle = new liquidcionDetalle();
                                        if (DataRowLiqutDetalle["txCodigoPrincipal"].ToString().Trim().Length != 0)
                                            liquDetalle.codigoPrincipal = DataRowLiqutDetalle["txCodigoPrincipal"].ToString().Trim();
                                        if (DataRowLiqutDetalle["txCodigoAuxiliar"].ToString().Trim().Length != 0)
                                            liquDetalle.codigoAuxiliar = DataRowLiqutDetalle["txCodigoAuxiliar"].ToString().Trim();
                                        liquDetalle.descripcion = DataRowLiqutDetalle["txDescripcion"].ToString().Trim().Contains("&amp;") ?
                                            DataRowLiqutDetalle["txDescripcion"].ToString().Trim().Replace("&amp:", "Y") : DataRowLiqutDetalle["txDescripcion"].ToString().Trim().Contains("&") ?
                                            DataRowLiqutDetalle["txDescripcion"].ToString().Trim().Replace("&", "Y") : DataRowLiqutDetalle["txDescripcion"].ToString().Trim();
                                        liquDetalle.cantidad = Convert.ToInt32(DataRowLiqutDetalle["qnCantidad"].ToString().Trim());
                                        liquDetalle.precioUnitario = Convert.ToDecimal(DataRowLiqutDetalle["qnPrecioUnitario"].ToString().Trim());
                                        liquDetalle.descuento = Convert.ToDecimal(DataRowLiqutDetalle["qnDescuento"].ToString().Trim());
                                        liquDetalle.precioTotalSinImpuesto = Convert.ToDecimal(DataRowLiqutDetalle["qnPrecioTotalSinImpuesto"].ToString().Trim());


                                        #region CAPTURO_LIQUIDACION_DETALLE_IMPUESTO
                                        DataSet LiquDetalleImpuestoAdicional = new DataSet();
                                        LiquDetalleImpuestoAdicional = LnconsultarLiquidacionDetalleImpuestoAdicional(
                                         1, Convert.ToInt32(DataRowLiqutDetalle["ciCompania"].ToString().Trim()),
                                            DataRowLiqutDetalle["txEstablecimiento"].ToString().Trim(),
                                            DataRowLiqutDetalle["txPuntoEmision"].ToString().Trim(),
                                            DataRowLiqutDetalle["txSecuencial"].ToString().Trim(),
                                            DataRowLiqutDetalle["txCodigoPrincipal"].ToString().Trim(),
                                            ref codigoRetorno, ref descripcionRetorno);

                                        if (codigoRetorno.Equals(0))
                                        {
                                            LiquDetalleImpuestoAdicional.Tables[0].TableName = "LiquidacionDetalleImpuesto";
                                            LiquDetalleImpuestoAdicional.Tables[1].TableName = "LiquidacionDetalleAdicional";
                                            #endregion CAPTURO_LIQUIDACION_DETALLE_IMPUESTO

                                            #region LIQUIDACION_DETALLE_ADICIONAL

                                            int DataRowsLiquidacionDetalleAdicional = LiquDetalleImpuestoAdicional.Tables["LiquidacionDetalleAdicional"].Rows.Count;
                                            if (DataRowsLiquidacionDetalleAdicional > 0)
                                            {
                                                LiquidacionDetalleDetAdicional[] liquiDetDetalleAdicional = new LiquidacionDetalleDetAdicional[DataRowsLiquidacionDetalleAdicional];

                                                foreach (DataRow DataRowLiquiDetalleAdicional in LiquDetalleImpuestoAdicional.Tables["LiquidacionDetalleAdicional"].Rows)
                                                {
                                                    if (DataRowLiquiDetalleAdicional["txNombre"].ToString().Trim().Length != 0 && DataRowLiquiDetalleAdicional["txValor"].ToString().Trim().Length != 0)
                                                    {
                                                        LiquidacionDetalleDetAdicional liquitDetalleAdicional = new LiquidacionDetalleDetAdicional();
                                                        if (DataRowLiquiDetalleAdicional["txNombre"].ToString().Trim().Length != 0)
                                                            liquitDetalleAdicional.nombre = DataRowLiquiDetalleAdicional["txNombre"].ToString().Trim();
                                                        if (DataRowLiquiDetalleAdicional["txValor"].ToString().Trim().Length != 0)
                                                            liquitDetalleAdicional.valor = DataRowLiquiDetalleAdicional["txValor"].ToString().Trim();

                                                        liquiDetDetalleAdicional[contLiquiDetImpuesto] = liquitDetalleAdicional;
                                                        contLiquiDetImpuesto++;
                                                    }
                                                }

                                                if (contLiquiDetImpuesto != 0)
                                                    liquDetalle.detallesAdicionales = liquiDetDetalleAdicional;
                                            }
                                            contLiquiDetImpuesto = 0;

                                            #endregion LIQUIDACION_DETALLE_ADICIONAL

                                            #region LIQUIDACION_DETALLE_IMPUESTO

                                            int DataRowsLiquidacionDetalleImpuesto = LiquDetalleImpuestoAdicional.Tables["LiquidacionDetalleImpuesto"].Rows.Count;
                                            if (DataRowsLiquidacionDetalleImpuesto > 0)
                                            {
                                                //facturaInfoFacturaTotalImpuesto[] factdetalleImpuestos = new facturaInfoFacturaTotalImpuesto[DataRowsFacturaDetalleImpuesto];                                     
                                                impuesto_LQ[] liquidetalleImpuestos = new impuesto_LQ[DataRowsLiquidacionDetalleImpuesto];

                                                foreach (DataRow DataRowLiquiDetalleImpuesto in LiquDetalleImpuestoAdicional.Tables["LiquidacionDetalleImpuesto"].Rows)
                                                {
                                                    impuesto_LQ liquidetalleImpuesto = new impuesto_LQ();
                                                    liquidetalleImpuesto.codigo = DataRowLiquiDetalleImpuesto["txCodigo"].ToString().Trim();
                                                    liquidetalleImpuesto.codigoPorcentaje = DataRowLiquiDetalleImpuesto["txCodigoPorcentaje"].ToString().Trim();
                                                    liquidetalleImpuesto.baseImponible = Convert.ToDecimal(DataRowLiquiDetalleImpuesto["qnBaseImponible"].ToString().Trim());
                                                    liquidetalleImpuesto.tarifa = Convert.ToDecimal(DataRowLiquiDetalleImpuesto["txTarifa"].ToString().Trim());// alerta hay q hacer un calculo;
                                                    liquidetalleImpuesto.valor = Convert.ToDecimal(DataRowLiquiDetalleImpuesto["qnValor"].ToString().Trim());
                                                    liquidetalleImpuestos[contLiquiDetImpuesto] = liquidetalleImpuesto;

                                                    contLiquiDetImpuesto++;
                                                }
                                                liquDetalle.impuestos = liquidetalleImpuestos;
                                            }
                                            contLiquiDetImpuesto = 0;

                                            #endregion LIQUIDACION_DETALLE_IMPUESTO
                                        }
                                        liquDetalles[cont] = liquDetalle;
                                        cont++;

                                        LiquDetalleImpuestoAdicional.Dispose();
                                    }

                                    liquidacion.detalles = liquDetalles;
                                }
                                cont = 0;
                                #endregion LIQUIDACION_DETALLE

                                #region LIQUIDACION_INFO_ADICIONAL
                                DataRow[] DataRowsLiquidacionInfoAdicional = DataRowLiquidacion.GetChildRows("LIQUIDACIONES_LiquidacionInfoAdicional");
                                cont = 0;
                                if (DataRowsLiquidacionInfoAdicional.LongLength > 0)
                                {
                                    liquidacionCampoAdicional[] liquidacionInformacionAdicional = new liquidacionCampoAdicional[DataRowsLiquidacionInfoAdicional.LongLength];
                                    foreach (DataRow DataRowLiquiInfoAdicional in DataRowsLiquidacionInfoAdicional)
                                    {
                                        int con = 0;
                                        if (DataRowLiquiInfoAdicional["txNombre"].ToString().Trim().Length != 0 && DataRowLiquiInfoAdicional["txValor"].ToString().Trim().Length != 0)
                                        {
                                            liquidacionCampoAdicional liquiInfoAdicional = new liquidacionCampoAdicional();
                                            liquiInfoAdicional.nombre = DataRowLiquiInfoAdicional["txNombre"].ToString().Trim();
                                            liquiInfoAdicional.Value = DataRowLiquiInfoAdicional["txValor"].ToString().Trim().Contains("&amp;") ? DataRowLiquiInfoAdicional["txValor"].ToString().Trim().Replace("&amp:", "Y") : DataRowLiquiInfoAdicional["txValor"].ToString().Trim().Contains("&") ? DataRowLiquiInfoAdicional["txValor"].ToString().Trim().Replace("&", "Y") : DataRowLiquiInfoAdicional["txValor"].ToString().Trim();
                                            liquidacionInformacionAdicional[cont] = liquiInfoAdicional;
                                            cont++;
                                        }
                                    }
                                    if (cont != 0)
                                        liquidacion.infoAdicional = liquidacionInformacionAdicional;
                                }
                                cont = 0;

                                #region Liquidacion Reembolso

                                if (DataRowLiquidacion["CodDocReembolso"].ToString().Trim().Equals(ConfigurationManager.AppSettings["CodDocReembolso"].Trim()))
                                {
                                    DataSet LiquidacionReembolo = null;
                                    LiquidacionReembolo = ConsultaliquiiReembolso(Convert.ToInt32(DataRowLiquidacion["ciCompania"].ToString().Trim()),
                                                                                  DataRowLiquidacion["txEstablecimiento"].ToString().Trim(),
                                                                                  DataRowLiquidacion["txPuntoEmision"].ToString().Trim(),
                                                                                  DataRowLiquidacion["txSecuencial"].ToString().Trim(),
                                                                                  ref codigoRetorno, ref descripcionRetorno);
                                    reembolsoDetalle[] liquidacionRee = new reembolsoDetalle[LiquidacionReembolo.Tables.Count];
                                    detalleImpuestos[] LiquiiReeDeta = new detalleImpuestos[LiquidacionReembolo.Tables.Count];
                                    cont = 0;
                                    foreach (DataRow liquiiReembolso in LiquidacionReembolo.Tables[0].Rows)
                                    {
                                        reembolsoDetalle liquidReembolso = new reembolsoDetalle();
                                        detalleImpuestos liquidReembolso_ = new detalleImpuestos();
                                        liquidReembolso.tipoIdentificacionProveedorReembolso = liquiiReembolso["txTipoIdentificacionProveedorReembolso"].ToString().Trim();
                                        liquidReembolso.identificacionProveedorReembolso = liquiiReembolso["txIdentificacionProveedorReembolso"].ToString().Trim();
                                        liquidReembolso.codPaisPagoProveedorReembolso = liquiiReembolso["txCodPaisPagoProveedorReembolso"].ToString().Trim();
                                        liquidReembolso.tipoProveedorReembolso = liquiiReembolso["txTipoProveedorReembolso"].ToString().Trim();
                                        liquidReembolso.codDocReembolso = liquiiReembolso["CodDocReembolso"].ToString().Trim();
                                        liquidReembolso.estabDocReembolso = liquiiReembolso["EstabDocReembolso"].ToString().Trim();
                                        liquidReembolso.ptoEmiDocReembolso = liquiiReembolso["PtoEmiDocReembolso"].ToString().Trim();
                                        liquidReembolso.secuencialDocReembolso = liquiiReembolso["SecuencialDocReembolso"].ToString().Trim();
                                        liquidReembolso.fechaEmisionDocReembolso = liquiiReembolso["txFechaEmisionDocReembolso"].ToString().Trim();
                                        liquidReembolso.numeroautorizacionDocReemb = liquiiReembolso["numeroautorizacionDocReemb"].ToString().Trim();
                                        liquidReembolso_.codigo = liquiiReembolso["codigo"].ToString().Trim();
                                        liquidReembolso_.codigoPorcentaje = liquiiReembolso["codigoPorcentaje"].ToString().Trim();
                                        liquidReembolso_.tarifa = liquiiReembolso["tarifa"].ToString().Trim();
                                        liquidReembolso_.baseImponibleReembolso = Convert.ToDecimal(liquiiReembolso["baseImponibleReembolso"].ToString().Trim());
                                        liquidReembolso_.impuestoReembolso = Convert.ToDecimal(liquiiReembolso["impuestoReembolso"].ToString().Trim());
                                        LiquiiReeDeta[cont] = liquidReembolso_;
                                        liquidReembolso.detalleImpuestos = LiquiiReeDeta;
                                        liquidacionRee[cont] = liquidReembolso;
                                        cont++;
                                    }
                                    if (cont != 0)
                                        liquidacion.reembolsos = liquidacionRee;

                                }
                                cont = 0;
                                #endregion


                                #endregion LIQUIDACION_INFO_ADICIONAL

                                #endregion LIQUIDACION

                                #region GENERA_CLAVE_ACCES
                                ClaveAcceso claveAcceso = new ClaveAcceso();
                                if (DataRowLiquidacion["txClaveAcceso"].ToString() == "" || DataRowLiquidacion["txClaveAcceso"].ToString() == null)
                                {
                                    #region Nuevo documento generacion de su clave                               
                                    string ClaveAccesoNormal = claveAcceso.GenerarClaveAccesoDocumento(liquidacion.infoTributaria.codDoc,
                                                                                                       liquidacion.infoTributaria.secuencial.ToString().Trim(),
                                                                                                       liquidacion.infoTributaria.ptoEmi,
                                                                                                       liquidacion.infoTributaria.estab,
                                                                                                       liquidacion.infoLiquidacionCompra.fechaEmision,
                                                                                                       liquidacion.infoTributaria.ruc,
                                                                                                       liquidacion.infoTributaria.ambiente);

                                    if (ClaveAccesoNormal.Length == 49)
                                    {
                                        liquidacion.infoTributaria.claveAcceso = ClaveAccesoNormal;
                                    }
                                    else
                                    {
                                        liquidacion.infoTributaria.claveAcceso = claveAcceso.GenerarClaveAccesoDocumento(liquidacion.infoTributaria.codDoc,
                                                                                                       liquidacion.infoTributaria.secuencial.ToString().Trim(),
                                                                                                       liquidacion.infoTributaria.ptoEmi,
                                                                                                       liquidacion.infoTributaria.estab,
                                                                                                       liquidacion.infoLiquidacionCompra.fechaEmision,
                                                                                                       liquidacion.infoTributaria.ruc,
                                                                                                       liquidacion.infoTributaria.ambiente);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Documento ya contiene una clave de Acceso
                                    if (liquidacion.infoLiquidacionCompra.fechaEmision.Replace("/", "") != DataRowLiquidacion["txClaveAcceso"].ToString().Substring(0, 8))
                                    {
                                        #region Nuevo documento generacion de su clave                               
                                        string ClaveAccesoNormal = claveAcceso.GenerarClaveAccesoDocumento(liquidacion.infoTributaria.codDoc,
                                                                                                       liquidacion.infoTributaria.secuencial.ToString().Trim(),
                                                                                                       liquidacion.infoTributaria.ptoEmi,
                                                                                                       liquidacion.infoTributaria.estab,
                                                                                                       liquidacion.infoLiquidacionCompra.fechaEmision,
                                                                                                       liquidacion.infoTributaria.ruc,
                                                                                                       liquidacion.infoTributaria.ambiente);

                                        if (ClaveAccesoNormal.Length == 49)
                                        {
                                            liquidacion.infoTributaria.claveAcceso = ClaveAccesoNormal;
                                        }
                                        else
                                        {
                                            liquidacion.infoTributaria.claveAcceso = claveAcceso.GenerarClaveAccesoDocumento(liquidacion.infoTributaria.codDoc,
                                                                                                       liquidacion.infoTributaria.secuencial.ToString().Trim(),
                                                                                                       liquidacion.infoTributaria.ptoEmi,
                                                                                                       liquidacion.infoTributaria.estab,
                                                                                                       liquidacion.infoLiquidacionCompra.fechaEmision,
                                                                                                       liquidacion.infoTributaria.ruc,
                                                                                                       liquidacion.infoTributaria.ambiente);
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                                #endregion GENERA_CLAVE_ACCESO
                                
                                Object obj = new Object();
                                obj = liquidacion;
                                xmlGenerado = new XmlGenerados();
                                xmlGenerado.Identity = Convert.ToInt32(DataRowLiquidacion["ciLiquidacion"].ToString().Trim());
                                xmlGenerado.CiCompania = compania.CiCompania;
                                xmlGenerado.XmlComprobante = Serializacion.serializar(obj);
                                xmlGenerado.ClaveAcceso = liquidacion.infoTributaria.claveAcceso;
                                xmlGenerado.NameXml = liquidacion.infoTributaria.claveAcceso;
                                xmlGenerado.CiTipoEmision = liquidacion.infoTributaria.tipoEmision;
                                xmlGenerado.CiTipoDocumento = liquidacion.infoTributaria.codDoc;
                                xmlGenerado.XmlEstado = CatalogoViaDoc.DocEstadoGenerado;
                                xmlGenerado.ciNumeroIntento = numeroIntentosFactura;
                                xmlLiqui.Add(xmlGenerado);

                            }
                            else
                            {
                                if (DataRowLiquidacion["xmlDocumentoAutorizado"].ToString() != "" && DataRowLiquidacion["xmlDocumentoAutorizado"].ToString() != null)
                                {
                                    liquidacion = (liquidacionCompra)Serializacion.desSerializar(DataRowLiquidacion["xmlDocumentoAutorizado"].ToString(), liquidacion.GetType());
                                    xmlGenerado = new XmlGenerados();
                                    xmlGenerado.Identity = Convert.ToInt32(DataRowLiquidacion["ciLiquidacion"].ToString().Trim());
                                    xmlGenerado.CiCompania = compania.CiCompania;
                                    xmlGenerado.XmlComprobante = DataRowLiquidacion["xmlDocumentoAutorizado"].ToString().Trim();
                                    xmlGenerado.ClaveAcceso = liquidacion.infoTributaria.claveAcceso;
                                    xmlGenerado.NameXml = liquidacion.infoTributaria.claveAcceso;
                                    xmlGenerado.CiTipoEmision = liquidacion.infoTributaria.tipoEmision;
                                    xmlGenerado.CiTipoDocumento = liquidacion.infoTributaria.codDoc;
                                    xmlGenerado.CiContingenciaDet = Convert.ToInt32(DataRowLiquidacion["ciContingenciaDet"].ToString().Trim());
                                    xmlGenerado.XmlEstado = ConfigurationManager.AppSettings.Get("GENERADO").Trim();
                                    xmlLiqui.Add(xmlGenerado);
                                }
                            }
                        }

                        catch (Exception ex)
                        {
                            xmlGenerado = new XmlGenerados();
                            xmlGenerado.Identity = Convert.ToInt32(DataRowLiquidacion["ciLiquidacion"].ToString().Trim());
                            xmlGenerado.MensajeError = ex.Message + "||||" + ex.StackTrace;
                            xmlGenerado.XmlEstado = CatalogoViaDoc.DocEstadoEFirmado;
                            xmlGenerado.txCodError = "101";
                            xmlGenerado.CiTipoDocumento = CatalogoViaDoc.DocumentoFactura;
                            xmlGenerado.CiCompania = compania.CiCompania;
                            xmlGenerado.CiContingenciaDet = 1;
                            xmlGenerado.XmlComprobante = Serializacion.serializar(liquidacion);
                            xmlGenerado.NameXml = liquidacion.infoTributaria.claveAcceso;
                            xmlGenerado.ClaveAcceso = liquidacion.infoTributaria.claveAcceso;
                            xmlGenerado.ciNumeroIntento = numeroIntentosFactura + 1;

                            actualizarEstado.ActualizarXmlComprobantes(xmlGenerado);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
            }

            return xmlLiqui;
        }

        protected DataSet LnconsultarLiquidacionDetalleImpuestoAdicional(int op, int ciCompañia, string txEstablecimiento, string txPuntoEmision, string txSecuencial,
                                                                     string txCodigoPrincipal, ref int codigoRetorno, ref string descripcionRetorno)
        {
            DataSet ds = new DataSet();

            ViaDoc.AccesoDatos.DocumentoAD _consultaDocumentos = new ViaDoc.AccesoDatos.DocumentoAD();

            return ds = _consultaDocumentos.DocumentosElectronicos(13, ciCompañia, txEstablecimiento, txPuntoEmision, txSecuencial, txCodigoPrincipal,
                                                                   0, "", ref codigoRetorno, ref descripcionRetorno);
        }

        protected DataSet ConsultaliquiiReembolso(int ciCompañia, string txEstablecimiento, string txPuntoEmision, string txSecuencial,
                                                    ref int codigoRetorno, ref string descripcionRetorno)
        {
            DataSet dsResultado = null;
            ViaDoc.AccesoDatos.DocumentoAD _consultaDocumentos = new ViaDoc.AccesoDatos.DocumentoAD();

            dsResultado = _consultaDocumentos.DocumentosElectronicos(14, ciCompañia, txEstablecimiento, txPuntoEmision, txSecuencial, "",
                                                                   0, "", ref codigoRetorno, ref descripcionRetorno);

            return dsResultado;
        }
    }
}
