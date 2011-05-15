using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIManager.DAO;
using System.Web.Services;
using System.Xml.XPath;
using System.Xml;
using System.IO;
using System.Xml.Xsl;
using System.Web.Security;
using PIManager.Login;
using PIManager.Models;

namespace PIManager.Visualization
{
    public partial class VisuProjectStudent : System.Web.UI.Page
    {
        ProjectAccess projectAccess; // reference to ProjectAccess
        MemberShipPIUser user; // user that is logged in

        /// <summary>
        /// Loads the page displaying the list of projects opened for the inscription.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            user = (MemberShipPIUser)Membership.GetUser();

            if (user == null)
                Response.Redirect("/Account/Login.aspx");

            projectAccess = new ProjectAccess();
            displayProjectList();
        }

        /// <summary>
        /// Creates and displays the list of projects. It gets list of projects 
        /// and inscription of the current student from the database. 
        /// </summary>
        private void displayProjectList()
        {
            // inserts a header to the list
            setHeader();

            // Gets the list of projects and inscriptions of the current student
            List<Project> projects = projectAccess.getProjectList();
            List<Int32> inscriptions = projectAccess.getInscriptions(user.PkPerson);
            Boolean opened = projectAccess.checkPeriodInscriptionOpen();

            // add each available project, passing project id when the person has
            // already subscribed for a project.
            for (int row = 0; row < projects.Count; row++)
            {
                if (inscriptions.Count == 0)
                    addProject(user.PkPerson, projects.ElementAt(row), opened);
                else
                    addProject(user.PkPerson, projects.ElementAt(row), opened, inscriptions.ElementAt(0));
            }
        }


        /// <summary>
        /// Sets header of the table
        /// </summary>
        private void setHeader()
        {
            TableHeaderRow header = new TableHeaderRow();
            header.CssClass = "tableAlternateHeader";
            ProjectTable.Controls.Add(header);

            TableCell hId = new TableCell();
            hId.Text = "Id";
            header.Controls.Add(hId);

            TableCell hTitle = new TableCell();
            hTitle.Text = "Projet";
            header.Controls.Add(hTitle);

            TableCell hInscription = new TableCell();
            hInscription.Text = "Inscription";
            header.Controls.Add(hInscription);
        }

        /// <summary>
        /// Inserts a project into the list, and add a link to suscribe if 
        /// the student has no project yet.
        /// </summary>
        /// <param name="idPerson">id of the student</param>
        /// <param name="p">project information</param>
        /// <param name="projectId">project id the student is already subscribed to</param>
        private void addProject(int idPerson, Project p, bool opened, int projectId = -1)
        {
            TableRow newProject = new TableRow();
            ProjectTable.Controls.Add(newProject);

            TableCell cellId = new TableCell();
            cellId.Text = p.Id.ToString();
            newProject.Controls.Add(cellId);

            TableCell cellName = new TableCell();
            LinkButton description = new LinkButton();
            description.Text = p.Name;
            description.Click += description_Click;
            description.CommandArgument = p.Id.ToString();
            cellName.Controls.Add(description);
            newProject.Controls.Add(cellName);

            TableCell cellInscription = new TableCell();
            
            // student has not subscribed yet.
            if (projectId == -1 && opened)
            {
                LinkButton subscribe = new LinkButton();
                subscribe.ID = p.Id.ToString();
                subscribe.Text = "S'inscrire";
                subscribe.Click += subscribe_Click;
                subscribe.CommandArgument = idPerson + "|" + p.Id;
                cellInscription.Controls.Add(subscribe);
            }
            else if (projectId == p.Id && opened)
            {
                LinkButton unsubscribe = new LinkButton();
                unsubscribe.ID = p.Id.ToString();
                unsubscribe.Text = "Se désinscrire";
                unsubscribe.Click += unsubscribe_Click;
                unsubscribe.CommandArgument = idPerson.ToString();
                cellInscription.Controls.Add(unsubscribe);
            }
            else
                cellInscription.Text = "-";
            newProject.Controls.Add(cellInscription);
        }

        /// <summary>
        /// Does the subscription of the student to the given project when 
        /// "subscribe"-link is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void subscribe_Click(Object sender, EventArgs e)
        {
            LinkButton subscribe = sender as LinkButton;
            string args = (string)subscribe.CommandArgument;
            string[] args_tab = args.Split('|');

            int idPerson = Int32.Parse(args_tab[0]);
            int idProject = Int32.Parse(args_tab[1]);

            Boolean saveDone = projectAccess.saveInscription(idPerson, idProject);

            if (!saveDone)
                errorLabel.Text = "Erreur: L'inscription n'a pas pu être effectuée. " +
                    "Le projet n'existe pas ou n'est plus disponible.";
            else
                msgLabel.Text = "Inscription effectuée.";

            ProjectTable.Rows.Clear();
            displayProjectList();
        }

        /// <summary>
        /// Cancels the subscription of the student when "unsubscribe"-link is clicked.
        /// </summary>
        /// <param name="sender">"unsubscribe"-link that was clicked on</param>
        /// <param name="e">arguments given to this command (person id)</param>
        public void unsubscribe_Click(Object sender, EventArgs e)
        {
            LinkButton unsubscribe = sender as LinkButton;
            int idPerson = Int32.Parse((string)unsubscribe.CommandArgument);
            Boolean cancelDone = projectAccess.cancelInscription(idPerson);

            if (!cancelDone)
                errorLabel.Text = "Erreur: La désinscription n'a pas pu être effectuée. " +
                    "Le projet n'existe pas ou vous n'êtes pas inscrit à ce projet.";
            else
                msgLabel.Text = "Désinscription effectuée.";

            ProjectTable.Rows.Clear();
            displayProjectList();
        }

        /// <summary>
        /// Displays the description of the project that is clicked.
        /// </summary>
        /// <param name="sender">link on the project name</param>
        /// <param name="e">arguments given to this command (project id)</param>
        public void description_Click(Object sender, EventArgs e)
        {
            // gets path to the xslt file
            string path = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, @"xslt\projectDescription.xslt");
            
            // Gets id of the project that must be displayed
            LinkButton description = sender as LinkButton;
            int idProject = Int32.Parse((string)description.CommandArgument);
            
            // Gets project data from database
            Project project = projectAccess.getProject(idProject);
            
            // Loads xslt file and executes the transformation to html. Result is stored in a StringWriter.
            XPathDocument doc = new XPathDocument(new StringReader("<description>"+project.Description+"</description>"));
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