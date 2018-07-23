using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SfxTool
{
    public partial class Form1 : Form

    {
        string Ips = ConfigurationManager.AppSettings["PingIps"];
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length == 2 && args[1] == "/h")
            {
                Thread.Sleep(60000);
            }
            SetAutoRun();
            KillAll();
            Thread.Sleep(1000);
            startAll();
            this.notifyIcon1.ShowBalloonTip(2000, "提示12", "监控守护程序开始运行！", ToolTipIcon.Info);
            

        }

        private void 打开主窗体ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void 退出程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            hidderFiddler();
        }
        private void hidderFiddler()
        {
            var fiddler = win32.FindWindow(null, "Progress Telerik Fiddler Web Debugger");
            win32.ShowWindow(fiddler, win32.ShowWindowCommands.Hide);
        }
        private void showFiddler()
        {
            var fiddler = win32.FindWindow(null, "Progress Telerik Fiddler Web Debugger");
            win32.ShowWindow(fiddler, win32.ShowWindowCommands.Show);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            showFiddler();
        }
        private void startAll()
        {
            startFiddler();
            startPingIp();
        }
        private void startFiddler()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tool\\Fiddler2\\Fiddler.exe");
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(startInfo);
         

        }
        private void startPingIp()
        {
            string[] ips = Ips.Split(',');

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tool/SfxPing.exe");
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            foreach(var ip in ips)
            {
                startInfo.Arguments = " " + ip + " " + ip + ".txt" + " 10";
                Process.Start(startInfo);
            }


        }
        private void KillAll()
        {
            KillFiddler();
            KillPing();

        }
        private void KillPing()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"C:\Windows\System32\taskkill.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = "/im SfxPing.exe /t /f";
            Process.Start(startInfo);
      
        }
        private void KillFiddler()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"C:\Windows\System32\taskkill.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = "/im Fiddler.exe /t /f";
            Process.Start(startInfo);
        }
        private void SetAutoRun()
        {
            try
            {
                string path = Application.ExecutablePath;
                RegistryKey rk = Registry.LocalMachine;
                RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                rk2.SetValue("SfxTool", path + " /h");
                rk2.Close();
                rk.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("设置开机启动失败"+ex.Message);
            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
         
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.timer2.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RestartPing();
        }
        private void RestartPing()
        {
            KillPing();
            Thread.Sleep(1000);
            startPingIp();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            hidderFiddler();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.timer2.Start();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
