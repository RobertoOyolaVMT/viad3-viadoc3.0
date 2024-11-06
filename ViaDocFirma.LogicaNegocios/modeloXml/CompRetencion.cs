using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public class comprobanteRetencion
{
    private infoTributaria_CR infoTributariaField;

    private comprobanteRetencionInfoCompRetencion infoCompRetencionField;
    private comprobanteRetencionDocsSustento docsSustentoField;
    private comprobanteRetencionCampoAdicional[] infoAdicionalField;

    private comprobanteRetencionID idField;

    private bool idFieldSpecified;

    private string versionField;

    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    // [XmlElement(ElementName = "infoTributaria")] 
    public infoTributaria_CR infoTributaria
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
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public comprobanteRetencionInfoCompRetencion infoCompRetencion
    {
        get
        {
            return this.infoCompRetencionField;
        }
        set
        {
            this.infoCompRetencionField = value;
        }
    }

    public comprobanteRetencionDocsSustento docsSustento
    {
        get
        {
            return this.docsSustentoField;
        }
        set
        {
            this.docsSustentoField = value;
        }
    }
    /// <comentarios/>
    [System.Xml.Serialization.XmlArrayItemAttribute("campoAdicional", IsNullable = false)]
    public comprobanteRetencionCampoAdicional[] infoAdicional
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
    public comprobanteRetencionID id
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
public class infoTributaria_CR
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

    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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

    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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

    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
public class impuesto_CR
{

    private string codigoField;

    private string codigoRetencionField;

    private decimal baseImponibleField;

    private decimal porcentajeRetenerField;

    private decimal valorRetenidoField;

    private string codDocSustentoField;

    private string numDocSustentoField;

    private string fechaEmisionDocSustentoField;

    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string codigoRetencion
    {
        get
        {
            return this.codigoRetencionField;
        }
        set
        {
            this.codigoRetencionField = value;
        }
    }

    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public decimal porcentajeRetener
    {
        get
        {
            return this.porcentajeRetenerField;
        }
        set
        {
            this.porcentajeRetenerField = value;
        }
    }

    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public decimal valorRetenido
    {
        get
        {
            return this.valorRetenidoField;
        }
        set
        {
            this.valorRetenidoField = value;
        }
    }

    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string codDocSustento
    {
        get
        {
            return this.codDocSustentoField;
        }
        set
        {
            this.codDocSustentoField = value;
        }
    }

    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string numDocSustento
    {
        get
        {
            return this.numDocSustentoField;
        }
        set
        {
            this.numDocSustentoField = value;
        }
    }

    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class comprobanteRetencionInfoCompRetencion
{

    private string fechaEmisionField;

    private string dirEstablecimientoField;

    private string contribuyenteEspecialField;

    private string obligadoContabilidadField;

    private string tipoIdentificacionSujetoRetenidoField;

    private string razonSocialSujetoRetenidoField;

    private string tipoSujetoRetenidoField;
    private string parteRelField;

    private string identificacionSujetoRetenidoField;

    private string periodoFiscalField;

    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string tipoIdentificacionSujetoRetenido
    {
        get
        {
            return this.tipoIdentificacionSujetoRetenidoField;
        }
        set
        {
            this.tipoIdentificacionSujetoRetenidoField = value;
        }
    }
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string tipoSujetoRetenido
    {
        get
        {
            return this.tipoSujetoRetenidoField;
        }
        set
        {
            this.tipoSujetoRetenidoField = value;
        }
    }
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string parteRel
    {
        get
        {
            return this.parteRelField;
        }
        set
        {
            this.parteRelField = value;
        }
    }
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string razonSocialSujetoRetenido
    {
        get
        {
            return this.razonSocialSujetoRetenidoField;
        }
        set
        {
            this.razonSocialSujetoRetenidoField = value;
        }
    }

    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string identificacionSujetoRetenido
    {
        get
        {
            return this.identificacionSujetoRetenidoField;
        }
        set
        {
            this.identificacionSujetoRetenidoField = value;
        }
    }

    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string periodoFiscal
    {
        get
        {
            return this.periodoFiscalField;
        }
        set
        {
            this.periodoFiscalField = value;
        }
    }
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[XmlRoot("docsSustento")]
public class comprobanteRetencionDocsSustento
{
    private comprobanteRetencionDocSustento docSustentoField;
    //[System.Xml.Serialization.XmlArrayItemAttribute( IsNullable = false, ElementName = "docSustento")]
   // [System.Xml.Serialization.XmlArrayItemAttribute("docSustento", IsNullable = false)]
    public comprobanteRetencionDocSustento docSustento
    {
        get
        {
            return this.docSustentoField;
        }
        set
        {
            this.docSustentoField = value;
        }
    }


}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
//[XmlRoot("docSustento")]
public class comprobanteRetencionDocSustento
{
    private string codSustentoField;
    private string codDocSustentoField;
    private string numDocSustentoField;
    private string fechaEmisionDocSustentoField;
    private string fechaRegistroContableField;
    private string pagoLocExtField;
    private string totalSinImpuestosField;
    private string importeTotalField;
    private comprobanteRetencionImpuestosDocSustento[] impuestosDocSustentoField;
    private comprobanteRetencionRetenciones[] retencionesField;
    private comprobanteRetencionPagos[] pagosField;

    /// <comentarios/>

    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string codSustento
    {
        get
        {
            return this.codSustentoField;
        }
        set
        {
            this.codSustentoField = value;
        }
    }

    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string codDocSustento
    {
        get
        {
            return this.codDocSustentoField;
        }
        set
        {
            this.codDocSustentoField = value;
        }
    }
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string numDocSustento
    {
        get
        {
            return this.numDocSustentoField;
        }
        set
        {
            this.numDocSustentoField = value;
        }
    }
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string fechaRegistroContable
    {
        get
        {
            return this.fechaRegistroContableField;
        }
        set
        {
            this.fechaRegistroContableField = value;
        }
    }
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string pagoLocExt
    {
        get
        {
            return this.pagoLocExtField;
        }
        set
        {
            this.pagoLocExtField = value;
        }
    }
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string totalSinImpuestos
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
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string importeTotal
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
    /// <comentarios/>
    [System.Xml.Serialization.XmlArrayItemAttribute("impuestoDocSustento", IsNullable = false)]

    public comprobanteRetencionImpuestosDocSustento[] impuestosDocSustento
    {
        get
        {
            return this.impuestosDocSustentoField;
        }
        set
        {
            this.impuestosDocSustentoField = value;
        }
    }
    /// <comentarios/>
    [System.Xml.Serialization.XmlArrayItemAttribute("retencion", IsNullable = false)]
    public comprobanteRetencionRetenciones[] retenciones
    {
        get
        {
            return this.retencionesField;
        }
        set
        {
            this.retencionesField = value;
        }
    }
    /// <comentarios/>
    [System.Xml.Serialization.XmlArrayItemAttribute("pago", IsNullable = false)]
    public comprobanteRetencionPagos[] pagos
    {
        get
        {
            return this.pagosField;
        }
        set
        {
            this.pagosField = value;
        }
    }
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class comprobanteRetencionImpuestosDocSustento
{
    private string codImpuestoDocSustentoField;
    private string codigoPorcentajeField;
    private string baseImponibleField;
    private string tarifaField;
    private string valorImpuestoField;

    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string codImpuestoDocSustento
    {
        get
        {
            return this.codImpuestoDocSustentoField;
        }
        set
        {
            this.codImpuestoDocSustentoField = value;
        }
    }
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string baseImponible
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
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string tarifa
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
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string valorImpuesto
    {
        get
        {
            return this.valorImpuestoField;
        }
        set
        {
            this.valorImpuestoField = value;
        }
    }
}
/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class comprobanteRetencionRetenciones
{
    private string codigoField;
    private string codigoRetencionField;
    private decimal baseImponibleField;
    private decimal porcentajeRetenerField;
    private decimal valorRetenidoField;
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string codigoRetencion
    {
        get
        {
            return this.codigoRetencionField;
        }
        set
        {
            this.codigoRetencionField = value;
        }
    }
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public decimal porcentajeRetener
    {
        get
        {
            return this.porcentajeRetenerField;
        }
        set
        {
            this.porcentajeRetenerField = value;
        }
    }
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public decimal valorRetenido
    {
        get
        {
            return this.valorRetenidoField;
        }
        set
        {
            this.valorRetenidoField = value;
        }
    }
}
/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class comprobanteRetencionPagos
{
    private string formaPagoField;
    private decimal totalField;

    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
}
/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class comprobanteRetencionCampoAdicional
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
public enum comprobanteRetencionID
{
    /// <comentarios/>
    comprobante,
}


