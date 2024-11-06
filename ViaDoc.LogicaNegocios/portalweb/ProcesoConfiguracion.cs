using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using ViaDoc.AccesoDatos.compania;
using ViaDoc.AccesoDatos.portalWeb;
using ViaDoc.AccesoDatos.winServAutorizacion;
using ViaDoc.AccesoDatos.winServFirmas;
using ViaDoc.Configuraciones;
using ViaDoc.EntidadNegocios;
using ViaDoc.EntidadNegocios.portalWeb;

namespace ViaDoc.LogicaNegocios.portalweb
{
    public class ProcesoConfiguracion
    {

        public List<Parametros> ConsultaParametros(int idCompania, ref int codigoRetorno, ref string mensajeRetorno)
        {
            List<Parametros> listaParametros = new List<Parametros>();
            ConfiguracionAD configuracionAD = new ConfiguracionAD();
            DataSet dsResultado = configuracionAD.MantenimientoDocumentos(1, "", idCompania, 0, 0, 0, 0, 0,
                                                                          0, 0, ref codigoRetorno, ref mensajeRetorno);

            if (codigoRetorno.Equals(0))
            {
                if (dsResultado.Tables.Count > 0)
                {
                    if (dsResultado.Tables[0].Rows.Count > 0)
                    {
                        DataTable dtResultado = dsResultado.Tables[0];

                        foreach (DataRow _lista in dtResultado.Rows)
                        {
                            Parametros _parametros = new Parametros()
                            {
                                idRegistro = int.Parse(_lista["idRegistro"].ToString()),
                                descripcion = _lista["descripcion"].ToString(),
                                idTipoDocumento = _lista["idTipoDocumento"].ToString(),
                                cantidadFirma = int.Parse(_lista["cantidadFirma"].ToString()),
                                cantidadAutorizacion = int.Parse(_lista["cantidadAutorizacion"].ToString()),
                                cantidadCorreo = int.Parse(_lista["cantidadCorreo"].ToString()),
                                reprocesoFirma = int.Parse(_lista["reprocesoFirma"].ToString()),
                                reprocesoCorreo = int.Parse(_lista["reprocesoCorreo"].ToString()),
                                reprocesoAutorizacion = int.Parse(_lista["reprocesoAutorizacion"].ToString()),
                                estado = _lista["estado"].ToString(),
                                idCompania = int.Parse(_lista["idCompania"].ToString()),
                                nombreComercial = _lista["nombreComercial"].ToString(),
                            };
                            listaParametros.Add(_parametros);
                        }
                    }
                }
                else
                {
                    codigoRetorno = 1;
                    mensajeRetorno = "";
                }
            }
            else
            {
                codigoRetorno = 1;
                mensajeRetorno = "";
            }
            return listaParametros;
        }


        public List<HoraNotificacion> ConsultaParametrosHorasNotificacion(ref int codigoRetorno, ref string mensajeRetorno)
        {
            List<HoraNotificacion> listaParametros = new List<HoraNotificacion>();
            ConfiguracionAD configuracionAD = new ConfiguracionAD();
            var dsResultado = configuracionAD.MantenimientoDocumentosHorasNotificacion(1, "", ref codigoRetorno, ref mensajeRetorno);

            if (codigoRetorno.Equals(0))
            {
                int contador = 0;
                foreach(var item in dsResultado)
                {
                    contador++;
                    string[] hora = item.Split('-');

                    listaParametros.Add(new HoraNotificacion()
                    {
                        idRegistro = contador,
                        HoraInicio = hora[0],
                        HoraFin = hora[1]
                    });
                }
            }
            else
            {
                codigoRetorno = 1;
                mensajeRetorno = "";
            }
            return listaParametros;
        }

        public List<HoraNotificacion> ConsultaParametrosHorasReproceso(int tipoProceso, ref int codigoRetorno, ref string mensajeRetorno)
        {
            List<HoraNotificacion> listaParametros = new List<HoraNotificacion>();
            ConfiguracionAD configuracionAD = new ConfiguracionAD();
            var dsResultado = configuracionAD.MantenimientoDocumentosHorasReproceso(1, tipoProceso, "", ref codigoRetorno, ref mensajeRetorno);

            if (codigoRetorno.Equals(0))
            {
                int contador = 0;
                foreach (var item in dsResultado)
                {
                    contador++;
                    string[] hora = item.Split('-');

                    listaParametros.Add(new HoraNotificacion()
                    {
                        idRegistro = contador,
                        HoraInicio = hora[0],
                        HoraFin = hora[1]
                    });
                }
            }
            else
            {
                codigoRetorno = 1;
                mensajeRetorno = "";
            }
            return listaParametros;
        }

