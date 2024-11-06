using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using ViaDoc.Configuraciones;
using ViaDoc.EntidadNegocios;
using ViaDoc.Utilitarios;

namespace ViaDocFirma.LogicaNegocios
{
    public class ProcesoFacturas
    {
        FirmaDocumentos actualizarEstado = new FirmaDocumentos();
        public List<XmlGenerados> ProcesaXmlFacturas(Compania compania, string Version, int numeroRegistro)
        {
            int contInicial = 0;
            int codigoRetorno = 0;
            bool banderaTblReemb = false;
            bool banderaReembolso = false;
            string descripcionRetorno = string.Empty;
            int numeroIntentosFactura = 0;
            List<XmlGenerados> xmlFact = new List<XmlGenerados>();
            ViaDoc.AccesoDatos.DocumentoAD _consultaDocumentos = new ViaDoc.AccesoDatos.DocumentoAD();

            try
            {
                DataSet dsFacturas = _consultaDocumentos.DocumentosElectronicos(4, compania.CiCompania, "", "", "", "", numeroRegistro, "", ref codigoRetorno, ref descripcionRetorno);
                if (codigoRetorno.Equals(0))
                {
                    contInicial = dsFacturas.Tables[0].Select("ciEstado='" + CatalogoViaDoc.DocEstadoInicial + "'").ToList().Count;
                    if (contInicial == 0)
                        contInicial = dsFacturas.Tables[0].Select("ciEstado='" + CatalogoViaDoc.DocEstadoEFirmado + "'").ToList().Count;

                    banderaTblReemb = dsFacturas.Tables.Count > 5 ? true : false;

                    if (banderaTblReemb)
                        banderaReembolso = dsFacturas.Tables[5].Rows.Count.Equals(0) ? false : true;

                    dsFacturas.Tables[0].TableName = "Factura";
                    dsFacturas.Tables[1].TableName = "FacturaInfoAdicional";
                    dsFacturas.Tables[2].TableName = "FacturaTotalImpuesto";
                    dsFacturas.Tables[3].TableName = "FacturaDetalle";
                    dsFacturas.Tables[4].TableName = "FacturaDetalleFormaPago";//Nuevo TAB Forma Pago
                    if (banderaTblReemb)
                        dsFacturas.Tables[5].TableName = "ReembolsoGastos";//Nuevo: Reembolso Gastos

                    #region RELACIONO_TABLAS_FACTURA
                    var Pk_Factura = new DataColumn[4];
                    var Fk_FacturaInfoAdicional = new DataColumn[4];
                    var Fk_FacturaTotalImpuesto = new DataColumn[4];
                    //Nuevo TAB Forma Pago
                    var Fk_FacturaFormaPago = new DataColumn[4];
                    var Fk_FacturaDetalle = new DataColumn[4];
                    var Fk_FacturaReembolso = banderaTblReemb ? new DataColumn[4] : null;//Nuevo: Reembolso Gastos

                    //Factura
                    Pk_Factura[0] = dsFacturas.Tables["Factura"].Columns["ciCompania"];
                    Pk_Factura[1] = dsFacturas.Tables["Factura"].Columns["txEstablecimiento"];
                    Pk_Factura[2] = dsFacturas.Tables["Factura"].Columns["txPuntoEmision"];
                    Pk_Factura[3] = dsFacturas.Tables["Factura"].Columns["txSecuencial"];

                    //FacturaInfoAdicional
                    Fk_FacturaInfoAdicional[0] = dsFacturas.Tables["FacturaInfoAdicional"].Columns["ciCompania"];
                    Fk_FacturaInfoAdicional[1] = dsFacturas.Tables["FacturaInfoAdicional"].Columns["txEstablecimiento"];
                    Fk_FacturaInfoAdicional[2] = dsFacturas.Tables["FacturaInfoAdicional"].Columns["txPuntoEmision"];
                    Fk_FacturaInfoAdicional[3] = dsFacturas.Tables["FacturaInfoAdicional"].Columns["txSecuencial"];

                    //FacturaTotalImpuesto
                    Fk_FacturaTotalImpuesto[0] = dsFacturas.Tables["FacturaTotalImpuesto"].Columns["ciCompania"];
                    Fk_FacturaTotalImpuesto[1] = dsFacturas.Tables["FacturaTotalImpuesto"].Columns["txEstablecimiento"];
                    Fk_FacturaTotalImpuesto[2] = dsFacturas.Tables["FacturaTotalImpuesto"].Columns["txPuntoEmision"];
                    Fk_FacturaTotalImpuesto[3] = dsFacturas.Tables["FacturaTotalImpuesto"].Columns["txSecuencial"];

                    //FacturaDetalle fk
                    Fk_FacturaDetalle[0] = dsFacturas.Tables["FacturaDetalle"].Columns["ciCompania"];
                    Fk_FacturaDetalle[1] = dsFacturas.Tables["FacturaDetalle"].Columns["txEstablecimiento"];
                    Fk_FacturaDetalle[2] = dsFacturas.Tables["FacturaDetalle"].Columns["txPuntoEmision"];
                    Fk_FacturaDetalle[3] = dsFacturas.Tables["FacturaDetalle"].Columns["txSecuencial"];

                    //FacturaDetalleformaPago fk
                    //Nuevo TAB Forma Pago
                    Fk_FacturaFormaPago[0] = dsFacturas.Tables["FacturaDetalleFormaPago"].Columns["ciCompania"];
                    Fk_FacturaFormaPago[1] = dsFacturas.Tables["FacturaDetalleFormaPago"].Columns["txEstablecimiento"];
                    Fk_FacturaFormaPago[2] = dsFacturas.Tables["FacturaDetalleFormaPago"].Columns["txPuntoEmision"];
                    Fk_FacturaFormaPago[3] = dsFacturas.Tables["FacturaDetalleFormaPago"].Columns["txSecuencial"];

                    //Nuevo: Reembolso Gastos
                    if (banderaReembolso)
                    {
                        Fk_FacturaReembolso[0] = dsFacturas.Tables["ReembolsoGastos"].Columns["ciCompania"];
                        Fk_FacturaReembolso[1] = dsFacturas.Tables["ReembolsoGastos"].Columns["txEstablecimiento"];
                        Fk_FacturaReembolso[2] = dsFacturas.Tables["ReembolsoGastos"].Columns["txPuntoEmision"];
                        Fk_FacturaReembolso[3] = dsFacturas.Tables["ReembolsoGastos"].Columns["txSecuencial"];
                    }

                    //
                    dsFacturas.Relations.Add("FACTURAS_FacturaInfoAdicional", Pk_Factura, Fk_FacturaInfoAdicional);
                    dsFacturas.Relations.Add("FACTURAS_FacturaTotalImpuesto", Pk_Factura, Fk_FacturaTotalImpuesto);
                    dsFacturas.Relations.Add("FACTURAS_FacturaDetalle", Pk_Factura, Fk_FacturaDetalle);
                    dsFacturas.Relations.Add("FACTURAS_FacturaFormaPago", Pk_Factura, Fk_FacturaFormaPago);//Nuevo TAB Forma Pago
                    if (banderaReembolso)
                        dsFacturas.Relations.Add("FACTURAS_ReembolsoGastos", Pk_Factura, Fk_FacturaReembolso);//Nuevo: Reembolso Gastos

                    #endregion RELACIONO_TABLAS_FACTURA

                    #region LLENA CLASES FACTURAS 

                    #region INFORMACION_TRIBUTARIA
                    factura Factura = new factura();
                    Factura.id = facturaID.comprobante;
                    Factura.idSpecified = true;
                    Factura.version = Version;

                    //compañia
                    infoTributaria_FA infoTributariaFA = new infoTributaria_FA();
                    infoTributariaFA.ambiente = compania.CiTipoAmbiente.ToString().Trim();
                    infoTributariaFA.dirMatriz = compania.TxDireccionMatriz; //proviene de Compania
                    if (compania.TxNombreComercial.Trim().Length != 0)
                        infoTributariaFA.nombreComercial = compania.TxNombreComercial;  //proviene de Compania 

                    infoTributariaFA.razonSocial = compania.TxRazonSocial;  //proviene de Compania
                    infoTributariaFA.ruc = compania.TxRuc;   //proviene de Compania

                    facturaInfoFactura FacturaInfoFactura = new facturaInfoFactura();
                    if (compania.TxContribuyenteEspecial.Trim().Length != 0)
                        FacturaInfoFactura.contribuyenteEspecial = compania.TxContribuyenteEspecial; //proviene de Compania

                    if (compania.TxObligadoContabilidad.Trim().Length != 0)
                    {
                        switch (compania.TxObligadoContabilidad)
                        {
                            case "S":
                                FacturaInfoFactura.obligadoContabilidad = "SI"; //proviene de Compania
                                break;
                            case "N":
                                FacturaInfoFactura.obligadoContabilidad = "NO"; //proviene de Compania
                                break;
                        }
                    }

                    #region Configuracion de Leyenda

                    #region Agente de retención
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
                                            infoTributariaFA.agenteRetencion = CatalogoViaDoc.LeyendaAgente.Trim().Replace("Agente de Retención Resolución Nro ", "");
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region Regimen Microempresas

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
                                            infoTributariaFA.regimenMicroempresas = CatalogoViaDoc.LeyendaRegimen.Trim();
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region Regimen Rimpe
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
                                            infoTributariaFA.contribuyenteRimpe = CatalogoViaDoc.LeyendaRegimenRimpe.Trim();
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #endregion

                    #endregion INFORMACION_TRIBUTARIA
                    int cntCiclo = 0;
                    foreach (DataRow DataRowFactura in dsFacturas.Tables["Factura"].Rows)
                    {
                        int ciFactura = Convert.ToInt32(DataRowFactura["ciFactura"].ToString().Trim());

                        var claveAccesoGenerada = string.Empty;
                        cntCiclo++;
                        try
                        {

                            if (DataRowFactura["ciEstado"].ToString().Trim() == CatalogoViaDoc.DocEstadoInicial
                                || DataRowFactura["ciEstado"].ToString().Trim() == CatalogoViaDoc.DocEstadoEFirmado)
                            {
                                #region FACTURA
                                infoTributariaFA.claveAcceso = DataRowFactura["txClaveAcceso"].ToString().Trim();  //proviene de CompRetencion
                                infoTributariaFA.estab = DataRowFactura["txEstablecimiento"].ToString().Trim();     //proviene de CompRetencion
                                infoTributariaFA.ptoEmi = DataRowFactura["txPuntoEmision"].ToString().Trim();    //proviene de CompRetencion          
                                infoTributariaFA.secuencial = DataRowFactura["txSecuencial"].ToString().Trim();    //proviene de CompRetencion
                                infoTributariaFA.tipoEmision = DataRowFactura["ciTipoEmision"].ToString().Trim();   //proviene de CompRetencion
                                infoTributariaFA.codDoc = DataRowFactura["ciTipoDocumento"].ToString().Trim();
                                Factura.infoTributaria = infoTributariaFA;
                                FacturaInfoFactura.fechaEmision = DataRowFactura["txFechaEmision"].ToString().Trim();

                                numeroIntentosFactura = int.Parse(DataRowFactura["ciNumeroIntento"].ToString().Trim());

                                if (DataRowFactura["txDireccion"].ToString().Trim().Length != 0)
                                {
                                    FacturaInfoFactura.dirEstablecimiento = DataRowFactura["txDireccion"].ToString().Trim();
                                }
                                FacturaInfoFactura.tipoIdentificacionComprador = DataRowFactura["ciTipoIdentificacionComprador"].ToString().Trim();

                                if (DataRowFactura["txGuiaRemision"].ToString().Trim().Length != 0)
                                {
                                    FacturaInfoFactura.guiaRemision = DataRowFactura["txGuiaRemision"].ToString().Trim();
                                }
                                else
                                {
                                    FacturaInfoFactura.guiaRemision = null;
                                }

                                FacturaInfoFactura.razonSocialComprador = DataRowFactura["txRazonSocialComprador"].ToString().Trim().Contains("&amp;") ?
                                    DataRowFactura["txRazonSocialComprador"].ToString().Trim().Replace("&", "&amp;") : DataRowFactura["txRazonSocialComprador"].ToString().Trim();
                                FacturaInfoFactura.identificacionComprador = DataRowFactura["txIdentificacionComprador"].ToString().Trim();
                                FacturaInfoFactura.totalSinImpuestos = Convert.ToDecimal(DataRowFactura["qnTotalSinImpuestos"].ToString().Trim());
                                FacturaInfoFactura.totalDescuento = Convert.ToDecimal(DataRowFactura["qnTotalDescuento"].ToString().Trim());
                                FacturaInfoFactura.propina = Convert.ToDecimal(DataRowFactura["qnPropina"].ToString().Trim());
                                FacturaInfoFactura.importeTotal = Convert.ToDecimal(DataRowFactura["qnImporteTotal"].ToString().Trim());

                                if (DataRowFactura["txMoneda"].ToString().Trim().Length != 0)
                                {
                                    FacturaInfoFactura.moneda = DataRowFactura["txMoneda"].ToString().Trim();
                                }
                                Factura.infoFactura = FacturaInfoFactura;

                                #region FACTURA_TOTAL_IMPUESTO
                                int cont = 0;
                                DataRow[] DataRowsFacturaTotalImpuesto = DataRowFactura.GetChildRows("FACTURAS_FacturaTotalImpuesto");
                                if (DataRowsFacturaTotalImpuesto.LongLength > 0)
                                {
                                    facturaInfoFacturaTotalImpuesto[] factTotalImpuestos = new facturaInfoFacturaTotalImpuesto[DataRowsFacturaTotalImpuesto.LongLength];
                                    foreach (DataRow DataRowFactTotalImpuesto in DataRowsFacturaTotalImpuesto)
                                    {
                                        facturaInfoFacturaTotalImpuesto factTotalImpuesto = new facturaInfoFacturaTotalImpuesto();

                                        factTotalImpuesto.codigo = DataRowFactTotalImpuesto["txCodigo"].ToString().Trim();
                                        factTotalImpuesto.codigoPorcentaje = DataRowFactTotalImpuesto["txCodigoPorcentaje"].ToString().Trim();
                                        factTotalImpuesto.baseImponible = Convert.ToDecimal(DataRowFactTotalImpuesto["qnBaseImponible"].ToString().Trim());
                                        if (DataRowFactTotalImpuesto["txTarifa"].ToString().Trim() != "")
                                        {
                                            factTotalImpuesto.tarifaSpecified = true;
                                            factTotalImpuesto.tarifa = Convert.ToDecimal(DataRowFactTotalImpuesto["txTarifa"].ToString().Trim());
                                        }
                                        else
                                        {
                                            factTotalImpuesto.tarifaSpecified = false;
                                        }

                                        factTotalImpuesto.valor = Convert.ToDecimal(DataRowFactTotalImpuesto["qnValor"].ToString().Trim());
                                        factTotalImpuestos[cont] = factTotalImpuesto;
                                        cont++;
                                    }

                                    Factura.infoFactura.totalConImpuestos = factTotalImpuestos;
                                }
                                cont = 0;
                                #endregion FACTURA_TOTAL_IMPUESTO

                                #region FACTURA_FORMA_PAGO
                                cont = 0;
                                DataRow[] DataRowsFacturaFormaPago = DataRowFactura.GetChildRows("FACTURAS_FacturaFormaPago");
                                if (DataRowsFacturaFormaPago.LongLength > 0)
                                {
                                    facturaInfoFacturaFormaPago[] factFormaPagos = new facturaInfoFacturaFormaPago[DataRowsFacturaFormaPago.LongLength];
                                    foreach (DataRow DataRowFacturaFormaPago in DataRowsFacturaFormaPago)
                                    {
                                        facturaInfoFacturaFormaPago factFormaPago = new facturaInfoFacturaFormaPago();

                                        factFormaPago.formaPago = DataRowFacturaFormaPago["txFormaPago"].ToString().Trim();
                                        factFormaPago.plazo = int.Parse(DataRowFacturaFormaPago["txPlazo"].ToString().Trim());
                                        factFormaPago.total = Convert.ToDecimal(DataRowFacturaFormaPago["qnTotal"].ToString().Trim());
                                        factFormaPago.unidadTiempo = DataRowFacturaFormaPago["txUnidadTiempo"].ToString().Trim();

                                        factFormaPagos[cont] = factFormaPago;
                                        cont++;
                                    }
                                    Factura.infoFactura.pagos = factFormaPagos;
                                }
                                cont = 0;
                                #endregion FACTURA_FORMA_PAGO

                                #region FACTURA_DETALLE
                                cont = 0;
                                DataRow[] DataRowsFacturaDetalle = DataRowFactura.GetChildRows("FACTURAS_FacturaDetalle");
                                if (DataRowsFacturaDetalle.LongLength > 0)
                                {
                                    facturaDetalle[] factDetalles = new facturaDetalle[DataRowsFacturaDetalle.LongLength];
                                    foreach (DataRow DataRowFactDetalle in DataRowsFacturaDetalle)
                                    {
                                        int contFactDetImpuesto = 0;
                                        facturaDetalle factDetalle = new facturaDetalle();
                                        if (DataRowFactDetalle["txCodigoPrincipal"].ToString().Trim().Length != 0)
                                            factDetalle.codigoPrincipal = DataRowFactDetalle["txCodigoPrincipal"].ToString().Trim();
                                        if (DataRowFactDetalle["txCodigoAuxiliar"].ToString().Trim().Length != 0)
                                            factDetalle.codigoAuxiliar = DataRowFactDetalle["txCodigoAuxiliar"].ToString().Trim();
                                        factDetalle.descripcion = DataRowFactDetalle["txDescripcion"].ToString().Trim().Contains("&amp;") ?
                                            DataRowFactDetalle["txDescripcion"].ToString().Trim().Replace("&amp:", "Y") : DataRowFactDetalle["txDescripcion"].ToString().Trim().Contains("&") ?
                                            DataRowFactDetalle["txDescripcion"].ToString().Trim().Replace("&", "Y") : DataRowFactDetalle["txDescripcion"].ToString().Trim();
                                        factDetalle.cantidad = Convert.ToInt32(DataRowFactDetalle["qnCantidad"].ToString().Trim());
                                        factDetalle.precioUnitario = Convert.ToDecimal(DataRowFactDetalle["qnPrecioUnitario"].ToString().Trim());
                                        factDetalle.descuento = Convert.ToDecimal(DataRowFactDetalle["qnDescuento"].ToString().Trim());
                                        factDetalle.precioTotalSinImpuesto = Convert.ToDecimal(DataRowFactDetalle["qnPrecioTotalSinImpuesto"].ToString().Trim());


                                        #region CAPTURO_FACTURA_DETALLE_IMPUESTO
                                        DataSet FactDetalleImpuestoAdicional = new DataSet();
                                        FactDetalleImpuestoAdicional = LnconsultarFacturaDetalleImpuestoAdicional(
                                         1, Convert.ToInt32(DataRowFactDetalle["ciCompania"].ToString().Trim()),
                                            DataRowFactDetalle["txEstablecimiento"].ToString().Trim(),
                                            DataRowFactDetalle["txPuntoEmision"].ToString().Trim(),
                                            DataRowFactDetalle["txSecuencial"].ToString().Trim(),
                                            DataRowFactDetalle["txCodigoPrincipal"].ToString().Trim(),
                                            ref codigoRetorno, ref descripcionRetorno);

                                        if (codigoRetorno.Equals(0))
                                        {
                                            FactDetalleImpuestoAdicional.Tables[0].TableName = "FacturaDetalleImpuesto";
                                            FactDetalleImpuestoAdicional.Tables[1].TableName = "FacturaDetalleAdicional";
                                            #endregion CAPTURO_FACTURA_DETALLE_IMPUESTO

                                            #region FACTURA_DETALLE_ADICIONAL

                                            int DataRowsFacturaDetalleAdicional = FactDetalleImpuestoAdicional.Tables["FacturaDetalleAdicional"].Rows.Count;
                                            if (DataRowsFacturaDetalleAdicional > 0)
                                            {
                                                facturaDetalleDetAdicional[] factDetDetalleAdicional = new facturaDetalleDetAdicional[DataRowsFacturaDetalleAdicional];

                                                foreach (DataRow DataRowFactDetalleAdicional in FactDetalleImpuestoAdicional.Tables["FacturaDetalleAdicional"].Rows)
                                                {
                                                    if (DataRowFactDetalleAdicional["txNombre"].ToString().Trim().Length != 0 && DataRowFactDetalleAdicional["txValor"].ToString().Trim().Length != 0)
                                                    {
                                                        facturaDetalleDetAdicional factDetalleAdicional = new facturaDetalleDetAdicional();
                                                        if (DataRowFactDetalleAdicional["txNombre"].ToString().Trim().Length != 0)
                                                            factDetalleAdicional.nombre = DataRowFactDetalleAdicional["txNombre"].ToString().Trim();
                                                        if (DataRowFactDetalleAdicional["txValor"].ToString().Trim().Length != 0)
                                                            factDetalleAdicional.valor = DataRowFactDetalleAdicional["txValor"].ToString().Trim().Contains("&amp;") ?
                                                                DataRowFactDetalleAdicional["txValor"].ToString().Trim().Replace("&amp:", "Y") : DataRowFactDetalleAdicional["txValor"].ToString().Trim().Contains("&") ?
                                                                DataRowFactDetalleAdicional["txValor"].ToString().Trim().Replace("&", "Y") : DataRowFactDetalleAdicional["txValor"].ToString().Trim();

                                                        factDetDetalleAdicional[contFactDetImpuesto] = factDetalleAdicional;
                                                        contFactDetImpuesto++;
                                                    }
                                                }

                                                if (contFactDetImpuesto != 0)
                                                    factDetalle.detallesAdicionales = factDetDetalleAdicional;
                                            }
                                            contFactDetImpuesto = 0;

                                            #endregion FACTURA_DETALLE_ADICIONAL

                                            #region FACTURA_DETALLE_IMPUESTO

                                            int DataRowsFacturaDetalleImpuesto = FactDetalleImpuestoAdicional.Tables["FacturaDetalleImpuesto"].Rows.Count;
                                            if (DataRowsFacturaDetalleImpuesto > 0)
                                            {
                                                impuesto_FA[] factdetalleImpuestos = new impuesto_FA[DataRowsFacturaDetalleImpuesto];

                                                foreach (DataRow DataRowFactDetalleImpuesto in FactDetalleImpuestoAdicional.Tables["FacturaDetalleImpuesto"].Rows)
                                                {
                                                    impuesto_FA factdetalleImpuesto = new impuesto_FA();
                                                    factdetalleImpuesto.codigo = DataRowFactDetalleImpuesto["txCodigo"].ToString().Trim();
                                                    factdetalleImpuesto.codigoPorcentaje = DataRowFactDetalleImpuesto["txCodigoPorcentaje"].ToString().Trim();
                                                    factdetalleImpuesto.baseImponible = Convert.ToDecimal(DataRowFactDetalleImpuesto["qnBaseImponible"].ToString().Trim());
                                                    factdetalleImpuesto.tarifa = Convert.ToDecimal(DataRowFactDetalleImpuesto["txTarifa"].ToString().Trim());// alerta hay q hacer un calculo;
                                                    factdetalleImpuesto.valor = Convert.ToDecimal(DataRowFactDetalleImpuesto["qnValor"].ToString().Trim());
                                                    factdetalleImpuestos[contFactDetImpuesto] = factdetalleImpuesto;

                                                    contFactDetImpuesto++;
                                                }
                                                factDetalle.impuestos = factdetalleImpuestos;
                                            }
                                            contFactDetImpuesto = 0;

                                            #endregion FACTURA_DETALLE_IMPUESTO
                                        }
                                        factDetalles[cont] = factDetalle;
                                        cont++;

                                        FactDetalleImpuestoAdicional.Dispose();
                                    }

                                    Factura.detalles = factDetalles;
                                }
                                cont = 0;
                                #endregion FACTURA_DETALLE

                                #region FACTURA_INFO_ADICIONAL
                                DataRow[] DataRowsFacturaInfoAdicional = DataRowFactura.GetChildRows("FACTURAS_FacturaInfoAdicional");
                                cont = 0;
                                if (DataRowsFacturaInfoAdicional.LongLength > 0)
                                {
                                    facturaCampoAdicional[] facturaInformacionAdicional = new facturaCampoAdicional[DataRowsFacturaInfoAdicional.LongLength];
                                    foreach (DataRow DataRowFactInfoAdicional in DataRowsFacturaInfoAdicional)
                                    {
                                        if (DataRowFactInfoAdicional["txNombre"].ToString().Trim().Length != 0 && DataRowFactInfoAdicional["txValor"].ToString().Trim().Length != 0)
                                        {
                                            facturaCampoAdicional factInfoAdicional = new facturaCampoAdicional();
                                            factInfoAdicional.nombre = DataRowFactInfoAdicional["txNombre"].ToString().Trim();
                                            factInfoAdicional.Value = DataRowFactInfoAdicional["txValor"].ToString().Trim().Contains("&amp;") ?
                                                DataRowFactInfoAdicional["txValor"].ToString().Trim().Replace("&amp:", "Y") : DataRowFactInfoAdicional["txValor"].ToString().Trim().Contains("&") ?
                                                DataRowFactInfoAdicional["txValor"].ToString().Trim().Replace("&", "Y") : DataRowFactInfoAdicional["txValor"].ToString().Trim();
                                            facturaInformacionAdicional[cont] = factInfoAdicional;
                                            cont++;
                                        }
                                    }
                                    if (cont != 0)
                                        Factura.infoAdicional = facturaInformacionAdicional;
                                }

                                cont = 0;
                                #endregion FACTURA_INFO_ADICIONAL

                                #region FACTURA_REEMBOLSO_GASTOS

                                if (banderaReembolso)
                                {
                                    DataRow[] DataRowsFacturaReembolsoGastos = DataRowFactura.GetChildRows("FACTURAS_ReembolsoGastos");
                                    cont = 0;
                                    //Limpiar campos reembolso
                                    Factura.reembolsos = null;
                                    Factura.infoFactura.codDocReembolso = null;
                                    Factura.infoFactura.totalComprobantesReembolso = null;
                                    Factura.infoFactura.totalBaseImponibleReembolso = null;
                                    Factura.infoFactura.totalImpuestoReembolso = null;

                                    if (DataRowsFacturaReembolsoGastos.LongLength > 0)
                                    {
                                        reembolsoDetalle[] facturaReembolsoDetalle = new reembolsoDetalle[DataRowsFacturaReembolsoGastos.LongLength];
                                        decimal totalComprobantesReembolso = 0;
                                        decimal totalBaseImponibleReembolso = 0;
                                        decimal totalImpuestoReembolso = 0;
                                        foreach (DataRow DataRowFacturaReembolsoGastos in DataRowsFacturaReembolsoGastos)
                                        {
                                            reembolsoDetalle reemDetalle = new reembolsoDetalle();
                                            reemDetalle.tipoIdentificacionProveedorReembolso = DataRowFacturaReembolsoGastos["txTipoIdProveedor"].ToString().Trim();
                                            reemDetalle.identificacionProveedorReembolso = DataRowFacturaReembolsoGastos["txIdProveedor"].ToString().Trim();
                                            reemDetalle.codPaisPagoProveedorReembolso = DataRowFacturaReembolsoGastos["txIdProveedor"].ToString().Trim();
                                            reemDetalle.tipoProveedorReembolso = DataRowFacturaReembolsoGastos["txIdProveedor"].ToString().Trim();
                                            reemDetalle.codPaisPagoProveedorReembolso = DataRowFacturaReembolsoGastos["codPaisPagoProveedor"].ToString().Trim();
                                            reemDetalle.tipoProveedorReembolso = DataRowFacturaReembolsoGastos["tipoProveedor"].ToString().Trim();
                                            reemDetalle.codDocReembolso = DataRowFacturaReembolsoGastos["ciTipoDocumento"].ToString().Trim();

                                            string espese = DataRowFacturaReembolsoGastos["txNumDocumento"].ToString().Trim();
                                            if (espese != null || espese != "" || espese != string.Empty)
                                            {
                                                string[] partes = espese.Split('-');
                                                reemDetalle.estabDocReembolso = partes[0];
                                                reemDetalle.ptoEmiDocReembolso = partes[1];
                                                reemDetalle.secuencialDocReembolso = partes[2];
                                            }
                                            else
                                            {
                                                reemDetalle.estabDocReembolso = DataRowFacturaReembolsoGastos["txEstablecimiento"].ToString().Trim();
                                                reemDetalle.ptoEmiDocReembolso = DataRowFacturaReembolsoGastos["txPuntoEmision"].ToString().Trim();
                                                reemDetalle.secuencialDocReembolso = DataRowFacturaReembolsoGastos["txSecuencial"].ToString().Trim();
                                            }

                                            reemDetalle.fechaEmisionDocReembolso = DataRowFacturaReembolsoGastos["txFechaEmision"].ToString().Trim();
                                            reemDetalle.numeroautorizacionDocReemb = DataRowFacturaReembolsoGastos["txClaveAcceso"].ToString().Trim();

                                            detalleImpuestos[] facturaReeDetalle = new detalleImpuestos[1];

                                            detalleImpuestos reemImpuestoIVA_ = new detalleImpuestos();
                                            reemImpuestoIVA_.codigo = DataRowFacturaReembolsoGastos["codigo"].ToString().Trim();
                                            reemImpuestoIVA_.codigoPorcentaje = DataRowFacturaReembolsoGastos["codigoPorcentaje"].ToString().Trim();
                                            reemImpuestoIVA_.tarifa = DataRowFacturaReembolsoGastos["tarifa"].ToString().Trim();
                                            reemImpuestoIVA_.baseImponibleReembolso = Convert.ToDecimal(DataRowFacturaReembolsoGastos["valorBase"]);
                                            reemImpuestoIVA_.impuestoReembolso = Convert.ToDecimal(DataRowFacturaReembolsoGastos["impIva"] ?? "0.00");

                                            totalComprobantesReembolso += (reemImpuestoIVA_.baseImponibleReembolso + reemImpuestoIVA_.impuestoReembolso);
                                            totalBaseImponibleReembolso += reemImpuestoIVA_.baseImponibleReembolso;
                                            totalImpuestoReembolso += reemImpuestoIVA_.impuestoReembolso;

                                            facturaReeDetalle[0] = reemImpuestoIVA_;

                                            reemDetalle.detalleImpuestos = facturaReeDetalle;

                                            facturaReembolsoDetalle[cont] = reemDetalle;
                                            cont++;
                                        }
                                        if (cont != 0)
                                        {
                                            Factura.reembolsos = facturaReembolsoDetalle;
                                            Factura.infoFactura.codDocReembolso = "41";
                                            Factura.infoFactura.totalComprobantesReembolso = totalComprobantesReembolso.ToString(System.Globalization.CultureInfo.InvariantCulture);
                                            Factura.infoFactura.totalBaseImponibleReembolso = totalBaseImponibleReembolso.ToString(System.Globalization.CultureInfo.InvariantCulture);
                                            Factura.infoFactura.totalImpuestoReembolso = totalImpuestoReembolso.ToString(System.Globalization.CultureInfo.InvariantCulture);
                                        }
                                    }
                                    cont = 0;
                                }
                                #endregion FACTURA_REEMBOLSO_GASTOS

                                #endregion FACTURA

                                #region GENERA_CLAVE_ACCESO
                                ClaveAcceso claveAcceso = new ClaveAcceso();
                                if (DataRowFactura["txClaveAcceso"].ToString() == "" || DataRowFactura["txClaveAcceso"].ToString() == null)
                                {
                                    #region Nuevo documento generacion de su clave                               
                                    string ClaveAccesoNormal = claveAcceso.GenerarClaveAccesoDocumento(Factura.infoTributaria.codDoc,
                                                                                                       Factura.infoTributaria.secuencial.ToString().Trim(),
                                                                                                       Factura.infoTributaria.ptoEmi,
                                                                                                       Factura.infoTributaria.estab,
                                                                                                       Factura.infoFactura.fechaEmision,
                                                                                                       Factura.infoTributaria.ruc,
                                                                                                       Factura.infoTributaria.ambiente);

                                    if (ClaveAccesoNormal.Length == 49)
                                    {
                                        Factura.infoTributaria.claveAcceso = ClaveAccesoNormal;
                                    }
                                    else
                                    {
                                        Factura.infoTributaria.claveAcceso = claveAcceso.GenerarClaveAccesoDocumento(Factura.infoTributaria.codDoc,
                                                                                                       Factura.infoTributaria.secuencial.ToString().Trim(),
                                                                                                       Factura.infoTributaria.ptoEmi,
                                                                                                       Factura.infoTributaria.estab,
                                                                                                       Factura.infoFactura.fechaEmision,
                                                                                                       Factura.infoTributaria.ruc,
                                                                                                       Factura.infoTributaria.ambiente);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Documento ya contiene una clave de Acceso
                                    if (Factura.infoFactura.fechaEmision.Replace("/", "") != DataRowFactura["txClaveAcceso"].ToString().Substring(0, 8))
                                    {
                                        #region Nuevo documento generacion de su clave                               
                                        string ClaveAccesoNormal = claveAcceso.GenerarClaveAccesoDocumento(Factura.infoTributaria.codDoc,
                                                                                                       Factura.infoTributaria.secuencial.ToString().Trim(),
                                                                                                       Factura.infoTributaria.ptoEmi,
                                                                                                       Factura.infoTributaria.estab,
                                                                                                       Factura.infoFactura.fechaEmision,
                                                                                                       Factura.infoTributaria.ruc,
                                                                                                       Factura.infoTributaria.ambiente);

                                        if (ClaveAccesoNormal.Length == 49)
                                        {
                                            Factura.infoTributaria.claveAcceso = ClaveAccesoNormal;
                                        }
                                        else
                                        {
                                            Factura.infoTributaria.claveAcceso = claveAcceso.GenerarClaveAccesoDocumento(Factura.infoTributaria.codDoc,
                                                                                                       Factura.infoTributaria.secuencial.ToString().Trim(),
                                                                                                       Factura.infoTributaria.ptoEmi,
                                                                                                       Factura.infoTributaria.estab,
                                                                                                       Factura.infoFactura.fechaEmision,
                                                                                                       Factura.infoTributaria.ruc,
                                                                                                       Factura.infoTributaria.ambiente);
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                                #endregion GENERA_CLAVE_ACCESO
                                //Object obj = new Object();
                                var obj = Factura;
                                var xmlGenerado = new XmlGenerados();
                                xmlGenerado.Identity = Convert.ToInt32(DataRowFactura["ciFactura"].ToString().Trim());
                                xmlGenerado.CiCompania = compania.CiCompania;
                                xmlGenerado.XmlComprobante = Serializacion.serializar(obj);
                                xmlGenerado.ClaveAcceso = Factura.infoTributaria.claveAcceso;
                                xmlGenerado.NameXml = Factura.infoTributaria.claveAcceso;
                                xmlGenerado.CiTipoEmision = Factura.infoTributaria.tipoEmision;
                                xmlGenerado.CiTipoDocumento = Factura.infoTributaria.codDoc;
                                xmlGenerado.XmlEstado = CatalogoViaDoc.DocEstadoGenerado;
                                xmlGenerado.ciNumeroIntento = numeroIntentosFactura;
                                xmlFact.Add(xmlGenerado);

                            }
                            else
                            {
                                if (DataRowFactura["xmlDocumentoAutorizado"].ToString() != "" && DataRowFactura["xmlDocumentoAutorizado"].ToString() != null)
                                {
                                    Factura = (factura)Serializacion.desSerializar(DataRowFactura["xmlDocumentoAutorizado"].ToString(), Factura.GetType());
                                    var xmlGenerado = new XmlGenerados();
                                    xmlGenerado.Identity = Convert.ToInt32(DataRowFactura["ciFactura"].ToString().Trim());
                                    xmlGenerado.CiCompania = compania.CiCompania;
                                    xmlGenerado.XmlComprobante = DataRowFactura["xmlDocumentoAutorizado"].ToString().Trim();
                                    xmlGenerado.ClaveAcceso = Factura.infoTributaria.claveAcceso;
                                    xmlGenerado.NameXml = Factura.infoTributaria.claveAcceso;
                                    xmlGenerado.CiTipoEmision = Factura.infoTributaria.tipoEmision;
                                    xmlGenerado.CiTipoDocumento = Factura.infoTributaria.codDoc;
                                    xmlGenerado.CiContingenciaDet = Convert.ToInt32(DataRowFactura["ciContingenciaDet"].ToString().Trim());
                                    xmlGenerado.XmlEstado = ConfigurationManager.AppSettings.Get("GENERADO").Trim();
                                    xmlFact.Add(xmlGenerado);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            var xmlGenerado = new XmlGenerados();
                            xmlGenerado.Identity = Convert.ToInt32(DataRowFactura["ciFactura"].ToString().Trim());
                            xmlGenerado.MensajeError = ex.Message + "||||" + ex.StackTrace;
                            xmlGenerado.XmlEstado = CatalogoViaDoc.DocEstadoEFirmado;
                            xmlGenerado.txCodError = "101";
                            xmlGenerado.CiTipoDocumento = CatalogoViaDoc.DocumentoFactura;
                            xmlGenerado.CiCompania = compania.CiCompania;
                            xmlGenerado.CiContingenciaDet = 1;
                            xmlGenerado.XmlComprobante = Serializacion.serializar(Factura);
                            xmlGenerado.NameXml = Factura.infoTributaria.claveAcceso;
                            xmlGenerado.ClaveAcceso = Factura.infoTributaria.claveAcceso;
                            xmlGenerado.ciNumeroIntento = numeroIntentosFactura + 1;

                            actualizarEstado.ActualizarXmlComprobantes(xmlGenerado);
                        }
                    }
                    dsFacturas.Dispose();
                    #endregion FACTURAS
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
            }
            return xmlFact;
        }

        protected DataSet LnconsultarFacturaDetalleImpuestoAdicional(int op, int ciCompañia, string txEstablecimiento, string txPuntoEmision, string txSecuencial,
                                                                     string txCodigoPrincipal, ref int codigoRetorno, ref string descripcionRetorno)
        {
            DataSet ds = new DataSet();

            ViaDoc.AccesoDatos.DocumentoAD _consultaDocumentos = new ViaDoc.AccesoDatos.DocumentoAD();

            return ds = _consultaDocumentos.DocumentosElectronicos(8, ciCompañia, txEstablecimiento, txPuntoEmision, txSecuencial, txCodigoPrincipal,
                                                                   0, "", ref codigoRetorno, ref descripcionRetorno);
        }
    }
}
