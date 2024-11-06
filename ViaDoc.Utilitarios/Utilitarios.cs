using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.Utilitarios
{
    public class Utilitarios
    {
        public string validaTipoIdentificacion(string ciTipoIdComprador)
        {
            string respuesta = String.Empty;
            if (ciTipoIdComprador.Equals("R"))
                respuesta = "04";
            else if (ciTipoIdComprador.Equals("CI"))
                respuesta = "05";
            else if (ciTipoIdComprador.Equals("PA"))
                respuesta = "06";
            else respuesta = ciTipoIdComprador;

            return respuesta;

        }


        public string validarDecimales(string valor, int Numdecimal)
        {
            string respuesta = String.Empty;

            decimal valorDecimal = Convert.ToDecimal(valor);
            respuesta = Convert.ToString(decimal.Round(valorDecimal, Numdecimal)).Replace(',', '.').Trim();

            return respuesta;
        }


        public string correoElectronico(string correo, string correoVendedor)
        {
            string respuesta = String.Empty;

            if (!String.IsNullOrEmpty(correoVendedor))
                respuesta = correo + "," + correoVendedor;
            else
                respuesta = correo;

            return respuesta;
        }


        public string CalculaDigitoVerificador(string txClaveAcceso)
        {
            int digitoVerificador = -1;
            try
            {
                char[] arrDigitosRuc = txClaveAcceso.ToCharArray();
                int[] arrResultado = new int[0];
                int cont = 2;
                decimal resultado = 0;

                for (int i = (txClaveAcceso.Length - 1); i >= 0; i--)
                {

                    int[] temp = new int[(txClaveAcceso.Length) - i];
                    if (arrResultado != null)
                        Array.Copy(arrResultado, temp, Math.Min(arrResultado.Length, temp.Length));
                    arrResultado = temp;

                    arrResultado[arrResultado.Length - 1] = Convert.ToInt32(arrDigitosRuc[i].ToString()) * cont;
                    cont += 1;
                    if (cont == 8)
                    {
                        cont = 2;
                    }
                }
                resultado = arrResultado.Sum() % 11;
                resultado = 11 - resultado;

                switch (Convert.ToInt32(resultado))
                {
                    case 10:
                        digitoVerificador = 1;
                        break;
                    case 11:
                        digitoVerificador = 0;
                        break;
                    default:
                        digitoVerificador = Convert.ToInt32(resultado);
                        break;
                }
            }
            catch (Exception ex)
            {

            }
            return digitoVerificador.ToString();
        }


        public bool validarCamposChar(ref string valor, string nombreCampo, int tamanio, bool flag)
        {
            bool respuesta = false;
            string valorTemp = "";
            try
            {
                char[] caracterValor = valor.ToArray();
                int j = 0;
                if (flag)//Si es verdadero agrega los 0
                {
                    if (validarSoloNumeros(valor))
                    {
                        for (int i = 0; i < tamanio; i++)
                        {
                            if (i > ((tamanio - 1) - valor.Length))
                            {
                                valorTemp += caracterValor[j++];
                            }
                            else
                            {
                                valorTemp += "0";
                            }
                        }
                    }
                    valor = valorTemp;
                }
                else
                {
                    validarSoloNumeros(valor);//si da error NO es un Numero válido
                    respuesta = true;// Numero correcto
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
            }
            return respuesta;
        }


        public bool validarCampos(ref string valor, string tipoDato, string nombreCampo, int tamanio)
        {
            bool respuesta = false;
            object valorValido = 0;
            try
            {
                switch (tipoDato)
                {
                    case "int":
                        valorValido = Convert.ToInt32(valor);//si da error NO es un Numero válido
                        respuesta = true;
                        break;
                    case "string":

                        if (valor.Length > tamanio)
                        {
                            throw new ArgumentException("El campo excede los limites de caracteres permitidos");
                        }
                        validarCaracterEspecial(ref valor);//Valida Caracteres Especiales
                        respuesta = true;
                        break;
                    case "decimal":
                        valorValido = Convert.ToDecimal(valor);//si da error NO es un Decimal válido
                        respuesta = true;
                        break;
                    case "char":
                        valorValido = Convert.ToInt32(valor);//si da error NO es un Numero válido
                        respuesta = true;
                        break;
                }

            }
            catch (Exception ex)
            {
               

            }
            return respuesta;
        }


        public bool validarSoloNumeros(string valor)
        {
            char[] caracteres = valor.ToArray();
            foreach (char caracter in caracteres)
            {
                if (!(caracter >= 48 && caracter <= 57))
                {    //si da error NO es un Numero válido
                    throw new ArgumentException("El campo solo permite números");
                }
            }

            return true; // Numero correcto
        }


        public void validarCaracterEspecial(ref string valor)
        {
            try
            {
                char[] caracteres = valor.ToArray();
                char[] caracteresPermitidos = new char[] { '*', ' ', '_', '-', '/', 'Á', 'É', 'Í', 'Ó', 'Ú', 'á', 'é', 'í', 'ó', 'ú', '.', ',', ':', '#', '@', '%', 'à', 'è', 'ì', 'ò', 'ù', 'À', 'È', 'Ì', 'Ò', 'Ù' };
                string valortemp = "";
                foreach (char caracter in caracteres)
                {
                    if ((caracter >= 48 && caracter <= 57)/*Numeros*/ || (caracter >= 65 && caracter <= 90)/*letras minusculas*/
                        || (caracter >= 97 && caracter <= 122)/*Letras Mayusculas*/)
                    {
                        valortemp += caracter;
                    }
                    else
                    {
                        //Caracteres Especiales Permitidos
                        foreach (char caracterPermitido in caracteresPermitidos)
                        {
                            if (caracter.Equals(caracterPermitido))
                            {
                                valortemp += caracter;
                                break;
                            }
                        }
                        if (caracter.Equals('Ñ'))
                        {
                            valortemp += 'N';
                        }
                        if (caracter.Equals('ñ'))
                        {
                            valortemp += 'n';
                        }
                    }
                }
                valor = valortemp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string Convertir_MD5(string clave)
        {
            byte[] arreglo_clave;
            string clave_encriptada = "";
            MD5CryptoServiceProvider objmd5 = new MD5CryptoServiceProvider();

            arreglo_clave = System.Text.Encoding.ASCII.GetBytes(clave);
            arreglo_clave = objmd5.ComputeHash(arreglo_clave);

            for (int i = 0; i < arreglo_clave.Length; i++)
            {
                clave_encriptada += arreglo_clave[i].ToString("x2").ToLower();
            }
            return clave_encriptada;
        }

        public static string ReemplazarCaracteresEspeciales(string xmlContent)
        {
            xmlContent = xmlContent.Replace("&lt;br&gt;", " ");
            xmlContent = xmlContent.Replace("&amp;", "&");
            xmlContent = xmlContent.Replace("&quot;", "");
            xmlContent = xmlContent.Replace("&apos;", "");
            xmlContent = xmlContent.Replace("&#x00D1;", "Ñ");
            xmlContent = xmlContent.Replace("&#x00F1;", "ñ");
            xmlContent = xmlContent.Replace("&#x00C1;", "Á");
            xmlContent = xmlContent.Replace("&#x00E1;", "á");
            xmlContent = xmlContent.Replace("&#x00C9;", "É");
            xmlContent = xmlContent.Replace("&#x00E9;", "é");
            xmlContent = xmlContent.Replace("&#x00CD;", "Í");
            xmlContent = xmlContent.Replace("&#x00ED;", "í");
            xmlContent = xmlContent.Replace("&#x00D3;", "Ó");
            xmlContent = xmlContent.Replace("&#x00F3;", "ó");
            xmlContent = xmlContent.Replace("&#x00DA;", "Ú");
            xmlContent = xmlContent.Replace("&#x00FA;", "ú");

            return xmlContent;
        }
    }
}
