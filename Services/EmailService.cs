using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Microsoft.Identity.Client;

namespace funduq.Services
{
    public class EmailService
    {

        public void ContactUs(string name, string userEmail, string subject, string message, string contactUsEmail)
        {
            Regex regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");

            if (!regex.IsMatch(userEmail))
            {
                throw new InvalidEmailException();
            }

            MailMessage email = new MailMessage(new MailAddress(userEmail, subject), new MailAddress(contactUsEmail));
            var myAppConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            email.Subject = subject;

            email.Body = $"From {name}\n" + message;
            email.IsBodyHtml = false;

            SmtpClient client = new SmtpClient();
            client.Host = myAppConfig.GetValue<string>("EmailService:Host");
            client.Port = myAppConfig.GetValue<int>("EmailService:Port");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            System.Net.NetworkCredential credential = new System.Net.NetworkCredential();
            credential.UserName = myAppConfig.GetValue<string>("EmailService:Username");
            credential.Password = myAppConfig.GetValue<string>("EmailService:Password");
            client.UseDefaultCredentials = false;
            client.Credentials = credential;
            client.Send(email);


        }
        public void ContactUsAutomaticReply(string name, string contactUsEmail, string userEmail)
        {
            MailMessage email = new MailMessage(new MailAddress(contactUsEmail, "Thank You for Contacting Us"), new MailAddress(userEmail));
            email.Subject = "Thank You for Contacting Us!";
            email.Body = $@"<!DOCTYPE html>
<html lang=""en"">

<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Email Template</title>
    <style>
        /* General styling for the body */
        body {{
            margin: 0;
            padding: 0;
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
        }}

        /* Container for the email content */
        .email-container {{
            max-width: 600px;
            margin: 0 auto;
            background-color: #ffffff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
        }}

        /* Header section */
        .email-header {{
            text-align: center;
            padding: 20px 0;
            background-color: #4154f1;
            color: white;
            border-top-left-radius: 8px;
            border-top-right-radius: 8px;
        }}

        .email-header h1 {{
            margin: 0;
            font-size: 24px;
        }}

        /* Main content */
        .email-content {{
            padding: 20px 0;
            font-size: 16px;
            line-height: 1.6;
            color: #333333;
        }}

        .email-content h2 {{
            font-size: 20px;
            color: #4154f1;
            margin-bottom: 10px;
        }}

        .email-content p {{
            margin-bottom: 10px;
        }}

        /* Button styling */
        .btn {{
            display: inline-block;
            padding: 10px 20px;
            background-color: #4154f1;
            color: white;
            text-decoration: none;
            border-radius: 5px;
            margin-top: 20px;
        }}

        /* Footer section */
        .email-footer {{
            text-align: center;
            padding: 20px 0;
            font-size: 14px;
            color: #777777;
            border-top: 1px solid #dddddd;
            margin-top: 20px;
        }}

        .email-footer a {{
            color: #007BFF;
            text-decoration: none;
        }}

        @media screen and (max-width: 600px) {{
            .email-container {{
                padding: 10px;
            }}

            .email-content h2 {{
                font-size: 18px;
            }}

            .btn {{
                padding: 8px 16px;
            }}
        }}
    </style>
</head>

<body>
    <div class=""email-container"">
        <!-- Header Section -->
        <div class=""email-header"">
            <h1>Thank You for Contacting Us!</h1>
        </div>

        <!-- Main Content Section -->
        <div class=""email-content"">
            <h2>Hello {name},</h2>
            
            <p>
Thank you for reaching out to us! We have received your message and will get back to you as soon as possible. Our usual response time is within [time frame, e.g., 24 hours].
            </p>
        </div>

        <!-- Footer Section -->
        <div class=""email-footer"">
            <p>
                &copy; 2024 Funduq. All rights reserved.
            </p>
            <p>
                <a href=""#"">Unsubscribe</a> | <a href=""#"">Privacy Policy</a>
            </p>
        </div>
    </div>
</body>

</html>";




            email.IsBodyHtml = true;

            var myAppConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            SmtpClient client = new SmtpClient();
            client.Host = myAppConfig.GetValue<string>("EmailService:Host");
            client.Port = myAppConfig.GetValue<int>("EmailService:Port");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            System.Net.NetworkCredential credential = new System.Net.NetworkCredential();
            credential.UserName = myAppConfig.GetValue<string>("EmailService:Username");
            credential.Password = myAppConfig.GetValue<string>("EmailService:Password");
            client.UseDefaultCredentials = false;
            client.Credentials = credential;
            client.Send(email);
        }

