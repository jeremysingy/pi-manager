using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PIManager
{
    public partial class _Default : System.Web.UI.Page
    {
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
