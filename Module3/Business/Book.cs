using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module3.DAL;


namespace Module3.Business
{
    public class Book
    {
        private string isbn;
        private string title;
        private double unitPrice;
        private int qoh;
        private int categoryId;
        private int publisherId;

        public string ISBN { get => isbn; set => isbn = value; }
        public string Title { get => title; set => title = value; }
        public double UnitPrice { get => unitPrice; set => unitPrice = value; }
        public int QOH { get => qoh; set => qoh = value; }
        public int CategoryId { get => categoryId; set => categoryId = value; }
        public int PublisherId { get => publisherId; set => publisherId = value; }



        public List<Book> BookList()
        {
            return (BookDB.GetListRecord());
        }





    }
}
