using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Module1.Business;
using Module1.Validation;
using Module1.GUI;
using Module2.GUI;
using Module3.GUI;
using Module4.GUI;


namespace HiTechDistribution_Matheus_1931358.GUI
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            //Validation
            string tempUserId = textBoxUserId.Text.Trim();
            if (!(Validator.isValidId(tempUserId)))
            {
                MessageBox.Show("User ID must be a 4-digit number", "Invalid User ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxUserId.Clear();
                textBoxUserId.Focus();
                return;
            }

            string tempPassword = textBoxPassword.Text.Trim();
            if (!(Validator.isValidPassword(tempPassword)))
            {
                MessageBox.Show("Password must be 12-digit and can not contain white spaces.", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxPassword.Clear();
                textBoxPassword.Focus();
                return;
            }

            User user = new User();
            user.UserId = Convert.ToInt32(textBoxUserId.Text.Trim());
            user.Password = textBoxPassword.Text.Trim();
            

            //Checking 
            if (user.SearchUser(user.UserId) != null)  //User exists
            {
                if (user.SearchUser(user.UserId).Password == user.Password)
                {
                    if ((Validator.isValidUserStatus(user.SearchUser(user.UserId).UserStatus)) && (user.SearchUser(user.UserId).UserStatus.ToString().ToLower()) == "active") //User is active
                    {
                        Employee emp = new Employee();
                        if (emp.SearchEmployee(user.UserId).JobId == 1)
                        {
                            MISManagerForm managerForm = new MISManagerForm();
                            this.Hide();
                            managerForm.ShowDialog();
                        }
                        if (emp.SearchEmployee(user.UserId).JobId == 2)
                        {
                            SalesManagerForm salesForm = new SalesManagerForm();
                            this.Hide();
                            salesForm.ShowDialog();
                        }
                        if (emp.SearchEmployee(user.UserId).JobId == 3)
                        {
                            BookForm bookForm = new BookForm();
                            this.Hide();
                            bookForm.ShowDialog();
                        }
                        if (emp.SearchEmployee(user.UserId).JobId == 4)
                        {
                            OrderClerkForm orderClerkForm = new OrderClerkForm();
                            this.Hide();
                            orderClerkForm.ShowDialog();
                        }
                        if (emp.SearchEmployee(user.UserId).JobId == 5)
                        {

                        }
                    }
                    else
                    {
                        MessageBox.Show("This user is currently inactive and can not access the system!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textBoxUserId.Clear();
                        textBoxPassword.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("Wrong User ID or Password.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBoxPassword.Clear();
                }


            }
            else
            {
                MessageBox.Show("Wrong User ID or Password.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBoxUserId.Clear();
            }
        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
