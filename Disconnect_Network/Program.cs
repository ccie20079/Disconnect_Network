using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Tools;
namespace Disconnect_Network
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            RegistryHelper.add_To_Boot_Options(Application.ProductName, Application.ExecutablePath);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FrmMain frmMain = new FrmMain();
            Application.Run(frmMain);
        }
    }
}
