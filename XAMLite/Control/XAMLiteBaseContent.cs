using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XAMLite
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class XAMLiteBaseContent : XAMLiteBaseContentControl
    {
        /// <summary>
        /// Object contained in the control, which might include
        /// string, date/time, etc.
        /// </summary>
        public virtual object Content { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteBaseContent(Game game)
            : base(game)
        {
            
        }
    }
}
