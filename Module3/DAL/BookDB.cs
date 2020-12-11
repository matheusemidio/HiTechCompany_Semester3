using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Module3.Business;
using System.Data;
using System.Data.SqlClient;

namespace Module3.DAL
{
    public class BookDB
    {
        public static List<Book> GetListRecord()
        {
            List<Book> listCustomer = new List<Book>();
            Book aBook;
            using (SqlConnection conn = UtilityDB.ConnectDB())
            {
                SqlCommand cmdSelect = new SqlCommand("SELECT * FROM Books", conn);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader();
                if (sqlReader.HasRows)
                {
                    while (sqlReader.Read())
                    {
                        aBook = new Book();
                        aBook.ISBN = sqlReader["ISBN"].ToString();
                        aBook.Title = sqlReader["Title"].ToString();
                        aBook.UnitPrice = Convert.ToDouble(sqlReader["UnitPrice"]);
                        aBook.QOH = Convert.ToInt32(sqlReader["QOH"]);
                        aBook.CategoryId = Convert.ToInt32(sqlReader["CategoryId"]);
                        aBook.PublisherId = Convert.ToInt32(sqlReader["PublisherId"]);

                        listCustomer.Add(aBook);
                    }
                }
                else
                {
                    listCustomer = null;
                }
            }
            return listCustomer;
        }

    }
}
