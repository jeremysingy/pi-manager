using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PIManager
{
    /// <summary>
    /// Default page class
    /// </summary>
    public partial class _Default : System.Web.UI.Page
    {
        /// <summary>
        /// Called when the page is loaded.
        /// Create all roles regarding the current login
        /// </summary>
        /// <param name="sender">Sender object of the event</param>
        /// <param name="e">Arguments of the event</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Roles.RoleExists("student"))
            {
                Roles.CreateRole("student");
            }
            if (!Roles.RoleExists("professor"))
            {
                Roles.CreateRole("professor");
            }
            if(Roles.IsUserInRole("student"))
            {
                Server.Transfer("Student/Default.aspx");
            }
            else if(Roles.IsUserInRole("professor"))
            {
                Server.Transfer("Professor/Default.aspx");
            }
        }
    }
}
