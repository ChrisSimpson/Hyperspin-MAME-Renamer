using System;
using System.Collections.ObjectModel;

namespace Renamer.Models
{
    /// <summary>
    /// Application settings
    /// </summary>
    public class Settings : PropertyChangedBase
    {
        #region Private Fields

        private bool _autoBestNameMapping;
        private bool _autoCrcMapping = true;
        private bool _autoExactNameMapping = true;
        private bool _crcCheck = true;
        private string _hyperSpinXmlFileName;
        private string _mameXmlFileName;
        private string _outputFolder;
        private string _romFolder;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets the set of artwork folders
        /// </summary>
        public ObservableCollection<String> ArtworkFolders { get; } = new ObservableCollection<string>();

        /// <summary>
        /// Auto best name mapping flag
        /// </summary>
        public bool AutoBestNameMapping
        {
            get { return _autoBestNameMapping; }
            set { SetProperty(ref _autoBestNameMapping, value); }
        }

        /// <summary>
        /// Auto CRC mapping flag
        /// </summary>
        public bool AutoCrcMapping
        {
            get { return _autoCrcMapping; }
            set { SetProperty(ref _autoCrcMapping, value); }
        }

        /// <summary>
        /// Auto exact name mapping flag
        /// </summary>
        public bool AutoExactNameMapping
        {
            get { return _autoExactNameMapping; }
            set { SetProperty(ref _autoExactNameMapping, value); }
        }

        /// <summary>
        /// CRC check flag
        /// </summary>
        public bool CrcCheck
        {
            get { return _crcCheck; }
            set { SetProperty(ref _crcCheck, value); }
        }

        /// <summary>
        /// The name of the HyperSpin XML file
        /// </summary>
        public string HyperSpinXmlFileName
        {
            get { return _hyperSpinXmlFileName; }
            set { SetProperty(ref _hyperSpinXmlFileName, value); }
        }

        /// <summary>
        /// The name of the MAME XML file
        /// </summary>
        public string MameXmlFileName
        {
            get { return _mameXmlFileName; }
            set { SetProperty(ref _mameXmlFileName, value); }
        }

        /// <summary>
        /// The output folder
        /// </summary>
        public string OutputFolder
        {
            get { return _outputFolder; }
            set { SetProperty(ref _outputFolder, value); }
        }

        /// <summary>
        /// The ROM folder
        /// </summary>
        public string RomFolder
        {
            get { return _romFolder; }
            set { SetProperty(ref _romFolder, value); }
        }

        #endregion Public Properties
    }
}