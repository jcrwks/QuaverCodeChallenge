using System;
using System.Text;

namespace QuaverCodeChallenge.Utils
{
    public class Encryption
    {
        public string DecryptString(string strEncrypted)
        {
            byte[] b;
            string decrypted = "";
            try
            {
                b = Convert.FromBase64String(strEncrypted);
                decrypted = ASCIIEncoding.ASCII.GetString(b);
            }
            catch (FormatException)
            {
                decrypted = "";
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                // handle exception here
            }
            return decrypted;
        }

        public string EncryptString(string strEncrypted)
        {
            byte[] b = ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = "";
            try
            {
                encrypted = Convert.ToBase64String(b);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                // handle exception here
            }
            return encrypted;
        }
    }
}
