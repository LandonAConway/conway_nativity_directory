using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conway_Nativity_Directory
{
    public static class Clipboard
    {
        private static object clipboard;

        public static object GetClipboard()
        {
            return clipboard;
        }

        public static void CopyToClipboard(object obj)
        {
            clipboard = obj;
        }
    }
}
