using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIManager.DAO;
using log4net;

namespace PIManager.Professor
{
    /// <summary>
    /// Page to get an image stored in database
    /// </summary>
    public partial class GetImage : System.Web.UI.Page
    {
        /// <summary>
        /// Get acces to the unique logger instance
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(GetImage));
        
        /// <summary>
        /// Give access to the project
        /// </summary>
        protected ProjectAccess myProjectAccess = new ProjectAccess();

        /// <summary>
        /// Called when the page is loaded
        /// </summary>
        /// <param name="sender">Sender object of the event</param>
        /// <param name="e">Arguments of the event</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.ContentType = "image";

                int id = -1;
                if(int.TryParse(Request.QueryString["id"], out id))
                    Response.BinaryWrite(myProjectAccess.getImage(id));
                
                Response.End();
            }
            catch (Exception exception)
            {
                log.Error("Error loading page: " + exception.Message, exception);
            }
        }
    }
}