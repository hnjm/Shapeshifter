using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Shapeshifter
{
    public static class FileExtensions
    {
        public static Task DeleteAsync(this FileInfo fi)
        {
            return Task.Factory.StartNew(() => fi.Delete());
        }
    }
}
