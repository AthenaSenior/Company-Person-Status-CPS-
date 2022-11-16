using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using FireSharp.EventStreaming;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Timers;

namespace Company_Person_Status__CPS_
{
    public partial class Form1 : Form
    {
        // Variables
        public User loggedInUser;
        public static string userFullName = "";
        private int index, userIndex;
        IEnumerable<Label> states, employeeNames;
        IEnumerable<PictureBox> userIcons;
        int awayTime = 0, hour, minute, seconds;

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "krI2GcYbBhPh2KkQ3TfoLb4I6LEXYlvt6HMosQZP",
            BasePath = "https://cps-ankamee-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;
        FirebaseResponse clientResponse;
        Dictionary<string, User> allUsers;

        public Form1()
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
            pictureBox2.BackColor = Color.Transparent;
            pictureBox1.BackColor = Color.Transparent;
            button1.BackColor = Color.Transparent;
            button2.BackColor = Color.Transparent;
            button3.BackColor = Color.Transparent;
            button1.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button1.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button2.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button2.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button3.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button3.FlatAppearance.MouseOverBackColor = Color.Transparent;
            this.Closing += OnClosing;
            states = panel5.Controls.OfType<Label>()
                .Where(label => label.Name.StartsWith("status"));
            employeeNames = panel5.Controls.OfType<Label>()
                .Where(label => label.Name.StartsWith("name"));
            userIcons = panel5.Controls.OfType<PictureBox>()
                .Where(label => label.Name.StartsWith("pictureBox"));
            clientResponse = client.Get("");
            allUsers = JsonConvert.DeserializeObject<Dictionary<string, User>>(clientResponse.Body.ToString());
            timer1.Interval = 1000;
            GlobalMouseHandler.MouseMovedEvent += GlobalMouseHandler_MouseMovedEvent;
            Application.AddMessageFilter(new GlobalMouseHandler());
        }

