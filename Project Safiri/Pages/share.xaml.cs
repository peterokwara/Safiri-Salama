using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace Project_Safiri.Pages
{
    public partial class share : PhoneApplicationPage
    {
        public share()
        {
            InitializeComponent();
        }

        private void Email_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.Subject = "";
            emailComposeTask.Body = "";
            emailComposeTask.To = "";
            emailComposeTask.Cc = "";
            emailComposeTask.Bcc = "";

            emailComposeTask.Show();
        }

        private void Messaging_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SmsComposeTask smsComposeTask = new SmsComposeTask();

            smsComposeTask.To = "";
            smsComposeTask.Body = "";
            smsComposeTask.Show();
        }
    }
}