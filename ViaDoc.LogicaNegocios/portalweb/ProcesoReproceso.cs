using System;
using System.Collections.Generic;
using System.Data;
using ViaDoc.AccesoDatos.portalWeb;
using ViaDoc.EntidadNegocios.portalWeb;

namespace ViaDoc.LogicaNegocios.portalweb
{
    public class ProcesoReproceso
    {
        ReprocesoAD Rep_Doc = new ReprocesoAD();
        
        public List<ResprocesoMD> ConsultaDocError(string compania, string Tipodocu, string NumDocu, string Fecha, string FechaHAsta, string CLaveAcceso, string Opcion, ref int codigoRetorno, ref string mensajeRetorno)
        {
            List<ResprocesoMD> objDocError = new List<ResprocesoMD>();
            try
            {
                DataSet dsRespuesta = Rep_Doc.ConsutaReproceso(compania, Tipodocu, NumDocu, Fecha, FechaHAsta, CLaveAcceso, Opcion, ref codigoRetorno, ref mensajeRetorno);
                if (dsRespuesta.Tables[0].Rows.Count > 0) 
                {
                    bool existe = dsRespuesta.Tables[0].Columns.Contains("RazonSocial") ? true : false;
                    if (existe)
                    {
                        foreach (DataRow row in dsRespuesta.Tables[0].Rows)
                        {
                            ResprocesoMD RM = new ResprocesoMD()
                            {
                                RazonSocial = row["RazonSocial"].ToString().Trim(),
                                TipoDocumento = row["TipoDocumento"].ToString().Trim(),
                                NumeroDocumento = row["NumeroDocumento"].ToString().Trim(),
                                ClaveAcceso = row["ClaveAcceso"].ToString().Trim(),
                                FechaEmision = row["FechaEmision"].ToString().Trim(),
                                FechaHoraAutorizacion = row["FechaHoraAutorizacion"].ToString().Trim(),
                                Estado = row["Estado"].ToString().Trim(),
                                CiEstado = row["CiEstado"].ToString().Trim(),
                                CodError = row["CodError"].ToString().Trim(),
                                MenError = row["MenError"].ToString().Trim(),
                                NumeroCiclos = Convert.ToInt32(row["NumeroCiclos"].ToString().Trim())
                            };
                            objDocError.Add(RM);
                        }
                    }
                    else
                    {
                        mensajeRetorno = "Estado Modificado";
                    }
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
            }

            return objDocError;
        }

    }
}
