namespace eCommerceAPI.Business.Products.Queries.GetProductDetails
{
    public class GetProductHeaderResponse
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int NoOfConfigs { get; set; }
        //public List<GetProductDetailResponse> DetailsResponse { get; set; }
    }
}
