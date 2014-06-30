using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using Microsoft.SharePoint;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;

namespace ADUserInformation.Business_Layer
{
    public class MailSender
    {

        #region Public Email-SMS Methods

        public bool SendMail(string mailSubject, string contacts, string msgBody, string pEmailCc)
        {
            bool blMailSucceed = false;
            if (contacts != "")
            {
                try
                {
                    if (!contacts.Contains(","))
                        contacts += ",";

                    if (!pEmailCc.Contains(","))
                        pEmailCc += ",";

                    string mailHost = ConfigurationManager.AppSettings["MailHost"];
                    int mailPort = int.Parse(ConfigurationManager.AppSettings["MailPort"]);
                    string FromEmailID = ConfigurationManager.AppSettings["FromEmailID"];

                    string[] contactList = contacts.Split(',');
                    string[] strEmailCc = pEmailCc.Split(',');

                    MailMessage Message = new System.Net.Mail.MailMessage();
                    SmtpClient sClient = new System.Net.Mail.SmtpClient();
                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add("MIME-Version", "1.0");
                    nvc.Add("charset", "iso-8859-1");
                    nvc.Add("From", FromEmailID);
                    DateTime dTime = DateTime.Now;
                    Guid gd = Guid.NewGuid();
                    string strMessageId = "<" + gd + "_" + dTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
                    nvc.Add("Message-Id", strMessageId);
                    Message.Headers.Add(nvc);
                    foreach (string emailId in contactList)
                    {
                        if (!string.IsNullOrEmpty(emailId))
                        {
                            Message.To.Add(emailId);
                        }
                    }

                    foreach (string email in strEmailCc)
                    {
                        if (!string.IsNullOrEmpty(email))
                        {
                            Message.CC.Add(email);
                        }
                    }

                    Message.From = new MailAddress(FromEmailID, "L&T-HE");
                    Message.Subject = mailSubject;
                    Message.Body = msgBody;
                    Message.IsBodyHtml = true;

                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(msgBody, System.Text.Encoding.UTF8, "text/html");
                    Message.AlternateViews.Add(htmlView);

                    SmtpClient smtp = new SmtpClient();




                    smtp.EnableSsl = false;
                    smtp.Port = mailPort;
                    smtp.Host = mailHost;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = true;

                    foreach (string emailId in contactList)
                    {
                        if (!string.IsNullOrEmpty(emailId))
                        {
                            //cnLF.WriteToLogFile("EmailSending.SendMailMultipleAttachmentsWithBody", "Before sending mail : " + emailId);
                        }
                    }

                    foreach (string emailId in strEmailCc)
                    {
                        if (!string.IsNullOrEmpty(emailId))
                        {
                            //cnLF.WriteToLogFile("EmailSending.SendMailMultipleAttachmentsWithBody", "Before sending mail : " + emailId);
                        }
                    }

                    smtp.Send(Message);
                    blMailSucceed = true;

                    foreach (string emailId in contactList)
                    {
                        if (!string.IsNullOrEmpty(emailId))
                        {
                            //cnLF.WriteToLogFile("MailSender.SendMailMultipleAttachmentsWithBody", "Mail sent to : " + emailId);
                        }
                    }

                    foreach (string emailId in strEmailCc)
                    {
                        if (!string.IsNullOrEmpty(emailId))
                        {
                            //cnLF.WriteToLogFile("MailSender.SendMailMultipleAttachmentsWithBody", "Mail sent to : " + emailId);
                        }
                    }

                }
                catch (Exception ex)
                {
                    //cnLF.WriteToLogFile("ContractNote.SendMail", ex.Message);
                }
            }



            return blMailSucceed;
        }

        #endregion

        #region Email Template

        public string GetMailTemplate()
        {
            return @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
	<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
	<title>Larsen &amp; Toubro</title>
</head>
<style>
	body
	{
		font-family: Verdana, Arial, Helvetica, sans-serif;
		font-size: 12px;
	}
	.table
	{
		border: solid 1px #999999;
	}
	td
	{
		padding: 10px;
	}
	.td
	{
		margin: 0px;
		padding: 0px;
		color: #FFFFFF;
		font-weight: bold;
	}
	p
	{
		margin-top: 10px;
	}
	.white_link
	{
		color: #FFFFFF;
	}
	.white_link:hover
	{
		color: #FFFFFF;
	}
</style>
<body>
	<table width='100%' border='0' cellspacing='0' cellpadding='0' class='table' align='center'>
		<tr>
			<td align='center'>
				<img src='http://www.larsentoubro.com/lntcorporate/Common/images/l&-T-logo.jpg' />
			</td>
		</tr>
		<tr>
			<td>
				<p>
					Hi _ISName_</p>                
				<br />
				<p>
				_msg_ <br />
				<a href='_Link_' target='_blank'>Click here</a>_linkmsg_<br />
				</p>
				<p>
					<b>Regards,</b><br />					 
					L&T, Powai, Mumbai
					</p>
			</td>
		</tr>
		<tr>
			<td>
				<br />
				<p>
					<strong>Disclaimer:</strong></p>
				<p>
					<i>This Email may contain confidential or privileged information for the intended recipient
						(s).
						<br />
						If you are not the intended recipient, please do not use or disseminate the information,
						notify the sender and delete it from your system. </i>
				</p>
			</td>
		</tr>
		<tr>
			<td bgcolor='#037afd'>
			</td>
		</tr>
	</table>
</body>
</html>

";
        }

        #endregion
    }
}
