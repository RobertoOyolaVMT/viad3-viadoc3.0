using javax.swing.text.html;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using ViaDoc.AccesoDatos.usuario;
using ViaDoc.EntidadNegocios.usuario;

namespace ViaDoc.LogicaNegocios.usuarios
{
    public class ProcesoUsuario
    {


        public void VerificarUsuario(string usuario, string contrasenia, ref string nombreUsuario, ref string usuarioCorrecto,
                                     ref int codigoRetorno, ref string mensajeRetorno)
        {
            UsuarioAD bdUsuario = new UsuarioAD();
            try
            {
                DataSet dsResultado = new DataSet();

                dsResultado = bdUsuario.MantenimientoUsuarios("LO", "", "", "", "", "", usuario.Trim(), contrasenia.Trim(),
                                                              "", "", "", "", 0, 0, "", "", ref codigoRetorno, ref mensajeRetorno);

                if (codigoRetorno.Equals(0))
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        if (dsResultado.Tables[0].Rows.Count > 0)
                        {
                            codigoRetorno = int.Parse(dsResultado.Tables[0].Rows[0]["CodigoRetorno"].ToString());

                            if (codigoRetorno.Equals(0))
                            {
                                nombreUsuario = dsResultado.Tables[0].Rows[0]["Nombre"].ToString();
                                usuarioCorrecto = dsResultado.Tables[0].Rows[0]["Usuario"].ToString();
                            }
                            else
                            {
                                mensajeRetorno = dsResultado.Tables[0].Rows[0]["MensajeRetorno"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }


        public List<Perfil> ObtenerMenu(string usuario, ref int codigoRetorno, ref string mensajeRetorno)
        {
            UsuarioAD bdUsuario = new UsuarioAD();
            List<Perfil> listaPerfil = new List<Perfil>();
            try
            {
                DataSet dsResultado = new DataSet();

                dsResultado = bdUsuario.MantenimientoUsuarios("MENU", "", "", "", "", "", usuario.Trim(), "",
                                                              "", "", "", "", 0, 0, "", "",
                                                              ref codigoRetorno, ref mensajeRetorno);
                List<Perfil> listaPerfiles = new List<Perfil>();
                if (codigoRetorno.Equals(0))
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        if (dsResultado.Tables[0].Rows.Count > 0)
                        {
                            DataTable dtMenu = dsResultado.Tables[0];
                            DataTable dtOpciones = dsResultado.Tables[1];


                            foreach (DataRow listaMenu in dtMenu.Rows)
                            {
                                int idCodigo = int.Parse(listaMenu["ciCodigoPerfiles"].ToString());

                                List<Opcion> listaOpciones = new List<Opcion>();
                                foreach (DataRow listaOpcion in dtOpciones.Rows)
                                {

                                    int idMenu = int.Parse(listaOpcion["ciCodigoPerfiles"].ToString());
                                    if (idMenu == idCodigo)
                                    {
                                        Opcion opcion = new Opcion()
                                        {
                                            NombreOpcion = listaOpcion["nombreOpcion"].ToString(),
                                            rutaControlador = listaOpcion["rutaControlador"].ToString(),
                                            rutaAccion = listaOpcion["rutaAccion"].ToString(),
                                            iconoOpcion = listaOpcion["iconoOpcion"].ToString(),
                                            codigoPerfil = int.Parse(listaMenu["ciCodigoPerfiles"].ToString())
                                        };
                                        listaOpciones.Add(opcion);
                                    }
                                }

                                Perfil perfil = new Perfil()
                                {
                                    idCodigo = int.Parse(listaMenu["ciCodigoPerfiles"].ToString()),
                                    iconoMenu = listaMenu["opcionIcono"].ToString(),
                                    NombreMenu = listaMenu["nombreMenu"].ToString(),
                                    listaOpcion = listaOpciones
                                };
                                listaPerfil.Add(perfil);




                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                codigoRetorno = 9999;
                mensajeRetorno = "DataSet de consulta NULL" + ex.ToString();
            }

            return listaPerfil;
        }

        public List<Usuarios> ConsultaUsuarios(ref int codigoRetorno, ref string mensajeRetorno)
        {
            List<Usuarios> ObjUsuario = new List<Usuarios>();
            UsuarioAD bdUsuario = new UsuarioAD();


            try
            {
                DataSet dsResultado = bdUsuario.MantenimientoUsuarios("CP", "", "", "", "", "", "", "",
                                                                       "", "", "", "", 0, 0, "", "", ref codigoRetorno, ref mensajeRetorno);


                if (codigoRetorno.Equals(0))
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        if (dsResultado.Tables[0].Rows.Count > 0)
                        {
                            DataTable dtMenu = dsResultado.Tables[0];
                            DataTable dtOpciones = dsResultado.Tables[1];

                            foreach (DataRow row in dsResultado.Tables[0].Rows)
                            {
                                Usuarios US = new Usuarios()
                                {
                                    ciCodigoPerfiles = row["ciCodigoPerfiles"].ToString().Trim(),
                                    txNombreOpcion = row["txNombreOpcion"].ToString().ToUpper(),
                                    ciEstado = row["ciEstado"].ToString().ToUpper(),
                                };

                                ObjUsuario.Add(US);
                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                codigoRetorno = 9999;
                mensajeRetorno = "DataSet de consulta NULL" + ex.ToString();
            }

            return ObjUsuario;
        }

        public List<MantUsuario> NewUser(string opcion, string CodUsuario, string NueUser, string NueContasenia, string XmlOpciones, ref int codigoRetorno, ref string mensajeRetorno)
        {
            List<MantUsuario> ObjMantUser = new List<MantUsuario>();
            UsuarioAD bdUsuario = new UsuarioAD();

            try
            {
                DataSet dsResultado = bdUsuario.MantenimientoUsuarios(opcion, "", "", "", "", CodUsuario, NueUser, NueContasenia,
                                                                     "", "", XmlOpciones, "", 0, 0, "", "", ref codigoRetorno, ref mensajeRetorno);
                if (codigoRetorno.Equals(0))
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        DataTable dtNU = dsResultado.Tables[1];
                        DataTable dtMU = null;
                        bool existe = dsResultado.Tables.Contains("Table2") ? true : false;
                        if (existe)
                        {
                            dtMU = dsResultado.Tables[2];
                        }

                        if (dsResultado.Tables[0].Rows.Count > 0)
                        {
                            if (opcion.Equals("NU"))
                            {
                                foreach (DataRow row in dtNU.Rows)
                                {
                                    codigoRetorno = Convert.ToInt32(row["CodigoRetorno"].ToString());
                                    mensajeRetorno = row["MensajeRetorno"].ToString().ToUpper();
                                }
                            }
                            else if (opcion.Equals("MU"))
                            {
                                foreach (DataRow row in dtMU.Rows)
                                {
                                    codigoRetorno = Convert.ToInt32(row["CodigoRetorno"].ToString());
                                    mensajeRetorno = row["MensajeRetorno"].ToString().ToUpper();
                                }
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                codigoRetorno = 9999;
                mensajeRetorno = "DataSet de consulta NULL" + ex.ToString();
            }


            return ObjMantUser;
        }


        public List<ConsultaUser> ConsultaUser(string Opcion, ref int codigoRetorno, ref string mensajeRetorno)
        {
            List<ConsultaUser> ObjConsultaUser = new List<ConsultaUser>();
            UsuarioAD bdUsuario = new UsuarioAD();


            try
            {
                DataSet dsResultado = bdUsuario.MantenimientoUsuarios(Opcion, "", "", "", "", "", "", "",
                                                                       "", "", "", "", 0, 0, "", "", ref codigoRetorno, ref mensajeRetorno);


                if (codigoRetorno.Equals(0))
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        if (dsResultado.Tables[0].Rows.Count > 0)
                        {
                            DataTable PerfilUser = dsResultado.Tables[0];
                            DataTable DatosUser = dsResultado.Tables[1];

                            foreach (DataRow row in dsResultado.Tables[0].Rows)
                            {
                                ConsultaUser CU = new ConsultaUser()
                                {
                                    txUsuario = row["txUsuario"].ToString().Trim(),
                                    ciEstado = row["ciEstado"].ToString().Trim(),
                                };

                                ObjConsultaUser.Add(CU);
                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                codigoRetorno = 9999;
                mensajeRetorno = "DataSet de consulta NULL" + ex.ToString();
            }

            return ObjConsultaUser;
        }

        public List<ConsultaUsuarioModel> ConsultaUserMod(string Opcion, string User, ref int codigoRetorno, ref string mensajeRetorno)
        {
            List<ConsultaUsuarioModel> ObjCUM = new List<ConsultaUsuarioModel>();
            UsuarioAD bdUsuario = new UsuarioAD();


            try
            {
                DataSet dsResultado = bdUsuario.MantenimientoUsuarios(Opcion, "", "", "", "", "", User, "",
                                                                       "", "", "", "", 0, 0, "", "", ref codigoRetorno, ref mensajeRetorno);


                if (codigoRetorno.Equals(0))
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        if (dsResultado.Tables[0].Rows.Count > 0)
                        {
                            DataTable PerfilUser = dsResultado.Tables[0];
                            DataTable DatosUser = dsResultado.Tables[1];
                            DataTable CheckUser = dsResultado.Tables[2];

                            foreach (DataRow row in dsResultado.Tables[1].Rows)
                            {
                                ConsultaUsuarioModel CUM = new ConsultaUsuarioModel()
                                {
                                    txCodigoUsuario = row["txCodigoUsuario"].ToString().Trim(),
                                    txUsuario = row["txUsuario"].ToString().Trim(),
                                    txPassword = row["txPassword"].ToString().Trim(),
                                    CiEstado = row["CiEstado"].ToString().Trim(),
                                };

                                ObjCUM.Add(CUM);
                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                codigoRetorno = 9999;
                mensajeRetorno = "DataSet de consulta NULL" + ex.ToString();
            }

            return ObjCUM;
        }

        public List<PerfilUserAsig> ConsultaUsuariosMod(string Opcion, string User, ref int codigoRetorno, ref string mensajeRetorno)
        {
            List<PerfilUserAsig> ObjPUA = new List<PerfilUserAsig>();
            UsuarioAD bdUsuario = new UsuarioAD();


            try
            {
                DataSet dsResultado = bdUsuario.MantenimientoUsuarios(Opcion, "", "", "", "", "", User, "",
                                                                       "", "", "", "", 0, 0, "", "", ref codigoRetorno, ref mensajeRetorno);


                if (codigoRetorno.Equals(0))
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        if (dsResultado.Tables[0].Rows.Count > 0)
                        {
                            DataTable PerfilUser = dsResultado.Tables[0];
                            DataTable DatosUser = dsResultado.Tables[1];
                            DataTable CheckUser = dsResultado.Tables[2];

                            foreach (DataRow row in dsResultado.Tables[2].Rows)
                            {
                                PerfilUserAsig PUA = new PerfilUserAsig()
                                {
                                    txUsuario = row["txUsuario"].ToString().Trim(),
                                    ciCodigoPerfiles = row["ciCodigoOpciones"].ToString().Trim(),
                                    ciEstado = row["ciEstado"].ToString().Trim(),
                                };

                                ObjPUA.Add(PUA);
                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                codigoRetorno = 9999;
                mensajeRetorno = "DataSet de consulta NULL" + ex.ToString();
            }

            return ObjPUA;
        }
    }
}
