using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using ViaDoc.AccesoDatos.portalWeb;
using ViaDoc.EntidadNegocios.portalWeb;
using static ViaDoc.EntidadNegocios.portalWeb.PorInicio;

namespace ViaDoc.LogicaNegocios.portalweb
{
    public class Portalinicio
    {
        public DataSet Reporte(int opcion, ref int codigoRetorno, ref string mensajeRetorno)
        {
            DataSet dsRespuesta = null;
            Bienvenido Inicio = new Bienvenido();

            try
            {

                dsRespuesta = Inicio.ReporteInicio(opcion, ref codigoRetorno, ref mensajeRetorno);
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
            }

            return dsRespuesta;
        }

        public List<Compañia> Compania(int opcion, ref int codigoRetorno, ref string mensajeRetorno)
        {
            Bienvenido Inicio = new Bienvenido();
            List<Compañia> ObjCom = new List<Compañia>();

            try
            {
                DataSet dsRespuesta = Inicio.ReporteInicio(opcion, ref codigoRetorno, ref mensajeRetorno);

                if (!dsRespuesta.Equals(null))
                {
                    DataTable dtRespuesta = dsRespuesta.Tables[0];
                    foreach(DataRow item in dtRespuesta.Rows)
                    {
                        Compañia com = new Compañia(){ 
                            RazonSocial = item["RazonSocial"].ToString().Trim(),
                            Estado = item["Estado"].ToString().Trim()
                        };
                        ObjCom.Add(com);
                    }
                }
            }
            catch (Exception ex)
            {

                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
            }

            return ObjCom;
        }

        public List<Doc> Document(int opcion, ref int codigoRetorno, ref string mensajeRetorno)
        {
            Bienvenido Inicio = new Bienvenido();
            List<Doc> ObjDoc = new List<Doc>();

            try
            {
                DataSet dsRespuesta = Inicio.ReporteInicio(opcion, ref codigoRetorno, ref mensajeRetorno);
                if (!dsRespuesta.Equals(null))
                {
                    DataTable dtRespuesta = dsRespuesta.Tables[0];
                    foreach (DataRow item in dtRespuesta.Rows)
                    {
                        Doc DC = new Doc() 
                        { 
                            Documento = item["Documento"].ToString().Trim(),
                            Estado = item["Estado"].ToString().Trim()
                        };
                        ObjDoc.Add(DC);
                    }
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
            }
            return ObjDoc;
        }
    }
}
