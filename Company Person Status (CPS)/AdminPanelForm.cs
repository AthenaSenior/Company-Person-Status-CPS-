using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Company_Person_Status__CPS_
{
    public partial class AdminPanelForm : Form
    {

        public static string userFullName = "";

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "krI2GcYbBhPh2KkQ3TfoLb4I6LEXYlvt6HMosQZP",
            BasePath = "https://cps-ankamee-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;

        public AdminPanelForm()
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

        private void AdminPanelForm_Load(object sender, EventArgs e)
        {
            FirebaseResponse clientResponse = client.Get("");
            Dictionary<string, User> allUsers = JsonConvert.DeserializeObject<Dictionary<string, User>>(clientResponse.Body.ToString());
            foreach (var user in allUsers)
            {
                if(!user.Value.isDeleted)
                listBox1.Items.Add(user.Value.FullName);
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            userFullName = listBox1.SelectedItem.ToString();
            TrackDurationForm tdf = new TrackDurationForm();
            tdf.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            userFullName = listBox1.SelectedItem.ToString();
            EditUserForm euf = new EditUserForm();
            euf.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddUserForm auf = new AddUserForm();
            auf.Owner = this;
            auf.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FirebaseResponse clientResponse = client.Get("");
            Dictionary<string, User> allUsers = JsonConvert.DeserializeObject<Dictionary<string, User>>(clientResponse.Body.ToString());
            DialogResult dialogResult = MessageBox.Show("Are you sure to remove " + listBox1.SelectedItem.ToString() + " from the CPS system?", "Remove User", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                foreach (var user in allUsers)
                {
                    if(user.Value.FullName.Equals(listBox1.SelectedItem.ToString()))
                    {
                        if(user.Value.AuthorizationLevelId == (int) AuthorizationTypes.Employer)
                        {
                            MessageBox.Show("Employer cannot be deleted.");
                            break;
                        }
                        var userForEdit = new User
                        {
                            Id = user.Value.Id,
                            AuthorizationLevelId = user.Value.AuthorizationLevelId,
                            AwayFor = user.Value.AwayFor,
                            FullName = user.Value.FullName,
                            Password = user.Value.Password,
                            StatusId = user.Value.StatusId,
                            ThisMonthAwayDuration = user.Value.ThisMonthAwayDuration,
                            ThisWeekAwayDuration = user.Value.ThisWeekAwayDuration,
                            TodaysAwayDuration = user.Value.TodaysAwayDuration,
                            Username = user.Value.Username,
                            isDeleted = true
                        };
                        client.UpdateAsync("/User" + userForEdit.Id, userForEdit);
                        listBox1.Items.Remove(listBox1.SelectedItem.ToString());
                        MessageBox.Show("User deleted.");
                        break;
                    }
                }
            }
        }

        private void DurationResetButton_Click(object sender, EventArgs e)
        {
            FirebaseResponse clientResponse = client.Get("");
            Dictionary<string, User> allUsers = JsonConvert.DeserializeObject<Dictionary<string, User>>(clientResponse.Body.ToString());
            DialogResult dialogResult = MessageBox.Show("Are you sure to reset ALL users' ALL durations? \n\nThis includes daily, weekly and monthly away durations and this operation is irreversible!", "One-Click Duration Reset", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                foreach (var user in allUsers)
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
                    client.UpdateAsync("/User" + userForEdit.Id, userForEdit);
                }
                MessageBox.Show("All users' daily, weekly and monthly away durations had been reset.");
            }
        }
    }
}
