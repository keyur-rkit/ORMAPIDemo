using API.Models;
using API.Models.Enum;

namespace API.BL.Interface
{
    /// <summary>
    /// Interface for all bl operations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataHandler<T> where T : class
    {
        /// <summary>
        /// Enum Entry Type proprety
        /// </summary>
        ENUMEntryType Type { get; set; }

        /// <summary>
        /// PreSave method
        /// </summary>
        /// <param name="objDTO"></param>
        void PreSave(T objDTO);

        /// <summary>
        /// Validation method
        /// </summary>
        /// <returns>Obj of Response Class</returns>
        Response Validation();

        /// <summary>
        /// Save Method
        /// </summary>
        /// <returns>Obj of Response Class</returns>
        Response Save();
    }
}