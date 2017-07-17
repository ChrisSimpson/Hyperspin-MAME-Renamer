namespace Renamer.Models
{
    /// <summary>
    /// Definition of a MAME software entry
    /// </summary>
    public class MameSoftware : PropertyChangedBase
    {
        #region Private Fields

        private string _crc;
        private string _description;
        private string _name;
        private string _romName;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// The CRC value
        /// </summary>
        public string Crc
        {
            get { return _crc; }
            set { SetProperty(ref _crc, value); }
        }

        /// <summary>
        /// The description
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        /// <summary>
        /// The name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        /// <summary>
        /// The ROM name
        /// </summary>
        public string RomName
        {
            get { return _romName; }
            set { SetProperty(ref _romName, value); }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Returns a string representation of the object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Description;
        }

        #endregion Public Methods
    }
}