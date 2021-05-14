using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using Microsoft.Win32;
using Vnpt.iLIS.SignThueXMLApp.Helpers;

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
            //RegisterMyProtocol();

            Vnpt.iLIS.Common.Controls.CommonMain.InitGlobalVariables();
            Vnpt.iLIS.Common.Controls.CommonMain.CommonGlobalVariables.ModuleKey = "Vnpt.iLIS.SignThueXMLApp";
            Vnpt.iLIS.Common.Controls.CommonMain.CommonGlobalVariables.RecentFolder.DataFolderRoot = System.IO.Path.Combine(Application.StartupPath, "Data");


            Vnpt.iLIS.Common.Controls.CommonMain.CommonGlobalVariables.RecentFolder.AppDataFolderRoot = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Vnpt", "iLIS");

            Vnpt.iLIS.Common.Controls.CommonMain.CommonGlobalVariables.RecentFolder.TemporaryFolderRoot = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "Vnpt", "iLIS");
            if (!System.IO.Directory.Exists(Vnpt.iLIS.Common.Controls.CommonMain.CommonGlobalVariables.RecentFolder.AppDataFolderRoot))
                System.IO.Directory.CreateDirectory(Vnpt.iLIS.Common.Controls.CommonMain.CommonGlobalVariables.RecentFolder.AppDataFolderRoot);
            if (!System.IO.Directory.Exists(Vnpt.iLIS.Common.Controls.CommonMain.CommonGlobalVariables.RecentFolder.TemporaryFolderRoot))
                System.IO.Directory.CreateDirectory(Vnpt.iLIS.Common.Controls.CommonMain.CommonGlobalVariables.RecentFolder.TemporaryFolderRoot);

            MessageBoxManager.Yes = "Đồng ý";
            MessageBoxManager.No = "Không";
            MessageBoxManager.Register();

            string[] args = Environment.GetCommandLineArgs();

            var lstParam = GetParamFromUrl(args);
            if (lstParam != null && lstParam.Count == 2)
            {
                Application.Run(new FormSignXmlThue(lstParam[0], lstParam[1]));
            }
            else
            {
                MessageBox.Show($"URL không đúng định dạng, vui lòng kiểm tra lại");
                System.Windows.Forms.Application.Exit();
            }
            Application.Run(new FormSignXmlThue());
        }
        public static List<string> GetParamFromUrl(string[] args)
        {
            if (args != null)
            {
                var lstResult = new List<string>();
                args[1] = args[1].Replace("kyso://", string.Empty);
                args[1] = System.Uri.UnescapeDataString(args[1]);
                args[1] = (args[1].EndsWith("/")) ? args[1].TrimEnd('/') : args[1];
                Console.WriteLine(args[1]);
                var query = HttpUtility.ParseQueryString(args[1]);
                Console.WriteLine(query);

                var token = query.Get("token");
                var idbiendong = query.Get("idbiendong");
                lstResult.Add(token);
                lstResult.Add(idbiendong);
                return lstResult;
            }
            return null;
        }

        static void RegisterMyProtocol()  //myAppPath = full path to your application
        {
            RegistryKey key = Registry.ClassesRoot.OpenSubKey("kyso");  

            if (key == null)  //if the protocol is not registered yet...we register it
            {
                key = Registry.ClassesRoot.CreateSubKey("kyso");
                key.SetValue(string.Empty, "URL: kyso Protocol");
                key.SetValue("URL Protocol", string.Empty);

                key = key.CreateSubKey(@"shell\open\command");
                //key.SetValue(string.Empty, myAppPath + " " + "%1");
                key.SetValue("", Environment.GetCommandLineArgs()[0].Replace("dll", "exe") + " " + "%1");
                //%1 represents the argument - this tells windows to open this program with an argument / parameter
            }

            key.Close();
        }
    }

}
