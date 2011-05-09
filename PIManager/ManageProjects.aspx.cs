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
        ProjectAccess projectAccess = new ProjectAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Get the projects from the DAO
            List<Project> projects = projectAccess.getProjects();

            // Bind the projects with the table
            ProjectsGrid.DataSource = projects;
            ProjectsGrid.DataBind();
        }
    }
}