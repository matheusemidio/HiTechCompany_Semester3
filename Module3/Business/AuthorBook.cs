using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module3.DAL;


namespace Module3.Business
{
    public class AuthorBook
    {
        private string isbn;
        private int authorId;
        private int yearPublished;
        private int edition;

        public String ISBN { get => isbn; set => isbn = value; }
        public int AuthorId { get => authorId; set => authorId = value; }
        public int YearPublished { get => yearPublished; set => yearPublished = value; }
        public int Edition { get => edition; set => edition = value; }

        public List<AuthorBook> AuthorBookList()
        {
            return (AuthorBookDB.GetListRecord());
        }
    }
}
