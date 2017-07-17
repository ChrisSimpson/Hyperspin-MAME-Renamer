namespace Renamer.Models
{
    /// <summary>
    /// Definition of a fuzzy match
    /// </summary>
    public class FuzzyMatch
    {
        #region Public Properties

        /// <summary>
        /// The distance
        /// </summary>
        public int Distance { get; set; }

        /// <summary>
        /// The software
        /// </summary>
        public MameSoftware MameSoftware { get; set; }

        #endregion Public Properties
    }
}