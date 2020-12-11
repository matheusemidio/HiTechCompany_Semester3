using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module3.DAL;


namespace Module3.Business
{
    public class Category
    {

        private int categoryId;
        private string categoryName;


        public int CategoryId { get => categoryId; set => categoryId = value; }
        public string CategoryName { get => categoryName; set => categoryName = value; }

        public List<Category> CategoryList()
        {
            return (CategoryDB.GetListRecord());
        }
    }
}
