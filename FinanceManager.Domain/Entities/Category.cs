using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FinanceManager.Domain.Entities
{
    public class Category
    {
        [HiddenInput(DisplayValue = false)]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Podaj nazwę kategorii!")]
        [Display(Name = "Nazwa kategorii")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Nazwa kategorii musi mieć pomiędzy 3 a 50 znaków!")]
        public string Name { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Type { get; set; }
    }
}
