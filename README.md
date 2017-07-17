# Hyperspin-MAME-Renamer

HyperSpin MAME Renamer is a Windows application that will take a HyperSpin XML file, a MAME softlist XML file, a folder containing ROMs, and multiple artwork folders. It will then use this information to rename the ROMs and artwork files so that they correspond to the MAME short name.

The application works in two stages. When you have entered your file/folder details it will process the data in the two XML files and attempt to automatically map the data. Various options for how data should be mapped are available:

* Automatically map games based on CRC values - When a game has the same CRC value in the HyperSpin database and the MAME XML file then it will be considered a match.
* Automatically map games where the name is an exact match (excluding ROM tags) - If a game cannot be matched by CRC then it will perform a fuzzy name search. If this option is selected then any games with the same name, apart from ROM tags such as (Europe), (USA) etc. will be considered a match.
* Automatically select the best name where no exact match is available - If a game cannot be matched by CRC or exact name then the best match will be automatically selected. This option is off by default.
You will be able to review the mappings on the "Mappings" tab, as well as manually amend the mappings by using the drop-down in the "Map To" column. This games listed in the drop-down are ordered by how close the names match. Note that when a game was matched by CRC only that game will appear in the list since we can be certain we have the right game and therefore don't need to do the fuzzy name search.

Once you are happy with the mappings, click the "Rename" button to proceed. The application will then use the mappings to rename the files. Optionally, the application can also verify that the ROM files match the CRC values in the MAME XML file as part of this process.

The files will be written to the output folder in the following structure:

* HyperSpin XML file.xml - A copy of the HyperSpin database file, with the names updated to use the MAME short names and the CRC values updated to those expected by MAME.
* "Roms" folder - The ROMs folder will contain the renamed ROMs. These will be automatically put into .zip files so that they can be used with MAME.
* "Artwork" folder - This folder will contain sub-folders for each artwork folder you selected. The names of the sub-folders will match the names of the input artwork folders e.g. if you selected C:\Users\Me\Downloads\Artwork4 then you will get a sub-folder called "Artwork4"
