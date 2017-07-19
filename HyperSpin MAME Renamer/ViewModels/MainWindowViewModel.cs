using Force.Crc32;
using Ionic.Zip;
using Renamer.Commands;
using Renamer.Extensions;
using Renamer.Models;
using Renamer.Properties;
using Renamer.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace Renamer.ViewModels
{
    /// <summary>
    /// Main Window View Model
    /// </summary>
    public class MainWindowViewModel : PropertyChangedBase
    {
        #region Private Fields

        private int _selectedTabIndex;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowViewModel()
        {
            BrowseHyperSpinXmlFileName = new RelayCommand(() => ExecuteBrowseHyperSpinXmlFileName());
            BrowseMameXmlFileName = new RelayCommand(() => ExecuteBrowseMameXmlFileName());
            BrowseRomFolder = new RelayCommand(() => ExecuteBrowseRomFolder());
            AddArtworkFolder = new RelayCommand(() => ExecuteAddArtworkFolder());
            RemoveArtworkFolder = new RelayCommand(() => ExecuteRemoveArtworkFolder(), () => CanRemoveArtworkFolder);
            BrowseOutputFolder = new RelayCommand(() => ExecuteBrowseOutputFolder());
            Start = new RelayCommand(() => ExecuteStart(), () => CanStart);
            Rename = new RelayCommand(() => ExecuteRename(), () => CanRename);

            GameMappings.CollectionChanged += GameMappings_CollectionChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets the title
        /// </summary>
        public static string Title
        {
            get { return Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyProductAttribute>().Product; }
        }

        /// <summary>
        /// Gets the AddArtworkFolder command
        /// </summary>
        public ICommand AddArtworkFolder { get; private set; }

        /// <summary>
        /// Gets the BrowseHyperSpinXmlFileName command
        /// </summary>
        public ICommand BrowseHyperSpinXmlFileName { get; private set; }

        /// <summary>
        /// Gets the BrowseMameXmlFileName command
        /// </summary>
        public ICommand BrowseMameXmlFileName { get; private set; }

        /// <summary>
        /// Gets the BrowseOutputFolder command
        /// </summary>
        public ICommand BrowseOutputFolder { get; private set; }

        /// <summary>
        /// Gets the BrowseRomFolder command
        /// </summary>
        public ICommand BrowseRomFolder { get; private set; }

        /// <summary>
        /// Gets a flag indicating whether the RemoveArtworkFolder command can be executed or not
        /// </summary>
        public bool CanRemoveArtworkFolder
        {
            get { return !String.IsNullOrWhiteSpace(SelectedArtworkFolder); }
        }

        /// <summary>
        /// Gets a flag indicating whether the Rename command can be executed or not
        /// </summary>
        public bool CanRename
        {
            get { return GameMappings.Any(g => g.SelectedMapping != null); }
        }

        /// <summary>
        /// Gets a flag indicating whether the Start command can be executed or not
        /// </summary>
        public bool CanStart
        {
            get { return !String.IsNullOrWhiteSpace(Settings.HyperSpinXmlFileName) && !String.IsNullOrWhiteSpace(Settings.MameXmlFileName) && !String.IsNullOrWhiteSpace(Settings.RomFolder) && !String.IsNullOrWhiteSpace(Settings.OutputFolder); }
        }

        /// <summary>
        /// The set of game mappings
        /// </summary>
        public GameMappingCollection GameMappings { get; } = new GameMappingCollection();

        /// <summary>
        /// The number of games that failed to rename
        /// </summary>
        public int GamesFailedToRename
        {
            get { return GameMappings.Count(g => g.Status == RenameStatus.Failed); }
        }

        /// <summary>
        /// The number of games in the list
        /// </summary>
        public int GamesInList
        {
            get { return GameMappings.Count; }
        }

        /// <summary>
        /// The number of games that have been successfully mapped
        /// </summary>
        public int GamesMapped
        {
            get { return GameMappings.Count(g => g.SelectedMapping != null); }
        }

        /// <summary>
        /// The number of games that have not been successfully mapped
        /// </summary>
        public int GamesNotMapped
        {
            get { return GameMappings.Count(g => g.SelectedMapping == null); }
        }

        /// <summary>
        /// The number of games that were renamed successfully
        /// </summary>
        public int GamesRenamedSuccessfully
        {
            get { return GameMappings.Count(g => g.Status == RenameStatus.Success); }
        }

        /// <summary>
        /// The number of games that have missing artwork
        /// </summary>
        public int GamesWithMissingArtwork
        {
            get { return GameMappings.Count(g => g.Status == RenameStatus.MissingArtwork); }
        }

        /// <summary>
        /// Gets the RemoveArtworkFolder command
        /// </summary>
        public ICommand RemoveArtworkFolder { get; private set; }

        /// <summary>
        /// Gets the Rename command
        /// </summary>
        public ICommand Rename { get; private set; }

        /// <summary>
        /// The selected artwork folder
        /// </summary>
        public string SelectedArtworkFolder { get; set; }

        /// <summary>
        /// The selected tab index
        /// </summary>
        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set { SetProperty(ref _selectedTabIndex, value); }
        }

        /// <summary>
        /// Gets the application settings
        /// </summary>
        public Settings Settings { get; } = new Settings();

        /// <summary>
        /// Gets the Start command
        /// </summary>
        public ICommand Start { get; private set; }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Invoke an action on the UI thread
        /// </summary>
        /// <param name="action">The action to be invoked</param>
        private static void InvokeOnUIThread(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }

        /// <summary>
        /// Execute the AddArtworkFolder command
        /// </summary>
        private void ExecuteAddArtworkFolder()
        {
            var folder = ServiceFactory.Get<IDialogService>().ShowFolderBrowserDialog();

            if (!String.IsNullOrWhiteSpace(folder))
            {
                Settings.ArtworkFolders.Add(folder);
            }
        }

        /// <summary>
        /// Execute the BrowseHyperSpinXmlFileName command
        /// </summary>
        private void ExecuteBrowseHyperSpinXmlFileName()
        {
            var fileName = ServiceFactory.Get<IDialogService>().ShowOpenFileDialog(Resources.XmlFileFilter);

            if (!String.IsNullOrWhiteSpace(fileName))
            {
                Settings.HyperSpinXmlFileName = fileName;
            }
        }

        /// <summary>
        /// Execute the BrowseMameXmlFileName command
        /// </summary>
        private void ExecuteBrowseMameXmlFileName()
        {
            var fileName = ServiceFactory.Get<IDialogService>().ShowOpenFileDialog(Resources.XmlFileFilter);

            if (!String.IsNullOrWhiteSpace(fileName))
            {
                Settings.MameXmlFileName = fileName;
            }
        }

        /// <summary>
        /// Execute the BrowseOutputFolder command
        /// </summary>
        private void ExecuteBrowseOutputFolder()
        {
            var folder = ServiceFactory.Get<IDialogService>().ShowFolderBrowserDialog();

            if (!String.IsNullOrWhiteSpace(folder))
            {
                Settings.OutputFolder = folder;
            }
        }

        /// <summary>
        /// Execute the BrowseRomFolder command
        /// </summary>
        private void ExecuteBrowseRomFolder()
        {
            var folder = ServiceFactory.Get<IDialogService>().ShowFolderBrowserDialog();

            if (!String.IsNullOrWhiteSpace(folder))
            {
                Settings.RomFolder = folder;
            }
        }

        /// <summary>
        /// Execute the RemoveArtworkFolder command
        /// </summary>
        private void ExecuteRemoveArtworkFolder()
        {
            if (!String.IsNullOrWhiteSpace(SelectedArtworkFolder))
            {
                Settings.ArtworkFolders.Remove(SelectedArtworkFolder);
            }
        }

        /// <summary>
        /// Execute the Rename command
        /// </summary>
        private void ExecuteRename()
        {
            if (!ValidateSettings())
            {
                return;
            }

            if (Directory.EnumerateFileSystemEntries(Settings.OutputFolder).Any())
            {
                if (!ServiceFactory.Get<IDialogService>().ShowConfirmationDialog(Resources.OutputFolderNotEmptyTitle, Resources.OutputFolderNotEmptyMessage))
                {
                    return;
                }
            }

            ServiceFactory.Get<IDialogService>().ShowProgressDialog(Resources.Processing, Resources.RenamingFiles, RenameFiles);
        }

        /// <summary>
        /// Execute the Start command
        /// </summary>
        private void ExecuteStart()
        {
            if (!ValidateSettings())
            {
                return;
            }

            ServiceFactory.Get<IDialogService>().ShowProgressDialog(Resources.Processing, Resources.MappingData, MapData);
        }

        /// <summary>
        /// Called when the game mappings collection changes
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">The event arguments</param>
        private void GameMappings_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("GamesInList");
            OnPropertyChanged("GamesMapped");
            OnPropertyChanged("GamesNotMapped");
            OnPropertyChanged("GamesRenamedSuccessfully");
            OnPropertyChanged("GamesFailedToRename");
            OnPropertyChanged("GamesWithMissingArtwork");
        }

        /// <summary>
        /// Initialise the output folder
        /// </summary>
        /// <param name="path">The path of the folder to be initialised</param>
        private void InitialiseOutputFolder(string path)
        {
            Directory.Delete(path, true);
            Directory.CreateDirectory(path);
            Directory.CreateDirectory(Path.Combine(path, Resources.Roms));

            var artworkRootFolder = Path.Combine(path, Resources.Artwork);
            foreach (var artworkFolder in Settings.ArtworkFolders)
            {
                Directory.CreateDirectory(Path.Combine(artworkRootFolder, artworkFolder.GetLastDirectoryName()));
            }
        }

        /// <summary>
        /// Peform the data mapping from the XML files
        /// </summary>
        private void MapData()
        {
            var dialogService = ServiceFactory.Get<IDialogService>();

            dialogService.ReportProgress(0, Resources.ReadingHyperSpinData);

            InvokeOnUIThread(() =>
            {
                GameMappings.LoadData(Settings.HyperSpinXmlFileName);
            });

            SelectedTabIndex = 1;

            dialogService.ReportProgress(0, Resources.ReadingMameData);
            var mameSoftwareList = new MameSoftwareCollection(Settings.MameXmlFileName);

            var index = 1;
            foreach (var gameMapping in GameMappings)
            {
                dialogService.ReportProgress(100 * index / GameMappings.Count, String.Format(CultureInfo.InvariantCulture, Resources.MappingProgress, index, GameMappings.Count, gameMapping.SourceName));

                if (Settings.AutoCrcMapping && !String.IsNullOrWhiteSpace(gameMapping.Crc))
                {
                    InvokeOnUIThread(() =>
                    {
                        gameMapping.Crc = gameMapping.Crc.ToUpperInvariant();
                    });

                    var mameSoftware = mameSoftwareList.FirstOrDefault(s => s.Crc.ToUpperInvariant() == gameMapping.Crc);

                    if (mameSoftware != null)
                    {
                        InvokeOnUIThread(() =>
                        {
                            gameMapping.PossibleMappings.Add(mameSoftware);
                            gameMapping.SelectedMapping = mameSoftware;
                            gameMapping.MappingMethod = Resources.Crc;
                        });
                    }
                }

                if (gameMapping.SelectedMapping == null)
                {
                    var cleanName = gameMapping.SourceName.Clean();

                    var matches = new List<FuzzyMatch>();
                    foreach (var mameSoftware in mameSoftwareList)
                    {
                        matches.Add(new FuzzyMatch()
                        {
                            Distance = cleanName.Distance(mameSoftware.Description.Clean()),
                            MameSoftware = mameSoftware
                        });
                    }

                    foreach (var match in matches.OrderBy(m => m.Distance))
                    {
                        gameMapping.PossibleMappings.Add(match.MameSoftware);

                        if (Settings.AutoExactNameMapping && match.Distance == 0)
                        {
                            InvokeOnUIThread(() =>
                            {
                                gameMapping.SelectedMapping = match.MameSoftware;
                                gameMapping.MappingMethod = Resources.ExactName;
                            });
                        }
                    }

                    if (Settings.AutoBestNameMapping && gameMapping.SelectedMapping == null)
                    {
                        InvokeOnUIThread(() =>
                        {
                            gameMapping.SelectedMapping = gameMapping.PossibleMappings.First();
                            gameMapping.MappingMethod = Resources.BestName;
                        });
                    }
                }

                index++;
            }
        }

        /// <summary>
        /// Perform the file renaming
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Exceptions are logged to the results grid.")]
        private void RenameFiles()
        {
            var dialogService = ServiceFactory.Get<IDialogService>();

            SelectedTabIndex = 2;

            InitialiseOutputFolder(Settings.OutputFolder);

            var romOutputFolder = Path.Combine(Settings.OutputFolder, Resources.Roms);
            var artworkOutputFolder = Path.Combine(Settings.OutputFolder, Resources.Artwork);

            var index = 1;
            foreach (var gameMapping in GameMappings)
            {
                try
                {
                    dialogService.ReportProgress(100 * index / GameMappings.Count, String.Format(CultureInfo.InvariantCulture, Resources.RenamingProgress, index, GameMappings.Count, gameMapping.SourceName));

                    if (gameMapping.SelectedMapping == null)
                    {
                        InvokeOnUIThread(() =>
                        {
                            gameMapping.Status = RenameStatus.Failed;
                            gameMapping.Details = Resources.NoMappingSelected;
                        });
                    }
                    else
                    {
                        var sourceRomFileName = Directory.EnumerateFiles(Settings.RomFolder, gameMapping.SourceName + ".*", SearchOption.TopDirectoryOnly).FirstOrDefault();
                        if (String.IsNullOrWhiteSpace(sourceRomFileName))
                        {
                            InvokeOnUIThread(() =>
                            {
                                gameMapping.Status = RenameStatus.Failed;
                                gameMapping.Details = Resources.RomFileNotFound;
                            });
                        }
                        else if (Settings.CrcCheck)
                        {
                            var buffer = File.ReadAllBytes(sourceRomFileName);
                            var crc = Crc32Algorithm.Compute(buffer).ToString("X8", CultureInfo.InvariantCulture);
                            if (crc != gameMapping.SelectedMapping.Crc.ToUpperInvariant())
                            {
                                InvokeOnUIThread(() =>
                                {
                                    gameMapping.Status = RenameStatus.Failed;
                                    gameMapping.Details = Resources.CrcNotMatched;
                                });
                            }
                        }

                        if (gameMapping.Status != RenameStatus.Failed)
                        {
                            var targetRomFileName = Path.Combine(romOutputFolder, gameMapping.SelectedMapping.RomName);
                            File.Copy(sourceRomFileName, targetRomFileName);

                            var zipFileName = Path.Combine(romOutputFolder, gameMapping.SelectedMapping.Name + ".zip");
                            using (var zipFile = File.Exists(zipFileName) ? ZipFile.Read(zipFileName) : new ZipFile(zipFileName))
                            {
                                zipFile.AddFile(targetRomFileName, String.Empty);
                                zipFile.Save();
                            }

                            File.Delete(targetRomFileName);

                            var missingArtwork = new List<String>();
                            foreach (var artworkFolder in Settings.ArtworkFolders)
                            {
                                var artworkFolderName = artworkFolder.GetLastDirectoryName();
                                var sourceArtworkFileName = Directory.EnumerateFiles(artworkFolder, gameMapping.SourceName + ".*", SearchOption.TopDirectoryOnly).FirstOrDefault();

                                if (String.IsNullOrWhiteSpace(sourceArtworkFileName))
                                {
                                    missingArtwork.Add(artworkFolderName);
                                }
                                else
                                {
                                    var targetArtworkFileName = Path.Combine(artworkOutputFolder, artworkFolderName, gameMapping.SelectedMapping.Name + Path.GetExtension(sourceArtworkFileName));
                                    File.Copy(sourceArtworkFileName, targetArtworkFileName, true);
                                }
                            }

                            if (missingArtwork.Any())
                            {
                                InvokeOnUIThread(() =>
                                {
                                    gameMapping.Status = RenameStatus.MissingArtwork;
                                    gameMapping.Details = String.Format(CultureInfo.InvariantCulture, Resources.MissingArtworkDetails, String.Join(", ", missingArtwork));
                                });
                            }
                            else
                            {
                                InvokeOnUIThread(() =>
                                {
                                    gameMapping.Status = RenameStatus.Success;
                                });
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    InvokeOnUIThread(() =>
                    {
                        gameMapping.Status = RenameStatus.Failed;
                        gameMapping.Details = e.Message;
                    });
                }
                index++;
            }

            GameMappings.SaveData(Path.Combine(Settings.OutputFolder, Path.GetFileName(Settings.HyperSpinXmlFileName)));
        }

        /// <summary>
        /// Validate the settings
        /// </summary>
        /// <returns>True if validation was successful, false if not.</returns>
        private bool ValidateSettings()
        {
            if (!File.Exists(Settings.HyperSpinXmlFileName))
            {
                ServiceFactory.Get<IDialogService>().ShowErrorMessageDialog(Resources.Error, String.Format(CultureInfo.InvariantCulture, Resources.FileNotFound, Settings.HyperSpinXmlFileName));
                return false;
            }

            if (!File.Exists(Settings.MameXmlFileName))
            {
                ServiceFactory.Get<IDialogService>().ShowErrorMessageDialog(Resources.Error, String.Format(CultureInfo.InvariantCulture, Resources.FileNotFound, Settings.MameXmlFileName));
                return false;
            }

            if (!Directory.Exists(Settings.RomFolder))
            {
                ServiceFactory.Get<IDialogService>().ShowErrorMessageDialog(Resources.Error, String.Format(CultureInfo.InvariantCulture, Resources.FolderDoesNotExist, Settings.RomFolder));
                return false;
            }

            foreach (var artworkFolder in Settings.ArtworkFolders)
            {
                if (!Directory.Exists(artworkFolder))
                {
                    ServiceFactory.Get<IDialogService>().ShowErrorMessageDialog(Resources.Error, String.Format(CultureInfo.InvariantCulture, Resources.FolderDoesNotExist, artworkFolder));
                    return false;
                }
            }

            if (!Directory.Exists(Settings.OutputFolder))
            {
                ServiceFactory.Get<IDialogService>().ShowErrorMessageDialog(Resources.Error, String.Format(CultureInfo.InvariantCulture, Resources.FolderDoesNotExist, Settings.OutputFolder));
                return false;
            }

            return true;
        }

        #endregion Private Methods
    }
}