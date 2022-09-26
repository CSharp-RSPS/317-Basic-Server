using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace RSPS.src.Data
{
    /// <summary>
    /// Contains file related operations and utilities
    /// </summary>
    public static class FileUtil
    {


        /// <summary>
        /// Retrieves the byte buffer for a file
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <returns>The buffer</returns>
        public static byte[] GetBuffer(string filePath)
        {
            byte[]? buffer = null;

            using (MemoryStream ms = new(File.ReadAllBytes(filePath)))
            {
                buffer = ms.ToArray();
            }
            return buffer;
        }

    }
}
