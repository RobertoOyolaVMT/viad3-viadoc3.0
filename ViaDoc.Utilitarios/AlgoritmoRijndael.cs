using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.Utilitarios
{
    public class AlgoritmoRijndael
    {
        public static String encryptString(String plainMessage, String Key, ref byte[] IV)
        {
            // Crear una instancia del algoritmo de Rijndael
            String StrClave = "";
            try
            {
                Rijndael RijndaelAlg = Rijndael.Create();
                byte[] key_ = System.Text.Encoding.UTF8.GetBytes(Key);
                if (IV == null || IV.Length.Equals(0))
                {
                    IV = RijndaelAlg.IV;
                }
                MemoryStream memoryStream = new MemoryStream();
                // Crear un flujo de cifrado basado en el flujo de los datos
                CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                             RijndaelAlg.CreateEncryptor(key_, IV),
                                                             CryptoStreamMode.Write);
                // Obtener la representación en bytes de la información a cifrar
                byte[] plainMessageBytes = UTF8Encoding.UTF8.GetBytes(plainMessage);
                // Cifrar los datos enviándolos al flujo de cifrado
                cryptoStream.Write(plainMessageBytes, 0, plainMessageBytes.Length);
                cryptoStream.FlushFinalBlock();
                byte[] cipherMessageBytes = memoryStream.ToArray();
                memoryStream.Close();
                cryptoStream.Close();
                // Retornar la representación de texto de los datos cifrados
                StrClave = Convert.ToBase64String(cipherMessageBytes);
            }
            catch (Exception ex)
            {
                StrClave = "";
            }
            return StrClave;
        }


        public string DescencriptartxtClaveEncrypt(string IvTextWS, string txtClaveEncrypt, string keyUisemilla)
        {
            string txtclaveDes = "";
            try
            {
                byte[] vector = Convert.FromBase64String(IvTextWS);
                txtclaveDes = decryptString(txtClaveEncrypt, keyUisemilla, vector);
            }
            catch (Exception ex)
            {
                
            }
            return txtclaveDes;
        }



        public static String decryptString(String encryptedMessage, String Key, byte[] IV)
        {
            String StrClaveDecrypt = "";
            try
            {
                byte[] cipherTextBytes = Convert.FromBase64String(encryptedMessage);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                Rijndael RijndaelAlg = Rijndael.Create();
                byte[] key_ = System.Text.Encoding.UTF8.GetBytes(Key);
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
                CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                             RijndaelAlg.CreateDecryptor(key_, IV),
                                                             CryptoStreamMode.Read);         
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                StrClaveDecrypt = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }
            catch (Exception ex)
            {
                StrClaveDecrypt = "";
            }
            return StrClaveDecrypt;
        }




        
    }
}
