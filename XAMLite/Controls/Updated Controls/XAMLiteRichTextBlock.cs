using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Microsoft.Xna.Framework;

namespace XAMLite
{
    public class RichTextInfo
    {
        public int StartIndex;

        public int EndIndex;

        public string Text;

        public FontFamily Font;

        public List<string> Tags;

        public RichTextInfo()
        {
            Tags = new List<string>();
        }
    }

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class XAMLiteRichTextBlock : XAMLiteTextBlockNew
    {
        /// <summary>
        /// 
        /// </summary>
        private List<string> _hTMLTags = new List<string> { "<b>", "<i>", "<u>", "<h1>", "<h2>", "<h3>", "<Font>", "<Font Color=" };

        /// <summary>
        /// 
        /// </summary>
        private List<XAMLiteLabelNew> _labels;

        /// <summary>
        /// 
        /// </summary>
        private List<RichTextInfo> _richTextInfo;

        /// <summary>
        /// 
        /// </summary>
        private Thickness _storedMargin;

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
                if (IsLoading)
                {
                    base.Margin = value;
                    _storedMargin = value;
                }

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
            _richTextInfo = new List<RichTextInfo>();
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
            _richTextInfo = new List<RichTextInfo>();
        }

        /// <summary>
        /// Loads the content for the Rich Text Block.
        /// </summary>
        protected override void LoadContent()
        {
            BuildTextBlocks();

            base.LoadContent();

            Margin = _storedMargin;

            foreach (var info in _richTextInfo)
            {
                var label = new XAMLiteLabelNew(Game);
                label.Content = info.Text;
                label.Foreground = Foreground;
                label.FontFamily = info.Font;
                label.Spacing = Spacing;
                label.Padding = Padding;
                label.Width = Width;
                _labels.Add(label);
            }

            var h = 0;
            int count = 0;
            foreach (var label in _labels)
            {
                Game.Components.Add(label);
                var append = false;
                var w = (int)label.MeasureString().X;
                if (count > 0)
                {
                    var s = _labels[count - 1].Content.ToString();
                    if (s.LastIndexOf("\n") != s.Length - 1)
                    {
                        append = true;
                    }
                }

                if (w > Width)
                {
                    label.Content = WordWrapper.Wrap(label.Content.ToString(), Width - 20, w, label.Padding);
                }

                if (append)
                {
                    var text = label.Content.ToString();
                    label.Content = "";

                    while (label.MeasureString().X < _labels[count - 1].MeasureString().X)
                    {
                        label.Content = " " + label.Content;
                    }

                    label.Content += text;
                }

                var m = label.Margin;
                label.Margin = new Thickness(m.Left, m.Top + h, m.Right, m.Bottom);

                h += (label.Content.ToString().LastIndexOf("\n") != label.Content.ToString().Length - 1) ? 0 : (int)label.MeasureHeight();

                Game.Components.Remove(label);
                Children.Add(label);
                count++;
            }

            TextLabel.Foreground = Brushes.Transparent;
        }

        /// <summary>
        /// 
        /// </summary>
        private void BuildTextBlocks()
        {
            // 1. Search for first tag
            // 2. If one is found, find its end position.
            //    A. If end position not found, throw error.
            // 3. Create separate string containing the block of text.
            // 4. Search for inner tags.
            // 5. Build blocks of rich text info.


            // 6. Once the entire text is archived, layout the text line by line.

            // go through the text block once.
            // 1. Start by finding the first tag in the block.

            var text = Text;
            for (int j = 0; j < Text.Length; j++)
            {
                var firstTag = _hTMLTags[0];
                var position = text.Length - 1;
                for (int i = 0; i < _hTMLTags.Count; i++)
                {
                    var index = text.IndexOf(_hTMLTags[i]);
                    if (index < position && index > -1)
                    {
                        position = index;
                        firstTag = _hTMLTags[i];
                    }
                }

                // If the first tag is not at the beginning, add this string to the list.
                if (position != 0 && position != text.Length - 1)
                {
                    var info = new RichTextInfo();
                    info.StartIndex = j;
                    info.EndIndex = j + position - 1;
                    info.Font = FontFamily;
                    info.Text = Text.Substring(info.StartIndex, position - 1);
                    _richTextInfo.Add(info);

                    position--;
                }
                else if (position == text.Length - 1)
                {
                    return;
                }

                // Extract the first excerpt with tags.
                var s = ExtractRawString(firstTag, j + position);

                // Determine fonts, etc. and add to the list.
                BuildTextInfo(s, j + position);

                j = _richTextInfo[_richTextInfo.Count - 1].EndIndex;
                text = Text.Substring(j);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private string ExtractRawString(string tag, int index)
        {
            var endTag = tag.Insert(1, "/");
            var text = Text.Substring(index);
            var pos = text.IndexOf(endTag);

            return Text.Substring(index, pos + endTag.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="position"> </param>
        private void BuildTextInfo(string s, int position)
        {
            // 1. Search for inner tags.
            var tags = CollectTags(s);

            // 2. Determine the correct font.
            var font = BuildFontFamily(tags);

            var tLength = s.Length;

            // 3. Remove all tags.
            var text = RemoveTags(s);
            
            // 4. Build the rich text info.
            var info = new RichTextInfo();
            info.StartIndex = position;
            info.EndIndex = position + tLength;
            info.Font = font;
            info.Text = text;

            _richTextInfo.Add(info);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="excerpt"></param>
        /// <returns></returns>
        private List<string> CollectTags(string excerpt)
        {
            var tagsInBlock = new List<string>();

            foreach (var tag in _hTMLTags)
            {
                if (excerpt.Contains(tag))
                {
                    tagsInBlock.Add(tag);
                }
            }

            return tagsInBlock;
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
        /// <param name="excerpt"></param>
        /// <returns></returns>
        private string RemoveTags(string excerpt)
        {
            var s = excerpt;
            var str = "";

            foreach (var tag in _hTMLTags)
            {
                s = excerpt;

                if (s.Contains(tag))
                {
                    var t = tag.ToCharArray()[1];

                    // remove the start tag.
                    s = s.Replace(tag, str);

                    // remove the end tag.
                    s = s.Replace("</" + t + ">", str);

                    excerpt = s;
                }
            }

            return s;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public float MeasureTextHeight()
        {
            var h = 0f;

            var count = 0;
            foreach (var label in _labels)
            {
                h += (int)label.MeasureHeight();

                if (count > 0)
                {
                    if (_labels[count - 1].Content.ToString().LastIndexOf("\n") != _labels[count - 1].Content.ToString().Length - 1)
                    {
                        h -= _labels[count - 1].MeasureString().Y;
                    }
                }

                count++;
            }

            return h;
        }
    }
}
