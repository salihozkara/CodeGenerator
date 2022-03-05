using System.Globalization;



if (args.Length != 3)
{
    Console.WriteLine("Usage: {0} <input_file> <input_srcStr> <output_destStr>",
        Environment.GetCommandLineArgs()[0]);
    return;
}

var sourcePath = Path.GetFullPath(args[0]);

var sourceString = args[1];
var destString = args[2];
if (sourceString == destString)
{
    Console.WriteLine("Source and destination strings are the same");
    return;
}

if (Directory.Exists(sourcePath))
{
    var sourceFiles = Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories);
    var sourceDir = Directory.GetDirectories(sourcePath, "*.*", SearchOption.AllDirectories);
    foreach (var dir in sourceDir)
    {
        DirCreator(dir, sourceString, destString);
    }

    foreach (var file in sourceFiles)
    {
        FileCreator(file, sourceString, destString);
    }
}
else if (File.Exists(sourcePath))
{
    FileCreator(sourcePath, sourceString, destString);
}
else
{
    Console.WriteLine("File or Directory not found");
}

void FileContentSwitcher(string file, string srcStr, string destStr)
{
    var fileContent = File.ReadAllText(file);
    fileContent = fileContent.Replace(srcStr, destStr);
    fileContent = fileContent.Replace(srcStr.ToTitleCase(), destStr.ToTitleCase());
    fileContent = fileContent.Replace(srcStr.ToUpper(), destStr.ToUpper());
    fileContent = fileContent.Replace(srcStr.ToLower(), destStr.ToLower());
    File.WriteAllText(file, fileContent);
}

void FileCreator(string file, string srcStr, string destSrc)
{
    var newFile = file.Replace(srcStr, destSrc);
    newFile = newFile.Replace(srcStr.ToTitleCase(), destSrc.ToTitleCase());
    newFile = newFile.Replace(srcStr.ToUpper(), destSrc.ToUpper());
    newFile = newFile.Replace(srcStr.ToLower(), destSrc.ToLower());
    File.Copy(file, newFile);
    FileContentSwitcher(newFile, srcStr, destSrc);
}

void DirCreator(string dir, string srcStr, string destStr)
{
    var newDir = dir.Replace(srcStr, destStr);
    newDir = newDir.Replace(srcStr.ToTitleCase(), destStr.ToTitleCase());
    newDir = newDir.Replace(srcStr.ToUpper(), destStr.ToUpper());
    newDir = newDir.Replace(srcStr.ToLower(), destStr.ToLower());
    Directory.CreateDirectory(newDir);
}


// string to title extends string
internal static class StringExtensions
{
    public static string ToTitleCase(this string str)
    {
        return new CultureInfo("en-US", false).TextInfo.ToTitleCase(str.ToLower());
    }
}