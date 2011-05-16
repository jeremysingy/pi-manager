using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using PIManager.DAO;
using PIManager.Models;
using System.Globalization;

namespace PIManager.Professor
{
    /// <summary>
    /// Page to open projects to the registration for students
    /// </summary>
    public partial class OpenProject : System.Web.UI.Page
    {
        /// <summary>
        /// Get acces to the unique logger instance
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(OpenProject));

        /// <summary>
        /// Get access to the projects
        /// </summary>
        ProjectAccess myProjectAccess = new ProjectAccess();

        /// <summary>
        /// List of projects from which inherit
        /// </summary>
        List<Project> myProjects;

        HashSet<int> myIdsToOpen;

        /// <summary>
        /// Called when the page is loaded
        /// </summary>
        /// <param name="sender">Sender object of the event</param>
        /// <param name="e">Arguments of the event</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //myProjects = myProjectAccess.getProjects();
                    createSessions();

                    // Bind the projects with the table
                    ProjectsGrid.DataSource = myProjects;
                    ProjectsGrid.DataBind();

                    // Add eventual confirmation message
                    if (Request.QueryString["confirm"] != null && Request.QueryString["confirm"] == "1")
                        phOpened.Visible = true;
                }
                else
                    getSessions();
            }
            catch (Exception exception)
            {
                log.Error("Error loading page: " + exception.Message, exception);
            }
        }

        /// <summary>
        /// Create all the sessions used by the page
        /// </summary>
        protected void createSessions()
        {
            Session["projects"] = myProjects = myProjectAccess.getProjects();
            Session["toOpen"] = myIdsToOpen = new HashSet<int>();
        }

        /// <summary>
        /// Obtain all the created sessions used by the page when the page is posted back
        /// </summary>
        protected void getSessions()
        {
            myProjects = (List<Project>)Session["projects"];
            myIdsToOpen = (HashSet<int>)Session["toOpen"];
        }

        //protected void onCheckboxChanged(object sender, GridViewDeleteEventArgs e)
        public void onCheckboxChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;

            if (checkbox.Checked)
                myIdsToOpen.Add(myProjects[row.RowIndex].Id);
            else
                myIdsToOpen.Remove(myProjects[row.RowIndex].Id);
        }

        /// <summary>
        /// Called when user clicked on the cancel button
        /// </summary>
        /// <param name="sender">Sender object of the event</param>
        /// <param name="e">Arguments of the event</param>
        protected void btCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }

        /// <summary>
        /// Called when user clicked on the submit button
        /// </summary>
        /// <param name="sender">Sender object of the event</param>
        /// <param name="e">Arguments of the event</param>
        protected void btSubmit_Click(object sender, EventArgs e)
        {
            if (myIdsToOpen.Count == 0)
            {
                lbErrorProjects.Visible = true;
                return;
            }

            DateTime dateOpen, dateClose;
            if(!validateDates(out dateOpen, out dateClose))
            {
                lbErrorProjects.Visible = false;
                lbErrorDates.Visible = true;
                return;
            }

            myProjectAccess.openRegistration(myIdsToOpen, dateOpen, dateClose);

            Response.Redirect("OpenProject.aspx?confirm=1");
        }

        /// <summary>
        /// Validate the dates passed by the form
        /// </summary>
        /// <returns>true if the dates are correct, false otherwise</returns>
        private bool validateDates(out DateTime dateOpen, out DateTime dateClose)
        {
            System.Globalization.DateTimeFormatInfo format = new DateTimeFormatInfo();
            format.ShortDatePattern = "dd.MM.yyyy HH:mm";
            format.DateSeparator = ".";

            bool ok1 = DateTime.TryParse(tbStart.Text, format, DateTimeStyles.None, out dateOpen);
            bool ok2 = DateTime.TryParse(tbEnd.Text, format, DateTimeStyles.None, out dateClose);

            if (!ok1 || !ok2)
                return false;

            if (dateOpen.CompareTo(dateClose) >= 0)
                return false;

            return true;
        }
    }
}