using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaDoc.AccesoDatos.portalWeb;
using ViaDoc.EntidadNegocios;

namespace ViaDoc.LogicaNegocios.catalogos
{
    public class ProcesoCatalogos
    {
        CatalogosAD _metodosConsultaAD = new CatalogosAD();
        int codigoRetorno = 0;
        string descripcionRetorno = string.Empty;

        public List<CatCompania> ConsultaEmpresa()
        {
            List<CatCompania> listaCompania = new List<CatCompania>();
            DataSet dsResultado = _metodosConsultaAD.ConsultaCatalogos(1, ref codigoRetorno, ref descripcionRetorno);

            if (codigoRetorno.Equals(0))
            {
                if (dsResultado.Tables.Count > 0)
                {
                    if(dsResultado.Tables[0].Rows.Count > 0)
                    {
                        DataTable dtResultado = dsResultado.Tables[0];
                        foreach(DataRow _lista in dtResultado.Rows)
                        {
                            CatCompania compania = new CatCompania()
                            {
                                idCompania = int.Parse(_lista["idCompania"].ToString().Trim()),
                                nombreComercial = _lista["nombreComercial"].ToString().Trim(),
                                razonSocial = _lista["razonSocial"].ToString().Trim(),
                                RucCompania = _lista["Ruc"].ToString().Trim()
                            };
                            listaCompania.Add(compania);
                        }
                    }
                }
            }

            return listaCompania;
        }

        public List<CatCompania> ConsultaEmpresaHistorico(string nombreHistorico)
        {
            List<CatCompania> listaCompania = new List<CatCompania>();
            DataSet dsResultado = _metodosConsultaAD.ConsultaCatalogosHistorico(nombreHistorico, 1, ref codigoRetorno, ref descripcionRetorno);

            if (codigoRetorno.Equals(0))
            {
                if (dsResultado.Tables.Count > 0)
                {
                    if (dsResultado.Tables[0].Rows.Count > 0)
                    {
                        DataTable dtResultado = dsResultado.Tables[0];
                        foreach (DataRow _lista in dtResultado.Rows)
                        {
                            CatCompania compania = new CatCompania()
                            {
                                idCompania = int.Parse(_lista["idCompania"].ToString().Trim()),
                                nombreComercial = _lista["nombreComercial"].ToString().Trim(),
                                razonSocial = _lista["razonSocial"].ToString().Trim(),
                                RucCompania = _lista["Ruc"].ToString().Trim()
                            };
                            listaCompania.Add(compania);
                        }
                    }
                }
            }

            return listaCompania;
        }

        public List<CatDocumento> ConsultaDocumento()
        {
            List<CatDocumento> listaDocumento = new List<CatDocumento>();
            DataSet dsResultado = _metodosConsultaAD.ConsultaCatalogos(2, ref codigoRetorno, ref descripcionRetorno);

            if (codigoRetorno.Equals(0))
            {
                if (dsResultado.Tables.Count > 0)
                {
                    if (dsResultado.Tables[0].Rows.Count > 0)
                    {
                        DataTable dtResultado = dsResultado.Tables[0];
                        foreach (DataRow _lista in dtResultado.Rows)
                        {
                            CatDocumento documento = new CatDocumento()
                            {
                                 idTipoDocumento = _lista["idTipoDocumento"].ToString(),
                                 descripcion = _lista["descripcion"].ToString()
                            };
                            listaDocumento.Add(documento);
                        }
                    }
                }
            }

            return listaDocumento;
        }

        public List<CatAmbiente> ConsultaAmbiente()
        {
            List<CatAmbiente> listaDocumento = new List<CatAmbiente>();
            DataSet dsResultado = _metodosConsultaAD.ConsultaCatalogos(3, ref codigoRetorno, ref descripcionRetorno);

            if (codigoRetorno.Equals(0))
            {
                if (dsResultado.Tables.Count > 0)
                {
                    if (dsResultado.Tables[0].Rows.Count > 0)
                    {
                        DataTable dtResultado = dsResultado.Tables[0];
                        foreach (DataRow _lista in dtResultado.Rows)
                        {
                            CatAmbiente documento = new CatAmbiente()
                            {
                                idTipoAmbiente = _lista["idTipoAmbiente"].ToString(),
                                descripcion = _lista["descripcion"].ToString()
                            };
                            listaDocumento.Add(documento);
                        }
                    }
                }
            }

            return listaDocumento;
        }

        public List<CatEstado> ConsultaEstado()
        {
            List<CatEstado> listaEstado = new List<CatEstado>();
            DataSet dsResultado = _metodosConsultaAD.ConsultaCatalogos(5, ref codigoRetorno, ref descripcionRetorno);

            if (codigoRetorno.Equals(0))
            {
                if (dsResultado.Tables.Count > 0)
                {
                    if (dsResultado.Tables[0].Rows.Count > 0)
                    {
                        DataTable dtResultado = dsResultado.Tables[0];
                        foreach (DataRow _lista in dtResultado.Rows)
                        {
                            CatEstado estado = new CatEstado()
                            {
                                idEstado = _lista["idEstado"].ToString(),
                                descripcion = _lista["descripcion"].ToString()
                            };
                            listaEstado.Add(estado);
                        }
                    }
                }
            }

            return listaEstado;
        }

        public List<CatContabilidad> ConsultaContabilidad()
        {
            List<CatContabilidad> listaEstado = new List<CatContabilidad>();
            DataSet dsResultado = _metodosConsultaAD.ConsultaCatalogos(6, ref codigoRetorno, ref descripcionRetorno);

            if (codigoRetorno.Equals(0))
            {
                if (dsResultado.Tables.Count > 0)
                {
                    if (dsResultado.Tables[0].Rows.Count > 0)
                    {
                        DataTable dtResultado = dsResultado.Tables[0];
                        foreach (DataRow _lista in dtResultado.Rows)
                        {
                            CatContabilidad estado = new CatContabilidad()
                            {
                                idContabilidad = _lista["idContabilidad"].ToString(),
                                descripcion = _lista["descripcion"].ToString()
                            };
                            listaEstado.Add(estado);
                        }
                    }
                }
            }

            return listaEstado;
        }
    }
}
