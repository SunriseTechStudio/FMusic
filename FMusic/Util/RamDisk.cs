using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMusic.Util
{
    class RamDisk
    {
        private static List<object> plist = new List<object>();

        public static void ReInterlize() => plist.Clear();

        public static int Add(object obj)
        {
            plist.Add(obj);
            return plist.Count - 1;
        }

        public static void Remove(object obj)
        {
            plist[plist.IndexOf(obj)] = "";
        }
    }
}
