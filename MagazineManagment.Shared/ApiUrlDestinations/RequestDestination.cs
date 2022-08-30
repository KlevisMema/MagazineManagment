namespace MagazineManagment.Shared.ApiUrlDestinations
{
    public static class RequestDestination
    {
        public const string ProductGetOrDeleteDefaultRoute = "Product";
        // Product
        public const string GetProductImage = "Product/GetProductImage";
        public const string GetCreateProductRoute = "CategoryNameOnly";
        public const string ProductCreateOrEditDefaultRoute = "api/Product";
        public const string ProductChangesMadeByEmployee = "Product/GetProducChangesByEmpolyees";
        public const string ProductChangesMadeByEmployeeDeleteRoute = "Product/DeleteProducChangesByEmpolyees";
        public const string SearchProduct = "Product/GetProductByName/";
        // Category
        public const string CategoryGetOrDeleteDefaultRoute = "Category";
        public const string CategoryCreateOrEditDefaultRoute = "api/Category";
        public const string ActivateCategory = "Category/ActivateCategory";
        //Profile
        public const string ProfileGetRoles = "Profile";
        public const string ProfileGetRole = "Profile/FindRole";
        public const string ProfilePostOrEditRoleRoute = "api/Profile";
        public const string ProfileAssignRoleToUsers = "api/Profile/AssignRoleToUsers/";
        public const string ProfileRemoveUsersFromRole = "api/Profile/RemoveRoleFromUsers/";
        public const string ProfileGetRoleDetailsRoute = "Profile/GetRole";
        public const string ProfileGetUsersInRole = "Profile/GetUsersOfARole/";
    }
}