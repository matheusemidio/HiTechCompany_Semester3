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
    public class PublisherDB
    {
        public static List<Publisher> GetListRecord()
        {
            List<Publisher> listCustomer = new List<Publisher>();
            Publisher aPublisher;
            using (SqlConnection conn = UtilityDB.ConnectDB())
            {
                SqlCommand cmdSelect = new SqlCommand("SELECT * FROM Publishers", conn);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader();
                if (sqlReader.HasRows)
                {
                    while (sqlReader.Read())
                    {
                        aPublisher = new Publisher();
                        aPublisher.PublisherId = Convert.ToInt32(sqlReader["PublisherId"]);
                        aPublisher.PublisherName = sqlReader["PublisherName"].ToString();

                        listCustomer.Add(aPublisher);
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
