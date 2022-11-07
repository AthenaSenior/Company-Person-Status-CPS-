using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Company_Person_Status__CPS_
{
    public partial class AddUserForm : Form
    {
        private int userCount = 0;
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
        }

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "krI2GcYbBhPh2KkQ3TfoLb4I6LEXYlvt6HMosQZP",
            BasePath = "https://cps-ankamee-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;

        private void button1_Click(object sender, EventArgs e)
        {
            var newUser = new User
            {
                Id = userCount + 1,
                AuthorizationLevelId = comboBox1.SelectedIndex + 1,
                AwayFor = 0,
                FullName = textBox3.Text,
                Password = textBox1.Text,
                StatusId = (int)StatusTypes.Offline,
                ThisMonthAwayDuration = 0,
                ThisWeekAwayDuration = 0,
                TodaysAwayDuration = 0,
                Username = textBox2.Text,
            };
            client.SetTaskAsync("/User"+newUser.Id, newUser);
            (this.Owner as AdminPanelForm).listBox1.Items.Add(newUser.FullName);
            MessageBox.Show("User added to the system.");
            this.Close();
        }

        private void AddUserForm_Load(object sender, EventArgs e)
        {
            FirebaseResponse clientResponse = client.Get("");
            Dictionary<string, User> allUsers = JsonConvert.DeserializeObject<Dictionary<string, User>>(clientResponse.Body.ToString());
            foreach (var user in allUsers)
            {
                userCount++;
            }
        }
    }
}
