namespace a3innuva.Importia.Validations.Console
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using a3innuva.TAA.Migration.SDK.Extensions;
    using a3innuva.TAA.Migration.SDK.Interfaces;
    using a3innuva.TAA.Migration.SDK.Serialization;
    using Newtonsoft.Json;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            string path = args[0];

            Stream stream = File.OpenRead(path);
            var text = await ValidateFile(stream, path);

            Console.WriteLine(text);
        }

        private static async Task<string> ValidateFile(Stream stream, string fileName)
        {
            try
            {
                string json;
                StringBuilder sb = new StringBuilder();

                Stream streamToRead = fileName.Contains(".zip") ? await UnZipStreamAsync(stream) : stream;

                using (StreamReader reader = new StreamReader(streamToRead, Encoding.UTF8))
                {
                    json = await reader.ReadToEndAsync();
                }

                var settings = JsonSerializationSettingsUtils.GetSettings();

                IMigrationSet set =
                    JsonConvert.DeserializeObject<IMigrationSet>(json, settings);

                var result = set.IsValid();

                sb.AppendLine($"Fichero: {fileName}");
                sb.AppendLine($"Migration set es válido: {result.isValid}");
                sb.AppendLine($"Migration info es válido: {set.Info.IsValid()}");
                sb.AppendLine($"Todas las entidades son válidas: {!result.errors.Any()}");

                if (result.errors.Any())
                {
                    foreach (var item in result.errors)
                    {
                        sb.AppendLine($"Línea : {item.Line} - {item.Code}");
                    }
                }

                return sb.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException?.Message ?? e.Message);
                return e.InnerException?.Message ?? e.Message;
            }
        }

        private static async Task<Stream> UnZipStreamAsync(Stream input)
        {
            using var zip = new ZipArchive(input, ZipArchiveMode.Read);
            if (!zip.Entries.Any())
                return null;

            using var zipStream = zip.Entries[0].Open();
            MemoryStream ms = new MemoryStream();

            await zipStream.CopyToAsync(ms).ConfigureAwait(false);
            ms.Position = 0;
            return ms;
        }
    }
}
