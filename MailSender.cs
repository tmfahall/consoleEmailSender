using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace MailSender
{
   class Program
    {
        static void Main(string[] args)
        {
            //args[0camId, 1camName, 2camDir]

            Console.WriteLine(args[0]);
            Console.WriteLine(args[1]);
            Console.WriteLine(args[2]);

            DateTime alertDate = DateTime.Now;

            MailMessage mail = new MailMessage();

            mail.To.Add("TO@EMAIL.COM"); // ENTER EMAIL ADDRESS WHERE YOU WANT NOTIFICATIONS SENT TO. MULTIPLE ADDRESSES CAN BE ADDED BY FOLLOWING THIS FORMAT.

            mail.From = new MailAddress("FROM@EMAIL.COM"); // ENTER YOUR EMAIL ADDRESS

            mail.Subject = "Motion Detected On Camera " + args[0] + " - " + args[1];

            mail.Body = "Motion was detected on camera " + args[0] + " - " + args[1] + " on " + alertDate.ToString("dd-MM-yyyy") + " at " + alertDate.ToString("hh:mm:ss");
			
            FileInfo result = null;
            var camDirectory = new DirectoryInfo (string.Format("D:\\video\\{0}\\grabs", args[2]));
			//CHANGE THE FIRST PART TO MATCH YOUR SYSTEM 
			//(EXAMPLE C:\\USERS\\ADMINISTRATOR\\APPDATA\\ISPY\\VIDEO\\{0}\\GRABS) 
			//THE LAST PART "\\{0}\\GRABS" NEEDS TO STAY THE SAME
			
            var list = camDirectory.GetFiles("*.jpg");
			//CREATES LIST OF ALL .JPG FILES
			
            if (list.Count() > 0)
            {
                result = list.OrderByDescending(f => f.LastWriteTime).First();
            }
			//IF THERE ARE ANY FILES, SORT THEM DESCENDING BY WRITE TIME AND TAKE THE FIRST ONE (THE MOST RECENT)
			
            var attachmentString = result.FullName;
			//GET PATH OF MOST RECENT FILE
			
            mail.Attachments.Add(new Attachment(attachmentString));
			//ADD MOST RECENT FILE


            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com"; //IF NOT USING GMAIL CHANGE THIS
            smtp.Port = 587;

            smtp.Credentials = new NetworkCredential("FROM@EMAIL.COM", "PASSWORD"); //ENTER YOUR USER NAME AND PASSWORD
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
    }
}
