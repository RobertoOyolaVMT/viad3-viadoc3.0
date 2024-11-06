#if _DYNAMIC_XMLSERIALIZER_COMPILATION
[assembly:System.Security.AllowPartiallyTrustedCallers()]
[assembly:System.Security.SecurityTransparent()]
[assembly:System.Security.SecurityRules(System.Security.SecurityRuleSet.Level1)]
#endif
[assembly:System.Reflection.AssemblyVersionAttribute("1.0.0.0")]
[assembly:System.Xml.Serialization.XmlSerializerVersionAttribute(ParentAssemblyId=@"41cf9571-be00-4dee-b191-92cff2239749,", Version=@"4.0.0.0")]
namespace Microsoft.Xml.Serialization.GeneratedAssembly {

    public class XmlSerializationWriterwsdocumento : System.Xml.Serialization.XmlSerializationWriter {

        public void Write1_insertar(object[] p) {
            WriteStartDocument();
            int pLength = p.Length;
            WriteStartElement(@"insertar", @"urn:insertar", null, true);
            if (pLength > 0) {
                WriteElementString(@"ruc_empresa", @"", ((global::System.String)p[0]), new System.Xml.XmlQualifiedName(@"string", @"http://www.w3.org/2001/XMLSchema"));
            }
            if (pLength > 1) {
                WriteElementString(@"tip_documento", @"", ((global::System.String)p[1]), new System.Xml.XmlQualifiedName(@"string", @"http://www.w3.org/2001/XMLSchema"));
            }
            if (pLength > 2) {
                WriteElementString(@"num_documento", @"", ((global::System.String)p[2]), new System.Xml.XmlQualifiedName(@"string", @"http://www.w3.org/2001/XMLSchema"));
            }
            if (pLength > 3) {
                WriteElementString(@"cedruc_cliente", @"", ((global::System.String)p[3]), new System.Xml.XmlQualifiedName(@"string", @"http://www.w3.org/2001/XMLSchema"));
            }
            if (pLength > 4) {
                WriteElementString(@"tipo_emision", @"", ((global::System.String)p[4]), new System.Xml.XmlQualifiedName(@"string", @"http://www.w3.org/2001/XMLSchema"));
            }
            if (pLength > 5) {
                WriteElementString(@"num_autorizacion", @"", ((global::System.String)p[5]), new System.Xml.XmlQualifiedName(@"string", @"http://www.w3.org/2001/XMLSchema"));
            }
            if (pLength > 6) {
                WriteElementString(@"xml_autorizado", @"", ((global::System.String)p[6]), new System.Xml.XmlQualifiedName(@"string", @"http://www.w3.org/2001/XMLSchema"));
            }
            if (pLength > 7) {
                WriteElementString(@"fec_emision", @"", ((global::System.String)p[7]), new System.Xml.XmlQualifiedName(@"string", @"http://www.w3.org/2001/XMLSchema"));
            }
            if (pLength > 8) {
                WriteElementString(@"fec_autorizacion", @"", ((global::System.String)p[8]), new System.Xml.XmlQualifiedName(@"string", @"http://www.w3.org/2001/XMLSchema"));
            }
            if (pLength > 9) {
                WriteElementString(@"ciContingenciaDet", @"", ((global::System.String)p[9]), new System.Xml.XmlQualifiedName(@"string", @"http://www.w3.org/2001/XMLSchema"));
            }
            WriteEndElement();
            WriteReferencedElements();
        }

        public void Write2_insertarInHeaders(object[] p) {
            WriteStartDocument();
            int pLength = p.Length;
            if (pLength > 0) {
                for (int i = 0; i < pLength; i++) {
                    if (p[i] != null) {
                        WritePotentiallyReferencingElement(null, null, p[i], p[i].GetType(), true, false);
                    }
                }
            }
            WriteReferencedElements();
        }

        protected override void InitCallbacks() {
        }
    }

    public class XmlSerializationReaderwsdocumento : System.Xml.Serialization.XmlSerializationReader {

