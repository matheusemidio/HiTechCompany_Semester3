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
            string tempAuthorId = textBoxAuthorId.Text.Trim();
            string tempYearPublished = textBoxYearPublished.Text.Trim();
            string tempEdition = textBoxEdition.Text.Trim();

            //Operation
            DataRow dr;
            
            DataRow[] rows = dtAuthorBook.Select();
            List<AuthorBook> listAuthorBooks = new List<AuthorBook>();
            List<AuthorBook> listAuthorBooks2 = new List<AuthorBook>();
            List<AuthorBook> listAuthorBooks3 = new List<AuthorBook>();



            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i]["ISBN"].ToString().ToLower() == tempISBN.ToLower())  //found
                {
                    aAuthorBook = new AuthorBook();
                    aAuthorBook.ISBN = rows[i]["ISBN"].ToString();
                    aAuthorBook.AuthorId = Convert.ToInt32(rows[i]["AuthorId"]);
                    aAuthorBook.YearPublished = Convert.ToInt32(rows[i]["YearPublished"]);
                    aAuthorBook.Edition = Convert.ToInt32(rows[i]["Edition"]);
                    listAuthorBooks.Add(aAuthorBook);

                }
                if (rows[i]["AuthorId"].ToString().ToLower() == tempAuthorId.ToLower())  //found
                {
                    aAuthorBook = new AuthorBook();
                    aAuthorBook.ISBN = rows[i]["ISBN"].ToString();
                    aAuthorBook.AuthorId = Convert.ToInt32(rows[i]["AuthorId"]);
                    aAuthorBook.YearPublished = Convert.ToInt32(rows[i]["YearPublished"]);
                    aAuthorBook.Edition = Convert.ToInt32(rows[i]["Edition"]);
                    listAuthorBooks2.Add(aAuthorBook);

                }
                if ((rows[i]["ISBN"].ToString().ToLower() == tempISBN.ToLower()) && (rows[i]["AuthorId"].ToString().ToLower() == tempAuthorId.ToLower()))
                {
                    aAuthorBook = new AuthorBook();
                    aAuthorBook.ISBN = rows[i]["ISBN"].ToString();
                    aAuthorBook.AuthorId = Convert.ToInt32(rows[i]["AuthorId"]);
                    aAuthorBook.YearPublished = Convert.ToInt32(rows[i]["YearPublished"]);
                    aAuthorBook.Edition = Convert.ToInt32(rows[i]["Edition"]);
                    listAuthorBooks3.Add(aAuthorBook);
                }

            }

            
            if (listAuthorBooks == null)//Does not Exists
            {
                MessageBox.Show("ISBN ID does not exist on the Database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxISBN.Clear();
                textBoxISBN.Focus();
            }


            else if (listAuthorBooks2 == null) //Does not Exists
            {
                MessageBox.Show("Author ID does not exist on the Database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxAuthorId.Clear();
                textBoxAuthorId.Focus();
            }
            else if(listAuthorBooks3 != null)   //This connection already exists
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
                    //if (!(Validator.isValidCustmerId(tempInput)))
                    //{
                    //    MessageBox.Show("ISBN must be a 13-digit number", "Invalid Customer ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    textBoxInput.Clear();
                    //    textBoxInput.Focus();
                    //    return;
                    //}
                    
                    
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
                    if (listAuthorBooks == null)
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
                    //if (!(Validator.isValidCustmerId(tempInput)))
                    //{
                    //    MessageBox.Show("ISBN must be a 13-digit number", "Invalid Customer ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    textBoxInput.Clear();
                    //    textBoxInput.Focus();
                    //    return;
                    //}


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
                    if (listAuthorBooks2 == null)
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

        }
    }
}
