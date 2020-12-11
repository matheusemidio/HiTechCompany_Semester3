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
    public class AuthorBookDB
    {
        public static List<AuthorBook> GetListRecord()
        {
            List<AuthorBook> listCustomer = new List<AuthorBook>();
            AuthorBook aAuthorBook;
            using (SqlConnection conn = UtilityDB.ConnectDB())
            {
                SqlCommand cmdSelect = new SqlCommand("SELECT * FROM AuthorBooks", conn);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader();
                if (sqlReader.HasRows)
                {
                    while (sqlReader.Read())
                    {
                        aAuthorBook = new AuthorBook();
                        aAuthorBook.ISBN = sqlReader["ISBN"].ToString();
                        aAuthorBook.AuthorId = Convert.ToInt32(sqlReader["AuthorId"]);
                        aAuthorBook.YearPublished = Convert.ToInt32(sqlReader["YearPublished"]);
                        aAuthorBook.Edition = Convert.ToInt32(sqlReader["Edition"]);

                        listCustomer.Add(aAuthorBook);
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
