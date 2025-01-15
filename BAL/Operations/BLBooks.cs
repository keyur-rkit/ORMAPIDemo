using BAL.Interface;
using MAL.DTO;
using MAL.Enum;
using MAL.POCO;
using MAL.Response;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System.Web;


namespace BAL.Operations
{
    public class BLBooks : IDataHandler<DTOBK01>
    {
        private BK01 _objBK01;
        private int _id;
        private Response<DTOBK01> _objResponse;
        private readonly IDbConnectionFactory _dbFactory;



        public BLBooks()
        {
            _objResponse = new Response<DTOBK01>();

            _dbFactory = ;
        }

        public ENUMEntryType Type { get; set; }

        public void PreSave(DTOBK01 objDTO)
        {
            throw new NotImplementedException();
        }

        public Response<DTOBK01> Save()
        {
            throw new NotImplementedException();
        }

        public Response<DTOBK01> Validation()
        {
            throw new NotImplementedException();
        }
    }
}
