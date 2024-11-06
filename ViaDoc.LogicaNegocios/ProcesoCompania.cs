using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaDoc.AccesoDatos;
using ViaDoc.AccesoDatos.compania;

namespace ViaDoc.LogicaNegocios
{
    public class ProcesoCompania
    {
        
        public DataSet ConsultaCompanias_y_Certificados(ref int codigoRetorno, ref string descripcionRetorno)
        {
            DataSet dsResultadoCompaniasCertificados = new DataSet();
           
            CompaniaAD companiaAD = new CompaniaAD();
            dsResultadoCompaniasCertificados = companiaAD.ConsultaCompanias_y_Certificados(1, ref codigoRetorno, ref descripcionRetorno);

            return dsResultadoCompaniasCertificados;
        }


        public DataTable ConsultarEsquemasDocumentos(string ciTipoDocumento, string txtVersionEsquema, ref int codigoRetorno, ref string mensajeRetorno)
        {
            DataTable dtConsultaEsquemaDocumentos = new DataTable();
            CompaniaAD companiaAD = new CompaniaAD();

            try
            {
                DataSet dsConsultaEsquema = companiaAD.ConsultarEsquemas(ciTipoDocumento, txtVersionEsquema, ref codigoRetorno, ref mensajeRetorno);

                if (dsConsultaEsquema.Tables.Count > 0)
                {
                    dtConsultaEsquemaDocumentos = dsConsultaEsquema.Tables[0];
                }
            }
            catch
            {

            }
            finally
            {

            }
            
            return dtConsultaEsquemaDocumentos;
        }
    }
}
