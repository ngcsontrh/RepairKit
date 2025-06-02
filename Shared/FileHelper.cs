using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public static class FileHelper
    {        
        public static bool SaveBase64File(string base64String, string filePath)
        {
            try
            {
                // Remove the data URL prefix if present
                if (base64String.StartsWith("data:"))
                {
                    var index = base64String.IndexOf(",");
                    if (index >= 0)
                    {
                        base64String = base64String.Substring(index + 1);
                    }
                }

                // Convert base64 string to byte array
                byte[] fileBytes = Convert.FromBase64String(base64String);

                // Write the byte array to the specified file path
                System.IO.File.WriteAllBytes(filePath, fileBytes);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
