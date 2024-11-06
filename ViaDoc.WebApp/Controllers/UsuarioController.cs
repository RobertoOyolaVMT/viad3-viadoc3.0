using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViaDoc.EntidadNegocios;
using ViaDoc.EntidadNegocios.portalWeb;
using ViaDoc.WebApp.Models;
using ViaDoc.EntidadNegocios.usuario;
using ViaDoc.LogicaNegocios.catalogos;
using ViaDoc.LogicaNegocios.usuarios;
using System.Security.Cryptography;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Data;
using com.sun.org.apache.xml.@internal.serialize;

namespace ViaDoc.WebApp.Controllers
{
    public class UsuarioController : Controller
    {

        private int codigoRetorno = 0;
        private string mensajeRetorno = string.Empty;
        ProcesoUsuario ProcUser = new ProcesoUsuario();

        // GET: Usuario
        public ActionResult Index()
        {
            if (Session["nombreUsuario"] == null)
            {
                return RedirectToAction("IniciarSesion", "Validacion");
            }
            ConsultaModelList ObjTblManUser = new ConsultaModelList();
            ObjTblManUser.objlistausuarios = ProcUser.ConsultaUsuarios(ref codigoRetorno, ref mensajeRetorno);
            ObjTblManUser.ObjConsultaUser = ProcUser.ConsultaUser("CU", ref codigoRetorno, ref mensajeRetorno);

            return View(ObjTblManUser);
        }

        public ActionResult MantenimientoUsers(string data, string dato_Perfil)
        {
            Utilitarios.Utilitarios objPassword = new Utilitarios.Utilitarios();
            string Password = string.Empty;
            string dataUser = data.Trim();
            string[] cadena = dataUser.Split('|');
            string[] cadena_1 = dato_Perfil.Split('|');
            string CodUsuario = cadena[1].Trim();
            string Opcion = cadena[0].Trim();
            string User = cadena[2].Trim();
            if (cadena[3].ToString().Trim().Equals(""))
            {

                ConsultaModelList ObjCML = new ConsultaModelList();
                ObjCML.ObjConsultaModelMOD = ProcUser.ConsultaUserMod("CP", User, ref codigoRetorno, ref mensajeRetorno);
                Password = ObjCML.ObjConsultaModelMOD[0].txPassword.ToString().Trim();
            }
            else 
            {
                Password = objPassword.Convertir_MD5(cadena[3].Trim());
            }
             
            string EstadoUser = cadena[4].Trim();
            string XmlOpciones = string.Empty;

            if (Opcion.Equals("NU"))
            {
                foreach(string COD in cadena_1)
                {
                    if (!COD.Equals(""))
                    {
                        XmlOpciones = XmlOpciones + "<table><CodigoOpcion>" + COD.Trim() + "</CodigoOpcion></table>";
                    }
                }
                XmlOpciones = "<root>" + XmlOpciones + "</root>";
                MantUsuarioLista ObjNewUser = new MantUsuarioLista();
                ObjNewUser.ObjListMantUsuario = ProcUser.NewUser(Opcion, CodUsuario, User, Password, XmlOpciones, ref codigoRetorno, ref mensajeRetorno);
            }
            else if (Opcion.Equals("MU") && EstadoUser.Equals("A"))
            {
                foreach (string COD in cadena_1)
                {
                    if (!COD.Equals(""))
                    {
                        XmlOpciones = XmlOpciones + "<table><CodigoOpcion>" + COD.Trim() + "</CodigoOpcion></table>";
                    }
                }
                XmlOpciones = "<root>" + XmlOpciones + "</root>";

                MantUsuarioLista ObjNewUser = new MantUsuarioLista();
                ObjNewUser.ObjListMantUsuario = ProcUser.NewUser(Opcion, CodUsuario, User, Password, XmlOpciones,  ref codigoRetorno, ref mensajeRetorno);
            }
            else if (Opcion.Equals("MU") && EstadoUser.Equals("I"))
            {

                MantUsuarioLista ObjNewUser = new MantUsuarioLista();
                ObjNewUser.ObjListMantUsuario = ProcUser.NewUser("EU", "", User, "", "", ref codigoRetorno, ref mensajeRetorno);
            }
            return base.Json(this.mensajeRetorno);
        }

        public ActionResult ConsultaMod(string Opcion, string User)
        {
            string IdUser = string.Empty;
            string User_1 = string.Empty;
            string MensajeError = string.Empty;
            string Password = string.Empty;
            string ciCodigoPerfiles = string.Empty;
            ConsultaModelList ObjCML = new ConsultaModelList();
            ObjCML.ObjConsultaModelMOD = ProcUser.ConsultaUserMod(Opcion, User, ref codigoRetorno, ref mensajeRetorno);
            ObjCML.ObjPerfilUserAsigMOD = ProcUser.ConsultaUsuariosMod(Opcion, User, ref codigoRetorno, ref mensajeRetorno);

            IdUser = ObjCML.ObjConsultaModelMOD[0].txCodigoUsuario.ToString().Trim();
            User_1 = ObjCML.ObjConsultaModelMOD[0].txUsuario.ToString().Trim();

            MensajeError = mensajeRetorno.Trim();

            for (int i = 0; i < ObjCML.ObjPerfilUserAsigMOD.Count; i++)
            {
                if (ciCodigoPerfiles.Equals(""))
                    ciCodigoPerfiles = ciCodigoPerfiles + ObjCML.ObjPerfilUserAsigMOD[i].ciCodigoPerfiles.ToString().Trim();
                else
                    ciCodigoPerfiles = ciCodigoPerfiles + "," + ObjCML.ObjPerfilUserAsigMOD[i].ciCodigoPerfiles.ToString().Trim();
            }

            mensajeRetorno = MensajeError + "|" + IdUser + "|" + User_1 + "|" + ciCodigoPerfiles;

            return base.Json(this.mensajeRetorno);
        }
    }
}
