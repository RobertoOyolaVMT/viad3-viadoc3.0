using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using ViaDoc.EntidadNegocios;
using ViaDoc.EntidadNegocios.compRetencion;
using ViaDoc.EntidadNegocios.factura;
using ViaDoc.EntidadNegocios.guiaRemision;
using ViaDoc.EntidadNegocios.Liquidacion;
using ViaDoc.EntidadNegocios.notaCredito;
using ViaDoc.EntidadNegocios.notaDebito;
using ViaDoc.LogicaNegocios;
using ViaDoc.LogicaNegocios.documentos;
using ViaDoc.Logs;
using ViaDoc.ServicioWcf.modelo;
using ViaDoc.Utilitarios;


namespace ViaDoc.ServicioWcf
{
    public class MetodosWcf
    {
        ConexionBDMongo objLogs = new ConexionBDMongo();
        Documentos documentos = new Documentos();

        //llamada a los metodos de viadoc
        public FacturaResponse ProcesarFacturas(List<Factura> factura)
        {
            FacturaResponse _responseFactura = new FacturaResponse();
            ClaveAcceso metodoClaveAcceso = new ClaveAcceso();
            PruebasDocumentos doc = new PruebasDocumentos();
            ProcesoFactura procesoFactura = new ProcesoFactura();
            int codigoRetorno = 0;
            int ciFactura = 0;
            string descripcionRetorno = string.Empty;
            string tipoDocumento = string.Empty;
            string claveAcceso = string.Empty;
            string idCompania = string.Empty;
            byte[] utfBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(factura));
            factura = JsonConvert.DeserializeObject<List<Factura>>(Encoding.UTF8.GetString(utfBytes, 0, utfBytes.Length));

