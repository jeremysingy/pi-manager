using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIManager.DAO;
using PIManager.Models;
using System.IO;
using System.Xml.XPath;
using System.Xml.Xsl;

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
            LinkButton description = new LinkButton();
            description.Text = title;
            description.Click += description_Click;
            description.CommandArgument = id.ToString();
            cellTitle.Controls.Add(description);
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
            cellHistory.Text = "<a href='/Professor/VisualizationHistory.aspx?id=" + id + "'>Voir historique</a>";
            newProject.Controls.Add(cellHistory);

            TableCell cellPDF = new TableCell();
            cellPDF.Text = "<a href='/Professor/ProjectPDF.aspx?id=" + id + "'>PDF</a>";
            newProject.Controls.Add(cellPDF);
        }

        /// <summary>
        /// Displays the description of the project that is clicked.
        /// </summary>
        /// <param name="sender">link on the project name</param>
        /// <param name="e">arguments given to this command (project id)</param>
        public void description_Click(Object sender, EventArgs e)
        {
            ProjectAccess projectAccess = new ProjectAccess();

            // gets path to the xslt file
            string path = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, @"xslt\fullProjectDescription.xslt");

            // Gets id of the project that must be displayed
            LinkButton description = sender as LinkButton;
            int idProject = Int32.Parse((string)description.CommandArgument);

            // Gets project data from database
            Project project = projectAccess.getProject(idProject);

            List<Technology> technos = projectAccess.getTechnologyProject(idProject);

            // Loads xslt file and executes the transformation to html. Result is stored in a StringWriter.
            string xml = "<project>\n";
            xml += "<title>" + project.Name + "</title>\n";
            xml += "<abreviation>" + project.Abreviation + "</abreviation>\n";
            xml += "<description>" + project.Description + "</description>\n";
            xml += "<nbStudent>" + project.NbStudents + "</nbStudent>\n";
            xml += "<technologies>\n";
            foreach(Technology techno in technos){
                xml += "<technology>" + techno.Name + "</technology>\n";
            }
            xml += "</technologies>\n";
            xml += "</project>\n";


            XPathDocument doc = new XPathDocument(new StringReader(xml));
            XslCompiledTransform xslt = new XslCompiledTransform();
            StringWriter sw = new StringWriter();

            xslt.Load(path);

            // get image from database
            byte[] image = projectAccess.getImage(idProject);

            if (image != null)
            {
                string encodedImage = "data:image/jpg;base64,";
                encodedImage += base64Encode(image);
                XsltArgumentList argsList = new XsltArgumentList();
                argsList.AddParam("IMAGE", "", encodedImage);
                xslt.Transform(doc, argsList, sw);
            }
            else
                xslt.Transform(doc, null, sw);

            descriptionPanel.Controls.Add(new LiteralControl(sw.ToString()));
        }

        private string base64Encode(byte[] bytes)
        {
            try
            {
                string encodedData = Convert.ToBase64String(bytes);
                return encodedData;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Encode " + e.Message);
            }
        }

    }
}