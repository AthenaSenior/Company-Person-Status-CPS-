using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace Company_Person_Status__CPS_
{
    public partial class Form1 : Form
    {
        // Variables
        public User loggedInUser;
        public static string userFullName = "";
        private readonly int inactivityConstant = 600;
        private int index, awayTime = 0, hour, minute, seconds, inactivityTime = 0;
        IEnumerable<Label> states, employeeNames;
        IEnumerable<PictureBox> userIcons;

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

            this.FormClosing += OnClosing;

            clientResponse = client.Get("");
            allUsers = JsonConvert.DeserializeObject<Dictionary<string, User>>(clientResponse.Body.ToString());

            InitializeComponent();
            timer1.Interval = 1000;
            timer2.Interval = 10000;
            states = panel5.Controls.OfType<Label>().Where(label => label.Name.StartsWith("status"));
            employeeNames = panel5.Controls.OfType<Label>().Where(label => label.Name.StartsWith("name"));
            userIcons = panel5.Controls.OfType<PictureBox>().Where(label => label.Name.StartsWith("pictureBox"));
        }


        // Methods

        private void mouseHook_MouseMoveEvent(object sender, MouseEventArgs e) // If mouse makes an activity.
        {
            stopInactivityTimer();
            if (label3.Text == "Away")
            {
                clientResponse = client.Get("");
                allUsers = JsonConvert.DeserializeObject<Dictionary<string, User>>(clientResponse.Body.ToString());
                var user = allUsers.FirstOrDefault(x => x.Value.FullName.Equals(label6.Text));
                changeStatus(user, (int)StatusTypes.Online, awayTime); // Polymorphism - Function Overloading
                stopTimer(); // Makes awayTime zero again.
                label3.Text = "Online";
                label3.ForeColor = Color.LightGreen;
                ControlButton.BackColor = Color.DarkOrange;
                ControlButton.Text = "Go Away";
                ControlButton.ForeColor = Color.Black;
                label4.Visible = false;
            }
        }

        private void mouseHook_MouseInactivityEvent(object sender, MouseEventArgs e) // If mouse stays inactive.
        {
            startInactivityTimer();
        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            if (loggedInUser != null)
            {
                clientResponse = client.Get("");
                allUsers = JsonConvert.DeserializeObject<Dictionary<string, User>>(clientResponse.Body.ToString());
                var user = allUsers.FirstOrDefault(x => x.Value.FullName.Equals(label6.Text));
                changeStatus(user, (int)StatusTypes.Offline, awayTime);
                label3.ForeColor = Color.Red;
                label3.Text = "Offline";
                label4.Visible = false;
                stopTimer();
                stopInactivityTimer();
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
                GlobalMouseHandler.MouseMovedEvent += mouseHook_MouseMoveEvent;
                GlobalMouseHandler.MouseInactivityEvent += mouseHook_MouseInactivityEvent;
                Application.AddMessageFilter(new GlobalMouseHandler());
                label3.ForeColor = Color.LightGreen;
                label3.Text = "Online";
                ControlButton.BackColor = Color.DarkOrange;
                ControlButton.ForeColor = Color.Black;
                ControlButton.Text = "Go Away";

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
                        ControlButton.Text = "Go Online";
                        ControlButton.ForeColor = Color.White;
                        label4.Visible = true;
                        var user = allUsers.FirstOrDefault(x => x.Value.FullName.Equals(label6.Text));
                        changeStatus(user, (int)StatusTypes.Away);
                        break;
                    }
                case "Away":
                    {
                        var user = allUsers.FirstOrDefault(x => x.Value.FullName.Equals(label6.Text));
                        changeStatus(user, (int)StatusTypes.Online, awayTime);
                        stopTimer(); // Makes awayTime zero again.
                        label3.Text = "Online";
                        label3.ForeColor = Color.LightGreen;
                        ControlButton.BackColor = Color.DarkOrange;
                        ControlButton.Text = "Go Away";
                        ControlButton.ForeColor = Color.Black;
                        label4.Visible = false;
                        break;
                    }
            }
        }

        private void AdminPanelButton_Click(object sender, EventArgs e)
        {
            AdminPanelForm apf = new AdminPanelForm();
            apf.Owner = this;
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
            index = 0;

            if (loggedInUser != null)
                label6.Text = allUsers.FirstOrDefault(x => x.Value.Id.Equals(loggedInUser.Id)).Value.FullName;

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
                            {
                                if (index < allUsers.Where(user => !user.Value.isDeleted).Count())
                                {
                                    userIcons.ElementAt(index).BackgroundImage = Properties.Resources.no_image;
                                    employeeNames.ElementAt(index).Text = user.Value.FullName;
                                    printStatus(states.ElementAt(index), user.Value.StatusId);
                                    index++;
                                }
                                break;
                            }
                    }
                }
            }
        }

        private void infoButton_Click(object sender, EventArgs e)
        {
            userFullName = label6.Text;
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
                FullName = user.Value.FullName,
                Password = user.Value.Password,
                StatusId = newStatusId,
                ThisMonthAwayDuration = user.Value.ThisMonthAwayDuration + awayTime - 1,
                ThisWeekAwayDuration = user.Value.ThisWeekAwayDuration + awayTime - 1,
                TodaysAwayDuration = user.Value.TodaysAwayDuration + awayTime - 1,
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

        private void startInactivityTimer()
        {
            timer2.Start();
            timer2.Tick += new EventHandler(timer2_Tick);
        }

        private void stopInactivityTimer()
        {
            timer2.Stop();
            timer2.Tick -= new EventHandler(timer2_Tick);
            inactivityTime = 0;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            inactivityTime++;

            if (inactivityTime == inactivityConstant && label3.Text == "Online")
            {
                clientResponse = client.Get("");
                allUsers = JsonConvert.DeserializeObject<Dictionary<string, User>>(clientResponse.Body.ToString());
                var user = allUsers.FirstOrDefault(x => x.Value.FullName.Equals(label6.Text));
                changeStatus(user, (int)StatusTypes.Away);
                startTimer();
                label4.Text = "for 00:00:00";
                label3.Text = "Away";
                label3.ForeColor = Color.DarkOrange;
                ControlButton.BackColor = Color.Green;
                ControlButton.Text = "Go Online";
                ControlButton.ForeColor = Color.White;
                label4.Visible = true;
            }
        }

        public void removeUserFieldIfUserRemoved(string username)
        {
            Label removedEmployeeName = employeeNames.FirstOrDefault(x => x.Text.Equals(username));
            int index = employeeNames.ToList().IndexOf(removedEmployeeName);
            userIcons.ElementAt(index).BackgroundImage = null;
            states.ElementAt(index).ResetText();
            employeeNames.ElementAt(index).ResetText();
        }
    }

    // New Class

    public class GlobalMouseHandler : IMessageFilter
    {
        private Point previousMousePosition = new Point();
        public static event EventHandler<MouseEventArgs> MouseMovedEvent = delegate { };
        public static event EventHandler<MouseEventArgs> MouseInactivityEvent = delegate { };

        #region IMessageFilter Members

        public bool PreFilterMessage(ref Message m)
        {
            Point currentMousePoint = Control.MousePosition;
            if (previousMousePosition.X != currentMousePoint.X || previousMousePosition.Y != currentMousePoint.Y)
            {
                previousMousePosition = currentMousePoint;
                MouseMovedEvent(this, new MouseEventArgs(MouseButtons.None, 0, currentMousePoint.X, currentMousePoint.Y, 0));
            }
            else
            {
                MouseInactivityEvent(this, new MouseEventArgs(MouseButtons.None, 0, currentMousePoint.X, currentMousePoint.Y, 0));
            }
            return false;
        }
        #endregion
    }
}

