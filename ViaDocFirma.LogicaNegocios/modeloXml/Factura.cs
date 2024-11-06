
using System.Xml.Serialization;


[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public class factura
{
    private infoTributaria_FA infoTributariaField;
    private facturaInfoFactura infoFacturaField;
    private facturaDetalle[] detallesField;
    private facturaCampoAdicional[] infoAdicionalField;
    private reembolsoDetalle[] reembolsosField;
    private facturaID idField;
    private bool idFieldSpecified;
    private string versionField;

    public infoTributaria_FA infoTributaria
    {
        get
        {
            return this.infoTributariaField;
        }
        set
        {
            this.infoTributariaField = value;
        }
    }
    public facturaInfoFactura infoFactura
    {
        get
        {
            return this.infoFacturaField;
        }
        set
        {
            this.infoFacturaField = value;
        }
    }
    [System.Xml.Serialization.XmlArrayItemAttribute("detalle", IsNullable = false)]
    public facturaDetalle[] detalles
    {
        get
        {
            return this.detallesField;
        }
        set
        {
            this.detallesField = value;
        }
    }
    [System.Xml.Serialization.XmlArrayItemAttribute("reembolsoDetalle", IsNullable = false)]
    public reembolsoDetalle[] reembolsos
    {
        get
        {
            return this.reembolsosField;
        }
        set
        {
            this.reembolsosField = value;
        }
    }
    [System.Xml.Serialization.XmlArrayItemAttribute("campoAdicional", IsNullable = false)]
    public facturaCampoAdicional[] infoAdicional
    {
        get
        {
            return this.infoAdicionalField;
        }
        set
        {
            this.infoAdicionalField = value;
        }
    }
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public facturaID id
    {
        get
        {
            return this.idField;
        }
        set
        {
            this.idField = value;
        }
    }
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool idSpecified
    {
        get
        {
            return this.idFieldSpecified;
        }
        set
        {
            this.idFieldSpecified = value;
        }
    }
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string version
    {
        get
        {
            return this.versionField;
        }
        set
        {
            this.versionField = value;
        }
    }
}


[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[XmlRoot("infoTributaria")]
public class infoTributaria_FA
{
    private string ambienteField;
    private string tipoEmisionField;
    private string razonSocialField;
    private string nombreComercialField;
    private string rucField;
    private string claveAccesoField;
    private string codDocField;
    private string estabField;
    private string ptoEmiField;
    private string secuencialField;
    private string dirMatrizField;
    private string agenteRetencionField;
    private string contribuyenteRimpeField;
    private string regimenMicroempresasField;

    public string ambiente
    {
        get
        {
            return this.ambienteField;
        }
        set
        {
            this.ambienteField = value;
        }
    }
    public string tipoEmision
    {
        get
        {
            return this.tipoEmisionField;
        }
        set
        {
            this.tipoEmisionField = value;
        }
    }
    public string razonSocial
    {
        get
        {
            return this.razonSocialField;
        }
        set
        {
            this.razonSocialField = value;
        }
    }
    public string nombreComercial
    {
        get
        {
            return this.nombreComercialField;
        }
        set
        {
            this.nombreComercialField = value;
        }
    }
    public string ruc
    {
        get
        {
            return this.rucField;
        }
        set
        {
            this.rucField = value;
        }
    }
    public string claveAcceso
    {
        get
        {
            return this.claveAccesoField;
        }
        set
        {
            this.claveAccesoField = value;
        }
    }
    public string codDoc
    {
        get
        {
            return this.codDocField;
        }
        set
        {
            this.codDocField = value;
        }
    }
    public string estab
    {
        get
        {
            return this.estabField;
        }
        set
        {
            this.estabField = value;
        }
    }
    public string ptoEmi
    {
        get
        {
            return this.ptoEmiField;
        }
        set
        {
            this.ptoEmiField = value;
        }
    }
    public string secuencial
    {
        get
        {
            return this.secuencialField;
        }
        set
        {
            this.secuencialField = value;
        }
    }
    public string dirMatriz
    {
        get
        {
            return this.dirMatrizField;
        }
        set
        {
            this.dirMatrizField = value;
        }
    }

    public string agenteRetencion
    {
        get
        {
            return this.agenteRetencionField;
        }
        set
        {
            this.agenteRetencionField = value;
        }
    }

    public string contribuyenteRimpe
    {
        get
        {
            return this.contribuyenteRimpeField;
        }
        set
        {
            this.contribuyenteRimpeField = value;
        }
    }
    public string regimenMicroempresas 

    {
        get
        {
            return this.regimenMicroempresasField;
        }
        set
        {
            this.regimenMicroempresasField = value;
        }
    }
}




[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[XmlRoot("impuesto")]
public class impuesto_FA
{
    private string codigoField;
    private string codigoPorcentajeField;
    private decimal tarifaField;
    private decimal baseImponibleField;
    private decimal valorField;

    public string codigo
    {
        get
        {
            return this.codigoField;
        }
        set
        {
            this.codigoField = value;
        }
    }
    public string codigoPorcentaje
    {
        get
        {
            return this.codigoPorcentajeField;
        }
        set
        {
            this.codigoPorcentajeField = value;
        }
    }
    public decimal tarifa
    {
        get
        {
            return this.tarifaField;
        }
        set
        {
            this.tarifaField = value;
        }
    }
    public decimal baseImponible
    {
        get
        {
            return this.baseImponibleField;
        }
        set
        {
            this.baseImponibleField = value;
        }
    }
    public decimal valor
    {
        get
        {
            return this.valorField;
        }
        set
        {
            this.valorField = value;
        }
    }
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[XmlRoot("detalleImpuesto")]
public class impuesto_RE
{
    private string codigoField;
    private string codigoPorcentajeField;
    private decimal tarifaField;
    private decimal baseImponibleReembolsoField;
    private decimal impuestoReembolsoField;

    public string codigo
    {
        get
        {
            return this.codigoField;
        }
        set
        {
            this.codigoField = value;
        }
    }
    public string codigoPorcentaje
    {
        get
        {
            return this.codigoPorcentajeField;
        }
        set
        {
            this.codigoPorcentajeField = value;
        }
    }
    public decimal tarifa
    {
        get
        {
            return this.tarifaField;
        }
        set
        {
            this.tarifaField = value;
        }
    }
    public decimal baseImponibleReembolso
    {
        get
        {
            return this.baseImponibleReembolsoField;
        }
        set
        {
            this.baseImponibleReembolsoField = value;
        }
    }
    public decimal impuestoReembolso
    {
        get
        {
            return this.impuestoReembolsoField;
        }
        set
        {
            this.impuestoReembolsoField = value;
        }
    }
}


[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[XmlRoot("formaPago")]
public class formaPago_FA
{
    private string formaPagoField;
    private decimal totalField;
    private int plazoField;
    private string unidadTiempoField;

    public string formaPago
    {
        get
        {
            return this.formaPagoField;
        }
        set
        {
            this.formaPagoField = value;
        }
    }
    public decimal total
    {
        get
        {
            return this.totalField;
        }
        set
        {
            this.totalField = value;
        }
    }
    public int plazo
    {
        get
        {
            return this.plazoField;
        }
        set
        {
            this.plazoField = value;
        }
    }
    public string unidadTiempo
    {
        get
        {
            return this.unidadTiempoField;
        }
        set
        {
            this.unidadTiempoField = value;
        }
    }
}


[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class facturaInfoFactura
{

    private string fechaEmisionField;

    private string dirEstablecimientoField;

    private string contribuyenteEspecialField;

    private string obligadoContabilidadField;

    private string tipoIdentificacionCompradorField;

    private string guiaRemisionField;

    private string razonSocialCompradorField;

    private string identificacionCompradorField;

    private decimal totalSinImpuestosField;

    private decimal totalDescuentoField;

    private string codDocReembolsoField;

    private string totalComprobantesReembolsoField;

    private string totalBaseImponibleReembolsoField;

    private string totalImpuestoReembolsoField;

    private facturaInfoFacturaTotalImpuesto[] totalConImpuestosField;

    private facturaInfoFacturaFormaPago[] formaPagoField;

    private decimal propinaField;

    private decimal importeTotalField;

    private string monedaField;

    public string fechaEmision
    {
        get
        {
            return this.fechaEmisionField;
        }
        set
        {
            this.fechaEmisionField = value;
        }
    }
    public string dirEstablecimiento
    {
        get
        {
            return this.dirEstablecimientoField;
        }
        set
        {
            this.dirEstablecimientoField = value;
        }
    }
    public string contribuyenteEspecial
    {
        get
        {
            return this.contribuyenteEspecialField;
        }
        set
        {
            this.contribuyenteEspecialField = value;
        }
    }
    public string obligadoContabilidad
    {
        get
        {
            return this.obligadoContabilidadField;
        }
        set
        {
            this.obligadoContabilidadField = value;
        }
    }
    public string tipoIdentificacionComprador
    {
        get
        {
            return this.tipoIdentificacionCompradorField;
        }
        set
        {
            this.tipoIdentificacionCompradorField = value;
        }
    }
    public string guiaRemision
    {
        get
        {
            return this.guiaRemisionField;
        }
        set
        {
            this.guiaRemisionField = value;
        }
    }
    public string razonSocialComprador
    {
        get
        {
            return this.razonSocialCompradorField;
        }
        set
        {
            this.razonSocialCompradorField = value;
        }
    }
    public string identificacionComprador
    {
        get
        {
            return this.identificacionCompradorField;
        }
        set
        {
            this.identificacionCompradorField = value;
        }
    }
    public decimal totalSinImpuestos
    {
        get
        {
            return this.totalSinImpuestosField;
        }
        set
        {
            this.totalSinImpuestosField = value;
        }
    }
    public decimal totalDescuento
    {
        get
        {
            return this.totalDescuentoField;
        }
        set
        {
            this.totalDescuentoField = value;
        }
    }
    public string codDocReembolso
    {
        get
        {
            return this.codDocReembolsoField;
        }
        set
        {
            this.codDocReembolsoField = value;
        }
    }
    public string totalComprobantesReembolso
    {
        get
        {
            return this.totalComprobantesReembolsoField;
        }
        set
        {
            this.totalComprobantesReembolsoField = value;
        }
    }
    public string totalBaseImponibleReembolso
    {
        get
        {
            return this.totalBaseImponibleReembolsoField;
        }
        set
        {
            this.totalBaseImponibleReembolsoField = value;
        }
    }
    public string totalImpuestoReembolso
    {
        get
        {
            return this.totalImpuestoReembolsoField;
        }
        set
        {
            this.totalImpuestoReembolsoField = value;
        }
    }
    [System.Xml.Serialization.XmlArrayItemAttribute("totalImpuesto", IsNullable = false)]
    public facturaInfoFacturaTotalImpuesto[] totalConImpuestos
    {
        get
        {
            return this.totalConImpuestosField;
        }
        set
        {
            this.totalConImpuestosField = value;
        }
    }
    public decimal propina
    {
        get
        {
            return this.propinaField;
        }
        set
        {
            this.propinaField = value;
        }
    }
    public decimal importeTotal
    {
        get
        {
            return this.importeTotalField;
        }
        set
        {
            this.importeTotalField = value;
        }
    }
    public string moneda
    {
        get
        {
            return this.monedaField;
        }
        set
        {
            this.monedaField = value;
        }
    }

    [System.Xml.Serialization.XmlArrayItemAttribute("pago", IsNullable = false)]
    public facturaInfoFacturaFormaPago[] pagos
    {
        get
        {
            return this.formaPagoField;
        }
        set
        {
            this.formaPagoField = value;
        }
    }
}


[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class facturaInfoFacturaTotalImpuesto
{
    private string codigoField;
    private string codigoPorcentajeField;
    private decimal baseImponibleField;
    private decimal tarifaField;
    private bool tarifaFieldSpecified;
    private decimal valorField;

    public string codigo
    {
        get
        {
            return this.codigoField;
        }
        set
        {
            this.codigoField = value;
        }
    }
    public string codigoPorcentaje
    {
        get
        {
            return this.codigoPorcentajeField;
        }
        set
        {
            this.codigoPorcentajeField = value;
        }
    }
    public decimal baseImponible
    {
        get
        {
            return this.baseImponibleField;
        }
        set
        {
            this.baseImponibleField = value;
        }
    }
    public decimal tarifa
    {
        get
        {
            return this.tarifaField;
        }
        set
        {
            this.tarifaField = value;
        }
    }
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool tarifaSpecified
    {
        get
        {
            return this.tarifaFieldSpecified;
        }
        set
        {
            this.tarifaFieldSpecified = value;
        }
    }
    public decimal valor
    {
        get
        {
            return this.valorField;
        }
        set
        {
            this.valorField = value;
        }
    }
}


[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class facturaInfoFacturaFormaPago
{
    private string formaPagoField;
    private decimal totalField;
    private int plazoField;
    private string unidadTiempoField;

    public string formaPago
    {
        get
        {
            return this.formaPagoField;
        }
        set
        {
            this.formaPagoField = value;
        }
    }

    public decimal total
    {
        get
        {
            return this.totalField;
        }
        set
        {
            this.totalField = value;
        }
    }

    public int plazo
    {
        get
        {
            return this.plazoField;
        }
        set
        {
            this.plazoField = value;
        }
    }

    public string unidadTiempo
    {
        get
        {
            return this.unidadTiempoField;
        }
        set
        {
            this.unidadTiempoField = value;
        }
    }
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class facturaDetalle
{

    private string codigoPrincipalField;

    private string codigoAuxiliarField;

    private string descripcionField;

    private decimal cantidadField;

    private decimal precioUnitarioField;

    private decimal descuentoField;

    private decimal precioTotalSinImpuestoField;

    private facturaDetalleDetAdicional[] detallesAdicionalesField;

    private impuesto_FA[] impuestosField;

    /// <comentarios/>
    public string codigoPrincipal
    {
        get
        {
            return this.codigoPrincipalField;
        }
        set
        {
            this.codigoPrincipalField = value;
        }
    }

    public string codigoAuxiliar
    {
        get
        {
            return this.codigoAuxiliarField;
        }
        set
        {
            this.codigoAuxiliarField = value;
        }
    }

    public string descripcion
    {
        get
        {
            return this.descripcionField;
        }
        set
        {
            this.descripcionField = value;
        }
    }

    public decimal cantidad
    {
        get
        {
            return this.cantidadField;
        }
        set
        {
            this.cantidadField = value;
        }
    }

    public decimal precioUnitario
    {
        get
        {
            return this.precioUnitarioField;
        }
        set
        {
            this.precioUnitarioField = value;
        }
    }

    public decimal descuento
    {
        get
        {
            return this.descuentoField;
        }
        set
        {
            this.descuentoField = value;
        }
    }

    public decimal precioTotalSinImpuesto
    {
        get
        {
            return this.precioTotalSinImpuestoField;
        }
        set
        {
            this.precioTotalSinImpuestoField = value;
        }
    }

    [System.Xml.Serialization.XmlArrayItemAttribute("detAdicional", IsNullable = false)]
    public facturaDetalleDetAdicional[] detallesAdicionales
    {
        get
        {
            return this.detallesAdicionalesField;
        }
        set
        {
            this.detallesAdicionalesField = value;
        }
    }

    [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    [System.Xml.Serialization.XmlArrayItemAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false, ElementName = "impuesto")]
    public impuesto_FA[] impuestos
    {
        get
        {
            return this.impuestosField;
        }
        set
        {
            this.impuestosField = value;
        }
    }
}


[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class facturaDetalleDetAdicional
{
    private string nombreField;
    private string valorField;

    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string nombre
    {
        get
        {
            return this.nombreField;
        }
        set
        {
            this.nombreField = value;
        }
    }

    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string valor
    {
        get
        {
            return this.valorField;
        }
        set
        {
            this.valorField = value;
        }
    }
}


[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class facturaCampoAdicional
{
    private string nombreField;
    private string valueField;

    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string nombre
    {
        get
        {
            return this.nombreField;
        }
        set
        {
            this.nombreField = value;
        }
    }

    [System.Xml.Serialization.XmlTextAttribute()]
    public string Value
    {
        get
        {
            return this.valueField;
        }
        set
        {
            this.valueField = value;
        }
    }
}


[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public enum facturaID
{
    /// <comentarios/>
    comprobante,
}