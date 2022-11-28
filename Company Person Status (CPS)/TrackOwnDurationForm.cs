using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Company_Person_Status__CPS_
{
    public partial class TrackOwnDurationForm : Form
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "krI2GcYbBhPh2KkQ3TfoLb4I6LEXYlvt6HMosQZP",
            BasePath = "https://cps-ankamee-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;
        FirebaseResponse clientResponse;
        Dictionary<string, User> allUsers;

        public TrackOwnDurationForm()
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

        private void TrackOwnDurationForm_Load(object sender, EventArgs e)
        {
            clientResponse = client.Get("");
            allUsers = JsonConvert.DeserializeObject<Dictionary<string, User>>(clientResponse.Body.ToString());
            label1.Text = Form1.userFullName;
            var user = allUsers.FirstOrDefault(x => x.Value.FullName.Equals(label1.Text));
            convertDailyAwaySeconds(user.Value.TodaysAwayDuration);
            setAuthorityText(user.Value.AuthorizationLevelId);
            printStatus(user.Value.StatusId);
        }

        private void setAuthorityText(int authorizationLevelId)
        {
            switch (authorizationLevelId)
            {
                case (int)AuthorizationTypes.Manager:
                    {
                        label4.Text = "Manager";
                        break;
                    }
                case (int)AuthorizationTypes.CEO:
                    {
                        label4.Text = "CEO";
                        break;
                    }
                case (int)AuthorizationTypes.Employer:
                    {
                        label4.Text = "Boss";
                        break;
                    }
                default:
                    {
                        label4.Text = "Employee";
                        break;
                    }
            }
        }

        private void printStatus(int statusId)
        {
            switch (statusId)
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

        private void convertDailyAwaySeconds(int dailyAwayDuration)
        {
            DailyHour.Text = (dailyAwayDuration / 3600).ToString();
            DailyMinute.Text = ((dailyAwayDuration % 3600) / 60).ToString();
            DailySecond.Text = ((dailyAwayDuration % 3600) % 60).ToString();
        }

    }
}
