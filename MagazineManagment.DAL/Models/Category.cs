using MagazineManagment.DAL.Models.Base;

namespace MagazineManagment.DAL.Models
{
    public  class Category : BaseModel 
    {
        public string CategoryName { get; set; }  
        // relation prop
        public ICollection<Product>? Products { get; set; }
    }
}
