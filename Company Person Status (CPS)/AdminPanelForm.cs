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
    }
}
