﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string FROM_EMAIL = "email@gmail.com";
        private string FROM_PASS = "emailPassword123!";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                fileName.Text = openFileDialog.FileName;
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            //setup
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential(FROM_EMAIL, FROM_PASS);
            SmtpServer.EnableSsl = true;

            MailMessage mail = new MailMessage();
            //default from email
            mail.From = new MailAddress(FROM_EMAIL);

            //email recipient
            string addresses = toEmail.Text;
            foreach (var address in addresses.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (address.Contains("@") == false || address.Contains(".com") == false)
                {
                    MessageBox.Show("Please enter actual email address");
                    return;
                }
                mail.To.Add(address);
            }
            //mail.CC.Add(new MailAddress("MyEmailID@gmail.com"));

            //email subject
            mail.Subject = subjectContent.Text;

            //email attachments
            if (string.IsNullOrEmpty(fileName.Text) == false)
            {
                mail.Attachments.Add(new Attachment(fileName.Text));
            }

            //email body
            mail.Body = emailContent.Text;

            //send email
            MessageBox.Show("Sending...\nPlease close this and wait for sent confirmation.");
            SmtpServer.Send(mail);
            MessageBox.Show("Sent");

            //refresh text boxes
            toEmail.Text = "";
            subjectContent.Text = "";
            fileName.Text = "";
            emailContent.Text = "\n\nSent from wpf app";
        }
    }
}
