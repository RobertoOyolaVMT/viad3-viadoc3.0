using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViaDoc.EntidadNegocios;
using ViaDoc.LogicaNegocios.catalogos;

namespace ViaDoc.WebApp.Models
{
    public class MetodosCatalogos
    {

        ProcesoCatalogos _procesoCatalogo = new ProcesoCatalogos();
        public List<MCompania> ConsultaCompania()
        {
            List<MCompania> listaCompania = new List<MCompania>();
            List<CatCompania> compania = _procesoCatalogo.ConsultaEmpresa();

            foreach(var _lista in compania)
            {
                MCompania mCompania = new MCompania()
                {
                    idCompania = _lista.idCompania,
                    nombreComercial = _lista.nombreComercial,
                    razonSocial = _lista.razonSocial,
                    RucCompania = _lista.RucCompania
                };
                listaCompania.Add(mCompania);
            }
            return listaCompania;
        }


        public List<MDocumento> ConsultaDocumento()
        {
            List<MDocumento> listaDocumento = new List<MDocumento>();
            List<CatDocumento> documento = _procesoCatalogo.ConsultaDocumento();

            foreach (var _lista in documento)
            {
                MDocumento mDocumento = new MDocumento()
                {
                    idTipoDocumento = _lista.idTipoDocumento,
                    descripcion = _lista.descripcion
                };
                listaDocumento.Add(mDocumento);
            }
            return listaDocumento;
        }
    }
}