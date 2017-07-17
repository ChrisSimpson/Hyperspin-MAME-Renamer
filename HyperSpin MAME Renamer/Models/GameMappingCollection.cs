using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Renamer.Models
{
    /// <summary>
    /// Collection of game mappings
    /// </summary>
    public class GameMappingCollection : ObservableContentCollection<GameMapping>
    {
        #region Private Fields

        private Dictionary<string, GameMapping> _gameMappingLookup = new Dictionary<string, GameMapping>();
        private XDocument _hyperSpinXmlDocument;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Load the data into the collection
        /// </summary>
        /// <param name="fileName">The name of the HyperSpin XML file to load the data from</param>
        public void LoadData(string fileName)
        {
            _hyperSpinXmlDocument = XDocument.Load(fileName);

            Clear();
            AddRange(_hyperSpinXmlDocument.Root.Elements("game").Select(g => new GameMapping()
            {
                SourceName = g.Attribute("name").Value,
                Crc = g.Element("crc").Value
            }));

            _gameMappingLookup = this.ToDictionary(g => g.SourceName);
        }

        /// <summary>
        /// Save the data stored in the collection
        /// </summary>
        /// <param name="fileName">The name of the HyperSpin XML file to save the data to</param>
        public void SaveData(string fileName)
        {
            var outputXmlDocument = new XDocument(_hyperSpinXmlDocument);

            foreach (var element in outputXmlDocument.Root.Elements("game").ToList())
            {
                var gameMapping = _gameMappingLookup[element.Attribute("name").Value];
                if (gameMapping.Status == RenameStatus.Failed)
                {
                    element.Remove();
                }
                else
                {
                    element.Attribute("name").Value = gameMapping.SelectedMapping.Name;
                    element.Element("crc").Value = gameMapping.SelectedMapping.Crc.ToUpperInvariant();
                }
            }

            outputXmlDocument.Save(fileName, SaveOptions.None);
        }

        #endregion Public Methods
    }
}