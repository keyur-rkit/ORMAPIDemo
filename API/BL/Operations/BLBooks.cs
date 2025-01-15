using ServiceStack.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using API.Models.DTO;
using API.Models.POCO;
using API.Models.Enum;
using API.BL.Interface;
using ServiceStack.OrmLite;
using API.Extensions;

namespace API.BL.Operations
{
    public class BLBooks : IDataHandler<DTOBK01>
    {
        private BK01 _objBK01;
        private int _id;
        private Response _objResponse;
        private readonly IDbConnectionFactory _dbFactory;

        public ENUMEntryType Type { get; set; }

        public BLBooks()
        {
            _objResponse = new Response(); 

            _dbFactory = HttpContext.Current.Application["DbFactory"] as IDbConnectionFactory;

            if(_dbFactory == null )
            {
                throw new Exception("IDbConnectionFactory not found");
            }
        }


        public bool IsBK01Exist(int id)
        {
            using (var db = _dbFactory.OpenDbConnection())
            {
                return db.Exists<BK01>(id);
            }
        } 

        public Response GetAll()
        {
            using (var db = _dbFactory.OpenDbConnection())
            {
                var result= db.Select<BK01>().ToList();
                if(result.Count == 0)
                {
                    _objResponse.IsError = true;
                    _objResponse.Message = "Zero books available";
                    _objResponse.Data = null;

                    return _objResponse;
                }
                _objResponse.IsError = false;
                _objResponse.Data = result;
                return _objResponse;
            }
        }

        public void PreSave(DTOBK01 objDTO)
        {
            _objBK01 = objDTO.Convert<BK01>();
            if(Type == ENUMEntryType.E)
            {

            }
        }

        public Response Validation()
        {
            return _objResponse;
        }

        public Response Save()
        {
            try
            {
                using (var db = _dbFactory.OpenDbConnection())
                {
                    if(Type == ENUMEntryType.A)
                    {
                        db.Insert(_objBK01);
                        _objResponse.Message = "Book Added";
                        _objResponse.Data = _objBK01;
                    }
                }
            }
            catch(Exception ex)
            {
                _objResponse.IsError = true;
                _objResponse.Message = ex.Message;
            }
            return _objResponse;
        }

        public Response GetById(int id)
        {
            if (!IsBK01Exist(id))
            {
                _objResponse.IsError = true;
                _objResponse.Message = "Book dose not Exist";
                _objResponse.Data = null;

                return _objResponse;
            }
            using (var db = _dbFactory.OpenDbConnection())
            {
                _objResponse.Data =  db.SingleById<BK01>(id);
                return _objResponse;
            }
        }

    }
}