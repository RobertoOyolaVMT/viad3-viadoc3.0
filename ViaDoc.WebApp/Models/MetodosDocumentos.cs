using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViaDoc.EntidadNegocios.portalWeb;
using ViaDoc.LogicaNegocios.portalweb;

namespace ViaDoc.WebApp.Models
{
    public class MetodosDocumentos
    {
        ProcesoDocumento _procesoDocumentos = new ProcesoDocumento();

        public List<MDocumentoTodos> ConsultaTodosDocumentos(string razonSocial, string claveAcceso, string numeroAutorizacion,
                                                             string numeroDocumento, string identificacionComprador, 
                                                             string tipoDocumento, string fechaDesde, string fechaHasta,
                                                             string razonSocialComprador, string filtroFechaDH, 
                                                             ref int codigoRetorno, ref string mensajeRetorno)
        {
            List<MDocumentoTodos> listaDocumentos = new List<MDocumentoTodos>();
            Documento requestDocumento = new Documento();
            requestDocumento.razonSocial = razonSocial;
            requestDocumento.claveAcceso = claveAcceso;
            requestDocumento.numeroAutorizacion = numeroAutorizacion;
            requestDocumento.NumeroDocumento = numeroDocumento;
            requestDocumento.identificacionComprador = identificacionComprador;
            requestDocumento.tipoDocumento = tipoDocumento;
            requestDocumento.fechaDesde = fechaDesde;
            requestDocumento.fechaHasta = fechaHasta;
            requestDocumento.razonSocialComprador = razonSocialComprador;
            requestDocumento.filtroFechaDH = filtroFechaDH;

            List<Documento> listaResultado = _procesoDocumentos.ConsultarDocumentosTodos(requestDocumento,
                                                                                         ref codigoRetorno, 
                                                                                         ref mensajeRetorno);

            if (codigoRetorno.Equals(0))
            {
                foreach(var lista in listaDocumentos)
                {
                    MDocumentoTodos mDocumento = new MDocumentoTodos()
                    {
                        claveAcceso = lista.claveAcceso,
                        razonSocial = lista.razonSocial,
                        razonSocialComprador = lista.razonSocialComprador,
                        identificacionComprador = lista.identificacionComprador,
                        NumeroDocumento = lista.NumeroDocumento,
                        documentoAutorizado = lista.documentoAutorizado,
                        fechaHoraAutorizacion = lista.fechaHoraAutorizacion,
                        descripcion = lista.descripcion,
                        descripcionEmision = lista.descripcionEmision,
                        descripcionEstado = lista.descripcionEstado,
                        tipoDocumento = lista.tipoDocumento,
                        idEstado = lista.idEstado,
                        idCompania = lista.idCompania
                    };
                    listaDocumentos.Add(mDocumento);
                }
            }
            return listaDocumentos;
        }



    }
}