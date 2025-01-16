using ServiceStack.Data;
using System;
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
        private Response _objResponse;
        private readonly IDbConnectionFactory _dbFactory;

        public ENUMEntryType Type { get; set; }
        public int Id { get; set; }

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
                return db.Exists<BK01>(x => x.K01F01 == id);
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
                _objResponse.Data = db.SingleById<BK01>(id);
                return _objResponse;
            }
        }

        public Response GetByCategory(string cat)
        {
            using (var db = _dbFactory.OpenDbConnection())
            {
                _objResponse.Data = db.Select<BK01>(b => b.K01F04 == cat);
                _objResponse.Message = $"Category : {cat}";
            }
            if(_objResponse.Data.Count == 0)
            {
                _objResponse.IsError = true;
                _objResponse.Message = "Category Invalid";
            }
            return _objResponse;
        }

        public Response GetNLatest(int num)
        {
            if(num <= 0)
            {
                _objResponse.IsError = true;
                _objResponse.Message = "Input Invalid";
                return _objResponse;
            }

            using (var db = _dbFactory.OpenDbConnection())
            {
                var q = db.From<BK01>()
                    .OrderByDescending(b => b.K01F06)
                    .Take(num);

                _objResponse.Data = db.Select(q);
                _objResponse.Message = $"Latest books";
            }
            if (_objResponse.Data.Count == 0)
            {
                _objResponse.IsError = true;
                _objResponse.Message = "Input Invalid";
            }
            return _objResponse;
        }

        public Response GetInRange(int min, int max)
        {
            if(min < 0 || max < 0 || min > max)
            {
                _objResponse.IsError = true;
                _objResponse.Message = "Input Invalid";
                return _objResponse;
            }

            using (var db = _dbFactory.OpenDbConnection())
            {
                var q = db.From<BK01>()
                    .Where(b => b.K01F05 > min && b.K01F05 < max);

                _objResponse.Data = db.Select(q);
                _objResponse.Message = "Books in range";
            }
            if (_objResponse.Data.Count == 0)
            {
                _objResponse.IsError = true;
                _objResponse.Message = "No books in range";
            }
            return _objResponse;
        }

        public void PreSave(DTOBK01 objDTO)
        {
            _objBK01 = objDTO.Convert<BK01>();
            if(Type == ENUMEntryType.E)
            {
                _objBK01.K01F01 = Id;
            }
        }

        public Response Validation()
        {
            if(Type == ENUMEntryType.E || Type == ENUMEntryType.D)
            {
                if(Id <= 0)
                {
                    _objResponse.IsError = true;
                    _objResponse.Message = "Invalid Id";
                }
                else if(!IsBK01Exist(Id))
                {
                    _objResponse.IsError = true;
                    _objResponse.Message = "Book not found";
                }
            }
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
                    }
                    else if(Type == ENUMEntryType.E)
                    {
                        db.Update(_objBK01);
                        _objResponse.Message = $"Book with Id {Id} Edited";
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

        public Response Delete()
        {
            try
            {
                using(var db = _dbFactory.OpenDbConnection())
                {
                    if(Type == ENUMEntryType.D)
                    {
                        db.DeleteById<BK01>(Id);
                        _objResponse.Message = $"Book with Id {Id} Deleted";
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

        public Response UpdatePrice(decimal price) 
        {
            try
            {
                using (var db = _dbFactory.OpenDbConnection())
                {
                    if(Type == ENUMEntryType.E)
                    {
                        db.UpdateOnly(() => new BK01 { K01F05 = price }, where: b => b.K01F01 == Id);
                        _objResponse.Message = $"Price of Book with Id {Id} changed to {price}";
                    }
                }
            }
            catch (Exception ex)
            {
                _objResponse.IsError = true;
                _objResponse.Message = ex.Message;
            }
            return _objResponse;
        }


    }
}