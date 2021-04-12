using Amazon;
using Amazon.Glacier;
using Amazon.Glacier.Transfer;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Framework.AWS.S3.Examples
{
    class Program
    {
        private const string bucketName = "tnt-sts-bucket";
        private const string key1 = "trung.tran/";
        // Specify your bucket region (an example region is shown).
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USWest2;
        private static IAmazonS3 s3Client;
        public static void Main()
        {
            var idKey = EnvironmentVariablesAWSCredentials.ENVIRONMENT_VARIABLE_ACCESSKEY;
            var secretKey = EnvironmentVariablesAWSCredentials.ENVIRONMENT_VARIABLE_SECRETKEY;
            var id = Environment.GetEnvironmentVariable(idKey, EnvironmentVariableTarget.User);
            var secret = Environment.GetEnvironmentVariable(secretKey, EnvironmentVariableTarget.User);
            s3Client = new AmazonS3Client(new BasicAWSCredentials(id, secret), bucketRegion);
            ArchiveAsync().Wait();
        }

        static async Task ArchiveAsync()
        {
            var findFolderRequest = new ListObjectsV2Request();
            findFolderRequest.BucketName = bucketName;
            findFolderRequest.Prefix = key1;

            ListObjectsV2Response findFolderResponse =
               await s3Client.ListObjectsV2Async(findFolderRequest);

            var requests = new List<PutObjectRequest>();

            foreach (var obj in findFolderResponse.S3Objects)
            {
                requests.Add(new PutObjectRequest
                {
                    Key = obj.Key,
                    BucketName = bucketName,
                    StorageClass = S3StorageClass.Glacier
                });
            }
            var test = await Task.WhenAll(requests.Select(o => s3Client.PutObjectAsync(o)));
        }

        static async Task CreateFoldersAsync(string bucketName, string path)
        {
            var findFolderRequest = new ListObjectsV2Request();
            findFolderRequest.BucketName = bucketName;
            findFolderRequest.Prefix = path;

            ListObjectsV2Response findFolderResponse =
               await s3Client.ListObjectsV2Async(findFolderRequest);


            if (findFolderResponse.S3Objects.Any())
            {
                return;
            }

            PutObjectRequest request = new PutObjectRequest()
            {
                BucketName = bucketName,
                Key = path,
                ContentBody = string.Empty
            };

            // add try catch in case you have exceptions shield/handling here 
            PutObjectResponse response = await s3Client.PutObjectAsync(request);
        }


        static async Task WritingAnObjectAsync()
        {
            try
            {
                await CreateFoldersAsync(bucketName, key1);

                var list = new ListObjectsV2Request()
                {
                    BucketName = bucketName,
                    Prefix = key1
                };
                var objectResp = await s3Client.ListObjectsV2Async(list);
                long totalSize = 0;
                foreach (var obj in objectResp.S3Objects)
                {
                    totalSize += obj.Size;
                }

                if (totalSize == 0)
                {
                    PutObjectRequest request = new PutObjectRequest()
                    {
                        BucketName = bucketName,
                        Key = key1 + "hello.txt",
                        ContentBody = "Hello, it's me"
                    };

                    // add try catch in case you have exceptions shield/handling here 
                    PutObjectResponse response = await s3Client.PutObjectAsync(request);
                }

                Console.WriteLine(totalSize);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine(
                        "Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    "Unknown encountered on server. Message:'{0}' when writing an object"
                    , e.Message);
            }
        }

        static async Task CreateBucketAsync()
        {
            try
            {
                var exists = await AmazonS3Util.DoesS3BucketExistV2Async(s3Client, bucketName);
                if (!exists)
                {
                    var putBucketRequest = new PutBucketRequest
                    {
                        BucketName = bucketName,
                        UseClientRegion = true
                    };

                    PutBucketResponse putBucketResponse = await s3Client.PutBucketAsync(putBucketRequest);
                }
                // Retrieve the bucket location.
                string bucketLocation = await FindBucketLocationAsync(s3Client);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }

        static async Task<string> FindBucketLocationAsync(IAmazonS3 client)
        {
            string bucketLocation;
            var request = new GetBucketLocationRequest()
            {
                BucketName = bucketName
            };
            GetBucketLocationResponse response = await client.GetBucketLocationAsync(request);
            bucketLocation = response.Location.ToString();
            return bucketLocation;
        }
    }
}
