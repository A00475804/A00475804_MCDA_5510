using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ProgAssign1
{
    internal class FileReader
    {
        bool outputFileExist = true;
        public void CsvReadWrite(String path, string outputPath, string logFilePath)
        {
            // Create or append to the log file
            using var logWriter = new StreamWriter(logFilePath, true);

            //checking if output file is present if it is already present then delete it
            if (File.Exists(outputPath) && outputFileExist)
            {
                File.Delete(outputPath);
                outputFileExist = false;
            }
            using var streamReader = new StreamReader(path);
            using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);
            using var writer = new StreamWriter(outputPath, true);
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));

            // header for csv
            String header = "First Name,Last Name,Street Number,Street,City,Province,Postal" +
                            " Code,Country,Phone Number,email Address,Date";

            //checking if file has 0 rows so header can be inserted
            if (writer.BaseStream.Length == 0)
                writer.WriteLine(header);

            try
            {
                // Extract the date from the path using regular expressions
                string datePart = ExtractDateFromPath(path);
                // Replace double backslashes with hyphens
                datePart = datePart.Replace("\\", "-");

                if (!DateTime.TryParse(datePart, out DateTime date))
                {
                    Console.WriteLine("Unable to extract a valid date from the path.  " + date.ToString());
                }
                //
                var records = new List<Customer>();
                while (csvReader.Read())
                {
                    try
                    {
                        records.Add(csvReader.GetRecord<Customer>());
                        if (records.Count >= 5000)
                        {
                            checkData(records, csv, date);
                            records.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        DataSummary.totalMissingData += 1;
                        logWriter.WriteLine("\n path of file where error occured : " + Path.GetFileName(path));
                        logWriter.WriteLine("Exception:  \n" + ex.StackTrace);
                    }
                }
                // Process any remaining records
                checkData(records, csv, date);
            }
            catch (Exception ex)
            {
                DataSummary.totalMissingData += 1;
                logWriter.WriteLine("\n path of file where error occured : " + Path.GetFileName(path));
                logWriter.WriteLine("Exception:  \n" + ex.StackTrace);
            }
        }

        public void checkData(List<Customer> data, CsvWriter csv, DateTime date)
        {
            foreach (var customer in data)
            {
                if (string.IsNullOrEmpty(customer.firstName) || string.IsNullOrEmpty(customer.lastName)
                    || string.IsNullOrEmpty(customer.street) || string.IsNullOrEmpty(customer.city)
                    || string.IsNullOrEmpty(customer.province) || string.IsNullOrEmpty(customer.postalCode)
                    || string.IsNullOrEmpty(customer.country) || string.IsNullOrEmpty(customer.email)
                    || (customer.streetNumber < 1) || (customer.phoneNumber < 1) || (!EmailCheck(customer.email)))
                {
                    DataSummary.totalMissingData += 1;
                    continue;
                }
                else
                {
                    DataSummary.totalCorrectData += 1;
                    csv.WriteRecord(customer);
                    csv.WriteField(date.ToString("yyyy/MM/dd"));
                    csv.NextRecord();
                }
            }
            csv.Flush();
        }

        public static string ExtractDateFromPath(string path)
        {
            // Define a regular expression pattern to match the date (yyyy\MM\dd) in the path
            string pattern = @"\d{4}\\\d{1,2}\\\d{1,2}";
            Match match = Regex.Match(path, pattern);

            if (match.Success)
            {
                return match.Value;
            }
            return null;
        }

        public static bool EmailCheck(string email)
        {
            // Use a regular expression to validate the email address
            string pattern = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";
            return Regex.IsMatch(email, pattern);
        }
    }
}
