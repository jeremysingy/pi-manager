using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
            hProjects.Text = "Projects";
            header.Controls.Add(hProjects);

            TableCell hStudents = new TableCell();
            hStudents.Text = "Etudiant inscrits";
            header.Controls.Add(hStudents);

        }

        private void getInscriptions()
        {
            //getProjectsWithInscription

            //show projectInscriptions
        }
    }
}