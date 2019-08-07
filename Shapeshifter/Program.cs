using System;

namespace Shapeshifter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ImageMaster shapeshifter = new ImageMaster();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}");
            }
        }
    }
}
