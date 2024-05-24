using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class CreateOrderRequestDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int TotalItem { get; set; }

        [Required]
        public int TotalOrderPrice { get; set; }
    }
}
