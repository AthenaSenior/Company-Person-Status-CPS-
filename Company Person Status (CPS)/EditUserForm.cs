using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Company_Person_Status__CPS_
{
    public partial class EditUserForm : Form
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "krI2GcYbBhPh2KkQ3TfoLb4I6LEXYlvt6HMosQZP",
            BasePath = "https://cps-ankamee-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FirebaseResponse clientResponse = client.Get("");
            Dictionary<string, User> allUsers = JsonConvert.DeserializeObject<Dictionary<string, User>>(clientResponse.Body.ToString());
            foreach (var user in allUsers)
            {
                if(label1.Text.Equals(user.Value.FullName))
                {
                    var userForEdit = new User
                    {
                        Id = user.Value.Id,
                        AuthorizationLevelId = comboBox1.SelectedIndex + 1,
                        AwayFor = user.Value.AwayFor,
                        FullName = textBox1.Text,
                        Password = user.Value.Password,
                        StatusId = user.Value.StatusId,
                        ThisMonthAwayDuration = user.Value.ThisMonthAwayDuration,
                        ThisWeekAwayDuration = user.Value.ThisWeekAwayDuration,
                        TodaysAwayDuration = user.Value.TodaysAwayDuration,
                        Username = textBox2.Text,
                        isDeleted = false
                    };
                    client.UpdateTaskAsync("/User"+userForEdit.Id, userForEdit);
                    MessageBox.Show("User updated.");
                    this.Close();
                    break;
                }
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void EditUserForm_Load(object sender, EventArgs e)
        {
            label1.Text = AdminPanelForm.userFullName;
            FirebaseResponse clientResponse = client.Get("");
            Dictionary<string, User> allUsers = JsonConvert.DeserializeObject<Dictionary<string, User>>(clientResponse.Body.ToString());
            foreach (var user in allUsers)
            {
                if (label1.Text.Equals(user.Value.FullName))
                {
                    textBox1.Text = user.Value.FullName;
                    textBox2.Text = user.Value.Username;
                    textBox3.Text = user.Value.Password;
                    comboBox1.SelectedIndex = user.Value.AuthorizationLevelId - 1;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FirebaseResponse clientResponse = client.Get("");
            Dictionary<string, User> allUsers = JsonConvert.DeserializeObject<Dictionary<string, User>>(clientResponse.Body.ToString());
            DialogResult dialogResult = MessageBox.Show("Are you sure to reset all away durations of " + label1.Text + "? \n\nWith this action, " + label1.Text + "'s all away durations become zero.", "Reset Duration", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                foreach (var user in allUsers)
                {
                    if (label1.Text.Equals(user.Value.FullName))
                    {
                        var userForEdit = new User
                        {
                            Id = user.Value.Id,
                            AuthorizationLevelId = user.Value.AuthorizationLevelId,
                            AwayFor = user.Value.AwayFor,
                            FullName = user.Value.FullName,
                            Password = user.Value.Password,
                            StatusId = user.Value.StatusId,
                            ThisMonthAwayDuration = 0,
                            ThisWeekAwayDuration = 0,
                            TodaysAwayDuration = 0,
                            Username = user.Value.Username,
                            isDeleted = false
                        };
                        client.UpdateTaskAsync("/User" + userForEdit.Id, userForEdit);
                        MessageBox.Show("User durations reset.");
                        break;
                    }
                }
            }
        }
    }
}
