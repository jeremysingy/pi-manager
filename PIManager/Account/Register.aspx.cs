using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PIManager.Account
{
    /// <summary>
    /// Page to register a new user (not used!)
    /// </summary>
    public partial class Register : System.Web.UI.Page
    {
        /// <summary>
        /// Called when the page is loaded
        /// </summary>
        /// <param name="sender">Sender object of the event</param>
        /// <param name="e">Arguments of the event</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterUser.ContinueDestinationPageUrl = Request.QueryString["ReturnUrl"];
        }

        /// <summary>
        /// Called when a user is created
        /// </summary>
        /// <param name="sender">Sender object of the event</param>
        /// <param name="e">Arguments of the event</param>
        protected void RegisterUser_CreatedUser(object sender, EventArgs e)
        {
            FormsAuthentication.SetAuthCookie(RegisterUser.UserName, false /* createPersistentCookie */);

            string continueUrl = RegisterUser.ContinueDestinationPageUrl;
            if (String.IsNullOrEmpty(continueUrl))
            {
                continueUrl = "~/";
            }
            Response.Redirect(continueUrl);
        }

    }
}
