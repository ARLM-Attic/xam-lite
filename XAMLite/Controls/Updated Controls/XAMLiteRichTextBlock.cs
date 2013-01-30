using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;

namespace XAMLite
{
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
        private bool _stringSeparated;

        /// <summary>
        /// 
        /// </summary>
        //private RasterizerState _rasterizeState;

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
            //RemoveAndReplaceTags();

            base.LoadContent();

            //_labels.Add(TextLabel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!_stringSeparated)
            {
                SeparateStrings();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SeparateStrings()
        {
            _blocks = Regex.Split(TextLabel.Content.ToString(), @"\n");
            
            _stringSeparated = true;

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
            {
                SpriteBatch.Begin();
            }

            var count = 0;
            for (var i = 0; i < _blocks.Length; i++)
            {
                //if (!_blocks[i].Contains("<b>") && !_blocks[i].Contains("</b>") && !_blocks[i].Contains("<i>") && !_blocks[i].Contains("</i>") && !_blocks[i].Contains("<u>") && !_blocks[i].Contains("</u>"))
                {
                    SpriteBatch.DrawString(TextLabel.SpriteFont, _blocks[i], new Vector2(TextLabel.Panel.X, TextLabel.Panel.Y + (TextLabel.Height * count)), Microsoft.Xna.Framework.Color.Black * (float)Opacity);
                }

                Console.WriteLine(count);
                count++;
            }

            SpriteBatch.End();
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
                    Console.WriteLine("Tag: " + tag + "  Index start: " + indexStart);
                    var c = tag.ToCharArray()[1];
                    var indexEnd = text.IndexOf("</" + c + ">") + 2;
                    Console.WriteLine("Index end: " + indexEnd);
                    if (indexStart >= 0)
                    {
                        var s = text.Substring(indexStart, (indexEnd + 2) - indexStart);
                        //Console.WriteLine(s);
                        var str = " ";
                        for (var i = 0; i < s.Length - 1; i++)
                        {
                            str = str.Insert(i, " ");
                        }

                        Text = Text.Replace(s, str);

                        text = text.Substring(indexEnd + 2);
                    }
                    else
                    {
                        text = string.Empty;
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
