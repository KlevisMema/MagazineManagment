namespace MagazineManagment.Web.ApiCalls.ApiUrlValues
{
    public class FetchApiValue
    {
        public const string SectionName = "ApiCalls";
        public string? ProductGet { get; set; }
        public string? GetAllCategories { get; set; }
        public string? ProductPostCreateOrEditDefaultUri { get; set; }
        public string? CategoryCreateOrEditDefaultUri { get; set; }
        public string? CategoryGetOrDeleteDefaultUri { get; set; }
        public string? ProfilePostOrEditRole { get;set; }
        public string? ProfileGetOrDeleteProfile { get; set; }
    }
}
