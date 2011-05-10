using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIManager.DataAccess;

namespace PIManager
{
    public partial class AddDocument : System.Web.UI.Page
    {
        ProjectAccess projectAccess;
        int idProject = 3; // TODO: must come from session !

        protected void Page_Load(object sender, EventArgs e)
        {
            projectAccess = new ProjectAccess();
        }

        public void btnUpload_Click(object sender, EventArgs e)
        {
            // checks file size before upload
            int size = uploadFile.PostedFile.ContentLength;
            if (size > 5120)
            {
                errorLabel.Text = "Le fichier est trop grand.<br />";
                return;
            }

            Boolean addDone = projectAccess.addDocument(idProject, uploadFile.PostedFile);

            if (addDone)
                msgLabel.Text = "Le fichier a été ajouté au projet.<br />";
            else
                errorLabel.Text = "Le fichier n'a pas pu être ajouté au projet.<br />";
        }
    }
}