            try
            {

                //ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin($"Inicio de proceso ProcesarFacturas  CantidadDoc : {factura.Count}");
                if (factura.Count > 0)
                {
                    foreach (var resultadoFactura in factura)
                    {
                        codigoRetorno = 0;
                        try
                        {
                            #region 2: Verifica si Existe Documentos ya en ViaDoc
                            DataSet existeDocumento = new DataSet();
                            tipoDocumento = ConfigurationManager.AppSettings["tipoDocumento.Factura"];

                            //preguntar si viene desde el WS
                            existeDocumento = procesoFactura.verificaExisteDocumento(tipoDocumento,
                                                                                 resultadoFactura.compania,
                                                                                 resultadoFactura.establecimiento,
                                                                                 resultadoFactura.puntoEmision,
                                                                                 resultadoFactura.secuencial,
                                                                                 ref codigoRetorno,
                                                                                 ref descripcionRetorno);

                            _responseFactura.codigoRetorno = codigoRetorno;
                            _responseFactura.mensajeRetorno = descripcionRetorno;
                            #endregion 2: Verifica si Existe Documentos ya en ViaDoc

                            idCompania = resultadoFactura.compania.ToString();
                            if (_responseFactura.codigoRetorno.Equals(0))
                            {
                                if (Convert.ToInt32(existeDocumento.Tables[0].Rows[0]["codigoRetorno"]).Equals(0) && !existeDocumento.Tables[0].Rows[0]["claveAcceso"].ToString().Equals(""))
                                {
                                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("El Documento SI Existe en viadoc | CodigoRetorno: " + existeDocumento.Tables[0].Rows[0]["codigoRetorno"]);
                                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("idCompañia: " + resultadoFactura.compania + " | " + "#Doc: " + resultadoFactura.establecimiento + "-" + resultadoFactura.puntoEmision + "-" + resultadoFactura.secuencial);
                                    _responseFactura.mensajeRetorno = descripcionRetorno;

                                    //Si el documento existe lo pone en la lista de documento
                                    DocumentosProcesados _documento = new DocumentosProcesados()
                                    {
                                        ciDocumento = 0,
                                        claveAcceso = existeDocumento.Tables[0].Rows[0]["claveAcceso"].ToString(),
                                        compania = resultadoFactura.compania,
                                        puntoEmision = resultadoFactura.puntoEmision,
                                        establecimiento = resultadoFactura.establecimiento,
                                        secuencial = resultadoFactura.secuencial,
                                        tipoDocumento = "A",
                                        codigoNumerico = resultadoFactura.codigoNumerico,
                                        codigoRetorno = codigoRetorno,
                                        descripcionRetorno = descripcionRetorno,
                                        tablaMurano = ""
                                    };
                                    _responseFactura.documentoProcesado.Add(_documento);
                                }
                                else
                                {
                                    //ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("El Documento NO Existe en viadoc");
                                    #region 3: Generar Clave de Acceso
                                    if (resultadoFactura.claveAcceso == null || resultadoFactura.claveAcceso.Equals(""))
                                    {
                                        resultadoFactura.claveAcceso = metodoClaveAcceso.GenerarClaveAccesoDocumento(tipoDocumento,
                                                                                                resultadoFactura.secuencial,
                                                                                                resultadoFactura.puntoEmision,
                                                                                                resultadoFactura.establecimiento,
                                                                                                resultadoFactura.fechaEmision,
                                                                                                resultadoFactura.ruc,
                                                                                                resultadoFactura.ambiente);
                                        claveAcceso = resultadoFactura.claveAcceso;
                                    }
                                    else
                                    {
                                        claveAcceso = resultadoFactura.claveAcceso;
                                    }
                                    #endregion 3: Generar Clave de Acceso

                                    if (codigoRetorno.Equals(0))
                                    {
                                        #region 5: Insertar Cabecera Facturas
                                        procesoFactura.InsertarFactura(resultadoFactura.compania, resultadoFactura.tipoEmision, resultadoFactura.claveAcceso, tipoDocumento,
                                                                       resultadoFactura.establecimiento, resultadoFactura.puntoEmision, resultadoFactura.secuencial,
                                                                       resultadoFactura.fechaEmision, resultadoFactura.tipoIdentificacionComprador, resultadoFactura.guiaRemision,
                                                                       resultadoFactura.razonSocialComprador, resultadoFactura.identificacionComprador, resultadoFactura.totalSinImpuestos,
                                                                       resultadoFactura.totalDescuento, resultadoFactura.propina, resultadoFactura.importeTotal, resultadoFactura.moneda,
                                                                       resultadoFactura.contingenciaDet, resultadoFactura.email, resultadoFactura.claveAcceso, "", "", "",
                                                                       "", resultadoFactura.ambiente, ref codigoRetorno, ref descripcionRetorno, ref ciFactura);

                                        #endregion 5: Insertar Cabecera Facturas

                                        #region 6: Insertar Detalle Facturas
                                        if (codigoRetorno.Equals(0))
                                        {
                                            if (resultadoFactura.detalleFactura.Count > 0)
                                            {
                                                resultadoFactura.idFactura = ciFactura;
                                                foreach (FacturaDetalle detalleFactura in resultadoFactura.detalleFactura)
                                                {
                                                    procesoFactura.InsertarFacturaDetalle(resultadoFactura.compania, resultadoFactura.establecimiento, resultadoFactura.puntoEmision,
                                                                                          resultadoFactura.secuencial, detalleFactura.codigoPrincipal, detalleFactura.codigoAuxiliar,
                                                                                          detalleFactura.descripcion.Replace(Environment.NewLine, ""), detalleFactura.cantidad, detalleFactura.precioUnitario,
                                                                                          detalleFactura.descuento, detalleFactura.precioTotalSinImpuesto, ref codigoRetorno,
                                                                                          ref descripcionRetorno);

                                                    if (codigoRetorno.Equals(0))
                                                    {
                                                        foreach (FacturaDetalleAdicional detalleAdicional in detalleFactura.detalleAdicional)
                                                        {
                                                            procesoFactura.InsertarFacturaDetalleAdicional(resultadoFactura.compania, resultadoFactura.establecimiento, resultadoFactura.puntoEmision,
                                                                                          resultadoFactura.secuencial, detalleAdicional.codigoPrincipal, detalleAdicional.nombre,
                                                                                          detalleAdicional.valor, ref codigoRetorno, ref descripcionRetorno);

                                                        }
                                                    }

                                                    if (codigoRetorno.Equals(0))
                                                    {
                                                        if (detalleFactura.detalleImpuesto.Count > 0)
                                                        {
                                                            foreach (FacturaDetalleImpuesto detalleImpuesto in detalleFactura.detalleImpuesto)
                                                            {
                                                                string[] cadenaTarifa = null;
                                                                string tarifa = string.Empty;
                                                                if (detalleImpuesto.tarifa.ToString().Contains("."))
                                                                {
                                                                    cadenaTarifa = detalleImpuesto.tarifa.ToString().Split('.');
                                                                    tarifa = cadenaTarifa[0].ToString().Trim();
                                                                }
                                                                else
                                                                {
                                                                    tarifa = detalleImpuesto.tarifa.ToString();
                                                                }

                                                                procesoFactura.InsertarFacturaDetalleImpuesto(resultadoFactura.compania, resultadoFactura.establecimiento, resultadoFactura.puntoEmision,
                                                                                              resultadoFactura.secuencial, detalleImpuesto.codigoPrincipal, detalleImpuesto.codigo,
                                                                                              detalleImpuesto.codigoPorcentaje, tarifa.ToString(), detalleImpuesto.baseImponible,
                                                                                              detalleImpuesto.valor, ref codigoRetorno, ref descripcionRetorno);

                                                            }
                                                        }
                                                        else
                                                        {
                                                            codigoRetorno = 1;
                                                            descripcionRetorno = "Factura No posee Detalle Impuesto..";
                                                        }
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                codigoRetorno = 1;
                                                descripcionRetorno = "Factura sin detalles";
                                            }
                                        }

                                        if (codigoRetorno.Equals(0))
                                        {
                                            if (resultadoFactura.totalImpuesto.Count > 0)
                                            {
                                                foreach (FacturaTotalImpuesto totalImpuesto in resultadoFactura.totalImpuesto)
                                                {
                                                    procesoFactura.InsertarTotalImpuesto(resultadoFactura.compania, resultadoFactura.establecimiento, resultadoFactura.puntoEmision,
                                                                                          resultadoFactura.secuencial, totalImpuesto.codigo, totalImpuesto.codigoPorcentaje,
                                                                                          totalImpuesto.tarifa, totalImpuesto.descuentoAdicional, totalImpuesto.baseImponible,
                                                                                          totalImpuesto.valor, ref codigoRetorno, ref descripcionRetorno);
                                                }
                                            }
                                            else
                                            {
                                                codigoRetorno = 1;
                                                descripcionRetorno = "Factura No posee Total Impuesto..";
                                            }
                                        }
                                        #endregion 6: Insertar Detalle Facturas

                                        #region 7: Insertar Facturas Forma de Pago
                                        if (codigoRetorno.Equals(0))
                                        {
                                            if (resultadoFactura.formaPago.Count > 0)
                                            {
                                                foreach (FacturaDetalleFormaPago facturapago in resultadoFactura.formaPago)
                                                {
                                                    //Utilitarios.logs.LogsFactura.LogsInicioFin(resultadoFactura.secuencial + " - " + facturapago.formaPago + " - " + facturapago.plazo + " - " + facturapago.total + " - " + facturapago.unidadTiempo);
                                                    procesoFactura.InsertarFacturaDetalleFormaPago(resultadoFactura.compania, resultadoFactura.establecimiento, resultadoFactura.puntoEmision,
                                                                                          resultadoFactura.secuencial, facturapago.formaPago, facturapago.plazo, facturapago.unidadTiempo,
                                                                                          facturapago.total, ref codigoRetorno, ref descripcionRetorno);
                                                }
                                            }
                                            else
                                            {
                                                codigoRetorno = 1;
                                                descripcionRetorno = "No posee Forma de Pago..";
                                            }
                                        }
                                        #endregion 7: Insertar Facturas Forma de Pago

                                        #region Insertar Facturas Informacion Adicional
                                        if (codigoRetorno.Equals(0))
                                        {
                                            if (resultadoFactura.infoAdicional.Count > 0)
                                            {
                                                foreach (FacturaInfoAdicional infoAdicional in resultadoFactura.infoAdicional)
                                                {
                                                    procesoFactura.InsertarFacturaInfoAdicional(resultadoFactura.compania, resultadoFactura.establecimiento, resultadoFactura.puntoEmision,
                                                                                      resultadoFactura.secuencial, infoAdicional.nombre, infoAdicional.valor.Replace(Environment.NewLine, ""), ref codigoRetorno,
                                                                                      ref descripcionRetorno);

                                                }
                                            }
                                            else
                                            {
                                                codigoRetorno = 1;
                                                descripcionRetorno = "No posee Informacion Adicional..";
                                            }
                                        }
                                        #endregion Insertar Facturas Informacion Adicional

                                        #region Insertar Detalle de Reembolso 
                                        if (codigoRetorno.Equals(0))
                                        {
                                            if (resultadoFactura.detalleReembolsos.Count > 0)
                                            {
                                                foreach (FacturaDetalleReembolso facturaReembolso in resultadoFactura.detalleReembolsos)
                                                {
                                                    //Utilitarios.logs.LogsFactura.LogsInicioFin(resultadoFactura.secuencial + " - " + facturaReembolso.numFacturaProveedor);
                                                    procesoFactura.InsertarFacturaDetalleReembolso(resultadoFactura.compania, facturaReembolso.tipo, resultadoFactura.establecimiento, resultadoFactura.puntoEmision,
                                                                                          resultadoFactura.secuencial, facturaReembolso.numFacturaProveedor, facturaReembolso.fechaEmision, facturaReembolso.numIdentificacionProveedor, resultadoFactura.claveAcceso,
                                                                                          facturaReembolso.subtotal, facturaReembolso.subTotIvaCero, facturaReembolso.subTotIva, facturaReembolso.subTotExcentoIva, facturaReembolso.valorIVA, facturaReembolso.valorICE,
                                                                                          facturaReembolso.valorINBPNR, facturaReembolso.total, facturaReembolso.detalle, facturaReembolso.subtotal, facturaReembolso.codigoImp, facturaReembolso.codigoPorcentajeImp,
                                                                                          facturaReembolso.tarifaImp, facturaReembolso.txTipoIdProveedor, facturaReembolso.codPaisPagoProveedor, facturaReembolso.tipoProveedor, ref codigoRetorno, ref descripcionRetorno);
                                                }
                                            }
                                            else
                                            {
                                                codigoRetorno = 1;
                                                descripcionRetorno = "No posee Reembolsos..";
                                            }
                                        }
                                        #endregion Insertar Detalle de Reembolso

                                        #region Actualizar Estado de Factura 
                                        if (codigoRetorno.Equals(0))
                                        {
                                            XmlGenerados item = new XmlGenerados()
                                            {
                                                CiCompania = resultadoFactura.compania,
                                                CiTipoDocumento = tipoDocumento,
                                                ClaveAcceso = resultadoFactura.claveAcceso,
                                                Identity = resultadoFactura.idFactura,
                                                XmlEstado = "A",
                                                CiContingenciaDet = 4

                                            };
                                            documentos.ActualizarEstadosComprobantes(item, ref codigoRetorno, ref descripcionRetorno);
                                        }
                                        #endregion Actualizar Estado de Factura
                                    }
                                    _responseFactura.mensajeRetorno = descripcionRetorno;

                                    DocumentosProcesados _documento = new DocumentosProcesados()
                                    {
                                        ciDocumento = 0,
                                        claveAcceso = resultadoFactura.claveAcceso,
                                        compania = resultadoFactura.compania,
                                        puntoEmision = resultadoFactura.puntoEmision,
                                        establecimiento = resultadoFactura.establecimiento,
                                        secuencial = resultadoFactura.secuencial,
                                        tipoDocumento = "A",
                                        codigoNumerico = resultadoFactura.codigoNumerico,
                                        codigoRetorno = codigoRetorno,
                                        descripcionRetorno = descripcionRetorno,
                                        tablaMurano = ""
                                    };
                                    _responseFactura.documentoProcesado.Add(_documento);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            codigoRetorno = 9999;
                            descripcionRetorno = "Detalle Error:" + ex.Message;
                            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(descripcionRetorno);
                        }

                    }
                }
                else
                {
                    _responseFactura.codigoRetorno = 1;
                    _responseFactura.mensajeRetorno = "Vacio";
                }
            }
            catch (Exception ex)
            {
                _responseFactura.codigoRetorno = 9999;
                _responseFactura.mensajeRetorno = "Exception: " + ex.Message;
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(descripcionRetorno);
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
            }

            return _responseFactura;
        }

        public CompRetencionResponse ProcesarCompRetencion(List<CompRetencion> compRetencion)
        {
            CompRetencionResponse _responseCompRetencion = new CompRetencionResponse();
            ProcesoCompRetencion procesoCompRetencion = new ProcesoCompRetencion();
            ClaveAcceso metodoClaveAcceso = new ClaveAcceso();
            string tipoDocumento = string.Empty;
            PruebasDocumentos doc = new PruebasDocumentos();
            int codigoRetorno = 0;
            int ciCompRetencion = 0;
            string descripcionRetorno = string.Empty;
            string idCompania = string.Empty;
            string claveAcceso = string.Empty;
            int insertaCabecera = 0, insertaDetalle = 0;
            try
            {
                if (compRetencion.Count > 0)
                {
                    foreach (var resultadoComp in compRetencion)
                    {
                        codigoRetorno = 0;
                        insertaCabecera = 0; insertaDetalle = 0;
                        try
                        {
                            DataSet existeDocumento = new DataSet();

                            tipoDocumento = ConfigurationManager.AppSettings["tipoDocumento.CompRetencion"];

                            existeDocumento = procesoCompRetencion.verificaExisteDocumento(tipoDocumento,
                                                                                           resultadoComp.compania,
                                                                                           resultadoComp.establecimiento,
                                                                                           resultadoComp.puntoEmision,
                                                                                           resultadoComp.secuencial,
                                                                                           ref codigoRetorno,
                                                                                           ref descripcionRetorno);

                            _responseCompRetencion.codigoRetorno = codigoRetorno;
                            _responseCompRetencion.mensajeRetorno = descripcionRetorno;

                            if (_responseCompRetencion.codigoRetorno.Equals(0))
                            {

                                if (Convert.ToInt32(existeDocumento.Tables[0].Rows[0]["codigoRetorno"]).Equals(0) && !existeDocumento.Tables[0].Rows[0]["claveAcceso"].ToString().Equals(""))
                                {
                                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("El Documento SI Existe en viadoc | CodigoRetorno: " + existeDocumento.Tables[0].Rows[0]["codigoRetorno"]);
                                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("idCompañia: " + resultadoComp.compania + "| Tipo de documento: 07 | " + "#Doc: " + resultadoComp.establecimiento + "-" + resultadoComp.puntoEmision + "-" + resultadoComp.secuencial);
                                    _responseCompRetencion.mensajeRetorno = descripcionRetorno;

                                    DocumentosProcesados _documento = new DocumentosProcesados()
                                    {
                                        ciDocumento = 0,
                                        claveAcceso = existeDocumento.Tables[0].Rows[0]["claveAcceso"].ToString(),
                                        compania = resultadoComp.compania,
                                        puntoEmision = resultadoComp.puntoEmision,
                                        establecimiento = resultadoComp.establecimiento,
                                        secuencial = resultadoComp.secuencial,
                                        tipoDocumento = "A",
                                        codigoNumerico = resultadoComp.codigoNumerico,
                                        codigoRetorno = codigoRetorno,
                                        descripcionRetorno = descripcionRetorno,
                                        tablaMurano = ""
                                    };
                                    _responseCompRetencion.documentoProcesado.Add(_documento);
                                }
                                else
                                {
                                    if (existeDocumento.Tables.Count > 0)
                                    {
                                        ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("El Documento NO Existe en viadoc");
                                        idCompania = resultadoComp.compania.ToString();
                                        string dsClaveAcceso = existeDocumento.Tables[0].Rows[0]["claveAcceso"].ToString();

                                        if (dsClaveAcceso.Equals(""))
                                        {
                                            if (resultadoComp.claveAcceso == null || resultadoComp.claveAcceso.Equals(""))
                                            {
                                                try
                                                {
                                                    resultadoComp.claveAcceso = metodoClaveAcceso.GenerarClaveAccesoDocumento(tipoDocumento,
                                                                                                                            resultadoComp.secuencial,
                                                                                                                            resultadoComp.puntoEmision,
                                                                                                                            resultadoComp.establecimiento,
                                                                                                                            resultadoComp.fechaEmision,
                                                                                                                            resultadoComp.ruc,
                                                                                                                            resultadoComp.ambiente);
                                                    claveAcceso = resultadoComp.claveAcceso;
                                                }
                                                catch (Exception ex)
                                                {
                                                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Error WS: " + ex.ToString());
                                                }
                                            }
                                            else
                                            {
                                                claveAcceso = resultadoComp.claveAcceso;
                                            }
                                        }
                                        else
                                        {
                                            resultadoComp.claveAcceso = dsClaveAcceso;
                                        }
                                    }

                                    if (codigoRetorno.Equals(0))
                                    {

                                        ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Clave: " + resultadoComp.claveAcceso);
                                        procesoCompRetencion.InsertarCompRetencion(resultadoComp.compania, resultadoComp.tipoEmision, resultadoComp.claveAcceso, tipoDocumento,
                                                                                   resultadoComp.establecimiento, resultadoComp.puntoEmision, resultadoComp.secuencial,
                                                                                   resultadoComp.fechaEmision, resultadoComp.tipoIdentificacionSujetoRetenido,
                                                                                   resultadoComp.razonSocialSujetoRetenido, resultadoComp.identificacionSujetoRetenido,
                                                                                   resultadoComp.periodoFiscal, resultadoComp.contingenciaDet, resultadoComp.email,
                                                                                   resultadoComp.claveAcceso, "", "", "", resultadoComp.ambiente, resultadoComp.codigoNumerico,
                                                                                   ref codigoRetorno, ref descripcionRetorno, ref ciCompRetencion);
                                        insertaCabecera = codigoRetorno;
                                        var documento = resultadoComp.establecimiento.ToString() + '-' + resultadoComp.puntoEmision.ToString() + '-' + resultadoComp.secuencial.ToString() + " InsertarCompRetencion " + " CodigoRetorno:" + codigoRetorno;
                                        ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(documento);
                                    }

                                    if (codigoRetorno.Equals(0))
                                    {
                                        resultadoComp.idCompRetencion = ciCompRetencion;

                                        if (resultadoComp.detalleRetencion.Count > 0)
                                        {
                                            foreach (var resultadoDetalle in resultadoComp.detalleRetencion)
                                            {
                                                procesoCompRetencion.InsertarCompRetencionDetalle(resultadoComp.compania, resultadoComp.establecimiento, resultadoComp.puntoEmision,
                                                                                                  resultadoComp.secuencial, resultadoDetalle.impuesto, resultadoDetalle.codRetencion.Replace(".00", ""),
                                                                                                  resultadoDetalle.baseImponible, resultadoDetalle.porcentajeRetener,
                                                                                                  resultadoDetalle.valorRetenido, resultadoDetalle.codDocSustento,
                                                                                                  resultadoDetalle.numDocSustento, resultadoDetalle.fechaEmisionDocSustento,
                                                                                                  ref codigoRetorno, ref descripcionRetorno);


                                            }
                                        }
                                        else
                                        {
                                            codigoRetorno = 1;
                                            descripcionRetorno = "CompRetencion no posee detalles";
                                        }

                                        insertaDetalle = codigoRetorno;
                                        var documento = resultadoComp.establecimiento.ToString() + '-' + resultadoComp.puntoEmision.ToString() + '-' + resultadoComp.secuencial.ToString() + " InsertarCompRetencionDetalle " + " CodigoRetorno:" + codigoRetorno;
                                        ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(documento);
                                    }
                                    if (codigoRetorno.Equals(0))
                                    {
                                        //ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("resultado comp: " + JsonConvert.SerializeObject(resultadoComp));
                                        // INSERTAR EN DOC SUSTENTO
                                        var docSustentoExiste = resultadoComp.docSustento != null ? true : false;


                                        if (docSustentoExiste && resultadoComp.docSustento.Count > 0)
                                        {
                                            foreach (var docsustento in resultadoComp.docSustento)
                                            {
                                                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("items del array doc sustento : " + docsustento);
                                                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Clave: " + resultadoComp.claveAcceso);
                                                procesoCompRetencion.InsertarComprobanteRetencionDocSustento(resultadoComp.compania, resultadoComp.establecimiento, resultadoComp.puntoEmision, resultadoComp.secuencial,
                                                                                           docsustento.fechaEmision, docsustento.codSustento,
                                                                                           docsustento.codDocSustento, docsustento.fechaRegistroContable,
                                                                                           docsustento.codImpuestoDocSustento, docsustento.codigoPorcentaje,
                                                                                           docsustento.totalSinImpuesto, docsustento.importeTotal, docsustento.baseImponible,
                                                                                           docsustento.tarifa, docsustento.valorImpuesto,
                                                                                           ref codigoRetorno, ref descripcionRetorno);

                                            }
                                        }
                                        else
                                        {
                                            codigoRetorno = 1;
                                            descripcionRetorno = "CompRetencion no posee Documentos Sustento";
                                        }
                                        var documento = resultadoComp.establecimiento.ToString() + '-' + resultadoComp.puntoEmision.ToString() + '-' + resultadoComp.secuencial.ToString() + " InsertarComprobanteRetencionDocSustento " + " CodigoRetorno:" + codigoRetorno;
                                        ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(documento);
                                    }
                                    if (codigoRetorno.Equals(0))
                                    {
                                        // INSERTAR EN FORMA DE PAGO
                                        if (resultadoComp.formasPago.Count > 0)
                                        {
                                            foreach (var formaPago in resultadoComp.formasPago)
                                            {
                                                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Clave: " + resultadoComp.claveAcceso);
                                                procesoCompRetencion.InsertarComprobanteRetencionFormaPago(resultadoComp.compania, resultadoComp.establecimiento, resultadoComp.puntoEmision, resultadoComp.secuencial,
                                                                                           formaPago.formaPago, formaPago.qnTotal,
                                                                                           ref codigoRetorno, ref descripcionRetorno);

                                            }
                                        }
                                        else
                                        {
                                            codigoRetorno = 1;
                                            descripcionRetorno = "CompRetencion no posee Documentos Sustento";
                                        }

                                        var documento = resultadoComp.establecimiento.ToString() + '-' + resultadoComp.puntoEmision.ToString() + '-' + resultadoComp.secuencial.ToString() + " InsertarComprobanteRetencionFormaPago " + " CodigoRetorno:" + codigoRetorno;
                                        ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(documento);

                                    }

                                    if (insertaCabecera == 0 && insertaDetalle == 0)
                                    {
                                        if (resultadoComp.infoAdicional.Count > 0)
                                        {
                                            foreach (var resultadoAdicinal in resultadoComp.infoAdicional)
                                            {
                                                procesoCompRetencion.InsertarComprobanteRetencionInfoAdicional(resultadoComp.compania, resultadoComp.establecimiento,
                                                                                                               resultadoComp.puntoEmision, resultadoComp.secuencial,
                                                                                                               resultadoAdicinal.nombre, resultadoAdicinal.valor,
                                                                                                               ref codigoRetorno, ref descripcionRetorno);

                                                var documento = resultadoComp.establecimiento.ToString() + '-' + resultadoComp.puntoEmision.ToString() + '-' + resultadoComp.secuencial.ToString() + " InsertarComprobanteRetencionInfoAdicional " + " Nombre:" + resultadoAdicinal.nombre + " Valor:" + resultadoAdicinal.valor;
                                                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(documento);
                                            }
                                        }
                                        else
                                        {
                                            codigoRetorno = 1;
                                            descripcionRetorno = "CompRetencion no posee Informacion Adicional";
                                        }
                                    }



                                    #region Actualizar Estado de Factura 
                                    // if (codigoRetorno.Equals(0))
                                    if (insertaCabecera == 0 && insertaDetalle == 0)
                                    {
                                        XmlGenerados item = new XmlGenerados()
                                        {
                                            CiCompania = resultadoComp.compania,
                                            CiTipoDocumento = tipoDocumento,
                                            ClaveAcceso = resultadoComp.claveAcceso,
                                            Identity = resultadoComp.idCompRetencion,
                                            XmlEstado = "A",
                                            CiContingenciaDet = 4
                                        };
                                        documentos.ActualizarEstadosComprobantes(item, ref codigoRetorno, ref descripcionRetorno);
                                    }
                                    #endregion Actualizar Estado de Factura

                                    _responseCompRetencion.mensajeRetorno = descripcionRetorno;

                                    DocumentosProcesados _documento = new DocumentosProcesados()
                                    {
                                        ciDocumento = 0,
                                        claveAcceso = resultadoComp.claveAcceso,
                                        compania = resultadoComp.compania,
                                        puntoEmision = resultadoComp.puntoEmision,
                                        establecimiento = resultadoComp.establecimiento,
                                        secuencial = resultadoComp.secuencial,
                                        tipoDocumento = "A",
                                        codigoNumerico = resultadoComp.codigoNumerico,
                                        codigoRetorno = codigoRetorno,
                                        descripcionRetorno = descripcionRetorno,
                                        tablaMurano = ""
                                    };
                                    _responseCompRetencion.documentoProcesado.Add(_documento);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            codigoRetorno = 1;
                            descripcionRetorno = "Detalle Error:" + ex.Message;
                            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                        }
                    }
                }
                else
                {
                    _responseCompRetencion.codigoRetorno = 1;
                    _responseCompRetencion.mensajeRetorno = "Vacio";
                }
            }
            catch (Exception ex)
            {
                _responseCompRetencion.codigoRetorno = 9999;
                _responseCompRetencion.mensajeRetorno = ex.Message;
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
            }
            return _responseCompRetencion;
        }

        public NotaCreditoResponse ProcesarNotaCredito(List<NotaCredito> notaCredito)
        {
            NotaCreditoResponse _responseNotaCredito = new NotaCreditoResponse();
            ClaveAcceso metodoClaveAcceso = new ClaveAcceso();
            ProcesoNotaCredito procesoNotaCredito = new ProcesoNotaCredito();
            string tipoDocumento = string.Empty;
            int codigoRetorno = 0;
            int ciNotaCredito = 0;
            string descripcionRetorno = string.Empty;
            string idCompania = string.Empty;
            string claveAcceso = string.Empty;

            if (notaCredito.Count > 0)
            {
                foreach (var resultadaCredito in notaCredito)
                {
                    codigoRetorno = 0;
                    try
                    {
                        DataSet existeDocumento = new DataSet();

                        tipoDocumento = ConfigurationManager.AppSettings["tipoDocumento.NotaCredito"];

                        existeDocumento = procesoNotaCredito.verificaExisteDocumento(tipoDocumento,
                                                                                      resultadaCredito.compania,
                                                                                      resultadaCredito.establecimiento,
                                                                                      resultadaCredito.puntoEmision,
                                                                                      resultadaCredito.secuencial,
                                                                                      ref codigoRetorno,
                                                                                      ref descripcionRetorno);
                        _responseNotaCredito.codigoRetorno = codigoRetorno;
                        _responseNotaCredito.mensajeRetorno = descripcionRetorno;

                        if (_responseNotaCredito.codigoRetorno.Equals(0))
                        {
                            if (Convert.ToInt32(existeDocumento.Tables[0].Rows[0]["codigoRetorno"]).Equals(0) && !existeDocumento.Tables[0].Rows[0]["claveAcceso"].ToString().Equals(""))
                            {
                                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("El Documento SI Existe en viadoc | CodigoRetorno: " + existeDocumento.Tables[0].Rows[0]["codigoRetorno"]);
                                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("idCompañia: " + resultadaCredito.compania + " | " + "#Doc: " + resultadaCredito.establecimiento + "-" + resultadaCredito.puntoEmision + "-" + resultadaCredito.secuencial);
                                _responseNotaCredito.mensajeRetorno = descripcionRetorno;

                                DocumentosProcesados _documento = new DocumentosProcesados()
                                {
                                    ciDocumento = 0,
                                    claveAcceso = existeDocumento.Tables[0].Rows[0]["claveAcceso"].ToString(),
                                    compania = resultadaCredito.compania,
                                    puntoEmision = resultadaCredito.puntoEmision,
                                    establecimiento = resultadaCredito.establecimiento,
                                    secuencial = resultadaCredito.secuencial,
                                    tipoDocumento = "A",
                                    codigoNumerico = resultadaCredito.codigoNumerico,
                                    codigoRetorno = codigoRetorno,
                                    descripcionRetorno = descripcionRetorno,
                                    tablaMurano = ""
                                };
                                _responseNotaCredito.documentoProcesado.Add(_documento);
                            }
                            else
                            {
                                if (existeDocumento.Tables.Count > 0)
                                {
                                    string dsClaveAcceso = existeDocumento.Tables[0].Rows[0]["claveAcceso"].ToString();

                                    if (dsClaveAcceso.Equals(""))
                                    {
                                        #region 3: Generar Clave de Acceso
                                        if (resultadaCredito.claveAcceso == null || resultadaCredito.claveAcceso.Equals(""))
                                        {
                                            resultadaCredito.claveAcceso = metodoClaveAcceso.GenerarClaveAccesoDocumento(tipoDocumento,
                                                                                                                      resultadaCredito.secuencial,
                                                                                                                      resultadaCredito.puntoEmision,
                                                                                                                      resultadaCredito.establecimiento,
                                                                                                                      resultadaCredito.fechaEmision,
                                                                                                                      resultadaCredito.ruc,
                                                                                                                      resultadaCredito.ambiente);

                                            claveAcceso = resultadaCredito.claveAcceso;

                                        }

                                        idCompania = resultadaCredito.compania.ToString();
                                        #endregion 3: Generar Clave de Acceso
                                    }
                                    else
                                    {
                                        resultadaCredito.claveAcceso = dsClaveAcceso;
                                    }

                                }


                                #region Insertar Cabecera Nota Credito
                                if (codigoRetorno.Equals(0))
                                {
                                    procesoNotaCredito.InsertarNotaCredito(resultadaCredito.compania, resultadaCredito.tipoEmision, resultadaCredito.claveAcceso, tipoDocumento,
                                                                           resultadaCredito.establecimiento, resultadaCredito.puntoEmision, resultadaCredito.secuencial,
                                                                           resultadaCredito.fechaEmision, resultadaCredito.tipoIdentificacionComprador,
                                                                           resultadaCredito.razonSocialComprador, resultadaCredito.identificacionComprador, resultadaCredito.rise,
                                                                           resultadaCredito.tipoDocumentoModificado, resultadaCredito.numeroDocumentoModificado,
                                                                           resultadaCredito.fechaEmisionDocumentoModificado, resultadaCredito.totalSinImpuestos,
                                                                           resultadaCredito.valorModificacion, resultadaCredito.moneda, resultadaCredito.motivo.Replace(Environment.NewLine, ""),
                                                                           resultadaCredito.contingenciaDet, resultadaCredito.email, resultadaCredito.claveAcceso, "", "", "",
                                                                           resultadaCredito.ambiente, resultadaCredito.codigoNumerico,
                                                                           resultadaCredito.ruc, ref codigoRetorno, ref descripcionRetorno, ref ciNotaCredito);
                                }
                                #endregion Insertar Cabecera Nota Credito

                                #region Insertar Cabecera Nota Credito detalle
                                if (codigoRetorno.Equals(0))
                                {
                                    resultadaCredito.idNotaCredito = ciNotaCredito;

                                    if (resultadaCredito.detalle.Count > 0)
                                    {
                                        foreach (var detalle in resultadaCredito.detalle)
                                        {
                                            procesoNotaCredito.InsertarNotaCreditoDEtalle(resultadaCredito.compania, resultadaCredito.establecimiento, resultadaCredito.puntoEmision,
                                                                                          resultadaCredito.secuencial, detalle.codigoInterno, detalle.codigoAdicional,
                                                                                          detalle.descripcion.Replace(Environment.NewLine, "").Replace(Environment.NewLine, "").Replace(Environment.NewLine, ""), detalle.cantidad, detalle.precioUnitario, detalle.descuento,
                                                                                          detalle.precioTotalSinImpuesto, ref codigoRetorno, ref descripcionRetorno);

                                            if (codigoRetorno.Equals(0))
                                            {
                                                foreach (var detalleAdicional in detalle.detalleAdicional)
                                                {
                                                    procesoNotaCredito.InsertarNotaCreditoDetalleAdicional(resultadaCredito.compania, resultadaCredito.establecimiento, resultadaCredito.puntoEmision,
                                                                                                           resultadaCredito.secuencial, detalleAdicional.codigoInterno, detalleAdicional.nombre,
                                                                                                           detalleAdicional.valor, ref codigoRetorno, ref descripcionRetorno);
                                                }
                                            }

                                            if (codigoRetorno.Equals(0))
                                            {
                                                if (detalle.detalleImpuesto.Count > 0)
                                                {
                                                    foreach (var detalleImpuesto in detalle.detalleImpuesto)
                                                    {
                                                        procesoNotaCredito.InsertarNotaCreditoDetalleImpuesto(resultadaCredito.compania, resultadaCredito.establecimiento, resultadaCredito.puntoEmision,
                                                                                                              resultadaCredito.secuencial, detalleImpuesto.codigoInterno, detalleImpuesto.codigo,
                                                                                                              detalleImpuesto.codigoPorcentaje, detalleImpuesto.tarifa, detalleImpuesto.baseImponible,
                                                                                                              detalleImpuesto.valor, ref codigoRetorno, ref descripcionRetorno);
                                                    }
                                                }
                                                else
                                                {
                                                    codigoRetorno = 1;
                                                    descripcionRetorno = "Nota de Credito sin detalles Impuesto";
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        codigoRetorno = 1;
                                        descripcionRetorno = "Nota de Credito sin detalles";
                                    }
                                }
                                #endregion Insertar Cabecera Nota Credito detalle

                                #region Insertar Cabecera Nota Credito total Impuesto 
                                if (codigoRetorno.Equals(0))
                                {
                                    if (resultadaCredito.totalImpuesto.Count > 0)
                                    {
                                        foreach (var totalImpuesto in resultadaCredito.totalImpuesto)
                                        {
                                            procesoNotaCredito.InsertarNotaCreditoTotalImpuesto(resultadaCredito.compania, resultadaCredito.establecimiento, resultadaCredito.puntoEmision,
                                                                                                resultadaCredito.secuencial, totalImpuesto.codigo, totalImpuesto.codigoPorcentaje,
                                                                                                totalImpuesto.baseImponible, totalImpuesto.valor, ref codigoRetorno, ref descripcionRetorno);
                                        }
                                    }
                                    else
                                    {
                                        codigoRetorno = 1;
                                        descripcionRetorno = "Nota de Credito sin detalles";
                                    }
                                }
                                #endregion Insertar Cabecera Nota Credito total Impuesto 

                                #region Insertar Cabecera Nota Credito Informacion Adicional
                                if (codigoRetorno.Equals(0))
                                {
                                    if (resultadaCredito.infoAdicional.Count > 0)
                                    {
                                        foreach (var infoAdicional in resultadaCredito.infoAdicional)
                                        {
                                            procesoNotaCredito.InsertarNotaCreditoInfoAdicional(resultadaCredito.compania, resultadaCredito.establecimiento,
                                                                                                resultadaCredito.puntoEmision, resultadaCredito.secuencial,
                                                                                                infoAdicional.nombre, infoAdicional.valor,
                                                                                                ref codigoRetorno, ref descripcionRetorno);
                                        }
                                    }
                                    else
                                    {
                                        codigoRetorno = 1;
                                        descripcionRetorno = "Nota de Credito sin detalles";
                                    }
                                }
                                #endregion Insertar Cabecera Nota Credito Informacion Adicional

                                #region Actualizar Estado de Factura 
                                if (codigoRetorno.Equals(0))
                                {
                                    XmlGenerados item = new XmlGenerados()
                                    {
                                        CiCompania = resultadaCredito.compania,
                                        CiTipoDocumento = tipoDocumento,
                                        ClaveAcceso = resultadaCredito.claveAcceso,
                                        Identity = resultadaCredito.idNotaCredito,
                                        XmlEstado = "A",
                                        CiContingenciaDet = 4
                                    };
                                    documentos.ActualizarEstadosComprobantes(item, ref codigoRetorno, ref descripcionRetorno);
                                }
                                #endregion Actualizar Estado de Factura

                                _responseNotaCredito.mensajeRetorno = descripcionRetorno;

                                DocumentosProcesados _documento = new DocumentosProcesados()
                                {
                                    ciDocumento = 0,
                                    claveAcceso = resultadaCredito.claveAcceso,
                                    compania = resultadaCredito.compania,
                                    puntoEmision = resultadaCredito.puntoEmision,
                                    establecimiento = resultadaCredito.establecimiento,
                                    secuencial = resultadaCredito.secuencial,
                                    tipoDocumento = tipoDocumento,
                                    codigoNumerico = resultadaCredito.codigoNumerico,
                                    codigoRetorno = codigoRetorno,
                                    descripcionRetorno = descripcionRetorno,
                                    tablaMurano = resultadaCredito.tablaMurano
                                };
                                _responseNotaCredito.documentoProcesado.Add(_documento);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        codigoRetorno = 1;
                        descripcionRetorno = "Detalle Error" + ex.Message;
                        ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                        //objLogs.creaLogMongo("Metodo Wcf", "9999", idCompania, claveAcceso, "Procesar Factura", descripcionRetorno, DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), "ViaDoc.ServicioWcf");
                    }

                }
            }
            return _responseNotaCredito;
        }

        public NotaDebitoResponse ProcesarNotaDebito(List<NotaDebito> notaDebito)
        {
            NotaDebitoResponse _responseNotaDebito = new NotaDebitoResponse();
            ClaveAcceso metodoClaveAcceso = new ClaveAcceso();
            ProcesoNotaDebito procesoNotaDebito = new ProcesoNotaDebito();
            string tipoDocumento = string.Empty;
            int codigoRetorno = 0;
            int ciNotaDebito = 0;
            string descripcionRetorno = string.Empty;
            string idCompania = string.Empty;
            string claveAcceso = string.Empty;

            try
            {
                if (notaDebito.Count > 0)
                {
                    foreach (var resultadaDebito in notaDebito)
                    {
                        codigoRetorno = 0;
                        try
                        {
                            codigoRetorno = 0;
                            DataSet existeDocumento = new DataSet();
                            tipoDocumento = ConfigurationManager.AppSettings["tipoDocumento.NotaDebito"];
                            existeDocumento = procesoNotaDebito.verificaExisteDocumento(tipoDocumento,
                                                                                          resultadaDebito.compania,
                                                                                          resultadaDebito.establecimiento,
                                                                                          resultadaDebito.puntoEmision,
                                                                                          resultadaDebito.secuencial,
                                                                                          ref codigoRetorno,
                                                                                          ref descripcionRetorno);
                            _responseNotaDebito.codigoRetorno = codigoRetorno;
                            _responseNotaDebito.mensajeRetorno = descripcionRetorno;

                            if (_responseNotaDebito.codigoRetorno.Equals(0))
                            {
                                if (Convert.ToInt32(existeDocumento.Tables[0].Rows[0]["codigoRetorno"]).Equals(0) && !existeDocumento.Tables[0].Rows[0]["claveAcceso"].ToString().Equals(""))
                                {
                                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("El Documento SI Existe en viadoc | CodigoRetorno: " + existeDocumento.Tables[0].Rows[0]["codigoRetorno"]);
                                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("idCompañia: " + resultadaDebito.compania + " | " + "#Doc: " + resultadaDebito.establecimiento + "-" + resultadaDebito.puntoEmision + "-" + resultadaDebito.secuencial);
                                    _responseNotaDebito.mensajeRetorno = descripcionRetorno;

                                    DocumentosProcesados _documento = new DocumentosProcesados()
                                    {
                                        ciDocumento = 0,
                                        claveAcceso = existeDocumento.Tables[0].Rows[0]["claveAcceso"].ToString(),
                                        compania = resultadaDebito.compania,
                                        puntoEmision = resultadaDebito.puntoEmision,
                                        establecimiento = resultadaDebito.establecimiento,
                                        secuencial = resultadaDebito.secuencial,
                                        tipoDocumento = "A",
                                        codigoNumerico = resultadaDebito.codigoNumerico,
                                        codigoRetorno = codigoRetorno,
                                        descripcionRetorno = descripcionRetorno,
                                        tablaMurano = ""
                                    };
                                    _responseNotaDebito.documentoProcesado.Add(_documento);
                                }
                                else
                                {
                                    if (existeDocumento.Tables.Count > 0)
                                    {
                                        string dsClaveAcceso = existeDocumento.Tables[0].Rows[0]["claveAcceso"].ToString();
                                        idCompania = resultadaDebito.compania.ToString();

                                        if (dsClaveAcceso.Equals(""))
                                        {
                                            #region 3: Generar Clave de Acceso
                                            if (resultadaDebito.claveAcceso == null || resultadaDebito.claveAcceso.Equals(""))
                                            {
                                                resultadaDebito.claveAcceso = metodoClaveAcceso.GenerarClaveAccesoDocumento(tipoDocumento,
                                                                                                                          resultadaDebito.secuencial,
                                                                                                                          resultadaDebito.puntoEmision,
                                                                                                                          resultadaDebito.establecimiento,
                                                                                                                          resultadaDebito.fechaEmision,
                                                                                                                          resultadaDebito.ruc,
                                                                                                                          resultadaDebito.ambiente);
                                                claveAcceso = resultadaDebito.claveAcceso;
                                            }
                                            #endregion 3: Generar Clave de Acceso
                                        }
                                        else
                                        {
                                            resultadaDebito.claveAcceso = dsClaveAcceso;
                                        }
                                    }
                                    #region Insertar Cabecera Nota Debito
                                    if (codigoRetorno.Equals(0))
                                    {
                                        procesoNotaDebito.InsertarNotaDebito(resultadaDebito.compania, resultadaDebito.tipoEmision, resultadaDebito.claveAcceso, tipoDocumento,
                                                                             resultadaDebito.establecimiento, resultadaDebito.puntoEmision, resultadaDebito.secuencial,
                                                                             resultadaDebito.fechaEmision, resultadaDebito.tipoIdentificacionComprador,
                                                                             resultadaDebito.razonSocialComprador, resultadaDebito.identificacionComprador, resultadaDebito.rise,
                                                                             resultadaDebito.tipoDocumentoModificado, resultadaDebito.numeroDocumentoModificado,
                                                                             resultadaDebito.fechaEmisionDocumentoModificado, resultadaDebito.totalSinImpuestos,
                                                                             resultadaDebito.valorTotal, resultadaDebito.contingenciaDet, resultadaDebito.email,
                                                                             resultadaDebito.claveAcceso, "", "", "", resultadaDebito.ambiente, resultadaDebito.codigoNumerico,
                                                                             resultadaDebito.ruc, ref codigoRetorno, ref descripcionRetorno, ref ciNotaDebito);
                                    }
                                    #endregion Insertar Cabecera Nota Debito

                                    #region Insertar Cabecera Nota Debito Impuesto
                                    if (codigoRetorno.Equals(0))
                                    {
                                        resultadaDebito.idNotaDebito = ciNotaDebito;
                                        if (resultadaDebito.impuesto.Count > 0)
                                        {
                                            foreach (var impuesto in resultadaDebito.impuesto)
                                            {
                                                procesoNotaDebito.InsertarNotaDebitoImpuesto(resultadaDebito.compania, resultadaDebito.establecimiento, resultadaDebito.puntoEmision,
                                                                                             resultadaDebito.secuencial, impuesto.codigo, impuesto.codigoPorcentaje, impuesto.tarifa,
                                                                                             impuesto.baseImponible, impuesto.valor, ref codigoRetorno, ref descripcionRetorno);
                                            }
                                        }
                                        else
                                        {
                                            codigoRetorno = 1;
                                            descripcionRetorno = "Nota Debito sin Impuesto";
                                        }
                                    }
                                    #endregion Insertar Cabecera Nota Debito Impuesto

                                    #region Insertar Cabecera Nota Debito Motivo 
                                    if (codigoRetorno.Equals(0))
                                    {
                                        if (resultadaDebito.motivo.Count > 0)
                                        {
                                            foreach (var motivo in resultadaDebito.motivo)
                                            {
                                                procesoNotaDebito.InsertarNotaDebitoMotivo(resultadaDebito.compania, resultadaDebito.establecimiento, resultadaDebito.puntoEmision,
                                                                                           resultadaDebito.secuencial, motivo.razon, motivo.valor, ref codigoRetorno,
                                                                                           ref descripcionRetorno);
                                            }
                                        }
                                        else
                                        {
                                            codigoRetorno = 1;
                                            descripcionRetorno = "Nota Debito sin Motivo";
                                        }
                                    }
                                    #endregion Insertar Cabecera Nota Debito Motivo

                                    #region Insertar Cabecera Nota Debito Informacion Adicional
                                    if (codigoRetorno.Equals(0))
                                    {
                                        if (resultadaDebito.infoAdicional.Count > 0)
                                        {
                                            foreach (var infoAdicional in resultadaDebito.infoAdicional)
                                            {
                                                procesoNotaDebito.InsertarNotaDebitoInfoAdicional(resultadaDebito.compania, resultadaDebito.establecimiento, resultadaDebito.puntoEmision,
                                                                                                  resultadaDebito.secuencial, infoAdicional.nombre, infoAdicional.valor,
                                                                                                  ref codigoRetorno, ref descripcionRetorno);
                                            }
                                        }
                                        else
                                        {
                                            codigoRetorno = 1;
                                            descripcionRetorno = "Nota Debito Información Adicional";
                                        }
                                    }
                                    #endregion Insertar Cabecera Nota Debito Informacion Adicional

                                    #region Actualizar Estado de Factura 
                                    if (codigoRetorno.Equals(0))
                                    {
                                        XmlGenerados item = new XmlGenerados()
                                        {
                                            CiCompania = resultadaDebito.compania,
                                            CiTipoDocumento = tipoDocumento,
                                            ClaveAcceso = resultadaDebito.claveAcceso,
                                            Identity = resultadaDebito.idNotaDebito,
                                            XmlEstado = "A",
                                            CiContingenciaDet = 4
                                        };
                                        documentos.ActualizarEstadosComprobantes(item, ref codigoRetorno, ref descripcionRetorno);
                                    }
                                    #endregion Actualizar Estado de Factura 

                                    _responseNotaDebito.mensajeRetorno = descripcionRetorno;

                                    DocumentosProcesados _documento = new DocumentosProcesados()
                                    {
                                        ciDocumento = 0,
                                        claveAcceso = resultadaDebito.claveAcceso,
                                        compania = resultadaDebito.compania,
                                        puntoEmision = resultadaDebito.puntoEmision,
                                        establecimiento = resultadaDebito.establecimiento,
                                        secuencial = resultadaDebito.secuencial,
                                        tipoDocumento = "A",
                                        codigoNumerico = resultadaDebito.codigoNumerico,
                                        codigoRetorno = codigoRetorno,
                                        descripcionRetorno = descripcionRetorno,
                                        tablaMurano = ""
                                    };
                                    _responseNotaDebito.documentoProcesado.Add(_documento);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                            codigoRetorno = 1;
                            descripcionRetorno = "Detalle Error" + ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _responseNotaDebito.codigoRetorno = 9999;
                _responseNotaDebito.mensajeRetorno = "Error Inesperado: " + ex.Message;
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                //objLogs.creaLogMongo("Metodo Wcf", "9999", idCompania, claveAcceso, "Procesar Nota Debito", _responseNotaDebito.mensajeRetorno, DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), "ViaDoc.ServicioWcf");
            }

            return _responseNotaDebito;
        }

        public GuiaRemisionResponse ProcesaGuiaRemision(List<GuiaRemision> guiaRemision)
        {
            GuiaRemisionResponse _responseGuiaRemision = new GuiaRemisionResponse();
            ProcesoGuiaRemision procesoGuiaRemision = new ProcesoGuiaRemision();
            ClaveAcceso metodoClaveAcceso = new ClaveAcceso();
            string tipoDocumento = string.Empty;
            int codigoRetorno = 0;
            int ciGuiaRemision = 0;
            string descripcionRetorno = string.Empty;
            string idCompania = string.Empty;
            string claveAcceso = string.Empty;

            try
            {
                if (guiaRemision.Count > 0)
                {
                    foreach (var resultadoGuia in guiaRemision)
                    {
                        codigoRetorno = 0;
                        try
                        {
                            DataSet existeDocumento = new DataSet();

                            tipoDocumento = ConfigurationManager.AppSettings["tipoDocumento.GuiRemision"];

                            existeDocumento = procesoGuiaRemision.verificaExisteDocumento(tipoDocumento,
                                                                                          resultadoGuia.compania,
                                                                                          resultadoGuia.establecimiento,
                                                                                          resultadoGuia.puntoEmision,
                                                                                          resultadoGuia.secuencial,
                                                                                          ref codigoRetorno,
                                                                                          ref descripcionRetorno);

                            _responseGuiaRemision.codigoRetorno = codigoRetorno;
                            _responseGuiaRemision.mensajeRetorno = descripcionRetorno;
                            Utilitarios.logs.LogsFactura.LogsInicioFin("1");
                            if (_responseGuiaRemision.codigoRetorno.Equals(0))
                            {

                                if (Convert.ToInt32(existeDocumento.Tables[0].Rows[0]["codigoRetorno"]).Equals(0) && !existeDocumento.Tables[0].Rows[0]["claveAcceso"].ToString().Equals(""))
                                {
                                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("El Documento SI Existe en viadoc | CodigoRetorno: " + existeDocumento.Tables[0].Rows[0]["codigoRetorno"]);
                                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("idCompañia: " + resultadoGuia.compania + " | " + "#Doc: " + resultadoGuia.establecimiento + "-" + resultadoGuia.puntoEmision + "-" + resultadoGuia.secuencial);
                                    _responseGuiaRemision.mensajeRetorno = descripcionRetorno;

                                    DocumentosProcesados _documento = new DocumentosProcesados()
                                    {
                                        ciDocumento = 0,
                                        claveAcceso = existeDocumento.Tables[0].Rows[0]["claveAcceso"].ToString(),
                                        compania = resultadoGuia.compania,
                                        puntoEmision = resultadoGuia.puntoEmision,
                                        establecimiento = resultadoGuia.establecimiento,
                                        secuencial = resultadoGuia.secuencial,
                                        tipoDocumento = "A",
                                        codigoNumerico = resultadoGuia.codigoNumerico,
                                        codigoRetorno = codigoRetorno,
                                        descripcionRetorno = descripcionRetorno,
                                        tablaMurano = ""
                                    };
                                    _responseGuiaRemision.documentoProcesado.Add(_documento);
                                }
                                else
                                {
                                    if (existeDocumento.Tables.Count > 0)
                                    {
                                        string dsClaveAcceso = existeDocumento.Tables[0].Rows[0]["claveAcceso"].ToString();
                                        idCompania = resultadoGuia.compania.ToString();

                                        if (dsClaveAcceso.Equals(""))
                                        {

                                            if (resultadoGuia.claveAcceso == null || resultadoGuia.claveAcceso.Equals(""))
                                            {
                                                resultadoGuia.claveAcceso = metodoClaveAcceso.GenerarClaveAccesoDocumento(tipoDocumento,
                                                                                                        resultadoGuia.secuencial,
                                                                                                        resultadoGuia.puntoEmision,
                                                                                                        resultadoGuia.establecimiento,
                                                                                                        resultadoGuia.fechaIniTransporte,
                                                                                                        resultadoGuia.ruc,
                                                                                                        resultadoGuia.ambiente);
                                                claveAcceso = resultadoGuia.claveAcceso;
                                            }
                                            else { Utilitarios.logs.LogsFactura.LogsInicioFin("Contador Factura 3.2: " + _responseGuiaRemision.codigoRetorno + "--" + _responseGuiaRemision.mensajeRetorno); }
                                        }
                                        else
                                        {
                                            resultadoGuia.claveAcceso = dsClaveAcceso;
                                        }
                                    }

                                    #region Insertar Guia Remision 
                                    if (codigoRetorno.Equals(0))
                                    {
                                        procesoGuiaRemision.InsertarGuiaRemision(resultadoGuia.compania, resultadoGuia.tipoEmision, resultadoGuia.claveAcceso, tipoDocumento,
                                                                                resultadoGuia.establecimiento, resultadoGuia.puntoEmision, resultadoGuia.secuencial,
                                                                                resultadoGuia.direccionPartida, resultadoGuia.razonSocialTransportista,
                                                                                resultadoGuia.tipoIdentificacionTransportista, resultadoGuia.rucTransportista, resultadoGuia.rise,
                                                                                resultadoGuia.fechaIniTransporte, resultadoGuia.fechaFinTransporte, resultadoGuia.placa,
                                                                                resultadoGuia.contingenciaDet, resultadoGuia.email, resultadoGuia.claveAcceso,
                                                                                "", "", "", resultadoGuia.ambiente, resultadoGuia.codigoNumerico, resultadoGuia.ruc,
                                                                                ref codigoRetorno, ref descripcionRetorno, ref ciGuiaRemision);
                                    }
                                    #endregion Insertar Guia Remision 

                                    #region Insertar Guia Remision Destinatario
                                    if (codigoRetorno.Equals(0))
                                    {
                                        foreach (var destinatario in resultadoGuia.destinatario)
                                        {
                                            procesoGuiaRemision.InsertarGuiaRemisionDestinatario(resultadoGuia.compania, resultadoGuia.establecimiento, resultadoGuia.puntoEmision,
                                                                                                  resultadoGuia.secuencial, destinatario.identificacionDestinatario,
                                                                                                  destinatario.razonSocialDestinatario, destinatario.direccionDestinatario,
                                                                                                  destinatario.motivoTraslado, destinatario.documentoAduaneroUnico,
                                                                                                  destinatario.codigoEstablecimientoDestino, destinatario.ruta,
                                                                                                  destinatario.tipoDocumentoSustento, destinatario.numeroDocumentoSustento,
                                                                                                  destinatario.numeroAutorizacionDocumentoSustento, destinatario.fechaEmisionDocumentoSustento,
                                                                                                  ref codigoRetorno, ref descripcionRetorno);

                                            #region Insertar Guia Remision Detalle
                                            if (codigoRetorno.Equals(0))
                                            {
                                                resultadoGuia.idGuiaRemision = ciGuiaRemision;

                                                if (destinatario.detalle.Count > 0)
                                                {
                                                    foreach (var detalle in destinatario.detalle)
                                                    {
                                                        procesoGuiaRemision.InsertarGuiaRemisionDestinatarioDetalle(resultadoGuia.compania, resultadoGuia.establecimiento, resultadoGuia.puntoEmision,
                                                                                                                    resultadoGuia.secuencial, detalle.identificacionDestinatario,
                                                                                                                    detalle.codigoInterno, detalle.codigoAdicional, detalle.descripcion.Replace(Environment.NewLine, ""),
                                                                                                                    detalle.cantidad, ref codigoRetorno, ref descripcionRetorno);

                                                        #region Insertar Guia Remision Detalle Adicional
                                                        if (codigoRetorno.Equals(0))
                                                        {
                                                            foreach (var detAdicional in detalle.adicional)
                                                            {
                                                                procesoGuiaRemision.InsertarGuiaRemisionDestinatarioDetalleAdicional(resultadoGuia.compania, resultadoGuia.establecimiento, resultadoGuia.puntoEmision,
                                                                                                                                     resultadoGuia.secuencial, detAdicional.identificacionDestinatario,
                                                                                                                                     detAdicional.codigoInterno, detAdicional.nombre, detAdicional.valor,
                                                                                                                                     ref codigoRetorno, ref descripcionRetorno);
                                                            }
                                                        }
                                                        #endregion Insertar Guia Remision Detalle Adicional
                                                    }
                                                }
                                                else
                                                {
                                                    codigoRetorno = 1;
                                                    descripcionRetorno = "Guia Remision sin Detalles";
                                                }
                                            }
                                            #endregion Insertar Guia Remision Detalle

                                        }
                                    }
                                    #endregion Insertar Guia Remision Destinatario                           

                                    #region Insertar Guia Remision Informacion Adicional
                                    if (codigoRetorno.Equals(0))
                                    {
                                        if (resultadoGuia.infoAdicional.Count > 0)
                                        {
                                            foreach (var infoAdicional in resultadoGuia.infoAdicional)
                                            {
                                                procesoGuiaRemision.InsertarGuiaRemisionInfoAdicional(resultadoGuia.compania, resultadoGuia.establecimiento, resultadoGuia.puntoEmision,
                                                                                                      resultadoGuia.secuencial, infoAdicional.nombre, infoAdicional.valor,
                                                                                                      ref codigoRetorno, ref descripcionRetorno);
                                            }
                                        }
                                        else
                                        {
                                            codigoRetorno = 1;
                                            descripcionRetorno = "Guia de Remision sin Información Adicional";
                                        }
                                    }
                                    #endregion Insertar Guia Remision Informacion Adicional

                                    #region Actualizar Estado de Factura 
                                    if (codigoRetorno.Equals(0))
                                    {
                                        XmlGenerados item = new XmlGenerados()
                                        {
                                            CiCompania = resultadoGuia.compania,
                                            CiTipoDocumento = tipoDocumento,
                                            ClaveAcceso = resultadoGuia.claveAcceso,
                                            Identity = resultadoGuia.idGuiaRemision,
                                            XmlEstado = "A",
                                            CiContingenciaDet = 4
                                        };
                                        documentos.ActualizarEstadosComprobantes(item, ref codigoRetorno, ref descripcionRetorno);
                                    }
                                    #endregion Actualizar Estado de Factura 
                                    _responseGuiaRemision.mensajeRetorno = descripcionRetorno;

                                    DocumentosProcesados _documento = new DocumentosProcesados()
                                    {
                                        ciDocumento = 0,
                                        claveAcceso = resultadoGuia.claveAcceso,
                                        compania = resultadoGuia.compania,
                                        puntoEmision = resultadoGuia.puntoEmision,
                                        establecimiento = resultadoGuia.establecimiento,
                                        secuencial = resultadoGuia.secuencial,
                                        tipoDocumento = "A",
                                        codigoNumerico = resultadoGuia.codigoNumerico,
                                        codigoRetorno = codigoRetorno,
                                        descripcionRetorno = descripcionRetorno,
                                        tablaMurano = ""
                                    };

                                    _responseGuiaRemision.documentoProcesado.Add(_documento);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                            codigoRetorno = 9999;
                            descripcionRetorno = "Detalle Error: " + ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _responseGuiaRemision.codigoRetorno = 9999;
                _responseGuiaRemision.mensajeRetorno = "Error Inesperado: " + ex.Message;
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                //objLogs.creaLogMongo("Metodo Wcf", "9999", idCompania, claveAcceso, "Procesar Nota Debito", _responseGuiaRemision.mensajeRetorno, DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), "ViaDoc.ServicioWcf");
            }
            return _responseGuiaRemision;
        }

        public LiquidacionResponse ProcesaLiquisacion(List<Liquidacion> liquidacion)
        {
            LiquidacionResponse _liquidacionResponse = new LiquidacionResponse();
            ClaveAcceso metodoClaveAcceso = new ClaveAcceso();
            ProcesoLiquidacion procesoLiquidacion = new ProcesoLiquidacion();

            int codigoRetorno = 0;
            int ciLiquidacion = 0;
            string descripcionRetorno = string.Empty;
            string tipoDocumento = string.Empty;
            string claveAcceso = string.Empty;
            string idCompania = string.Empty;

            try
            {
                if (liquidacion.Count > 0)
                {
                    foreach (var resultadoliquidacion in liquidacion)
                    {
                        codigoRetorno = 0;

                        try
                        {
                            #region 2: Verifica si Existe Documentos ya en ViaDoc
                            DataSet existeDocumento = null;
                            tipoDocumento = ConfigurationManager.AppSettings["tipoDocumento.Liquidacion"];

                            existeDocumento = procesoLiquidacion.verificaExisteDocumento(tipoDocumento,
                                                                                 resultadoliquidacion.compania,
                                                                                 resultadoliquidacion.establecimiento,
                                                                                 resultadoliquidacion.puntoEmision,
                                                                                 resultadoliquidacion.secuencial,
                                                                                 ref codigoRetorno,
                                                                                 ref descripcionRetorno);

                            _liquidacionResponse.codigoRetorno = codigoRetorno;
                            _liquidacionResponse.mensajeRetorno = descripcionRetorno;

                            #endregion 2: Verifica si Existe Documentos ya en ViaDoc
                            idCompania = resultadoliquidacion.compania.ToString();

                            if (_liquidacionResponse.codigoRetorno.Equals(0))
                            {

                                if (Convert.ToInt32(existeDocumento.Tables[0].Rows[0]["codigoRetorno"]).Equals(0) && !existeDocumento.Tables[0].Rows[0]["claveAcceso"].ToString().Equals(""))
                                {
                                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("El Documento SI Existe en viadoc | CodigoRetorno: " + existeDocumento.Tables[0].Rows[0]["codigoRetorno"]);
                                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("idCompañia: " + resultadoliquidacion.compania + " | " + "#Doc: " + resultadoliquidacion.establecimiento + "-" + resultadoliquidacion.puntoEmision + "-" + resultadoliquidacion.secuencial);
                                    _liquidacionResponse.mensajeRetorno = descripcionRetorno;

                                    DocumentosProcesados _documento = new DocumentosProcesados()
                                    {
                                        ciDocumento = 0,
                                        claveAcceso = existeDocumento.Tables[0].Rows[0]["claveAcceso"].ToString(),
                                        compania = resultadoliquidacion.compania,
                                        puntoEmision = resultadoliquidacion.puntoEmision,
                                        establecimiento = resultadoliquidacion.establecimiento,
                                        secuencial = resultadoliquidacion.secuencial,
                                        tipoDocumento = "A",
                                        codigoNumerico = resultadoliquidacion.codigoNumerico,
                                        codigoRetorno = codigoRetorno,
                                        descripcionRetorno = descripcionRetorno,
                                        tablaMurano = ""
                                    };
                                    _liquidacionResponse.documentoProcesado.Add(_documento);
                                }
                                else
                                {
                                    if (existeDocumento.Tables.Count > 0)
                                    {
                                        string dsClaveAcceso = existeDocumento.Tables[0].Rows[0]["claveAcceso"].ToString();
                                        if (dsClaveAcceso.Equals("") || dsClaveAcceso == null)
                                        {
                                            #region 3: Generar Clave de Acceso
                                            if (resultadoliquidacion.claveAcceso == null || resultadoliquidacion.claveAcceso.Equals(""))
                                            {
                                                resultadoliquidacion.claveAcceso = metodoClaveAcceso.GenerarClaveAccesoDocumento(tipoDocumento,
                                                                                                        resultadoliquidacion.secuencial,
                                                                                                        resultadoliquidacion.puntoEmision,
                                                                                                        resultadoliquidacion.establecimiento,
                                                                                                        resultadoliquidacion.fechaEmision,
                                                                                                        resultadoliquidacion.ruc,
                                                                                                        resultadoliquidacion.ambiente);
                                                claveAcceso = resultadoliquidacion.claveAcceso;
                                            }


                                            #endregion 3: Generar Clave de Acceso
                                        }
                                        else
                                        {
                                            ///////////////Borrar
                                            string clav = "";
                                            clav = metodoClaveAcceso.GenerarClaveAccesoDocumento(tipoDocumento,
                                                                                                    resultadoliquidacion.secuencial,
                                                                                                    resultadoliquidacion.puntoEmision,
                                                                                                    resultadoliquidacion.establecimiento,
                                                                                                    resultadoliquidacion.fechaEmision,
                                                                                                    resultadoliquidacion.ruc,
                                                                                                    resultadoliquidacion.ambiente);

                                            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Clave ViaDoc: " + clav);
                                            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Clave Pto Venta: " + resultadoliquidacion.claveAcceso);
                                            ///////////////hasta aqui borrar
                                            ///
                                            resultadoliquidacion.claveAcceso = dsClaveAcceso;
                                        }
                                    }

                                    if (codigoRetorno.Equals(0))
                                    {
                                        #region 5: Insertar Cabecera Liquidacion
                                        procesoLiquidacion.InsertarLiquidacion(resultadoliquidacion.compania, resultadoliquidacion.tipoEmision, resultadoliquidacion.claveAcceso, tipoDocumento,
                                                                       resultadoliquidacion.establecimiento, resultadoliquidacion.puntoEmision, resultadoliquidacion.secuencial,
                                                                       resultadoliquidacion.fechaEmision, resultadoliquidacion.tipoIdentificacionProvedor,
                                                                       resultadoliquidacion.razonSocialProvedor, resultadoliquidacion.identificacionProvedor, resultadoliquidacion.totalSinImpuestos,
                                                                       resultadoliquidacion.totalDescuento, resultadoliquidacion.CodDocReembolso, resultadoliquidacion.totalComprobantesReembolso, resultadoliquidacion.totalBaseImponibleReembolso
                                                                       , resultadoliquidacion.totalImpuestoReembolso, resultadoliquidacion.importeTotal, resultadoliquidacion.moneda,
                                                                       resultadoliquidacion.contingenciaDet, resultadoliquidacion.email, resultadoliquidacion.claveAcceso, "", "", "",
                                                                       "", resultadoliquidacion.ambiente, ref codigoRetorno, ref descripcionRetorno, ref ciLiquidacion);

                                        #endregion 5: Insertar Cabecera Liquidacion
                                        #region 6: Insertar Detalle Liquidacion
                                        if (codigoRetorno.Equals(0))
                                        {
                                            if (resultadoliquidacion.Liquidaciondetalle.Count > 0)
                                            {
                                                resultadoliquidacion.idLiquidacion = ciLiquidacion;
                                                foreach (LiquidacionDetalle detalleLiquidacion in resultadoliquidacion.Liquidaciondetalle)
                                                {
                                                    procesoLiquidacion.InsertarLiquidacionDetalle(resultadoliquidacion.compania, resultadoliquidacion.establecimiento, resultadoliquidacion.puntoEmision,
                                                                                          resultadoliquidacion.secuencial, detalleLiquidacion.codigoPrincipal, detalleLiquidacion.codigoAuxiliar,
                                                                                          detalleLiquidacion.descripcion.Replace(Environment.NewLine, ""), detalleLiquidacion.cantidad, detalleLiquidacion.precioUnitario,
                                                                                          detalleLiquidacion.descuento, detalleLiquidacion.precioTotalSinImpuesto, ref codigoRetorno,
                                                                                          ref descripcionRetorno);
                                                    if (codigoRetorno.Equals(0))
                                                    {
                                                        if (detalleLiquidacion.LiquidaciondetalleAdicional.Count > 1)
                                                        {
                                                            foreach (LiquidacionDetalleAdicional detalleAdicional in detalleLiquidacion.LiquidaciondetalleAdicional)
                                                            {
                                                                procesoLiquidacion.InsertarliquidacionDetalleAdicional(resultadoliquidacion.compania, resultadoliquidacion.establecimiento, resultadoliquidacion.puntoEmision,
                                                                                              resultadoliquidacion.secuencial, detalleAdicional.codigoPrincipal, detalleAdicional.nombre,
                                                                                              detalleAdicional.valor, ref codigoRetorno, ref descripcionRetorno);
                                                            }
                                                        }
                                                    }

                                                    if (codigoRetorno.Equals(0))
                                                    {
                                                        if (detalleLiquidacion.LiquidaciondetalleImpuesto.Count > 0)
                                                        {
                                                            foreach (LiquidacionDetalleImpuesto detalleImpuesto in detalleLiquidacion.LiquidaciondetalleImpuesto)
                                                            {
                                                                procesoLiquidacion.InsertarLiquidacionDetalleImpuesto(resultadoliquidacion.compania, resultadoliquidacion.establecimiento, resultadoliquidacion.puntoEmision,
                                                                                              resultadoliquidacion.secuencial, detalleImpuesto.codigoPrincipal, detalleImpuesto.codigo,
                                                                                              detalleImpuesto.codigoPorcentaje, detalleImpuesto.tarifa.ToString(), detalleImpuesto.baseImponible,
                                                                                              detalleImpuesto.valor, ref codigoRetorno, ref descripcionRetorno);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            codigoRetorno = 1;
                                                            descripcionRetorno = "Liquidacion No posee Detalle Impuesto..";
                                                        }
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                codigoRetorno = 1;
                                                descripcionRetorno = "Liquidacion sin detalles";
                                            }
                                        }

                                        if (codigoRetorno.Equals(0))
                                        {
                                            if (resultadoliquidacion.LiquidaciontotalImpuesto.Count > 0)
                                            {
                                                foreach (LiquidacionTotalImpuesto totalImpuesto in resultadoliquidacion.LiquidaciontotalImpuesto)
                                                {
                                                    procesoLiquidacion.InsertarTotalLiquidacion(resultadoliquidacion.compania, resultadoliquidacion.establecimiento, resultadoliquidacion.puntoEmision,
                                                                                          resultadoliquidacion.secuencial, totalImpuesto.codigo, totalImpuesto.codigoPorcentaje,
                                                                                          totalImpuesto.tarifa, totalImpuesto.descuentoAdicional, totalImpuesto.baseImponible,
                                                                                          totalImpuesto.valor, ref codigoRetorno, ref descripcionRetorno);
                                                }
                                            }
                                            else
                                            {
                                                codigoRetorno = 1;
                                                descripcionRetorno = "Liquidacion No posee Total Impuesto..";
                                            }
                                        }
                                        #endregion 6: Insertar Detalle Liquidacion
                                        #region 7: Insertar Liquidacion Forma de Pago
                                        if (codigoRetorno.Equals(0))
                                        {
                                            if (resultadoliquidacion.LiquidacionformaPago.Count > 0)
                                            {
                                                foreach (LiquidacionDetalleFormaPago Liquidacionapago in resultadoliquidacion.LiquidacionformaPago)
                                                {
                                                    Utilitarios.logs.LogsFactura.LogsInicioFin(resultadoliquidacion.secuencial + " - " + Liquidacionapago.formaPago + " - " + Liquidacionapago.plazo + " - " + Liquidacionapago.total + " - " + Liquidacionapago.unidadTiempo);
                                                    procesoLiquidacion.InsertarLiquidacioDetalleFormaPago(resultadoliquidacion.compania, resultadoliquidacion.establecimiento, resultadoliquidacion.puntoEmision,
                                                                                          resultadoliquidacion.secuencial, Liquidacionapago.formaPago, Liquidacionapago.plazo, Liquidacionapago.unidadTiempo,
                                                                                          Liquidacionapago.total, ref codigoRetorno, ref descripcionRetorno);
                                                }
                                            }
                                            else
                                            {
                                                codigoRetorno = 1;
                                                descripcionRetorno = "No posee Forma de Pago..";
                                            }
                                        }
                                        #endregion 7: Insertar Liquidacion Forma de Pago
                                        #region Insertar Liquidacion Informacion Adicional
                                        if (codigoRetorno.Equals(0))
                                        {
                                            if (resultadoliquidacion.LiquidacioninfoAdicional.Count > 0)
                                            {
                                                foreach (LiquidacionInfoAdicional infoAdicional in resultadoliquidacion.LiquidacioninfoAdicional)
                                                {
                                                    procesoLiquidacion.InsertarLiquidacionInfoAdicional(resultadoliquidacion.compania, resultadoliquidacion.establecimiento, resultadoliquidacion.puntoEmision,
                                                                                      resultadoliquidacion.secuencial, infoAdicional.nombre, infoAdicional.valor, ref codigoRetorno,
                                                                                      ref descripcionRetorno);
                                                }
                                            }
                                            else
                                            {
                                                codigoRetorno = 1;
                                                descripcionRetorno = "No posee Informacion Adicional..";
                                            }
                                        }
                                        #endregion Insertar Liquidacion Informacion Adicional
                                        #region Insertar Liquidaicon Reembolso
                                        if (codigoRetorno.Equals(0))
                                        {
                                            if (resultadoliquidacion.LiquidacionReembolso.Count > 0)
                                            {
                                                foreach (LiquidacionReembolso LiquiReembolso in resultadoliquidacion.LiquidacionReembolso)
                                                {
                                                    procesoLiquidacion.InsertarLiquidacionReembolso(resultadoliquidacion.compania, resultadoliquidacion.tipoIdentificacionProvedor, resultadoliquidacion.identificacionProvedor,
                                                                                                    LiquiReembolso.txCodPaisPagoProveedorReembolso, LiquiReembolso.txTipoProveedorReembolso,
                                                                                                    resultadoliquidacion.CodDocReembolso, resultadoliquidacion.establecimiento, resultadoliquidacion.puntoEmision,
                                                                                                    resultadoliquidacion.secuencial, resultadoliquidacion.fechaEmision, resultadoliquidacion.claveAcceso,
                                                                                                    LiquiReembolso.codigo, LiquiReembolso.codigoPorcentaje, LiquiReembolso.tarifa,
                                                                                                    resultadoliquidacion.totalBaseImponibleReembolso, resultadoliquidacion.totalImpuestoReembolso,
                                                                                                    ref codigoRetorno, ref descripcionRetorno);
                                                }
                                            }
                                            else
                                            {
                                                codigoRetorno = 1;
                                                descripcionRetorno = "No posee Informacion Adicional..";
                                            }
                                        }
                                        #endregion
                                        #region Actualizar Estado de Liquidacion 
                                        if (codigoRetorno.Equals(0))
                                        {
                                            XmlGenerados item = new XmlGenerados()
                                            {
                                                CiCompania = resultadoliquidacion.compania,
                                                CiTipoDocumento = tipoDocumento,
                                                ClaveAcceso = resultadoliquidacion.claveAcceso,
                                                Identity = resultadoliquidacion.idLiquidacion,
                                                XmlEstado = "A",
                                                CiContingenciaDet = 4

                                            };
                                            documentos.ActualizarEstadosComprobantes(item, ref codigoRetorno, ref descripcionRetorno);
                                        }
                                        #endregion Actualizar Estado de Liquidacion
                                    }
                                    _liquidacionResponse.mensajeRetorno = descripcionRetorno;

                                    DocumentosProcesados _documento = new DocumentosProcesados()
                                    {
                                        ciDocumento = 0,
                                        claveAcceso = resultadoliquidacion.claveAcceso,
                                        compania = resultadoliquidacion.compania,
                                        puntoEmision = resultadoliquidacion.puntoEmision,
                                        establecimiento = resultadoliquidacion.establecimiento,
                                        secuencial = resultadoliquidacion.secuencial,
                                        tipoDocumento = "A",
                                        codigoNumerico = resultadoliquidacion.codigoNumerico,
                                        codigoRetorno = codigoRetorno,
                                        descripcionRetorno = descripcionRetorno,
                                        tablaMurano = ""
                                    };
                                    _liquidacionResponse.documentoProcesado.Add(_documento);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("error Liquidacion:" + ex.Message);
                            codigoRetorno = 9999;
                            descripcionRetorno = "Detalle Error:" + ex.Message;
                            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(descripcionRetorno);
                        }
                    }
                }
                else
                {
                    _liquidacionResponse.codigoRetorno = 1;
                    _liquidacionResponse.mensajeRetorno = "Vacio";
                }
            }
            catch (Exception ex)
            {
                _liquidacionResponse.codigoRetorno = 9999;
                _liquidacionResponse.mensajeRetorno = "Exception: " + ex.Message;
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(descripcionRetorno);
            }

            return _liquidacionResponse;
        }
    }
}