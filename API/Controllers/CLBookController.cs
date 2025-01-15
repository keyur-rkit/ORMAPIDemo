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
            var books = _objBLBook.GetAll();
            return Ok(books);
        }

        [HttpGet]
        [Route("GetBookById")]
        public IHttpActionResult GetBookById(int id)
        {
            var book = _objBLBook.GetById(id);
            return Ok(book);
        }

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
    }
}
