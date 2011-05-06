using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIManager.DataAccess;
using System.Data.SqlClient;

namespace PIManager
{
    public partial class ModifyProject : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //try
            //{
            int id = int.Parse(Request.QueryString["id"]);

            ProjectAccess projectAccess = new ProjectAccess();
            TechnologyAccess technoAccess = new TechnologyAccess();
            PersonAccess personAccess = new PersonAccess();
            Project project = projectAccess.getProject(id);

            List<Technology> technologies = technoAccess.getTechnologies();
            List<Person> persons = personAccess.getPersons(2);

            fillFields(project, technologies, persons);
            //}
            //catch (Exception exception)
            //{
            //    tbTitle.Text = exception.Message;
            //}
        }

        protected void fillFields(Project project, List<Technology> technologies, List<Person> professors)
        {
            lbName.Text = project.Name;
            tbTitle.Text = project.Name;
            tbDescription.Text = project.Description;

            listTechnologies.DataSource = technologies;
            listTechnologies.DataBind();

            //gridTechnologies.DataSource = technologies;
            //gridTechnologies.DataBind();

            tbNbStudents.Text = project.NbStudents.ToString();

            listClients.DataSource = professors;
            listClients.DataBind();
        }
    }
}