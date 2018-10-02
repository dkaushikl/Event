using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Hosting;
using Newtonsoft.Json;

namespace EventApi.Utility
{
    public static class Utility
    {
        public static T DeserializeObject<T>(string s)
        {
            return (T)JsonConvert.DeserializeObject(s, typeof(T));
        }

        //public static void SendMail(MailModel objModelMail)
        //{
        //    try
        //    {
        //        var mail = new MailMessage();
        //        mail.To.Add(objModelMail.To);
        //        mail.From = new MailAddress(ConfigurationManager.AppSettings["mailAccount"]);
        //        mail.Subject = objModelMail.Subject;
        //        mail.Body = objModelMail.Body;
        //        mail.IsBodyHtml = true;
        //        var smtp = new SmtpClient
        //        {
        //            Host = "smtp.gmail.com",
        //            Port = 587,
        //            EnableSsl = true,
        //            UseDefaultCredentials = false,
        //            Credentials = new NetworkCredential
        //            (ConfigurationManager.AppSettings["mailAccount"],
        //                ConfigurationManager.AppSettings["mailPassword"])
        //        };
        //        smtp.Send(mail);
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog(ex.Message);
        //    }
        //}

        public static void WriteLog(string text)
        {
            text += Environment.NewLine;
            text += "-----------------------------------------------------------";

            text += "-----------------------------------------------------------";
            // ReSharper disable once AssignNullToNotNullAttribute
            using (var objStream = new StreamWriter(HostingEnvironment.MapPath("~/Log.txt"), true))
            {
                objStream.Write(Environment.NewLine + " Inline Start  : " + text + " " +
                                DateTime.Now.ToUniversalTime());
            }
        }

        public static string Encrypt(string clearText)
        {
            const string encryptionKey = "JustBlog";
            var clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (var encryptor = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(encryptionKey,
                    new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                if (encryptor == null) return clearText;
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }

                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }

            return clearText;
        }

        public static string Decrypt(string cipherText)
        {
            const string encryptionKey = "JustBlog";
            var cipherBytes = Convert.FromBase64String(cipherText);
            using (var encryptor = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(encryptionKey,
                    new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                if (encryptor == null) return cipherText;
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }

                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            return cipherText;
        }

        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            var data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                var item = GetItem<T>(row);
                data.Add(item);
            }

            return data;
        }

        public static T GetItem<T>(DataRow dr)
        {
            var temp = typeof(T);
            var obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
                foreach (var pro in temp.GetProperties())
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        // ReSharper disable once RedundantJumpStatement
                        continue;
            return obj;
        }
    }
}