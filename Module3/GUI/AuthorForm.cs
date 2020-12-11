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
    public partial class AuthorForm : Form
    {
        SqlDataAdapter da;
        DataSet dsHiTech;
        DataTable dtAuthor;
        SqlCommandBuilder sqlBuilder;
        Author aAuthor = new Author();

        public AuthorForm()
        {
            InitializeComponent();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            //Validation
            string tempAuthorId = textBoxAuthorId.Text.Trim();
            string tempAuthorFirstName = textBoxFirstName.Text.Trim();
            string tempLastName = textBoxLastName.Text.Trim();
            string tempEmail = textBoxEmail.Text.Trim();

            //Operation
            DataRow dr;
            dr = dtAuthor.Rows.Find(tempAuthorId);

            if (dr != null) //Exists
            {
                MessageBox.Show("Author ID is already on the Database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxAuthorId.Clear();
                textBoxAuthorId.Focus();

            }
            else //Does not exists
            {
                dr = dtAuthor.NewRow();
                dr["AuthorId"] = textBoxAuthorId.Text.Trim();
                dr["FirstName"] = textBoxFirstName.Text.Trim();
                dr["LastName"] = textBoxLastName.Text.Trim();
                dr["Email"] = textBoxEmail.Text.Trim();



                dtAuthor.Rows.Add(dr);
                da.Update(dsHiTech, "Authors");

                MessageBox.Show("Author information was saved successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBoxAuthorId.Clear();
                textBoxFirstName.Clear();
                textBoxLastName.Clear();
                textBoxEmail.Clear();
            }
        }

        private void AuthorForm_Load(object sender, EventArgs e)
        {
            dsHiTech = new DataSet("HiTechDS");
            dtAuthor = new DataTable("Authors");
            dsHiTech.Tables.Add(dtAuthor);

            dtAuthor.Columns.Add("AuthorId", typeof(Int32));
            dtAuthor.Columns.Add("FirstName", typeof(String));
            dtAuthor.Columns.Add("LastName", typeof(String));
            dtAuthor.Columns.Add("Email", typeof(String));
            

            dtAuthor.PrimaryKey = new DataColumn[] { dtAuthor.Columns["AuthorId"] };

            da = new SqlDataAdapter("SELECT * FROM Authors", UtilityDB.ConnectDB());
            sqlBuilder = new SqlCommandBuilder(da);
            da.Fill(dsHiTech.Tables["Authors"]);

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
                    labelDisplay.Text = "Please enter the Author ID";
                    textBoxInput.Clear();
                    textBoxInput.Focus();
                    break;
                case 1:
                    labelDisplay.Text = "Please enter the Author Name";
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
                //Search By Author ID
                case 0:
                    string tempInput = textBoxInput.Text.Trim();
                    //if (!(Validator.isValidCustmerId(tempInput)))
                    //{
                    //    MessageBox.Show("ISBN must be a 13-digit number", "Invalid Customer ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    textBoxInput.Clear();
                    //    textBoxInput.Focus();
                    //    return;
                    //}
                    DataRow dr = dtAuthor.Rows.Find(tempInput);
                    if (dr != null)  //found
                    {
                        //dataGridViewCustomers.Rows.Clear();
                        //dataGridViewCustomers.Refresh();

                        aAuthor.AuthorId = Convert.ToInt32(dr["AuthorId"]);
                        aAuthor.FirstName = dr["FirstName"].ToString();
                        aAuthor.LastName = dr["LastName"].ToString();
                        aAuthor.Email = dr["Email"].ToString();


                        if (aAuthor == null)
                        {
                            MessageBox.Show("Some error occurred", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            dataGridViewCustomers.DataSource = new List<Author> { aAuthor };
                        }
                    }
                    else
                    {
                        MessageBox.Show("Author does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                        dataGridViewCustomers.DataSource = null;

                    }

                    break;
                case 1:
                    //Serch by Customer Name
                    string tempInput2 = textBoxInput.Text.Trim();
                    if (!(Validator.IsValidName(tempInput2)))
                    {
                        MessageBox.Show("Invalid Book Name", "Invalid Customer Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                        return;
                    }
                    List<Author> listAuthor = new List<Author>();
                    Author author = new Author();
                    listAuthor = aAuthor.AuthorList();
                    if (listAuthor == null)
                    {
                        MessageBox.Show("Some error occured! Database is empty", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        //Book book = new Book();
                        bool test = true;

                        foreach (Author aAuthor in listAuthor)
                        {
                            if (aAuthor.FirstName.ToLower() == tempInput2.ToLower().ToString())
                            {
                                author.AuthorId = aAuthor.AuthorId;
                                author.FirstName = aAuthor.FirstName;
                                author.LastName = aAuthor.LastName;
                                author.Email = aAuthor.Email;


                                if (aAuthor == null)
                                {
                                    MessageBox.Show("Some error occurred", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    dataGridViewCustomers.DataSource = new List<Author> { author };
                                    test = false;
                                }
                            }

                        }
                        if (test)
                        {
                            MessageBox.Show("Author does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            textBoxInput.Clear();
                            textBoxInput.Focus();
                            dataGridViewCustomers.DataSource = null;
                        }
                    }
                    break;
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
            string tempAuthorId = textBoxAuthorId.Text.Trim();
            string tempAuthorFirstName = textBoxFirstName.Text.Trim();
            string tempLastName = textBoxLastName.Text.Trim();
            string tempEmail = textBoxEmail.Text.Trim();

            //Operation
            DataRow dr = dtAuthor.NewRow();
            dr = dtAuthor.Rows.Find(tempAuthorId);

            if (dr != null) //Exists
            {
                if (Convert.ToInt32(dr["AuthorId"]) == Convert.ToInt32(tempAuthorId))
                {
                    //dr = dtAuthor.NewRow();
                    dr["AuthorId"] = textBoxAuthorId.Text.Trim();
                    dr["FirstName"] = textBoxFirstName.Text.Trim();
                    dr["LastName"] = textBoxLastName.Text.Trim();
                    dr["Email"] = textBoxEmail.Text.Trim();



                    //dtAuthor.Rows.Add(dr);
                    da.Update(dsHiTech, "Authors");

                    MessageBox.Show("Author information was updated successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBoxAuthorId.Clear();
                    textBoxFirstName.Clear();
                    textBoxLastName.Clear();
                    textBoxEmail.Clear();                  
              
                }


            }
            else //Does not exists
            {

                MessageBox.Show("Author ID is not on the Database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxAuthorId.Clear();
                textBoxAuthorId.Focus();
            }

            
        }
    }
}
