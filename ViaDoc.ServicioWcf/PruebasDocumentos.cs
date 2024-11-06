using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using ViaDoc.EntidadNegocios.factura;
using ViaDoc.ServicioWcf.modelo;

namespace ViaDoc.ServicioWcf
{
    public class PruebasDocumentos
    {

        public string FacturaRequest()
        {
            string _requestFactura = @"{'procesaFactura': true,
                                 'facturaRequest': [
                                      {
                                          'compania':1,
                                          'tipoEmision':1,
                                          'tipoDocumento': '01',
                                          'establecimiento': '001', 
                                          'puntoEmision':'001',
                                          'secuencial':'000000104',
                                          'claveAcceso':'',
                                          'fechaEmision':'05/02/2020',
                                          'tipoIdentificacionProvedor':'04',
                                          'razonSocialProvedor':'MUEBLERIA DORMIHOGAR',
                                          'identificacionProvedor':'0942015215',
                                          'totalSinImpuestos': 6787.00,
                                          'totalDescuento': 0.00,
                                          'propina': 0.00,
                                          'importeTotal': 6787.00,
                                          'moneda': 'DOLAR',
                                          'Email':'josephpluas@gmail.com',
                                          'ContigenciaDet': 0,
                                          'estado': 'A',
                                          'ambiente': '1',
                                          'ruc':'0943434556001',
                                          'codigoNumerico': 408,
                                          'DetalleFactura' :[
                                                    {
                                                        'codigoPrincipal':'1',
                                                        'codigoAuxiliar': '1',
                                                        'descripcion':'01-COMPUTADORA DELL',
                                                        'cantidad': 1,
                                                        'precioUnitario': 6787.00,
                                                        'descuento': 0.00,
                                                        'precioTotalSinImpuesto': 6787.00,
                                                        'detalleImpuesto':[
                                                                {
                                                                   'codigoPrincipal':'1',
                                                                   'codigo': '2',
                                                                   'codigoPorcentaje':'0',
                                                                   'tarifa': '1', 
                                                                   'baseImponible': 6787.00,
                                                                   'valor': 0
                                                            }
                                                        ],
                                                        'detalleAdicional':[
                                                                {
                                                                    'codigoPrincipal':'01',
                                                                    'nombre':'SII',
                                                                    'valor': 0.00
                                                            }
                                                        ]
                                                    }
                                            ],
                                            'formaPago':[
                                                  {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 },
                                                 {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 }
                                          ],
                                         'totalImpuesto': [
                                           {
                                               'codigo':'2',
                                               'codigoPorcentaje':'2',
                                               'tarifa':'0',
                                               'descuentoAdicional': 0,
                                               'baseImponible': 6787.00,
                                               'valor': 0.00
                                            }
                                         ],
                                         'infoAdicional':[
                                            {
                                                'nombre':'CORREO',
                                                'valor':'josephpluas@gmail.com'
                                            },
                                            {
                                                'nombre':'TELEFONO',
                                                'valor':'096257272'
                                            },
                                            {
                                                'nombre':'VENDEDOR',
                                                'valor':'Joseph Pluas'
                                            }
                                         ],
                                    },
                                    {
                                          'compania':1,
                                          'tipoEmision':1,
                                          'tipoDocumento': '01',
                                          'establecimiento': '001', 
                                          'puntoEmision':'001',
                                          'secuencial':'000000105',
                                          'claveAcceso':'',
                                          'fechaEmision':'05/02/2020',
                                          'tipoIdentificacionComprador':'04',
                                          'guiaRemision':'',
                                          'razonSocialComprador':'MUEBLERIA DORMIHOGAR',
                                          'identificacionComprador':'0942015215',
                                          'totalSinImpuestos': 6787.00,
                                          'totalDescuento': 0.00,
                                          'propina': 0.00,
                                          'importeTotal': 6787.00,
                                          'moneda': 'DOLAR',
                                          'Email':'josephpluas@gmail.com',
                                          'ContigenciaDet': 0,
                                          'estado': 'A',
                                          'ambiente': '1',
                                          'ruc':'0943434556001',
                                          'codigoNumerico': 408,
                                          'DetalleFactura' :[
                                                    {
                                                        'codigoPrincipal':'1',
                                                        'codigoAuxiliar': '1',
                                                        'descripcion':'01-COMPUTADORA DELL',
                                                        'cantidad': 1,
                                                        'precioUnitario': 6787.00,
                                                        'descuento': 0.00,
                                                        'precioTotalSinImpuesto': 6787.00,
                                                        'detalleImpuesto':[
                                                                {
                                                                   'codigoPrincipal':'1',
                                                                   'codigo': '2',
                                                                   'codigoPorcentaje':'0',
                                                                   'tarifa': '1', 
                                                                   'baseImponible': 6787.00,
                                                                   'valor': 0
                                                            }
                                                        ],
                                                        'detalleAdicional':[
                                                                {
                                                                    'codigoPrincipal':'01',
                                                                    'nombre':'SII',
                                                                    'valor': 0.00
                                                            }
                                                        ]
                                                    }
                                            ],
                                            'formaPago':[
                                                  {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 },
                                                 {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 }
                                          ],
                                         'totalImpuesto': [
                                           {
                                               'codigo':'2',
                                               'codigoPorcentaje':'2',
                                               'tarifa':'0',
                                               'descuentoAdicional': 0,
                                               'baseImponible': 6787.00,
                                               'valor': 0.00
                                            }
                                         ],
                                         'infoAdicional':[
                                            {
                                                'nombre':'CORREO',
                                                'valor':'josephpluas@gmail.com'
                                            },
                                            {
                                                'nombre':'TELEFONO',
                                                'valor':'096257272'
                                            },
                                            {
                                                'nombre':'VENDEDOR',
                                                'valor':'Joseph Pluas'
                                            }
                                         ],
                                    },
                                    {
                                          'compania':1,
                                          'tipoEmision':1,
                                          'tipoDocumento': '01',
                                          'establecimiento': '001', 
                                          'puntoEmision':'001',
                                          'secuencial':'000000106',
                                          'claveAcceso':'',
                                          'fechaEmision':'05/02/2020',
                                          'tipoIdentificacionComprador':'04',
                                          'guiaRemision':'',
                                          'razonSocialComprador':'MUEBLERIA DORMIHOGAR',
                                          'identificacionComprador':'0942015215',
                                          'totalSinImpuestos': 6787.00,
                                          'totalDescuento': 0.00,
                                          'propina': 0.00,
                                          'importeTotal': 6787.00,
                                          'moneda': 'DOLAR',
                                          'Email':'josephpluas@gmail.com',
                                          'ContigenciaDet': 0,
                                          'estado': 'A',
                                          'ambiente': '1',
                                          'ruc':'0943434556001',
                                          'codigoNumerico': 408,
                                          'DetalleFactura' :[
                                                    {
                                                        'codigoPrincipal':'1',
                                                        'codigoAuxiliar': '1',
                                                        'descripcion':'01-COMPUTADORA DELL',
                                                        'cantidad': 1,
                                                        'precioUnitario': 6787.00,
                                                        'descuento': 0.00,
                                                        'precioTotalSinImpuesto': 6787.00,
                                                        'detalleImpuesto':[
                                                                {
                                                                   'codigoPrincipal':'1',
                                                                   'codigo': '2',
                                                                   'codigoPorcentaje':'0',
                                                                   'tarifa': '1', 
                                                                   'baseImponible': 6787.00,
                                                                   'valor': 0
                                                            }
                                                        ],
                                                        'detalleAdicional':[
                                                                {
                                                                    'codigoPrincipal':'01',
                                                                    'nombre':'SII',
                                                                    'valor': 0.00
                                                            }
                                                        ]
                                                    }
                                            ],
                                            'formaPago':[
                                                  {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 },
                                                 {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 }
                                          ],
                                         'totalImpuesto': [
                                           {
                                               'codigo':'2',
                                               'codigoPorcentaje':'2',
                                               'tarifa':'0',
                                               'descuentoAdicional': 0,
                                               'baseImponible': 6787.00,
                                               'valor': 0.00
                                            }
                                         ],
                                         'infoAdicional':[
                                            {
                                                'nombre':'CORREO',
                                                'valor':'josephpluas@gmail.com'
                                            },
                                            {
                                                'nombre':'TELEFONO',
                                                'valor':'096257272'
                                            },
                                            {
                                                'nombre':'VENDEDOR',
                                                'valor':'Joseph Pluas'
                                            }
                                         ],
                                    },
                                    {
                                          'compania':1,
                                          'tipoEmision':1,
                                          'tipoDocumento': '01',
                                          'establecimiento': '001', 
                                          'puntoEmision':'001',
                                          'secuencial':'000000107',
                                          'claveAcceso':'',
                                          'fechaEmision':'05/02/2020',
                                          'tipoIdentificacionComprador':'04',
                                          'guiaRemision':'',
                                          'razonSocialComprador':'MUEBLERIA DORMIHOGAR',
                                          'identificacionComprador':'0942015215',
                                          'totalSinImpuestos': 6787.00,
                                          'totalDescuento': 0.00,
                                          'propina': 0.00,
                                          'importeTotal': 6787.00,
                                          'moneda': 'DOLAR',
                                          'Email':'josephpluas@gmail.com',
                                          'ContigenciaDet': 0,
                                          'estado': 'A',
                                          'ambiente': '1',
                                          'ruc':'0943434556001',
                                          'codigoNumerico': 408,
                                          'DetalleFactura' :[
                                                    {
                                                        'codigoPrincipal':'1',
                                                        'codigoAuxiliar': '1',
                                                        'descripcion':'01-COMPUTADORA DELL',
                                                        'cantidad': 1,
                                                        'precioUnitario': 6787.00,
                                                        'descuento': 0.00,
                                                        'precioTotalSinImpuesto': 6787.00,
                                                        'detalleImpuesto':[
                                                                {
                                                                   'codigoPrincipal':'1',
                                                                   'codigo': '2',
                                                                   'codigoPorcentaje':'0',
                                                                   'tarifa': '1', 
                                                                   'baseImponible': 6787.00,
                                                                   'valor': 0
                                                            }
                                                        ],
                                                        'detalleAdicional':[
                                                                {
                                                                    'codigoPrincipal':'01',
                                                                    'nombre':'SII',
                                                                    'valor': 0.00
                                                            }
                                                        ]
                                                    }
                                            ],
                                            'formaPago':[
                                                  {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 },
                                                 {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 }
                                          ],
                                         'totalImpuesto': [
                                           {
                                               'codigo':'2',
                                               'codigoPorcentaje':'2',
                                               'tarifa':'0',
                                               'descuentoAdicional': 0,
                                               'baseImponible': 6787.00,
                                               'valor': 0.00
                                            }
                                         ],
                                         'infoAdicional':[
                                            {
                                                'nombre':'CORREO',
                                                'valor':'josephpluas@gmail.com'
                                            },
                                            {
                                                'nombre':'TELEFONO',
                                                'valor':'096257272'
                                            },
                                            {
                                                'nombre':'VENDEDOR',
                                                'valor':'Joseph Pluas'
                                            }
                                         ],
                                    },
                                    {
                                          'compania':1,
                                          'tipoEmision':1,
                                          'tipoDocumento': '01',
                                          'establecimiento': '001', 
                                          'puntoEmision':'001',
                                          'secuencial':'000000108',
                                          'claveAcceso':'',
                                          'fechaEmision':'05/02/2020',
                                          'tipoIdentificacionComprador':'04',
                                          'guiaRemision':'',
                                          'razonSocialComprador':'MUEBLERIA DORMIHOGAR',
                                          'identificacionComprador':'0942015215',
                                          'totalSinImpuestos': 6787.00,
                                          'totalDescuento': 0.00,
                                          'propina': 0.00,
                                          'importeTotal': 6787.00,
                                          'moneda': 'DOLAR',
                                          'Email':'josephpluas@gmail.com',
                                          'ContigenciaDet': 0,
                                          'estado': 'A',
                                          'ambiente': '1',
                                          'ruc':'0943434556001',
                                          'codigoNumerico': 408,
                                          'DetalleFactura' :[
                                                    {
                                                        'codigoPrincipal':'1',
                                                        'codigoAuxiliar': '1',
                                                        'descripcion':'01-COMPUTADORA DELL',
                                                        'cantidad': 1,
                                                        'precioUnitario': 6787.00,
                                                        'descuento': 0.00,
                                                        'precioTotalSinImpuesto': 6787.00,
                                                        'detalleImpuesto':[
                                                                {
                                                                   'codigoPrincipal':'1',
                                                                   'codigo': '2',
                                                                   'codigoPorcentaje':'0',
                                                                   'tarifa': '1', 
                                                                   'baseImponible': 6787.00,
                                                                   'valor': 0
                                                            }
                                                        ],
                                                        'detalleAdicional':[
                                                                {
                                                                    'codigoPrincipal':'01',
                                                                    'nombre':'SII',
                                                                    'valor': 0.00
                                                            }
                                                        ]
                                                    }
                                            ],
                                            'formaPago':[
                                                  {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 },
                                                 {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 }
                                          ],
                                         'totalImpuesto': [
                                           {
                                               'codigo':'2',
                                               'codigoPorcentaje':'2',
                                               'tarifa':'0',
                                               'descuentoAdicional': 0,
                                               'baseImponible': 6787.00,
                                               'valor': 0.00
                                            }
                                         ],
                                         'infoAdicional':[
                                            {
                                                'nombre':'CORREO',
                                                'valor':'josephpluas@gmail.com'
                                            },
                                            {
                                                'nombre':'TELEFONO',
                                                'valor':'096257272'
                                            },
                                            {
                                                'nombre':'VENDEDOR',
                                                'valor':'Joseph Pluas'
                                            }
                                         ],
                                    },
                                    {
                                          'compania':1,
                                          'tipoEmision':1,
                                          'tipoDocumento': '01',
                                          'establecimiento': '001', 
                                          'puntoEmision':'001',
                                          'secuencial':'000000109',
                                          'claveAcceso':'',
                                          'fechaEmision':'05/02/2020',
                                          'tipoIdentificacionComprador':'04',
                                          'guiaRemision':'',
                                          'razonSocialComprador':'MUEBLERIA DORMIHOGAR',
                                          'identificacionComprador':'0942015215',
                                          'totalSinImpuestos': 6787.00,
                                          'totalDescuento': 0.00,
                                          'propina': 0.00,
                                          'importeTotal': 6787.00,
                                          'moneda': 'DOLAR',
                                          'Email':'josephpluas@gmail.com',
                                          'ContigenciaDet': 0,
                                          'estado': 'A',
                                          'ambiente': '1',
                                          'ruc':'0943434556001',
                                          'codigoNumerico': 408,
                                          'DetalleFactura' :[
                                                    {
                                                        'codigoPrincipal':'1',
                                                        'codigoAuxiliar': '1',
                                                        'descripcion':'01-COMPUTADORA DELL',
                                                        'cantidad': 1,
                                                        'precioUnitario': 6787.00,
                                                        'descuento': 0.00,
                                                        'precioTotalSinImpuesto': 6787.00,
                                                        'detalleImpuesto':[
                                                                {
                                                                   'codigoPrincipal':'1',
                                                                   'codigo': '2',
                                                                   'codigoPorcentaje':'0',
                                                                   'tarifa': '1', 
                                                                   'baseImponible': 6787.00,
                                                                   'valor': 0
                                                            }
                                                        ],
                                                        'detalleAdicional':[
                                                                {
                                                                    'codigoPrincipal':'01',
                                                                    'nombre':'SII',
                                                                    'valor': 0.00
                                                            }
                                                        ]
                                                    }
                                            ],
                                            'formaPago':[
                                                  {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 },
                                                 {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 }
                                          ],
                                         'totalImpuesto': [
                                           {
                                               'codigo':'2',
                                               'codigoPorcentaje':'2',
                                               'tarifa':'0',
                                               'descuentoAdicional': 0,
                                               'baseImponible': 6787.00,
                                               'valor': 0.00
                                            }
                                         ],
                                         'infoAdicional':[
                                            {
                                                'nombre':'CORREO',
                                                'valor':'josephpluas@gmail.com'
                                            },
                                            {
                                                'nombre':'TELEFONO',
                                                'valor':'096257272'
                                            },
                                            {
                                                'nombre':'VENDEDOR',
                                                'valor':'Joseph Pluas'
                                            }
                                         ],
                                    },
                                    {
                                          'compania':1,
                                          'tipoEmision':1,
                                          'tipoDocumento': '01',
                                          'establecimiento': '001', 
                                          'puntoEmision':'001',
                                          'secuencial':'000000110',
                                          'claveAcceso':'',
                                          'fechaEmision':'05/02/2020',
                                          'tipoIdentificacionComprador':'04',
                                          'guiaRemision':'',
                                          'razonSocialComprador':'MUEBLERIA DORMIHOGAR',
                                          'identificacionComprador':'0942015215',
                                          'totalSinImpuestos': 6787.00,
                                          'totalDescuento': 0.00,
                                          'propina': 0.00,
                                          'importeTotal': 6787.00,
                                          'moneda': 'DOLAR',
                                          'Email':'josephpluas@gmail.com',
                                          'ContigenciaDet': 0,
                                          'estado': 'A',
                                          'ambiente': '1',
                                          'ruc':'0943434556001',
                                          'codigoNumerico': 408,
                                          'DetalleFactura' :[
                                                    {
                                                        'codigoPrincipal':'1',
                                                        'codigoAuxiliar': '1',
                                                        'descripcion':'01-COMPUTADORA DELL',
                                                        'cantidad': 1,
                                                        'precioUnitario': 6787.00,
                                                        'descuento': 0.00,
                                                        'precioTotalSinImpuesto': 6787.00,
                                                        'detalleImpuesto':[
                                                                {
                                                                   'codigoPrincipal':'1',
                                                                   'codigo': '2',
                                                                   'codigoPorcentaje':'0',
                                                                   'tarifa': '1', 
                                                                   'baseImponible': 6787.00,
                                                                   'valor': 0
                                                            }
                                                        ],
                                                        'detalleAdicional':[
                                                                {
                                                                    'codigoPrincipal':'01',
                                                                    'nombre':'SII',
                                                                    'valor': 0.00
                                                            }
                                                        ]
                                                    }
                                            ],
                                            'formaPago':[
                                                  {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 },
                                                 {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 }
                                          ],
                                         'totalImpuesto': [
                                           {
                                               'codigo':'2',
                                               'codigoPorcentaje':'2',
                                               'tarifa':'0',
                                               'descuentoAdicional': 0,
                                               'baseImponible': 6787.00,
                                               'valor': 0.00
                                            }
                                         ],
                                         'infoAdicional':[
                                            {
                                                'nombre':'CORREO',
                                                'valor':'josephpluas@gmail.com'
                                            },
                                            {
                                                'nombre':'TELEFONO',
                                                'valor':'096257272'
                                            },
                                            {
                                                'nombre':'VENDEDOR',
                                                'valor':'Joseph Pluas'
                                            }
                                         ],
                                    },
                                    {
                                          'compania':1,
                                          'tipoEmision':1,
                                          'tipoDocumento': '01',
                                          'establecimiento': '001', 
                                          'puntoEmision':'001',
                                          'secuencial':'000000111',
                                          'claveAcceso':'',
                                          'fechaEmision':'05/02/2020',
                                          'tipoIdentificacionComprador':'04',
                                          'guiaRemision':'',
                                          'razonSocialComprador':'MUEBLERIA DORMIHOGAR',
                                          'identificacionComprador':'0942015215',
                                          'totalSinImpuestos': 6787.00,
                                          'totalDescuento': 0.00,
                                          'propina': 0.00,
                                          'importeTotal': 6787.00,
                                          'moneda': 'DOLAR',
                                          'Email':'josephpluas@gmail.com',
                                          'ContigenciaDet': 0,
                                          'estado': 'A',
                                          'ambiente': '1',
                                          'ruc':'0943434556001',
                                          'codigoNumerico': 408,
                                          'DetalleFactura' :[
                                                    {
                                                        'codigoPrincipal':'1',
                                                        'codigoAuxiliar': '1',
                                                        'descripcion':'01-COMPUTADORA DELL',
                                                        'cantidad': 1,
                                                        'precioUnitario': 6787.00,
                                                        'descuento': 0.00,
                                                        'precioTotalSinImpuesto': 6787.00,
                                                        'detalleImpuesto':[
                                                                {
                                                                   'codigoPrincipal':'1',
                                                                   'codigo': '2',
                                                                   'codigoPorcentaje':'0',
                                                                   'tarifa': '1', 
                                                                   'baseImponible': 6787.00,
                                                                   'valor': 0
                                                            }
                                                        ],
                                                        'detalleAdicional':[
                                                                {
                                                                    'codigoPrincipal':'01',
                                                                    'nombre':'SII',
                                                                    'valor': 0.00
                                                            }
                                                        ]
                                                    }
                                            ],
                                            'formaPago':[
                                                  {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 },
                                                 {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 }
                                          ],
                                         'totalImpuesto': [
                                           {
                                               'codigo':'2',
                                               'codigoPorcentaje':'2',
                                               'tarifa':'0',
                                               'descuentoAdicional': 0,
                                               'baseImponible': 6787.00,
                                               'valor': 0.00
                                            }
                                         ],
                                         'infoAdicional':[
                                            {
                                                'nombre':'CORREO',
                                                'valor':'josephpluas@gmail.com'
                                            },
                                            {
                                                'nombre':'TELEFONO',
                                                'valor':'096257272'
                                            },
                                            {
                                                'nombre':'VENDEDOR',
                                                'valor':'Joseph Pluas'
                                            }
                                         ],
                                    },
                                    {
                                          'compania':1,
                                          'tipoEmision':1,
                                          'tipoDocumento': '01',
                                          'establecimiento': '001', 
                                          'puntoEmision':'001',
                                          'secuencial':'000000112',
                                          'claveAcceso':'',
                                          'fechaEmision':'05/02/2020',
                                          'tipoIdentificacionComprador':'04',
                                          'guiaRemision':'',
                                          'razonSocialComprador':'MUEBLERIA DORMIHOGAR',
                                          'identificacionComprador':'0942015215',
                                          'totalSinImpuestos': 6787.00,
                                          'totalDescuento': 0.00,
                                          'propina': 0.00,
                                          'importeTotal': 6787.00,
                                          'moneda': 'DOLAR',
                                          'Email':'josephpluas@gmail.com',
                                          'ContigenciaDet': 0,
                                          'estado': 'A',
                                          'ambiente': '1',
                                          'ruc':'0943434556001',
                                          'codigoNumerico': 408,
                                          'DetalleFactura' :[
                                                    {
                                                        'codigoPrincipal':'1',
                                                        'codigoAuxiliar': '1',
                                                        'descripcion':'01-COMPUTADORA DELL',
                                                        'cantidad': 1,
                                                        'precioUnitario': 6787.00,
                                                        'descuento': 0.00,
                                                        'precioTotalSinImpuesto': 6787.00,
                                                        'detalleImpuesto':[
                                                                {
                                                                   'codigoPrincipal':'1',
                                                                   'codigo': '2',
                                                                   'codigoPorcentaje':'0',
                                                                   'tarifa': '1', 
                                                                   'baseImponible': 6787.00,
                                                                   'valor': 0
                                                            }
                                                        ],
                                                        'detalleAdicional':[
                                                                {
                                                                    'codigoPrincipal':'01',
                                                                    'nombre':'SII',
                                                                    'valor': 0.00
                                                            }
                                                        ]
                                                    }
                                            ],
                                            'formaPago':[
                                                  {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 },
                                                 {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 }
                                          ],
                                         'totalImpuesto': [
                                           {
                                               'codigo':'2',
                                               'codigoPorcentaje':'2',
                                               'tarifa':'0',
                                               'descuentoAdicional': 0,
                                               'baseImponible': 6787.00,
                                               'valor': 0.00
                                            }
                                         ],
                                         'infoAdicional':[
                                            {
                                                'nombre':'CORREO',
                                                'valor':'josephpluas@gmail.com'
                                            },
                                            {
                                                'nombre':'TELEFONO',
                                                'valor':'096257272'
                                            },
                                            {
                                                'nombre':'VENDEDOR',
                                                'valor':'Joseph Pluas'
                                            }
                                         ],
                                    },
                                    {
                                          'compania':1,
                                          'tipoEmision':1,
                                          'tipoDocumento': '01',
                                          'establecimiento': '001', 
                                          'puntoEmision':'001',
                                          'secuencial':'000000113',
                                          'claveAcceso':'',
                                          'fechaEmision':'05/02/2020',
                                          'tipoIdentificacionComprador':'04',
                                          'guiaRemision':'',
                                          'razonSocialComprador':'MUEBLERIA DORMIHOGAR',
                                          'identificacionComprador':'0942015215',
                                          'totalSinImpuestos': 6787.00,
                                          'totalDescuento': 0.00,
                                          'propina': 0.00,
                                          'importeTotal': 6787.00,
                                          'moneda': 'DOLAR',
                                          'Email':'josephpluas@gmail.com',
                                          'ContigenciaDet': 0,
                                          'estado': 'A',
                                          'ambiente': '1',
                                          'ruc':'0943434556001',
                                          'codigoNumerico': 408,
                                          'DetalleFactura' :[
                                                    {
                                                        'codigoPrincipal':'1',
                                                        'codigoAuxiliar': '1',
                                                        'descripcion':'01-COMPUTADORA DELL',
                                                        'cantidad': 1,
                                                        'precioUnitario': 6787.00,
                                                        'descuento': 0.00,
                                                        'precioTotalSinImpuesto': 6787.00,
                                                        'detalleImpuesto':[
                                                                {
                                                                   'codigoPrincipal':'1',
                                                                   'codigo': '2',
                                                                   'codigoPorcentaje':'0',
                                                                   'tarifa': '1', 
                                                                   'baseImponible': 6787.00,
                                                                   'valor': 0
                                                            }
                                                        ],
                                                        'detalleAdicional':[
                                                                {
                                                                    'codigoPrincipal':'01',
                                                                    'nombre':'SII',
                                                                    'valor': 0.00
                                                            }
                                                        ]
                                                    }
                                            ],
                                            'formaPago':[
                                                  {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 },
                                                 {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 }
                                          ],
                                         'totalImpuesto': [
                                           {
                                               'codigo':'2',
                                               'codigoPorcentaje':'2',
                                               'tarifa':'0',
                                               'descuentoAdicional': 0,
                                               'baseImponible': 6787.00,
                                               'valor': 0.00
                                            }
                                         ],
                                         'infoAdicional':[
                                            {
                                                'nombre':'CORREO',
                                                'valor':'josephpluas@gmail.com'
                                            },
                                            {
                                                'nombre':'TELEFONO',
                                                'valor':'096257272'
                                            },
                                            {
                                                'nombre':'VENDEDOR',
                                                'valor':'Joseph Pluas'
                                            }
                                         ],
                                    },
                                    {
                                          'compania':1,
                                          'tipoEmision':1,
                                          'tipoDocumento': '01',
                                          'establecimiento': '001', 
                                          'puntoEmision':'001',
                                          'secuencial':'000000114',
                                          'claveAcceso':'',
                                          'fechaEmision':'05/02/2020',
                                          'tipoIdentificacionComprador':'04',
                                          'guiaRemision':'',
                                          'razonSocialComprador':'MUEBLERIA DORMIHOGAR',
                                          'identificacionComprador':'0942015215',
                                          'totalSinImpuestos': 6787.00,
                                          'totalDescuento': 0.00,
                                          'propina': 0.00,
                                          'importeTotal': 6787.00,
                                          'moneda': 'DOLAR',
                                          'Email':'josephpluas@gmail.com',
                                          'ContigenciaDet': 0,
                                          'estado': 'A',
                                          'ambiente': '1',
                                          'ruc':'0943434556001',
                                          'codigoNumerico': 408,
                                          'DetalleFactura' :[
                                                    {
                                                        'codigoPrincipal':'1',
                                                        'codigoAuxiliar': '1',
                                                        'descripcion':'01-COMPUTADORA DELL',
                                                        'cantidad': 1,
                                                        'precioUnitario': 6787.00,
                                                        'descuento': 0.00,
                                                        'precioTotalSinImpuesto': 6787.00,
                                                        'detalleImpuesto':[
                                                                {
                                                                   'codigoPrincipal':'1',
                                                                   'codigo': '2',
                                                                   'codigoPorcentaje':'0',
                                                                   'tarifa': '1', 
                                                                   'baseImponible': 6787.00,
                                                                   'valor': 0
                                                            }
                                                        ],
                                                        'detalleAdicional':[
                                                                {
                                                                    'codigoPrincipal':'01',
                                                                    'nombre':'SII',
                                                                    'valor': 0.00
                                                            }
                                                        ]
                                                    }
                                            ],
                                            'formaPago':[
                                                  {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 },
                                                 {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 }
                                          ],
                                         'totalImpuesto': [
                                           {
                                               'codigo':'2',
                                               'codigoPorcentaje':'2',
                                               'tarifa':'0',
                                               'descuentoAdicional': 0,
                                               'baseImponible': 6787.00,
                                               'valor': 0.00
                                            }
                                         ],
                                         'infoAdicional':[
                                            {
                                                'nombre':'CORREO',
                                                'valor':'josephpluas@gmail.com'
                                            },
                                            {
                                                'nombre':'TELEFONO',
                                                'valor':'096257272'
                                            },
                                            {
                                                'nombre':'VENDEDOR',
                                                'valor':'Joseph Pluas'
                                            }
                                         ],
                                    },
                                    {
                                          'compania':1,
                                          'tipoEmision':1,
                                          'tipoDocumento': '01',
                                          'establecimiento': '001', 
                                          'puntoEmision':'001',
                                          'secuencial':'000000115',
                                          'claveAcceso':'',
                                          'fechaEmision':'05/02/2020',
                                          'tipoIdentificacionComprador':'04',
                                          'guiaRemision':'',
                                          'razonSocialComprador':'MUEBLERIA DORMIHOGAR',
                                          'identificacionComprador':'0942015215',
                                          'totalSinImpuestos': 6787.00,
                                          'totalDescuento': 0.00,
                                          'propina': 0.00,
                                          'importeTotal': 6787.00,
                                          'moneda': 'DOLAR',
                                          'Email':'josephpluas@gmail.com',
                                          'ContigenciaDet': 0,
                                          'estado': 'A',
                                          'ambiente': '1',
                                          'ruc':'0943434556001',
                                          'codigoNumerico': 408,
                                          'DetalleFactura' :[
                                                    {
                                                        'codigoPrincipal':'1',
                                                        'codigoAuxiliar': '1',
                                                        'descripcion':'01-COMPUTADORA DELL',
                                                        'cantidad': 1,
                                                        'precioUnitario': 6787.00,
                                                        'descuento': 0.00,
                                                        'precioTotalSinImpuesto': 6787.00,
                                                        'detalleImpuesto':[
                                                                {
                                                                   'codigoPrincipal':'1',
                                                                   'codigo': '2',
                                                                   'codigoPorcentaje':'0',
                                                                   'tarifa': '1', 
                                                                   'baseImponible': 6787.00,
                                                                   'valor': 0
                                                            }
                                                        ],
                                                        'detalleAdicional':[
                                                                {
                                                                    'codigoPrincipal':'01',
                                                                    'nombre':'SII',
                                                                    'valor': 0.00
                                                            }
                                                        ]
                                                    }
                                            ],
                                            'formaPago':[
                                                  {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 },
                                                 {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 }
                                          ],
                                         'totalImpuesto': [
                                           {
                                               'codigo':'2',
                                               'codigoPorcentaje':'2',
                                               'tarifa':'0',
                                               'descuentoAdicional': 0,
                                               'baseImponible': 6787.00,
                                               'valor': 0.00
                                            }
                                         ],
                                         'infoAdicional':[
                                            {
                                                'nombre':'CORREO',
                                                'valor':'josephpluas@gmail.com'
                                            },
                                            {
                                                'nombre':'TELEFONO',
                                                'valor':'096257272'
                                            },
                                            {
                                                'nombre':'VENDEDOR',
                                                'valor':'Joseph Pluas'
                                            }
                                         ],
                                    },
                                    {
                                          'compania':1,
                                          'tipoEmision':1,
                                          'tipoDocumento': '01',
                                          'establecimiento': '001', 
                                          'puntoEmision':'001',
                                          'secuencial':'000000116',
                                          'claveAcceso':'',
                                          'fechaEmision':'05/02/2020',
                                          'tipoIdentificacionComprador':'04',
                                          'guiaRemision':'',
                                          'razonSocialComprador':'MUEBLERIA DORMIHOGAR',
                                          'identificacionComprador':'0942015215',
                                          'totalSinImpuestos': 6787.00,
                                          'totalDescuento': 0.00,
                                          'propina': 0.00,
                                          'importeTotal': 6787.00,
                                          'moneda': 'DOLAR',
                                          'Email':'josephpluas@gmail.com',
                                          'ContigenciaDet': 0,
                                          'estado': 'A',
                                          'ambiente': '1',
                                          'ruc':'0943434556001',
                                          'codigoNumerico': 408,
                                          'DetalleFactura' :[
                                                    {
                                                        'codigoPrincipal':'1',
                                                        'codigoAuxiliar': '1',
                                                        'descripcion':'01-COMPUTADORA DELL',
                                                        'cantidad': 1,
                                                        'precioUnitario': 6787.00,
                                                        'descuento': 0.00,
                                                        'precioTotalSinImpuesto': 6787.00,
                                                        'detalleImpuesto':[
                                                                {
                                                                   'codigoPrincipal':'1',
                                                                   'codigo': '2',
                                                                   'codigoPorcentaje':'0',
                                                                   'tarifa': '1', 
                                                                   'baseImponible': 6787.00,
                                                                   'valor': 0
                                                            }
                                                        ],
                                                        'detalleAdicional':[
                                                                {
                                                                    'codigoPrincipal':'01',
                                                                    'nombre':'SII',
                                                                    'valor': 0.00
                                                            }
                                                        ]
                                                    }
                                            ],
                                            'formaPago':[
                                                  {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 },
                                                 {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 }
                                          ],
                                         'totalImpuesto': [
                                           {
                                               'codigo':'2',
                                               'codigoPorcentaje':'2',
                                               'tarifa':'0',
                                               'descuentoAdicional': 0,
                                               'baseImponible': 6787.00,
                                               'valor': 0.00
                                            }
                                         ],
                                         'infoAdicional':[
                                            {
                                                'nombre':'CORREO',
                                                'valor':'josephpluas@gmail.com'
                                            },
                                            {
                                                'nombre':'TELEFONO',
                                                'valor':'096257272'
                                            },
                                            {
                                                'nombre':'VENDEDOR',
                                                'valor':'Joseph Pluas'
                                            }
                                         ],
                                    },
                                    {
                                          'compania':1,
                                          'tipoEmision':1,
                                          'tipoDocumento': '01',
                                          'establecimiento': '001', 
                                          'puntoEmision':'001',
                                          'secuencial':'000000117',
                                          'claveAcceso':'',
                                          'fechaEmision':'05/02/2020',
                                          'tipoIdentificacionComprador':'04',
                                          'guiaRemision':'',
                                          'razonSocialComprador':'MUEBLERIA DORMIHOGAR',
                                          'identificacionComprador':'0942015215',
                                          'totalSinImpuestos': 6787.00,
                                          'totalDescuento': 0.00,
                                          'propina': 0.00,
                                          'importeTotal': 6787.00,
                                          'moneda': 'DOLAR',
                                          'Email':'josephpluas@gmail.com',
                                          'ContigenciaDet': 0,
                                          'estado': 'A',
                                          'ambiente': '1',
                                          'ruc':'0943434556001',
                                          'codigoNumerico': 408,
                                          'DetalleFactura' :[
                                                    {
                                                        'codigoPrincipal':'1',
                                                        'codigoAuxiliar': '1',
                                                        'descripcion':'01-COMPUTADORA DELL',
                                                        'cantidad': 1,
                                                        'precioUnitario': 6787.00,
                                                        'descuento': 0.00,
                                                        'precioTotalSinImpuesto': 6787.00,
                                                        'detalleImpuesto':[
                                                                {
                                                                   'codigoPrincipal':'1',
                                                                   'codigo': '2',
                                                                   'codigoPorcentaje':'0',
                                                                   'tarifa': '1', 
                                                                   'baseImponible': 6787.00,
                                                                   'valor': 0
                                                            }
                                                        ],
                                                        'detalleAdicional':[
                                                                {
                                                                    'codigoPrincipal':'01',
                                                                    'nombre':'SII',
                                                                    'valor': 0.00
                                                            }
                                                        ]
                                                    }
                                            ],
                                            'formaPago':[
                                                  {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 },
                                                 {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 }
                                          ],
                                         'totalImpuesto': [
                                           {
                                               'codigo':'2',
                                               'codigoPorcentaje':'2',
                                               'tarifa':'0',
                                               'descuentoAdicional': 0,
                                               'baseImponible': 6787.00,
                                               'valor': 0.00
                                            }
                                         ],
                                         'infoAdicional':[
                                            {
                                                'nombre':'CORREO',
                                                'valor':'josephpluas@gmail.com'
                                            },
                                            {
                                                'nombre':'TELEFONO',
                                                'valor':'096257272'
                                            },
                                            {
                                                'nombre':'VENDEDOR',
                                                'valor':'Joseph Pluas'
                                            }
                                         ],
                                    },
                                    {
                                          'compania':1,
                                          'tipoEmision':1,
                                          'tipoDocumento': '01',
                                          'establecimiento': '001', 
                                          'puntoEmision':'001',
                                          'secuencial':'000000118',
                                          'claveAcceso':'',
                                          'fechaEmision':'05/02/2020',
                                          'tipoIdentificacionComprador':'04',
                                          'guiaRemision':'',
                                          'razonSocialComprador':'MUEBLERIA DORMIHOGAR',
                                          'identificacionComprador':'0942015215',
                                          'totalSinImpuestos': 6787.00,
                                          'totalDescuento': 0.00,
                                          'propina': 0.00,
                                          'importeTotal': 6787.00,
                                          'moneda': 'DOLAR',
                                          'Email':'josephpluas@gmail.com',
                                          'ContigenciaDet': 0,
                                          'estado': 'A',
                                          'ambiente': '1',
                                          'ruc':'0943434556001',
                                          'codigoNumerico': 408,
                                          'DetalleFactura' :[
                                                    {
                                                        'codigoPrincipal':'1',
                                                        'codigoAuxiliar': '1',
                                                        'descripcion':'01-COMPUTADORA DELL',
                                                        'cantidad': 1,
                                                        'precioUnitario': 6787.00,
                                                        'descuento': 0.00,
                                                        'precioTotalSinImpuesto': 6787.00,
                                                        'detalleImpuesto':[
                                                                {
                                                                   'codigoPrincipal':'1',
                                                                   'codigo': '2',
                                                                   'codigoPorcentaje':'0',
                                                                   'tarifa': '1', 
                                                                   'baseImponible': 6787.00,
                                                                   'valor': 0
                                                            }
                                                        ],
                                                        'detalleAdicional':[
                                                                {
                                                                    'codigoPrincipal':'01',
                                                                    'nombre':'SII',
                                                                    'valor': 0.00
                                                            }
                                                        ]
                                                    }
                                            ],
                                            'formaPago':[
                                                  {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 },
                                                 {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 }
                                          ],
                                         'totalImpuesto': [
                                           {
                                               'codigo':'2',
                                               'codigoPorcentaje':'2',
                                               'tarifa':'0',
                                               'descuentoAdicional': 0,
                                               'baseImponible': 6787.00,
                                               'valor': 0.00
                                            }
                                         ],
                                         'infoAdicional':[
                                            {
                                                'nombre':'CORREO',
                                                'valor':'josephpluas@gmail.com'
                                            },
                                            {
                                                'nombre':'TELEFONO',
                                                'valor':'096257272'
                                            },
                                            {
                                                'nombre':'VENDEDOR',
                                                'valor':'Joseph Pluas'
                                            }
                                         ],
                                    },
                                    {
                                          'compania':1,
                                          'tipoEmision':1,
                                          'tipoDocumento': '01',
                                          'establecimiento': '001', 
                                          'puntoEmision':'001',
                                          'secuencial':'000000119',
                                          'claveAcceso':'',
                                          'fechaEmision':'05/02/2020',
                                          'tipoIdentificacionComprador':'04',
                                          'guiaRemision':'',
                                          'razonSocialComprador':'MUEBLERIA DORMIHOGAR',
                                          'identificacionComprador':'0942015215',
                                          'totalSinImpuestos': 6787.00,
                                          'totalDescuento': 0.00,
                                          'propina': 0.00,
                                          'importeTotal': 6787.00,
                                          'moneda': 'DOLAR',
                                          'Email':'josephpluas@gmail.com',
                                          'ContigenciaDet': 0,
                                          'estado': 'A',
                                          'ambiente': '1',
                                          'ruc':'0943434556001',
                                          'codigoNumerico': 408,
                                          'DetalleFactura' :[
                                                    {
                                                        'codigoPrincipal':'1',
                                                        'codigoAuxiliar': '1',
                                                        'descripcion':'01-COMPUTADORA DELL',
                                                        'cantidad': 1,
                                                        'precioUnitario': 6787.00,
                                                        'descuento': 0.00,
                                                        'precioTotalSinImpuesto': 6787.00,
                                                        'detalleImpuesto':[
                                                                {
                                                                   'codigoPrincipal':'1',
                                                                   'codigo': '2',
                                                                   'codigoPorcentaje':'0',
                                                                   'tarifa': '1', 
                                                                   'baseImponible': 6787.00,
                                                                   'valor': 0
                                                            }
                                                        ],
                                                        'detalleAdicional':[
                                                                {
                                                                    'codigoPrincipal':'01',
                                                                    'nombre':'SII',
                                                                    'valor': 0.00
                                                            }
                                                        ]
                                                    }
                                            ],
                                            'formaPago':[
                                                  {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 },
                                                 {
                                                      'formaPago':'20',
                                                      'plazo':'12',
                                                      'unidadTiempo': 'DIAS',
                                                      'total': 560.00
                                                 }
                                          ],
                                         'totalImpuesto': [
                                           {
                                               'codigo':'2',
                                               'codigoPorcentaje':'2',
                                               'tarifa':'0',
                                               'descuentoAdicional': 0,
                                               'baseImponible': 6787.00,
                                               'valor': 0.00
                                            }
                                         ],
                                         'infoAdicional':[
                                            {
                                                'nombre':'CORREO',
                                                'valor':'josephpluas@gmail.com'
                                            },
                                            {
                                                'nombre':'TELEFONO',
                                                'valor':'096257272'
                                            },
                                            {
                                                'nombre':'VENDEDOR',
                                                'valor':'Joseph Pluas'
                                            }
                                         ],
                                    }
                                  ],
                              }";
            return _requestFactura;
        }


