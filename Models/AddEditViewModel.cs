using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HelmonyCornDog.Models
{
    public class AddEditViewModel
    {

        public AddEditViewModel()
        {
            Categories = new List<Category>();
        }

        public int Id { get; set; }
        [MaxLength(20)]
        [DisplayName("نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]

        public string Name { get; set; }
        [DisplayName("توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public string Description { get; set; }
        [DisplayName("قیمت")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public decimal Price { get; set; }
        [DisplayName("تعداد موجودیت")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public int QuantityinStock { get; set; }
        [DisplayName("تصویر")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public IFormFile Picture { get; set; }
        [DisplayName("وزن")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public string Weight { get; set; }
        [DisplayName("ترکیبات")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public string Combinations { get; set; }
        [DisplayName("مدت زمان پخت")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public string Cooktime { get; set; }
        [DisplayName("توضیحات دیگر(کوتاه)")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public string Otherexplanations { get; set; }

        [DisplayName("گروه ها")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public List<Category>? Categories { get; set; }






    }
}
