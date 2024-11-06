using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios.portalWeb
{
    public class PorInicio
    {
        public class Compañia
        {
            public string RazonSocial { get; set; }
            public string Estado { get; set; }
        }
        public class listaComp
        {
            public List<Compañia> objComp = new List<Compañia>();
        }

        public class Doc
        {
            public string Documento { get; set; }
            public string Estado { get; set; }
        }
        public class listDoc
        {
            public List<Doc> objDoc = new List<Doc>();
        }

        public class ListInicio
        {
            public List<Compañia> objComp = new List<Compañia>();
            public List<Doc> objDoc = new List<Doc>();
        }
    }
}
