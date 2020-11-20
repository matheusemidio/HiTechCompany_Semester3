using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Module2.Business;
using Module2.DAL;

namespace Module2.DAL
{
    class CustomerDB
    {
        public static List<Customer> GetListRecord()
        {
            List<Customer> listCustomer = new List<Customer>();
            Customer aCustomer;
            using (SqlConnection conn = UtilityDB.ConnectDB())
            {
                SqlCommand cmdSelect = new SqlCommand("SELECT * FROM Customers", conn);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader();
                if (sqlReader.HasRows)
                {
                    while (sqlReader.Read())
                    {
                        aCustomer = new Customer();
                        aCustomer.CustomerId = Convert.ToInt32(sqlReader["CustomerId"]);
                        aCustomer.CustomerName = sqlReader["CustomerName"].ToString();
                        aCustomer.StreetName = sqlReader["StreetName"].ToString();
                        aCustomer.City = sqlReader["City"].ToString();
                        aCustomer.Province = sqlReader["Province"].ToString();
                        aCustomer.PostalCode = sqlReader["PostalCode"].ToString();
                        aCustomer.PhoneNumber = sqlReader["PhoneNumber"].ToString();
                        aCustomer.FaxNumber = sqlReader["FaxNumber"].ToString();
                        aCustomer.Email = sqlReader["Email"].ToString();
                        aCustomer.CreditLimit = Convert.ToInt32(sqlReader["CreditLimit"]);
                        listCustomer.Add(aCustomer);
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
