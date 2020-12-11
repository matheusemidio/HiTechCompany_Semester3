using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module3.DAL;

namespace Module3.Business
{
    public class Publisher
    {

        private int publisherId;
        private string publisherName;

        public int PublisherId { get => publisherId; set => publisherId = value; }
        public string PublisherName { get => publisherName; set => publisherName = value; }

        public List<Publisher> PublisherList()
        {
            return (PublisherDB.GetListRecord());
        }
    }
}