        public string FacturaPrueba()
        {
            Utilitarios.logs.LogsFactura.LogsInicioFin("************FacturaPrueba************");
            string ruta = ConfigurationManager.AppSettings["pathJsonFactura"];
            string _requestFactura = File.ReadAllText(ruta);





            return _requestFactura;
        }


        public Factura FacturaRequestUna()
        {
            Factura factura = new Factura();
            string _requestFactura = @"
{
    'compania': 1,
    'tipoEmision': 1,
    'tipoDocumento': '01',
    'establecimiento': '001',
    'puntoEmision': '131',
    'secuencial': '000001153',
    'claveAcceso': null,
    'fechaEmision': '21\/04\/2020',
    'tipoIdentificacionComprador': '07',
    'guiaRemision': '',
    'razonSocialComprador': 'CONSUMIDOR final',
    'identificacioncomprador': '9999999999999',
    'totalsinimpuestos': 1.25,
    'totaldescuento': 0,
    'propina': 0,
    'importetotal': 1.4,
    'moneda': 'dolar',
    'email': 'facturacionelectronica@labraquelabra.com',
    'ContigenciaDet': 0,
    'estado': 'A',
    'ambiente': '1',
    'ruc': '0992446501001',
    'codigoNumerico': '202003040041',
    'DetalleFactura': [
        {
            'codigoPrincipal': '2-1',
            'codigoAuxiliar': '101',
            'descripcion': 'DISCO GRANDE FREIR 24u',
            'cantidad': 1,
            'preciounitario': 1.25,
            'descuento': 0,
            'preciototalsinimpuesto': 1.25,
            'detalleimpuesto': [
                {
                    'codigoprincipal': '2-1',
                    'codigo': 2,
                    'codigoporcentaje': '2',
                    'tarifa': '12.00',
                    'baseimponible': '1.2500',
                    'valor': 0.15
                }
            ],
            'detalleadicional': [
                {
                    'codigoprincipal': '',
                    'nombre': '',
                    'valor': ''
                }
            ]
        }
    ],
    'formapago': [
        {
            'formapago': '01',
            'plazo': 0,
            'unidadtiempo': 'dias',
            'total': 1.4
        }
    ],
    'totalimpuesto': [
        {
            'codigo': 2,
            'codigoporcentaje': '2',
            'tarifa': '12.00',
            'descuentoadicional': 0,
            'baseimponible': 0,
            'valor': 0.15
        }
    ],
    'infoadicional': [
        {
            'nombre': 'correo',
            'valor': 'cbalcazar@viamatica.com'
        },
        {
            'nombre': 'TELEFONO',
            'valor': '9999999999'
        },
        {
            'nombre': 'VENDEDOR',
            'valor': 'YELKINS MARIANGEL TERAN BACALAO'
        }
    ]
}";
            factura = Newtonsoft.Json.JsonConvert.DeserializeObject<Factura>(_requestFactura);

            return factura;
        }




