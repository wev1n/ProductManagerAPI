using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProductManagerAPI.Data.Entites
{
    [Index(nameof(Sku), IsUnique = true)]
    public class Product
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public required string Name { get; set; }

        [Column(TypeName = "nchar(6)")]
        public required string Sku { get; set; }

        [MaxLength(250)]
        public required string Description { get; set; }

        [MaxLength(100)]
        public required string Url { get; set; }

        public required int Price { get; set; }
    }
}
