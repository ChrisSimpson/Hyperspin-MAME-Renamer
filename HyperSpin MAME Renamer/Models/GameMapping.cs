using Renamer.Properties;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Renamer.Models
{
    /// <summary>
    /// Definition of a game mapping
    /// </summary>
    public class GameMapping : PropertyChangedBase
    {
        #region Private Fields

        private static Brush _amberBrush = new SolidColorBrush(Color.FromRgb(252, 211, 86));
        private static Brush _greenBrush = new SolidColorBrush(Color.FromRgb(86, 252, 125));
        private static Brush _redBrush = new SolidColorBrush(Color.FromRgb(252, 86, 86));
        private static Brush _whiteBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));

        private string _crc;
        private string _details;
        private string _mappingMethod;
        private MameSoftware _selectedMapping;
        private string _sourceName;
        private RenameStatus _status;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// The CRC
        /// </summary>
        public string Crc
        {
            get { return _crc; }
            set { SetProperty(ref _crc, value); }
        }

        /// <summary>
        /// The Details
        /// </summary>
        public string Details
        {
            get { return _details; }
            set { SetProperty(ref _details, value); }
        }

        /// <summary>
        /// The background colour to use on the mapping grid
        /// </summary>
        public Brush MappingBackground
        {
            get { return _selectedMapping == null ? _redBrush : _greenBrush; }
        }

        /// <summary>
        /// The mapping method that was used
        /// </summary>
        public string MappingMethod
        {
            get { return _mappingMethod; }
            set { SetProperty(ref _mappingMethod, value); }
        }

        /// <summary>
        /// The list of possible mappings
        /// </summary>
        public ICollection<MameSoftware> PossibleMappings { get; } = new List<MameSoftware>();

        /// <summary>
        /// The background colour to use on the results grid
        /// </summary>
        public Brush ResultsBackground
        {
            get
            {
                switch (Status)
                {
                    case RenameStatus.Success:
                        return _greenBrush;

                    case RenameStatus.Failed:
                        return _redBrush;

                    case RenameStatus.MissingArtwork:
                        return _amberBrush;

                    default:
                        return _whiteBrush;
                }
            }
        }

        /// <summary>
        /// The selected mapping
        /// </summary>
        public MameSoftware SelectedMapping
        {
            get { return _selectedMapping; }
            set
            {
                SetProperty(ref _selectedMapping, value);
                MappingMethod = Resources.Manual;

                OnPropertyChanged("ShortName");
            }
        }

        /// <summary>
        /// The short name of the selected mapping
        /// </summary>
        public string ShortName
        {
            get { return _selectedMapping != null ? _selectedMapping.Name : String.Empty; }
        }

        /// <summary>
        /// The source name
        /// </summary>
        public string SourceName
        {
            get { return _sourceName; }
            set { SetProperty(ref _sourceName, value); }
        }

        /// <summary>
        /// The status
        /// </summary>
        public RenameStatus Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        /// <summary>
        /// The status description
        /// </summary>
        public string StatusDescription
        {
            get
            {
                switch (Status)
                {
                    case RenameStatus.Success:
                        return Resources.Success;

                    case RenameStatus.Failed:
                        return Resources.Failed;

                    case RenameStatus.MissingArtwork:
                        return Resources.MissingArtwork;

                    default:
                        return String.Empty;
                }
            }
        }

        #endregion Public Properties
    }
}