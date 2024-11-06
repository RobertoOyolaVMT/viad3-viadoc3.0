using System;
using System.Collections.Generic;
using System.Linq;
using ViaDoc.EntidadNegocios;
using ViaDoc.EntidadNegocios.portalWeb;
using ViaDoc.LogicaNegocios.portalweb;

namespace ViaDoc.WebApp.Models
{
    public class MetodosConfiguracion
    {
        ProcesoConfiguracion _procesoConfiguracion = new ProcesoConfiguracion();

        public List<MParametro> ConsultaParametrosEmpresa(int Opcion, ref int codigoRetorno, ref string mensajeRetorno)
        {

            List<MParametro> data = new List<MParametro>();
            List<Parametros> resultadoParametros = _procesoConfiguracion.ConsultaParametros(Opcion, ref codigoRetorno, ref mensajeRetorno);

            if (codigoRetorno.Equals(0))
            {
                foreach (var lista in resultadoParametros)
                {
                    MParametro mParametro = new MParametro()
                    {
                        idRegistro = lista.idRegistro,
                        idTipoDocumento = lista.idTipoDocumento,
                        descripcion = lista.descripcion,
                        cantidadFirma = lista.cantidadFirma,
                        cantidadCorreo = lista.cantidadCorreo,
                        cantidadAutorizacion = lista.cantidadAutorizacion,
                        reprocesoAutorizacion = lista.reprocesoAutorizacion,
                        reprocesoCorreo = lista.reprocesoCorreo,
                        reprocesoFirma = lista.reprocesoFirma,
                        estado = lista.estado,
                        idCompania = lista.idCompania,
                        nombreComercial = lista.nombreComercial
                    };
                    data.Add(mParametro);
                }
            }

            return data;
        }


        public void IngresosParametrosEmpresa(MParametro mParametro, ref int codigoRetorno, ref string mensajeRetorno)
        {
            Parametros parametros = new Parametros();
            parametros.idRegistro = mParametro.idRegistro;
            parametros.idCompania = mParametro.idCompania;
            parametros.idTipoDocumento = mParametro.idTipoDocumento;
            parametros.cantidadFirma = mParametro.cantidadFirma;
            parametros.cantidadCorreo = mParametro.cantidadCorreo;
            parametros.cantidadAutorizacion = mParametro.cantidadAutorizacion;
            parametros.reprocesoFirma = mParametro.reprocesoFirma;
            parametros.reprocesoCorreo = mParametro.reprocesoCorreo;
            parametros.reprocesoAutorizacion = mParametro.reprocesoAutorizacion;

            _procesoConfiguracion.InsertarParametros(parametros, ref codigoRetorno, ref mensajeRetorno);
        }


        public void EliminarParametrosEmpresa(int idRegistro, ref int codigoRetorno, ref string mensajeRetorno)
        {
            Parametros parametros = new Parametros();
            parametros.idRegistro = idRegistro;
            _procesoConfiguracion.EliminarParametros(parametros, ref codigoRetorno, ref mensajeRetorno);
        }

        public List<HoraNotificacion> ConsultaParametrosHorasNotificacion(ref int codigoRetorno, ref string mensajeRetorno)
        {

            List<HoraNotificacion> data = new List<HoraNotificacion>();
            var resultadoParametros = _procesoConfiguracion.ConsultaParametrosHorasNotificacion(ref codigoRetorno, ref mensajeRetorno);

            if (codigoRetorno.Equals(0))
            {
                foreach (var lista in resultadoParametros)
                {
                    HoraNotificacion mParametro = new HoraNotificacion()
                    {
                        idRegistro = lista.idRegistro,
                        HoraInicio = lista.HoraInicio,
                        HoraFin = lista.HoraFin,
                    };
                    data.Add(mParametro);
                }
            }

            if (data.Count == 0)
            {
                codigoRetorno = 1;
                mensajeRetorno = "No existe registro parametrizados";
            }

            return data;
        }


        public void IngresosParametrosHorasNotificacion(List<HoraNotificacion> mParametro, ref int codigoRetorno, ref string mensajeRetorno)
        {
            _procesoConfiguracion.InsertarNotificacionHoras(mParametro, ref codigoRetorno, ref mensajeRetorno);
        }


        public List<HoraNotificacion> ConsultaParametrosHorasReproceso(int tipoProceso, ref int codigoRetorno, ref string mensajeRetorno)
        {

            List<HoraNotificacion> data = new List<HoraNotificacion>();
            var resultadoParametros = _procesoConfiguracion.ConsultaParametrosHorasReproceso(tipoProceso, ref codigoRetorno, ref mensajeRetorno);

            if (codigoRetorno.Equals(0))
            {
                foreach (var lista in resultadoParametros)
                {
                    HoraNotificacion mParametro = new HoraNotificacion()
                    {
                        idRegistro = lista.idRegistro,
                        HoraInicio = lista.HoraInicio,
                        HoraFin = lista.HoraFin,
                    };
                    data.Add(mParametro);
                }
            }

            if (data.Count == 0)
            {
                codigoRetorno = 1;
                mensajeRetorno = "No existe registro parametrizados";
            }

            return data;
        }

        public void IngresosParametrosHorasReproceso(int tipoProceso, List<HoraNotificacion> mParametro, ref int codigoRetorno, ref string mensajeRetorno)
        {
            _procesoConfiguracion.InsertarHorasReproceso(tipoProceso, mParametro, ref codigoRetorno, ref mensajeRetorno);
        }
    }
}