namespace EventApi.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
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
            {
                foreach (var pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    }
                    else
                    {
                        // ReSharper disable once RedundantJumpStatement
                        continue;
                    }
                }
            }

            return obj;
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