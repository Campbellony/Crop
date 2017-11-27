using System;
using System.Collections.Generic;
using System.Text;

namespace Crop.Parsing
{
    public class USDA_WY
    {
        private const string WINTER_WHEAT = "Hard Red Winter Wheat";
        private const string SPRING_WHEAT = "Dark Northern Spring Wheat";
        private const string DURUM_WHEAT = "Durum Wheat";
        private const string END_CHECK = "nb - no bid";
        public static IEnumerable<Models.BuyerProduct> Shred(string input)
        {
            var buffer = new List<Models.BuyerProduct>();
            DateTime asOf = DateTime.Now;
            Products currentProduct = Products.None;
            var lines = input.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                if (i == 1)
                {
                    asOf = DateTime.Parse(lines[i].Substring(22, 16));
                    continue;
                }

                if (lines[i].Contains(WINTER_WHEAT))
                {
                    currentProduct = Products.WinterWheat;
                    continue;
                }

                if (lines[i].Contains(SPRING_WHEAT))
                {
                    currentProduct = Products.SpringWheat;
                    continue;
                }

                if (currentProduct != Products.None && lines[i].Contains(DURUM_WHEAT))
                {
                    currentProduct = Products.DurumWheat;
                    continue;
                }

                if (lines[i].Contains(END_CHECK))
                {
                    currentProduct = Products.None;
                    continue;
                }

                if (currentProduct != Products.None)
                {
                    ParseLine(buffer, currentProduct, lines[i], asOf);
                }
            }

            return buffer;
        }

        private static void ParseLine(List<Models.BuyerProduct> buffer, Products currentProduct, string line, DateTime asOf)
        {
            if (line.Length > 0)
            {

                var expression = new System.Text.RegularExpressions.Regex(@"\s\s+");
                var parts = expression.Split(line);
                if (parts[0].Length > 0 && parts.Length > 1)
                {
                    var b = new Models.Buyer();
                    b.Name = parts[0];

                    switch (currentProduct)
                    {
                        case Products.WinterWheat:
                            ParseColumn(buffer, b, parts[1], WINTER_WHEAT, "Ordinary", asOf);
                            ParseColumn(buffer, b, parts[2], WINTER_WHEAT, "11 pct", asOf);
                            ParseColumn(buffer, b, parts[3], WINTER_WHEAT, "12 pct", asOf);
                            ParseColumn(buffer, b, parts[4], WINTER_WHEAT, "13 pct", asOf);
                            break;
                        case Products.SpringWheat:
                            ParseColumn(buffer, b, parts[1], SPRING_WHEAT, "13 pct", asOf);
                            ParseColumn(buffer, b, parts[2], SPRING_WHEAT, "14 pct", asOf);
                            ParseColumn(buffer, b, parts[3], SPRING_WHEAT, "15 pct", asOf);
                            break;
                        case Products.DurumWheat:
                            ParseColumn(buffer, b, parts[1], DURUM_WHEAT, "13 pct", asOf);
                            ParseColumn(buffer, b, parts[2], "Barley", "Malt", asOf);
                            ParseColumn(buffer, b, parts[3], "Barley", "Feed", asOf);
                            break;
                    }
                }
            }
        }

        private static void ParseColumn(List<Models.BuyerProduct> buffer, Models.Buyer buyer, string priceRange, string productName, string quality, DateTime asOf)
        {
            if (priceRange.TrimEnd() != "--" && priceRange.TrimEnd() != "nb" && priceRange.TrimEnd() != "na")
            {
                var p = new Models.Product();
                p.Name = productName;
                p.Quality = quality;

                var price = new Models.Price();
                price.AsOf = asOf;

                if (priceRange.Contains("-"))
                {
                    var prices = priceRange.Split('-');
                    price.MinPrice = decimal.Parse(prices[0]);
                    price.MaxPrice = decimal.Parse(prices[1]);
                }
                else
                {
                    price.MinPrice = decimal.Parse(priceRange);
                    price.MaxPrice = decimal.Parse(priceRange);
                }
                var bp = new Models.BuyerProduct();
                bp.Buyer = buyer;
                bp.Product = p;
                bp.Price = price;
                buffer.Add(bp);
            }
        }
    }

    public enum Products
    {
        None,
        WinterWheat,
        SpringWheat,
        DurumWheat
    }
}
