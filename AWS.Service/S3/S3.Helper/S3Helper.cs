using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWS.Service.S3.S3.Helper
{
    public class S3Helper : IS3Helper
    {
        IAmazonS3 _s3;
        public S3Helper(IAmazonS3 s3) {
            _s3 = s3;
        }

        public async Task<bool> CreateBucket(string name)
        {
            var createResponse = await _s3.PutBucketAsync(name);
            if (createResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteFile(string key, string bucketName)
        {
            try
            {
                DeleteObjectResponse response = await _s3.DeleteObjectAsync(bucketName, key);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.NoContent)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Stream> GetFile(string key, string bucketName)
        {
            GetObjectResponse response = await _s3.GetObjectAsync(bucketName, key);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                return response.ResponseStream;
            else
                return null;
        }

        /*public (string name, string name2) Test() {

            return ("", "");
        }*/

        public async Task<bool> UploadFile(Stream inputStream, string pathFile, string bucketName)
        {
            string source = Directory.GetCurrentDirectory(); // /src/CarsService.API
            string fileName = string.Empty;
            PutObjectResponse response = null;
            using (FileStream fsSource = new FileStream(source + "/matching.png", FileMode.Open, FileAccess.Read))
            {
                string fileExtension = Path.GetExtension(source + "/matching.png");

                fileName = $"{DateTime.Now.Ticks}{fileExtension}";
                PutObjectRequest request = new PutObjectRequest()
                {
                    InputStream = fsSource,
                    BucketName = bucketName,
                    Key = fileName
                };
                response = await _s3.PutObjectAsync(request);
            }

            // await new TransferUtility(_s3).UploadAsync(source + "/matching.png", name, fileName);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }
    }
}
