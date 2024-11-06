using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ViaDoc.Utilitarios
{
    public class Validacion
    {
        public DataTable dtRespuesta = new DataTable("Validacion");
        public DataTable dtCamposObligatorios = new DataTable("Validacion");
        public int contadorError = 0;
        private static CultureInfo cultureInfo;
        private static CultureInfo myCIclone;
        public Validacion()
        {
            //dtRespuesta.Columns.Add("nombreColumna");
            dtRespuesta.Columns.Add("Error");
            dtCamposObligatorios.Columns.Add("Error");
        }

        public bool validarCampos(ref string valor, string tipoDato, string nombreCampo, int tamanio, ref string descripcionRetorno)
        {
            bool respuesta = false;
            object valorValido = 0;
            try
            {
                if (valor.Length > tamanio)
                {
                    throw new ArgumentException("El campo excede los limites de caracteres permitidos");
                }
                validarCaracterEspecial(ref valor, ref descripcionRetorno);//Valida Caracteres Especiales
                respuesta = true;
            }
            catch (Exception ex)
            {
                contadorError++;
                if (tipoDato == "string")
                {
                    descripcionRetorno = nombreCampo + " - " + ex.Message;
                }
                if (tipoDato == "char")
                {
                    descripcionRetorno = nombreCampo + " - " + ex.Message;
                }

            }
            return respuesta;
        }

        public bool validarCamposChar(ref string valor, string nombreCampo, int tamanio, bool flag, ref string descripcionRetorno)
        {
            bool respuesta = false;
            string valorTemp = "";
            try
            {
                char[] caracterValor = valor.ToArray();
                int j = 0;
                if (flag)//Si es verdadero agrega los 0
                {
                    if (validarSoloNumeros(valor, ref descripcionRetorno))
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
                    validarSoloNumeros(valor, ref descripcionRetorno);//si da error NO es un Numero válido
                    respuesta = true;// Numero correcto
                }

            }
            catch (Exception ex)
            {
                contadorError++;
                descripcionRetorno = nombreCampo + " - " + ex.Message;
            }
            return respuesta;
        }

        public void agregarCamposObligatorios(string nombreCampo, ref string descripcionRetorno)
        {
            descripcionRetorno = nombreCampo + " - " + "Este campo es obligatorio";
        }

        public void validarCaracterEspecial(ref string valor, ref string descripcionRetorno)
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
                            if (caracter.ToString().Trim().Equals(caracterPermitido.ToString().Trim()))
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
                descripcionRetorno = ex.Message;
            }
        }

        public bool validarSoloNumeros(string valor, ref string descripcionRetorno)
        {
            char[] caracteres = valor.ToArray();
            foreach (char caracter in caracteres)
            {
                if (!(caracter >= 48 && caracter <= 57))
                {    //si da error NO es un Numero válido
                    descripcionRetorno = "El campo solo permite números";
                }
            }

            return true; // Numero correcto
        }

        public void ValidarFormatoFecha(ref string input, string nombreCampo, ref string descripcionRetorno)
        {
            try
            {
                string Result = "";

                string[] patterns = {"\\b(?<day>\\d{1,2})/(?<month>\\d{1,2})/(?<year>\\d{2,4})\\b",
                                     "\\b(?<year>\\d{2,4})/(?<day>\\d{1,2})/(?<month>\\d{1,2})\\b"};

                int cont = 0;
                foreach (string pattern in patterns)
                {
                    if (cont == 0)
                        Result = Regex.Replace(input, pattern, "${day}/${month}/${year}", RegexOptions.None);

                    if (String.Compare(Result, input) == 0)
                    {
                        Result = Regex.Replace(input, pattern, "${day}/${month}/${year}", RegexOptions.None);
                    }

                    cont++;
                }

                input = Result;

            }
            catch (Exception ex)
            {
                contadorError++;
                descripcionRetorno = nombreCampo + " - " + ex.Message;

            }
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
    }
}
