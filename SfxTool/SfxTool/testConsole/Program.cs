using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace testConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Timers.Timer t = new System.Timers.Timer(1000 * 10);
            t.Elapsed += delegate (object sender, System.Timers.ElapsedEventArgs e) { Console.WriteLine("1"); };
            t.Start();
            Console.ReadLine();

        }
    }
}