namespace eCommerceAPI.Business.Products.Commands.AddProductItem
{
    public class AddProductItemCommand
    {
        public int SelectedProductId { get; set; }
        public int Price { get; set; }
        public string Size { get; set; }
        public string TypeName { get; set; }
        public int QtyInStock { get; set; }

    }
}
