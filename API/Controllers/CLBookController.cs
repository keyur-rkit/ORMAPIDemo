using API.BL.Operations;
using API.Models;
using API.Models.DTO;
using API.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace API.Controllers
{
    /// <summary>
    /// Book Controller for methods related books table 
    /// </summary>
    [RoutePrefix("api/Books")]
    public class CLBookController : ApiController
    {
        private BLBooks _objBLBook = new BLBooks();
        private Response _objResponse;

        /// <summary>
        /// Get all books method
        /// </summary>
        /// <returns>IHttpActionResult response</returns>
        [HttpGet]
        [Route("GetAllBooks")]
        public IHttpActionResult GetAllBooks()
        {
            _objResponse = _objBLBook.GetAll();
            return Ok(_objResponse);
        }

        /// <summary>
        /// Get book by id method
        /// </summary>
        /// <param name="id">Id of the book.</param>
        /// <returns>IHttpActionResult response</returns>
        [HttpGet]
        [Route("GetBookById")]
        public IHttpActionResult GetBookById(int id)
        {
            _objResponse = _objBLBook.GetById(id);
            return Ok(_objResponse);
        }

        /// <summary>
        /// Get books by category method
        /// </summary>
        /// <param name="category">Category of books to retrieve.</param>
        /// <returns>IHttpActionResult response</returns>

        [HttpGet]
        [Route("GetBooksByCategory")]
        public IHttpActionResult GetBooksByCategory(string category)
        {
            _objResponse = _objBLBook.GetByCategory(category);
            return Ok(_objResponse);
        }

        /// <summary>
        /// Get N number of latest books
        /// </summary>
        /// <param name="numberOfBooks">Number of latest books to retrieve.</param>
        /// <returns>IHttpActionResult response</returns>
        [HttpGet]
        [Route("GetNLatestBooks")]
        public IHttpActionResult GetNLatestBooks(int numberOfBooks)
        {
            _objResponse = _objBLBook.GetNLatest(numberOfBooks);
            return Ok(_objResponse);
        }

        /// <summary>
        /// Get books in specified range method
        /// </summary>
        /// <param name="minimum">Minimum book price.</param>
        /// <param name="maximum">Maximum book price.</param>
        /// <returns>IHttpActionResult response</returns>
        [HttpGet]
        [Route("GetBooksInRange")]
        public IHttpActionResult GetBooksInRange(int minimum,int maximum)
        {
            _objResponse = _objBLBook.GetInRange(minimum, maximum);
            return Ok(_objResponse);
        }

        /// <summary>
        /// Get Max Price of Book method
        /// </summary>
        /// <returns>IHttpActionResult response</returns>
        [HttpGet]
        [Route("GetMaxPrice")]
        public IHttpActionResult GetMaxPrice()
        {
            _objResponse = _objBLBook.MaxPrice();
            return Ok(_objResponse);
        }

        /// <summary>
        /// Add new book method
        /// </summary>
        /// <param name="objDTOBK01">DTO containing book details.</param>
        /// <returns>IHttpActionResult response</returns>
        [HttpPost]
        [Route("AddBook")]
        public IHttpActionResult AddBook(DTOBK01 objDTOBK01)
        {
            _objBLBook.Type = ENUMEntryType.A;
            _objBLBook.PreSave(objDTOBK01);
            _objResponse = _objBLBook.Validation();
            if (!_objResponse.IsError)
            {
                _objResponse = _objBLBook.Save();
            }
            return Ok(_objResponse);
        }

        /// <summary>
        /// Edit book method
        /// </summary>
        /// <param name="id">Id of the book to update.</param>
        /// <param name="objDTOBK01">DTO containing updated book details.</param>
        /// <returns>IHttpActionResult response</returns>
        [HttpPut]
        [Route("EditBook")]
        public IHttpActionResult EditBook(int id, DTOBK01 objDTOBK01)
        {
            _objBLBook.Type = ENUMEntryType.E;
            _objBLBook.Id = id;
            _objBLBook.PreSave(objDTOBK01);
            _objResponse = _objBLBook.Validation();
            if (!_objResponse.IsError)
            {
                _objResponse= _objBLBook.Save();
            }

            return Ok(_objResponse);
        }

        /// <summary>
        /// Delete book is exist method
        /// </summary>
        /// <param name="id">Id of the book to delete.</param>
        /// <returns>IHttpActionResult response</returns>
        [HttpDelete]
        [Route("DeleteBook")]
        public IHttpActionResult DeleteBook(int id)
        {
            _objBLBook.Type = ENUMEntryType.D;
            _objBLBook.Id = id;
            _objResponse = _objBLBook.Validation();
            if (!_objResponse.IsError)
            {
                _objResponse = _objBLBook.Delete();
            }
            return Ok(_objResponse);
        }

        /// <summary>
        /// Edit only price of book method
        /// </summary>
        /// <param name="id">Id of the book.</param>
        /// <param name="price">New price to set for the book.</param>
        /// <returns>IHttpActionResult response</returns>
        [HttpPatch]
        [Route("EditBookPrice")]
        public IHttpActionResult EditBookPrice(int id,decimal price)
        {
            _objBLBook.Type = ENUMEntryType.E;
            _objBLBook.Id = id;
            _objResponse = _objBLBook.Validation();
            if (!_objResponse.IsError)
            {
                _objResponse = _objBLBook.UpdatePrice(price); 
            }
            return Ok(_objResponse);
        }
    }
}
