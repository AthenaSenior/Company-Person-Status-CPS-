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
    public partial class TrackDurationForm : Form
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "krI2GcYbBhPh2KkQ3TfoLb4I6LEXYlvt6HMosQZP",
            BasePath = "https://cps-ankamee-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;
        public TrackDurationForm()
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

        private void TrackDurationForm_Load(object sender, EventArgs e)
        {
            label1.Text = AdminPanelForm.userFullName;
            FirebaseResponse clientResponse = client.Get("");
            Dictionary<string, User> allUsers = JsonConvert.DeserializeObject<Dictionary<string, User>>(clientResponse.Body.ToString());
            foreach (var user in allUsers)
            {
                if (label1.Text.Equals(user.Value.FullName))
                {
                    switch(user.Value.StatusId)
                    {
                        case (int)StatusTypes.Offline:
                            {
                                label3.ForeColor = Color.Red;
                                label3.Text = "OFFLINE";
                                break;
                            }
                        case (int)StatusTypes.Online:
                            {
                                label3.ForeColor = Color.Green;
                                label3.Text = "ONLINE";
                                break;
                            }
                        case (int)StatusTypes.Away:
                            {
                                label3.ForeColor = Color.Yellow;
                                label3.Text = "AWAY";
                                break;
                            }
                        default:
                            break;
                    }
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}
