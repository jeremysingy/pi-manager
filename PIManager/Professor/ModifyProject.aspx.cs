using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIManager.DAO;
using System.Data.SqlClient;
using System.Configuration;
using log4net;
using PIManager.Models;
using System.Drawing;
using System.IO;

namespace PIManager
{
    /// <summary>
    /// Page to modify a projectthat is still not opened to the students
    /// </summary>
    public partial class ModifyProject : System.Web.UI.Page
    {
        /// <summary>
        /// Get acces to the unique logger instance
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(ModifyProject));

        /// <summary>
        /// Give access to the project
        /// </summary>
        protected ProjectAccess myProjectAccess = new ProjectAccess();

        /// <summary>
        /// Give access to the technologies
        /// </summary>
        protected TechnologyAccess myTechnoAccess = new TechnologyAccess();

        /// <summary>
        /// Give access to the person
        /// </summary>
        protected PersonAccess myPersonAccess = new PersonAccess();

        /// <summary>
        /// Reference to the modified project
        /// </summary>
        protected Project myModifiedProject;

        /// <summary>
        /// Technologies of to the project
        /// </summary>
        protected List<Technology> myProjectTechnos;

        /// <summary>
        /// All technologies to put in the list
        /// </summary>
        protected Dictionary<int, Technology> myTechnologies;

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
                    // Show the eventual error
                    if (Request.QueryString["error"] != null && Request.QueryString["error"] == "1")
                        phError.Visible = true;

                    createSessions();
                    List<Person> persons = myPersonAccess.getPersons(2);

                    fillFields(persons);
                }
                else
                    getSessions();
            }
            catch (Exception exception)
            {
                log.Error("Error loading page: " + exception.Message);
                Response.Redirect("ManageProjects.aspx");
            }
        }

        /// <summary>
        /// Create all the sessions used by the page
        /// </summary>
        protected void createSessions()
        {
            int projectId = int.Parse(Request.QueryString["id"]);
            Session["oldProj"] = myModifiedProject = myProjectAccess.getProject(projectId);
            Session["projectTechnos"] = myProjectTechnos = new List<Technology>();
            Session["technologies"] = myTechnologies = myTechnoAccess.getTechnologies();
        }

        /// <summary>
        /// Obtain all the created sessions used by the page when the page is posted back
        /// </summary>
        protected void getSessions()
        {
            myModifiedProject = (Project)Session["oldProj"];
            myProjectTechnos = (List<Technology>)Session["projectTechnos"];
            myTechnologies = (Dictionary<int, Technology>)Session["technologies"];
        }

        /// <summary>
        /// Fill the fields of the form with the proper informations
        /// </summary>
        /// <param name="professors">List of professor to fill the list</param>
        private void fillFields(List<Person> professors)
        {
            foreach (int id in myModifiedProject.TechnologyIds)
                myProjectTechnos.Add(myTechnologies[id]);

            lbName.Text = myModifiedProject.Name;
            tbTitle.Text = myModifiedProject.Name;
            tbAbreviation.Text = myModifiedProject.Abreviation;
            tbDescription.Text = myModifiedProject.Description;

            if (myModifiedProject.HasImage)
                previewImage.ImageUrl = "/GetImage.aspx?id=" + myModifiedProject.Id;
            else
                previewImage.ImageUrl = "/Images/noimage.png";

            gridTechnologies.DataSource = myProjectTechnos;
            gridTechnologies.DataBind();

            listTechnologies.DataSource = myTechnologies.Values;
            listTechnologies.DataBind();

            tbNbStudents.Text = myModifiedProject.NbStudents.ToString();

            listClients.DataSource = professors;
            listClients.DataBind();
            listClients.SelectedValue = myModifiedProject.ClientId.ToString();
        }

        /// <summary>
        /// Called when a delete event is called in the technologies grid
        /// </summary>
        /// <param name="sender">Sender object of the event</param>
        /// <param name="e">Arguments of the event</param>
        protected void onRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            myProjectTechnos.RemoveAt(e.RowIndex);
            gridTechnologies.DataSource = myProjectTechnos;
            gridTechnologies.DataBind();
        }

        /// <summary>
        /// Called when user clicked on the Add Technologie button
        /// </summary>
        /// <param name="sender">Sender object of the event</param>
        /// <param name="e">Arguments of the event</param>
        protected void btAddTechno_Click(object sender, EventArgs e)
        {
            if (!myProjectTechnos.Contains(myTechnologies[int.Parse(listTechnologies.SelectedValue)]))
                myProjectTechnos.Add(myTechnologies[int.Parse(listTechnologies.SelectedValue)]);
            gridTechnologies.DataSource = myProjectTechnos;
            gridTechnologies.DataBind();
        }

        /// <summary>
        /// Called when user clicked on the cancel button
        /// </summary>
        /// <param name="sender">Sender object of the event</param>
        /// <param name="e">Arguments of the event</param>
        protected void btCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageProjects.aspx");
        }

        /// <summary>
        /// Called when user clicked on the submit button
        /// </summary>
        /// <param name="sender">Sender object of the event</param>
        /// <param name="e">Arguments of the event</param>
        protected void btSubmit_Click(object sender, EventArgs e)
        {
            if (myProjectTechnos.Count == 0)
            {
                lbErrorTechnos.Visible = true;
                return;
            }

            if (!validateImage(uploadImage.PostedFile))
            {
                lbErrorImage.Visible = true;
                return;
            }

            Project newProject = new Project(myModifiedProject.Id,
                                             tbTitle.Text,
                                             tbAbreviation.Text,
                                             tbDescription.Text,
                                             int.Parse(tbNbStudents.Text),
                                             int.Parse(listClients.SelectedValue));

            byte[] rawImage;
            using (var binaryReader = new BinaryReader(uploadImage.PostedFile.InputStream))
            {
                rawImage = binaryReader.ReadBytes(uploadImage.PostedFile.ContentLength);
            }

            if (myProjectAccess.modifyProject(myModifiedProject, newProject, myProjectTechnos, rawImage))
                Response.Redirect("ManageProjects.aspx?confirm=2");
            else
                Response.Redirect("ModifyProject.aspx?id=" + myModifiedProject.Id + "&error=1");
        }

        private bool validateImage(HttpPostedFile imageFile)
        {
            if (imageFile.ContentLength == 0)
                return true;

            return imageFile.ContentLength <= 512 * 1024 &&
                   (imageFile.ContentType == "image/gif" ||
                    imageFile.ContentType == "image/png" ||
                    imageFile.ContentType == "image/jpeg" ||
                    imageFile.ContentType == "image/bmp");
        }
    }
}