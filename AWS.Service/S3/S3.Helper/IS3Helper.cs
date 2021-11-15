using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWS.Service.S3.S3.Helper
{
     interface IS3Helper
    {
        //Bucket
        Task<bool> CreateBucket(string name);

        //File
        Task<bool> UploadFile(Stream inputStream, string pathFile, string bucketName);
        Task<Stream> GetFile(string key, string bucketName);
        Task<bool> DeleteFile(string key, string bucketName);
    }
}