        // Methods
        private void GlobalMouseHandler_MouseMovedEvent(object sender, MouseEventArgs e) // If mouse makes an activity.
        {
            if(label3.Text == "Away") { 
                clientResponse = client.Get("");
                allUsers = JsonConvert.DeserializeObject<Dictionary<string, User>>(clientResponse.Body.ToString());
                foreach (var user in allUsers)
                {
                    if (user.Value.FullName.Equals(loggedInUser.FullName))
                    {
                        changeStatus(user, (int)StatusTypes.Online, awayTime); // Polymorphism - Function Overloading
                        break;
                    }
                }
                stopTimer(); // Makes awayTime zero again.
                label3.Text = "Online";
                label3.ForeColor = Color.LightGreen;
                ControlButton.BackColor = Color.DarkOrange;
                ControlButton.Text = "            Go Away";
                ControlButton.ForeColor = Color.Black;
                label4.Visible = false;
        }
    }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            if (loggedInUser != null)
            {
                foreach (var user in allUsers)
                {
                    if (user.Value.FullName.Equals(loggedInUser.FullName))
                    {
                        changeStatus(user, (int)StatusTypes.Offline);
                        break;
                    }
                }
                label3.ForeColor = Color.Red;
                label3.Text = "Offline";
                MessageBox.Show("You exited from the system and your status become offline.", "Quit");  //This is important. Do not erase this otherwise system does not update the status in db
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Owner = this;
            loginForm.ShowDialog();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Help help = new Help();
            help.ShowDialog();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void userLoggedIn(User user)
        {
            loggedInUser = user;
            if (loggedInUser != null)
            {
                label6.Text = loggedInUser.FullName;
                label3.ForeColor = Color.LightGreen;
                label3.Text = "Online";
                ControlButton.BackColor = Color.DarkOrange;
                ControlButton.ForeColor = Color.Black;
                ControlButton.Text = "            Go Away";

                switch (loggedInUser.AuthorizationLevelId)
                {
                    case (int)AuthorizationTypes.Employer:
                        {
                            AdminPanelButton.Text = "  Admin Panel";
                            AdminPanelButton.Visible = true;
                            break;
                        }
                    default:
                        {
                            AdminPanelButton.Visible = false;
                            break;
                        }
                }
            }
        }

        private void ControlButton_Click(object sender, EventArgs e)
        {
            clientResponse = client.Get("");
            allUsers = JsonConvert.DeserializeObject<Dictionary<string, User>>(clientResponse.Body.ToString());
            switch (label3.Text)
            {
                case "Online":
                    {
                        startTimer();
                        label4.Text = "for 00:00:00";
                        label3.Text = "Away";
                        label3.ForeColor = Color.DarkOrange;
                        ControlButton.BackColor = Color.Green;
                        ControlButton.Text = "          Go Online";
                        ControlButton.ForeColor = Color.White;
                        label4.Visible = true;
                        foreach (var user in allUsers)
                        {
                            if (user.Value.FullName.Equals(loggedInUser.FullName))
                            {
                                changeStatus(user, (int)StatusTypes.Away);
                                break;
                            }
                        }
                        break;
                    }
                case "Away":
                    {
                        foreach (var user in allUsers)
                        {
                            if (user.Value.FullName.Equals(loggedInUser.FullName))
                            {
                                changeStatus(user, (int)StatusTypes.Online, awayTime); // Polymorphism - Function Overloading
                                break;
                            }
                        }
                        stopTimer(); // Makes awayTime zero again.
                        label3.Text = "Online";
                        label3.ForeColor = Color.LightGreen;
                        ControlButton.BackColor = Color.DarkOrange;
                        ControlButton.Text = "            Go Away";
                        ControlButton.ForeColor = Color.Black;
                        label4.Visible = false;
                        break;
                    }
            }
        }

        private void AdminPanelButton_Click(object sender, EventArgs e)
        {
            AdminPanelForm apf = new AdminPanelForm();
            apf.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            getLiveData();
        }

        async void getLiveData()
        {
            while (true)
            {
                FirebaseResponse clientResponse = await client.GetAsync("");
                Dictionary<string, User> allUsers = JsonConvert.DeserializeObject<Dictionary<string, User>>(clientResponse.Body.ToString());
                updateScreen(allUsers);
            }
        }

        private void updateScreen(Dictionary<string, User> allUsers)
        {
            index = userIcons.Count() - 1; // Local variables
            userIndex = 20;

            foreach (var user in allUsers)
            {
                if (!user.Value.isDeleted)
                {
                    switch (user.Value.AuthorizationLevelId)
                    {
                        case (int)AuthorizationTypes.Employer:
                            {
                                Employer.Text = user.Value.FullName;
                                printStatus(EmployerStatus, user.Value.StatusId);
                                break;
                            }
                        case (int)AuthorizationTypes.CEO:
                            {
                                CEO.Text = user.Value.FullName;
                                printStatus(CEOStatus, user.Value.StatusId);
                                break;
                            }
                        case (int)AuthorizationTypes.Manager:
                            {
                                Manager.Text = user.Value.FullName;
                                printStatus(ManagerStatus, user.Value.StatusId);
                                break;
                            }
                        default:
                            { if (index > (userIndex - allUsers.Count()))
                                {
                                    userIcons.ElementAt(index).Visible = true;
                                    employeeNames.ElementAt(index).Text = user.Value.FullName;
                                    printStatus(states.ElementAt(index), user.Value.StatusId);
                                    index--;
                                    userIndex++;
                                }
                                else if (index <= 15 && index >= 0)
                                {
                                    userIcons.ElementAt(index).Visible = true;
                                    employeeNames.ElementAt(index).Text = user.Value.FullName;
                                    printStatus(states.ElementAt(index), user.Value.StatusId);
                                    index--;
                                }
                                break;
                            }
                    }
                }
            }
        }

        private void infoButton_Click(object sender, EventArgs e)
        {
            userFullName = loggedInUser.FullName;
            TrackOwnDurationForm todf = new TrackOwnDurationForm();
            todf.Show();
        }

        private void printStatus(Label label, int statusId)
        {
            switch (statusId)
            {
                case (int)StatusTypes.Offline:
                    {
                        label.Text = "OFFLINE";
                        label.ForeColor = Color.Red;
                        break;
                    }
                case (int)StatusTypes.Online:
                    {
                        label.Text = "ONLINE";
                        label.ForeColor = Color.LightGreen;
                        break;
                    }
                case (int)StatusTypes.Away:
                    {
                        label.Text = " AWAY";
                        label.ForeColor = Color.DarkOrange;
                        break;
                    }
            }
        }

        // Overloading Function
        private void changeStatus(KeyValuePair<string, User> user, int newStatusId)
        {
            var userWithNewStatus = new User
            {
                Id = user.Value.Id,
                AuthorizationLevelId = user.Value.AuthorizationLevelId,
                AwayFor = user.Value.AwayFor,
                FullName = user.Value.FullName,
                Password = user.Value.Password,
                StatusId = newStatusId,
                ThisMonthAwayDuration = user.Value.ThisMonthAwayDuration,
                ThisWeekAwayDuration = user.Value.ThisWeekAwayDuration,
                TodaysAwayDuration = user.Value.TodaysAwayDuration,
                Username = user.Value.Username,
                isDeleted = false
            };
            client.UpdateAsync("/User" + userWithNewStatus.Id, userWithNewStatus);
        }

        private void changeStatus(KeyValuePair<string, User> user, int newStatusId, int awayTime)
        {
            var userWithNewStatus = new User
            {
                Id = user.Value.Id,
                AuthorizationLevelId = user.Value.AuthorizationLevelId,
                AwayFor = user.Value.AwayFor,
                FullName = user.Value.FullName,
                Password = user.Value.Password,
                StatusId = newStatusId,
                ThisMonthAwayDuration = user.Value.ThisMonthAwayDuration + awayTime,
                ThisWeekAwayDuration = user.Value.ThisWeekAwayDuration + awayTime,
                TodaysAwayDuration = user.Value.TodaysAwayDuration + awayTime,
                Username = user.Value.Username,
                isDeleted = false
            };
            client.UpdateAsync("/User" + userWithNewStatus.Id, userWithNewStatus);
        }

        private void startTimer()
        {
            timer1.Start();
            timer1.Tick += new EventHandler(timer1_Tick);
        }

        private void stopTimer()
        {
            timer1.Stop();
            timer1.Tick -= new EventHandler(timer1_Tick);
            awayTime = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            hour = awayTime / 3600;
            minute = (awayTime % 3600) / 60;
            seconds = (awayTime % 3600) % 60;
            label4.Text = "for " + hour.ToString("00") + ":" + minute.ToString("00") + ":" + seconds.ToString("00");
            awayTime++;
        }
    }
    public class GlobalMouseHandler : IMessageFilter
    {
        private const int WM_MOUSEMOVE = 0x0200;
        private System.Drawing.Point previousMousePosition = new System.Drawing.Point();
        public static event EventHandler<MouseEventArgs> MouseMovedEvent = delegate { };

        #region IMessageFilter Members

        public bool PreFilterMessage(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == WM_MOUSEMOVE)
            {
                System.Drawing.Point currentMousePoint = Control.MousePosition;
                if (previousMousePosition.X != currentMousePoint.X && previousMousePosition.Y != currentMousePoint.Y)
                {
                    previousMousePosition = currentMousePoint;
                    MouseMovedEvent(this, new MouseEventArgs(MouseButtons.None, 0, currentMousePoint.X, currentMousePoint.Y, 0));
                }
            }
            // Always allow message to continue to the next filter control
            return false;
        }

        #endregion
    }
}

