using System.ComponentModel.DataAnnotations;
using Clothing_Store.Core.ViewModels.Orders;

namespace Clothing_Store.Core.ViewModels.Customers
{
    public class CustomerViewModel
    {
        [Required(ErrorMessage = "Полето е задължително.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Името трябва да бъде с дължина между 3 и 30 символа.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Полето е задължително.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Фамилията трябва да бъде с дължина между 3 и 30 символа.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Полето е задължително.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Полето е задължително.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Адресът трябва да бъде с дължина между 5 и 100 символа.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Полето е задължително.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Името на градът трябва да бъде с дължина между 3 и 30 символа.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Полето е задължително.")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Пощенският код да бъде с дължина между 3 и 15 символа.")]
        public string CityPinCode { get; set; }

        [Required(ErrorMessage = "Полето е задължително.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Името на общината трябва да бъде с дължина между 3 и 30 символа.")]
        public string Region { get; set; }

        [Required(ErrorMessage = "Полето е задължително.")]
        [Phone]
        public string Phone { get; set; }

        public bool IsInformationSaved { get; set; }

        public bool IsCustomerWantsToPayOnline { get; set; }

        public IQueryable<OrderViewModel> Orders { get; set; }
    }
}
