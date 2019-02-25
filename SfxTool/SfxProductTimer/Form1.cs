using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SfxProductTimer
{
    public partial class Form1 : Form
    {
        InternetExplorerDriver ie;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            test("http://10.68.4.118/MZSK/Cash/Cash.aspx?popedomSystemId=1&userSysId=109101969&workstationId=-1&tsCheck=63686697515.732&rtime=1551071925304", "http://10.68.4.118/MZSK/AjaxServer/ReCash.aspx?para=GetAmount&userSysId=109101969&workstationId=-1&popedomSystemId=1&ie6=sb0.352377044457666");
        }
        bool flagQuit = false;
       private void test(string initUrl,string finishUrl)
        {
            //测试开始
            //打开初始页面
            var ie = openUrl(initUrl);

            //测试结束退出 
            if (flagQuit)
            {
                ie.Quit();
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private InternetExplorerDriver openUrl(string url)
        {
           InternetExplorerOptions options = new InternetExplorerOptions();
            options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
            InternetExplorerDriver ie = new InternetExplorerDriver(options);
            {
                System.Environment.SetEnvironmentVariable("webdriver.ie.driver", String.Format(@"{0}\IEDriverServer.exe", System.IO.Directory.GetCurrentDirectory()));
                ie.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(5);

            }
        
            ie.Url = url;
            return ie;
            
        }
    }
}
