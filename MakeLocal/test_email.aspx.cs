using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class test_email : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string FROM = "";   // Replace with your "From" address. This address must be verified.
        string TO = "";  // Replace with a "To" address. If your account is still in the
        // sandbox, this address must be verified.

        string SUBJECT = "Amazon SES test (SMTP interface accessed using C#)";
        string BODY = "This email was sent through the Amazon SES SMTP interface by using C#.";

        // Supply your SMTP credentials below. Note that your SMTP credentials are different from your AWS credentials.
        string SMTP_USERNAME = "";  // Replace with your SMTP username.
        string SMTP_PASSWORD = "+QFIc";  // Replace with your SMTP password.

        // Amazon SES SMTP host name. This example uses the 米国西部 (オレゴン) region.
        string HOST = "email-smtp.us-west-2.amazonaws.com";

        // The port you will connect to on the Amazon SES SMTP endpoint. We are choosing port 587 because we will use
        // STARTTLS to encrypt the connection.
        const int PORT = 25;

        // Create an SMTP client with the specified host name and port.
        using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(HOST, PORT))
        {
            // Create a network credential with your SMTP user name and password.
            client.Credentials = new System.Net.NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);

            // Use SSL when accessing Amazon SES. The SMTP session will begin on an unencrypted connection, and then
            // the client will issue a STARTTLS command to upgrade to an encrypted connection using SSL.
            client.EnableSsl = true;

            // Send the email.

                client.Send(FROM, TO, SUBJECT, BODY);
        }

    }
}
