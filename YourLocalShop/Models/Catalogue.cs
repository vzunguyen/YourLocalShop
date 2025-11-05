namespace YourLocalShop.Models
{
    public class Catalogue
    {
        private static Catalogue? _instance;
        private static readonly object _lock = new();

        public static Catalogue Instance
        {
            get { lock (_lock) return _instance ??= new Catalogue(); }
        }

        public List<Product> Products { get; } = new();

        private Catalogue() { Seed(); }

        private void Seed()
        {
            int id = 1;

            // Pantry
            Products.Add(new Product { Id=id++, Name="Corn Flakes 380g", Price=7.00m, StockQty=100, CategoryName="Pantry", Description="Crunchy Nut Corn Flakes Breakfast Cereal" });
            Products.Add(new Product { Id=id++, Name="Instant Noodle 600g", Price=8.00m, StockQty=100, CategoryName="Pantry", Description="Spicy flavoured Instant Noodle 5 Packs" });
            Products.Add(new Product { Id=id++, Name="Honey 500g", Price=9.00m, StockQty=100, CategoryName="Pantry", Description="Pure Honey 100% Australian Honey" });
            Products.Add(new Product { Id=id++, Name="Peanut Butter 375g", Price=6.50m, StockQty=100, CategoryName="Pantry", Description="Smooth Natural Peanut Butter" });
            Products.Add(new Product { Id=id++, Name="Pasta Fusilli 500g", Price=2.50m, StockQty=100, CategoryName="Pantry", Description="Durum Wheat Pasta" });
            Products.Add(new Product { Id=id++, Name="BBQ Sauce 500ml", Price=4.20m, StockQty=100, CategoryName="Pantry", Description="Reduced Salt and Sugar BBQ Sauce" });

            // Cleaning
            Products.Add(new Product { Id=id++, Name="Toilet Paper 8 Packs", Price=8.00m, StockQty=100, CategoryName="Cleaning", Description="100% Recycled Toilet Paper" });
            Products.Add(new Product { Id=id++, Name="Laundry Powder 2kg", Price=30.00m, StockQty=100, CategoryName="Cleaning", Description="For Front and Top Washing Machines" });
            Products.Add(new Product { Id=id++, Name="Dishwashing Liquid 800ml", Price=9.00m, StockQty=100, CategoryName="Cleaning", Description="Fresh Lemon Dishwashing Liquid" });
            Products.Add(new Product { Id=id++, Name="Glass Cleaner Spray 500ml", Price=5.00m, StockQty=100, CategoryName="Cleaning", Description="Ammonia Free" });

            // Drinks
            Products.Add(new Product { Id=id++, Name="Tropical Juice 2L", Price=4.80m, StockQty=100, CategoryName="Drinks", Description="Apple, Pear and Pineapple Juice" });
            Products.Add(new Product { Id=id++, Name="Coke 1.25L", Price=4.00m, StockQty=100, CategoryName="Drinks", Description="Zero Sugar Soft Drink" });
            Products.Add(new Product { Id=id++, Name="Sparkling Water 1.25L", Price=3.50m, StockQty=100, CategoryName="Drinks", Description="Lightly Sparkling Water" });
            Products.Add(new Product { Id=id++, Name="Ginger Beer 10 Packs", Price=17.50m, StockQty=100, CategoryName="Drinks", Description="375ml x 10 Packs" });

            // Snacks
            Products.Add(new Product { Id=id++, Name="Chocolate Block 180g", Price=7.50m, StockQty=100, CategoryName="Snacks", Description="60% Dark Chocolate" });
            Products.Add(new Product { Id=id++, Name="Potato Chips 165g", Price=6.00m, StockQty=100, CategoryName="Snacks", Description="Ridge Cut Potato Chips" });
            Products.Add(new Product { Id=id++, Name="Biscuits 250g", Price=2.50m, StockQty=100, CategoryName="Snacks", Description="Milk Arrowroot Biscuits" });
            Products.Add(new Product { Id=id++, Name="Cream Wafers 125g", Price=1.25m, StockQty=100, CategoryName="Snacks", Description="Strawberry Cream Wafers" });

            // Dairy
            Products.Add(new Product { Id=id++, Name="Butter 250g", Price=4.00m, StockQty=100, CategoryName="Dairy", Description="Unsalted Butter" });
            Products.Add(new Product { Id=id++, Name="Yoghurt 1kg", Price=7.00m, StockQty=100, CategoryName="Dairy", Description="Plain Greek Yogurt" });
            Products.Add(new Product { Id=id++, Name="Milk 2L", Price=3.20m, StockQty=100, CategoryName="Dairy", Description="Full Cream Milk" });
            Products.Add(new Product { Id=id++, Name="Sliced Cheese 250g", Price=5.50m, StockQty=100, CategoryName="Dairy", Description="Tasty Cheese 24 slices" });
        }
    }
}
