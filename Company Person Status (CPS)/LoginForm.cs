﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System.Linq;

namespace Company_Person_Status__CPS_
{
    public partial class LoginForm : Form
    {
        // Variables
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "krI2GcYbBhPh2KkQ3TfoLb4I6LEXYlvt6HMosQZP",
            BasePath = "https://cps-ankamee-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;
        FirebaseResponse clientResponse;
        Dictionary<string, User> allUsers;

        public LoginForm()
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

        // Methods

        private void button1_Click(object sender, EventArgs e) // Check User and Login
        {
            var user = allUsers.FirstOrDefault(x => x.Value.Username.Equals(textBox1.Text) && x.Value.Password.Equals(textBox2.Text) && !x.Value.isDeleted);
            if(user.Key != null)
            {
                logInToSystem(user);
            }
            else
            {
                label3.Visible = true; // Error message to user.
            }
        }
        private void logInToSystem(KeyValuePair<string, User> user)
        {
            var loggedInUser = new User
            {
                Id = user.Value.Id,
                AuthorizationLevelId = user.Value.AuthorizationLevelId,
                FullName = user.Value.FullName,
                Password = user.Value.Password,
                StatusId = (int)StatusTypes.Online,
                ThisMonthAwayDuration = user.Value.ThisMonthAwayDuration,
                ThisWeekAwayDuration = user.Value.ThisWeekAwayDuration,
                TodaysAwayDuration = user.Value.TodaysAwayDuration,
                Username = user.Value.Username,
                isDeleted = false
            };
            client.UpdateAsync("/User" + loggedInUser.Id, loggedInUser);
            (this.Owner as Form1).panel2.Visible = true;
            (this.Owner as Form1).userLoggedIn(loggedInUser);
            label3.Visible = false;
            this.Close();
        }
    }
}
