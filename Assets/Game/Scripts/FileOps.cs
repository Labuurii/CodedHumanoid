using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class FileOps
{
    public static string CreateTmpFile(string extension)
    {
        var path = Path.GetTempPath() + stripped_guid();
        path = Path.ChangeExtension(path, extension);
        return path;
    }

    private static string stripped_guid()
    {
        //names starting with numbers are generally not supported in programming langs
        //- are generally now allowed in programming langs

        var s = Guid.NewGuid().ToString();
        if (s[0] >= '0' && s[1] <= '9')
            s = 'A' + s;
        return s.Replace('-', '_');
    }

    public static string CreateTmpDir()
    {
        var dir = Path.Combine(Path.GetTempPath(), stripped_guid());
        Directory.CreateDirectory(dir);
        return dir;
    }
}
