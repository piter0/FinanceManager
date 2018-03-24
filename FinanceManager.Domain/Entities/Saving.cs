using System;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FinanceManager.Domain.Entities
{
    public class Saving
    {
        [HiddenInput(DisplayValue = false)]
        public int SavingID { get; set; }

        [Required(ErrorMessage = "Podaj opis oszczędności!")]
        [Display(Name = "Opis")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Opis musi mieć pomiędzy 3 a 50 znaków!")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Wybierz kategorię oszczędności!")]
        [Display(Name = "Kategoria")]
        public string Category { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Kwota musi być wartością dodatnią!")]
        [Display(Name = "Kwota")]
        [RegularExpression(@"^(0|-?\d{0,9}(\,\d{0,2})?)$", ErrorMessage = "Kwota może mieć maksymalnie dwa miejsca po przecinku!")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Podaj prawidłową datę!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data")]
        public DateTime Date { get; set; }
    }
}
