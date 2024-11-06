using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios.usuario
{
    public class Perfil
    {
        public int idCodigo { get; set; }
        public string NombreMenu { get; set; }
        public string iconoMenu { get; set; }
        public List<Opcion> listaOpcion { get; set; }

        public Perfil()
        {
            listaOpcion = new List<Opcion>();
        }
    }

    public class Opcion
    {
        public string NombreOpcion { get; set; }
        public string rutaControlador { get; set; }
        public string rutaAccion { get; set; }
        public string iconoOpcion { get; set; }
        public int codigoPerfil { get; set; }
        public int codigoPerfilOpcion { get; set; }
    }

    public class Usuarios
    {
        public string ciCodigoPerfiles { get; set; }
        public string txNombreOpcion { get; set; }
        public string ciEstado { get; set; }

    }

    public class UsuarioLista
    {
        public List<Usuarios> objlistausuarios = new List<Usuarios>();
    }

    public class MantUsuario
    {
        public string CodUsuario { get; set; }
        public string NueUser { get; set; }
        public string NueContasenia { get; set; }
        public string XmlOpciones { get; set; }
    }
    public class MantUsuarioLista 
    {
        public List<MantUsuario> ObjListMantUsuario = new List<MantUsuario>();
    }

    public class ConsultaUsuarioModel
    {
        public string txCodigoUsuario { get; set; }
        public string txUsuario { get; set; }
        public string CiEstado { get; set; }
        public string txPassword { get; set; }
    }

    public class ConsultaUsuarioModlist
    {
        public List<ConsultaUsuarioModel> ObjConsultaModel = new List<ConsultaUsuarioModel>();
    }

    public class PerfilUserAsig
    {
        public string txUsuario { get; set; }
        public string ciCodigoPerfiles { get; set; }
        public string ciEstado { get; set; }
    }

    public class PerfilUserAsigList
    {
        public List<PerfilUserAsig> ObjPerfilUserAsig = new List<PerfilUserAsig>();
    }

    public class ConsultaUser
    {
        public string txUsuario { get; set; }
        public string ciEstado { get; set; }
    }

    public class ConsultaUserlist
    {
        public List<ConsultaUser> ObjConsultaUser = new List<ConsultaUser>();
    }

    public class ConsultaModelList
    {
        public List<Usuarios> objlistausuarios = new List<Usuarios>();
        public List<ConsultaUsuarioModel> ObjConsultaModelMOD = new List<ConsultaUsuarioModel>();
        public List<PerfilUserAsig> ObjPerfilUserAsigMOD = new List<PerfilUserAsig>();
        public List<ConsultaUser> ObjConsultaUser = new List<ConsultaUser>();
    }
}
