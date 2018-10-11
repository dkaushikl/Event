namespace EventApi.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Web;
    using System.Web.Hosting;

    public static class Utility
    {
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

        public static string GetCode()
        {
            var random = new Random();
            const string input = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, 10).Select(x => input[random.Next(0, input.Length)]);
            return new string(chars.ToArray());
        }

        public static T GetItem<T>(DataRow dr)
        {
            var temp = typeof(T);
            var obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            foreach (var pro in temp.GetProperties())
                if (pro.Name == column.ColumnName) pro.SetValue(obj, dr[column.ColumnName], null);
                else continue;

            return obj;
        }

        public static string PopulateForgotPasswordBody(string email, string code)
        {
            var body = string.Empty;
            using (var reader =
                new StreamReader(HttpContext.Current.Server.MapPath("~//Templates/ForgotPassword.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{Email}", email);
            body = body.Replace("{Code}", code);

            return body;
        }

        public static void SendForgotPasswordMail(string email, string verificationCode)
        {
            var url = HttpContext.Current.Request.Url.Authority;
            using (var mm = new MailMessage("elaunch.kaushik@gmail.com", email))
            {
                mm.Subject = "AmbryHill Forgot Password";
                mm.Body = "<table width='100%' border='0' cellspacing='0' cellpadding='0' style='margin-top:5px;'>"
                          + "<tr><td align='left' valign='top' style='font-family:Pacifico, Arial, Helvetica, sans-serif; font-size:13px; color:#000;'>"
                          + "<div style='font-size:28px; color:#669d89;'><br> Hello <span style='color:#f27c7e'> "
                          + email + "</span></div>"
                          + "<div> <br>A request to reset your password has been made on <a href='http://www.ambryhill.com' title='AmbryHill' alt='AmbryHill' target='_top'>http://www.ambryhill.com</a>,"
                          + " please follow the link below to reset your account password. If you have not requested a password reset please disregard this email.<br/><br>To reset your password:<br><br><a href='http://"
                          + url + "/Account/SetPassword?id=" + verificationCode + "'>http://" + url
                          + "/Account/SetPassword?id=" + verificationCode
                          + "</a><br/><br/>Kind Regards,<br/><br/>The Ambryhill Team.</div></td></tr></table>";
                mm.IsBodyHtml = true;
                var networkCred = new NetworkCredential("elaunch.kaushik@gmail.com", "Kaushik@elnch05#");
                var smtp = new SmtpClient
                               {
                                   Host = "smtp.gmail.com",
                                   EnableSsl = true,
                                   UseDefaultCredentials = true,
                                   Credentials = networkCred,
                                   Port = 587
                               };
                smtp.Send(mm);
            }
        }

        public static bool SendMail(string email, string subject, string message)
        {
            try
            {
                var mail = new MailMessage();
                var smtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("elaunch.kaushik@gmail.com");
                mail.To.Add(email);
                mail.IsBodyHtml = true;
                mail.Subject = subject;
                mail.Body = message;
                smtpServer.Port = 587;
                smtpServer.UseDefaultCredentials = false;
                smtpServer.Credentials = new NetworkCredential("elaunch.kaushik@gmail.com", "Kaushik@elnch05#");
                smtpServer.EnableSsl = true;
                smtpServer.Send(mail);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void WriteLog(string text)
        {
            text += Environment.NewLine;
            text += "-----------------------------------------------------------";

            text += "-----------------------------------------------------------";

            // ReSharper disable once AssignNullToNotNullAttribute
            using (var objStream = new StreamWriter(HostingEnvironment.MapPath("~/Log.txt"), true))
            {
                objStream.Write(
                    Environment.NewLine + " Inline Start  : " + text + " " + DateTime.Now.ToUniversalTime());
            }
        }
    }
}