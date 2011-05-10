using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIManager.DataAccess;
using System.Data.SqlClient;
using System.Configuration;

namespace PIManager
{
    public partial class ModifyProject : System.Web.UI.Page
    {
        protected ProjectAccess projectAccess = new ProjectAccess();
        protected TechnologyAccess technoAccess = new TechnologyAccess();
        protected PersonAccess personAccess = new PersonAccess();
        protected int myProjectId;
        protected Project myModifiedProject;

        protected List<Technology> myProjectTechnos;
        protected Dictionary<int, Technology> myTechnologies;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    createSessions();
                    List<Person> persons = personAccess.getPersons(2);

                    fillFields(myModifiedProject, persons);
                }

                getSessions();
            }
            catch (Exception exception)
            {
                tbTitle.Text = exception.Message;
            }
        }

        protected void createSessions()
        {
            Session["projectId"] = myProjectId = int.Parse(Request.QueryString["id"]);
            Session["oldProj"] = myModifiedProject = projectAccess.getProject(myProjectId);
            Session["projectTechnos"] = myProjectTechnos = new List<Technology>();
            Session["technologies"] = myTechnologies = technoAccess.getTechnologies();
        }

        protected void getSessions()
        {
            myProjectId = (int)Session["projectId"];
            myModifiedProject = (Project)Session["oldProj"];
            myProjectTechnos = (List<Technology>)Session["projectTechnos"];
            myTechnologies = (Dictionary<int, Technology>)Session["technologies"];
        }

        protected void fillFields(Project project, List<Person> professors)
        {
            foreach(int id in project.TechnologyIds)
                myProjectTechnos.Add(myTechnologies[id]);
            
            lbName.Text = project.Name;
            tbTitle.Text = project.Name;
            tbAbreviation.Text = project.Abreviation;
            tbDescription.Text = project.Description;

            gridTechnologies.DataSource = myProjectTechnos;
            gridTechnologies.DataBind();

            listTechnologies.DataSource = myTechnologies.Values;
            listTechnologies.DataBind();

            tbNbStudents.Text = project.NbStudents.ToString();

            listClients.DataSource = professors;
            listClients.DataBind();
            listClients.SelectedValue = project.ClientID.ToString();
        }

        protected void onRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            myProjectTechnos.RemoveAt(e.RowIndex);
            gridTechnologies.DataSource = myProjectTechnos;
            gridTechnologies.DataBind();
        }

        protected void btAddTechno_Click(object sender, EventArgs e)
        {
            myProjectTechnos.Add(myTechnologies[int.Parse(listTechnologies.SelectedValue)]);
            gridTechnologies.DataSource = myProjectTechnos;
            gridTechnologies.DataBind();
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageProjects.aspx");
        }

        protected void btSubmit_Click(object sender, EventArgs e)
        {
            Project newProject = new Project(tbTitle.Text,
                                             tbAbreviation.Text,
                                             tbDescription.Text,
                                             int.Parse(tbNbStudents.Text),
                                             int.Parse(listClients.SelectedValue));

            if (projectAccess.modifyProject(myProjectId, myModifiedProject, newProject, myProjectTechnos))
                Response.Redirect("ManageProjects.aspx");
            else
                Response.Redirect("ManageProjects.aspx?error=1");
        }

        //[ConfigurationPropertyAttribute("requestValidationMode", DefaultValue = "4.0")]
        //public Version RequestValidationMode { get; set; }
    }
}