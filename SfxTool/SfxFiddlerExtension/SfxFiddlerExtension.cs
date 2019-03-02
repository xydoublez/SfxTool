using Fiddler;
using SQLite.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class SfxFiddlerExtension : IAutoTamper // Ensure class is public, or Fiddler won't see it!
{
    string sUserAgent = "";
    static string databaseName = "SfxFiddlerData.db";
    static string database = String.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), databaseName);
    static string dataSource = "data source=" + database;
    static string ruleSource = "data source=" + String.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), "SfxFiddlerRule.db");
    static string tableName = "SfxFiddlerLog";
    static List<SfxFiddlerRule> rules = new List<SfxFiddlerRule>();
    public SfxFiddlerExtension()
    {
        /* NOTE: It's possible that Fiddler UI isn't fully loaded yet, so don't add any UI in the constructor.

           But it's also possible that AutoTamper* methods are called before OnLoad (below), so be
           sure any needed data structures are initialized to safe values here in this constructor */

        sUserAgent = "SfxFiddlerExtension";
    }

    public void OnLoad() { /* Load your UI here */
        try
        {
            ThreadPool.SetMaxThreads(4, 4);
            rules = GetRules();



        }
        catch(Exception ex)
        {
            FiddlerApplication.Log.LogString("SfxFiddler自定义插件出错！" + ex.Message + ex.StackTrace);
        }
    }
    public void OnBeforeUnload() { }

    public void AutoTamperRequestBefore(Session oSession)
    {
        oSession.oRequest["User-Agent"] = sUserAgent;
    }
    public void AutoTamperRequestAfter(Session oSession)
    {
    }
    public void AutoTamperResponseBefore(Session oSession)
    {
    }
    public void AutoTamperResponseAfter(Session oSession)
    {
        try
        {
            ThreadPool.QueueUserWorkItem(AfterResponseExec, oSession);
            //AfterResponseExec(oSession);
        }
        catch (Exception ex)
        {
            FiddlerApplication.Log.LogString("SfxFiddler自定义插件出错！" + ex.Message + ex.StackTrace);
        }

    }
    private void AfterResponseExec(object session)
    {
        var s = (Session)session;
        FiddlerApplication.Log.LogString("AutoTamperResponseAfterSfxFiddler自定义插件");
        var info = getInfo(s);
        info = "SessionId:\t" + s.id + "\r\n" + info;
        info += "url:" + s.url;
        info += "processId:" + s.LocalProcessID + "processName:" + s.LocalProcess;
        FiddlerApplication.Log.LogString(info);
        createTable();
        insertTable(s);
        SendMessage(s);
    }
    
    public void OnBeforeReturningError(Session oSession)
    {
    }
    private string getInfo(Session session)
    {


        return string.Format("ClientConnected:\t{0:HH:mm:ss.fff}\r\nClientBeginRequest:\t{1:HH:mm:ss.fff}\r\nGotRequestHeaders:\t{2:HH:mm:ss.fff}\r\nClientDoneRequest:\t{3:HH:mm:ss.fff}\r\nDetermine Gateway:\t{4,0}ms\r\nDNS Lookup: \t\t{5,0}ms\r\nTCP/IP Connect:\t{6,0}ms\r\nHTTPS Handshake:\t{7,0}ms\r\nServerConnected:\t{8:HH:mm:ss.fff}\r\nFiddlerBeginRequest:\t{9:HH:mm:ss.fff}\r\nServerGotRequest:\t{10:HH:mm:ss.fff}\r\nServerBeginResponse:\t{11:HH:mm:ss.fff}\r\nGotResponseHeaders:\t{12:HH:mm:ss.fff}\r\nServerDoneResponse:\t{13:HH:mm:ss.fff}\r\nClientBeginResponse:\t{14:HH:mm:ss.fff}\r\nClientDoneResponse:\t{15:HH:mm:ss.fff}\r\n{16}", new object[]
                {
                     session.Timers.ClientConnected,
                     session.Timers.ClientBeginRequest,
                     session.Timers.FiddlerGotRequestHeaders,
                     session.Timers.ClientDoneRequest,
                     session.Timers.GatewayDeterminationTime,
                     session.Timers.DNSTime,
                     session.Timers.TCPConnectTime,
                     session.Timers.HTTPSHandshakeTime,
                     session.Timers.ServerConnected,
                     session.Timers.FiddlerBeginRequest,
                     session.Timers.ServerGotRequest,
                     session.Timers.ServerBeginResponse,
                     session.Timers.FiddlerGotResponseHeaders,
                     session.Timers.ServerDoneResponse,
                     session.Timers.ClientBeginResponse,
                     session.Timers.ClientDoneResponse,
                     string.Format("\t请求耗时:\t{0:h\\:mm\\:ss\\.fff}\r\n", getOverallElapsed(session))
                });


    }
    private TimeSpan getOverallElapsed(Session session)
    {
        TimeSpan timeSpan = session.Timers.ServerDoneResponse - session.Timers.ServerGotRequest;
        if (timeSpan > TimeSpan.Zero)
        {
            //return string.Format("\t请求耗时:\t{0:h\\:mm\\:ss\\.fff}\r\n", timeSpan);
            return timeSpan;
        }
        return TimeSpan.Zero;
    }

    public void OnPeekAtResponseHeaders(Session oSession)
    {
        try
        {
            FiddlerApplication.Log.LogString("OnPeekAtResponseHeaders");
            var info = getInfo(oSession);
            info = "SessionId:\t" + oSession.id + "\r\n" + info;
            FiddlerApplication.Log.LogString(info);
        }
        catch (Exception ex)
        {
            FiddlerApplication.Log.LogString("SfxFiddler自定义插件出错！" + ex.Message + ex.StackTrace);
        }
    }
    private void createTable()
    {
        using (var conn = new SQLiteConnection(dataSource))
        {
            using (var cmd = new SQLiteCommand())
            {
                cmd.Connection = conn;
                conn.Open();
                var sh = new SQLiteHelper(cmd);
                if (!sh.ExistsTable(tableName))
                {

                    var tb = new SQLiteTable(tableName);
                    tb.Columns.Add(new SQLiteColumn("id", true));
                    tb.Columns.Add(new SQLiteColumn("url", ColType.Text));
                    tb.Columns.Add(new SQLiteColumn("referer", ColType.Text));
                    tb.Columns.Add(new SQLiteColumn("status", ColType.Integer));
                    tb.Columns.Add(new SQLiteColumn("duration", ColType.Integer));
                    tb.Columns.Add(new SQLiteColumn("FiddlerId", ColType.Integer));
                    tb.Columns.Add(new SQLiteColumn("requestContent", ColType.Text));
                    tb.Columns.Add(new SQLiteColumn("responseContent", ColType.Text));
                    tb.Columns.Add(new SQLiteColumn("keyword", ColType.Text));
                    tb.Columns.Add(new SQLiteColumn("insertTime", ColType.DateTime));
                    tb.Columns.Add(new SQLiteColumn("processName", ColType.Text));
                    tb.Columns.Add(new SQLiteColumn("processId", ColType.Integer));
                    tb.Columns.Add(new SQLiteColumn("ServerGotRequestTime", ColType.DateTime));
                    tb.Columns.Add(new SQLiteColumn("ServerDoneResponseTime", ColType.DateTime));
                    sh.CreateTable(tb);
                    conn.Close();
                }
            }
        }
    }
    
    private void insertTable(Session session)
    {

        var duration = getOverallElapsed(session).TotalMilliseconds;
        var dic = new Dictionary<string, object>() {
                { "url" ,session.fullUrl },
                { "referer", session.oRequest["referer"] },
                { "status", session.responseCode},
                { "duration", (int)duration },
                { "FiddlerId", session.id },
                { "requestContent", session.GetRequestBodyAsString() },
                { "responseContent", "" },
                { "keyword", "" },
                { "insertTime", DateTime.Now },
                { "ServerGotRequestTime",session.Timers.ServerGotRequest },
                { "ServerDoneResponseTime",session.Timers.ServerDoneResponse },
                {"processName",session.LocalProcess },
                {"processId",session.LocalProcessID }
            };
        using (var conn = new SQLiteConnection(dataSource))
        {
            using (var cmd = new SQLiteCommand())
            {
                cmd.Connection = conn;
                conn.Open();
                var sh = new SQLiteHelper(cmd);
                sh.Insert(tableName, dic);
            }
        }

    }
    private void SendMessage(Session session)
    {
        
            
        foreach (var rule in rules)
        {
            if (session.uriContains(rule.EndKeyword))
            {
                var info = rule.EndKeyword;
                info += "|" + session.Timers.ServerDoneResponse.ToString("yyyy-MM-dd HH:mm:ss.fff");
                info += "|" + session.LocalProcess;
                info += "|" + session.LocalProcessID;
                info += "|" + rule.StartKeyword;
                try
                {
                    using (TcpClient client = new TcpClient("127.0.0.1", 63351))
                    {
                        client.Client.Send(System.Text.Encoding.UTF8.GetBytes(info));
                    }

                }
                catch (Exception ex)
                {
                    FiddlerApplication.Log.LogString("SfxFiddler自定义插件出错！" + ex.Message + ex.StackTrace);
                }
            }
        }
           
       
    }
    private List<SfxFiddlerRule> GetRules()
    {
        using (var conn = new SQLiteConnection(ruleSource))
        {
            using (var cmd = new SQLiteCommand())
            {
                cmd.Connection = conn;
                conn.Open();
                var sh = new SQLiteHelper(cmd);
                var dt = sh.Select("select * from SfxFiddlerRule");
                List<SfxFiddlerRule> list = new List<SfxFiddlerRule>();
                foreach (DataRow row in dt.Rows)
                {
                    SfxFiddlerRule rule = new SfxFiddlerRule();
                    rule.Id = int.Parse(row["Id"].ToString());
                    rule.RuleName = row["RuleName"].ToString();
                    rule.StartUrl = row["StartUrl"].ToString();
                    rule.EndUrl = row["EndUrl"].ToString();
                    rule.Module = row["Module"].ToString();
                    rule.Version = row["Version"].ToString();
                    rule.InsertTime = DateTime.Parse(row["InsertTime"].ToString());
                    rule.EndKeyword = row["EndKeyword"].ToString();
                    rule.StartKeyword = row["StartKeyword"].ToString();
                    list.Add(rule);

                }
                return list;
            }
        }
    }
    public class SfxFiddlerRule
    {
        public int Id { get; set; }
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

