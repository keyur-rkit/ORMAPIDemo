

namespace API.Models.DTO
{
    /// <summary>
    /// Data Transfer Object (DTO) for book information (DTOBK01).
    /// This class is used to represent book data for transfer purposes.
    /// </summary>
    public class DTOBK01
    {
        /// <summary>
        /// Book Title
        /// </summary>
        public string K01F02 { get; set; }

        /// <summary>
        /// Book Author
        /// </summary>
        public string K01F03 { get; set; }

        /// <summary>
        /// Book Category
        /// </summary>
        public string K01F04 { get; set; }

        /// <summary>
        /// Book Price
        /// </summary>
        public decimal K01F05 { get; set; }

    }
}
