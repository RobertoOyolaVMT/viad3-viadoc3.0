using System.Xml.Serialization;

[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public class notaDebito
{

    private infoTributaria_ND infoTributariaField;
    private notaDebitoInfoNotaDebito infoNotaDebitoField;
    private notaDebitoMotivos motivosField;
    private notaDebitoCampoAdicional[] infoAdicionalField;
    private notaDebitoID idField;
    private bool idFieldSpecified;
    private string versionField;

    public infoTributaria_ND infoTributaria
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
    public notaDebitoInfoNotaDebito infoNotaDebito
    {
        get
        {
            return this.infoNotaDebitoField;
        }
        set
        {
            this.infoNotaDebitoField = value;
        }
    }

    /// <comentarios/>
    public notaDebitoMotivos motivos
    {
        get
        {
            return this.motivosField;
        }
        set
        {
            this.motivosField = value;
        }
    }

    /// <comentarios/>
    [System.Xml.Serialization.XmlArrayItemAttribute("campoAdicional", IsNullable = false)]
    public notaDebitoCampoAdicional[] infoAdicional
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
    public notaDebitoID id
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
public class infoTributaria_ND
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
public class impuesto_ND
{

    private string codigoField;

    private string codigoPorcentajeField;

    private decimal tarifaField;

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

public class notaDebitoInfoNotaDebito
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

    private impuesto_ND[] impuestosField;

    private decimal valorTotalField;

    /// <comentarios/>
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
    [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    [System.Xml.Serialization.XmlArrayItemAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false, ElementName = "impuesto")]
    public impuesto_ND[] impuestos
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

    /// <comentarios/>
    public decimal valorTotal
    {
        get
        {
            return this.valorTotalField;
        }
        set
        {
            this.valorTotalField = value;
        }
    }
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class notaDebitoMotivos
{

    private notaDebitoMotivosMotivo[] motivoField;

    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute("motivo")]
    public notaDebitoMotivosMotivo[] motivo
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
public class notaDebitoMotivosMotivo
{

    private string razonField;

    private decimal valorField;

    /// <comentarios/>
    public string razon
    {
        get
        {
            return this.razonField;
        }
        set
        {
            this.razonField = value;
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
public class notaDebitoCampoAdicional
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
public enum notaDebitoID
{

    /// <comentarios/>
    comprobante,
}

