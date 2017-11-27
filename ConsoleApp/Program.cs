using System;

namespace Crop
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                var scrape = new Utility.WebScraper("https://www.ams.usda.gov/mnreports/bl_gr110.txt");
                var bp = Parsing.USDA_WY.Shred(scrape.Content);
                Console.WriteLine(bp.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
