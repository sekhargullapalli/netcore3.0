using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIAFactbook18App.Data
{
    public class AzureBlbStorageService
    {
        public AzureBlbStorageService(string connectionstring) { ConnectionString = connectionstring; }
        public string ConnectionString { get; set; } = "";
        public CloudBlobClient StorageClient => CloudStorageAccount.Parse(ConnectionString).CreateCloudBlobClient();
        public async Task<List<string>> GetBlockBlobList(string targetContainer)
        {
            List<string> _list = new List<string>();
            CloudBlobContainer container = StorageClient.GetContainerReference(targetContainer);
            BlobContinuationToken token = null;
            do
            {
                BlobResultSegment resultSegment = await container.ListBlobsSegmentedAsync(token);
                token = resultSegment.ContinuationToken;
                foreach (IListBlobItem item in resultSegment.Results)
                    if (item.GetType() == typeof(CloudBlockBlob))
                        _list.Add(((CloudBlockBlob)item).Name);
            } while (token != null);
            return _list;
        }
        public async Task<bool> ContainsFile(string filename, string targetContainer)
        {
            var list = await GetBlockBlobList(targetContainer);
            return list.Where(x => x == filename).FirstOrDefault() != null;
        }
        public async Task UploadFile(string targetContainer, string filename, byte[] fileData)
        {
            CloudBlobContainer container = StorageClient.GetContainerReference(targetContainer);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);
            await blockBlob.UploadFromByteArrayAsync(fileData, 0, fileData.Length);
        }
        public async Task DeleteFile(string targetContainer, string filename)
        {
            CloudBlobContainer container = StorageClient.GetContainerReference(targetContainer);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);
            await blockBlob.DeleteIfExistsAsync();
        }
        public async Task<byte[]> GetFileStream(string targetContainer, string filename)
        {
            CloudBlobContainer container = StorageClient.GetContainerReference(targetContainer);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);
            await blockBlob.FetchAttributesAsync();
            long fileByteLength = blockBlob.Properties.Length;
            byte[] fileContent = new byte[fileByteLength];
            for (int i = 0; i < fileByteLength; i++)
                fileContent[i] = 0x20;
            await blockBlob.DownloadToByteArrayAsync(fileContent, 0);
            return fileContent;
        }
    }
}
