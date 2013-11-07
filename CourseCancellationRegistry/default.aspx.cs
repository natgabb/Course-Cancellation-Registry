using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CourseCancellationRegistry
{
    /// <summary>
    /// Default page. Accepts user input for email.
    /// </summary>
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Submit button click event handler.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event arg</param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                Session["Email"] = EmailTextBox.Text;
                Response.Redirect("courses.aspx");
            }
        }
    }
}