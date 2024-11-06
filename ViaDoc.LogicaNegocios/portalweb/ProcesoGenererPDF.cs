using sun.reflect.generics.scope;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaDoc.AccesoDatos.portalWeb;
using ViaDoc.EntidadNegocios.portalWeb;

namespace ViaDoc.LogicaNegocios.portalweb
{
    public class ProcesoGenererPDF
    {
        GenerarPDFAD metodoGenraPDF = new GenerarPDFAD();

        public DataSet ConsultaGenraPDF(int Opcion, string codEmpresa, string codDocumento, string fechaDesde, string fechaHasta, ref int codigoRetorno, ref string mensajeRetorno)
        {
            DataSet dsResultado = null;
            try
            {
                dsResultado = metodoGenraPDF.ConsultaGenerePDF(Opcion,codEmpresa, codDocumento, fechaDesde, fechaHasta, ref codigoRetorno, ref mensajeRetorno);

            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Error: " + ex.Message);
            }

            return dsResultado;
        }
    }
}
