using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vnpt.iLIS.SignThueXMLApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            Vnpt.iLIS.Common.Controls.CommonMain.InitGlobalVariables();
            Vnpt.iLIS.Common.Controls.CommonMain.CommonGlobalVariables.ModuleKey = "Vnpt.iLIS.SignThueXMLApp";
            Vnpt.iLIS.Common.Controls.CommonMain.CommonGlobalVariables.RecentFolder.DataFolderRoot = System.IO.Path.Combine(Application.StartupPath, "Data");


            Vnpt.iLIS.Common.Controls.CommonMain.CommonGlobalVariables.RecentFolder.AppDataFolderRoot = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Vnpt", "iLIS");

            Vnpt.iLIS.Common.Controls.CommonMain.CommonGlobalVariables.RecentFolder.TemporaryFolderRoot = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "Vnpt", "iLIS");
            if (!System.IO.Directory.Exists(Vnpt.iLIS.Common.Controls.CommonMain.CommonGlobalVariables.RecentFolder.AppDataFolderRoot))
                System.IO.Directory.CreateDirectory(Vnpt.iLIS.Common.Controls.CommonMain.CommonGlobalVariables.RecentFolder.AppDataFolderRoot);
            if (!System.IO.Directory.Exists(Vnpt.iLIS.Common.Controls.CommonMain.CommonGlobalVariables.RecentFolder.TemporaryFolderRoot))
                System.IO.Directory.CreateDirectory(Vnpt.iLIS.Common.Controls.CommonMain.CommonGlobalVariables.RecentFolder.TemporaryFolderRoot);

            //var args = "";
            //if (Environment.GetCommandLineArgs().Length > 1)
            //    args = Environment.GetCommandLineArgs()[1];
            //var lstParam = GetParamFromUrl(args);
            ////foreach (var param in lstParam)
            ////{
            ////    MessageBox.Show(param);
            ////}
            //if (lstParam != null && lstParam.Count == 2)
            //{
            //    Application.Run(new FormSignXmlThue(lstParam[0], lstParam[1]));
            //}
            //else
            //{
            //    MessageBox.Show($"URL không đúng định dạng, vui lòng kiểm tra lại");
            //    System.Windows.Forms.Application.Exit();
            //}
            Application.Run(new FormSignXmlThue());
        }
        public static List<string> GetParamFromUrl(string url)
        {
            if (url != null)
            {
                var tmp = url.Split(':');
                if (tmp.Length == 2)
                {
                    var paramss = tmp[1].Split('&');
                    var lstResult = new List<string>();
                    foreach (var param in paramss)
                    {
                        lstResult.Add(param);
                    }
                    return lstResult;
                }
                else
                {
                    return null;
                }
            }
            else return null;
        }
    }
    
}
