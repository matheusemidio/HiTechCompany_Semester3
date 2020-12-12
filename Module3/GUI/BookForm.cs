using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Module1.Validation;
using Module3.DAL;
using Module3.Business;
using System.Data.SqlClient;


namespace Module3.GUI
{

    public partial class BookForm : Form
    {
        SqlDataAdapter da;
        DataSet dsHiTech;
        DataTable dtBooks;
        SqlCommandBuilder sqlBuilder;
        Book aBook = new Book();

        public BookForm()
        {
            InitializeComponent();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            //Validation
            string tempIsbn = textBoxISBN.Text.Trim();
            if (!(Validator.isValidISBN(tempIsbn)))
            {
                MessageBox.Show("ISBN must be 13-digit number", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxISBN.Clear();
                textBoxISBN.Focus();
                return;
            }
            string tempQOH = textBoxQOH.Text.Trim();
            if (!(Validator.isValidUnitPrice(tempQOH)))
            {
                MessageBox.Show("Invalid QOH", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxQOH.Clear();
                textBoxQOH.Focus();
                return;
            }

            string tempTitle = textBoxTitle.Text.Trim();
            if (!(Validator.isValidStreetName(tempTitle)))
            {
                MessageBox.Show("Invalid Title", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxTitle.Clear();
                textBoxTitle.Focus();
                return;
            }
            string tempUnitPrice = textBoxUnitPrice.Text.Trim();
            if (!(Validator.isValidUnitPrice(tempUnitPrice)))
            {
                MessageBox.Show("Invalid Unit Price", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxUnitPrice.Clear();
                textBoxUnitPrice.Focus();
                return;
            }

            //string tempCategoryId = textBoxCategoryID.Text.Trim();
            string tempCategoryId = comboBoxCategory.SelectedValue.ToString();
            //string tempPublisherId = textBoxPublisherID.Text.Trim();
            string tempPublisherId = comboBoxPublisher.SelectedValue.ToString();
            //Operation
            DataRow dr;
            dr = dtBooks.Rows.Find(tempIsbn);

            if (dr != null) //Exists
            {
                MessageBox.Show("ISBN is already on the Database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxISBN.Clear();
                textBoxISBN.Focus();

            }
            else //Does not exists
            {
                dr = dtBooks.NewRow();
                dr["ISBN"] = textBoxISBN.Text.Trim();
                dr["Title"] = textBoxTitle.Text.Trim();
                dr["UnitPrice"] = textBoxUnitPrice.Text.Trim();
                dr["QOH"] = textBoxQOH.Text.Trim();
                //dr["CategoryId"] = textBoxCategoryID.Text.Trim();
                dr["CategoryId"] = comboBoxCategory.SelectedValue.ToString();
                //dr["PublisherId"] = textBoxPublisherID.Text.Trim();
                dr["PublisherId"] = comboBoxPublisher.SelectedValue.ToString();


                dtBooks.Rows.Add(dr);
                da.Update(dsHiTech, "Books");

                MessageBox.Show("Book information was saved successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BookForm_Load(object sender, EventArgs e)
        {
            dsHiTech = new DataSet("HiTechDS");
            dtBooks = new DataTable("Books");
            dsHiTech.Tables.Add(dtBooks);

            dtBooks.Columns.Add("ISBN", typeof(String));
            dtBooks.Columns.Add("Title", typeof(String));
            dtBooks.Columns.Add("UnitPrice", typeof(Double));
            dtBooks.Columns.Add("QOH", typeof(Int32));
            dtBooks.Columns.Add("CategoryId", typeof(Int32));
            dtBooks.Columns.Add("PublisherId", typeof(Int32));

            dtBooks.PrimaryKey = new DataColumn[] { dtBooks.Columns["ISBN"] };

            da = new SqlDataAdapter("SELECT * FROM Books", UtilityDB.ConnectDB());
            sqlBuilder = new SqlCommandBuilder(da);
            da.Fill(dsHiTech.Tables["Books"]);



            //Populating ComboBoxes
            List<Category> listCategory = new List<Category>();
            Category category = new Category();
            listCategory = category.CategoryList();
            foreach (Category aCategory in listCategory)
            {
                comboBoxCategory.DataSource = listCategory;
                comboBoxCategory.DisplayMember = "CategoryName";
                comboBoxCategory.ValueMember = "CategoryId";
            }

            List<Publisher> listPublisher = new List<Publisher>();
            Publisher publisher = new Publisher();
            listPublisher = publisher.PublisherList();

            foreach (Publisher aPublisher in listPublisher)
            {
                comboBoxPublisher.DataSource = listPublisher;
                comboBoxPublisher.DisplayMember = "PublisherName";
                comboBoxPublisher.ValueMember = "PublisherId";
            }

        }

        private void buttonListBooks_Click(object sender, EventArgs e)
        {
            List<Book> listBook = new List<Book>();
            Book customer = new Book();
            listBook = customer.BookList();
            if (listBook == null)
            {
                MessageBox.Show("Some error occured! Database is empty", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                dataGridViewCustomers.DataSource = listBook;
            }
        }

        private void buttonListPublishers_Click(object sender, EventArgs e)
        {
            List<Publisher> listPublisher = new List<Publisher>();
            Publisher publisher = new Publisher();
            listPublisher = publisher.PublisherList();
            if (listPublisher == null)
            {
                MessageBox.Show("Some error occured! Database is empty", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                dataGridViewCustomers.DataSource = listPublisher;
            }
        }

        private void buttonListCategories_Click(object sender, EventArgs e)
        {
            List<Category> listCategory = new List<Category>();
            Category category = new Category();
            listCategory = category.CategoryList();
            if (listCategory == null)
            {
                MessageBox.Show("Some error occured! Database is empty", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                dataGridViewCustomers.DataSource = listCategory;
            }
        }

        private void comboBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {


            //string selected = comboBoxCategory.GetItemText(comboBoxCategory.SelectedItem);
            //MessageBox.Show(selected);
            //MessageBox.Show(comboBoxCategory.SelectedValue.ToString());


        }

        private void comboBoxPublisher_SelectedIndexChanged(object sender, EventArgs e)
        {

            //string selected = comboBoxPublisher.GetItemText(comboBoxPublisher.SelectedItem);
            //MessageBox.Show(selected);
            //MessageBox.Show(comboBoxPublisher.SelectedValue.ToString());

        }

        private void booksToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            BookForm bookForm = new BookForm();
            this.Hide();
            bookForm.ShowDialog();
        }

        private void categoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CategoriesForm categoriesForm = new CategoriesForm();
            this.Hide();
            categoriesForm.ShowDialog();
        }

        private void publisherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PublishersForm publishersForm = new PublishersForm();
            this.Hide();
            publishersForm.ShowDialog();
        }

        private void authorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AuthorForm authorForm = new AuthorForm();
            this.Hide();
            authorForm.ShowDialog();
        }

        private void authorBooksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AuthorBooksForm authorBooksForm = new AuthorBooksForm();
            this.Hide();
            authorBooksForm.ShowDialog();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            //Validation
            string tempIsbn = textBoxISBN.Text.Trim();
            if (!(Validator.isValidISBN(tempIsbn)))
            {
                MessageBox.Show("ISBN must be 13-digit number", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxISBN.Clear();
                textBoxISBN.Focus();
                return;
            }
            string tempQOH = textBoxQOH.Text.Trim();
            if (!(Validator.isValidUnitPrice(tempQOH)))
            {
                MessageBox.Show("Invalid QOH", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxQOH.Clear();
                textBoxQOH.Focus();
                return;
            }

            string tempTitle = textBoxTitle.Text.Trim();
            if (!(Validator.isValidStreetName(tempTitle)))
            {
                MessageBox.Show("Invalid Title", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxTitle.Clear();
                textBoxTitle.Focus();
                return;
            }
            string tempUnitPrice = textBoxUnitPrice.Text.Trim();
            if (!(Validator.isValidUnitPrice(tempUnitPrice)))
            {
                MessageBox.Show("Invalid Unit Price", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxUnitPrice.Clear();
                textBoxUnitPrice.Focus();
                return;
            }

            //string tempCategoryId = textBoxCategoryID.Text.Trim();
            string tempCategoryId = comboBoxCategory.SelectedValue.ToString();
            //string tempPublisherId = textBoxPublisherID.Text.Trim();
            string tempPublisherId = comboBoxPublisher.SelectedValue.ToString();


            //Operation
            DataRow dr = dtBooks.NewRow(); ;
            dr = dtBooks.Rows.Find(tempIsbn);

            if (dr != null) //Exists
            {
                if (Convert.ToInt64(dr["ISBN"]) == Convert.ToInt64(textBoxISBN.Text))
                {

                    dr["ISBN"] = textBoxISBN.Text.Trim();
                    dr["Title"] = textBoxTitle.Text.Trim();
                    dr["UnitPrice"] = textBoxUnitPrice.Text.Trim();
                    dr["QOH"] = textBoxQOH.Text.Trim();
                    //dr["CategoryId"] = textBoxCategoryID.Text.Trim();
                    dr["CategoryId"] = comboBoxCategory.SelectedValue.ToString();
                    //dr["PublisherId"] = textBoxPublisherID.Text.Trim();
                    dr["PublisherId"] = comboBoxPublisher.SelectedValue.ToString();
                    //dtBooks.Rows.Add(dr);
                    da.Update(dsHiTech, "Books");
                    MessageBox.Show("Book information was updated successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            else //Does not exists
            {

                MessageBox.Show("Customer is not on the Database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxISBN.Clear();
                textBoxISBN.Focus();
            }
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int indexSelected = comboBox.SelectedIndex;
            switch (indexSelected)
            {
                case 0:
                    labelDisplay.Text = "Please enter the ISBN";
                    textBoxInput.Clear();
                    textBoxInput.Focus();
                    break;
                case 1:
                    labelDisplay.Text = "Please enter the Book Title";
                    textBoxInput.Clear();
                    textBoxInput.Focus();
                    break;

                default:
                    break;
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
                    if (!(Validator.isValidISBN(tempInput)))
                    {
                        MessageBox.Show("ISBN must be 13-digit number", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                        return;
                    }

                    DataRow dr = dtBooks.Rows.Find(tempInput);
                    if (dr != null)  //found
                    {
                        //dataGridViewCustomers.Rows.Clear();
                        //dataGridViewCustomers.Refresh();
                        Book book2 = new Book();
                        book2.ISBN = dr["ISBN"].ToString();
                        book2.Title = dr["Title"].ToString();
                        book2.UnitPrice = Convert.ToDouble(dr["UnitPrice"]);
                        book2.QOH = Convert.ToInt32(dr["QOH"]);
                        book2.CategoryId = Convert.ToInt32(dr["CategoryId"]);
                        book2.PublisherId = Convert.ToInt32(dr["PublisherId"]);

                        if (book2 == null)
                        {
                            MessageBox.Show("Some error occurred", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            dataGridViewCustomers.DataSource = new List<Book> { book2 };
                        }
                    }
                    else
                    {
                        MessageBox.Show("Customer does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                        dataGridViewCustomers.DataSource = null;

                    }

                    break;
                case 1:
                    //Serch by Customer Name

                    string tempInput2 = textBoxInput.Text.Trim();
                    if (!(Validator.isValidStreetName(tempInput2)))
                    {
                        MessageBox.Show("Invalid Book Name", "Invalid Customer Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                        return;
                    }
                    List<Book> listBook = new List<Book>();
                    Book book = new Book();
                    listBook = book.BookList();
                    if (listBook == null)
                    {
                        MessageBox.Show("Some error occured! Database is empty", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        //Book book = new Book();
                        bool test = true;

                        foreach (Book aBook in listBook)
                        {
                            if (aBook.Title.ToLower() == tempInput2.ToLower().ToString())
                            {
                                book.ISBN = aBook.ISBN;
                                book.Title = aBook.Title;
                                book.UnitPrice = aBook.UnitPrice;
                                book.QOH = aBook.QOH;
                                book.CategoryId = aBook.CategoryId;
                                book.PublisherId = aBook.PublisherId;

                                if (book == null)
                                {
                                    MessageBox.Show("Some error occurred", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    dataGridViewCustomers.DataSource = new List<Book> { book };
                                    test = false;
                                }
                            }

                        }
                        if (test)
                        {
                            MessageBox.Show("Book does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            textBoxInput.Clear();
                            textBoxInput.Focus();
                            dataGridViewCustomers.DataSource = null;
                        }
                    }
                    break;
            }
        }

        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            DialogResult answer = MessageBox.Show("Would you like to exit the application?", "Exit Window", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (answer == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            ////Validation
            //string tempIsbn = textBoxISBN.Text.Trim();
            //if (!(Validator.isValidISBN(tempIsbn)))
            //{
            //    MessageBox.Show("ISBN must be 13-digit number", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    textBoxISBN.Clear();
            //    textBoxISBN.Focus();
            //    return;
            //}
            //string tempQOH = textBoxQOH.Text.Trim();
            //if (!(Validator.isValidUnitPrice(tempQOH)))
            //{
            //    MessageBox.Show("Invalid QOH", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    textBoxQOH.Clear();
            //    textBoxQOH.Focus();
            //    return;
            //}

            //string tempTitle = textBoxTitle.Text.Trim();
            //if (!(Validator.isValidStreetName(tempTitle)))
            //{
            //    MessageBox.Show("Invalid Title", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    textBoxTitle.Clear();
            //    textBoxTitle.Focus();
            //    return;
            //}
            //string tempUnitPrice = textBoxUnitPrice.Text.Trim();
            //if (!(Validator.isValidUnitPrice(tempUnitPrice)))
            //{
            //    MessageBox.Show("Invalid Unit Price", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    textBoxUnitPrice.Clear();
            //    textBoxUnitPrice.Focus();
            //    return;
            //}

            ////string tempCategoryId = textBoxCategoryID.Text.Trim();
            //string tempCategoryId = comboBoxCategory.SelectedValue.ToString();
            ////string tempPublisherId = textBoxPublisherID.Text.Trim();
            //string tempPublisherId = comboBoxPublisher.SelectedValue.ToString();


            ////Operation
            //DataRow dr = dtBooks.NewRow(); 
            //dr = dtBooks.Rows.Find(tempIsbn);

            //if (dr != null) //Exists
            //{
            //    if (Convert.ToInt64(dr["ISBN"]) == Convert.ToInt64(textBoxISBN.Text))
            //    {

            //        dr.Delete();
            //        da.Update(dsHiTech, "Books");
            //        MessageBox.Show(dr.RowState.ToString(), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }

            //}
            //else //Does not exists
            //{

            //    MessageBox.Show("Book is not on the Database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    textBoxISBN.Clear();
            //    textBoxISBN.Focus();
            //}
        }
    }
}
