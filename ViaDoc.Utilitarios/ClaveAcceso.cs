using System;

namespace ViaDoc.Utilitarios
{
    public class ClaveAcceso
    {
        public string GenerarClaveAccesoDocumento(string ciTipoDocumento, string txSecuencial, string txPuntoEmision, string txEstablecimiento,
                                                  string txFechaEmision, string ruc, string ciAmbiente)
        {
            string claveAcceso = String.Empty;
            Utilitarios util = new Utilitarios();

            claveAcceso = txFechaEmision.Replace("/", "").Replace("-","");
            claveAcceso += ciTipoDocumento;
            claveAcceso += ruc.Trim();
            claveAcceso += ciAmbiente.Trim();
            claveAcceso += txEstablecimiento + "" + txPuntoEmision;
            claveAcceso += txSecuencial.ToString();
            claveAcceso += NumeroAletorio();
            claveAcceso += "1";
            claveAcceso += util.CalculaDigitoVerificador(claveAcceso);
            return claveAcceso;
        }

        public string NumeroAletorio()
        {
            Random r = new Random();
            var ramdon = r.Next(0, 99999999).ToString();
            ramdon = ramdon.PadLeft(8, '0');
            return ramdon.ToString();
        }
    }
}