using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViaDoc.EntidadNegocios.compRetencion;
using ViaDoc.EntidadNegocios.factura;



namespace ViaDoc.ServicioWcf.modelo
{
    public class DocumentoRequest
    {
    }


    public class FacturaRequest
    {
        public bool procesaFactura { get; set; }
        //public List<Factura> facturaRequest { get; set; }
        public Factura facturaRequest { get; set; }

        public FacturaRequest()
        {
            facturaRequest = new Factura();
            //facturaRequest = new List<Factura>();
        }
    }

    public class CompRetencionRequest
    {
        public bool procesaCompRetencion { get; set; }
        public List<CompRetencion> compRetencionRequest { get; set; }

        public CompRetencionRequest()
        {
            compRetencionRequest = new List<CompRetencion>();
        }
    }
}