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
    [RoutePrefix("api/Books")]
    public class CLBookController : ApiController
    {
        private BLBooks _objBLBook = new BLBooks();
        private Response _objResponse;

        [HttpGet]
        [Route("GetAllBooks")]
        public IHttpActionResult GetAllBooks()
        {
            _objResponse = _objBLBook.GetAll();
            return Ok(_objResponse);
        }

        [HttpGet]
        [Route("GetBookById")]
        public IHttpActionResult GetBookById(int id)
        {
            _objResponse = _objBLBook.GetById(id);
            return Ok(_objResponse);
        }

        [HttpGet]
        [Route("GetBooksByCategory")]
        public IHttpActionResult GetBooksByCategory(string category)
        {
            _objResponse = _objBLBook.GetByCategory(category);
            return Ok(_objResponse);
        }

        [HttpGet]
        [Route("GetNLatestBooks")]
        public IHttpActionResult GetNLatestBooks(int numberOfBooks)
        {
            _objResponse = _objBLBook.GetNLatest(numberOfBooks);
            return Ok(_objResponse);
        }

        [HttpGet]
        [Route("GetBooksInRange")]
        public IHttpActionResult GetBooksInRange(int minimum,int maximum)
        {
            _objResponse = _objBLBook.GetInRange(minimum, maximum);
            return Ok(_objResponse);
        }

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
