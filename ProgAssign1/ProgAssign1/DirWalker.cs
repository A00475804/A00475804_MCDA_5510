using System.Diagnostics;

namespace ProgAssign1
{
    internal class DirWalker
    {
        readonly FileReader reader = new();
        public void walk(string path, string outputPath, string logFilePath)
        {
            List<string> fileList = new List<string>(Directory.EnumerateFiles(path, "*.csv", SearchOption.AllDirectories));
            foreach (string filepath in fileList)
            {
                reader.CsvReadWrite(filepath, outputPath, logFilePath);
            }
        }

        public static void Main(string[] args)
        {
            // Stopwatch to check the time taken for execution
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string defaultInputPath = "C://Users//mehta//Desktop//smu//programming//ASSIGNMENT//c#//Sample Data//";
            string defaultOutputPath = "C:\\Users\\mehta\\Desktop\\smu\\programming\\ASSIGNMENT\\c#\\ProgAssign1\\output\\output.csv";
            string defaultLogFilePath = "C:\\Users\\mehta\\Desktop\\smu\\programming\\ASSIGNMENT\\c#\\ProgAssign1\\log\\log.txt";

            Console.WriteLine("Please input the path for data input");
            string inputPath = Console.ReadLine();

            string outputPath;
            string logFilePath;

            try
            {
                if (!string.IsNullOrWhiteSpace(inputPath) && Directory.Exists(Path.GetDirectoryName(inputPath)))
                {
                    Console.WriteLine("The new input path: " + inputPath);
                }
                else
                {
                    Console.WriteLine("Invalid input path or directory does not exist. Using the default input path.");
                    inputPath = defaultInputPath;
                }

                Console.WriteLine("Please input the path for the output file (or press Enter to use the default):");
                outputPath = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(outputPath))
                {
                    outputPath = defaultOutputPath;
                }

                if (IsFilePathValid(outputPath))
                {
                    Console.WriteLine("Output file path is valid: " + outputPath);
                }
                else
                {
                    Console.WriteLine("Invalid output file path.");
                }

                Console.WriteLine("\nPlease input the path for the log file (or press Enter to use the default):");
                logFilePath = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(logFilePath))
                {
                    logFilePath = defaultLogFilePath;
                }

                if (IsFilePathValid(logFilePath))
                {
                    Console.WriteLine("Log file path is valid: " + logFilePath);
                }
                else
                {
                    Console.WriteLine("Invalid log file path.");
                }

                DirWalker fw = new();
                Console.WriteLine("\nThe code has started running with input path: " + inputPath);
                fw.walk(inputPath, outputPath,logFilePath);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("The file or directory cannot be found.");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("The file or directory cannot be found.");
            }
            catch (DriveNotFoundException)
            {
                Console.WriteLine("The drive specified in 'path' is invalid.");
            }
            catch (PathTooLongException)
            {
                Console.WriteLine("'path' exceeds the maximum supported path length.");
            }

            sw.Stop();
            TimeSpan timeSpan = sw.Elapsed;

            // Putting all the data into the console
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n Total Execution time: " + timeSpan);
            Console.WriteLine("\n The wrong data found is : " + DataSummary.totalMissingData);
            Console.WriteLine("\n The correct data found is : " + DataSummary.totalCorrectData);
            Console.WriteLine("\n The total data found is : " + (DataSummary.totalMissingData + DataSummary.totalCorrectData));
            Console.ResetColor();
            Console.WriteLine("\n Execution end");
        }

        public static bool IsFilePathValid(string path)
        {
            try
            {
                string directory = Path.GetDirectoryName(path);
                return !string.IsNullOrWhiteSpace(directory) && Directory.Exists(directory);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
