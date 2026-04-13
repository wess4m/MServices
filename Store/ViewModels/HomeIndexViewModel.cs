using Store.Models;
using Store.Models.Dto;

namespace Store.ViewModels
{
    public class HomeIndexViewModel
    {
        public List<ProductDto> Products { get; set; }
        public CartDto Cart { get; set; }
    }
}
