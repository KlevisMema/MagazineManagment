namespace MagazineManagment.Shared.ApiUrlDestinations
{
    public static class RequestDestination
    {
        public const string Product = "Product";
        //get methods
        public const string GetProductImage = "Product/GetProductImage/";
        public const string GetCreateProductRoute = "CategoryNameOnly";
        public const string GetEditProduct = "Product/";
        public const string GetCategories = "Category";
        public const string GetCategory = "Category/";
        // post methods
        public const string PostCreateProductRoute = "api/Product";
        public const string PostEditProduct = "api/Product";
        public const string PostCreateCategory = "api/Category";
    }
}
