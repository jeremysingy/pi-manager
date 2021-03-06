﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Xml.Xsl;
using System.Xml.XPath;
using PIManager.Models;
using PIManager.DAO;

namespace PIManager.Professor
{
    /// <summary>
    /// Page to view a project description in a PDF document
    /// </summary>
    public partial class ProjectPDF : System.Web.UI.Page
    {
        /// <summary>
        /// Project to visualize in PDF
        /// </summary>
        private Project project;

        /// <summary>
        /// Called when the page is loaded
        /// </summary>
        /// <param name="sender">Sender object of the event</param>
        /// <param name="e">Arguments of the event</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            string desc = getProjectFromId(int.Parse(Request.QueryString.Get("id")));

            using (var m = new MemoryStream())
            {
                var document = new Document();

                try
                {
                    var writer = PdfWriter.GetInstance(document, m);
                    writer.CloseStream = false;

                    document.Open();

                    WriteDocument(document, desc);
                }
                catch (DocumentException ex)
                {
                    DBManager.getLog().Error("Error during PDF generation : " + ex.Message);
                }

                document.Close();

                Response.Clear();
                Response.AppendHeader("content-disposition", "attachment; filename=" + "project.pdf");
                Response.ContentType = "Application/pdf";
                Response.BinaryWrite(m.GetBuffer());
                Response.End();
            }
        }


        /// <summary>
        /// Write the informations of the project in the PDF file
        /// </summary>
        /// <param name="document">PDF document</param>
        /// <param name="desc">Description of the project</param>
        private void WriteDocument(Document document, string desc)
        {
            var title = "Projet : " + project.Name;

            document.AddTitle(title);

            document.Add(new Paragraph(project.Name + " (" + project.Abreviation + ")", new Font(Font.FontFamily.HELVETICA, 20)));

            document.Add(new Paragraph("Nombre d'étudiants " + project.NbStudents, new Font(Font.FontFamily.HELVETICA, 16)));

            document.Add(new Paragraph("Description : ", new Font(Font.FontFamily.HELVETICA, 16)));

            document.Add(new Paragraph(desc, new Font(Font.FontFamily.HELVETICA, 12)));

            document.Add(new Paragraph("Technologies : ", new Font(Font.FontFamily.HELVETICA, 16)));

            string technoStr = "";
            for(int i = 0; i < project.Technology.Count; i++)
            {
                if(i==0)
                {
                    technoStr += project.Technology[i].Name;
                }
                else
                {
                    technoStr += ", " + project.Technology[i].Name;
                }
                
            }

            document.Add(new Paragraph(technoStr, new Font(Font.FontFamily.HELVETICA, 12)));
        }


        /// <summary>
        /// Transform the project from database to texte
        /// </summary>
        /// <param name="idProject">Id of the choosed project</param>
        /// <returns>The description in XML</returns>
        private string getProjectFromId(int idProject)
        {
            ProjectAccess projectAccess = new ProjectAccess();

            // gets path to the xslt file
            string path = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, @"xslt\ProjectDescriptionToText.xslt");
       
            
            // Gets project data from database
            project = projectAccess.getProject(idProject);

            List<Technology> technos = projectAccess.getTechnologyProject(idProject);
            project.Technology = technos;
            
            // Loads xslt file and executes the transformation to texte. Result is stored in a StringWriter.
            string xml = "<project>\n";
            xml += "<title>" + project.Name + "</title>\n";
            xml += "<abreviation>" + project.Abreviation + "</abreviation>\n";
            xml += "<description>" + project.Description + "</description>\n";
            xml += "<nbStudent>" + project.NbStudents + "</nbStudent>\n";
            xml += "<technologies>\n";
            
            foreach (Technology techno in technos)
            {
                xml += "<technology>" + techno.Name + "</technology>\n";
            }
            
            xml += "</technologies>\n";
            xml += "</project>\n";
             
            XPathDocument doc = new XPathDocument(new StringReader(xml));
            XslCompiledTransform xslt = new XslCompiledTransform();
            StringWriter sw = new StringWriter();

            xslt.Load(path);

            // get image from database
            byte[] image = null;

            if (image != null)
            {
                string encodedImage = "data:image/jpg;base64,";
                encodedImage += base64Encode(image);
                XsltArgumentList argsList = new XsltArgumentList();
                argsList.AddParam("IMAGE", "", encodedImage);
                xslt.Transform(doc, argsList, sw);
            }
            else
                xslt.Transform(doc, null, sw);

            return sw.ToString();
        }

        private string base64Encode(byte[] bytes)
        {
            try
            {
                string encodedData = Convert.ToBase64String(bytes);
                return encodedData;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Encode " + e.Message);
            }
        }
    }
}