        public void InsertarNotificacionHoras(List<HoraNotificacion> parametros, ref int codigoRetorno, ref string mensajeRetorno)
        {
            ConfiguracionAD configuracionAD = new ConfiguracionAD();
            string listaHoraNotificacion = string.Empty;

            foreach(var item in parametros)
            {
                listaHoraNotificacion = listaHoraNotificacion + item.HoraInicio + "-" + item.HoraFin;
                listaHoraNotificacion = listaHoraNotificacion + Environment.NewLine;
            }

           configuracionAD.MantenimientoDocumentosHorasNotificacion(2, listaHoraNotificacion, ref codigoRetorno, ref mensajeRetorno);            
        }

        public void InsertarHorasReproceso(int tipoProceso, List<HoraNotificacion> parametros, ref int codigoRetorno, ref string mensajeRetorno)
        {
            ConfiguracionAD configuracionAD = new ConfiguracionAD();
            string listaHoraNotificacion = string.Empty;

            foreach (var item in parametros)
            {
                listaHoraNotificacion = listaHoraNotificacion + item.HoraInicio + "-" + item.HoraFin;
                listaHoraNotificacion = listaHoraNotificacion + Environment.NewLine;
            }

            configuracionAD.MantenimientoDocumentosHorasReproceso(2, tipoProceso, listaHoraNotificacion, ref codigoRetorno, ref mensajeRetorno);
        }

        public void InsertarParametros(Parametros parametros, ref int codigoRetorno, ref string mensajeRetorno)
        {
            ConfiguracionAD configuracionAD = new ConfiguracionAD();
            DataSet dsResultado = configuracionAD.MantenimientoDocumentos(2, parametros.idTipoDocumento, parametros.idCompania,
                                                                          parametros.idRegistro, parametros.cantidadFirma,
                                                                          parametros.cantidadAutorizacion, parametros.cantidadCorreo,
                                                                          parametros.reprocesoFirma, parametros.reprocesoCorreo,
                                                                          parametros.reprocesoAutorizacion,
                                                                          ref codigoRetorno, ref mensajeRetorno);
            if (codigoRetorno.Equals(0))
            {
                if (dsResultado.Tables.Count > 0)
                {
                    if (dsResultado.Tables[0].Rows.Count > 0)
                    {
                        codigoRetorno = int.Parse(dsResultado.Tables[0].Rows[0]["codigoRetorno"].ToString());
                        mensajeRetorno = dsResultado.Tables[0].Rows[0]["mensajeRetorno"].ToString();
                    }
                    else
                    {
                        codigoRetorno = 1;
                        mensajeRetorno = "Tuvimos un incoveniente al realizar este proceso";
                    }
                }
                else
                {
                    codigoRetorno = 1;
                    mensajeRetorno = "Tuvimos un incoveniente al realizar este proceso";
                }
            }
            else
            {
                codigoRetorno = 1;
                mensajeRetorno = "Tuvimos un incoveniente al realizar este proceso";
            }
        }

        public void EliminarParametros(Parametros parametros, ref int codigoRetorno, ref string mensajeRetorno)
        {
            ConfiguracionAD configuracionAD = new ConfiguracionAD();
            DataSet dsResultado = configuracionAD.MantenimientoDocumentos(3, "", 0, parametros.idRegistro, 0, 0, 0, 0,
                                                                          0, 0, ref codigoRetorno, ref mensajeRetorno);
            if (codigoRetorno.Equals(0))
            {
                if (dsResultado.Tables.Count > 0)
                {
                    if (dsResultado.Tables[0].Rows.Count > 0)
                    {
                        codigoRetorno = int.Parse(dsResultado.Tables[0].Rows[0]["codigoRetorno"].ToString());
                        mensajeRetorno = dsResultado.Tables[0].Rows[0]["mensajeRetorno"].ToString();
                    }
                    else
                    {
                        codigoRetorno = 1;
                        mensajeRetorno = "Tuvimos un incoveniente al realizar este proceso";
                    }
                }
                else
                {
                    codigoRetorno = 1;
                    mensajeRetorno = "Tuvimos un incoveniente al realizar este proceso";
                }
            }
            else
            {
                codigoRetorno = 1;
                mensajeRetorno = "Tuvimos un incoveniente al realizar este proceso";
            }
        }

