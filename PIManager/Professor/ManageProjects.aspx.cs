using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIManager.DAO;
using System.Collections.ObjectModel;
using log4net;
using PIManager.Models;

namespace PIManager
{
    /// <summary>
    /// Page to manage projects that are still not opened to the students
    /// </summary>
    public partial class ManageProjects : System.Web.UI.Page
    {
        /// <summary>
        /// Get acces to the unique logger instance
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(ManageProjects));

        ProjectAccess myProjectAccess = new ProjectAccess();
        List<Project> myProjects;

        /// <summary>
        /// Called when the page is loaded
        /// </summary>
        /// <param name="sender">Sender object of the event</param>
        /// <param name="e">Arguments of the event</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Get the projects from the DAO
                myProjects = myProjectAccess.getProjects();

                // Bind the projects with the table
                ProjectsGrid.DataSource = myProjects;
                ProjectsGrid.DataBind();

                // Add eventual confirmation messages
                if (Request.QueryString["confirm"] != null && Request.QueryString["confirm"] == "1")
                    phAdded.Visible = true;
                if (Request.QueryString["confirm"] != null && Request.QueryString["confirm"] == "2")
                    phModified.Visible = true;
                if (Request.QueryString["confirm"] != null && Request.QueryString["confirm"] == "3")
                    phDeleted.Visible = true;
            }
            catch(Exception exception)
            {
                log.Error("Error loading page: " + exception.Message);
            }
        }

        /// <summary>
        /// Called when a delete event is called in the projects grid
        /// </summary>
        /// <param name="sender">Sender object of the event</param>
        /// <param name="e">Arguments of the event</param>
        protected void onRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            myProjectAccess.deleteProject(myProjects[e.RowIndex].Id);
            Response.Redirect("ManageProjects.aspx?confirm=3");
        }

        /// <summary>
        /// Called the data are bound in the projects grid
        /// </summary>
        /// <param name="sender">Sender object of the event</param>
        /// <param name="e">Arguments of the event</param>
        protected void onRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // reference the Delete LinkButton
                LinkButton db = (LinkButton)e.Row.Cells[4].Controls[0];

                db.OnClientClick = string.Format("return confirm('Êtes-vous sûr(e) de vouloir supprimer le projet \"{0}\"');",
                                                 convertName(e.Row.Cells[0].Text));
            }
        }

        /// <summary>
        /// Convert the name of a project to be properly shawn in Javascript
        /// </summary>
        /// <param name="projectName">Name of the project to convert</param>
        /// <returns>Converted project name</returns>
        private string convertName(string projectName)
        {
            return Server.HtmlDecode(projectName).Replace("'", @"\'");
        }
    }
}