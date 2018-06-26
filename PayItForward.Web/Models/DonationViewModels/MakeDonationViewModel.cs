namespace PayItForward.Web.Models.DonationViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using PayItForward.Models;

    public class MakeDonationViewModel
    {
        [Display(Name = "Donate")]
        public decimal AmountToDonate { get; set; }

        public decimal AvilableMoneyAmount { get; set; } = 0;
    }
}
