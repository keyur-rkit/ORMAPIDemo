using MAL.Enum;
using MAL.Response;

namespace BAL.Interface
{
    public interface IDataHandler<T> where T : class
    {
        ENUMEntryType Type { get; set; }

        void PreSave(T objDTO);

        Response<T> Validation();

        Response<T> Save();
    }
}
