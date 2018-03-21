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
        public string Name { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Type { get; set; }
    }
}
