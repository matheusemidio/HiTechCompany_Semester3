using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module2.DAL;

namespace Module2.Business
{
    public class Customer
    {
        private int customerId;
        private string customerName;
        private string streetName;
        private string city;
        private string province;
        private string postalCode;
        private string phoneNumber;
        private string faxNumber;
        private string email;
        private int creditLimit;

        public int CustomerId { get => customerId; set => customerId = value; }
        public string CustomerName { get => customerName; set => customerName = value; }
        public string StreetName { get => streetName; set => streetName = value; }
        public string City { get => city; set => city = value; }
        public string Province { get => province; set => province = value; }
        public string PostalCode { get => postalCode; set => postalCode = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public string FaxNumber { get => faxNumber; set => faxNumber = value; }
        public string Email { get => email; set => email = value; }
        public int CreditLimit { get => creditLimit; set => creditLimit = value; }


        public List<Customer> CustomerList()
        {
            return (CustomerDB.GetListRecord());
        }



    }
}
