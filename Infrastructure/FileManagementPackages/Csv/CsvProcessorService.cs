using CsvHelper;
using System.Globalization;

namespace Infrastructure.FileManagementPackages.Csv
{
    public class CsvProcessorService
    {
        public MemoryStream ExportCsv<T>(List<T> result)
        {
            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream);
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(result);

            return memoryStream;
        }
    }
}
