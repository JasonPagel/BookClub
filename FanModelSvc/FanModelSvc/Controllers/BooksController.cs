using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FanModelSvc.Services;
using FanModelSvc.Models;

namespace FanModelSvc.Controllers
{
    public class BooksController : ApiController
    {
        private BookRepository repo;

        public BooksController()
        {
            repo = new BookRepository();
        }

        [HttpGet]
        [MyAuthorization]
        public Book[] Get()
        {
            return repo.GetBooks();
        }

        [HttpGet]
        [MyAuthorization]
        public Book[] Get(string search)
        {
            return repo.GetBooks(search);
        }

        [HttpGet]
        [MyAuthorization]
        public Book[] Get(Guid author)
        {
            return repo.GetBooks(author);
        }
    }
}
