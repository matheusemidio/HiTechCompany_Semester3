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
    public class AuthorDB
    {
        public static List<Author> GetListRecord()
        {
            List<Author> listAuthor = new List<Author>();
            Author aAuthor;
            using (SqlConnection conn = UtilityDB.ConnectDB())
            {
                SqlCommand cmdSelect = new SqlCommand("SELECT * FROM Authors", conn);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader();
                if (sqlReader.HasRows)
                {
                    while (sqlReader.Read())
                    {
                        aAuthor = new Author();
                        aAuthor.AuthorId = Convert.ToInt32(sqlReader["AuthorId"]);
                        aAuthor.FirstName = sqlReader["FirstName"].ToString();
                        aAuthor.LastName = sqlReader["LastName"].ToString();
                        aAuthor.Email = sqlReader["Email"].ToString();

                        listAuthor.Add(aAuthor);
                    }
                }
                else
                {
                    listAuthor = null;
                }
            }
            return listAuthor;
        }

    }
}
