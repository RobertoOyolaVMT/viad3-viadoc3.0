using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaDoc.EntidadNegocios.factura;

namespace ViaDoc.Probador
{
    public class GenerarLista
    {

        public List<Factura> GenerarListaFactura(DataSet dsFactura)
        {
            List<Factura> _requestFactura = new List<Factura>();
            DataTable dtFactura = new DataTable();
            if (dsFactura != null)
            {
                dtFactura = dsFactura.Tables[0];


                foreach (DataRow resultados in dtFactura.Rows)
                {

                    Factura _factura = new Factura()
                    {
                        ambiente = resultados[0].ToString()

                    };
                }
            }

            return _requestFactura;
        }



    }
}
