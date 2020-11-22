using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Module1.Validation;
using Module2.DAL;
using Module2.Business;


namespace Module2.GUI
{

    public partial class SalesManagerForm : Form
    {
        SqlDataAdapter da;
        DataSet dsHiTech;
        DataTable dtCustomers;
        SqlCommandBuilder sqlBuilder;
        Customer aCustomer = new Customer();

        public SalesManagerForm()
        {
            InitializeComponent();
        }

        private void SalesManagerForm_Load(object sender, EventArgs e)
        {
            dsHiTech = new DataSet("HiTechDS");
            dtCustomers = new DataTable("Customers");
            dsHiTech.Tables.Add(dtCustomers);

            dtCustomers.Columns.Add("CustomerId", typeof(Int32));
            dtCustomers.Columns.Add("CustomerName", typeof(String));
            dtCustomers.Columns.Add("StreetName", typeof(String));
            dtCustomers.Columns.Add("City", typeof(String));
            dtCustomers.Columns.Add("Province", typeof(String));
            dtCustomers.Columns.Add("PostalCode", typeof(String));
            dtCustomers.Columns.Add("PhoneNumber", typeof(String));
            dtCustomers.Columns.Add("FaxNumber", typeof(String));
            dtCustomers.Columns.Add("Email", typeof(String));
            dtCustomers.Columns.Add("CreditLimit", typeof(Int32));

            dtCustomers.PrimaryKey = new DataColumn[] { dtCustomers.Columns["CustomerId"] };

            da = new SqlDataAdapter("SELECT * FROM Customers", UtilityDB.ConnectDB());
            sqlBuilder = new SqlCommandBuilder(da);
            da.Fill(dsHiTech.Tables["Customers"]);



        }
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            
            string tempCustId = textBoxCustomerID.Text.Trim();
            if (!(Validator.isValidCustmerId(tempCustId)))
            {
                MessageBox.Show("Customer ID must be 2-digit number", "Invalid Customer ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxCustomerID.Clear();
                textBoxCustomerID.Focus();
                return;
            }

            string tempCustName = textBoxCustomerName.Text.Trim();
            if (!(Validator.IsValidName(tempCustName)))
            {
                MessageBox.Show("Invalid Customer Name", "Invalid Customer Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxCustomerName.Clear();
                textBoxCustomerName.Focus();
                return;
            }

            string tempStreet = textBoxStreetName.Text.Trim();
            if (!(Validator.IsValidName(tempStreet)))
            {
                MessageBox.Show("Invalid Street Name", "Invalid Street Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxStreetName.Clear();
                textBoxStreetName.Focus();
                return;
            }

            string tempCity = textBoxCity.Text.Trim();
            if (!(Validator.IsValidName(tempCity)))
            {
                MessageBox.Show("Invalid City Name", "Invalid City Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxCity.Clear();
                textBoxCity.Focus();
                return;
            }

            string tempProvince = textBoxProvince.Text.Trim();
            if (!(Validator.IsValidName(tempProvince)))
            {
                MessageBox.Show("Invalid Province Name", "Invalid Province Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxProvince.Clear();
                textBoxProvince.Focus();
                return;
            }

            string tempPostalCode = textBoxPostalCode.Text.Trim();
            string tempPhone = textBoxPhoneNumber.Text.Trim();
            string tempFax = textBoxFaxNumber.Text.Trim();
            string tempEmail = textBoxEmail.Text.Trim();

            string tempCredit = textBoxCreditLimit.Text.Trim();
            if (!(Validator.isValidCreditLimit(tempCredit)))
            {
                MessageBox.Show("Invalid Credit Limit", "Invalid Credit Limit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxCreditLimit.Clear();
                textBoxCreditLimit.Focus();
                return;
            }

            DataRow dr;
            dr = dtCustomers.Rows.Find(tempCustId);
            if (dr != null) //Exists
            {
                MessageBox.Show("Customer ID is already on the Database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxCustomerID.Clear();
                textBoxCustomerID.Focus();
                
            }
            else //Does not exists
            {
                dr = dtCustomers.NewRow();
                dr["CustomerId"] = textBoxCustomerID.Text.Trim();
                dr["CustomerName"] = textBoxCustomerName.Text.Trim();
                dr["StreetName"] = textBoxStreetName.Text.Trim();
                dr["City"] = textBoxCity.Text.Trim();
                dr["Province"] = textBoxProvince.Text.Trim();
                dr["PostalCode"] = textBoxPostalCode.Text.Trim();
                dr["PhoneNumber"] = textBoxPhoneNumber.Text.Trim();
                dr["FaxNumber"] = textBoxFaxNumber.Text.Trim();
                dr["Email"] = textBoxEmail.Text.Trim();
                dr["CreditLimit"] = textBoxCreditLimit.Text.Trim();
                dtCustomers.Rows.Add(dr);
                da.Update(dsHiTech, "Customers");

                MessageBox.Show(dr.RowState.ToString(), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void buttonList_Click(object sender, EventArgs e)
        {
            //Customer customer = new Customer();
            List<Customer> listCustomer = new List<Customer>();
            Customer customer = new Customer();
            listCustomer = customer.CustomerList();
            if (listCustomer == null)
            {
                MessageBox.Show("Some error occured! Database is empty", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                dataGridViewCustomers.DataSource = listCustomer;
            }
        }


        private void comboBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            int indexSelected = comboBox.SelectedIndex;
            switch (indexSelected)
            {
                case 0:
                    labelDisplay.Text = "Please enter the Customer ID";
                    textBoxInput.Clear();
                    textBoxInput.Focus();
                    break;
                case 1:
                    labelDisplay.Text = "Please enter the Customer Name";
                    textBoxInput.Clear();
                    textBoxInput.Focus();
                    break;

                default:
                    break;
            }
        }

        private void buttonSearch_Click_1(object sender, EventArgs e)
        {
            int indexSelected = comboBox.SelectedIndex;
            switch (indexSelected)
            {
                //Search By Customer ID
                case 0:
                    string tempCustId = textBoxInput.Text.Trim();
                    if (!(Validator.isValidCustmerId(tempCustId)))
                    {
                        MessageBox.Show("Customer ID must be 2-digit number", "Invalid Customer ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBoxCustomerID.Clear();
                        textBoxCustomerID.Focus();
                        return;
                    }
                    DataRow dr = dtCustomers.Rows.Find(tempCustId);
                    if (dr != null)  //found
                    {
                        //dataGridViewCustomers.Rows.Clear();
                        //dataGridViewCustomers.Refresh();
                        Customer cust = new Customer();
                        cust.CustomerId = Convert.ToInt32(dr["CustomerId"]);
                        cust.CustomerName = dr["CustomerName"].ToString();
                        cust.StreetName = dr["StreetName"].ToString();
                        cust.City = dr["City"].ToString();
                        cust.Province = dr["Province"].ToString();
                        cust.PostalCode = dr["PostalCode"].ToString();
                        cust.PhoneNumber = dr["PhoneNumber"].ToString();
                        cust.FaxNumber = dr["FaxNumber"].ToString();
                        cust.Email = dr["Email"].ToString();
                        cust.CreditLimit = Convert.ToInt32(dr["CreditLimit"]);

                        if(cust == null)
                        {
                            MessageBox.Show("Some error occurred", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            dataGridViewCustomers.DataSource = new List<Customer> { cust};
                        }
                    }
                    else
                    {
                        MessageBox.Show("Customer does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBoxCustomerID.Clear();
                        textBoxCustomerID.Focus();
                    }

                    break;
                case 1:
                    //Serch by Customer Name
                    string tempCustName = textBoxInput.Text.Trim();
                    if (!(Validator.IsValidName(tempCustName)))
                    {
                        MessageBox.Show("Invalid Customer Name", "Invalid Customer Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBoxCustomerName.Clear();
                        textBoxCustomerName.Focus();
                        return;
                    }
                    Customer cust1 = new Customer();
                    //DataRow dr3 = new DataRow();
                    //DataRow dr2 = dtCustomers.Rows.Find(tempCustName.ToString());
                    //List<DataRow> list = list.Any(DataRow => DataRow["FirstName"].Equals(tempCustName.ToString()));
                    //List<DataRow> listDr = listDr.FindAll(dr3["FirstName"] tempCustName.ToString());
                    DataRow[] rows = dtCustomers.Select("FirstName =" + tempCustName,ToString());

                    if (rows != null)  //found
                    {
                        foreach(DataRow dr2 in rows)
                        {
                            cust1.CustomerId = Convert.ToInt32(dr2["CustomerId"]);
                            cust1.CustomerName = dr2["CustomerName"].ToString();
                            cust1.StreetName = dr2["StreetName"].ToString();
                            cust1.City = dr2["City"].ToString();
                            cust1.Province = dr2["Province"].ToString();
                            cust1.PostalCode = dr2["PostalCode"].ToString();
                            cust1.PhoneNumber = dr2["PhoneNumber"].ToString();
                            cust1.FaxNumber = dr2["FaxNumber"].ToString();
                            cust1.Email = dr2["Email"].ToString();
                            cust1.CreditLimit = Convert.ToInt32(dr2["CreditLimit"]);
                        }
                        //dataGridViewCustomers.Rows.Clear();
                        //dataGridViewCustomers.Refresh();
                        //Customer cust = new Customer();
                        //cust.CustomerId = Convert.ToInt32(dr2["CustomerId"]);
                        //cust.CustomerName = dr2["CustomerName"].ToString();
                        //cust.StreetName = dr2["StreetName"].ToString();
                        //cust.City = dr2["City"].ToString();
                        //cust.Province = dr2["Province"].ToString();
                        //cust.PostalCode = dr2["PostalCode"].ToString();
                        //cust.PhoneNumber = dr2["PhoneNumber"].ToString();
                        //cust.FaxNumber = dr2["FaxNumber"].ToString();
                        //cust.Email = dr2["Email"].ToString();
                        //cust.CreditLimit = Convert.ToInt32(dr2["CreditLimit"]);

                        if (cust1 == null)
                        {
                            MessageBox.Show("Some error occurred", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            dataGridViewCustomers.DataSource = new List<Customer> { cust1 };
                        }
                    }
                        break;
                default:
                    break;

            }
        }

        private void buttonExit_Click_1(object sender, EventArgs e)
        {
            DialogResult answer = MessageBox.Show("Would you like to exit the application?", "Exit Window", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (answer == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            string tempCustId = textBoxCustomerID.Text.Trim();
            if (!(Validator.isValidCustmerId(tempCustId)))
            {
                MessageBox.Show("Customer ID must be 2-digit number", "Invalid Customer ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxCustomerID.Clear();
                textBoxCustomerID.Focus();
                return;
            }

            string tempCustName = textBoxCustomerName.Text.Trim();
            if (!(Validator.IsValidName(tempCustName)))
            {
                MessageBox.Show("Invalid Customer Name", "Invalid Customer Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxCustomerName.Clear();
                textBoxCustomerName.Focus();
                return;
            }

            string tempStreet = textBoxStreetName.Text.Trim();
            if (!(Validator.IsValidName(tempStreet)))
            {
                MessageBox.Show("Invalid Street Name", "Invalid Street Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxStreetName.Clear();
                textBoxStreetName.Focus();
                return;
            }

            string tempCity = textBoxCity.Text.Trim();
            if (!(Validator.IsValidName(tempCity)))
            {
                MessageBox.Show("Invalid City Name", "Invalid City Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxCity.Clear();
                textBoxCity.Focus();
                return;
            }

            string tempProvince = textBoxProvince.Text.Trim();
            if (!(Validator.IsValidName(tempProvince)))
            {
                MessageBox.Show("Invalid Province Name", "Invalid Province Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxProvince.Clear();
                textBoxProvince.Focus();
                return;
            }

            string tempPostalCode = textBoxPostalCode.Text.Trim();
            string tempPhone = textBoxPhoneNumber.Text.Trim();
            string tempFax = textBoxFaxNumber.Text.Trim();
            string tempEmail = textBoxEmail.Text.Trim();

            string tempCredit = textBoxCreditLimit.Text.Trim();
            if (!(Validator.isValidCreditLimit(tempCredit)))
            {
                MessageBox.Show("Invalid Credit Limit", "Invalid Credit Limit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxCreditLimit.Clear();
                textBoxCreditLimit.Focus();
                return;
            }

            DataRow dr = dtCustomers.NewRow();
            dr = dtCustomers.Rows.Find(tempCustId);
            if(dr != null) //Exists
            {
                if(Convert.ToInt32(dr["CustomerId"]) == Convert.ToInt32(textBoxCustomerID.Text))
                {
                    dr["CustomerId"] = textBoxCustomerID.Text.Trim();
                    dr["CustomerName"] = textBoxCustomerName.Text.Trim();
                    dr["StreetName"] = textBoxStreetName.Text.Trim();
                    dr["City"] = textBoxCity.Text.Trim();
                    dr["Province"] = textBoxProvince.Text.Trim();
                    dr["PostalCode"] = textBoxPostalCode.Text.Trim();
                    dr["PhoneNumber"] = textBoxPhoneNumber.Text.Trim();
                    dr["FaxNumber"] = textBoxFaxNumber.Text.Trim();
                    dr["Email"] = textBoxEmail.Text.Trim();
                    dr["CreditLimit"] = textBoxCreditLimit.Text.Trim();
                    //dtCustomers.Rows.(dr);
                    da.Update(dsHiTech, "Customers");

                    MessageBox.Show(dr.RowState.ToString(), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
            }
            else  //Does not exist
            {
                MessageBox.Show("Customer does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxCustomerID.Clear();
                textBoxCustomerID.Focus();
            }

        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            string tempCustId = textBoxCustomerID.Text.Trim();
            if (!(Validator.isValidCustmerId(tempCustId)))
            {
                MessageBox.Show("Customer ID must be 2-digit number", "Invalid Customer ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxCustomerID.Clear();
                textBoxCustomerID.Focus();
                return;
            }

            //string tempCustName = textBoxCustomerName.Text.Trim();
            //if (!(Validator.IsValidName(tempCustName)))
            //{
            //    MessageBox.Show("Invalid Customer Name", "Invalid Customer Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    textBoxCustomerName.Clear();
            //    textBoxCustomerName.Focus();
            //    return;
            //}

            //string tempStreet = textBoxStreetName.Text.Trim();
            //if (!(Validator.IsValidName(tempStreet)))
            //{
            //    MessageBox.Show("Invalid Street Name", "Invalid Street Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    textBoxStreetName.Clear();
            //    textBoxStreetName.Focus();
            //    return;
            //}

            //string tempCity = textBoxCity.Text.Trim();
            //if (!(Validator.IsValidName(tempCity)))
            //{
            //    MessageBox.Show("Invalid City Name", "Invalid City Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    textBoxCity.Clear();
            //    textBoxCity.Focus();
            //    return;
            //}

            //string tempProvince = textBoxProvince.Text.Trim();
            //if (!(Validator.IsValidName(tempProvince)))
            //{
            //    MessageBox.Show("Invalid Province Name", "Invalid Province Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    textBoxProvince.Clear();
            //    textBoxProvince.Focus();
            //    return;
            //}

            //string tempPostalCode = textBoxPostalCode.Text.Trim();
            //string tempPhone = textBoxPhoneNumber.Text.Trim();
            //string tempFax = textBoxFaxNumber.Text.Trim();
            //string tempEmail = textBoxEmail.Text.Trim();

            //string tempCredit = textBoxCreditLimit.Text.Trim();
            //if (!(Validator.isValidCreditLimit(tempCredit)))
            //{
            //    MessageBox.Show("Invalid Credit Limit", "Invalid Credit Limit", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    textBoxCreditLimit.Clear();
            //    textBoxCreditLimit.Focus();
            //    return;
            //}

            DataRow dr = dtCustomers.NewRow();
            dr = dtCustomers.Rows.Find(tempCustId);
            if (dr != null) //Exists
            {
                if (Convert.ToInt32(dr["CustomerId"]) == Convert.ToInt32(textBoxCustomerID.Text))
                {
                    dr.Delete();
                    da.Update(dsHiTech, "Customers");
                    MessageBox.Show(dr.RowState.ToString(), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
            }
            else  //Does not exist
            {
                MessageBox.Show("Customer does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxCustomerID.Clear();
                textBoxCustomerID.Focus();
            }
        }
    }
}
