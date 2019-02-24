using Fiddler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public class SfxFiddlerExtension : IAutoTamper // Ensure class is public, or Fiddler won't see it!
{
    string sUserAgent = "";

    public SfxFiddlerExtension()
    {
        /* NOTE: It's possible that Fiddler UI isn't fully loaded yet, so don't add any UI in the constructor.

           But it's also possible that AutoTamper* methods are called before OnLoad (below), so be
           sure any needed data structures are initialized to safe values here in this constructor */

        sUserAgent = "SfxFiddlerExtension";
    }

    public void OnLoad() { /* Load your UI here */ }
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
            FiddlerApplication.Log.LogString("AutoTamperResponseAfterSfxFiddler自定义插件");
            var info = getInfo(oSession);
            info = "SessionId:\t" + oSession.id + "\r\n" + info;
            FiddlerApplication.Log.LogString(info);
        }
        catch (Exception ex)
        {
            FiddlerApplication.Log.LogString("SfxFiddler自定义插件出错！" + ex.Message + ex.StackTrace);
        }

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
                     getOverallElapsed(session)
                });


    }
    private string getOverallElapsed(Session session)
    {
        TimeSpan timeSpan = session.Timers.ServerDoneResponse - session.Timers.ClientDoneRequest;
        if (timeSpan > TimeSpan.Zero)
        {
            return string.Format("\t请求耗时:\t{0:h\\:mm\\:ss\\.fff}\r\n", timeSpan);
        }
        return string.Empty;
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
}

