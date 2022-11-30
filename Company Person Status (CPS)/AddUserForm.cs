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
    public partial class AddUserForm : Form
    {
        // Variables //
        private readonly int MAX_PEOPLE = 24;
        
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "krI2GcYbBhPh2KkQ3TfoLb4I6LEXYlvt6HMosQZP",
            BasePath = "https://cps-ankamee-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;
        FirebaseResponse clientResponse;
        Dictionary<string, User> allUsers;


        public AddUserForm()
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
            if(textBox1.Text.Length == 0 || textBox2.Text.Length == 0 || textBox3.Text.Length == 0 || comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            if (allUsers.FirstOrDefault(x => x.Value.Username.Equals(textBox2.Text) && !x.Value.isDeleted).Key != null)
            {
                MessageBox.Show("Username has taken.");
                return;
            }

            if (comboBox1.SelectedIndex == (int) AuthorizationTypes.Employer - 1)
            {
                var user = allUsers.FirstOrDefault(x => x.Value.AuthorizationLevelId == (int)AuthorizationTypes.Employer);
                if (user.Key != null)
                MessageBox.Show("Cannot add another employer. There is only one: " + user.Value.FullName);
            }


            else if (comboBox1.SelectedIndex == (int)AuthorizationTypes.CEO - 1)
            {
                var user = allUsers.FirstOrDefault(x => x.Value.AuthorizationLevelId == (int)AuthorizationTypes.CEO && !x.Value.isDeleted);

                if (user.Key != null)
                {
                    MessageBox.Show("Cannot add another CEO. There is only one: " + user.Value.FullName);
                    return;
                }
                else
                {
                    user = allUsers.FirstOrDefault(x => x.Value.isDeleted == true);
                    if(user.Key != null)
                    {
                        updateDeletedUserWithNewOne(user.Value.Id);
                    }
                    else
                    {
                        createNewUserAndAddToDB();
                    }
                }
            }


            else if (comboBox1.SelectedIndex == (int)AuthorizationTypes.Manager - 1)
            {
                var user = allUsers.FirstOrDefault(x => x.Value.AuthorizationLevelId == (int)AuthorizationTypes.Manager && !x.Value.isDeleted);

                if (user.Key != null)
                {
                    MessageBox.Show("Cannot add another manager. There is only one: " + user.Value.FullName);
                    return;
                }

                else
                {
                    user = allUsers.FirstOrDefault(x => x.Value.isDeleted == true);
                    if (user.Key != null)
                    {
                        updateDeletedUserWithNewOne(user.Value.Id);
                    }
                    else
                    {
                        createNewUserAndAddToDB();
                    }
                }
            }


            else {
                var user = allUsers.FirstOrDefault(x => x.Value.isDeleted == true);
                if(user.Key != null)
                {
                    updateDeletedUserWithNewOne(user.Value.Id);
                }
                else
                {
                    createNewUserAndAddToDB();
                }
            }
        }

        private void createNewUserAndAddToDB()
        {
            clientResponse = client.Get("");
            allUsers = JsonConvert.DeserializeObject<Dictionary<string, User>>(clientResponse.Body.ToString());
            var user = new User
            {
                Id = allUsers.Count + 101,
                AuthorizationLevelId = comboBox1.SelectedIndex + 1,
                FullName = textBox3.Text,
                Password = textBox1.Text,
                StatusId = (int)StatusTypes.Offline,
                ThisMonthAwayDuration = 0,
                ThisWeekAwayDuration = 0,
                TodaysAwayDuration = 0,
                Username = textBox2.Text,
                isDeleted = false
            };
            client.SetAsync("/User" + user.Id, user);
            (this.Owner as AdminPanelForm).allUsersField.Items.Add(user.FullName);
            MessageBox.Show("User added to the system.");
            if ((this.Owner as AdminPanelForm).allUsersField.Items.Count == MAX_PEOPLE)
            {
                (this.Owner as AdminPanelForm).addUserButton.Enabled = false;
            }
            this.Close();
        }

        private void updateDeletedUserWithNewOne(int userId)
        {
            var user = new User
            {
                Id = userId,
                AuthorizationLevelId = comboBox1.SelectedIndex + 1,
                FullName = textBox3.Text,
                Password = textBox1.Text,
                StatusId = (int)StatusTypes.Offline,
                ThisMonthAwayDuration = 0,
                ThisWeekAwayDuration = 0,
                TodaysAwayDuration = 0,
                Username = textBox2.Text,
                isDeleted = false
            };
            client.UpdateAsync("/User" + user.Id, user);
            (this.Owner as AdminPanelForm).allUsersField.Items.Add(user.FullName);
            MessageBox.Show("User added to the system.");
            if ((this.Owner as AdminPanelForm).allUsersField.Items.Count == MAX_PEOPLE)
            {
                (this.Owner as AdminPanelForm).addUserButton.Enabled = false;
            }
            this.Close();
        }
    }
}
