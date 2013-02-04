using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;

namespace XAMLite
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class XAMLiteRichTextBlock : XAMLiteTextBlockNew
    {
        /// <summary>
        /// 
        /// </summary>
        private List<string> _hTMLTags = new List<string> { "<b>", "<i>", "<u>", "<Font>", "<Font Color=" };

        /// <summary>
        /// 
        /// </summary>
        private List<XAMLiteLabelNew> _labels;

        /// <summary>
        /// 
        /// </summary>
        private string[] _blocks;  

        /// <summary>
        /// 
        /// </summary>
        //private bool _stringSeparated;

        /// <summary>
        /// 
        /// </summary>
        //private RasterizerState _rasterizeState;

        /// <summary>
        /// This modifies both the grid's and the text label's margins but 
        /// leaves the background alone.  This allows for scrolling when it is 
        /// attached to a scroll bar.
        /// </summary>
        public override Thickness Margin
        {
            get
            {
                if (_labels == null)
                {
                    return base.Margin;
                }

                if (TextLabel != null)
                {
                    return TextLabel.Margin;
                }

                return base.Margin;
            }

            set
            {
                if (_labels == null)
                {
                    base.Margin = value;
                }
                else
                {
                    var t = Margin.Top - value.Top;
                    var l = Margin.Left - value.Left;

                    foreach (var label in _labels)
                    {
                        label.Margin = new Thickness(label.Margin.Left - l, label.Margin.Top - t, label.Margin.Right, label.Margin.Bottom);
                    }

                    base.Margin = value;
                }
            }
        }

        /// <summary>
        /// Basic constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteRichTextBlock(Game game)
            : base(game)
        {
            _labels = new List<XAMLiteLabelNew>();
            //_blocks = new List<string>();
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
            //_blocks = new List<string>();
        }

        /// <summary>
        /// Loads the content for the Rich Text Block.
        /// </summary>
        protected override void LoadContent()
        {
            RemoveAndReplaceTags();

            base.LoadContent();

            foreach (var label in _labels)
            {
                Children.Add(label);
            }

            //_labels.Add(TextLabel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //if (!_stringSeparated)
            //{
            //    SeparateStrings();
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        private void SeparateStrings()
        {
            _blocks = Regex.Split(TextLabel.Content.ToString(), @"\n");
            
            //_stringSeparated = true;

            for (var i = 0; i < _blocks.Length; i++)
            {
                Console.WriteLine("Line: " + i + 1 + "    " +_blocks[i]);
            }

            Console.WriteLine("Text Height: " + TextLabel.Height);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            //base.Draw(gameTime);
            if (_blocks == null)
            {
                return;
            }

            //if (TextLabel.Parent != null)
            //{
            //    _rasterizeState = new RasterizerState { ScissorTestEnable = true };
            //    SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, _rasterizeState);
            //    SpriteBatch.GraphicsDevice.ScissorRectangle = TextLabel.Parent.Panel;
            //}
            //else
            //{
            //    SpriteBatch.Begin();
            //}

            //var count = 0;
            //for (var i = 0; i < _blocks.Length; i++)
            //{
            //    //if (!_blocks[i].Contains("<b>") && !_blocks[i].Contains("</b>") && !_blocks[i].Contains("<i>") && !_blocks[i].Contains("</i>") && !_blocks[i].Contains("<u>") && !_blocks[i].Contains("</u>"))
            //    {
            //        SpriteBatch.DrawString(TextLabel.SpriteFont, _blocks[i], new Vector2(TextLabel.Panel.X, TextLabel.Panel.Y + (TextLabel.Height * count)), Microsoft.Xna.Framework.Color.Black * (float)Opacity);
            //    }

            //    Console.WriteLine(count);
            //    count++;
            //}

            //SpriteBatch.End();
        }

        /// <summary>
        /// 
        /// </summary>
        private void RemoveAndReplaceTags()
        {
            foreach (var tag in _hTMLTags)
            {
                var text = Text;

                while (text.Contains(tag))
                {
                    var indexStart = text.IndexOf(tag);

                    var c = tag.ToCharArray()[1];
                    var indexEnd = text.IndexOf("</" + c + ">") + 2;

                    if (indexStart >= 0)
                    {
                        // get the whole substring
                        var s = text.Substring(indexStart, (indexEnd + 2) - indexStart);

                        //s = ReplaceInnerTags(s);
                        Text = Text.Replace(s, ReplaceInnerTags(s));

                        text = text.Substring(indexEnd + 2);
                    }
                    else
                    {
                        text = string.Empty;
                    }
                }
            }
        }

        private string ReplaceInnerTags(string excerpt)
        {
            var s = excerpt;
            var str = "";
            var tagsInBlock = new List<string>();

            foreach (var tag in _hTMLTags)
            {
                s = excerpt;

                if (s.Contains(tag))
                {      
                    tagsInBlock.Add(tag);
                    var t = tag.ToCharArray()[1];

                    // remove the start tag.
                    s = s.Replace(tag, str);

                    // remove the end tag.
                    s = s.Replace("</" + t + ">", str);


                    // Replace the old text with the new text.
                    Text = Text.Replace(excerpt, s);

                    excerpt = s;
                }
            }

            BuildLabel(tagsInBlock, s);

            return s;
        }

        private int position = 0;

        /// <summary>
        /// 
        /// </summary>
        private void BuildLabel(List<string> tags, string s)
        {
            FontFamily font = BuildFontFamily(tags);
            var label = new XAMLiteLabelNew(Game)
                {
                    Content = s,
                    Foreground = Foreground,
                    FontFamily = font,
                    Margin = new Thickness(0, position, 0, 0)
                };
            _labels.Add(label);

            position += 20;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private FontFamily BuildFontFamily(List<string> tags)
        {
            var s = FontFamily.ToString();

            if (tags.Contains(_hTMLTags[0]) && tags.Contains(_hTMLTags[1]))
            {
                s += "BoldItalic";
            }
            else if (tags.Contains(_hTMLTags[0]))
            {
                s += "Bold";
            }
            else if (tags.Contains(_hTMLTags[1]))
            {
                s += "Italic";
            }

            return new FontFamily(s);
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
