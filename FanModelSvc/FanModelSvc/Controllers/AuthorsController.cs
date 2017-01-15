using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using FanModelSvc.Models;
using FanModelSvc.Services;

namespace FanModelSvc.Controllers
{
    [MyAuthorization]
    public class AuthorsController : ApiController
    {

        private AuthorsRepository repo;

        public AuthorsController()
        {
            this.repo = new Services.AuthorsRepository();
        }

        [HttpGet]
        public Author[] Get()
        {
            return repo.GetAllAuthors();
        }

        [HttpGet]
        public Author[] Get(string search)
        {
            return repo.GetAuthors(search);
        }

        [HttpPut]
        public void Put(Author author)
        {
            int test = 1;
            if (test == 1)
            {
                if (!repo.UpdateAuthor(author))
                    throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        public HttpResponseMessage Post(Author author)
        {
            int test = 1;
            if (test == 1)
            {
                repo.SaveAuthor(author);

                var response = Request.CreateResponse<Author>(System.Net.HttpStatusCode.Created, author);
                return response;
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpDelete]
        public HttpResponseMessage Delete(Guid id)
        {
            int test = 1;
            if (test == 1)
            {
                if (repo.DeleteAuthor(id))
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}
