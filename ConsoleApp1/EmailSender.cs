using System;
using System.Net.Mail;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Collections.Generic;


class Programm
{
    private const string GmailUsernameKey = "GmailUsername";
    private const string GmailPasswordKey = "GmailPassword";
    private const string EmailSubject = "EmailSubject";
    private const string EmailBodyKey = "EmailBody";
    private const string AddressFilePathKey = "AddressFilePath";
    private const string AttachmentFilePathAway = "AttachmnetFilePath";

    private const string SmtpServer = "smtp.gmail.com";
    private const int smtpPort = 587;

    static void Main(string[] args)
    {
        Console.Write("Enter your Gmail username: ");
        string username = Console.ReadLine();

        Console.Write("Enter your Gmail password: ");
        string password = ReadPassword();

        Console.Write("Enter the mail subject: ");
        string subject = Console.ReadLine();

        Console.Write("Enter the mail body: ");
        string body = Console.ReadLine();

        Console.Write("Enter the address file path (CSV file): ");
        string addressesFilePath = Console.ReadLine();

        Console.Write("Enter the attachment file path (optinal): ");
        string attachmentFilePath = Console.ReadLine();


        if (string.IsNullOrEmpty(username)
            || string.IsNullOrEmpty(password)
            || string.IsNullOrEmpty(subject)
            || string.IsNullOrEmpty(body)
            || string.IsNullOrEmpty(attachmentFilePath)
            || string.IsNullOrEmpty(addressesFilePath))
        {
            Console.WriteLine("Please enter all required values. ");
            return;
        }

        string[] addresses = ReadAddressFromCsv(addressesFilePath);

        using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
        {
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(username, password);

            int emailCount = 0;
            List<string> sentAddresses = new List<string>();

            Console.WriteLine($"Emails sent: {emailCount}; ");
            Console.WriteLine();

            foreach (string address in addresses)
            {
                try
                {
                    MailMessage message = CreateEmailMessage(username, address, subject, body, attachmentFilePath);

                    client.Send(message);
                    emailCount++;
                    sentAddresses.Add(address);
                    Console.SetCursorPosition(0, 0);
                    Console.Write($"Emails sent to: {emailCount}; ");
                    Console.WriteLine("");
                    foreach (string sentAddress in sentAddresses)
                    {
                        Console.WriteLine($" - {sentAddress}");
                    }
                }
                catch (SmtpException ex) { Console.WriteLine($"Error sending mail {address}: {ex.Message}"); }
            }

            Thread.Sleep(2000);
        }
    }

    static string[] ReadAddressFromCsv(string filePath)
    {
        return File.ReadAllLines(filePath);
    }

    static MailMessage CreateEmailMessage(string from, string to, string subject, string body, string addtachmentfilePath)
    {
        MailMessage message = new MailMessage(from, to, subject, body);

        if (!string.IsNullOrEmpty(addtachmentfilePath))
        {
            Attachment attachment = new Attachment(addtachmentfilePath);
            message.Attachments.Add(attachment);
        }

        return message;
    }

    static string ReadPassword()
    {
        string password = "";
        ConsoleKeyInfo key;
        do
        {
            key = Console.ReadKey(true);

            if(key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
            {
                password += key.KeyChar;
                Console.Write("*");
            }
            else if(key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, password.Length - 1);
                Console.Write("\b \b");
            }
        } while (key.Key != ConsoleKey.Enter);

        Console.WriteLine("");

        return password;
    }
}
