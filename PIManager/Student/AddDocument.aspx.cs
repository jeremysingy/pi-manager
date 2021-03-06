﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIManager.DAO;
using System.Web.Security;
using PIManager.Login;

namespace PIManager
{
    /// <summary>
    /// Page to add a document in a project
    /// </summary>
    public partial class AddDocument : System.Web.UI.Page
    {
        /// <summary>
        /// Get access to the projects
        /// </summary>
        protected ProjectAccess projectAccess;

        /// <summary>
        /// User that is logged in
        /// </summary>
        protected MemberShipPIUser user;

        /// <summary>
        /// Id of the project
        /// </summary>
        protected int idProject;

        /// <summary>
        /// Called when the page is loaded
        /// </summary>
        /// <param name="sender">Sender object of the event</param>
        /// <param name="e">Arguments of the event</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            projectAccess = new ProjectAccess();

            // gets current user data
            user = (MemberShipPIUser)Membership.GetUser();

            if (user == null)
                Response.Redirect("/Account/Login.aspx");

            List<Int32> inscriptions = projectAccess.getInscriptions(user.PkPerson);
            bool opened = projectAccess.checkPeriodInscriptionOpen();
            
            // redirects to the default page if accessing this page without having subscribe to a project
            // or when period of inscription is still opened.
            if(inscriptions.Count == 0 || opened)
                Response.Redirect("/Student/Default.aspx");

            // gets id of the project for the current student
            idProject = inscriptions.ElementAt(0);
        }

        /// <summary>
        /// Adds a document to the project the student is subscribed to.
        /// </summary>
        /// <param name="sender">button "envoyer"</param>
        /// <param name="e">posted file</param>
        protected void btnUpload_Click(object sender, EventArgs e)
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