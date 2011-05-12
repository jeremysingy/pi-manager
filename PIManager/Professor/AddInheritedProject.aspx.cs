using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIManager.DAO;
using PIManager.Models;
using log4net;

namespace PIManager.Professor
{
    /// <summary>
    /// Page to add a project inherited from an existing one
    /// </summary>
    public partial class AddInheritedProject : System.Web.UI.Page
    {
        /// <summary>
        /// Get acces to the unique logger instance
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(AddInheritedProject));

        /// <summary>
        /// Get access to the projects
        /// </summary>
        ProjectAccess myProjectAccess = new ProjectAccess();

        /// <summary>
        /// List of projects from which inherit
        /// </summary>
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
                myProjects = myProjectAccess.getProjects();

                // Bind the projects with the table
                ProjectsGrid.DataSource = myProjects;
                ProjectsGrid.DataBind();
            }
            catch (Exception exception)
            {
                log.Error("Error loading page: " + exception.Message);
            }
        }
    }
}