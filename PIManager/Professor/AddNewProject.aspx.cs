using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIManager.DAO;
using log4net;
using PIManager.Models;
using System.IO;

namespace PIManager.Professor
{
    /// <summary>
    /// Page to add a new project
    /// </summary>
    public partial class AddNewProject : System.Web.UI.Page
    {
        /// <summary>
        /// Get acces to the unique logger instance
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(AddNewProject));

        /// <summary>
        /// True if the project has a parent, false otherwise
        /// </summary>
        protected bool myHasParent = false;

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
        /// Reference to the parent project (if any)
        /// </summary>
        protected Project myParentProject = null;

        /// <summary>
        /// Technologies added to the project
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
            }
        }

        /// <summary>
        /// Create all the sessions used by the page
        /// </summary>
        protected void createSessions()
        {
            int parentId = Request.QueryString["parent"] == null ? -1 : int.Parse(Request.QueryString["parent"]);
            Session["hasParent"] = myHasParent = parentId > 0;

            if (myHasParent)
                Session["parentProj"] = myParentProject = myProjectAccess.getProject(parentId);
            
            Session["projectTechnos"] = myProjectTechnos = new List<Technology>();
            Session["technologies"] = myTechnologies = myTechnoAccess.getTechnologies();
        }

        /// <summary>
        /// Obtain all the created sessions used by the page when the page is posted back
        /// </summary>
        protected void getSessions()
        {
            myHasParent = (bool)Session["hasParent"];
            
            if(myHasParent)
                myParentProject = (Project)Session["parentProj"];

            myProjectTechnos = (List<Technology>)Session["projectTechnos"];
            myTechnologies = (Dictionary<int, Technology>)Session["technologies"];
        }

        /// <summary>
        /// Fill the fields of the form with the proper informations
        /// </summary>
        /// <param name="professors">List of professor to fill the list</param>
        private void fillFields(List<Person> professors)
        {
            if (myHasParent)
            {
                if (myHasParent)
                    foreach (int id in myParentProject.TechnologyIds)
                        myProjectTechnos.Add(myTechnologies[id]);

                tbTitle.Text = myParentProject.Name;
                tbAbreviation.Text = myParentProject.Abreviation;
                tbDescription.Text = myParentProject.Description;
            }

            gridTechnologies.DataSource = myProjectTechnos;
            gridTechnologies.DataBind();

            listTechnologies.DataSource = myTechnologies.Values;
            listTechnologies.DataBind();

            if (myHasParent)
                tbNbStudents.Text = myParentProject.NbStudents.ToString();

            listClients.DataSource = professors;
            listClients.DataBind();

            if (myHasParent)
                listClients.SelectedValue = myParentProject.ClientId.ToString();
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
            if(!myProjectTechnos.Contains(myTechnologies[int.Parse(listTechnologies.SelectedValue)]))
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
            Response.Redirect("AddProject.aspx");
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

            int parentId = myHasParent ? myParentProject.Id : -1;

            Project newProject = new Project(tbTitle.Text,
                                             tbAbreviation.Text,
                                             tbDescription.Text,
                                             int.Parse(tbNbStudents.Text),
                                             int.Parse(listClients.SelectedValue),
                                             parentId);

            byte[] rawImage;
            using (var binaryReader = new BinaryReader(uploadImage.PostedFile.InputStream))
            {
                rawImage = binaryReader.ReadBytes(uploadImage.PostedFile.ContentLength);
            }

            myProjectAccess.addProject(newProject, myProjectTechnos, rawImage);
            //Response.Redirect("Default.aspx");

            test.Text = tbDescription.Text;
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