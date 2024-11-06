using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaDoc.AccesoDatos.winServMigracion;
using ViaDoc.EntidadNegocios.factura;

namespace ViaDocMigrador.LogicaNegocios.PorocesoDocumentos
{
    public class MigraFactura
    {
        public void Factura()
        {
            ProcesoMigracion conexion = new ProcesoMigracion();
            try
            {
                DataSet dsFactura = conexion.ConsultaFactura();
                dsFactura.Tables[0].TableName = "Factura";
                dsFactura.Tables[1].TableName = "FacturaDetalle";
                dsFactura.Tables[2].TableName = "FacturaInfoAdicional";
                dsFactura.Tables[3].TableName = "FacturaTotalImpuesto";
                dsFactura.Tables[4].TableName = "AmbienteCompanias";
                dsFactura.Tables[5].TableName = "FacturaDetalleFormaPago";

                foreach (DataRow FacturaCab in dsFactura.Tables["Factura"].Rows)
                {
                    Factura ObjfacturaCab = new Factura();
                    ObjfacturaCab.compania = Convert.ToInt32(FacturaCab["ciCompania"].ToString().Trim());
                    ObjfacturaCab.establecimiento = FacturaCab["txEstablecimiento"].ToString().Trim();
                    ObjfacturaCab.puntoEmision = FacturaCab["txPuntoEmision"].ToString().Trim();
                    ObjfacturaCab.secuencial = FacturaCab["txSecuencial"].ToString().Trim();
                    ObjfacturaCab.fechaEmision = FacturaCab["txFechaEmision"].ToString().Trim();
                    ObjfacturaCab.tipoIdentificacionComprador = FacturaCab["ciTipoIdentificacionComprador"].ToString().Trim();
                    ObjfacturaCab.guiaRemision = FacturaCab["txGuiaRemision"].ToString().Trim();
                    ObjfacturaCab.razonSocialComprador = FacturaCab["txRazonSocialComprador"].ToString().Trim();
                    ObjfacturaCab.identificacionComprador = FacturaCab["txIdentificacionComprador"].ToString().Trim();
                    ObjfacturaCab.totalSinImpuestos = Convert.ToDecimal(FacturaCab["qnTotalSinImpuestos"].ToString().Trim());
                    ObjfacturaCab.totalDescuento = Convert.ToDecimal(FacturaCab["qnTotalSinImpuestos"].ToString().Trim());
                    ObjfacturaCab.propina = Convert.ToDecimal(FacturaCab["qnPropina"].ToString().Trim());
                    ObjfacturaCab.importeTotal = Convert.ToDecimal(FacturaCab["qnImporteTotal"].ToString().Trim());
                    ObjfacturaCab.moneda = FacturaCab["txMoneda"].ToString().Trim();
                    ObjfacturaCab.email = FacturaCab["txEmail"].ToString().Trim();

                    DataView dv = new DataView(dsFactura.Tables["FacturaDetalle"]);
                    dv.RowFilter = "txEstablecimiento+'-'+txPuntoEmision+'-'+txSecuencial = '" + ObjfacturaCab.establecimiento + "-" + ObjfacturaCab.puntoEmision + "-" + ObjfacturaCab.secuencial + "' and ciCompania ='" + ObjfacturaCab.compania + "'";
                    DataTable Facturadetalle = dv.ToTable();

                    foreach (DataRow FacDetalle in Facturadetalle.Rows)
                    {
                        FacturaDetalle ObjFacturaDetalle = new FacturaDetalle();
                        ObjFacturaDetalle.codigoPrincipal = FacDetalle["txCodigoPrincipal"].ToString().Trim();
                        ObjFacturaDetalle.codigoAuxiliar = FacDetalle["txCodigoAuxiliar"].ToString().Trim();
                        ObjFacturaDetalle.descripcion = FacDetalle["txDescripcion"].ToString().Trim();
                        ObjFacturaDetalle.cantidad = Convert.ToInt32(FacDetalle["qnCantidad"].ToString().Trim());
                        ObjFacturaDetalle.precioUnitario = Convert.ToDecimal(FacDetalle["qnPrecioUnitario"].ToString().Trim());
                        ObjFacturaDetalle.descuento = Convert.ToDecimal(FacDetalle["qnDescuento"].ToString().Trim());
                        ObjFacturaDetalle.precioTotalSinImpuesto = Convert.ToDecimal(FacDetalle["qnPrecioTotalSinImpuesto"].ToString().Trim());
                    }

                    DataView dv1 = new DataView(dsFactura.Tables["FacturaInfoAdicional"]);
                    dv1.RowFilter = "txEstablecimiento+'-'+txPuntoEmision+'-'+txSecuencial = '" + ObjfacturaCab.establecimiento + "-" + ObjfacturaCab.puntoEmision + "-" + ObjfacturaCab.secuencial + "' and ciCompania ='" + ObjfacturaCab.compania + "'";
                    DataTable FacturadetalleAdicional = dv.ToTable();

                    foreach (DataRow FactDetalleAdicional in FacturadetalleAdicional.Rows)
                    {
                        FacturaDetalleAdicional ObjFactDetalleAdi = new FacturaDetalleAdicional();
                        ObjFactDetalleAdi.codigoPrincipal = FactDetalleAdicional["txCodigoPrincipal"].ToString().Trim();
                        ObjFactDetalleAdi.nombre = FactDetalleAdicional["txNombre"].ToString().Trim();
                        ObjFactDetalleAdi.valor = FactDetalleAdicional["txValor"].ToString().Trim();
                    }

                    DataView dv2 = new DataView(dsFactura.Tables["FacturaTotalImpuesto"]);
                    dv2.RowFilter = "txEstablecimiento+'-'+txPuntoEmision+'-'+txSecuencial = '" + ObjfacturaCab.establecimiento + "-" + ObjfacturaCab.puntoEmision + "-" + ObjfacturaCab.secuencial + "' and ciCompania ='" + ObjfacturaCab.compania + "'";
                    DataTable FacturaTotalImpuesto = dv.ToTable();

                    foreach(DataRow FacTotalImpuesto in FacturaTotalImpuesto.Rows)
                    {
                        FacturaDetalleImpuesto ObjFacTotalImpuesto = new FacturaDetalleImpuesto();
                        ObjFacTotalImpuesto.codigo = FacTotalImpuesto[""].ToString().Trim();


                    }
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
            }
        }
    }
}
