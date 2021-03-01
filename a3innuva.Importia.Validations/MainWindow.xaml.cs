namespace a3innuva.Migration.Validation
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using a3innuva.TAA.Migration.SDK.Extensions;
    using a3innuva.TAA.Migration.SDK.Interfaces;
    using a3innuva.TAA.Migration.SDK.Serialization;
    using Microsoft.Win32;
    using Newtonsoft.Json;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Stream stream = File.OpenRead(openFileDialog.FileName);
                resultView.Text = await this.ValidateFile(stream, openFileDialog.FileName);
            }
                
        }

        private async Task<string> ValidateFile(Stream stream, string fileName)
        {
            string text;

            try
            {
                StringBuilder sb = new StringBuilder();

                Stream streamToRead = fileName.Contains(".zip") ? await this.UnZipStreamAsync(stream) : stream;

                using (StreamReader reader = new StreamReader(streamToRead, Encoding.UTF8))
                {
                    text = await reader.ReadToEndAsync();
                }
            

                IMigrationSet set = JsonConvert.DeserializeObject<IMigrationSet>(text, JsonSerializationSettingsUtils.GetSettings());

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
                text = e.Message;
            }

            return text;
        }

        private async Task<Stream> UnZipStreamAsync(Stream input)
        {
            using (var zip = new ZipArchive(input, ZipArchiveMode.Read))
            {
                if (!zip.Entries.Any())
                    return null;

                using (var zipStream = zip.Entries[0].Open())
                {
                    MemoryStream ms = new MemoryStream();

                    await zipStream.CopyToAsync(ms).ConfigureAwait(false);
                    ms.Position = 0;
                    return ms;
                }
            }
        }
    }
}
