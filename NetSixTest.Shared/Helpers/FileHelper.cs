using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NetSixTest.Shared.Helpers
{
    public static class FileHelper
    {
       public static string ComputeImageHash(byte[] data)
        {

            using (var sha256 = SHA256.Create())
            using (var stream = new MemoryStream(data))
            {
                byte[] hashBytes = sha256.ComputeHash(stream);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}
