using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using PIManager.Login;
using PIManager.DAO;

namespace PIManager.Student
{
    public partial class Student : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // checks if period of inscription is opened
            ProjectAccess projectAccess = new ProjectAccess();
            bool opened = projectAccess.checkPeriodInscriptionOpen();

            // if opened, display only the link to see the list of projects available
            if (opened)
            {
                visuProjectsLink.Visible = true;
                addDocumentLink.Visible = false;
            }
            // else display only the link to add a document to the project (only if the student has subscribed to a project).
            else
            {
                visuProjectsLink.Visible = false;

                MemberShipPIUser user = (MemberShipPIUser)Membership.GetUser();
                if (user == null)
                    Response.Redirect("/Account/Login.aspx");
                List<Int32> inscriptions = projectAccess.getInscriptions(user.PkPerson);
                if (inscriptions.Count != 0)
                    addDocumentLink.Visible = true;
                else
                    addDocumentLink.Visible = false;
            }
        }
    }
}