        public string CompRetencion()
        {
            string _requestCompRetencion = @"{'procesaCompRetencion': true,
                                 'compRetencionRequest': [
                                      {
                                          'compania':'1',
                                          'establecimiento': '001', 
                                          'puntoEmision':'001',
                                          'secuencial':'000000001',
                                          'fechaEmision':'10-12-2019',
                                          'identificacionSujetoRetenido':'01',
                                          'periodoFiscal':'0',
                                          'razonSocialSujetoRetenido':'MUEBLERIA DORMIHOGAR',
                                          'identificacionComprador':'0942015215',
                                          'tipoIdentificacionSujetoRetenido': 20.00,
                                          'email': 20.00,
                                          'ambiente': 20.00,
                                          'codigoNumerico': 0,
                                          'ruc': 0,
                                          'detalleRetencion': [
                                           {
                                               'impuesto':'0',
                                               'codRetencion':'2',
                                               'baseImponible':'0',
                                               'porcentajeRetener':'0',
                                               'valorRetenido':'20.00',
                                               'codDocSustento':'20.00',
                                               'numDocSustento':'20.00',
                                               'fechaEmisionDocSustento':'HFHF'
                                            }
                                         ],
                                         'infoAdicional':[{
                                                'nombre':'VENDEDOR',
                                                'valor':'JOSVIN'
                                            },
                                            {
                                                'nombre':'VALOR PAGO',
                                                'valor':'0.00'
                                            },
                                            {
                                                'nombre':'TELEFONO',
                                                'valor':'0963389829'
                                            }
                                        ]
                                    }],
                              }";

