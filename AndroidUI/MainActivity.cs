using Android.App;
using Android.Widget;
using Android.OS;

namespace AndroidUI
{
    [Activity(Label = "AndroidUI", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton);

            button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };
        }

        protected void Refresh_Click()
        {
            // var scrape = new Utility.WebScraper("https://www.ams.usda.gov/mnreports/bl_gr110.txt");
            // var bp = Parsing.USDA_WY.Shred(scrape.Content);
        }
    }
}

