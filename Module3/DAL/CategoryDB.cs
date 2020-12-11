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
    public class CategoryDB
    {
        public static List<Category> GetListRecord()
        {
            List<Category> listCustomer = new List<Category>();
            Category aCategory;
            using (SqlConnection conn = UtilityDB.ConnectDB())
            {
                SqlCommand cmdSelect = new SqlCommand("SELECT * FROM Categories", conn);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader();
                if (sqlReader.HasRows)
                {
                    while (sqlReader.Read())
                    {
                        aCategory = new Category();
                        aCategory.CategoryId = Convert.ToInt32(sqlReader["CategoryId"]);
                        aCategory.CategoryName = sqlReader["CategoryName"].ToString();

                        listCustomer.Add(aCategory);
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
