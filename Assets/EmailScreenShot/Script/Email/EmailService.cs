using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.IO;

public class EmailService  {
	private string _email, _password, _name, _stmpAddress;
	private int _port;

	public EmailService(string email, string password, string name) {
		this._email = email;
		this._password = password;
		this._name = name;
		this._stmpAddress = "smtp.gmail.com";
		this._port = 587;
	}

	public void Send(string p_subject, string p_content, string[] p_targetEmails, string[] p_attachmentDir) {
		MailMessage mail = CreateMail(p_subject, p_content, _email, _name, p_targetEmails, p_attachmentDir);
		SmtpClient smtp = CreateSMTP(_email, _password, _stmpAddress);

		smtp.SendAsync(mail, "user states . . .");
	}

	private MailMessage CreateMail(string p_subject, string p_content, string p_email, string p_name, 
		string[] p_targetEmails, string[] attachmentPics) {
		MailMessage mail = new MailMessage();
		
        mail.From = new MailAddress (p_email, p_name, System.Text.Encoding.UTF8);
		
		//Add target
		foreach (string targetEmail in p_targetEmails) {
			mail.To.Add (targetEmail);
		}
		
		//Add attachment
		foreach (string attachment in attachmentPics) {
			if (File.Exists(attachment)) {
				mail.Attachments.Add (new Attachment (@attachment) );
			}
		}

        mail.Subject = p_subject;
        mail.Body = p_content;
        mail.SubjectEncoding = System.Text.Encoding.UTF8;
        mail.BodyEncoding = System.Text.Encoding.UTF8;
        mail.Priority = MailPriority.High;

		return mail;
	}

	private SmtpClient CreateSMTP(string p_email, string p_password, string p_smtpAddress) {
		    SmtpClient smtpServer = new SmtpClient(p_smtpAddress);
			smtpServer.Port = 587;
			smtpServer.Credentials = new System.Net.NetworkCredential (p_email, p_password) as ICredentialsByHost;
			smtpServer.EnableSsl = true;
			ServicePointManager.ServerCertificateValidationCallback = 
				delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) 
			{ return true; };

		return smtpServer;
	}
	
}
