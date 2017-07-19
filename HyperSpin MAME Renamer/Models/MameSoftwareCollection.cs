using Renamer.Properties;
using Renamer.Services;
using System;
using System.Globalization;
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

            var softwareElements = mameXmlDocument.Root.Elements("software");
            foreach (var softwareElement in softwareElements)
            {
                var romItem = 1;
                var romElements = softwareElement.Descendants("rom");

                foreach (var romElement in romElements)
                {
                    Add(new MameSoftware()
                    {
                        Name = softwareElement.Attribute("name").Value,
                        Description = softwareElement.Element("description").Value + (romElements.Count() > 1 ? " #" + romItem.ToString(CultureInfo.InvariantCulture) : String.Empty),
                        RomName = romElement.Attribute("name").Value,
                        Crc = romElement.Attribute("crc").Value
                    });
                    romItem++;
                }
            }

            if (!this.Any())
            {
                ServiceFactory.Get<IDialogService>().ShowErrorMessageDialog(Resources.Error, Resources.MameXmlError);
            }
        }

        #endregion Public Constructors
    }
}