using System;
using System.IO;
using System.Reflection;

namespace UNIB.Unpacker
{
    class Utils
    {
        public static String iGetApplicationPath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public static String iGetApplicationVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public static void iSetInfo(String m_String)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(m_String);
            Console.ResetColor();
        }

        public static void iSetError(String m_String)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(m_String + "!");
            Console.ResetColor();
        }

        public static void iSetWarning(String m_String)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(m_String + "!");
            Console.ResetColor();
        }

        public static String iCheckArgumentsPath(String m_Arg)
        {
            // FIXED: Use the system-specific directory separator character
            // This will be '\' on Windows and '/' on Linux/macOS
            char separator = Path.DirectorySeparatorChar;

            // Ensure the path ends with the correct separator
            if (!m_Arg.EndsWith(separator.ToString()))
            {
                m_Arg += separator;
            }
            return m_Arg;
        }

        public static void iCreateDirectory(String m_Directory)
        {
            // FIXED: This original logic was flawed.
            // It tried to get the directory name of a path that might already BE a directory.
            // This is a more robust way to ensure a directory exists.
            if (!Directory.Exists(m_Directory))
            {
                Directory.CreateDirectory(m_Directory);
            }
        }
    }
}
