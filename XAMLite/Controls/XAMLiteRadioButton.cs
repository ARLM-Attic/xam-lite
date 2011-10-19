using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Media;

namespace XAMLite
{
    public class XAMLiteRadioButton : XAMLiteControl
    {
        public bool IsChecked { get; set; }

        public string Content
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        public string GroupName { get; set; }

        public XAMLiteRadioButton(Game game)
            : base(game)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            //
            if (Visible == System.Windows.Visibility.Visible)
            {
                this.spriteBatch.Begin();

                this.spriteBatch.End();
            }
        }
    }
}
