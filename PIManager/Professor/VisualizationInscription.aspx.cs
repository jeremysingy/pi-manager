using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using PIManager.DataAccess;

namespace PIManager.Professor
{
    public partial class VisualizationInscription : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            initTable();

            getInscriptions();
        }


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