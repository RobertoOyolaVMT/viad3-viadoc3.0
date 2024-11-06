using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaDoc.EntidadNegocios;
using ViaDoc.AccesoDatos.certificado;
using System.Security.Cryptography.X509Certificates;

namespace ViaDoc.LogicaNegocios.certificado
{
    public class ProcesoCertificado
    {
        public List<Certificado> InsertarCertificado(string opcion, string Data, byte[] obCertificado, string Ruc, ref int codigoRetorno, ref string mensajeRetorno)
        {
            List<Certificado> ObjCertificado = new List<Certificado>();
            CertificadoAD bdCert = new CertificadoAD();
            DataSet dsResultado = null;

            try
            {
                dsResultado = bdCert.ManteniemtoCertificado(opcion, Data, obCertificado, Ruc, ref codigoRetorno, ref mensajeRetorno);

                if (codigoRetorno.Equals(0))
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        if (dsResultado.Tables[0].Rows.Count > 0)
                        {
                            DataTable dtmensaje = dsResultado.Tables[0];

                            foreach (DataRow row in dtmensaje.Rows)
                            {
                                codigoRetorno = Convert.ToInt32(row["CodigoRetorno"].ToString());
                                mensajeRetorno = row["MensajeRetorno"].ToString().ToUpper();
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("InsertarCertificado: "+ex.ToString());
                codigoRetorno = 9999;
                mensajeRetorno = "DataSet de consulta NULL" + ex.ToString();
            }

            return ObjCertificado;
        }

        public List<SucuersalCompania> ConsuSecuEmpres(string opcion, string Ruc, ref int codigoRetorno, ref string mensajeRetorno)
        {
            List<SucuersalCompania> ObjSC = new List<SucuersalCompania>();
            CertificadoAD bdCert = new CertificadoAD();
            DataSet dsResultado = null;

            try
            {
                dsResultado = bdCert.ConsultaSucursalCompania(opcion, Ruc, ref codigoRetorno, ref mensajeRetorno);

                if (codigoRetorno.Equals(0))
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        if (dsResultado.Tables[0].Rows.Count > 0)
                        {
                            DataTable dtCertificado = dsResultado.Tables[0];
                            DataTable dtmensaje = dsResultado.Tables[1];

                            foreach (DataRow row in dtCertificado.Rows)
                            {
                                SucuersalCompania SC = new SucuersalCompania()
                                {
                                    secuencialCia = row["secuencialCia"].ToString().Trim(),
                                    newIdcompañia = row["newIdcompañia"].ToString().Trim(),
                                };
                                ObjSC.Add(SC);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                codigoRetorno = 9999;
                mensajeRetorno = "DataSet de consulta NULL" + ex.ToString();
            }

            return ObjSC;
        }

        public bool DetallleCertificado( string Ruta, string pass, ref Certificado certificado)
        {
            string FcDesde = string.Empty;
            string FcHasta = string.Empty;
            bool Valida = false;

            try
            {
                X509Certificate2 objCert = new X509Certificate2(Ruta, pass);

                FcDesde = objCert.NotBefore.ToString().Trim();
                FcHasta = objCert.NotAfter.ToString().Trim();

                certificado.FcDesde = Convert.ToDateTime(FcDesde);
                certificado.FcHasta = Convert.ToDateTime(FcHasta);
                Valida = true;
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                Valida = false;
            }

            return Valida;
        }

        public string RetornaUiSemilla(Guid keytmp)
        {
            string key = keytmp.ToString().Replace("-", "");
            try
            {

                key = key.Substring(0, key.Length / 2); // esto es la clave el uisemilla
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
            }
            return key;
        }
    }
}
