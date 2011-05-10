using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIManager.DataAccess;

namespace PIManager.Professor
{
    public partial class AddProject : System.Web.UI.Page
    {
        protected TechnologyAccess myTechnoAccess = new TechnologyAccess();
        protected PersonAccess myPersonAccess = new PersonAccess();
        protected List<Technology> myProjectTechnos;
        protected Dictionary<int, Technology> myTechnologies;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Session["projectTechnos"] = myProjectTechnos = new List<Technology>();
                Session["technologies"] = myTechnologies = myTechnoAccess.getTechnologies();

                List<Person> persons = myPersonAccess.getPersons(2);

                fillFields(persons);
            }
            else
            {
                myProjectTechnos = (List<Technology>)Session["projectTechnos"];
                myTechnologies = (Dictionary<int, Technology>)Session["technologies"];
            }
        }

        protected void fillFields(List<Person> professors)
        {
            gridTechnologies.DataSource = myProjectTechnos;
            gridTechnologies.DataBind();

            listTechnologies.DataSource = myTechnologies.Values;
            listTechnologies.DataBind();

            listClients.DataSource = professors;
            listClients.DataBind();
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
            //Response.Redirect("ManageProjects.aspx");
        }

        protected void btSubmit_Click(object sender, EventArgs e)
        {
            /*Project newProject = new Project(tbTitle.Text,
                                             tbAbreviation.Text,
                                             tbDescription.Text,
                                             int.Parse(tbNbStudents.Text),
                                             int.Parse(listClients.SelectedValue));

            if (projectAccess.modifyProject(myProjectId, myModifiedProject, newProject, myProjectTechnos))
                Response.Redirect("ManageProjects.aspx");
            else
                Response.Redirect("ManageProjects.aspx?error=1");*/
        }
    }
}