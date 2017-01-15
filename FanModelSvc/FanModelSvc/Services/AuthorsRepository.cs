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
                        new Author {id = new Guid("6e404c58-b91f-43b2-8719-1f3f760c2fd1"), name="Stephen King", description="Scary story specialist", image="https://upload.wikimedia.org/wikipedia/commons/thumb/e/e3/Stephen_King%2C_Comicon.jpg/440px-Stephen_King%2C_Comicon.jpg" },
                        new Author { id = new Guid("966870b4-8d5b-4365-951b-5744a0903759"), name="Anne Rice", description="asd; flkasd; f kasd; flkas d; flkasd; flkasdfkasdfha ksjdfh aksdjfha lsdkjfhask dfjhasdfk ljahdflkajdfh alksdjfh akldsfjha skdjfahsldk fjalksdfjh akdsfjh adskfjha sdklfjha sdklfjhasd fkljasdfh aklsdjfha sdklfjha sdkfljhasd flkajsdfhlakdsfjh asdkfjh asdfklja dsfkljasdfk ljahsdflkaj sdfhklajsdfh akldsjfh akldsjfh aksdfjh asdkjfha sdlkfja sdflkja d", image="https://upload.wikimedia.org/wikipedia/commons/5/55/Anne_Rice.jpg" },
                        new Author {id = new Guid("3a5c42a6-7469-4735-b60c-e8055dbc6862"), name="Dean Koontz", description="sci-fi and drama", image="https://images-na.ssl-images-amazon.com/images/I/31VFgcQjZZL._UX250_.jpg" }
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