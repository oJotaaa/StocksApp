using StocksApp.Entities;
using StocksApp.ServiceContracts.CustomValidators;
using System.ComponentModel.DataAnnotations;

namespace StocksApp.ServiceContracts.DTO
{
    public class BuyOrderRequest
    {
        [Required(ErrorMessage = "StockSymbol can't be empty")]
        public string? StockSymbol { get; set; }

        [Required(ErrorMessage = "StockName can't be empty")]
        public string? StockName { get; set; }

        [MinimumDate]
        public DateTime DateAndTimeOfOrder { get; set; }

        [Range(1, 100000, ErrorMessage = "Quantity must be 1 to 100000")]
        public uint Quantity { get; set; }

        [Range(1, 10000, ErrorMessage = "Price must be 1 to 10000")]
        public double Price { get; set; }
    }
}
