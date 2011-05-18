using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using PIManager.DAO;
using PIManager.Models;

namespace PIManager.Professor
{
    /// <summary>
    /// Page to view all inscriptions of a project
    /// </summary>
    public partial class VisualizationInscription : System.Web.UI.Page
    {
        /// <summary>
        /// Called when the page is loaded
        /// </summary>
        /// <param name="sender">Sender object of the event</param>
        /// <param name="e">Arguments of the event</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            initTable();
            getInscriptions();

        }

        /// <summary>
        /// Init the table to view informations
        /// </summary>
        private void initTable()
        {
            TableHeaderRow header = new TableHeaderRow();
            header.CssClass = "tableAlternateHeader";
            InscriptionsTable.Controls.Add(header);

            TableCell hProjects = new TableCell();
            hProjects.Text = "Projets";
            header.Controls.Add(hProjects);

            TableCell hStudents = new TableCell();
            hStudents.Text = "Etudiant inscrits";
            header.Controls.Add(hStudents);

        }

        /// <summary>
        /// Get inscriptions of the project
        /// </summary>
        private void getInscriptions()
        {
            ProjectAccess projectAccess = new ProjectAccess();

            Hashtable projects = projectAccess.getProjectInscriptions();

            foreach (DictionaryEntry projectEntry in projects)
            {
                Project project = (Project)projectEntry.Value;

                TableRow newProject = new TableRow();
                InscriptionsTable.Controls.Add(newProject);

                TableCell cellTitle = new TableCell();
                cellTitle.Text = project.Name;
                newProject.Controls.Add(cellTitle);

                string personInscription = "<ul>\n";
                foreach (Person person in project.Inscriptions)
                {
                    personInscription += "<li>" + person.CompleteName + "</li>\n";
                }
                personInscription += "</ul>";

                TableCell cellInscriptions = new TableCell();
                cellInscriptions.Text = personInscription;
                newProject.Controls.Add(cellInscriptions);

                TableCell cellInscription = new TableCell();
            }
        }
    }
}