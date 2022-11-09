using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Company_Person_Status__CPS_
{
    public partial class Form1 : Form
    {
        public User loggedInUser;

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "krI2GcYbBhPh2KkQ3TfoLb4I6LEXYlvt6HMosQZP",
            BasePath = "https://cps-ankamee-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;
        public Form1()
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
            }
            catch
            {
                MessageBox.Show("No internet connection found. Please contact with your employer.");
            }
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
            this.Closing += OnClosing;
        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            FirebaseResponse clientResponse = client.Get("");
            Dictionary<string, User> allUsers = JsonConvert.DeserializeObject<Dictionary<string, User>>(clientResponse.Body.ToString());
            DialogResult result = MessageBox.Show("Are you sure to close CPS?\n\nWith this action, your status become OFFLINE.", "You are closing CPS",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                foreach (var user in allUsers)
                {
                    if (user.Value.FullName.Equals(loggedInUser.FullName))
                    {
                        var exitingUser = new User
                        {
                            Id = user.Value.Id,
                            AuthorizationLevelId = user.Value.AuthorizationLevelId,
                            AwayFor = user.Value.AwayFor,
                            FullName = user.Value.FullName,
                            Password = user.Value.Password,
                            StatusId = (int)StatusTypes.Offline,
                            ThisMonthAwayDuration = user.Value.ThisMonthAwayDuration,
                            ThisWeekAwayDuration = user.Value.ThisWeekAwayDuration,
                            TodaysAwayDuration = user.Value.TodaysAwayDuration,
                            Username = user.Value.Username,
                            isDeleted = false
                        };
                        client.UpdateTaskAsync("/User" + exitingUser.Id, exitingUser);
                        break;
                    }
                }
                MessageBox.Show("You exited from the system.", "Exit"); //This is important. Do not erase this otherwise system does not update the status in db
            }
            else if (result == DialogResult.No)
                cancelEventArgs.Cancel = false;
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
                label3.Text = "ONLINE";
                ControlButton.BackColor = Color.DarkOrange;
                ControlButton.ForeColor = Color.Black;
                ControlButton.Text = "            Go Away";

                switch (loggedInUser.AuthorizationLevelId)
                {
                    case (int) AuthorizationTypes.Employer:
                        {
                            AdminPanelButton.Text = "  Admin Panel";
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
            FirebaseResponse clientResponse = client.Get("");
            Dictionary<string, User> allUsers = JsonConvert.DeserializeObject<Dictionary<string, User>>(clientResponse.Body.ToString());
            switch (label3.Text)
            {
                case "ONLINE":
                    {
                        label3.Text = "AWAY";
                        label3.ForeColor = Color.DarkOrange;
                        ControlButton.BackColor = Color.Green;
                        ControlButton.Text = "          Go Online";
                        ControlButton.ForeColor = Color.White;
                        label4.Visible = true;
                        foreach (var user in allUsers)
                        {
                            if (user.Value.FullName.Equals(loggedInUser.FullName))
                            {
                                var exitingUser = new User
                                {
                                    Id = user.Value.Id,
                                    AuthorizationLevelId = user.Value.AuthorizationLevelId,
                                    AwayFor = user.Value.AwayFor,
                                    FullName = user.Value.FullName,
                                    Password = user.Value.Password,
                                    StatusId = (int)StatusTypes.Away,
                                    ThisMonthAwayDuration = user.Value.ThisMonthAwayDuration,
                                    ThisWeekAwayDuration = user.Value.ThisWeekAwayDuration,
                                    TodaysAwayDuration = user.Value.TodaysAwayDuration,
                                    Username = user.Value.Username,
                                    isDeleted = false
                                };
                                client.UpdateTaskAsync("/User" + exitingUser.Id, exitingUser);
                                break;
                            }
                        }
                        break;
                    }
                case "AWAY":
                    {
                        label3.Text = "ONLINE";
                        label3.ForeColor = Color.Green;
                        ControlButton.BackColor = Color.DarkOrange;
                        ControlButton.Text = "            Go Away";
                        ControlButton.ForeColor = Color.Black;
                        label4.Visible = false;
                        foreach (var user in allUsers)
                        {
                            if (user.Value.FullName.Equals(loggedInUser.FullName))
                            {
                                var userWithNewStatus = new User
                                {
                                    Id = user.Value.Id,
                                    AuthorizationLevelId = user.Value.AuthorizationLevelId,
                                    AwayFor = user.Value.AwayFor,
                                    FullName = user.Value.FullName,
                                    Password = user.Value.Password,
                                    StatusId = (int)StatusTypes.Online,
                                    ThisMonthAwayDuration = user.Value.ThisMonthAwayDuration,
                                    ThisWeekAwayDuration = user.Value.ThisWeekAwayDuration,
                                    TodaysAwayDuration = user.Value.TodaysAwayDuration,
                                    Username = user.Value.Username,
                                    isDeleted = false
                                };
                                client.UpdateTaskAsync("/User" + userWithNewStatus.Id, userWithNewStatus);
                                break;
                            }
                        }
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

        public void getMajorUsers()
        {
            FirebaseResponse clientResponse = client.Get("");
            Dictionary<string, User> allUsers = JsonConvert.DeserializeObject<Dictionary<string, User>>(clientResponse.Body.ToString());

            foreach (var user in allUsers)
            {
                switch (user.Value.AuthorizationLevelId)
                {
                    case (int)AuthorizationTypes.Employer:
                        {
                            Employer.Text = user.Value.FullName;
                            printStatus(EmployerStatus, user.Value.StatusId);
                            break;
                        }
                    case (int)AuthorizationTypes.CEO:
                        {
                            CEO.Text = user.Value.FullName;
                            printStatus(CEOStatus, user.Value.StatusId);
                            break;
                        }
                    case (int)AuthorizationTypes.Manager:
                        {
                            Manager.Text = user.Value.FullName;
                            printStatus(ManagerStatus, user.Value.StatusId);
                            break;
                        }
                }
            }
        }
        private void printStatus(Label label, int statusId)
        {
            switch (statusId)
            {
                case (int) StatusTypes.Offline:
                    {
                        label.Text = "OFFLINE";
                        label.ForeColor = Color.Red;
                        break;
                    }
                case (int) StatusTypes.Online:
                    {
                        label.Text = "ONLINE";
                        label.ForeColor = Color.Green;
                        break;
                    }
                case (int) StatusTypes.Away:
                    {
                        label.Text = "AWAY";
                        label.ForeColor = Color.DarkOrange;
                        break;
                    }
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Exit or no?",
                               "My First Application",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
