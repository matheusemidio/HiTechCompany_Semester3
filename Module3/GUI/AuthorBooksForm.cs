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
    public partial class AuthorBooksForm : Form
    {
        SqlDataAdapter da;
        DataSet dsHiTech;
        DataTable dtAuthorBook;
        SqlCommandBuilder sqlBuilder;
        AuthorBook aAuthorBook = new AuthorBook();
        public AuthorBooksForm()
        {
            InitializeComponent();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            //Validation
            string tempISBN = textBoxISBN.Text.Trim();
            if (!(Validator.isValidISBN(tempISBN)))
            {
                MessageBox.Show("ISBN must be 13-digit number", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxISBN.Clear();
                textBoxISBN.Focus();
                return;
            }

            string tempAuthorId = textBoxAuthorId.Text.Trim();
            if (!(Validator.isValidTwoDigitId(tempAuthorId)))
            {
                MessageBox.Show("Author ID must be 2-digit number", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxAuthorId.Clear();
                textBoxAuthorId.Focus();
                return;
            }

            string tempYearPublished = textBoxYearPublished.Text.Trim();
            if (!(Validator.isValidId(tempYearPublished)))
            {
                MessageBox.Show("Year must be 4-digit number", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxYearPublished.Clear();
                textBoxYearPublished.Focus();
                return;
            }
            string tempEdition = textBoxEdition.Text.Trim();
            if (!(Validator.isValidCreditLimit(tempEdition)))
            {
                MessageBox.Show("Edition must be 2-digit number", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxEdition.Clear();
                textBoxEdition.Focus();
                return;
            }



            //Operation
            DataRow dr;
            AuthorBook aAuthorBook = new AuthorBook();
            DataRow[] rows = dtAuthorBook.Select();
            List<AuthorBook> listAuthorBooks = aAuthorBook.AuthorBookList();
            List<AuthorBook> listAuthorBooks1 = new List<AuthorBook>();
            List<AuthorBook> listAuthorBooks2 = new List<AuthorBook>();
            List<AuthorBook> listAuthorBooks3 = new List<AuthorBook>();

            foreach(AuthorBook authorBook in listAuthorBooks)
            {

                if (authorBook.ISBN.ToString().ToLower() == tempISBN.ToLower())  //found
                {
                    aAuthorBook = new AuthorBook();
                    aAuthorBook.ISBN = authorBook.ISBN;
                    aAuthorBook.AuthorId = Convert.ToInt32(authorBook.AuthorId);
                    aAuthorBook.YearPublished = Convert.ToInt32(authorBook.YearPublished);
                    aAuthorBook.Edition = Convert.ToInt32(authorBook.Edition);
                    listAuthorBooks1.Add(aAuthorBook);

                }
                if (authorBook.AuthorId.ToString().ToLower() == tempAuthorId.ToLower())  //found
                {
                    aAuthorBook = new AuthorBook();
                    aAuthorBook.ISBN = authorBook.ISBN.ToString();
                    aAuthorBook.AuthorId = Convert.ToInt32(authorBook.AuthorId);
                    aAuthorBook.YearPublished = Convert.ToInt32(authorBook.YearPublished);
                    aAuthorBook.Edition = Convert.ToInt32(authorBook.Edition);
                    listAuthorBooks2.Add(aAuthorBook);

                }
                if ((authorBook.ISBN.ToString().ToLower() == tempISBN.ToLower()) && (authorBook.AuthorId.ToString().ToLower() == tempAuthorId.ToLower()))
                {
                    aAuthorBook = new AuthorBook();
                    aAuthorBook.ISBN = authorBook.ISBN;
                    aAuthorBook.AuthorId = Convert.ToInt32(authorBook.AuthorId);
                    aAuthorBook.YearPublished = Convert.ToInt32(authorBook.YearPublished);
                    aAuthorBook.Edition = Convert.ToInt32(authorBook.Edition);
                    listAuthorBooks3.Add(aAuthorBook);
                }

            }



            //for (int i = 0; i < rows.Length; i++)
            //{
            //    if (rows[i]["ISBN"].ToString().ToLower() == tempISBN.ToLower())  //found
            //    {
            //        aAuthorBook = new AuthorBook();
            //        aAuthorBook.ISBN = rows[i]["ISBN"].ToString();
            //        aAuthorBook.AuthorId = Convert.ToInt32(rows[i]["AuthorId"]);
            //        aAuthorBook.YearPublished = Convert.ToInt32(rows[i]["YearPublished"]);
            //        aAuthorBook.Edition = Convert.ToInt32(rows[i]["Edition"]);
            //        listAuthorBooks.Add(aAuthorBook);

            //    }
            //    if (rows[i]["AuthorId"].ToString().ToLower() == tempAuthorId.ToLower())  //found
            //    {
            //        aAuthorBook = new AuthorBook();
            //        aAuthorBook.ISBN = rows[i]["ISBN"].ToString();
            //        aAuthorBook.AuthorId = Convert.ToInt32(rows[i]["AuthorId"]);
            //        aAuthorBook.YearPublished = Convert.ToInt32(rows[i]["YearPublished"]);
            //        aAuthorBook.Edition = Convert.ToInt32(rows[i]["Edition"]);
            //        listAuthorBooks2.Add(aAuthorBook);

            //    }
            //    if ((rows[i]["ISBN"].ToString().ToLower() == tempISBN.ToLower()) && (rows[i]["AuthorId"].ToString().ToLower() == tempAuthorId.ToLower()))
            //    {
            //        aAuthorBook = new AuthorBook();
            //        aAuthorBook.ISBN = rows[i]["ISBN"].ToString();
            //        aAuthorBook.AuthorId = Convert.ToInt32(rows[i]["AuthorId"]);
            //        aAuthorBook.YearPublished = Convert.ToInt32(rows[i]["YearPublished"]);
            //        aAuthorBook.Edition = Convert.ToInt32(rows[i]["Edition"]);
            //        listAuthorBooks3.Add(aAuthorBook);
            //    }

            //}


            if (listAuthorBooks1.Count == 0)//Does not Exists
            {
                MessageBox.Show("ISBN ID does not exist on the Database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxISBN.Clear();
                textBoxISBN.Focus();
            }


            else if (listAuthorBooks2.Count == 0) //Does not Exists
            {
                MessageBox.Show("Author ID does not exist on the Database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxAuthorId.Clear();
                textBoxAuthorId.Focus();
            }
            else if(listAuthorBooks3.Count != 0)   //This connection already exists
            {
                MessageBox.Show("This ISBN is already linked to this Author ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxISBN.Clear();
                textBoxAuthorId.Clear();
                
            }
            else //Exists and this connection does not exists
            {
                dr = dtAuthorBook.NewRow();
                dr["ISBN"] = textBoxISBN.Text.Trim();
                dr["AuthorId"] = textBoxAuthorId.Text.Trim();
                dr["YearPublished"] = textBoxYearPublished.Text.Trim();
                dr["Edition"] = textBoxEdition.Text.Trim();



                dtAuthorBook.Rows.Add(dr);
                da.Update(dsHiTech, "AuthorBooks");

                MessageBox.Show("AuthorBook information was saved successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBoxISBN.Clear();
                textBoxAuthorId.Clear();
                textBoxYearPublished.Clear();
                textBoxEdition.Clear();
            }
        }

        private void AuthorBooksForm_Load(object sender, EventArgs e)
        {
            dsHiTech = new DataSet("HiTechDS");
            dtAuthorBook = new DataTable("AuthorBooks");
            dsHiTech.Tables.Add(dtAuthorBook);

            dtAuthorBook.Columns.Add("ISBN", typeof(String));
            dtAuthorBook.Columns.Add("AuthorId", typeof(Int32));
            dtAuthorBook.Columns.Add("YearPublished", typeof(Int32));
            dtAuthorBook.Columns.Add("Edition", typeof(Int32));
            

            dtAuthorBook.PrimaryKey = new DataColumn[] { dtAuthorBook.Columns["ISBN"], dtAuthorBook.Columns["AuthorId"] };

            da = new SqlDataAdapter("SELECT * FROM AuthorBooks", UtilityDB.ConnectDB());
            sqlBuilder = new SqlCommandBuilder(da);
            da.Fill(dsHiTech.Tables["AuthorBooks"]);

        }


        private void booksToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            BookForm bookForm = new BookForm();
            this.Hide();
            bookForm.ShowDialog();
        }

        private void categoriesToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            CategoriesForm categoriesForm = new CategoriesForm();
            this.Hide();
            categoriesForm.ShowDialog();
        }

        private void publisherToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            PublishersForm publishersForm = new PublishersForm();
            this.Hide();
            publishersForm.ShowDialog();
        }

        private void authorsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            AuthorForm authorForm = new AuthorForm();
            this.Hide();
            authorForm.ShowDialog();
        }

        private void authorBooksToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            AuthorBooksForm authorBooksForm = new AuthorBooksForm();
            this.Hide();
            authorBooksForm.ShowDialog();
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
                    labelDisplay.Text = "Please enter the Author ID";
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
                    string tempInput = textBoxInput.Text.Trim();
                    if (!(Validator.isValidISBN(tempInput)))
                    {
                        MessageBox.Show("ISBN must be a 13-digit number", "Invalid Customer ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                        return;
                    }


                    DataRow[] rows = dtAuthorBook.Select();
                    List<AuthorBook> listAuthorBooks = new List<AuthorBook>();


                    for (int i=0; i< rows.Length; i++)
                    {
                        
                        if (rows[i]["ISBN"].ToString().ToLower() == tempInput.ToLower())  //found
                        {
                            aAuthorBook = new AuthorBook();
                            aAuthorBook.ISBN = rows[i]["ISBN"].ToString();
                            aAuthorBook.AuthorId = Convert.ToInt32(rows[i]["AuthorId"]);
                            aAuthorBook.YearPublished = Convert.ToInt32(rows[i]["YearPublished"]);
                            aAuthorBook.Edition = Convert.ToInt32(rows[i]["Edition"]);
                            listAuthorBooks.Add(aAuthorBook);
                            

                        }
                        
                    }
                    if (listAuthorBooks.Count == 0)
                    {
                        MessageBox.Show("Book does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                        dataGridViewCustomers.DataSource = null;

                    }
                    else
                    {
                        dataGridViewCustomers.DataSource = listAuthorBooks;

                    }

                    break;
                case 1:
                    string tempInput2 = textBoxInput.Text.Trim();
                    if (!(Validator.isValidTwoDigitId(tempInput2)))
                    {
                        MessageBox.Show("Author ID must be a 2-digit number", "Invalid Customer ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                        return;
                    }


                    DataRow[] rows2 = dtAuthorBook.Select();
                    List<AuthorBook> listAuthorBooks2 = new List<AuthorBook>();

                    for (int i = 0; i < rows2.Length; i++)
                    {
                        //DataRow dr = dtAuthorBook.Rows.Find(tempInput);

                        if (rows2[i]["AuthorId"].ToString().ToLower() == tempInput2.ToLower())  //found
                        {
                            aAuthorBook = new AuthorBook();
                            //dataGridViewCustomers.Rows.Clear();
                            //dataGridViewCustomers.Refresh();
                            //AuthorBook book2 = new Book();
                            aAuthorBook.ISBN = rows2[i]["ISBN"].ToString();
                            aAuthorBook.AuthorId = Convert.ToInt32(rows2[i]["AuthorId"]);
                            aAuthorBook.YearPublished = Convert.ToInt32(rows2[i]["YearPublished"]);
                            aAuthorBook.Edition = Convert.ToInt32(rows2[i]["Edition"]);
                            listAuthorBooks2.Add(aAuthorBook);

                            //if (aAuthorBook == null)
                            //{
                            //    MessageBox.Show("Some error occurred", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //}
                            //else
                            //{
                            //    dataGridViewCustomers.DataSource = new List<AuthorBook> { aAuthorBook };
                            //}


                        }

                    }
                    if (listAuthorBooks2.Count == 0)
                    {
                        MessageBox.Show("Author does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                        dataGridViewCustomers.DataSource = null;


                    }
                    else
                    {
                        dataGridViewCustomers.DataSource = listAuthorBooks2;

                    }

                    break;
                
                    break;
            }
        }

        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void buttonListAuthorBooks_Click(object sender, EventArgs e)
        {
            List<AuthorBook> listAuthorBooks = new List<AuthorBook>();
            AuthorBook authorBook = new AuthorBook();
            listAuthorBooks = authorBook.AuthorBookList();
            if (listAuthorBooks == null)
            {
                MessageBox.Show("Some error occured! Database is empty", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                dataGridViewCustomers.DataSource = listAuthorBooks;
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

        private void buttonUpdate_Click(object sender, EventArgs e)
        {


            //Validation
            string tempISBN = textBoxISBN.Text.Trim();
            if (!(Validator.isValidISBN(tempISBN)))
            {
                MessageBox.Show("ISBN must be 13-digit number", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxISBN.Clear();
                textBoxISBN.Focus();
                return;
            }

            string tempAuthorId = textBoxAuthorId.Text.Trim();
            if (!(Validator.isValidTwoDigitId(tempAuthorId)))
            {
                MessageBox.Show("Author ID must be 2-digit number", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxAuthorId.Clear();
                textBoxAuthorId.Focus();
                return;
            }

            string tempYearPublished = textBoxYearPublished.Text.Trim();
            if (!(Validator.isValidId(tempYearPublished)))
            {
                MessageBox.Show("Year must be 4-digit number", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxYearPublished.Clear();
                textBoxYearPublished.Focus();
                return;
            }
            string tempEdition = textBoxEdition.Text.Trim();
            if (!(Validator.isValidCreditLimit(tempEdition)))
            {
                MessageBox.Show("Edition must be 2-digit number", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxEdition.Clear();
                textBoxEdition.Focus();
                return;
            }

            //Operation
            AuthorBook aAuthorBook = new AuthorBook();
            DataRow dr = dtAuthorBook.NewRow();

            //DataRow[] rows = dtAuthorBook.Select();
            List<AuthorBook> listAuthorBooks = aAuthorBook.AuthorBookList();
            List<AuthorBook> listAuthorBooks1 = new List<AuthorBook>();
            List<AuthorBook> listAuthorBooks2 = new List<AuthorBook>();
            List<AuthorBook> listAuthorBooks3 = new List<AuthorBook>();

            foreach (AuthorBook authorBook in listAuthorBooks)
            {

                if (authorBook.ISBN.ToString().ToLower() == tempISBN.ToLower())  //found
                {
                    aAuthorBook = new AuthorBook();
                    aAuthorBook.ISBN = authorBook.ISBN;
                    aAuthorBook.AuthorId = Convert.ToInt32(authorBook.AuthorId);
                    aAuthorBook.YearPublished = Convert.ToInt32(authorBook.YearPublished);
                    aAuthorBook.Edition = Convert.ToInt32(authorBook.Edition);
                    listAuthorBooks1.Add(aAuthorBook);

                }
                if (authorBook.AuthorId.ToString().ToLower() == tempAuthorId.ToLower())  //found
                {
                    aAuthorBook = new AuthorBook();
                    aAuthorBook.ISBN = authorBook.ISBN.ToString();
                    aAuthorBook.AuthorId = Convert.ToInt32(authorBook.AuthorId);
                    aAuthorBook.YearPublished = Convert.ToInt32(authorBook.YearPublished);
                    aAuthorBook.Edition = Convert.ToInt32(authorBook.Edition);
                    listAuthorBooks2.Add(aAuthorBook);

                }
                if ((authorBook.ISBN.ToString().ToLower() == tempISBN.ToLower()) && (authorBook.AuthorId.ToString().ToLower() == tempAuthorId.ToLower()))
                {
                    aAuthorBook = new AuthorBook();
                    aAuthorBook.ISBN = authorBook.ISBN;
                    aAuthorBook.AuthorId = Convert.ToInt32(authorBook.AuthorId);
                    aAuthorBook.YearPublished = Convert.ToInt32(authorBook.YearPublished);
                    aAuthorBook.Edition = Convert.ToInt32(authorBook.Edition);
                    listAuthorBooks3.Add(aAuthorBook);
                }

            }



            if (listAuthorBooks1.Count == 0)//Does not Exists
            {
                MessageBox.Show("ISBN ID does not exist on the Database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxISBN.Clear();
                textBoxISBN.Focus();
            }


            else if (listAuthorBooks2.Count == 0) //Does not Exists
            {
                MessageBox.Show("Author ID does not exist on the Database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxAuthorId.Clear();
                textBoxAuthorId.Focus();
            }
            else if (listAuthorBooks3.Count == 1)   //This connection already exists
            {
                dr = dtAuthorBook.NewRow();
                if ((listAuthorBooks3[0].ISBN.ToString().Trim() == tempISBN) && (listAuthorBooks3[0].AuthorId.ToString().Trim() == tempAuthorId)) {
                    dr["ISBN"] = tempISBN;
                    dr["AuthorId"] = tempAuthorId;
                    dr["YearPublished"] = tempYearPublished;
                    dr["Edition"] = tempEdition;
                    //dtAuthorBook.Rows.Add(dr);
                    //dtAuthorBook.AcceptChanges();

                    da.Update(dsHiTech, "AuthorBooks");

                    MessageBox.Show("AuthorBook information was saved successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show(dr.RowState.ToString(), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBoxISBN.Clear();
                    textBoxAuthorId.Clear();
                    textBoxYearPublished.Clear();
                    textBoxEdition.Clear();
                }
                else //Error
                {
                    MessageBox.Show("Some error occurred1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show(listAuthorBooks3[0].ISBN.ToString() + "\n" + listAuthorBooks3[0].AuthorId.ToString());
                }



            }
            else //Error
            {
                MessageBox.Show("Some error occurred2", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            string tempISBN = textBoxISBN.Text.Trim();
            if (!(Validator.isValidISBN(tempISBN)))
            {
                MessageBox.Show("ISBN must be 13-digit number", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxISBN.Clear();
                textBoxISBN.Focus();
                return;
            }

            string tempAuthorId = textBoxAuthorId.Text.Trim();
            if (!(Validator.isValidTwoDigitId(tempAuthorId)))
            {
                MessageBox.Show("Author ID must be 2-digit number", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxAuthorId.Clear();
                textBoxAuthorId.Focus();
                return;
            }
            DialogResult answer = MessageBox.Show("Are you sure you want to delete this AuthorBook?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if(DialogResult == answer)
            {
                MessageBox.Show("Deleted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonListAuthors_Click(object sender, EventArgs e)
        {
            List<Author> listAuthor = new List<Author>();
            Author author = new Author();
            listAuthor = author.AuthorList();
            if (listAuthor == null)
            {
                MessageBox.Show("Some error occured! Database is empty", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                dataGridViewCustomers.DataSource = listAuthor;
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
    }
}
