using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace ViaDoc.LogicaNegocios.certificado
{
    public class AlgoritmoRijndael
    {
        Rijndael rijndael = Rijndael.Create();
        //=> http://blog.jorgeivanmeza.com/2010/10/cifrado-y-descifrado-simetrico-con-rijndael-aes-utilizando-cmono/

        public byte[] key()
        {
            byte[] key = rijndael.Key;
            return key;
        }

        //vector ok
        public byte[] iv()
        {
            byte[] iv = rijndael.IV;
            return iv;
        }

        public static String encryptString(String plainMessage, String Key, ref byte[] IV)
        {
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

                CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                             RijndaelAlg.CreateEncryptor(key_, IV),
                                                             CryptoStreamMode.Write);

                byte[] plainMessageBytes = UTF8Encoding.UTF8.GetBytes(plainMessage);

                cryptoStream.Write(plainMessageBytes, 0, plainMessageBytes.Length);

                cryptoStream.FlushFinalBlock();

                byte[] cipherMessageBytes = memoryStream.ToArray();

                memoryStream.Close();
                cryptoStream.Close();

                StrClave = Convert.ToBase64String(cipherMessageBytes);
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());

                StrClave = "";
            }

            return StrClave;
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
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                StrClaveDecrypt = "";
            }
            return StrClaveDecrypt;
        }


        public string DescencriptartxtClaveEncrypt(string IvTextWS, string txtClaveEncrypt, string keyUisemilla)
        {
            string txtclaveDes = "";
            try
            {
                byte[] vector = Convert.FromBase64String(IvTextWS);
                txtclaveDes = AlgoritmoRijndael.decryptString(txtClaveEncrypt, keyUisemilla, vector);
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                txtclaveDes = "";

            }
            return txtclaveDes;
        }
    }
}
