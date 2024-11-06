using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViaDoc.EntidadNegocios.usuario;
using ViaDoc.LogicaNegocios.usuarios;


namespace ViaDoc.WebApp.Models
{
    public class MetodosAdministracion
    {


        public void ValidarUsuario(string usuario, string contrasenia, ref string nombreUsuario, ref string usuarioCorrecto,
                                     ref int codigoRetorno, ref string mensajeRetorno)
        {

            ProcesoUsuario metodosProceso = new ProcesoUsuario();

            metodosProceso.VerificarUsuario(usuario, contrasenia, ref nombreUsuario, ref usuarioCorrecto,
                                           ref codigoRetorno, ref mensajeRetorno);

        }



        public List<Perfil> ObtenerMenu(string usuario)
        {
            List<Perfil> responsePerfiles = new List<Perfil>();
            ProcesoUsuario metodosProceso = new ProcesoUsuario();
            int codigoRetorno = 0;
            string mensajeRetorno = string.Empty;

            responsePerfiles = metodosProceso.ObtenerMenu(usuario, ref codigoRetorno, ref mensajeRetorno);

            return responsePerfiles;
        }
    }
}