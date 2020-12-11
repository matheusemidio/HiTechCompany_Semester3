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

    public partial class CategoriesForm : Form
    {
        SqlDataAdapter da;
        DataSet dsHiTech;
        DataTable dtCategories;
        SqlCommandBuilder sqlBuilder;
        Category aCategory = new Category();
        public CategoriesForm()
        {
            InitializeComponent();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            //Validation
            string tempCategoryId = textBoxCategoryID.Text.Trim();
            string tempCategoryName = textBoxCategoryName.Text.Trim();

            //Operation
            DataRow dr;
            dr = dtCategories.Rows.Find(tempCategoryId);

            if (dr != null) //Exists
            {
                MessageBox.Show("Category ID is already on the Database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxCategoryID.Clear();
                textBoxCategoryID.Focus();

            }
            else //Does not exists
            {
                dr = dtCategories.NewRow();
                dr["CategoryId"] = textBoxCategoryID.Text.Trim();
                dr["CategoryName"] = textBoxCategoryName.Text.Trim();



                dtCategories.Rows.Add(dr);
                da.Update(dsHiTech, "Categories");

                MessageBox.Show("Category information was saved succssfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBoxCategoryID.Clear();
                textBoxCategoryName.Clear();

            }
        }

        private void CategoriesForm_Load(object sender, EventArgs e)
        {
            dsHiTech = new DataSet("HiTechDS");
            dtCategories = new DataTable("Categories");
            dsHiTech.Tables.Add(dtCategories);

            dtCategories.Columns.Add("CategoryId", typeof(Int32));
            dtCategories.Columns.Add("CategoryName", typeof(String));


            dtCategories.PrimaryKey = new DataColumn[] { dtCategories.Columns["CategoryId"] };

            da = new SqlDataAdapter("SELECT * FROM Categories", UtilityDB.ConnectDB());
            sqlBuilder = new SqlCommandBuilder(da);
            da.Fill(dsHiTech.Tables["Categories"]);
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
                    labelDisplay.Text = "Please enter the Category ID";
                    textBoxInput.Clear();
                    textBoxInput.Focus();
                    break;
                case 1:
                    labelDisplay.Text = "Please enter the Category Name";
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
                    if (!(Validator.isValidCustmerId(tempInput)))
                    {
                        MessageBox.Show("Category ID must be a 2-digit number", "Invalid Customer ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                        return;
                    }
                    DataRow dr = dtCategories.Rows.Find(tempInput);
                    if (dr != null)  //found
                    {
                        //dataGridViewCustomers.Rows.Clear();
                        //dataGridViewCustomers.Refresh();
                        Category category2 = new Category();
                        category2.CategoryId = Convert.ToInt32(dr["CategoryId"]);
                        category2.CategoryName = dr["CategoryName"].ToString();


                        if (category2 == null)
                        {
                            MessageBox.Show("Some error occurred", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            dataGridViewCustomers.DataSource = new List<Category> { category2 };
                        }
                    }
                    else
                    {
                        MessageBox.Show("Category does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                        dataGridViewCustomers.DataSource = null;

                    }

                    break;
                case 1:
                    //Serch by Customer Name
                    string tempInput2 = textBoxInput.Text.Trim();
                    //if (!(Validator.IsValidName(tempInput2)))
                    //{
                    //    MessageBox.Show("Invalid Category Name", "Invalid Customer Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    textBoxInput.Clear();
                    //    textBoxInput.Focus();
                    //    return;
                    //}
                    List<Category> listCategory = new List<Category>();
                    Category category = new Category();
                    listCategory = category.CategoryList();
                    if (listCategory == null)
                    {
                        MessageBox.Show("Some error occured! Database is empty", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        //Book book = new Book();
                        bool test = true;

                        foreach (Category aCategory in listCategory)
                        {
                            if (aCategory.CategoryName.ToLower() == tempInput2.ToLower().ToString())
                            {
                                category.CategoryId = aCategory.CategoryId;
                                category.CategoryName = aCategory.CategoryName;


                                if (category == null)
                                {
                                    MessageBox.Show("Some error occurred", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    dataGridViewCustomers.DataSource = new List<Category> { category };
                                    test = false;
                                }
                            }

                        }
                        if (test)
                        {
                            MessageBox.Show("Category does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            textBoxInput.Clear();
                            textBoxInput.Focus();
                            dataGridViewCustomers.DataSource = null;
                        }
                    }
                    break;
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
            string tempCategoryId = textBoxCategoryID.Text.Trim();
            string tempCategoryName = textBoxCategoryName.Text.Trim();

            //Operation
            DataRow dr = dtCategories.NewRow();
            dr = dtCategories.Rows.Find(tempCategoryId);

            if (dr != null) //Exists
            {
                if (Convert.ToInt32(dr["CategoryId"]) == Convert.ToInt32(tempCategoryId))
                {
                   
                    dr["CategoryId"] = textBoxCategoryID.Text.Trim();
                    dr["CategoryName"] = textBoxCategoryName.Text.Trim();

                    //dtCategories.Rows.Add(dr);
                    da.Update(dsHiTech, "Categories");

                    MessageBox.Show("Category information was updated successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBoxCategoryID.Clear();
                    textBoxCategoryName.Clear();
                }


            }
            else //Does not exists
            {

                MessageBox.Show("Category ID is not on the Database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxCategoryID.Clear();
                textBoxCategoryID.Focus();
            }
        }
    }
}