            return _requestCompRetencion;
        }


        public string NotaCredito()
        {
            string _requestNotaCredito = @"{'procesaNotaCredito': true,
                                 'notaCreditoRequest': [
                                      {
                                          'compania':1,
                                          'tipoEmision':1,
                                          'tipoDocumento': 'A',
                                          'establecimiento': '001', 
                                          'puntoEmision':'001',
                                          'secuencial':'000000001',
                                          'fechaEmision':'10-12-2019',
                                          'tipoIdentificacionComprador':'01',
                                          'razonSocialComprador':'MUEBLERIA DORMIHOGAR',
                                          'identificacionComprador':'0942015215',
                                          'rise':'02',
                                          'tipoDocumentoModificado':'SII',
                                          'numeroDocumentoModificado':'FACTURA',
                                          'fechaEmisionDocumentoModificado':'01'
                                          'totalSinImpuestos': 20.00,
                                          'valorModificado': 20.00,
                                          'moneda': 0,
                                          'motivo':'Producto dañado',
                                          'Email':'josephpluas@gmail.com',
                                          'contigenciaDet':0,
                                          'estado': '1',
                                          'ambiente': '1',
                                          'codigoNumerico':'09888',
                                          'ruc':'0943434556001',
                                          'detalle' :[
                                                    {
                                                        'codigoInterno':'1',
                                                        'codigoAdicional': '001',
                                                        'descripcion':'01-COMPUTADORA DELL',
                                                        'cantidad': 1,
                                                        'precioUnitario': 10.00,
                                                        'descuento': 0.00,
                                                        'precioTotalSinImpuesto': 15.00,
                                                        'detalleImpuesto':[
                                                                {
                                                                   'codigoInterno':'1',
                                                                   'codigo': '001',
                                                                   'codigoPorcentaje':'01-COMPUTADORA DELL',
                                                                   'tarifa': '1', 
                                                                   'baseImponible': 0,
                                                                   'valor': 0
                                                            }
                                                        ],
                                                        'detalleAdicional':[
                                                                {
                                                                    'codigoPrincipal':'01',
                                                                    'nombre':'SII',
                                                                    'valor': 0.00
                                                            }
                                                        ]
                                                    },
                                                    {
                                                        'codigoInterno':'1',
                                                        'codigoAdicional': '001',
                                                        'descripcion':'01-COMPUTADORA DELL',
                                                        'cantidad': 1,
                                                        'precioUnitario': 10.00,
                                                        'descuento': 0.00,
                                                        'precioTotalSinImpuesto': 15.00,
                                                        'detalleImpuesto':[
                                                                {
                                                                   'codigoInterno':'1',
                                                                   'codigo': '001',
                                                                   'codigoPorcentaje':'01-COMPUTADORA DELL',
                                                                   'tarifa': '1', 
                                                                   'baseImponible': 0,
                                                                   'valor': 0
                                                            }
                                                        ],
                                                        'detalleAdicional':[
                                                                {
                                                                    'codigoPrincipal':'01',
                                                                    'nombre':'SII',
                                                                    'valor': 0.00
                                                            }
                                                        ]
                                                    }
                                            ],
                                         'totalImpuesto': [
                                           {
                                               'codigo':'0',
                                               'codigoPorcentaje':'2',
                                               'baseImponible': 20.00,
                                               'valor': 20.00
                                            }
                                         ],
                                         'infoAdicional':[
                                            {
                                                'nombre':'CORREO',
                                                'valor':'josephpluas@gmail.com'
                                            },
                                            {
                                                'nombre':'TELEFONO',
                                                'valor':'096257272'
                                            },
                                            {
                                                'nombre':'VENDEDOR',
                                                'valor':'Joseph Pluas'
                                            }
                                         ],
                                    }],
                              }";
            return _requestNotaCredito;
        }


        public string GuiaRemision()
        {
            string _requestGuiaRemision = @"{'procesaGuiaRemision': true,
                                 'guiaRemisionRequest': [
                                      {
                                          'compania':1,
                                          'tipoEmision':1,
                                          'tipoDocumento': 'A',
                                          'establecimiento': '001', 
                                          'puntoEmision':'001',
                                          'secuencial':'000000001',
                                          'direccionPartida':'9 de Octubre',
                                          'razonSocialTransportista':'MUEBLERIA DORMIHOGAR',
                                          'tipoIdentificacionTransportista':'01',
                                          'rucTransportista':'0942015215001',
                                          'rise':''
                                          'fechaIniTransporte':'03-02-2020',
                                          'fechaFinTransporte': '03-02-2020',
                                          'placa': '8979-ANS',
                                          'email':'josephpluas@gmail.com',
                                          'estado': '1',
                                          'ambiente': '1',
                                          'codigoNumerico':'DSDSDS'
                                          'ruc':'0943434556001',
                                          'DetalleFactura' :[
                                                    {
                                                        'codigoPrincipal':'1',
                                                        'codigoAuxiliar': '001',
                                                        'descripcion':'01-COMPUTADORA DELL',
                                                        'cantidad': 1,
                                                        'precioUnitario': 10.00,
                                                        'descuento': 0.00,
                                                        'precioTotalSinImpuesto': 15.00,
                                                        'detalleImpuesto':[
                                                                {
                                                                   'codigoPrincipal':'1',
                                                                   'codigo': '2',
                                                                   'codigoPorcentaje':'01-COMPUTADORA DELL',
                                                                   'tarifa': '1', 
                                                                   'baseImponible': 0,
                                                                   'valor': 0
                                                            }
                                                        ],
                                                        'detalleAdicional':[
                                                                {
                                                                    'codigoPrincipal':'01',
                                                                    'nombre':'SII',
                                                                    'valor': 0.00
                                                            }
                                                        ]
                                                    },
                                                    {
                                                        'codigoPrincipal':'2',
                                                        'codigoAuxiliar': '001',
                                                        'descripcion':'01-LAPTOP DELL ',
                                                        'cantidad': '1',
                                                        'precioUnitario': 10.00,
                                                        'descuento': 0.00,
                                                        'precioTotalSinImpuesto': 15.00,
                                                        'detalleImpuesto':[
                                                                {
                                                                   'codigoPrincipal':'1',
                                                                   'codigo': '2',
                                                                   'codigoPorcentaje':'01-COMPUTADORA DELL',
                                                                   'tarifa': '1', 
                                                                   'baseImponible': 0,
                                                                   'valor': 0
                                                            }
                                                        ],
                                                        'detalleAdicional':[
                                                                {
                                                                    'codigoPrincipal':'01',
                                                                    'nombre':'SII',
                                                                    'valor': 0.00
                                                            }
                                                        ]
                                                    }
                                            ],
                                            'formaPago':[
                                                  {
                                                      'formaPago':'CORRIENTE',
                                                      'plazo':'12',
                                                      'unidadTiempo': 0.00,
                                                      'total': 15.00
                                                 },
                                                 {
                                                      'formaPago':'CORRIENTE',
                                                      'plazo':'12',
                                                      'unidadTiempo': 0.00,
                                                      'total': 15.00
                                                 }
                                          ],
                                         'totalImpuesto': [
                                           {
                                               'codigo':'2',
                                               'codigoPorcentaje':'2',
                                               'tarifa':'0',
                                               'descuentoAdicional': 0,
                                               'baseImponible': 20.00,
                                               'valor': 20.00
                                            }
                                         ],
                                         'infoAdicional':[
                                            {
                                                'nombre':'CORREO',
                                                'valor':'josephpluas@gmail.com'
                                            },
                                            {
                                                'nombre':'TELEFONO',
                                                'valor':'096257272'
                                            },
                                            {
                                                'nombre':'VENDEDOR',
                                                'valor':'Joseph Pluas'
                                            }
                                         ],
                                    }],
                              }";
            return _requestGuiaRemision;

        }
    }
}