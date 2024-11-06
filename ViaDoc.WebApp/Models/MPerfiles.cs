using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViaDoc.WebApp.Models
{

    public class MPerfiles
    {
        public int idCodigo { get; set; }
        public string NombreMenu { get; set; }
        public string iconoMenu { get; set; }    
        public List<MOpciones> listaOpciones { get; set; }

        public MPerfiles()
        {
            listaOpciones = new List<MOpciones>();
        }
    }

    public class MOpciones
    {
        public int codigoPerfilOpcion { get; set; }
        public int codigoOpcion { get; set; }
        public string NombreOpcion { get; set; }
        public string rutaControlador { get; set; }
        public string rutaAccion { get; set; }
        public string iconoOpcion { get; set; }
        
    }
}