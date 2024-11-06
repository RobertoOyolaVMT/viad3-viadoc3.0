using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ViaDoc.EntidadNegocios.usuario;
using ViaDoc.WebApp.Models;

namespace ViaDoc.WebApp.Controllers
{
    public class NavbarController : Controller
    {
        // GET: Navbar
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Navbar()
        {
            MetodosAdministracion metodosAdministrativo = new MetodosAdministracion();
            List<Perfil> perfilesUsuario = new List<Perfil>();
            string usuario = Session["usuarioLogin"].ToString();
            List<MPerfiles> perfilesUsuarioModel = new List<MPerfiles>();
            perfilesUsuario = await Task.Run(() => metodosAdministrativo.ObtenerMenu(usuario));

            foreach (var _perfil in perfilesUsuario)
            {
                List<MOpciones> listaOpciones = new List<MOpciones>();
                int ciCodigo = _perfil.idCodigo;

                foreach (var _opciones in _perfil.listaOpcion)
                {
                    int ciCodigoPerfil = _opciones.codigoPerfil;

                    if (ciCodigo == ciCodigoPerfil)
                    {
                        MOpciones mOpciones = new MOpciones()
                        {
                            codigoOpcion = _opciones.codigoPerfil,
                            codigoPerfilOpcion = _opciones.codigoPerfilOpcion,
                            iconoOpcion = _opciones.iconoOpcion,
                            NombreOpcion = _opciones.NombreOpcion,
                            rutaControlador = _opciones.rutaControlador,
                            rutaAccion = _opciones.rutaAccion
                        };
                        listaOpciones.Add(mOpciones);
                    }
                }

                MPerfiles _mperfil = new MPerfiles()
                {
                    iconoMenu = _perfil.iconoMenu,
                    idCodigo = _perfil.idCodigo,
                    NombreMenu = _perfil.NombreMenu,
                    listaOpciones = listaOpciones
                };
                perfilesUsuarioModel.Add(_mperfil);
            }
            return PartialView("_Navbar", perfilesUsuarioModel);
        }
    }
}