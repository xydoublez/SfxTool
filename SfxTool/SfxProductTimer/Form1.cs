using OpenQA.Selenium.IE;
using SQLite.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SfxProductTimer
{
    public partial class Form1 : Form
    {
        string dataSource = "data source=SfxFiddlerRule.db";
        string HISIP = System.Configuration.ConfigurationManager.AppSettings["HISIP"];
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var rule = cbRules.SelectedItem as SfxFiddlerRule;
            test(rule.StartUrl,rule.EndUrl);
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
            
            createTable2();
            LoadRules();
        }
        private void createTable2()
        {
            using (var conn = new SQLiteConnection(dataSource))
            {
                using (var cmd = new SQLiteCommand())
                {
                    cmd.Connection = conn;
                    conn.Open();
                    var sh = new SQLiteHelper(cmd);
                    if (!sh.ExistsTable("SfxFiddlerRule"))
                    {

                        var tb = new SQLiteTable("SfxFiddlerRule");
                        tb.Columns.Add(new SQLiteColumn("Id", true));
                        tb.Columns.Add(new SQLiteColumn("RuleName", ColType.Text));
                        tb.Columns.Add(new SQLiteColumn("StartUrl", ColType.Text));
                        tb.Columns.Add(new SQLiteColumn("EndUrl", ColType.Text));
                        tb.Columns.Add(new SQLiteColumn("StartKeyword", ColType.Text));
                        tb.Columns.Add(new SQLiteColumn("EndKeyword", ColType.Text));
                        tb.Columns.Add(new SQLiteColumn("Module", ColType.Text));
                        tb.Columns.Add(new SQLiteColumn("Version", ColType.Text));
                        tb.Columns.Add(new SQLiteColumn("InsertTime", ColType.DateTime));
                        sh.CreateTable(tb);
                        conn.Close();
                    }
                }
            }
        }
        private void LoadRules()
        {
            using (var conn = new SQLiteConnection(dataSource))
            {
                using (var cmd = new SQLiteCommand())
                {
                    cmd.Connection = conn;
                    conn.Open();
                    var sh = new SQLiteHelper(cmd);
                    var dt = sh.Select("select * from SfxFiddlerRule");
                    List<SfxFiddlerRule> list = new List<SfxFiddlerRule>();
                    foreach(DataRow row in dt.Rows)
                    {
                        SfxFiddlerRule rule = new SfxFiddlerRule();
                        rule.Id = int.Parse(row["Id"].ToString());
                        rule.RuleName = row["RuleName"].ToString();
                        rule.StartUrl = row["StartUrl"].ToString();
                        rule.EndUrl = row["EndUrl"].ToString();
                        rule.Module = row["Module"].ToString();
                        rule.Version = row["Version"].ToString();
                        rule.InsertTime = DateTime.Parse(row["InsertTime"].ToString());
                        rule = replaceIP(rule);
                        list.Add(rule);

                    }
                    cbRules.DataSource = list;
                    cbRules.DisplayMember = "RuleName";
                    cbRules.ValueMember = "RuleName";
                }
            }
        }
        private SfxFiddlerRule replaceIP(SfxFiddlerRule rule)
        {
            rule.StartUrl = rule.StartUrl.Replace("{HISIP}", HISIP);
            rule.EndUrl = rule.EndUrl.Replace("{HISIP}", HISIP);
            return rule;
        }
        private InternetExplorerDriver openUrl(string url)
        {
           InternetExplorerOptions options = new InternetExplorerOptions();
            options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
            InternetExplorerDriver ie = new InternetExplorerDriver(options);
            {
                System.Environment.SetEnvironmentVariable("webdriver.ie.driver", String.Format(@"{0}\IEDriverServer.exe", System.IO.Directory.GetCurrentDirectory()));
                //ie.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(5);

            }
        
            ie.Url = url;
            return ie;
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void log(string messgage,string action)
        {
            string info = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" + action + "\r\n";
            info += messgage + "\r\n";
            this.Invoke(new Action(() => {
                this.rbResult.AppendText(info);
            }));
        }
        private void insertTable(SfxFiddlerRule rule)
        {

            //var dic = new Dictionary<string, object>() {
            //    { "url" ,session.fullUrl },
            //    { "referer", session.oRequest["referer"] },
            //    { "status", session.responseCode},
            //    { "duration", (int)duration },
            //    { "FiddlerId", session.id },
            //    { "requestContent", session.GetRequestBodyAsString() },
            //    { "responseContent", "" },
            //    { "keyword", "" },
            //    { "insertTime", DateTime.Now },
            //    { "ServerGotRequestTime",session.Timers.ServerGotRequest },
            //    { "ServerDoneResponseTime",session.Timers.ServerDoneResponse }
            //};
            //using (var conn = new SQLiteConnection(dataSource))
            //{
            //    using (var cmd = new SQLiteCommand())
            //    {
            //        cmd.Connection = conn;
            //        conn.Open();
            //        var sh = new SQLiteHelper(cmd);
            //        sh.Insert("", dic);
            //    }
            //}

        }
    }

    public class SfxFiddlerRule
    {
        public  int Id { get; set; }
        public string RuleName { get; set; }
        public string StartUrl { get; set; }
        public string EndUrl { get; set; }
        public string StartKeyword { get; set; }
        public string EndKeyword { get; set; }
        public string Module { get; set; }
        public string Version { get; set; }
        public DateTime InsertTime { get; set; }

    }
}