        public void SendEmail(PdfService pdfService, string fromEmail, string mailTitle, string userEmail, string FirstName, string LastName, string hotelName, decimal totalAmount, DateOnly checkInDate, DateOnly checkOutDate,string roomName)
        {


            MailMessage message = new MailMessage(new MailAddress(fromEmail, mailTitle), new MailAddress(userEmail));

            message.Subject = "Funduq Booking Invoice";
            message.Body = $@"<!DOCTYPE html>
<html lang=""en"">

<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Email Template</title>
    <style>
        /* General styling for the body */
        body {{
            margin: 0;
            padding: 0;
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
        }}

        /* Container for the email content */
        .email-container {{
            max-width: 600px;
            margin: 0 auto;
            background-color: #ffffff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
        }}

        /* Header section */
        .email-header {{
            text-align: center;
            padding: 20px 0;
            background-color: #4154f1;
            color: white;
            border-top-left-radius: 8px;
            border-top-right-radius: 8px;
        }}

        .email-header h1 {{
            margin: 0;
            font-size: 24px;
        }}

        /* Main content */
        .email-content {{
            padding: 20px 0;
            font-size: 16px;
            line-height: 1.6;
            color: #333333;
        }}

        .email-content h2 {{
            font-size: 20px;
            color: #4154f1;
            margin-bottom: 10px;
        }}

        .email-content p {{
            margin-bottom: 10px;
        }}

        /* Button styling */
        .btn {{
            display: inline-block;
            padding: 10px 20px;
            background-color: #4154f1;
            color: white;
            text-decoration: none;
            border-radius: 5px;
            margin-top: 20px;
        }}

        /* Footer section */
        .email-footer {{
            text-align: center;
            padding: 20px 0;
            font-size: 14px;
            color: #777777;
            border-top: 1px solid #dddddd;
            margin-top: 20px;
        }}

        .email-footer a {{
            color: #007BFF;
            text-decoration: none;
        }}

        @media screen and (max-width: 600px) {{
            .email-container {{
                padding: 10px;
            }}

            .email-content h2 {{
                font-size: 18px;
            }}

            .btn {{
                padding: 8px 16px;
            }}
        }}
    </style>
</head>

<body>
    <div class=""email-container"">
        <!-- Header Section -->
        <div class=""email-header"">
            <h1>Funduq Booking Invoice</h1>
        </div>

        <!-- Main Content Section -->
        <div class=""email-content"">
            <h2>Hello {FirstName} {LastName},</h2>
            <p>
                 Thank you for choosing {hotelName} for your upcoming stay in {roomName}. We’re excited to welcome you!
            </p>
            <p>
You can find the invoice in the attached file <br/>
           If you have any special requests or need assistance before your arrival, please don’t hesitate to reach out to us. We want to ensure your stay is as comfortable and enjoyable as possible.
            </p>
        </div>

        <!-- Footer Section -->
        <div class=""email-footer"">
            <p>
                &copy; 2024 Funduq. All rights reserved.
            </p>
            <p>
                <a href=""#"">Unsubscribe</a> | <a href=""#"">Privacy Policy</a>
            </p>
        </div>
    </div>
</body>

</html>";



            var generatedInvoice = pdfService.GenerateInvoice(totalAmount, $"{FirstName} {LastName}", hotelName, checkInDate.ToString("yyyy-MM-dd"), checkOutDate.ToString("yyyy-MM-dd"),roomName);
            var generatedInvoiceStream = new MemoryStream(generatedInvoice);

            message.Attachments.Add(new Attachment(generatedInvoiceStream, "funduq_bill.pdf", "application/pdf"));
            message.IsBodyHtml = true;

            var myAppConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            SmtpClient client = new SmtpClient();
            client.Host = myAppConfig.GetValue<string>("EmailService:Host");
            client.Port = myAppConfig.GetValue<int>("EmailService:Port");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            System.Net.NetworkCredential credential = new System.Net.NetworkCredential();
            credential.UserName = myAppConfig.GetValue<string>("EmailService:Username");
            credential.Password = myAppConfig.GetValue<string>("EmailService:Password");
            client.UseDefaultCredentials = false;
            client.Credentials = credential;
            client.Send(message);
        }
    }

    public class InvalidEmailException : Exception
    {
        private string errorMessage = "Invalid Email";
        public string getMessage()
        {
            return errorMessage;
        }
    }
}

