using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Tools;
namespace Disconnect_Network
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }
        private void FrmMain_Paint(object sender, PaintEventArgs e)
        {
            this.Hide();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerCheckTime_Tick(object sender, EventArgs e)
        {
            //判断是否到了晚上 21:25分
            DateTime dtNow = DateTime.Now;
            if ((dtNow.Hour == 21 && dtNow.Minute >= 55) || dtNow.Hour > 21 || (dtNow.Hour >= 0 && dtNow.Hour < 5))
            {
                CmdHelper.runCmd("arp -d");
                //每隔10分钟，绑定一次。
                add_static_arp(getIdx(), GetGWIpAddr(), "00-00-00-00-00-00");
            }
            else
            {
                CmdHelper.runCmd("arp -d");
                //正确的arp
                add_static_arp(getIdx(), GetGWIpAddr(), GetGWMacAddr());
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetGWMacAddr()
        {
            string path = Application.StartupPath + "\\arp_a.txt";
            string gwIPAddr = GetGWIpAddr();
            CmdHelper.runCmd(string.Format(@"ping {0}", gwIPAddr));
            CmdHelper.runCmd(string.Format(@"arp -a > {0}", path));
            string contentOfArpInfo = FileHelper.readFile(path);
            contentOfArpInfo = new Regex("[\\s]+").Replace(contentOfArpInfo, ",");
            string[] contentOfArpInfoArray = contentOfArpInfo.Split(',');
            for (int i = 0; i <= contentOfArpInfoArray.Length - 1; i++)
            {
                if (contentOfArpInfoArray[i].Contains(gwIPAddr))
                {
                    return contentOfArpInfoArray[i + 1].ToString();
                }
            }
            return "";
        }
        private string getMacAddr()
        {
            IList<String> iListStr = NetHelper.GetMacsByNetworkInterface();
            return iListStr[0];
        }
        private string getIpAddr()
        {
            return NetHelper.getIPAddr();
        }
        private string getIdx()
        {
            string path = Application.StartupPath + "\\netsh.txt";
            CmdHelper.runCmd(string.Format(@"netsh -c i i show in  >{0}", path));
            string strContent = FileHelper.readFile(path);
            // richTextBox2.Text = new Regex("[\\s]+").Replace(ss, ",");
            strContent = new Regex("[\\s]+").Replace(strContent, ",");
            string[] strArray = strContent.Split(',');
            string resultIdx;
            for (int i = 0; i <= strArray.Length - 1; i++)
            {
                if (strArray[i].ToString().Contains("本地"))
                {
                    resultIdx = strArray[i - 4].ToString();
                    return resultIdx;
                }
            }
            return "";
        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            //先删除所有arp缓存.
            CmdHelper.runCmd("arp -d");
            //正确的arp
            add_static_arp(getIdx(), GetGWIpAddr(), GetGWMacAddr());
            //开启计时器，检查时间。
            timerCheckTime.Start();
        }
        private string GetGWIpAddr()
        {
            return NetworkHelper.GetGateway();
        }
        /// <summary>
        /// 绑定arp
        /// </summary>
        private void add_static_arp(string idx, string ipaddress, string macAddress)
        {
            string cmdStr = string.Format(@"netsh -c i i add neighbors {0} {1} {2}", idx, ipaddress, macAddress);
            CmdHelper.runCmd(cmdStr);
        }
        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            timerCheckTime.Stop();
        }
    }
}
