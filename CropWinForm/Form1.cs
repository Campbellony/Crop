using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CropWinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private IEnumerable<Crop.Models.BuyerProduct> buyerProducts;
        private void button1_Click(object sender, EventArgs e)
        {
            this.productBox.Items.Clear();
            var scrape = new Utility.WebScraper("https://www.ams.usda.gov/mnreports/bl_gr110.txt");
            this.buyerProducts = Crop.Parsing.USDA_WY.Shred(scrape.Content);
            this.BindProducts();
        }

        private void BindProducts()
        {
            var productList = new List<string>();
            foreach(var buyerProduct in this.buyerProducts)
            {
                if (productList.Contains(buyerProduct.Product.Name) == false)
                { 
                   productList.Add( buyerProduct.Product.Name);
                    productBox.Items.Add(buyerProduct.Product.Name);
                }
            }
        }

        private void productBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedProduct = this.productBox.SelectedItem;
            controlsPanel.Controls.Clear();

            foreach (var buyerProduct in this.buyerProducts)
            {
                if (buyerProduct.Product.Name == selectedProduct)
                {
                    var pc = new PriceControl();
                    pc.groupBox1.Text = buyerProduct.Buyer.Name;
                    pc.QualityLabel.Text = buyerProduct.Product.Quality;
                    pc.MinLabel.Text = buyerProduct.Price.MinPrice.ToString();
                    pc.MaxLabel.Text = buyerProduct.Price.MaxPrice.ToString();

                    controlsPanel.Controls.Add(pc);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.button1_Click(this, e);
        }
    }
}
