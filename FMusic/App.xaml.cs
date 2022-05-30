using FMusic.Util.NetMusicCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FMusic
{
    public partial class App : Application
    {
        #region 资源回收
        [DllImport("kernel32.dll")]
        public static extern bool SetProcessWorkingSetSize(IntPtr proc, int min, int max);


        public void FulshMemor()
        {
            GC.Collect();
            GC.WaitForFullGCComplete();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
        }

        #region 资源加载
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string resourceName = "FMusic.Library." + new AssemblyName(args.Name).Name + ".dll";
            
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (!stream.Equals(null))
                {
                    byte[] assemblyData = new byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
                return null;
            }
        }
        #endregion

        public App()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            var timer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(10) };
            timer.Tick += (s, e) => FulshMemor();
            timer.Start();
            
        }

        #endregion
    }
}
