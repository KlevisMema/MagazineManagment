using MagazineManagment.DAL.Models.Base;

namespace MagazineManagment.DAL.Models
{
    public class ProductRecordsChanged : BaseModel
    {
        public Guid ProductId { get; set; }
        public int? ProductInStock { get; set; }
        public string? UpdatedBy { get; set; }
        public int QunatityBeforeRemoval { get; set; }
    }
}
