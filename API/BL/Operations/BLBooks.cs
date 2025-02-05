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
using ServiceStack;
using System.Data;
using System.Collections.Generic;

namespace API.BL.Operations
{
    /// <summary>
    /// Handles book-related CRUD operations.
    /// </summary>
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

            if (_dbFactory == null)
            {
                throw new Exception("IDbConnectionFactory not found");
            }
        }

        /// <summary>
        /// Checks if a book exists by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if book exists, otherwise false.</returns>
        public bool IsBK01Exist(int id)
        {
            using (IDbConnection db = _dbFactory.OpenDbConnection())
            {
                return db.Exists<BK01>(x => x.K01F01 == id);
            }
        }

        /// <summary>
        /// Retrieves all books.
        /// </summary>
        /// <returns>Response with book list.</returns>
        public Response GetAll()
        {
            using (IDbConnection db = _dbFactory.OpenDbConnection())
            {
                List<BK01> result = db.Select<BK01>().ToList();
                if (result.Count == 0)
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

        /// <summary>
        /// Retrieves a book by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Response with book data.</returns>
        public Response GetById(int id)
        {
            if (!IsBK01Exist(id))
            {
                _objResponse.IsError = true;
                _objResponse.Message = "Book dose not Exist";
                _objResponse.Data = null;

                return _objResponse;
            }
            using (IDbConnection db = _dbFactory.OpenDbConnection())
            {
                _objResponse.Data = db.SingleById<BK01>(id);
                return _objResponse;
            }
        }

        /// <summary>
        /// Retrieves books by category.
        /// </summary>
        /// <param name="cat"></param>
        /// <returns>Response with books in the specified category.</returns>
        public Response GetByCategory(string cat)
        {
            using (IDbConnection db = _dbFactory.OpenDbConnection())
            {
                //// changes : query insted of :b => b.K01F04 == cat 
                _objResponse.Data = db.Select<BK01>("SELECT * FROM BK01 WHERE K01F04 = @cat", new {cat});
                _objResponse.Message = $"Category : {cat}";
            }
            if (_objResponse.Data.Count == 0)
            {
                _objResponse.IsError = true;
                _objResponse.Message = "Category Invalid";
            }
            return _objResponse;
        }

        /// <summary>
        /// Retrieves the latest books based on a given number.
        /// </summary>
        /// <param name="num"></param>
        /// <returns>Response with the latest books.</returns>
        public Response GetNLatest(int num)
        {
            if (num <= 0)
            {
                _objResponse.IsError = true;
                _objResponse.Message = "Input Invalid";
                return _objResponse;
            }

            using (IDbConnection db = _dbFactory.OpenDbConnection())
            {
                _objResponse.Data = db.Select(db.From<BK01>().OrderByDescending(b => b.K01F06).Take(num));
                _objResponse.Message = $"Latest books";
            }
            if (_objResponse.Data.Count == 0)
            {
                _objResponse.IsError = true;
                _objResponse.Message = "Input Invalid";
            }
            return _objResponse;
        }

        /// <summary>
        /// Retrieves books within a specified price range.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns>Response with books in the price range.</returns>
        public Response GetInRange(int min, int max)
        {
            if (min < 0 || max < 0 || min > max)
            {
                _objResponse.IsError = true;
                _objResponse.Message = "Input Invalid";
                return _objResponse;
            }

            using (IDbConnection db = _dbFactory.OpenDbConnection())
            {
                _objResponse.Data = db.Select(db.From<BK01>().Where(b => b.K01F05 > min && b.K01F05 < max));
                _objResponse.Message = "Books in range";
            }
            if (_objResponse.Data.Count == 0)
            {
                _objResponse.IsError = true;
                _objResponse.Message = "No books in range";
            }
            return _objResponse;
        }

        /// <summary>
        /// Prepares a book object for save or update.
        /// </summary>
        /// <param name="objDTO"></param>
        public void PreSave(DTOBK01 objDTO)
        {
            _objBK01 = objDTO.Convert<BK01>();
            if (Type == ENUMEntryType.E)
            {
                _objBK01.K01F01 = Id;
            }
        }

        /// <summary>
        /// Validates before saving, updating, or deleting a book.
        /// </summary>
        /// <returns>Response with validation result.</returns>
        public Response Validation()
        {
            if (Type == ENUMEntryType.E || Type == ENUMEntryType.D)
            {
                if (Id <= 0)
                {
                    _objResponse.IsError = true;
                    _objResponse.Message = "Invalid Id";
                }
                else if (!IsBK01Exist(Id))
                {
                    _objResponse.IsError = true;
                    _objResponse.Message = "Book not found";
                }
            }
            return _objResponse;
        }

        /// <summary>
        /// Saves a new book or updates an existing one.
        /// </summary>
        /// <returns>Response with operation status.</returns>
        public Response Save()
        {
            try
            {
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    if (Type == ENUMEntryType.A)
                    {
                        db.Insert(_objBK01);
                        _objResponse.Message = "Book Added";
                    }
                    else if (Type == ENUMEntryType.E)
                    {
                        db.Update(_objBK01);
                        _objResponse.Message = $"Book with Id {Id} Edited";
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

        /// <summary>
        /// Deletes a book by ID.
        /// </summary>
        /// <returns>Response with operation status.</returns>
        public Response Delete()
        {
            try
            {
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    if (Type == ENUMEntryType.D)
                    {
                        db.DeleteById<BK01>(Id);
                        _objResponse.Message = $"Book with Id {Id} Deleted";
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

        /// <summary>
        /// Updates the price of a book.
        /// </summary>
        /// <param name="price"></param>
        /// <returns>Response with updated price information.</returns>
        public Response UpdatePrice(decimal price)
        {
            try
            {
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    if (Type == ENUMEntryType.E)
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

        /// <summary>
        /// Find Max Price of Books 
        /// </summary>
        /// <returns>Max Price in decimal</returns>
        public Response MaxPrice()
        {
            try
            {
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    decimal maxPrice = db.Scalar<decimal>("SELECT MAX(K01F05) FROM BK01");
                    _objResponse.Data = maxPrice; 
                    _objResponse.Message = $"Max Price of book : {maxPrice}";

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
