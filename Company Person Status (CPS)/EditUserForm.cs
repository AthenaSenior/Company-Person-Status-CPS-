using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Company_Person_Status__CPS_
{
    public partial class EditUserForm : Form
    {
        // Variables //
        private readonly int MAX_EMPLOYEE = 21;

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "krI2GcYbBhPh2KkQ3TfoLb4I6LEXYlvt6HMosQZP",
            BasePath = "https://cps-ankamee-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;
        Dictionary<string, User> allUsers;
        FirebaseResponse clientResponse;
        private bool changeCEO, changeManager, changeEmployer;

        public EditUserForm()
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
            clientResponse = client.Get("");
            allUsers = JsonConvert.DeserializeObject<Dictionary<string, User>>(clientResponse.Body.ToString());
        }

        // Methods //
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0 || textBox2.Text.Length == 0 || comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            if (comboBox1.SelectedIndex == (int)AuthorizationTypes.Employer - 1)
            {
                var user = allUsers.FirstOrDefault(x => x.Value.AuthorizationLevelId == (int)AuthorizationTypes.Employer);
                if (user.Key != null && !changeEmployer)
                {
                    MessageBox.Show("Cannot add another employer. There is only one: " + user.Value.FullName);
                    return;
                }
                else if(allUsers.FirstOrDefault(x => x.Value.FullName.Equals(label1.Text)).Value.AuthorizationLevelId != comboBox1.SelectedIndex + 1)
                {
                    user = allUsers.FirstOrDefault(x => x.Value.FullName.Equals(label1.Text));
                    updateUser(user);
                    MessageBox.Show("Authorized people's field has changed. Please restart the program.");
                }
                else
                {
                    user = allUsers.FirstOrDefault(x => x.Value.FullName.Equals(label1.Text));
                    updateUser(user);
                }
            }


            else if (comboBox1.SelectedIndex == (int)AuthorizationTypes.CEO - 1)
            {
                var user = allUsers.FirstOrDefault(x => x.Value.AuthorizationLevelId == (int)AuthorizationTypes.CEO);
                if (user.Key != null && !changeCEO)
                {
                    MessageBox.Show("Cannot add another CEO. There is only one: " + user.Value.FullName);
                    return;
                }
                else if (allUsers.FirstOrDefault(x => x.Value.FullName.Equals(label1.Text)).Value.AuthorizationLevelId != comboBox1.SelectedIndex + 1)
                {
                    user = allUsers.FirstOrDefault(x => x.Value.FullName.Equals(label1.Text));
                    updateUser(user);
                    MessageBox.Show("Authorized people's field has changed. Please restart the program.");
                }
                else
                {
                    user = allUsers.FirstOrDefault(x => x.Value.FullName.Equals(label1.Text));
                    updateUser(user);
                }
            }


            else if (comboBox1.SelectedIndex == (int)AuthorizationTypes.Manager - 1)
            {
                var user = allUsers.FirstOrDefault(x => x.Value.AuthorizationLevelId == (int)AuthorizationTypes.Manager);
                if(user.Key != null && !changeManager)
                {
                    MessageBox.Show("Cannot add another manager. There is only one: " + user.Value.FullName);
                    return;
                }
                else if (allUsers.FirstOrDefault(x => x.Value.FullName.Equals(label1.Text)).Value.AuthorizationLevelId != comboBox1.SelectedIndex + 1)
                {
                    user = allUsers.FirstOrDefault(x => x.Value.FullName.Equals(label1.Text));
                    updateUser(user);
                    MessageBox.Show("Authorized people's field has changed. Please restart the program.");
                }
                else
                {
                    user = allUsers.FirstOrDefault(x => x.Value.FullName.Equals(label1.Text));
                    updateUser(user);
                }
            }

            else 
            {
                if (allUsers.FirstOrDefault(x => x.Value.FullName.Equals(label1.Text)).Value.AuthorizationLevelId != comboBox1.SelectedIndex + 1 && allUsers.Where(x => x.Value.AuthorizationLevelId != (int)AuthorizationTypes.Employee).Count() == MAX_EMPLOYEE)
                {
                    var user = allUsers.FirstOrDefault(x => x.Value.FullName.Equals(label1.Text));
                    updateUser(user);
                    MessageBox.Show("Authorized people's field has changed. Please restart the program.");
                    Application.Restart();
                    return;
                }
                else if (allUsers.Where(x => x.Value.AuthorizationLevelId == (int)AuthorizationTypes.Employee).Count() == MAX_EMPLOYEE)
                {
                    MessageBox.Show("There are already 21 employees. Cannot edit!");
                    return;
                }
                else
                {
                    var user = allUsers.FirstOrDefault(x => x.Value.FullName.Equals(label1.Text));
                    updateUser(user);
                }
            }
        }

        private void EditUserForm_Load(object sender, EventArgs e)
        {
            label1.Text = AdminPanelForm.userFullName;
            var user = allUsers.FirstOrDefault(x => x.Value.FullName.Equals(label1.Text));
            getSelectedUserData(user);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to reset all away durations of " + label1.Text + "? \n\nWith this action, " + label1.Text + "'s all away durations become zero.", "Reset Duration", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                var user = allUsers.FirstOrDefault(x => x.Value.FullName.Equals(label1.Text));
                resetUserDuration(user);
            }
        }

        private void updateUser(KeyValuePair<string, User> user)
        {
            var userForEdit = new User
            {
                Id = user.Value.Id,
                AuthorizationLevelId = comboBox1.SelectedIndex + 1,
                FullName = textBox1.Text,
                Password = user.Value.Password,
                StatusId = user.Value.StatusId,
                ThisMonthAwayDuration = user.Value.ThisMonthAwayDuration,
                ThisWeekAwayDuration = user.Value.ThisWeekAwayDuration,
                TodaysAwayDuration = user.Value.TodaysAwayDuration,
                Username = textBox2.Text,
                isDeleted = false
            };
            client.UpdateAsync("/User" + userForEdit.Id, userForEdit);
            MessageBox.Show("User updated.");
            (this.Owner as AdminPanelForm).allUsersField.Items.Add(userForEdit.FullName);
            (this.Owner as AdminPanelForm).allUsersField.Items.Remove((this.Owner as AdminPanelForm).allUsersField.SelectedItem);
            this.Close();
        }

        private void getSelectedUserData(KeyValuePair<string, User> user)
        {
            textBox1.Text = user.Value.FullName;
            textBox2.Text = user.Value.Username;
            textBox3.Text = user.Value.Password;
            comboBox1.SelectedIndex = user.Value.AuthorizationLevelId - 1;
            switch(user.Value.AuthorizationLevelId)
            {
                case (int)AuthorizationTypes.Manager:
                    {
                        changeManager = true;
                        changeCEO = false;
                        changeEmployer = false;
                        break;
                    }
                case (int)AuthorizationTypes.CEO:
                    {
                        changeManager = false;
                        changeCEO = true;
                        changeEmployer = false;
                        break;
                    }
                case (int)AuthorizationTypes.Employer:
                    {
                        changeManager = false;
                        changeCEO = false;
                        changeEmployer = true;
                        break;
                    }
                default:
                    {
                        changeManager = false;
                        changeCEO = false;
                        changeEmployer = false;
                        break;
                    }
            }

        }

        private void resetUserDuration(KeyValuePair<string,User> user)
        {
            var userForEdit = new User
            {
                Id = user.Value.Id,
                AuthorizationLevelId = user.Value.AuthorizationLevelId,
                FullName = user.Value.FullName,
                Password = user.Value.Password,
                StatusId = user.Value.StatusId,
                ThisMonthAwayDuration = 0,
                ThisWeekAwayDuration = 0,
                TodaysAwayDuration = 0,
                Username = user.Value.Username,
                isDeleted = false
            };
            client.UpdateAsync("/User" + userForEdit.Id, userForEdit);
            MessageBox.Show("User durations reset.");
        }

    }
}
