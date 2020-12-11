using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module3.DAL;


namespace Module3.Business
{
    public class Author
    {
        private int authorId;
        private string firstName;
        private string lastName;
        private string email;
        private int categoryId;
        private int publisherId;

        public int AuthorId { get => authorId; set => authorId = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Email { get => email; set => email = value; }

        public List<Author> AuthorList()
        {
            return (AuthorDB.GetListRecord());
        }

    }
}
