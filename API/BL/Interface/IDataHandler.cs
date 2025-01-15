using API.Models;
using API.Models.Enum;

namespace API.BL.Interface
{
    public interface IDataHandler<T> where T : class
    {
        ENUMEntryType Type { get; set; }

        void PreSave(T objDTO);

        Response Validation();

        Response Save();
    }
}