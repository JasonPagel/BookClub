using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FanModelSvc.Models;

namespace FanModelSvc.Services
{
    public class BookRepository
    {
        private const string CacheKey = "BookStore";

        public BookRepository()
        {

            var currentData = GetCurrentData();
            if (currentData == null)
            {
                CacheData(new Book[0]);
                var stephenId = new Guid("6e404c58-b91f-43b2-8719-1f3f760c2fd1");
                var deanId = new Guid("3a5c42a6-7469-4735-b60c-e8055dbc6862");
                var anneId = new Guid("966870b4-8d5b-4365-951b-5744a0903759");

                var books = new Book[]
                {
                    new Book() {id = Guid.NewGuid(), name="It", authorid=stephenId, description="scary clown story", image="https://images-na.ssl-images-amazon.com/images/I/51MeIXuanQL._AC_US160_.jpg", pages=1000, published=new DateTime(1985,11,3) },
                    new Book() {id = Guid.NewGuid(), name="The Dark Tower I: The Gunslinger", authorid=stephenId, description="other dimensions asdlkfjasdlf alsdjkfladksjf alsdkfj aldskfjalsdf klasdkjf alsdkfjalds fkjasldfkajsldfkj sdlfkjsldfkja lsdfkj sldfkjsldf jsldkfjsld jflsdkjf sldfkjs ldfkjsdlfj ksldfkjsldfkj sldfkj sldfkjsd flsldfj sldkfjs ldfkj sldkfjsldfkjs dflksjdflk sdfjlsd jflsdjkf klj", image="https://images-na.ssl-images-amazon.com/images/I/51wc33jjtxL._AC_US160_.jpg", pages=750, published=new DateTime(1986,1,1) },
                    new Book() {id = Guid.NewGuid(), name="Prince Lestat and the Realms of Atlantis", authorid=anneId, description="other dimensions asdlkfjasdlf alsdjkfladksjf alsdkfj aldskfjalsdf klasdkjf alsdkfjalds fkjasldfkajsldfkj sdlfkjsldfkja lsdfkj sldfkjsldf jsldkfjsld jflsdkjf sldfkjs ldfkjsdlfj ksldfkjsldfkj sldfkj sldfkjsd flsldfj sldkfjs ldfkj sldkfjsldfkjs dflksjdflk sdfjlsd jflsdjkf klj", image="https://images-na.ssl-images-amazon.com/images/I/511Ch2DOAxL._AC_US320_FMwebp_QL65_.jpg", pages=432, published=new DateTime(2016,11,29) },
                    new Book() {id = Guid.NewGuid(), name="The Dark Tower II: The Drawing of the Three", authorid=stephenId, description="other dimensions", image="https://images-na.ssl-images-amazon.com/images/I/51o1eA9vjIL._AC_US160_.jpg", pages=862, published=new DateTime(1988,12,6) },
                    new Book() {id = Guid.NewGuid(), name="Prince Lestat", authorid=anneId, description="other dimensions asdlkfjasdlf alsdjkfladksjf alsdkfj aldskfjalsdf klasdkjf alsdkfjalds fkjasldfkajsldfkj sdlfkjsldfkja lsdfkj sldfkjsldf jsldkfjsld jflsdkjf sldfkjs ldfkjsdlfj ksldfkjsldfkj sldfkj sldfkjsd flsldfj sldkfjs ldfkj sldkfjsldfkjs dflksjdflk sdfjlsd jflsdjkf klj", image="https://images-na.ssl-images-amazon.com/images/I/41jLXf-FBsL._AC_US320_FMwebp_QL65_.jpg", pages=643, published=new DateTime(2014,3,14) },
                    new Book() {id = Guid.NewGuid(), name="Watchers", authorid=deanId, description="there are people watching us", image="https://images-na.ssl-images-amazon.com/images/I/51R+PUSbmLL._AC_US160_.jpg", pages=632, published=new DateTime(2003,1,28) }
                };

                CacheData(books);
            }
        }

        public bool DeleteBookByAuthor(Guid id)
        {
            try
            {
                var books = GetCurrentData();
                books.RemoveAll(x => x.authorid == id);
                CacheData(books);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public Book[] GetBooks()
        {
            return GetCurrentDataWithAuthor().ToArray();
        }
        public Book[] GetBooks(Guid authorid)
        {
            try
            {
                var currentData = GetCurrentDataWithAuthor();
                var books = currentData.Where(x => x.authorid == authorid);
                return books.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Book[0];
            }
        }

        public Book[] GetBooks(string search)
        {
            var currentData = GetCurrentDataWithAuthor();
            var lSearch = search.ToLower();
            var books = currentData.Where(x => x.name.ToLower().Contains(lSearch) || x.authorname.ToLower().Contains(lSearch));
            return books.ToArray();
        }

        private List<Book> GetCurrentDataWithAuthor()
        {
            var books = GetCurrentData();

            foreach (var book in books)
            {
                book.authorname = GetAuthorName(book.authorid);
            }

            return books;
        }
        private List<Book> GetCurrentData()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var currentData = ((Book[])ctx.Cache[CacheKey]).ToList();
                    return currentData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return null;
                }
            }
            return null;
        }
        private bool CacheData(IEnumerable<Book> books)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                ctx.Cache[CacheKey] = books.ToArray();
                return true;
            }

            return false;
        }

        private string GetAuthorName(Guid id)
        {
            var authors = new AuthorsRepository();
            var found = authors.GetAuthor(id);
            if (found != null)
                return found.name;
            return string.Empty;
        }
        private Guid GetAuthorId(string name)
        {
            var authors = new AuthorsRepository();
            var found = authors.GetAuthors(name);
            if (found != null)
            {
                var author = found.FirstOrDefault();

                if (author != null)
                    return author.id;
            }

            return Guid.Empty;
        }
    }

}
