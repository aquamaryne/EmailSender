using System;
using System.Net.Mail;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Collections.Generic;


class Programm
{
    static void Main(string[] args)
    {
        string username = GetConfigValue("GmailUsername");
        string password = GetConfigValue("GmailPassword");
        string subject = GetConfigValue("EmailSubject");
        string body = GetConfigValue("EmailBody");
        string addressesFilePath = GetConfigValue("AddressFilePath");

        string[] addresses = ReadAddressFromCsv(addressesFilePath);

        SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
        client.EnableSsl = true;
        client.Credentials = new NetworkCredential(username, password);

        int emailCount = 0;
        List<string> sentAddresses = new List<string>();

        Console.Write($"Emails sent: {emailCount}");
        Console.WriteLine();

        foreach(string address in addresses)
        {
            try
            {
                MailMessage message = CreateEmailMessage(username, address, subject, body, GetConfigValue("AttachmentFilePath"));

                client.Send(message);
                emailCount++;
                sentAddresses.Add(address);
                Console.SetCursorPosition(0, 0);
                Console.Write($"Emails sent to: {emailCount}; ");
                Console.WriteLine("");
                foreach(string sentAddress in sentAddresses)
                {
                    Console.Write($" - {sentAddress}");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error sending mail to " + address + ": " + ex.Message);
            }

            Thread.Sleep(2000);
        }
    }

    static string GetConfigValue(string key)
    {
        Dictionary<string, string> config = new Dictionary<string, string>()
        {
            {"GmailUsername", ""}, //set your google email
            {"GmailPassword", ""}, //set your google pass
            {"EmailSubject", ""},  //set your mail title
            {"EmailBody", ""},     //set your mail message
            {"AddressFilePath", "" }, //for using if you have multiple addresses in .csv file
            {"AttachmentFilePath", ""}, //set your image or another file
        };

        return config[key];
    }
    static string[] ReadAddressFromCsv(string filePath)
    {
        return File.ReadAllLines(filePath);
    }

    static MailMessage CreateEmailMessage(string from, string to, string subject, string body, string addtachmentfilePath)
    {
        MailMessage message = new MailMessage(from, to, subject, body);

        if(!string.IsNullOrEmpty(addtachmentfilePath))
        {
            Attachment attachment = new Attachment(addtachmentfilePath);
            message.Attachments.Add(attachment);
        }

        return message;
    }
}