        public List<Smtp> ConsultaConfigSmtp(Smtp smtp, ref int codigoRetorno, ref string mensajeRetorno)
        {
            List<Smtp> listaSmtp = new List<Smtp>();

            try
            {
                CompaniaAD configuracion = new CompaniaAD();
                DataTable dtResultado = configuracion.GuardaConfigSMTP("C2", smtp.CiCompania, smtp.RazonSocial, "", "", "", "", "", "",
                    "", "", "", "", ref codigoRetorno, ref mensajeRetorno);

                if (codigoRetorno.Equals(0))
                {
                    if (dtResultado.Rows.Count > 0)
                    {
                        foreach (DataRow lista in dtResultado.Rows)
                        {
                            Smtp _smtp = new Smtp()
                            {
                                CiCompania = lista["ciCompania"].ToString(),
                                HostServidor = lista["HostServidor"].ToString(),
                                Puerto = lista["puerto"].ToString(),
                                EnableSsl = lista["EnableSsl"].ToString(),
                                EmailCredencial = lista["emailCredencial"].ToString(),
                                PassCredencial = lista["passCredencial"].ToString(),
                                MailAddressfrom = lista["MailAddressfrom"].ToString(),
                                Para = lista["para"].ToString(),
                                Cc = lista["cc"].ToString(),
                                Asunto = lista["Asunto"].ToString(),
                                UrlPortal = lista["urlPortal"].ToString(),
                                RazonSocial = lista["txRazonSocial"].ToString(),
                                RucCompania = lista["txRuc"].ToString(),
                                ActivarNotificacion = lista["activarNotificacion"].ToString()
                            };
                            listaSmtp.Add(_smtp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                mensajeRetorno = ex.Message;
            }
            return listaSmtp;
        }

        public void InsertarConfiguracionSmtp(Smtp smtp, ref int codigoRetorno, ref string mensajeRetorno)
        {
            try
            {
                CompaniaAD configuracion = new CompaniaAD();
                DataTable dtResultado = configuracion.GuardaConfigSMTP("N", smtp.CiCompania, smtp.RazonSocial, smtp.HostServidor,
                smtp.Puerto, smtp.EnableSsl, smtp.EmailCredencial, smtp.PassCredencial, smtp.MailAddressfrom, smtp.Para, smtp.Cc,
                smtp.Asunto, smtp.ActivarNotificacion, ref codigoRetorno, ref mensajeRetorno);

                if (codigoRetorno.Equals(0))
                {
                    if (dtResultado.Rows.Count > 0)
                    {
                        foreach(DataRow row in dtResultado.Rows)
                        {
                            mensajeRetorno = row["MensajeRetorno"].ToString().Trim().ToUpper();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                mensajeRetorno = ex.Message;
            }
        }

        public void EliminarConfiguracionSmtp(Smtp smtp, ref int codigoRetorno, ref string mensajeRetorno)
        {
            try
            {
                CompaniaAD configuracion = new CompaniaAD();

                DataTable dtResultado = configuracion.GuardaConfigSMTP("E", smtp.CiCompania, "", "", "", "", "", "", "",
                   "", "", "", "", ref codigoRetorno, ref mensajeRetorno);
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                mensajeRetorno = ex.Message;
            }
        }


        public List<Compania> ConsultaCompanias(Compania compania,string DataCompania ,ref int codigoRetorno, ref string mensajeRetorno)
        {
            List<Compania> listaCompania = new List<Compania>();
            try
            {
                DataSet dsResultado = new CompaniaAD().MantenimientoCompania(1, 0, "", compania.TxRazonSocial, "", "", ""
                                                        , "", "","","","", "", null, "", ref codigoRetorno, ref mensajeRetorno);

                if (codigoRetorno.Equals(0))
                {
                    foreach (DataRow lista in dsResultado.Tables[0].Rows)
                    {
                        Compania _compania = new Compania()
                        {
                            CiCompania = int.Parse(lista["ciCompania"].ToString().Trim()),
                            TxRuc = lista["txRuc"].ToString().Trim(),
                            TxRazonSocial = lista["txRazonSocial"].ToString().Trim(),
                            TxNombreComercial = lista["txNombreComercial"].ToString().Trim(),
                            TxDireccionMatriz = lista["txDireccionMatriz"].ToString().Trim(),
                            TxContribuyenteEspecial = lista["txContribuyenteEspecial"].ToString().Trim(),
                            TxAgenteRetencion = lista["txAgenteRetencion"].ToString().Trim(),
                            TxRegimenMicroempresas = lista["txRegimenMicroempresas"].ToString().Trim(),
                            TxContribuyenteRimpe = lista["txContribuyenteRimpe"].ToString().Trim(),
                            UiCompania = lista["uiCompania"].ToString().Trim(),
                            TxObligadoContabilidad = lista["txObligadoContabilidad"].ToString().Trim(),
                            CiEstado = lista["ciEstado"].ToString().Trim(),
                            CiTipoAmbiente = int.Parse(lista["ciTipoAmbiente"].ToString().Trim())
                        };
                        listaCompania.Add(_compania);
                    }
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                mensajeRetorno = ex.Message;
            }
            return listaCompania;
        }

        public List<Compania> InsertarCompanias(Compania compania, int opcion,ref int codigoRetorno, ref string mensajeRetorno)
        {
            List<Compania> listaCompania = new List<Compania>();
            try
            {
               DataSet dsresp = new CompaniaAD().MantenimientoCompania(opcion, 0, compania.TxRuc, compania.TxRazonSocial, compania.TxNombreComercial, compania.UiCompania, compania.TxDireccionMatriz
                                             , compania.TxContribuyenteEspecial, compania.TxObligadoContabilidad, compania.TxAgenteRetencion, compania.TxRegimenMicroempresas, compania.TxContribuyenteRimpe, compania.TipoAmbiente, compania.LogoCompania, compania.CiEstado, ref codigoRetorno, ref mensajeRetorno);

                if (codigoRetorno.Equals(0))
                {
                    if (dsresp.Tables[0].Rows.Count > 0)
                    {
                        foreach(DataRow row in dsresp.Tables[0].Rows)
                        {
                            mensajeRetorno = row["MensajeRetorno"].ToString().Trim().ToUpper();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                mensajeRetorno = ex.Message;
            }
            return listaCompania;
        }

        public void EliminarCompanias(Compania compania, ref int codigoRetorno, ref string mensajeRetorno)
        {
            try
            {
                new CompaniaAD().MantenimientoCompania(6, 0, "", compania.TxRazonSocial, "", "", ""
                                                        , "", "","","","", "", null, compania.CiEstado, ref codigoRetorno, ref mensajeRetorno);
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                mensajeRetorno = ex.Message;
            }
        }

        public List<Sucursal> ConsultaSucursal(Sucursal sucursal, ref int codigoRetorno, ref string mensajeRetorno)
        {
            List<Sucursal> listaSurcusal = new List<Sucursal>();

            try
            {
                DataSet dsResultado = new CompaniaAD().MantenimientoSucursal(4, sucursal.ciCompania, "", "", "",
                                                                             ref codigoRetorno, ref mensajeRetorno);

                if (codigoRetorno.Equals(0))
                {
                    foreach (DataRow lista in dsResultado.Tables[0].Rows)
                    {
                        Sucursal _sucursal = new Sucursal()
                        {
                            ciCompania = int.Parse(lista["ciCompania"].ToString()),
                            ciSucursal = lista["ciSucursal"].ToString(),
                            direccion = lista["txDireccion"].ToString(),
                            ciEstado = lista["ciEstado"].ToString(),
                        };
                        listaSurcusal.Add(_sucursal);
                    }
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                mensajeRetorno = ex.Message;
            }
            return listaSurcusal;
        }

        public void EliminarSucursal(Sucursal sucursal, ref int codigoRetorno, ref string mensajeRetorno)
        {
            try
            {
                new CompaniaAD().MantenimientoSucursal(4, sucursal.ciCompania, "", "", "", ref codigoRetorno, ref mensajeRetorno);
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                mensajeRetorno = ex.Message;
            }
        }

        public DataSet InsertarSucursal(Sucursal sucursal, ref int codigoRetorno, ref string mensajeRetorno)
        {
            DataSet dsRespuesta = null;
            try
            {
                dsRespuesta = new CompaniaAD().MantenimientoSucursal(5, sucursal.ciCompania, sucursal.ciSucursal, sucursal.direccion, sucursal.ciEstado,
                                                       ref codigoRetorno, ref mensajeRetorno);
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                mensajeRetorno = ex.Message;
            }

            return dsRespuesta;
        }

        public UrlSri ConsultarUrlSri(int ciTipoAmbiente, ref int codigoRetorno, ref string mensajeRetorno)
        {
            UrlSri urlSri = new UrlSri();
            try
            {               
                DataTable dtResultado = new UrlSriAC().ObtenerURLSRI("C", "", "", "", ciTipoAmbiente.ToString(),
                                                                     ref codigoRetorno, ref mensajeRetorno);

                if (codigoRetorno.Equals(0))
                {
                    urlSri.urlRecepcion = dtResultado.Rows[0]["txUrlRecepcion"].ToString().Trim();
                    urlSri.urlAutorizacion = dtResultado.Rows[0]["txUrlAutorizacion"].ToString().Trim();
                }

            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                mensajeRetorno = ex.Message;
            }
            return urlSri;
        }

        public void InsertarUrlSri(int ciTipoAmbiente, UrlSri urlSri, ref int codigoRetorno, ref string mensajeRetorno)
        {
            try
            {
                DataTable dtResultado = new UrlSriAC().ObtenerURLSRI("P", urlSri.urlRecepcion, urlSri.urlAutorizacion, "", ciTipoAmbiente.ToString(),
                                                                     ref codigoRetorno, ref mensajeRetorno);

                if (codigoRetorno.Equals(0))
                {
                    codigoRetorno = int.Parse(dtResultado.Rows[0]["codigoRetorno"].ToString().Trim());
                    mensajeRetorno = dtResultado.Rows[0]["mensajeRetorno"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                mensajeRetorno = ex.Message;
            }
        }


        public List<Certificado> ConsultarCertificado(Certificado certificado, ref int codigoRetorno, ref string mensajeRetorno)
        {
            List<Certificado> listaCertificados = new List<Certificado>();

            try
            {
                DateTime fechaActual = DateTime.Now;
                DataSet dsResultado = new CompaniasCertificadosAD().MantenimientoCertificado(4, certificado.CiCompania, 0, "",
                  "", "", "", fechaActual, fechaActual, "", ref codigoRetorno, ref mensajeRetorno);

                if (codigoRetorno.Equals(0))
                {
                    foreach (DataRow lista in dsResultado.Tables[0].Rows)
                    {
                        Certificado _certificado = new Certificado()
                        {
                            FcDesde = DateTime.Parse(lista["fechaDesde"].ToString()),
                            FcHasta = DateTime.Parse(lista["fechaHasta"].ToString()),
                            razonSocial = lista["razonSocial"].ToString(),
                            ruc = lista["ruc"].ToString(),
                            estado = lista["estado"].ToString()
                        };
                        listaCertificados.Add(_certificado);
                    }
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                mensajeRetorno = ex.Message;
            }

            return listaCertificados;
        }

        public void InsertarCertificado(Certificado certificado, ref int codigoRetorno, ref string mensajeRetorno)
        {
            try
            {
                string obCertificado = BitConverter.ToString(certificado.ObCertificado);

                DataSet dsResultado = new CompaniasCertificadosAD().MantenimientoCertificado(1, certificado.CiCompania, 0, certificado.UiSemilla.ToString(),
                   certificado.TxClave, certificado.txKey, obCertificado, certificado.FcDesde, certificado.FcHasta, "A", ref codigoRetorno,
                   ref mensajeRetorno);

                if (codigoRetorno.Equals(0))
                {
                    codigoRetorno = int.Parse(dsResultado.Tables[0].Rows[0]["codigoRetorno"].ToString());
                    mensajeRetorno = dsResultado.Tables[0].Rows[0]["mensajeRetorno"].ToString();
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                mensajeRetorno = ex.Message;
            }
        }

        public void InsertarHorasProceso()
        {

        }

        public void guardarHorasServicios(List<TiempoServicio> tiempoServicios, string tipoServicio, ref int codigoRetorno,
            ref string mensajeRetorno)
        {
            try
            {
                string horasServicios = string.Empty;
                int contador = tiempoServicios.Count;
                int contadorLista = 0;
                bool crearArchivo = false;

                if (!Directory.Exists(CatalogoViaDoc.rutaHorasServicios))
                {
                    crearArchivo = true;
                    Directory.CreateDirectory(CatalogoViaDoc.rutaHorasServicios);
                }


                foreach (var lista in tiempoServicios)
                {
                    horasServicios += lista.horaInicio + "-" + lista.horaFin + ";";
                    contadorLista++;

                    if (!contadorLista.Equals(contador))
                        horasServicios += Environment.NewLine;
                }

                if (tipoServicio.Equals("FIRMA"))
                    File.WriteAllText(CatalogoViaDoc.rutaHorasServicios, horasServicios);
                if (tipoServicio.Equals("AUTORIZACION"))
                    File.WriteAllText(CatalogoViaDoc.rutaHorasServicios, horasServicios);
                if (tipoServicio.Equals("CORREO"))
                    File.WriteAllText(CatalogoViaDoc.rutaHorasServicios, horasServicios);
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                mensajeRetorno = ex.Message;
            }
        }

        public List<TiempoServicio> ConsultarHorasProceso(TiempoServicio tiempoServicio, ref int codigoRetorno, ref string mensajeRetorno)
        {
            List<TiempoServicio> listaTiempoServicio = new List<TiempoServicio>();
            string horaServicio = string.Empty;
            string descripcionServicio = string.Empty;
            try
            {
                StreamReader file = null;
                bool crearArchivo = false;
                if (!Directory.Exists(CatalogoViaDoc.rutaHorasServicios))
                {
                    crearArchivo = true;
                    Directory.CreateDirectory(CatalogoViaDoc.rutaHorasServicios);
                }

                if (tiempoServicio.tipoServicio.Equals("FIRMA"))
                {
                    descripcionServicio = "Servicio Firma";
                    if (!crearArchivo)
                    {
                        if (File.Exists(CatalogoViaDoc.rutaHorasServicios + "HoraProcesoFirma.txt") == false)
                            System.IO.File.WriteAllText(CatalogoViaDoc.rutaHorasServicios + "HoraProcesoFirma.txt", "");

                        file = new StreamReader(CatalogoViaDoc.rutaHorasServicios + "HoraProcesoFirma.txt");
                    }
                       
                }
                if (tiempoServicio.tipoServicio.Equals("AUTORIZACION"))
                {
                    descripcionServicio = "Servicio Autorización";
                    if (!crearArchivo)
                    {
                        if (File.Exists(CatalogoViaDoc.rutaHorasServicios + "HoraProcesoAutorizacion.txt") == false)
                            System.IO.File.WriteAllText(CatalogoViaDoc.rutaHorasServicios + "HoraProcesoAutorizacion.txt", "");

                        file = new StreamReader(CatalogoViaDoc.rutaHorasServicios + "HoraProcesoAutorizacion.txt");
                    }
                }
                if (tiempoServicio.tipoServicio.Equals("CORREO"))
                {
                    descripcionServicio = "Servicio Notificación";
                    if (!crearArchivo)
                    {
                        if (File.Exists(CatalogoViaDoc.rutaHorasServicios + "HoraProcesoNotificacion.txt") == false)
                            System.IO.File.WriteAllText(CatalogoViaDoc.rutaHorasServicios + "HoraProcesoNotificacion.txt", "");

                        file = new StreamReader(CatalogoViaDoc.rutaHorasServicios + "HoraProcesoNotificacion.txt");
                    }
                }

                if (!crearArchivo)
                {
                    var line = "";                    
                    char[] delimiter = new char[] { '\t' };
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] _horas = line.Split('-');

                        TiempoServicio _tiempo = new TiempoServicio()
                        {
                            tipoServicio = descripcionServicio,
                            horaInicio = _horas[0],
                            horaFin = _horas[1]
                        };
                        listaTiempoServicio.Add(_tiempo);
                    }
                    file.Close();
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                mensajeRetorno = ex.Message;
            }
            return listaTiempoServicio;
        }
    }
}
