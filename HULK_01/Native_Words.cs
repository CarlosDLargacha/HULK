using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HULK_01
{
    internal class Native_Words
    {
        internal static bool IsNativeWord(string token)
        {
            switch (token)
            {
                case "let": return true;
                case "in": return true;
                case "print": return true;
                case "else": return true;
                case "if": return true;
                case "function": return true;
            }
            return false;
        }
    }
}
