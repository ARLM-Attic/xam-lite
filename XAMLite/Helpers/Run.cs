namespace XAMLite
{
    // for mocking the WPF constructor
    // XAMLiteTextBlock textBlock = new XAMLiteTextBlock(this, new Run("This is text."));
    // this value gets placed into the "string Run" of XAMLiteTextBlock.cs
    public class Run
    {
        /// <summary>
        /// Contains the content of the Run.
        /// </summary>
        public string TextBlock { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="textBlock"></param>
        public Run(string textBlock)
        {
            TextBlock = textBlock;
        }
    }
}
