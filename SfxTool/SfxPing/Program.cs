using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;

namespace SfxPing
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (args.Length != 3)
            {
                Console.WriteLine("参数不正确！正确如 pingx 127.0.0.1 log.txt [超时数单位毫秒] ");
                return;
            }
            try
            {
                string ip = args[0];
                string log = args[1];
                int number = 0;
                if (args.Length == 3)
                {
                    int.TryParse(args[2], out number);
                }
                //远程服务器IP
                string ipStr = ip;
                //构造Ping实例
                Ping pingSender = new Ping();
                //Ping 选项设置
                PingOptions options = new PingOptions();
                options.DontFragment = true;
                //测试数据
                string data = "test data abcabctest data abcabc";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                //设置超时时间
                int timeout = 6000;
                //调用同步 send 方法发送数据,将返回结果保存至PingReply实例
                while (true)
                {
                    PingReply reply = pingSender.Send(ipStr, timeout, buffer, options);
                    string result = "";
                    if (reply.Status == IPStatus.Success)
                    {
                        if (reply.RoundtripTime == 0)
                        {
                            result = string.Format("{0}  来自 {1} 的回复: 字节={2} 时间<1ms TTL={4}",
                                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                reply.Address.ToString(),
                                reply.Buffer.Length,
                                reply.RoundtripTime,
                                reply.Options.Ttl);
                        }
                        else
                        {
                            result = string.Format("{0}  来自 {1} 的回复: 字节={2} 时间={3}ms TTL={4}",
                               DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                               reply.Address.ToString(),
                               reply.Buffer.Length,
                               reply.RoundtripTime,
                               reply.Options.Ttl);
                        }
                    }
                    else
                    {
                        result = string.Format("{0} 失败 状态：{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), reply.Status.ToString());
                        File.AppendAllText(log, result + "\r\n");
                    }
                    Console.WriteLine(result);
                    if (number >= 0 && reply.RoundtripTime >= number)
                    {
                        File.AppendAllText(log, result + "\r\n");
                    }

                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("请输入正确的IP或日志文件名！");
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
        }
    }
}
