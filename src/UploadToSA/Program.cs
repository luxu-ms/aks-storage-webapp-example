using Azure.Storage.Blobs;

async Task UploadFromFileAsync(
    BlobContainerClient containerClient,
    string localFilePath, string blobFilePath)
{
    BlobClient blobClient;
    if (!string.IsNullOrEmpty(blobFilePath))
    {
        blobClient = containerClient.GetBlobClient(blobFilePath);
    }
    else
    {
        string fileName = Path.GetFileName(localFilePath);
        blobClient = containerClient.GetBlobClient(fileName);
    }

    await blobClient.UploadAsync(localFilePath, true);
}

// Define connection string and container name  
string connectionString = "<your connect string>";
string containerName = "appcat";

// Create a BlobServiceClient object  
BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

// Create a BlobContainerClient object  
BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

var localFilePath = "d:/test.png";

int totalCount = 100000;
int batchSize = 1000;
Task[] uploadTasks = new Task[batchSize];
for (int i = 1; i <= totalCount; i++)
{
    uploadTasks[(i-1) % batchSize] = UploadFromFileAsync(containerClient, localFilePath, "subFolder" + i + "/" + "test" + i + ".png");
    if (i % batchSize == 0)
    {
        Console.WriteLine("Current progress: " + i);
        Task.WaitAll(uploadTasks);
        uploadTasks = new Task[batchSize];
    }    
}

