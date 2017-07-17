using System.Linq;
using System.Xml.Linq;

namespace Renamer.Models
{
    /// <summary>
    /// Collection of MAME software entries
    /// </summary>
    public class MameSoftwareCollection : ObservableContentCollection<MameSoftware>
    {
        #region Public Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileName">The name of the MAME XML file to load the data from</param>
        public MameSoftwareCollection(string fileName)
        {
            var mameXmlDocument = XDocument.Load(fileName);

            AddRange(mameXmlDocument.Root.Elements("software").Select(s => new MameSoftware()
            {
                Name = s.Attribute("name").Value,
                Description = s.Element("description").Value,
                RomName = s.Descendants("rom").First().Attribute("name").Value,
                Crc = s.Descendants("rom").First().Attribute("crc").Value,
            }));
        }

        #endregion Public Constructors
    }
}