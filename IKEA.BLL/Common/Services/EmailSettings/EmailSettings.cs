using IKEA.DAL.Models.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Common.Services.EmailSettings
{
	public class EmailSettings : IEmailSettings
	{
		public void SendEmail(Email email)
		{
			var client = new SmtpClient("smtp.gmail.com", 587);
			client.EnableSsl = true;
			//Sender-Reciver
			client.Credentials = new NetworkCredential("abdosaad2480@gmail.com", "jxqvrjlvjracenjn");//Generite Password
			client.Send("abdosaad2480@gmail.com", email.To, email.Subject, email.Body);
		}
	}
}
