using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Company_Person_Status__CPS_
{
    public partial class AddUserForm : Form
    {
        // Variables //
        private int userCount = 0;
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
            if(comboBox1.SelectedIndex == (int) AuthorizationTypes.Employer - 1)
            {
                foreach (var user in allUsers)
                {
                    if (user.Value.AuthorizationLevelId == (int)AuthorizationTypes.Employer)
                    {
                        MessageBox.Show("Cannot add another employer. There is only one: " + user.Value.FullName);
                        return;
                    }
                }
            }


            else if (comboBox1.SelectedIndex == (int)AuthorizationTypes.CEO - 1)
            {
                bool notAnyDeletedUserDetected = true;
                foreach (var user in allUsers)
                {
                    if (user.Value.AuthorizationLevelId == (int)AuthorizationTypes.CEO)
                    {
                        MessageBox.Show("Cannot add another CEO. There is only one: " + user.Value.FullName);
                        return;
                    }
                    if(user.Value.isDeleted)
                    {
                        updateDeletedUserWithNewOne(user.Value.Id);
                        notAnyDeletedUserDetected = false;
                        break;
                    }
                }
                if(notAnyDeletedUserDetected)
                {
                    createNewUserAndAddToDB();
                }
            }


            else if (comboBox1.SelectedIndex == (int)AuthorizationTypes.Manager - 1)
            {
                bool notAnyDeletedUserDetected = true;
                foreach (var user in allUsers)
                {
                    if (user.Value.AuthorizationLevelId == (int)AuthorizationTypes.Manager)
                    {
                        MessageBox.Show("Cannot add another manager. There is only one: " + user.Value.FullName);
                        return;
                    }
                    if (user.Value.isDeleted)
                    {
                        updateDeletedUserWithNewOne(user.Value.Id);
                        notAnyDeletedUserDetected = false;
                        break;
                    }
                }
                if (notAnyDeletedUserDetected)
                {
                    createNewUserAndAddToDB();
                }
            }


            else {
                bool notAnyDeletedUserDetected  = true;
                foreach (var user in allUsers)
                {
                    if (user.Value.isDeleted)
                    {
                        updateDeletedUserWithNewOne(user.Value.Id);
                        notAnyDeletedUserDetected = false;
                        break;
                    }
                }
                if (notAnyDeletedUserDetected)
                {
                    createNewUserAndAddToDB();
                }
            }
        }

        private void AddUserForm_Load(object sender, EventArgs e)
        {
            foreach (var user in allUsers)
            {
                userCount++;
            }
        }

        private void createNewUserAndAddToDB()
        {
            var user = new User
            {
                Id = userCount + 101,
                AuthorizationLevelId = comboBox1.SelectedIndex + 1,
                AwayFor = 0,
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
            (this.Owner as AdminPanelForm).listBox1.Items.Add(user.FullName);
            MessageBox.Show("User added to the system.");
            if ((this.Owner as AdminPanelForm).listBox1.Items.Count == MAX_PEOPLE)
            {
                (this.Owner as AdminPanelForm).button1.Enabled = false;
            }
            this.Close();
        }

        private void updateDeletedUserWithNewOne(int userId)
        {
            var user = new User
            {
                Id = userId,
                AuthorizationLevelId = comboBox1.SelectedIndex + 1,
                AwayFor = 0,
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
            (this.Owner as AdminPanelForm).listBox1.Items.Add(user.FullName);
            MessageBox.Show("User added to the system.");
            if ((this.Owner as AdminPanelForm).listBox1.Items.Count == MAX_PEOPLE)
            {
                (this.Owner as AdminPanelForm).button1.Enabled = false;
            }
            this.Close();
        }



    }
}
