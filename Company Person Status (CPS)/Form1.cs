using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Company_Person_Status__CPS_
{
    public partial class Form1 : Form
    {
        public User loggedInUser;
        public Form1()
        {
            InitializeComponent();
            pictureBox2.BackColor = Color.Transparent;
            pictureBox1.BackColor = Color.Transparent;
            button1.BackColor = Color.Transparent;
            button2.BackColor = Color.Transparent;
            button3.BackColor = Color.Transparent;
            button1.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button1.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button2.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button2.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button3.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button3.FlatAppearance.MouseOverBackColor = Color.Transparent;      
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Owner = this;
            loginForm.ShowDialog();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Help help = new Help();
            help.ShowDialog();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void userLoggedIn(User user)
        {
            loggedInUser = user;
            if (loggedInUser != null)
            {
                label6.Text = loggedInUser.FullName;
                label3.ForeColor = Color.Green;
                label3.Text = "Online";
                ControlButton.BackColor = Color.Yellow;
                ControlButton.Text = "Go Away";

                switch (loggedInUser.AuthorizationLevelId)
                {
                    case (int) AuthorizationTypes.Employer:
                        {
                            AdminPanelButton.Text = "Admin Panel";
                            AdminPanelButton.Visible = true;
                            break;
                        }
                    default:
                        {
                            AdminPanelButton.Visible = false;
                            break;
                        }
                }
            }
        }

        private void ControlButton_Click(object sender, EventArgs e)
        {
            switch(label3.Text)
            {
                case "Online":
                    {
                        label3.Text = "Away";
                        label3.ForeColor = Color.Yellow;
                        ControlButton.BackColor = Color.Green;
                        ControlButton.Text = "Go Online";
                        label4.Visible = true;
                        break;
                    }
                case "Away":
                    {
                        label3.Text = "Online";
                        label3.ForeColor = Color.Green;
                        ControlButton.BackColor = Color.Yellow;
                        ControlButton.Text = "Go Away";
                        label4.Visible = false;
                        break;
                    }
            }
        }

        private void AdminPanelButton_Click(object sender, EventArgs e)
        {
            AdminPanelForm apf = new AdminPanelForm();
            apf.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
