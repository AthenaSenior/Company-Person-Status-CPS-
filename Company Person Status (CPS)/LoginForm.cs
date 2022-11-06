using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;


namespace Company_Person_Status__CPS_
{
    public partial class LoginForm : Form
    {
        // Variables
        int iteration;

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret= "krI2GcYbBhPh2KkQ3TfoLb4I6LEXYlvt6HMosQZP",
            BasePath= "https://cps-ankamee-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;

        // Constructor
        public LoginForm() 
        {
            InitializeComponent();
        }

        // Check Internet Connection
        private void LoginForm_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
            }
            catch
            {
                MessageBox.Show("No internet connection found. Please contact with your employer.");
            }
        }

        private void button1_Click(object sender, EventArgs e) // Check User and Login
        {
            iteration = 0;
            FirebaseResponse clientResponse = client.Get("");
            Dictionary<string, User> allUsers = JsonConvert.DeserializeObject<Dictionary<string,User>>(clientResponse.Body.ToString());

            foreach (var user in allUsers)
            {
                    if (textBox1.Text.Equals(user.Value.Username) && textBox2.Text.Equals(user.Value.Password))
                    {
                        label3.Visible = false;
                        this.Close();

                        var loggedInUser = new User
                        {
                            AuthorizationLevelId = user.Value.AuthorizationLevelId,
                            AwayFor = user.Value.AwayFor,
                            FullName = user.Value.FullName,
                            Password = user.Value.Password,
                            StatusId = (int)StatusTypes.Online,
                            ThisMonthAwayDuration = user.Value.ThisMonthAwayDuration,
                            ThisWeekAwayDuration = user.Value.ThisWeekAwayDuration,
                            TodaysAwayDuration = user.Value.TodaysAwayDuration,
                            Username = user.Value.Username
                        };

                        client.UpdateTaskAsync(user.Value.FullName, loggedInUser);
                        (this.Owner as Form1).panel2.Visible = true;
                        (this.Owner as Form1).userLoggedIn(loggedInUser);
                        break;
                    }
                    else
                    {
                        if (iteration == allUsers.Count - 1)
                        {
                            label3.Visible = true;
                        }
                    }
                }
                iteration++;
            }
        }
    }
