using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Module4.Models;
using Module1.Validation;

namespace Module4.GUI
{
     
    
    public partial class OrderDetailsForm : Form
    {

        HiTechDistributionDBEntities2 dbEntities = new HiTechDistributionDBEntities2();


        public OrderDetailsForm()
        {
            InitializeComponent();
        }

        private void booksToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OrderClerkForm orderClerkForm = new OrderClerkForm();
            this.Hide();
            orderClerkForm.ShowDialog();
        }

        private void categoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrderDetailsForm orderDetailsForm = new OrderDetailsForm();
            this.Hide();
            orderDetailsForm.ShowDialog();
        }

        private void buttonListOrdersDetails_Click(object sender, EventArgs e)
        {
            //var listOrderDetails = (from OrderDetail in dbEntities.OrderDetails select OrderDetail).ToList<OrderDetail>();
            //dataGridViewOrderDetail.DataSource = listOrderDetails;
            var listOrdersDetails = from orderDetail in dbEntities.OrderDetails
                             select new
                             {
                                 orderDetail.OrderId,
                                 orderDetail.ISBN,
                                 orderDetail.QuantityOrdered
                             };
            dataGridViewOrderDetail.DataSource = listOrdersDetails.ToList();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            //validation
            string tempOrderId = textBoxOrderId.Text.Trim();
            if (!(Validator.isValidTwoDigitId(tempOrderId)))
            {
                MessageBox.Show("Order ID must be 2-digit number", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxOrderId.Clear();
                textBoxOrderId.Focus();
                return;
            }
            Order order = new Order();
            order = dbEntities.Orders.Find(Convert.ToInt32(tempOrderId));
            if (order == null)
            {
                MessageBox.Show("This order does not exist on the database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxOrderId.Clear();
                textBoxOrderId.Focus();

                return;
            }
            string tempISBN = textBoxISBN.Text.Trim();
            if (!(Validator.isValidISBN(tempISBN)))
            {
                MessageBox.Show("ISBN must be 13-digit number", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxISBN.Clear();
                textBoxISBN.Focus();
                return;
            }
            Book book = new Book();
            book = dbEntities.Books.Find(tempISBN);
            if (book == null)
            {
                MessageBox.Show("This book does not exist on the database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxISBN.Clear();
                textBoxISBN.Focus();
                return;
            }
            string tempQuantityOrdered = textBoxQuantityOrdered.Text.Trim();
            if (!(Validator.isValidThreeDigitNumber(tempQuantityOrdered)))
            {
                MessageBox.Show("Invalid quantity ordered", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxISBN.Clear();
                textBoxISBN.Focus();
                return;
            }

            if(Convert.ToInt32(tempQuantityOrdered) > book.QOH)
            {
                MessageBox.Show("We do not have the enough quantity of this book on our inventory", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dataGridViewOrderDetail.DataSource = book;
                textBoxQuantityOrdered.Clear();
                return;
            }



            //operation
            OrderDetail orderDetail = new OrderDetail();

            orderDetail.OrderId = Convert.ToInt32(tempOrderId);
            orderDetail.ISBN = tempISBN.ToString();
            orderDetail.QuantityOrdered = Convert.ToInt32(tempQuantityOrdered);

            OrderDetail aOrderDetail = new OrderDetail();

            //aOrderDetail = dbEntities.Set<OrderDetail>().Find(Convert.ToInt32(aOrderDetail.OrderId), aOrderDetail.ISBN);
            int tempOrderId2 = Convert.ToInt32(tempOrderId);
            //aOrderDetail = dbEntities.OrderDetails.Find(Convert.ToInt32(aOrderDetail.OrderId), aOrderDetail.ISBN);
            var query = from ord in dbEntities.Orders
                        from book1 in dbEntities.Books
                        join orderDet in dbEntities.OrderDetails
                            on new { ord.OrderId, book1.ISBN } equals new { orderDet.OrderId, orderDet.ISBN }
                            into details
                        from det in details
                        select new { ord.OrderId, book1.ISBN, det.QuantityOrdered };
            //System.Diagnostics.Debug.WriteLine(query);
            OrderDetail od = new OrderDetail();
            Object[] ArrayOfObjects = new Object[] { query.ToArray()};
            od.OrderId = Convert.ToInt32(ArrayOfObjects[0]);
            od.ISBN = ArrayOfObjects[1].ToString();
            od.QuantityOrdered = Convert.ToInt32(ArrayOfObjects[2]);
            if (od != null)
            {


                MessageBox.Show("This Order Detail is already on the database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxOrderId.Clear();
                textBoxISBN.Clear();

            }
            else
            {
                book.QOH = book.QOH - orderDetail.QuantityOrdered;
                //var b = dbEntities.Books.First<Book>();
                var b = dbEntities.Books.Find(tempISBN);
                b.ISBN = book.ISBN;
                b.Title = book.Title;
                b.UnitPrice = book.UnitPrice;
                b.QOH = book.QOH;
                b.CategoryId = book.CategoryId;
                b.PublisherId = book.PublisherId;
                this.dbEntities.OrderDetails.Add(orderDetail);
                this.dbEntities.SaveChanges();
                MessageBox.Show("Order Detail Information was saved successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                textBoxOrderId.Clear();
                textBoxISBN.Clear();
                textBoxQuantityOrdered.Clear();

            }
        }

        private void buttonListBooks_Click(object sender, EventArgs e)
        {
            //var listBooks = (from book in dbEntities.Books select book).ToList<Book>();
            //dataGridViewOrderDetail.DataSource = listBooks;
            var listBooks = from book in dbEntities.Books
                             select new
                             {
                                 book.ISBN,
                                 book.Title,
                                 book.UnitPrice,
                                 book.QOH,
                                 book.CategoryId,
                                 book.PublisherId
                             };
            dataGridViewOrderDetail.DataSource = listBooks.ToList();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            //validation
            string tempOrderId = textBoxOrderId.Text.Trim();
            if (!(Validator.isValidTwoDigitId(tempOrderId)))
            {
                MessageBox.Show("Order ID must be 2-digit number", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxISBN.Clear();
                textBoxISBN.Focus();
                return;
            }
            Order order = new Order();
            order = dbEntities.Orders.Find(Convert.ToInt32(tempOrderId));
            if (order == null)
            {
                MessageBox.Show("This order does not exist on the database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxOrderId.Clear();
                return;
            }
            string tempISBN = textBoxISBN.Text.Trim();

            if (!(Validator.isValidISBN(tempISBN)))
            {
                MessageBox.Show("ISBN must be 13-digit number", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxISBN.Clear();
                textBoxISBN.Focus();
                return;
            }
            Book book = new Book();
            book = dbEntities.Books.Find(tempISBN);
            if (book == null)
            {
                MessageBox.Show("This book does not exist on the database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxISBN.Clear();
                return;
            }
            string tempQuantityOrdered = textBoxQuantityOrdered.Text.Trim();
            if (!(Validator.isValidThreeDigitNumber(tempQuantityOrdered)))
            {
                MessageBox.Show("Invalid quantity ordered", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxISBN.Clear();
                textBoxISBN.Focus();
                return;
            }

            if (Convert.ToInt32(tempQuantityOrdered) > book.QOH)
            {
                MessageBox.Show("We do not have the enough quantity of this book on our inventory", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dataGridViewOrderDetail.DataSource = book;
                textBoxQuantityOrdered.Clear();
                return;
            }




            //operation
            OrderDetail orderDetail = new OrderDetail();

            orderDetail.OrderId = Convert.ToInt32(tempOrderId);
            orderDetail.ISBN = tempISBN.ToString();
            orderDetail.QuantityOrdered = Convert.ToInt32(tempQuantityOrdered);

            OrderDetail aOrderDetail = new OrderDetail();
            aOrderDetail = dbEntities.OrderDetails.Find(Convert.ToInt32(aOrderDetail.OrderId), aOrderDetail.ISBN);

            if ((aOrderDetail != null))
            {

                book.QOH = book.QOH - orderDetail.QuantityOrdered;
                //var b = dbEntities.Books.First<Book>();
                var b = dbEntities.Books.Find(tempISBN);
                b.ISBN = book.ISBN;
                b.Title = book.Title;
                b.UnitPrice = book.UnitPrice;
                b.QOH = book.QOH;
                b.CategoryId = book.CategoryId;
                b.PublisherId = book.PublisherId;
                aOrderDetail.OrderId = orderDetail.OrderId;
                aOrderDetail.ISBN = orderDetail.ISBN;
                aOrderDetail.QuantityOrdered = orderDetail.QuantityOrdered;
                this.dbEntities.OrderDetails.Add(orderDetail);
                this.dbEntities.SaveChanges();

                MessageBox.Show("Order Detail Information was updated successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBoxOrderId.Clear();
                textBoxISBN.Clear();
                textBoxQuantityOrdered.Clear();



            }
            else
            {                
                MessageBox.Show("This Order ID or this ISBN are not on the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxOrderId.Clear();
                textBoxISBN.Clear();
            }
        }

        private void buttonListOrders_Click(object sender, EventArgs e)
        {
            //var listOrders = (from order in dbEntities.Orders select order).ToList<Order>();
            //dataGridViewOrderDetail.DataSource = listOrders;
            var listOrders = from order in dbEntities.Orders
                             select new
                             {
                                 order.OrderId,
                                 order.OrderDate,
                                 order.DeliveringDate,
                                 order.ShippingDate,
                                 order.OrderStatus,
                                 order.CustomerId,
                                 order.EmployeeId
                             };
            dataGridViewOrderDetail.DataSource = listOrders.ToList();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            DialogResult answer = MessageBox.Show("Would you like to exit the application?", "Exit Window", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (answer == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void buttonGeneratePrice_Click(object sender, EventArgs e)
        {
            var listOrdersDetails = from orderDetail in dbEntities.OrderDetails
                                    select new
                                    {
                                        orderDetail.OrderId,
                                        orderDetail.ISBN,
                                        orderDetail.QuantityOrdered
                                    };
            //dataGridViewOrderDetail.DataSource = listOrdersDetails.ToList();
            var listOrders = from order in dbEntities.Orders
                             select new
                             {
                                 order.OrderId,
                                 order.OrderDate,
                                 order.DeliveringDate,
                                 order.ShippingDate,
                                 order.OrderStatus,
                                 order.CustomerId,
                                 order.EmployeeId
                             };
            //dataGridViewOrderDetail.DataSource = listOrders.ToList();
            
            //foreach(Order o in listOrders.ToList())
            //{
            //    double sum = 0;
            //    foreach (OrderDetail od in listOrdersDetails.ToList())
            //    {
            //        if (o.OrderId == od.OrderId)
            //        {
            //            sum += od.QuantityOrdered *
            //        }
            //    }
            //}

        }
    }
}
