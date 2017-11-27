using System;

namespace Utility
{
    public class WebScraper
    {
        private string URL { get; set; }
        public string Content { get; private set; }
        public WebScraper(string url)
        {
            this.URL = url;
            this.ReadContentAsync();
        }

        public void ReadContentAsync()
        {
            var client = System.Net.HttpWebRequest.Create(this.URL);
            var x = client.GetResponse();
           var stream = x.GetResponseStream();

            var reader = new System.IO.StreamReader(stream);
           this.Content =  reader.ReadToEnd();
        }   
    }
}
