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
        FirebaseResponse clientResponse;
        Dictionary<string, User> allUsers;

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
            clientResponse = client.Get("");
            allUsers = JsonConvert.DeserializeObject<Dictionary<string, User>>(clientResponse.Body.ToString());
        }

        private void TrackDurationForm_Load(object sender, EventArgs e)
        {
            label1.Text = AdminPanelForm.userFullName;
            foreach (var user in allUsers)
            {
                if (label1.Text.Equals(user.Value.FullName))
                {
                    convertDailyAwaySeconds(user.Value.TodaysAwayDuration);
                    convertWeeklyAwaySeconds(user.Value.ThisWeekAwayDuration); 
                    convertMonthlyAwaySeconds(user.Value.ThisMonthAwayDuration);
                    printStatus(user.Value.StatusId);
                }
            }
        }

        private void convertDailyAwaySeconds(int seconds)
        {
            DailyHour.Text = (seconds / 3600).ToString();
            DailyMinute.Text = ((seconds % 3600) / 60).ToString();
            DailySecond.Text = ((seconds % 3600) % 60).ToString();
        }

        private void convertWeeklyAwaySeconds(int seconds)
        {
            WeeklyHour.Text = (seconds / 3600).ToString();
            WeeklyMinute.Text = ((seconds % 3600) / 60).ToString();
            WeeklySecond.Text = ((seconds % 3600) % 60).ToString();
        }

        private void convertMonthlyAwaySeconds(int seconds)
        {
            MonthlyHour.Text = (seconds / 3600).ToString();
            MonthlyMinute.Text = ((seconds % 3600) / 60).ToString();
            MonthlySecond.Text = ((seconds % 3600) % 60).ToString();
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
    }
}
