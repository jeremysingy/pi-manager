using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIManager.DAO;
using PIManager.Models;

namespace PIManager.Professor
{
    public partial class VisualizationGroup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            initTable();

            showProjects();

        }


        private void initTable()
        {
            TableHeaderRow header = new TableHeaderRow();
            header.CssClass = "tableAlternateHeader";
            ProjectsTable.Controls.Add(header);

            TableCell hID = new TableCell();
            hID.Text = "ID";
            header.Controls.Add(hID);

            TableCell hTitle = new TableCell();
            hTitle.Text = "Titre";
            header.Controls.Add(hTitle);

            TableCell hTechno = new TableCell();
            hTechno.Text = "Technologie";
            header.Controls.Add(hTechno);

            TableCell hGroup = new TableCell();
            hGroup.Text = "Groupe";
            header.Controls.Add(hGroup);

            TableCell hClient = new TableCell();
            hClient.Text = "Client";
            header.Controls.Add(hClient);

            TableCell hHistory = new TableCell();
            hHistory.Text = "Historique";
            header.Controls.Add(hHistory);

        }


        private void showProjects()
        {


            ProjectAccess projectAccess = new ProjectAccess();

            List<Project> projects = projectAccess.getFullProject();


            foreach (Project project in projects)
            {
                string clientName = projectAccess.getPersonCompletName(project.ClientId);
                List<Technology> technologys = projectAccess.getTechnologyProject(project.Id);
                List<Person> groups = projectAccess.getProjectGroup(project.Id);
                AddTableLine(project.Id.ToString(), project.Name, technologys, groups, clientName);
            }

            
        }


        private void AddTableLine(string id, string title, List<Technology> technoList, List<Person> groupList, string client)
        {
            TableRow newProject = new TableRow();
            ProjectsTable.Controls.Add(newProject);

            TableCell cellID = new TableCell();
            cellID.Text = id;
            newProject.Controls.Add(cellID);

            TableCell cellTitle = new TableCell();
            cellTitle.Text = title;
            newProject.Controls.Add(cellTitle);

            string technoStr = "<ul>\n";
            foreach (Technology techno in technoList)
            {
                technoStr += "<li>" + techno.Name + "</li>\n";
            }
            technoStr += "</ul>";

            TableCell cellTechno = new TableCell();
            cellTechno.Text = technoStr;
            newProject.Controls.Add(cellTechno);

            string personGroup = "<ul>\n";
            foreach (Person person in groupList)
            {
                personGroup += "<li>" + person.CompleteName + "</li>\n";
            }
            personGroup += "</ul>";

            TableCell cellGroup = new TableCell();
            cellGroup.Text = personGroup;
            newProject.Controls.Add(cellGroup);

            TableCell cellClient = new TableCell();
            cellClient.Text = client;
            newProject.Controls.Add(cellClient);

            TableCell cellHistory = new TableCell();
            cellHistory.Text = "<a href='/Professor/VisualizationHistory.aspx?id=" + id + "'>Voir historique</a>"; ;
            newProject.Controls.Add(cellHistory);
        }

    }
}