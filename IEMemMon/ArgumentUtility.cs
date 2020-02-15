using System;
using System.Runtime.CompilerServices;

namespace IEMemMon
{
    public static class ArgumentUtility
    {
        public static void NullCheck(object o, [CallerMemberName]string calledBy = "")
        {
            if (o == null) { throw new ArgumentNullException(calledBy); }
        }
    }
}