using System;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using FinanceManager.Domain.Entities;

namespace FinanceManager.Domain.Entities
{
    public class Expense
    {
        [HiddenInput(DisplayValue = false)]
        public int ExpenseID { get; set; }

        [Required(ErrorMessage = "Podaj opis wydatku!")]
        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Wybierz kategorię wydatku!")]
        [Display(Name = "Kategoria")]
        public string Category { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Kwota musi być wartością dodatnią!")]
        [Display(Name = "Cena")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Podaj prawidłową datę!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data")]
        public DateTime Date { get; set; }
    }
}
