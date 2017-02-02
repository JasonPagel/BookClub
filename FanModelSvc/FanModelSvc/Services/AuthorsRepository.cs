using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FanModelSvc.Models;

namespace FanModelSvc.Services
{
    public class AuthorsRepository
    {

        private const string CacheKey = "AuthorStore";
        private BookRepository books;

        public AuthorsRepository()
        {
            books = new BookRepository();

            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                if (ctx.Cache[CacheKey] == null)
                {
                    var authors = new Author[]
                    {
                        new Author {id = new Guid("6e404c58-b91f-43b2-8719-1f3f760c2fd1"), name="Stephen King", description="Lorem ipsum dolor sit amet, dui nisl dapibus. Felis sed elit. Sodales pede nam luctus dapibus, nulla porta ut lorem scelerisque amet, sodales cras rutrum, cursus vehicula hendrerit porttitor. Donec justo ipsum. Parturient iaculis, porttitor deserunt mi nec, lectus porttitor, malesuada lorem nulla nulla congue nonummy maecenas, tristique ut volutpat eget voluptates sed. Molestie ut pellentesque viverra et molestie, in at integer nulla, diam adipiscing asperiores gravida tempor, nullam metus libero massa, tellus sit felis. Sociis nunc, purus eu, nonummy amet malesuada duis habitasse vivamus, ac eu in, aliquam pede. Urna vulputate malesuada neque pede vitae varius, ac hymenaeos amet ipsum pretium imperdiet, vestibulum magnis morbi porttitor tempus, felis viverra vestibulum lacus mauris eros ut, turpis ultrices nec quis urna rhoncus.", image="https://upload.wikimedia.org/wikipedia/commons/thumb/e/e3/Stephen_King%2C_Comicon.jpg/440px-Stephen_King%2C_Comicon.jpg" },
                        new Author { id = new Guid("966870b4-8d5b-4365-951b-5744a0903759"), name="Anne Rice", description="Lorem ipsum dolor sit amet, dui nisl dapibus. Felis sed elit. Sodales pede nam luctus dapibus, nulla porta ut lorem scelerisque amet, sodales cras rutrum, cursus vehicula hendrerit porttitor. Donec justo ipsum. Parturient iaculis, porttitor deserunt mi nec, lectus porttitor, malesuada lorem nulla nulla congue nonummy maecenas, tristique ut volutpat eget voluptates sed. Molestie ut pellentesque viverra et molestie, in at integer nulla, diam adipiscing asperiores gravida tempor, nullam metus libero massa, tellus sit felis. Sociis nunc, purus eu, nonummy amet malesuada duis habitasse vivamus, ac eu in, aliquam pede. Urna vulputate malesuada neque pede vitae varius, ac hymenaeos amet ipsum pretium imperdiet, vestibulum magnis morbi porttitor tempus, felis viverra vestibulum lacus mauris eros ut, turpis ultrices nec quis urna rhoncus.", image="https://upload.wikimedia.org/wikipedia/commons/5/55/Anne_Rice.jpg" },
                        new Author {id = new Guid("3a5c42a6-7469-4735-b60c-e8055dbc6862"), name="Dean Koontz", description="Lorem ipsum dolor sit amet, dui nisl dapibus. Felis sed elit. Sodales pede nam luctus dapibus, nulla porta ut lorem scelerisque amet, sodales cras rutrum, cursus vehicula hendrerit porttitor. Donec justo ipsum. Parturient iaculis, porttitor deserunt mi nec, lectus porttitor, malesuada lorem nulla nulla congue nonummy maecenas, tristique ut volutpat eget voluptates sed. Molestie ut pellentesque viverra et molestie, in at integer nulla, diam adipiscing asperiores gravida tempor, nullam metus libero massa, tellus sit felis. Sociis nunc, purus eu, nonummy amet malesuada duis habitasse vivamus, ac eu in, aliquam pede. Urna vulputate malesuada neque pede vitae varius, ac hymenaeos amet ipsum pretium imperdiet, vestibulum magnis morbi porttitor tempus, felis viverra vestibulum lacus mauris eros ut, turpis ultrices nec quis urna rhoncus.", image="https://images-na.ssl-images-amazon.com/images/I/31VFgcQjZZL._UX250_.jpg" }
                    };

                    ctx.Cache[CacheKey] = authors;
                }
            }
        }

        public Author GetAuthor(Guid id)
        {
            var ctx = HttpContext.Current;

            try
            {
                if (ctx != null)
                {
                    var authors = ((Author[])ctx.Cache[CacheKey]).ToList();
                    var author = authors.FirstOrDefault(x => x.id == id);
                    return author;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

            return null;
        }

        private Author[] UpdateStats(Author[] authors)
        {
            foreach (var author in authors)
            {
                author.bookcount = books.GetBooks(author.id).Count();
            }
            return authors;
        }

        public Author[] GetAllAuthors()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                return UpdateStats((Author[])ctx.Cache[CacheKey]);
            }

            return new Author[0];
        }

        public bool UpdateAuthor(Author author)
        {
            var ctx = HttpContext.Current;

            try
            {
                if (ctx != null)
                {
                    var authors = ((Author[])ctx.Cache[CacheKey]).ToList();
                    
                    var iCnt = authors.RemoveAll(x => x.id == author.id);

                    if (iCnt > 0)
                    {
                        authors.Add(author);
                        ctx.Cache[CacheKey] = authors.ToArray();
                        return true;
                    }

                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public Author[] GetAuthors(string search)
        {
            var ctx = HttpContext.Current;
            
            try
            {
                if (ctx != null)
                {
                    var authors = (Author[])ctx.Cache[CacheKey];
                    return UpdateStats(authors.Where(x => x.name.ToLower().Contains(search.ToLower())).ToArray());
                }

                return new Author[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Author[0];
            }

        }

        public bool SaveAuthor(Author author)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var currentData = ((Author[])ctx.Cache[CacheKey]).ToList();
                    author.id = Guid.NewGuid();
                    currentData.Add(author);
                    ctx.Cache[CacheKey] = currentData.ToArray();

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }

            return false;
        }

        public bool DeleteAuthor(Guid id)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var currentData = ((Author[])ctx.Cache[CacheKey]).ToList();

                    var items = currentData.Where(x => x.id == id);
                    if (items.Count() < 1)
                        return false;

                    if (books.DeleteBookByAuthor(id))
                    {
                        currentData.RemoveAll(x => x.id == id);
                        ctx.Cache[CacheKey] = currentData.ToArray();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }

            return false;
        }
    }
}