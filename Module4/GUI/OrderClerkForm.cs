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
using System.Diagnostics;




namespace Module4.GUI
{

    public partial class OrderClerkForm : Form
    {
        HiTechDistributionDBEntities2 dbEntities = new HiTechDistributionDBEntities2();
        Order order = new Order();
        public DateTime? StartDate { get; set; }

        public OrderClerkForm()
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

        private void OrderClerkForm_Load(object sender, EventArgs e)
        {
            comboBoxOrderStatus.SelectedIndex = 0;
            dateTimePickerOrderDate.Format = DateTimePickerFormat.Custom;
            dateTimePickerOrderDate.CustomFormat = "yyyy/MM/dd";
            dateTimePickerShippingDate.Format = DateTimePickerFormat.Custom;
            dateTimePickerShippingDate.CustomFormat = "yyyy/MM/dd";
            dateTimePickerDeliveringDate.Format = DateTimePickerFormat.Custom;
            dateTimePickerDeliveringDate.CustomFormat = "yyyy/MM/dd";


        }

        private void buttonListOrders_Click(object sender, EventArgs e)
        {
            //var listOrders = (from order in dbEntities.Orders select order).ToList<Order>();
            //dataGridViewOrder.DataSource = listOrders;
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
            dataGridViewOrder.DataSource = listOrders.ToList();
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
            string tempOrderStatus = comboBoxOrderStatus.SelectedItem.ToString();

            string tempCustomerId = textBoxCustomerId.Text.Trim();
            if (!(Validator.isValidTwoDigitId(tempCustomerId)))
            {
                MessageBox.Show("Customer ID must be 2-digit number", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxOrderId.Clear();
                textBoxOrderId.Focus();
                return;
            }
            string tempEmployeeId = textBoxEmployeeId.Text.Trim();
            if (!(Validator.isValidId(tempEmployeeId)))
            {
                MessageBox.Show("Employee ID must be 4-digit number", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxOrderId.Clear();
                textBoxOrderId.Focus();
                return;
            }

            //Special Validation with dates
            DateTime tempOrderDate = Convert.ToDateTime(dateTimePickerOrderDate.Value);
            if (!(dateTimePickerOrderDate.Checked))//Not checked
            {
                MessageBox.Show("Please, select an Order Date", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DateTime tempShippingDate = Convert.ToDateTime(dateTimePickerShippingDate.Value);
            if (!(dateTimePickerShippingDate.Checked))//Not checked
            {
                MessageBox.Show("Please, select a Shipping Date", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (DateTime.Compare(tempOrderDate,tempShippingDate) > 0)
            {
                MessageBox.Show("Shipping Date can not be before Order date", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;


            }
            DateTime tempDeliveringDate = Convert.ToDateTime(dateTimePickerDeliveringDate.Value);
            if (!(dateTimePickerDeliveringDate.Checked))//Not checked
            {
                MessageBox.Show("Please, select a Delivering Date", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (DateTime.Compare(tempOrderDate, tempDeliveringDate) > 0)
            {
                MessageBox.Show("Delivering date can not be before Order date", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //MessageBox.Show(tempOrderDate.ToShortDateString() + "\n" + tempShippingDate.ToShortDateString());
                return;


            }
            if (DateTime.Compare(tempShippingDate, tempDeliveringDate) > 0)
            {
                MessageBox.Show("Delivering date can not be before Shipping date", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                return;


            }

            //operation
            order.OrderId = Convert.ToInt32(tempOrderId);
            order.OrderDate = Convert.ToDateTime(tempOrderDate.ToShortDateString());
            order.ShippingDate = Convert.ToDateTime(tempShippingDate.ToShortDateString());
            order.DeliveringDate = Convert.ToDateTime(tempDeliveringDate.ToShortDateString());
            order.OrderStatus = tempOrderStatus;
            order.CustomerId = Convert.ToInt32(tempCustomerId);
            order.EmployeeId = Convert.ToInt32(tempEmployeeId);
            //Order aOrder = new Order();
            //aOrder = dbEntities.Orders.Find(Convert.ToInt32(order.OrderId));
            int tempOrderId2 = Convert.ToInt32(tempOrderId);
            //var searchOrder2 = from aOrder in dbEntities.Orders.Where(aOrder => aOrder.OrderId.Equals(tempOrderId2))
            //                  select new
            //                  {
            //                      aOrder.OrderId,
            //                      aOrder.OrderDate,
            //                      aOrder.DeliveringDate,
            //                      aOrder.ShippingDate,
            //                      aOrder.OrderStatus,
            //                      aOrder.CustomerId,
            //                      aOrder.EmployeeId
            //                  };

            var searchOrder2 = dbEntities.Orders.Any(aOrder => aOrder.OrderId.Equals(tempOrderId2));
            System.Diagnostics.Debug.Write(searchOrder2);
            if (!(searchOrder2))
            {
                
                dbEntities.Orders.Add(order);
                dbEntities.SaveChanges();
                MessageBox.Show("Order Information was saved successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBoxOrderId.Clear();
                textBoxCustomerId.Clear();
                textBoxEmployeeId.Clear();

            }
            else
            {
                MessageBox.Show("This Order Id is already on the database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxOrderId.Clear();

            }


        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            int indexSelected = comboBox.SelectedIndex;
            switch (indexSelected)
            {
                //Search By ISBN
                case 0:
                    //Validation
                    string tempInput = textBoxInput.Text.Trim();
                    if (!(Validator.isValidTwoDigitId(tempInput)))
                    {
                        MessageBox.Show("Order ID must be 2-digit number", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                        return;
                    }

                    //Order order = dbEntities.Orders.Find(Convert.ToInt32(tempInput));
                    int tempInput1 = Convert.ToInt32(tempInput);
                    var searchOrder = from order in dbEntities.Orders.Where(order => order.OrderId.Equals(tempInput1))
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
                    //Order order1 = new Order();
                    if (searchOrder != null)  //found
                    {
                        //dataGridViewOrder.DataSource = new List<Order> { order };
                        dataGridViewOrder.DataSource = searchOrder.ToList();



                    }
                    else
                    {
                        MessageBox.Show("Order ID does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                        dataGridViewOrder.DataSource = null;

                    }

                    break;
                case 1:
                    //Serch by Customer Name

                    string tempInput2 = textBoxInput.Text.Trim();
                    if (!(Validator.isValidTwoDigitId(tempInput2)))
                    {
                        MessageBox.Show("Customer ID must be 2-digit number", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                        return;
                    }
                    int tempInput3 = Convert.ToInt32(tempInput2);
                    var searchCustomer = from order in dbEntities.Orders.Where(order => order.CustomerId.Equals(tempInput3)).ToArray()
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
                    if (searchCustomer != null)  //found
                    {
                        //dataGridViewOrder.DataSource = new List<Order> { order };
                        dataGridViewOrder.DataSource = searchCustomer.ToList();



                    }
                    else
                    {
                        MessageBox.Show("Customer ID does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                        dataGridViewOrder.DataSource = null;

                    }
                    //List<Order> listOrders = new List<Order>();
                    //Order order1 = new Order();
                    //listOrders = dbEntities.Orders.< tempInput2 >;
                    //if (listBook == null)
                    //{
                    //    MessageBox.Show("Some error occured! Database is empty", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //}
                    //else
                    //{
                    //    //Book book = new Book();
                    //    bool test = true;

                    //    foreach (Book aBook in listBook)
                    //    {
                    //        if (aBook.Title.ToLower() == tempInput2.ToLower().ToString())
                    //        {
                    //            book.ISBN = aBook.ISBN;
                    //            book.Title = aBook.Title;
                    //            book.UnitPrice = aBook.UnitPrice;
                    //            book.QOH = aBook.QOH;
                    //            book.CategoryId = aBook.CategoryId;
                    //            book.PublisherId = aBook.PublisherId;

                    //            if (book == null)
                    //            {
                    //                MessageBox.Show("Some error occurred", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //            }
                    //            else
                    //            {
                    //                dataGridViewCustomers.DataSource = new List<Book> { book };
                    //                test = false;
                    //            }
                    //        }

                    //    }
                    //    if (test)
                    //    {
                    //        MessageBox.Show("Book does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //        textBoxInput.Clear();
                    //        textBoxInput.Focus();
                    //        dataGridViewCustomers.DataSource = null;
                    //    }
                    //}
                    break;
            }
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int indexSelected = comboBox.SelectedIndex;
            switch (indexSelected)
            {
                case 0:
                    labelDisplay.Text = "Please enter the Order ID";
                    textBoxInput.Clear();
                    textBoxInput.Focus();
                    break;
                case 1:
                    labelDisplay.Text = "Please enter the Customer ID";
                    textBoxInput.Clear();
                    textBoxInput.Focus();
                    break;

                default:
                    break;
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            DialogResult answer = MessageBox.Show("Would you like to exit the application?", "Exit Window", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (answer == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void buttonListCustomers_Click(object sender, EventArgs e)
        {
            var listCustomer = from customer in dbEntities.Customers
                             select new
                             {
                                customer.CustomerId,
                                customer.CustomerName,
                                customer.StreetName,
                                customer.City,
                                customer.Province,
                                customer.PostalCode,
                                customer.PhoneNumber,
                                customer.FaxNumber,
                                customer.Email,
                                customer.CreditLimit

                             };
            dataGridViewOrder.DataSource = listCustomer.ToList();
        }

        private void buttonListEmployees_Click(object sender, EventArgs e)
        {
            var listEmployees = from employee in dbEntities.Employees
                               select new
                               {
                                    employee.EmployeeId,
                                    employee.FirstName,
                                    employee.LastName,
                                    employee.PhoneNumber,
                                    employee.Email,
                                    employee.JobId

                               };
            dataGridViewOrder.DataSource = listEmployees.ToList();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
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
            string tempOrderStatus = comboBoxOrderStatus.SelectedItem.ToString();

            string tempCustomerId = textBoxCustomerId.Text.Trim();
            if (!(Validator.isValidTwoDigitId(tempCustomerId)))
            {
                MessageBox.Show("Customer ID must be 2-digit number", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxOrderId.Clear();
                textBoxOrderId.Focus();
                return;
            }
            string tempEmployeeId = textBoxEmployeeId.Text.Trim();
            if (!(Validator.isValidId(tempEmployeeId)))
            {
                MessageBox.Show("Employee ID must be 4-digit number", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxOrderId.Clear();
                textBoxOrderId.Focus();
                return;
            }

            //Special Validation with dates
            DateTime tempOrderDate = Convert.ToDateTime(dateTimePickerOrderDate.Value);
            if (!(dateTimePickerOrderDate.Checked))//Not checked
            {
                MessageBox.Show("Please, select an Order Date", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DateTime tempShippingDate = Convert.ToDateTime(dateTimePickerShippingDate.Value);
            if (!(dateTimePickerShippingDate.Checked))//Not checked
            {
                MessageBox.Show("Please, select a Shipping Date", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (DateTime.Compare(tempOrderDate, tempShippingDate) > 0)
            {
                MessageBox.Show("Shipping Date can not be before Order date", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;


            }
            DateTime tempDeliveringDate = Convert.ToDateTime(dateTimePickerDeliveringDate.Value);
            if (!(dateTimePickerDeliveringDate.Checked))//Not checked
            {
                MessageBox.Show("Please, select a Delivering Date", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (DateTime.Compare(tempOrderDate, tempDeliveringDate) > 0)
            {
                MessageBox.Show("Delivering date can not be before Order date", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //MessageBox.Show(tempOrderDate.ToShortDateString() + "\n" + tempShippingDate.ToShortDateString());
                return;


            }
            if (DateTime.Compare(tempShippingDate, tempDeliveringDate) > 0)
            {
                MessageBox.Show("Delivering date can not be before Shipping date", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;


            }

            //operation
            order.OrderId = Convert.ToInt32(tempOrderId);
            order.OrderDate = Convert.ToDateTime(tempOrderDate.ToShortDateString());
            order.ShippingDate = Convert.ToDateTime(tempShippingDate.ToShortDateString());
            order.DeliveringDate = Convert.ToDateTime(tempDeliveringDate.ToShortDateString());
            order.OrderStatus = tempOrderStatus;
            order.CustomerId = Convert.ToInt32(tempCustomerId);
            order.EmployeeId = Convert.ToInt32(tempEmployeeId);
            //Order aOrder = new Order();
            //aOrder = dbEntities.Orders.Find(Convert.ToInt32(order.OrderId));
            int tempOrderId2 = Convert.ToInt32(tempOrderId);
            //var searchOrder2 = from aOrder in dbEntities.Orders.Where(aOrder => aOrder.OrderId.Equals(tempOrderId2))
            //                  select new
            //                  {
            //                      aOrder.OrderId,
            //                      aOrder.OrderDate,
            //                      aOrder.DeliveringDate,
            //                      aOrder.ShippingDate,
            //                      aOrder.OrderStatus,
            //                      aOrder.CustomerId,
            //                      aOrder.EmployeeId
            //                  };

            var searchOrder2 = dbEntities.Orders.Any(aOrder => aOrder.OrderId.Equals(tempOrderId2));
            System.Diagnostics.Debug.Write(searchOrder2);
            if (searchOrder2)
            {
                //dbEntities.Orders.Add(order);
                dbEntities.Orders.Attach(order);
                dbEntities.Entry(order).State = System.Data.Entity.EntityState.Modified;
                dbEntities.SaveChanges();
                MessageBox.Show("Order Information was saved successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBoxOrderId.Clear();
                textBoxCustomerId.Clear();
                textBoxEmployeeId.Clear();

            }
            else
            {
                MessageBox.Show("This Order Id is already on the database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxOrderId.Clear();

            }
        }
    }
}