        public object[] Read1_insertarResponse() {
            Reader.MoveToContent();
            object[] p = new object[1];
            Reader.MoveToContent();
            int whileIterations0 = 0;
            int readerCount0 = ReaderCount;
            while (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                string root = Reader.GetAttribute("root", "http://schemas.xmlsoap.org/soap/encoding/");
                if (root == null || System.Xml.XmlConvert.ToBoolean(root)) break;
                ReadReferencedElement();
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations0, ref readerCount0);
            }
            bool isEmptyWrapper = Reader.IsEmptyElement;
            Reader.ReadStartElement();
            Fixup fixup = new Fixup(p, new System.Xml.Serialization.XmlSerializationFixupCallback(this.fixup_Read1_insertarResponse), 1);
            AddFixup(fixup);
            IsReturnValue = true;
            bool[] paramsRead = new bool[1];
            Reader.MoveToContent();
            int whileIterations1 = 0;
            int readerCount1 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && (IsReturnValue || ((object) Reader.LocalName == (object)id1_respuesta && (object) Reader.NamespaceURI == (object)id2_Item))) {
                        object rre = ReadReferencingElement(id3_string, id4_Item, out fixup.Ids[0]);
                        try {
                            p[0] = (global::System.String)rre;
                        }
                        catch (System.InvalidCastException) {
                            throw CreateInvalidCastException(typeof(global::System.String), rre, null);
                        }
                        Referenced(p[0]);
                        IsReturnValue = false;
                        paramsRead[0] = true;
                    }
                    else {
                        UnknownNode((object)p);
                    }
                }
                else {
                    UnknownNode((object)p);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations1, ref readerCount1);
            }
            if (!isEmptyWrapper) ReadEndElement();
            ReadReferencedElements();
            return p;
        }

        void fixup_Read1_insertarResponse(object objFixup) {
            Fixup fixup = (Fixup)objFixup;
            object[] p = (object[])fixup.Source;
            string[] ids = fixup.Ids;
            if (ids[0] != null) {
                p[0] = GetTarget(ids[0]);
            }
        }

        public object[] Read2_insertarResponseOutHeaders() {
            Reader.MoveToContent();
            object[] p = new object[0];
            System.Collections.ArrayList hrefList = new System.Collections.ArrayList();
            System.Collections.ArrayList hrefListIsObject = new System.Collections.ArrayList();
            bool[] paramsRead = new bool[0];
            Reader.MoveToContent();
            int whileIterations2 = 0;
            int readerCount2 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (Reader.GetAttribute("root", "http://schemas.xmlsoap.org/soap/encoding/") == "0") {
                        if (Reader.GetAttribute("id", null) != null) { ReadReferencedElement(); } else { UnknownNode((object)p); } continue;
                    }
                    string refElemId = null;
                    object refElem = ReadReferencingElement(null, null, true, out refElemId);
                    if (refElemId != null) {
                        hrefList.Add(refElemId);
                        hrefListIsObject.Add(false);
                    }
                    else if (refElem != null) {
                        hrefList.Add(refElem);
                        hrefListIsObject.Add(true);
                    }
                }
                else {
                    UnknownNode((object)p);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations2, ref readerCount2);
            }
            int isObjectIndex = 0;
            foreach (object obj in hrefList) {
                bool isReferenced = true;
                bool isObject = (bool)hrefListIsObject[isObjectIndex++];
                object refObj = isObject ? obj : GetTarget((string)obj);
                if (refObj == null) continue;
                System.Type refObjType = refObj.GetType();
                string refObjId = null;
                isReferenced = false;
                if (isObject && isReferenced) Referenced(refObj); // need to mark this obj as ref'd since we didn't do GetTarget
            }
            ReadReferencedElements();
            return p;
        }

        protected override void InitCallbacks() {
        }

        string id2_Item;
        string id3_string;
        string id1_respuesta;
        string id4_Item;

        protected override void InitIDs() {
            id2_Item = Reader.NameTable.Add(@"");
            id3_string = Reader.NameTable.Add(@"string");
            id1_respuesta = Reader.NameTable.Add(@"respuesta");
            id4_Item = Reader.NameTable.Add(@"http://www.w3.org/2001/XMLSchema");
        }
    }

    public abstract class XmlSerializer1 : System.Xml.Serialization.XmlSerializer {
        protected override System.Xml.Serialization.XmlSerializationReader CreateReader() {
            return new XmlSerializationReaderwsdocumento();
        }
        protected override System.Xml.Serialization.XmlSerializationWriter CreateWriter() {
            return new XmlSerializationWriterwsdocumento();
        }
    }

    public sealed class ArrayOfObjectSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"insertar", @"urn:insertar");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriterwsdocumento)writer).Write1_insertar((object[])objectToSerialize);
        }
    }

    public sealed class ArrayOfObjectSerializer1 : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"insertarResponse", @"urn:insertar");
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReaderwsdocumento)reader).Read1_insertarResponse();
        }
    }

    public sealed class ArrayOfObjectSerializer2 : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"insertarInHeaders", @"urn:wsdocumento");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriterwsdocumento)writer).Write2_insertarInHeaders((object[])objectToSerialize);
        }
    }

    public sealed class ArrayOfObjectSerializer3 : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"insertarResponseOutHeaders", @"urn:wsdocumento");
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReaderwsdocumento)reader).Read2_insertarResponseOutHeaders();
        }
    }

    public class XmlSerializerContract : global::System.Xml.Serialization.XmlSerializerImplementation {
        public override global::System.Xml.Serialization.XmlSerializationReader Reader { get { return new XmlSerializationReaderwsdocumento(); } }
        public override global::System.Xml.Serialization.XmlSerializationWriter Writer { get { return new XmlSerializationWriterwsdocumento(); } }
        System.Collections.Hashtable readMethods = null;
        public override System.Collections.Hashtable ReadMethods {
            get {
                if (readMethods == null) {
                    System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
                    _tmp[@"ViaDocEnvioCorreo.Negocios.WsPortalFacturacionElectronica.wsdocumento:System.String insertar(System.String, System.String, System.String, System.String, System.String, System.String, System.String, System.String, System.String, System.String):Response"] = @"Read1_insertarResponse";
                    _tmp[@"ViaDocEnvioCorreo.Negocios.WsPortalFacturacionElectronica.wsdocumento:System.String insertar(System.String, System.String, System.String, System.String, System.String, System.String, System.String, System.String, System.String, System.String):OutHeaders"] = @"Read2_insertarResponseOutHeaders";
                    if (readMethods == null) readMethods = _tmp;
                }
                return readMethods;
            }
        }
        System.Collections.Hashtable writeMethods = null;
        public override System.Collections.Hashtable WriteMethods {
            get {
                if (writeMethods == null) {
                    System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
                    _tmp[@"ViaDocEnvioCorreo.Negocios.WsPortalFacturacionElectronica.wsdocumento:System.String insertar(System.String, System.String, System.String, System.String, System.String, System.String, System.String, System.String, System.String, System.String)"] = @"Write1_insertar";
                    _tmp[@"ViaDocEnvioCorreo.Negocios.WsPortalFacturacionElectronica.wsdocumento:System.String insertar(System.String, System.String, System.String, System.String, System.String, System.String, System.String, System.String, System.String, System.String):InHeaders"] = @"Write2_insertarInHeaders";
                    if (writeMethods == null) writeMethods = _tmp;
                }
                return writeMethods;
            }
        }
        System.Collections.Hashtable typedSerializers = null;
        public override System.Collections.Hashtable TypedSerializers {
            get {
                if (typedSerializers == null) {
                    System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
                    _tmp.Add(@"ViaDocEnvioCorreo.Negocios.WsPortalFacturacionElectronica.wsdocumento:System.String insertar(System.String, System.String, System.String, System.String, System.String, System.String, System.String, System.String, System.String, System.String)", new ArrayOfObjectSerializer());
                    _tmp.Add(@"ViaDocEnvioCorreo.Negocios.WsPortalFacturacionElectronica.wsdocumento:System.String insertar(System.String, System.String, System.String, System.String, System.String, System.String, System.String, System.String, System.String, System.String):OutHeaders", new ArrayOfObjectSerializer3());
                    _tmp.Add(@"ViaDocEnvioCorreo.Negocios.WsPortalFacturacionElectronica.wsdocumento:System.String insertar(System.String, System.String, System.String, System.String, System.String, System.String, System.String, System.String, System.String, System.String):InHeaders", new ArrayOfObjectSerializer2());
                    _tmp.Add(@"ViaDocEnvioCorreo.Negocios.WsPortalFacturacionElectronica.wsdocumento:System.String insertar(System.String, System.String, System.String, System.String, System.String, System.String, System.String, System.String, System.String, System.String):Response", new ArrayOfObjectSerializer1());
                    if (typedSerializers == null) typedSerializers = _tmp;
                }
                return typedSerializers;
            }
        }
        public override System.Boolean CanSerialize(System.Type type) {
            if (type == typeof(global::ViaDocEnvioCorreo.Negocios.WsPortalFacturacionElectronica.wsdocumento)) return true;
            return false;
        }
        public override System.Xml.Serialization.XmlSerializer GetSerializer(System.Type type) {
            return null;
        }
    }
}
