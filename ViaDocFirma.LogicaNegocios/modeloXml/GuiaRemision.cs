using System.Xml.Serialization;

[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public class guiaRemision
{
    private infoTributaria_GR infoTributariaField;
    private guiaRemisionInfoGuiaRemision infoGuiaRemisionField;
    private guiaRemisionDestinatarios destinatariosField;
    private guiaRemisionCampoAdicional[] infoAdicionalField;
    private guiaRemisionID idField;
    private string versionField;

    /// <comentarios/>
    public infoTributaria_GR infoTributaria
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
    public guiaRemisionInfoGuiaRemision infoGuiaRemision
    {
        get
        {
            return this.infoGuiaRemisionField;
        }
        set
        {
            this.infoGuiaRemisionField = value;
        }
    }

    /// <comentarios/>
    public guiaRemisionDestinatarios destinatarios
    {
        get
        {
            return this.destinatariosField;
        }
        set
        {
            this.destinatariosField = value;
        }
    }

    /// <comentarios/>
    [System.Xml.Serialization.XmlArrayItemAttribute("campoAdicional", IsNullable = false)]
    public guiaRemisionCampoAdicional[] infoAdicional
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
    public guiaRemisionID id
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
public class infoTributaria_GR
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
public class detalle
{
    private string codigoInternoField;
    private string codigoAdicionalField;
    private string descripcionField;
    private decimal cantidadField;
    private detalleDetAdicional[] detallesAdicionalesField;

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
    [System.Xml.Serialization.XmlArrayItemAttribute("detAdicional", IsNullable = false)]
    public detalleDetAdicional[] detallesAdicionales
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
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class detalleDetAdicional
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
public class destinatario
{
    private string identificacionDestinatarioField;
    private string razonSocialDestinatarioField;
    private string dirDestinatarioField;
    private string motivoTrasladoField;
    private string docAduaneroUnicoField;
    private string codEstabDestinoField;
    private string rutaField;
    private string codDocSustentoField;
    private string numDocSustentoField;
    private string numAutDocSustentoField;
    private string fechaEmisionDocSustentoField;
    private destinatarioDetalles detallesField;

    public string identificacionDestinatario
    {
        get
        {
            return this.identificacionDestinatarioField;
        }
        set
        {
            this.identificacionDestinatarioField = value;
        }
    }

    /// <comentarios/>
    public string razonSocialDestinatario
    {
        get
        {
            return this.razonSocialDestinatarioField;
        }
        set
        {
            this.razonSocialDestinatarioField = value;
        }
    }

    /// <comentarios/>
    public string dirDestinatario
    {
        get
        {
            return this.dirDestinatarioField;
        }
        set
        {
            this.dirDestinatarioField = value;
        }
    }

    /// <comentarios/>
    public string motivoTraslado
    {
        get
        {
            return this.motivoTrasladoField;
        }
        set
        {
            this.motivoTrasladoField = value;
        }
    }

    /// <comentarios/>
    [System.Xml.Serialization.XmlElement("docAduaneroUnico", IsNullable = false)]
    public string docAduaneroUnico
    {
        get
        {
            return this.docAduaneroUnicoField;
        }
        set
        {
            this.docAduaneroUnicoField = value;
        }
    }

    /// <comentarios/>
    [System.Xml.Serialization.XmlElement("codEstabDestino", IsNullable = false)]
    public string codEstabDestino
    {
        get
        {
            return this.codEstabDestinoField;
        }
        set
        {
            this.codEstabDestinoField = value;
        }
    }

    /// <comentarios/>
    [System.Xml.Serialization.XmlElement("ruta", IsNullable = false)]
    public string ruta
    {
        get
        {
            return this.rutaField;
        }
        set
        {
            this.rutaField = value;
        }
    }

    /// <comentarios/>
    [System.Xml.Serialization.XmlElement("codDocSustento", IsNullable = false)]
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

    [System.Xml.Serialization.XmlElement("numDocSustento", IsNullable = false)]
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
    [System.Xml.Serialization.XmlElement("numAutDocSustento", IsNullable = false)]
    public string numAutDocSustento
    {
        get
        {
            return this.numAutDocSustentoField;
        }
        set
        {
            this.numAutDocSustentoField = value;
        }
    }

    /// <comentarios/>
    [System.Xml.Serialization.XmlElement("fechaEmisionDocSustento", IsNullable = false)]
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
    public destinatarioDetalles detalles
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
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class destinatarioDetalles
{
    private detalle[] detalleField;

    [System.Xml.Serialization.XmlElementAttribute("detalle")]
    public detalle[] detalle
    {
        get
        {
            return this.detalleField;
        }
        set
        {
            this.detalleField = value;
        }
    }
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class guiaRemisionInfoGuiaRemision
{
    private string dirEstablecimientoField;
    private string dirPartidaField;
    private string razonSocialTransportistaField;
    private string tipoIdentificacionTransportistaField;
    private string rucTransportistaField;
    private string riseField;
    private string obligadoContabilidadField;
    private string contribuyenteEspecialField;
    private string fechaIniTransporteField;
    private string fechaFinTransporteField;
    private string placaField;

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
    public string dirPartida
    {
        get
        {
            return this.dirPartidaField;
        }
        set
        {
            this.dirPartidaField = value;
        }
    }

    /// <comentarios/>
    public string razonSocialTransportista
    {
        get
        {
            return this.razonSocialTransportistaField;
        }
        set
        {
            this.razonSocialTransportistaField = value;
        }
    }

    /// <comentarios/>
    public string tipoIdentificacionTransportista
    {
        get
        {
            return this.tipoIdentificacionTransportistaField;
        }
        set
        {
            this.tipoIdentificacionTransportistaField = value;
        }
    }

    /// <comentarios/>
    public string rucTransportista
    {
        get
        {
            return this.rucTransportistaField;
        }
        set
        {
            this.rucTransportistaField = value;
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
    public string fechaIniTransporte
    {
        get
        {
            return this.fechaIniTransporteField;
        }
        set
        {
            this.fechaIniTransporteField = value;
        }
    }

    /// <comentarios/>
    public string fechaFinTransporte
    {
        get
        {
            return this.fechaFinTransporteField;
        }
        set
        {
            this.fechaFinTransporteField = value;
        }
    }

    /// <comentarios/>
    public string placa
    {
        get
        {
            return this.placaField;
        }
        set
        {
            this.placaField = value;
        }
    }
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class guiaRemisionDestinatarios
{
    private destinatario[] destinatarioField;

    [System.Xml.Serialization.XmlElementAttribute("destinatario")]
    public destinatario[] destinatario
    {
        get
        {
            return this.destinatarioField;
        }
        set
        {
            this.destinatarioField = value;
        }
    }
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class guiaRemisionCampoAdicional
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

[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public enum guiaRemisionID
{
    comprobante,
}
