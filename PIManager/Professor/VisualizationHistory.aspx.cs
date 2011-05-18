using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIManager.Models;
using PIManager.DAO;

namespace PIManager.Professor
{
    /// <summary>
    /// Page to visualize the history of a project
    /// </summary>
    public partial class VisualizationHistory : System.Web.UI.Page
    {
        /// <summary>
        /// Called when the page is loaded
        /// </summary>
        /// <param name="sender">Sender object of the event</param>
        /// <param name="e">Arguments of the event</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            int choosed_project = int.Parse(Request.QueryString.Get("id"));

            ProjectAccess projectAccess = new ProjectAccess();

            Stack<Project> history = projectAccess.getHistoryProject(choosed_project);

            int margin = 0;
            bool choosed_pass = false;

            while (history.Count != 0)
            {
                Project project = history.Pop();

                int pk_project = project.Id;
                string project_name = project.Name;

                if (!choosed_pass)
                {
                    string toAdd = "";
                    if (pk_project == choosed_project)
                    {
                        toAdd = "<div style='margin-left: " + margin + "px;'><strong>" + project_name + "</strong></div>\n";
                    }
                    else
                    {
                        toAdd = "<div style='margin-left: " + margin + "px;'>" + project_name + "</div>\n";
                    }
                    margin += 20;
                    historyMsg.Text += toAdd;

                    if (pk_project == choosed_project)
                    {
                        choosed_pass = true;
                    }
                }
                else
                {
                    string toAdd = "<div style='margin-left: " + margin + "px;'>" + project_name + "</div>\n";
                    historyMsg.Text += toAdd;
                }
            }

        }
    }
}