using ServiceStack.DataAnnotations;
using System;

namespace API.Models.POCO
{
    /// <summary>
    /// Plain Old CLR Object (POCO) for student information (STD01).
    /// This class represents the book entity for the database or internal processing.
    /// </summary>
    public class BK01
    {
        /// <summary>
        /// Book ID
        /// </summary>
        [AutoIncrement]
        [PrimaryKey]
        public int K01F01 { get; set; }

        /// <summary>
        /// Book Title
        /// </summary>
        [Required]
        [StringLength(150)]
        public string K01F02 { get; set; }

        /// <summary>
        /// Book Author
        /// </summary>
        [Required]
        [StringLength(100)]
        public string K01F03 { get; set; }

        /// <summary>
        /// Book Category
        /// </summary>
        [StringLength(100)]
        public string K01F04 { get; set; }

        /// <summary>
        /// Book Price
        /// </summary>
        [Required]
        [DecimalLength(8, 2)]
        public decimal K01F05 { get; set; }

        /// <summary>
        /// CreatedAt
        /// </summary>
        [Required]
        [IgnoreOnUpdate]
        public DateTime K01F06 { get; set; } = DateTime.Now;

        /// <summary>
        /// UpdatedAt
        /// </summary>
        [Required]
        public DateTime K01F07 { get; set; } = DateTime.Now;


    }
}