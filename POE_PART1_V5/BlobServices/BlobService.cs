using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace POE_PART1_V5.Services
{
    public class BlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName = "venue-images";

        public BlobService(IConfiguration configuration)
        {
            string connectionString = configuration.GetSection("Storage:ConnectionString").Value;
            _containerName = configuration.GetSection("Storage:Container").Value ?? "venue-images";
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            // Create the container if it doesn't exist
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            // Create a unique name for the blob
            string blobName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            // Upload the file
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }

            // Return the URL to the blob
            return blobClient.Uri.ToString();
        }

        public async Task DeleteImageAsync(string blobUrlString)
        {
            if (string.IsNullOrEmpty(blobUrlString))
                return;

            try
            {
                // Extract the blob name from the URL
                Uri blobUri = new Uri(blobUrlString);
                string blobName = Path.GetFileName(blobUri.LocalPath);

                // Get the container client
                BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

                // Get the blob client and delete the blob
                BlobClient blobClient = containerClient.GetBlobClient(blobName);
                await blobClient.DeleteIfExistsAsync();
            }
            catch (Exception)
            {
                // Handle or log errors as needed
                // If the URL isn't a valid blob URL or there are other issues
            }
        }
    }
}