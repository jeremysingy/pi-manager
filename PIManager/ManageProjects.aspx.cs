using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIManager.DataAccess;
using System.Collections.ObjectModel;

namespace PIManager
{
    /// <summary>
    /// Page to manage projects that are still not opened to the students
    /// </summary>
    public partial class ManageProjects : System.Web.UI.Page
    {
        ProjectAccess myProjectAccess = new ProjectAccess();
        List<Project> myProjects;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Get the projects from the DAO
            myProjects = myProjectAccess.getProjects();

            // Bind the projects with the table
            ProjectsGrid.DataSource = myProjects;
            ProjectsGrid.DataBind();

            // Show the eventual error
            if (Request.QueryString["error"] != null && Request.QueryString["error"] == "1")
                lbError.Text = "The project has been modified or deleted by another user. Please redo your modifications";
        }

        protected void onRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //myProjectAccess
            //lbError.Text = myProjects[e.RowIndex].Name + " : " + myProjects[e.RowIndex].Id;

            myProjectAccess.deleteProject(myProjects[e.RowIndex].Id);
        }
    }
}