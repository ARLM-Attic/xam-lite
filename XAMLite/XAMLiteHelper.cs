using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XAMLite
{
    /// <summary>
    /// Static helper methods for XAMLite.
    /// </summary>
    public static class XAMLiteHelper
    {
        /// <summary>
        /// Calls dispose, but does a null check first.
        /// </summary>
        /// <param name="control"></param>
        public static void SafeDispose(XAMLiteControl control)
        {
            if (control != null)
            {
                control.Dispose();
            }
        }
    }
}