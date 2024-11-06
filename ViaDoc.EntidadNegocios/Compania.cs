using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaDoc.AccesoDatos;
using ViaDoc.AccesoDatos.compania;

namespace ViaDoc.EntidadNegocios
{


    public class CompaniaLista
    {
        public List<Compania> objListaCompania { get; set; }
    }

    public class Compania
    {
        public int CiCompania { get; set; }
        public string UiCompania { get; set; }
        public string TxRuc { get; set; }
        public string TxRazonSocial { get; set; }
        public string TxNombreComercial { get; set; }
        public string TxDireccionMatriz { get; set; }
        public string TxContribuyenteEspecial { get; set; }
        public string TxObligadoContabilidad { get; set; }
        public string TxAgenteRetencion { get; set; }
        public string TxRegimenMicroempresas { get; set; }
        public string TxContribuyenteRimpe { get; set; }
        public int CiTipoAmbiente { get; set; }
        public string TipoAmbiente { get; set; }
        public string CiEstado { get; set; }
        public string Estado { get; set; }
        public Byte[] LogoCompania { get; set; }


        public Compania()
        {
            this.CiCompania = 0;
            this.UiCompania = "";
            this.TxRuc = "";
            this.TxRazonSocial = "";
            this.TxNombreComercial = "";
            this.TxDireccionMatriz = "";
            this.TxContribuyenteEspecial = "";
            this.TxObligadoContabilidad = "";
            this.TxAgenteRetencion = "";
            this.TxRegimenMicroempresas = "";
            this.TxContribuyenteRimpe = "";
            this.CiTipoAmbiente = 0;
            this.TipoAmbiente = "";
            this.CiEstado = "";
            this.Estado = "";
            this.LogoCompania = null;
        }

        
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
