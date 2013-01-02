using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XAMLite
{
    using System.Windows;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class XAMLiteRichTextBlock : XAMLiteTextBlockNew
    {
        /// <summary>
        /// 
        /// </summary>
        private List<string> _hTMLTags = new List<string> { "<b>", "<i>", "<u>" };

        /// <summary>
        /// 
        /// </summary>
        private List<XAMLiteLabelNew> _labels; 
 
        /// <summary>
        /// Basic constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteRichTextBlock(Game game)
            : base(game)
        {
            _labels = new List<XAMLiteLabelNew>();
        }

        /// <summary>
        /// Construct with pre-loaded Run of text.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="run"></param>
        public XAMLiteRichTextBlock(Game game, Run run)
            : base(game, run)
        {
            _labels = new List<XAMLiteLabelNew>();
        }

        /// <summary>
        /// Loads the content for the Rich Text Block.
        /// </summary>
        protected override void LoadContent()
        {
            RemoveAndReplaceTags();

            base.LoadContent();

            _labels.Add(TextLabel);
        }

        /// <summary>
        /// 
        /// </summary>
        private void RemoveAndReplaceTags()
        {
            foreach (var tag in _hTMLTags)
            {
                var indexStart = Text.IndexOf(tag);
                var c = tag.ToCharArray()[1];
                var indexEnd = Text.IndexOf("</" + c + ">") + 2;

                if (indexStart >= 0)
                {
                    Text = Text.Remove(indexStart, (indexEnd + 2) - indexStart);

                    for (int i = 0; i < (indexEnd + 2) - indexStart; i++)
                    {
                        Text = Text.Insert(i, " ");
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void UpdateForTextWrapping()
        {
            base.UpdateForTextWrapping();

            // When applicable, wrap the text and create the label.
            //if (TextWrapping == TextWrapping.Wrap)
            //{
            //    Text = WordWrapper.Wrap(Text, Width, (int)TextLabel.MeasureString().X, TextLabel.Padding);
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int HTMLTagCounter()
        {
            return 0;
        }
    }
}
