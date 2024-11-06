using System.Xml.Serialization;


[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public class notaCredito
{
    private infoTributaria_NC infoTributariaField;

    private notaCreditoInfoNotaCredito infoNotaCreditoField;

    private notaCreditoDetalle[] detallesField;

    private notaCreditoCampoAdicional[] infoAdicionalField;

    private notaCreditoID idField;

    private string versionField;

    /// <comentarios/>
    public infoTributaria_NC infoTributaria
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

    /// <comentarios/>
    public notaCreditoInfoNotaCredito infoNotaCredito
    {
        get
        {
            return this.infoNotaCreditoField;
        }
        set
        {
            this.infoNotaCreditoField = value;
        }
    }

    /// <comentarios/>
    [System.Xml.Serialization.XmlArrayItemAttribute("detalle", IsNullable = false)]
    public notaCreditoDetalle[] detalles
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

    /// <comentarios/>
    [System.Xml.Serialization.XmlArrayItemAttribute("campoAdicional", IsNullable = false)]
    public notaCreditoCampoAdicional[] infoAdicional
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

    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public notaCreditoID id
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

    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKEN")]
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

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[XmlRoot("infoTributaria")]
public class infoTributaria_NC
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
    private string regimenMicroempresasField;

    private string agenteRetencionField;

    private string contribuyenteRimpeField;

    /// <comentarios/>
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

    /// <comentarios/>
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

    /// <comentarios/>
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

    /// <comentarios/>
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

    /// <comentarios/>
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

    /// <comentarios/>
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

    /// <comentarios/>
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

    /// <comentarios/>
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

    /// <comentarios/>
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

    /// <comentarios/>
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

    /// <comentarios/>
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

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[XmlRoot("impuesto")]
public class impuesto_NC
{

    private string codigoField;

    private string codigoPorcentajeField;

    private decimal tarifaField;

    private bool tarifaFieldSpecified;

    private decimal baseImponibleField;

    private decimal valorField;

    /// <comentarios/>
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

    /// <comentarios/>
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

    /// <comentarios/>
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

    /// <comentarios/>
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

    /// <comentarios/>
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

    /// <comentarios/>
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

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]

public class notaCreditoInfoNotaCredito
{

    private string fechaEmisionField;

    private string dirEstablecimientoField;

    private string tipoIdentificacionCompradorField;

    private string razonSocialCompradorField;

    private string identificacionCompradorField;

    private string contribuyenteEspecialField;

    private string obligadoContabilidadField;

    private string riseField;

    private string codDocModificadoField;

    private string numDocModificadoField;

    private string fechaEmisionDocSustentoField;

    private decimal totalSinImpuestosField;

    private decimal valorModificacionField;

    private string monedaField;

    private totalConImpuestosTotalImpuesto[] totalConImpuestosField;

    private string motivoField;

    /// <comentarios/>
    /// 
    //[System.Xml.Serialization.XmlElement("fechaEmision", IsNullable = false)]
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

    /// <comentarios/>
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

    /// <comentarios/>
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

    /// <comentarios/>
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

    /// <comentarios/>
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

    /// <comentarios/>
    /// 
    [System.Xml.Serialization.XmlElement("contribuyenteEspecial", IsNullable = false)]
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

    /// <comentarios/>
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

    /// <comentarios/>
    [System.Xml.Serialization.XmlElement("rise", IsNullable = false)]
    public string rise
    {
        get
        {
            return this.riseField;
        }
        set
        {
            this.riseField = value;
        }
    }

    /// <comentarios/>
    public string codDocModificado
    {
        get
        {
            return this.codDocModificadoField;
        }
        set
        {
            this.codDocModificadoField = value;
        }
    }

    /// <comentarios/>
    public string numDocModificado
    {
        get
        {
            return this.numDocModificadoField;
        }
        set
        {
            this.numDocModificadoField = value;
        }
    }

    /// <comentarios/>
    public string fechaEmisionDocSustento
    {
        get
        {
            return this.fechaEmisionDocSustentoField;
        }
        set
        {
            this.fechaEmisionDocSustentoField = value;
        }
    }

    /// <comentarios/>
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

    /// <comentarios/>
    public decimal valorModificacion
    {
        get
        {
            return this.valorModificacionField;
        }
        set
        {
            this.valorModificacionField = value;
        }
    }

    /// <comentarios/>
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

    /// <comentarios/>
    [System.Xml.Serialization.XmlArrayItemAttribute("totalImpuesto", IsNullable = false)]
    public totalConImpuestosTotalImpuesto[] totalConImpuestos
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

    /// <comentarios/>
    public string motivo
    {
        get
        {
            return this.motivoField;
        }
        set
        {
            this.motivoField = value;
        }
    }
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class totalConImpuestosTotalImpuesto
{

    private string codigoField;

    private string codigoPorcentajeField;

    private decimal baseImponibleField;

    private decimal valorField;

    /// <comentarios/>
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

    /// <comentarios/>
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

    /// <comentarios/>
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

    /// <comentarios/>
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

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class notaCreditoDetalle
{

    private string codigoInternoField;

    private string codigoAdicionalField;

    private string descripcionField;

    private decimal cantidadField;

    private decimal precioUnitarioField;

    private decimal descuentoField;

    private bool descuentoFieldSpecified;

    private decimal precioTotalSinImpuestoField;

    private notaCreditoDetalleDetAdicional[] detallesAdicionalesField;

    private impuesto_NC[] impuestosField;

    /// <comentarios/>
    public string codigoInterno
    {
        get
        {
            return this.codigoInternoField;
        }
        set
        {
            this.codigoInternoField = value;
        }
    }

    /// <comentarios/>
    /// 
    [System.Xml.Serialization.XmlElement("codigoAdicional", IsNullable = false)]
    public string codigoAdicional
    {
        get
        {
            return this.codigoAdicionalField;
        }
        set
        {
            this.codigoAdicionalField = value;
        }
    }

    /// <comentarios/>
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

    /// <comentarios/>
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

    /// <comentarios/>
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

    /// <comentarios/>
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

    /// <comentarios/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool descuentoSpecified
    {
        get
        {
            return this.descuentoFieldSpecified;
        }
        set
        {
            this.descuentoFieldSpecified = value;
        }
    }

    /// <comentarios/>
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

    /// <comentarios/>
    [System.Xml.Serialization.XmlArrayItemAttribute("detAdicional", IsNullable = false)]
    public notaCreditoDetalleDetAdicional[] detallesAdicionales
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

    /// <comentarios/>
    //[System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
    [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    [System.Xml.Serialization.XmlArrayItemAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false, ElementName = "impuesto")]
    public impuesto_NC[] impuestos
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

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class notaCreditoDetalleDetAdicional
{

    private string nombreField;

    private string valorField;

    /// <comentarios/>
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

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class notaCreditoCampoAdicional
{

    private string nombreField;

    private string valueField;

    /// <comentarios/>
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

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public enum notaCreditoID
{

    /// <comentarios/>
    comprobante,
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public class totalConImpuestos
{

    private totalConImpuestosTotalImpuesto[] totalImpuestoField;

    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute("totalImpuesto")]
    public totalConImpuestosTotalImpuesto[] totalImpuesto
    {
        get
        {
            return this.totalImpuestoField;
        }
        set
        {
            this.totalImpuestoField = value;
        }
    }
}

