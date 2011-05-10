using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIManager.DataAccess;
using System.Web.Security;
using PIManager.Login;

namespace PIManager
{
    public partial class AddDocument : System.Web.UI.Page
    {
        ProjectAccess projectAccess;
        MemberShipPIUser user; // user that is logged in
        int idProject;

        protected void Page_Load(object sender, EventArgs e)
        {
            projectAccess = new ProjectAccess();

            // gets current user data
            user = (MemberShipPIUser)Membership.GetUser();
            List<Int32> inscriptions = projectAccess.getInscriptions(user.PK_Person);

            // gets id of the project for the current student
            idProject = inscriptions.ElementAt(0);
        }

        /// <summary>
        /// Adds a document to the project the student is subscribed to.
        /// </summary>
        /// <param name="sender">button "envoyer"</param>
        /// <param name="e">posted file</param>
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