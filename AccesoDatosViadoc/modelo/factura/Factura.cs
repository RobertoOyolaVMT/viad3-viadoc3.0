using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatosViadoc.modelo.factura
{
    class Factura
    {

        public string ciCompania { get; set; }
        public string txEstablecimiento { get; set; }
        public string txPuntoEmision { get; set; }
        public string txSecuencial { get; set; }
        public string txFechaEmision { get; set; }
        public string ciTipoIdentificacionComprador { get; set; }
        public string txGuiaRemision { get; set; }
        public string txRazonSocialComprador { get; set; }
        public string txIdentificacionComprador { get; set; }
        public string qnTotalSinImpuestos { get; set; }
        public string qnTotalDescuento { get; set; }
        public string qnPropina { get; set; }
        public string qnImporteTotal { get; set; }
        public string txMoneda { get; set; }
        public string txEmail { get; set; }
        public string ciAmbiente { get; set; }
        public string ciCodigoNumerico { get; set; }
        public string ruc { get; set; }


        public List<FacturaDetalle> detalleFactura = new List<FacturaDetalle>();
        public List<FacturaTotalImpuesto> totalImpuesto = new List<FacturaTotalImpuesto>();
        public List<FacturaDetalleFormaPago> formaPago = new List<FacturaDetalleFormaPago>();
        public List<FacturaInfoAdicional> infoAdicional = new List<FacturaInfoAdicional>();

        private ConexionViadoc conexion;

    
        public DataSet verificaExisteFactura(int ciCompania, string txEstablecimeinto, string txPuntoEmision, string txSecuencial)
        {

            ConexionViadoc con = new ConexionViadoc();
            string sql = "dbo.SP_ObtenerClaveAcceso_ViaDoc";
            DataSet dsResultado = new DataSet();
            try     
            {               
                con.tipoBase("Viadoc");
                con.crearComandoSql(sql);
                con.agregarParametroSP("@ciCompania", ciCompania, DbType.String, ParameterDirection.Input);
                con.agregarParametroSP("@txEstablecimeinto", txEstablecimeinto, DbType.String, ParameterDirection.Input);
                con.agregarParametroSP("@txPuntoEmision", txPuntoEmision, DbType.String, ParameterDirection.Input);
                con.agregarParametroSP("@txSecuencial", txSecuencial, DbType.String, ParameterDirection.Input);
                dsResultado = con.EjecutarConsultaDatSet();
               
            }
            catch (Exception ex)
            {
                dsResultado = null;
               
            }
            finally
            {
                if (con != null)
                    con.desconectar();
            }
            return dsResultado;

        }


    }
}
