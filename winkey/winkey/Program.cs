using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace winkey
{
    class Program
    {
        static void Main(string[] args)
        {
            Revealer revealer = new Revealer();
        }
    }
    public class Revealer
    {
        List<String> keys = new List<String>();
        List<String> wlans = new List<String>();

        public Revealer()
        {
            keys.Clear();
            Init();
        }

        private void Init()
        {
            ReadWLAN();
            GetWLANKeys();
        }


        private void GetWLANKeys()
        {
            foreach(string wlan in wlans)
            {
                //Console.WriteLine(wlan);
                string key = GetKeyForWLAN(wlan);
                Console.WriteLine(wlan + " : " + key);
            }
        }

        private string GetKeyForWLAN(string wlan)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.Arguments = "wlan show profile name=\"" + wlan + "\" key=clear";
            psi.FileName = "netsh";
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;

            Process p = new Process();
            p.StartInfo = psi;

            p.Start();
            //p.WaitForExit();

            Thread.Sleep(1000);

            string val = String.Empty;
            StreamReader reader = p.StandardOutput;
            while (reader.Peek() >= 0)
            {
                string ln = reader.ReadLine();
                if (ln.Contains("Schlüsselinhalt") || ln.Contains("Key Content"))
                {
                    val = ln.Split(':')[1];
                    break;
                }
            }
            return val;
        }

        private void ReadWLAN()
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.Arguments = "wlan show profiles";
            psi.FileName = "netsh";
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;

            Process p = new Process();
            p.StartInfo = psi;

            p.Start();
            //p.WaitForExit();

            Thread.Sleep(1000);

            StreamReader reader = p.StandardOutput;
            bool started = false;
            while (reader.Peek() >= 0)
            {
                string ln = reader.ReadLine();
                if (ln == "---------------")
                    started = true;

                if (started)
                {
                    try { wlans.Add(ln.Split(':')[1].Trim());}
                    catch {}
                }
            }
        }
    }
}
