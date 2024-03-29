using System.Text.RegularExpressions;

foreach (var file in Directory.EnumerateFiles(Environment.CurrentDirectory, @"*.txt"))
{
    var outFile = Path.ChangeExtension(file, ".txt.out");

    var contents = File.ReadAllText(file);
    contents = contents.Replace("\r\n", "\n");

    var splitAt = contents.IndexOf("\n\n");
    var (table, script) = (contents[0..splitAt], contents[(splitAt + 2)..]);

    var m = RegexOptions.Multiline;
    script = Regex.Replace(script, @"[ \t]*(//|#)[^\r\n]+", ""); // Remove comments.
    script = Regex.Replace(script, @"[ \t]+", " "); // Collapse whitespace.
    script = Regex.Replace(script, @"^[ \t]+|[ \t]+$", "", m); // Remove leading/trailing whitespace.
    script = Regex.Replace(script, @"\n+", "\n").Trim(); // Remove blank lines.
    script = Regex.Replace(script, @"[ \t]*(\<=?|\>=?|==|{|})[ \t]*", "$1").Trim(); // Remove blank lines.
    script = Regex.Replace(script, @"(?<=\d)[ \t]*(?:s(?:ec(?:onds?)?))", "s").Trim(); // Simplify units.

    contents = $"{table}\n\n{script}";
    File.WriteAllText(outFile, contents);
}
