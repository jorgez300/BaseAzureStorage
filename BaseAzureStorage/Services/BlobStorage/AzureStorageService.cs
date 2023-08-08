using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace BaseAzureStorage.Services.BlobStorage
{
    public class AzureStorageService
    {
        private readonly string _storageAccount = "devstoreaccount1";
        private readonly string _accessKey = "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==";
        private readonly string _blobUri = $"http://127.0.0.1:10000/devstoreaccount1";
        private readonly BlobServiceClient _blobServiceClient;


        public AzureStorageService()
        {
            var credential = new StorageSharedKeyCredential(_storageAccount, _accessKey);
            _blobServiceClient = new BlobServiceClient(new Uri(_blobUri), credential);
        }


        public async Task ListBlobContainersAsync()
        {
            var containers = _blobServiceClient.GetBlobContainersAsync();
            await foreach (var container in containers)
            {
                Console.WriteLine(container.Name);
            }
        }

        public async Task<List<Uri>> UploadFileAsync()
        {
            var blobUris = new List<Uri>();
            string filePath = "hello.txt";
            var blobContainer = _blobServiceClient.GetBlobContainerClient("local");

            var blob = blobContainer.GetBlobClient($"carpeta2/{filePath}");



            await blob.UploadAsync(filePath, true);
            blob.SetAccessTier(AccessTier.Cool);
            blobUris.Add(blob.Uri);


            return blobUris;
        }

        public async Task<List<Uri>> UploadFileAsync(Stream file, string Carpeta, string Archivo)
        {
            var blobUris = new List<Uri>();
            var blobContainer = _blobServiceClient.GetBlobContainerClient("local");

            var blob = blobContainer.GetBlobClient($"{Carpeta}/{Archivo}");



            await blob.UploadAsync(file, true);
            blob.SetAccessTier(AccessTier.Cool);
            blobUris.Add(blob.Uri);


            return blobUris;
        }

        public async Task DeleteFileAsync(string Carpeta, string Archivo)
        {

            var blobContainer = _blobServiceClient.GetBlobContainerClient("local");

            var blob = blobContainer.GetBlobClient($"{Carpeta}/{Archivo}");
            await blob.DeleteIfExistsAsync();

        }

        public async Task<dynamic> DownloadFileAsync(string Carpeta, string Archivo)
        {

            var blobContainer = _blobServiceClient.GetBlobContainerClient("local");

            var blob = blobContainer.GetBlobClient($"{Carpeta}/{Archivo}");

            if (await blob.ExistsAsync())
            {

                var data = await blob.OpenReadAsync();
                Stream blobContent = data;

                var content = await blob.DownloadContentAsync();

                string name = Archivo;
                string contentType = content.Value.Details.ContentType;

                return new
                {
                    content = blobContent,
                    contentType = contentType,
                    name = name
                };

            }

            return null;

        }

        public async Task<List<string>> ListFileAsync(string? Carpeta = null)
        {
            List<string> list = new List<string>();

            var blobContainer = _blobServiceClient.GetBlobContainerClient("local");

            var blobs = blobContainer.GetBlobs();

            foreach (BlobItem _item in blobs)
            {

                if (Carpeta == null)
                {
                    list.Add(_item.Name);
                }
                else
                {
                    if (_item.Name.Contains($"{Carpeta}/"))
                    {
                        list.Add(_item.Name);
                    }
                }
            }

            return list;

        }

    }
}
