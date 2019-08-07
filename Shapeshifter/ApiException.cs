using System;
using System.Collections.Generic;
using System.Text;

namespace Shapeshifter
{
    class ApiException : Exception
    {
        public int StatusCode { get; set; }

        public string Content { get; set; }
    }
}
