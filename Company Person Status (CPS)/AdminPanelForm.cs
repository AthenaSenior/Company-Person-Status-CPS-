﻿using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Company_Person_Status__CPS_
{
    public partial class AdminPanelForm : Form
    {
        private readonly int MAX_PEOPLE = 24;
        public static string userFullName = "";

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "krI2GcYbBhPh2KkQ3TfoLb4I6LEXYlvt6HMosQZP",
            BasePath = "https://cps-ankamee-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;
        FirebaseResponse clientResponse;
        Dictionary<string, User> allUsers;

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
            clientResponse = client.Get("");
            allUsers = JsonConvert.DeserializeObject<Dictionary<string, User>>(clientResponse.Body.ToString());
        }

        private void AdminPanelForm_Load(object sender, EventArgs e)
        {
            foreach (var user in allUsers)
            {
                addUserIntoList(user);
            }
            checkPeopleAndDisableAddButton();
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            enableOtherButtons();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (allUsersField.SelectedItem != null)
            {
                TrackDurationForm tdf = new TrackDurationForm();
                userFullName = allUsersField.SelectedItem.ToString();
                tdf.Show();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(allUsersField.SelectedItem != null)
            {
                EditUserForm euf = new EditUserForm();
                userFullName = allUsersField.SelectedItem.ToString();
                euf.Owner = this;
                euf.Show();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           AddUserForm auf = new AddUserForm();
           auf.Owner = this;
           auf.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            clientResponse = client.Get("");
            allUsers = JsonConvert.DeserializeObject<Dictionary<string, User>>(clientResponse.Body.ToString());
            if (allUsersField.SelectedItem != null)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure to remove " + allUsersField.SelectedItem.ToString() + " from the CPS system?", "Remove User", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    var user = allUsers.FirstOrDefault(x => x.Value.FullName.Equals(allUsersField.SelectedItem.ToString()));
                    if (user.Value.AuthorizationLevelId == (int)AuthorizationTypes.Employer)
                    {
                        MessageBox.Show("Employer cannot be deleted.");
                    }
                    else
                    {
                        var userForEdit = new User
                        {
                            Id = user.Value.Id,
                            AuthorizationLevelId = user.Value.AuthorizationLevelId,
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
                        allUsersField.Items.Remove(allUsersField.SelectedItem.ToString());
                        MessageBox.Show("User deleted.");
                        checkPeopleAndDisableAddButton();
                        (this.Owner as Form1).removeUserFieldIfUserRemoved(userForEdit.FullName);
                    }
                }
            }
        }

        private void DurationResetButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to reset ALL users' ALL durations? \n\nThis includes daily, weekly and monthly away durations and this operation is irreversible!", "One-Click Duration Reset", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                foreach (var user in allUsers)
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
                        isDeleted = user.Value.isDeleted
                    };
                    client.UpdateAsync("/User" + userForEdit.Id, userForEdit);
                }
                MessageBox.Show("All users' daily, weekly and monthly away durations had been reset.");
            }
        }

        private void addUserIntoList(KeyValuePair<string, User> user)
        {
            if (!user.Value.isDeleted)
                allUsersField.Items.Add(user.Value.FullName);
        }

        private void checkPeopleAndDisableAddButton()
        {
            if (allUsersField.Items.Count >= MAX_PEOPLE) // If members are full, cannot add more (24 for this CPS)
            {
                addUserButton.Enabled = false;
            }
            else
            {
                addUserButton.Enabled = true;
            }
        }
        private void enableOtherButtons()
        {
            trackDurationButton.Enabled = true;
            removeUserButton.Enabled = true;
            editUserButton.Enabled = true;
        }
    }
}
