# MP4DurationEditor 
#### Edit the duration of MP4 files without corrupting them!
A simple single-file C# script that edits bytes of MP4 files with the duration you want.

**WARNING:** this program only edits the bytes of the file, it won't copy the content of the file to fit the duration.
## Idea
I was inspired by infinite discord videos, so i wanted to automate that.
## Build
To run this script, you'll need to build it.
It uses .NET Core, which you can install here: https://dotnet.microsoft.com/download

1. Clone the repository: `git clone https://github.com/NedoProgrammer/MP4DurationEditor.git`

2. `cd` into the project directory: `cd MP4DurationEditor`

3. Build the project: `dotnet build --configuration Release`  

When finished, you'll find the executable in `/bin/Release/netcoreapp3.1/`
## Dependencies
ExifTool for getting video's metadata: https://exiftool.org/

NExifTool - a wrapper for ExifTool: https://github.com/AerisG222/NExifTool

Hexify for editing the hex of the mp4 file: https://github.com/lowleveldesign/hexify
## Usage
Simply run the executable, and follow the on-screen instructions :)
