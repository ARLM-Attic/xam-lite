using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XAMLite
{
    using System.Windows;

    /// <summary>
    /// The orientation of the control, whether horizontal or vertical.
    /// </summary>
    public enum Orientation
    {
        Horizontal,

        Vertical
    }

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class XAMLiteScrollBar : XAMLiteGridNew
    {
        /// <summary>
        /// The orientation of the scroll bar, whether horizontal or vertical.
        /// </summary>
        public Orientation Orientation;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteScrollBar(Game game)
            : base(game)
        {
            Orientation = Orientation.Vertical;
        }

        /// <summary>
        /// Loads the content for the control.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            var backDrop = new XAMLiteImageNew(Game)
            {
                SourceName = "Icons/ScrollBackDrop",
                RenderTransform = Orientation == Orientation.Vertical ? RenderTransform.Normal : RenderTransform.RotateCounterClockwise90,
                Height = Orientation == Orientation.Vertical ? Height : 21,
                Width = Orientation == Orientation.Vertical ? 21 : Width
            };
            Children.Add(backDrop);

            var upArrowButton = new XAMLiteImageWithRolloverNew(Game)
                {
                    SourceName = "Icons/ArrowButton", 
                    RolloverSourceName = "Icons/ArrowButtonHover", 
                    RenderTransform = Orientation == Orientation.Vertical ? RenderTransform.Normal : RenderTransform.RotateCounterClockwise90
                };
            Children.Add(upArrowButton);

            var upArrowButtonMouseDown = new XAMLiteImageNew(Game)
            {
                SourceName = "Icons/ArrowButtonMouseDown",
                RenderTransform = Orientation == Orientation.Vertical ? RenderTransform.Normal : RenderTransform.RotateCounterClockwise90,
                Visibility = Visibility.Hidden
            };
            Children.Add(upArrowButtonMouseDown);

            var downArrowButton = new XAMLiteImageWithRolloverNew(Game)
            {
                SourceName = "Icons/ArrowButton",
                RolloverSourceName = "Icons/ArrowButtonHover",
                RenderTransform = Orientation == Orientation.Vertical ? RenderTransform.FlipVertical : RenderTransform.RotateClockwise90
            };
            Children.Add(downArrowButton);

            var downArrowButtonMouseDown = new XAMLiteImageNew(Game)
            {
                SourceName = "Icons/ArrowButtonMouseDown",
                RenderTransform = Orientation == Orientation.Vertical ? RenderTransform.FlipVertical : RenderTransform.RotateClockwise90,
                Visibility = Visibility.Hidden
            };
            Children.Add(downArrowButtonMouseDown);
        }
    }
}
