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
    public partial class PublishersForm : Form
    {
        SqlDataAdapter da;
        DataSet dsHiTech;
        DataTable dtPublishers;
        SqlCommandBuilder sqlBuilder;
        Publisher aPublisher = new Publisher();
        public PublishersForm()
        {
            InitializeComponent();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            //Validation

            string tempPublisherId = textBoxPublisherId.Text.Trim();
            if (!(Validator.isValidTwoDigitId(tempPublisherId)))
            {
                MessageBox.Show("Publisher ID must be 2-digit number", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxPublisherId.Clear();
                textBoxPublisherId.Focus();
                return;
            }
            string tempPublisherName = textBoxPublisherName.Text.Trim();
            if (!(Validator.IsValidName(tempPublisherName)))
            {
                MessageBox.Show("Publisher Name is invalid", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxPublisherName.Clear();
                textBoxPublisherName.Focus();
                return;
            }

            //Operation
            DataRow dr;
            dr = dtPublishers.Rows.Find(tempPublisherId);

            if (dr != null) //Exists
            {
                MessageBox.Show("Publisher ID is already on the Database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxPublisherId.Clear();
                textBoxPublisherId.Focus();

            }
            else //Does not exists
            {
                dr = dtPublishers.NewRow();
                dr["PublisherId"] = textBoxPublisherId.Text.Trim();
                dr["PublisherName"] = textBoxPublisherName.Text.Trim();



                dtPublishers.Rows.Add(dr);
                da.Update(dsHiTech, "Publishers");

                MessageBox.Show("Publisher information was saved successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBoxPublisherId.Clear();
                textBoxPublisherName.Clear();
            }
        }

        private void PublishersForm_Load(object sender, EventArgs e)
        {
            dsHiTech = new DataSet("HiTechDS");
            dtPublishers = new DataTable("Publishers");
            dsHiTech.Tables.Add(dtPublishers);

            dtPublishers.Columns.Add("PublisherId", typeof(Int32));
            dtPublishers.Columns.Add("PublisherName", typeof(String));
            

            dtPublishers.PrimaryKey = new DataColumn[] { dtPublishers.Columns["PublisherId"] };

            da = new SqlDataAdapter("SELECT * FROM Publishers", UtilityDB.ConnectDB());
            sqlBuilder = new SqlCommandBuilder(da);
            da.Fill(dsHiTech.Tables["Publishers"]);
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
                    labelDisplay.Text = "Please enter the Publisher ID";
                    textBoxInput.Clear();
                    textBoxInput.Focus();
                    break;
                case 1:
                    labelDisplay.Text = "Please enter the Publisher Name";
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
                    if (!(Validator.isValidTwoDigitId(tempInput)))
                    {
                        MessageBox.Show("Publisher ID must be a 2-digit number", "Invalid Customer ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                        return;
                    }
                    DataRow dr = dtPublishers.Rows.Find(tempInput);
                    if (dr != null)  //found
                    {
                        //dataGridViewCustomers.Rows.Clear();
                        //dataGridViewCustomers.Refresh();
                        Publisher publisher2 = new Publisher();
                        publisher2.PublisherId = Convert.ToInt32(dr["PublisherId"]);
                        publisher2.PublisherName = dr["PublisherName"].ToString();
                        

                        if (publisher2 == null)
                        {
                            MessageBox.Show("Some error occurred", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            dataGridViewCustomers.DataSource = new List<Publisher> { publisher2 };
                        }
                    }
                    else
                    {
                        MessageBox.Show("Publisher does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        MessageBox.Show("Invalid Publisher Name", "Invalid Customer Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                        return;
                    }
                    List<Publisher> listPublisher = new List<Publisher>();
                    Publisher publisher = new Publisher();
                    listPublisher = publisher.PublisherList();
                    if (listPublisher == null)
                    {
                        MessageBox.Show("Some error occured! Database is empty", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        //Book book = new Book();
                        bool test = true;

                        foreach (Publisher aPublisher in listPublisher)
                        {
                            if (aPublisher.PublisherName.ToLower() == tempInput2.ToLower().ToString())
                            {
                                publisher.PublisherId = aPublisher.PublisherId;
                                publisher.PublisherName = aPublisher.PublisherName;
                                

                                if (publisher == null)
                                {
                                    MessageBox.Show("Some error occurred", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    dataGridViewCustomers.DataSource = new List<Publisher> { publisher };
                                    test = false;
                                }
                            }

                        }
                        if (test)
                        {
                            MessageBox.Show("Publisher does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            textBoxInput.Clear();
                            textBoxInput.Focus();
                            dataGridViewCustomers.DataSource = null;
                        }
                    }
                    break;
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
            string tempPublisherId = textBoxPublisherId.Text.Trim();
            if (!(Validator.isValidTwoDigitId(tempPublisherId)))
            {
                MessageBox.Show("Publisher ID must be 2-digit number", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxPublisherId.Clear();
                textBoxPublisherId.Focus();
                return;
            }
            string tempPublisherName = textBoxPublisherName.Text.Trim();
            if (!(Validator.IsValidName(tempPublisherName)))
            {
                MessageBox.Show("Publisher Name is invalid", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxPublisherName.Clear();
                textBoxPublisherName.Focus();
                return;
            }

            //Operation
            DataRow dr = dtPublishers.NewRow();
            dr = dtPublishers.Rows.Find(tempPublisherId);

            if (dr != null) //Exists
            {
                if (Convert.ToInt32(dr["PublisherId"]) == Convert.ToInt32(tempPublisherId))
                {
                    dr["PublisherId"] = textBoxPublisherId.Text.Trim();
                    dr["PublisherName"] = textBoxPublisherName.Text.Trim();



                    //dtPublishers.Rows.Add(dr);
                    da.Update(dsHiTech, "Publishers");

                    MessageBox.Show("Publisher information was updated successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBoxPublisherId.Clear();
                    textBoxPublisherName.Clear();

                }


            }
            else //Does not exists
            {

                MessageBox.Show("Publisher ID is not on the Database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxPublisherId.Clear();
                textBoxPublisherName.Focus();
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            //string tempPublisherId = textBoxPublisherId.Text.Trim();
            //if (!(Validator.isValidTwoDigitId(tempPublisherId)))
            //{
            //    MessageBox.Show("Publisher ID must be 2-digit number", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    textBoxPublisherId.Clear();
            //    textBoxPublisherId.Focus();
            //    return;
            //}
            //string tempPublisherName = textBoxPublisherName.Text.Trim();
            //if (!(Validator.IsValidName(tempPublisherName)))
            //{
            //    MessageBox.Show("Publisher Name is invalid", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    textBoxPublisherName.Clear();
            //    textBoxPublisherName.Focus();
            //    return;
            //}

            ////Operation
            //DataRow dr = dtPublishers.NewRow();
            //dr = dtPublishers.Rows.Find(tempPublisherId);

            //if (dr != null) //Exists
            //{
            //    if (Convert.ToInt32(dr["PublisherId"]) == Convert.ToInt32(tempPublisherId))
            //    {
            //        dr.Delete();
            //        da.Update(dsHiTech, "Publishers");
            //        MessageBox.Show(dr.RowState.ToString(), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //    }


            //}
            //else //Does not exists
            //{

            //    MessageBox.Show("Publisher ID is not on the Database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    textBoxPublisherId.Clear();
            //    textBoxPublisherName.Focus();
            //}

        }
    }
}
