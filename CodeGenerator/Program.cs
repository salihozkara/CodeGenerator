using System.Globalization;



if (args.Length != 3)
{
    Console.WriteLine($"Usage: { (object)Environment.GetCommandLineArgs()[0]} <input_file> <input_srcStr> <output_destStr>");
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
var fileCount = 0;
var completeCount = 0;
var errorCount = 0;
if (Directory.Exists(sourcePath))
{
    var sourceFiles = Directory.GetFiles(sourcePath, "", SearchOption.AllDirectories);
    fileCount = sourceFiles.Length;

    var sourceDir = Directory.GetDirectories(sourcePath, "", SearchOption.AllDirectories);
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
    return;
}
Console.WriteLine("{0} files processed, {1} complete, {2} errors,{3} total", fileCount, completeCount, errorCount, completeCount + errorCount);
void FileContentSwitcher(string file, string srcStr, string destStr)
{
    try
    {
        var fileContent = File.ReadAllText(file);
        fileContent = fileContent.ReplaceByCase(srcStr, destStr);
        File.WriteAllText(file, fileContent);
    }
    catch (Exception e)
    {
        if (e is AccessViolationException)
        {
            Console.WriteLine("Access violation exception");
        }
        else
        {
            Console.WriteLine("Exception: " + e.Message);
        }
    }
}

void FileCreator(string file, string srcStr, string destSrc)
{
    var newFile = file.ReplaceByCase(srcStr, destSrc);
    var fileInfo = new FileInfo(newFile);

    try
    {
        if (fileInfo.Directory is { Exists: false })
        {
            fileInfo.Directory.Create();
        }
        File.Copy(file, newFile, true);
        FileContentSwitcher(newFile, srcStr, destSrc);
        completeCount++;
    }
    catch (Exception e)
    {
        errorCount++;
        Console.WriteLine("File Exception:" + e.Message);
    }

}

void DirCreator(string dir, string srcStr, string destStr)
{
    var newDir = dir.ReplaceByCase(srcStr, destStr);
    try
    {
        Directory.CreateDirectory(newDir);
    }
    catch (Exception e)
    {
        Console.WriteLine("Directory Exception:"+e.Message);
    }
}

static class StringExtensions
{
    public static string ReplaceByCase(this string str, string oldValue, string newValue)
    {
        str = str.Replace(oldValue, newValue);
        // upper replace
        str = str.Replace(oldValue.ToUpper(), newValue.ToUpper());
        // lower replace
        str = str.Replace(oldValue.ToLower(), newValue.ToLower());
        // title replace
        str = str.Replace(oldValue.ToTitle(), newValue.ToTitle());
        return str;
    }
    // ToTitle
    public static string ToTitle(this string str)
    {
        return new CultureInfo("en-US", false).TextInfo.ToTitleCase(str.ToLower());
    }
}