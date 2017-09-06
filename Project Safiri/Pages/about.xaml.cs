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
using Project_Safiri.Resources;

namespace Project_Safiri.Pages
{
    public partial class about : PhoneApplicationPage
    {
        public about()
        {
            InitializeComponent();
        }

        private void Rate_Click(object sender, RoutedEventArgs e)
        {
            RateMyApp.Helpers.FeedbackHelper.Default.Review();
        }

        private void contact_click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.Subject = "Safiri Salama Customer Feedback";
            emailComposeTask.Body = "";
            emailComposeTask.To = "peter.ojakaa@outlook.com";

            emailComposeTask.Show();
        }

        private void privacy_click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(AppResources.PrivacyText);
        }
    }
}