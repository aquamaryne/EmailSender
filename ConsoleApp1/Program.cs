using System;
using System.Net.Mail;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
class Programm
{
    static void Main(string[] args)
    {
        Console.WriteLine("Program is starting");

        string username = "";       //set you google username (corporate)
        string pass = "";           //set you password from gamil
        string subject = "";        //set your mail title
        string body = "";           //set your mail body

        string[] addresses = File.ReadAllLines("");  //using for sent multiple addresses
        SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
        client.EnableSsl = true;
        client.Credentials = new NetworkCredential(username, pass);

        int emailCount = 0;
        List<string> sentAddresses = new List<string>();
        Console.Write($"Emails sent: {emailCount}");
        Console.WriteLine();

        foreach (string address in addresses)
        {
            MailMessage message = new MailMessage(username, address, subject, body);
            try
            {
                client.Send(message);
                emailCount++;
                sentAddresses.Add(address);
                Console.SetCursorPosition(0, 0);
                Console.Write($"Emails sent to: {emailCount}; ");
                Console.WriteLine("");
                foreach(string sentAddress in sentAddresses)
                {
                    Console.WriteLine($" - {sentAddress}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending mail to " + address + ": " + ex.Message);
            }

            Thread.Sleep(5000);
        }
    }
}
