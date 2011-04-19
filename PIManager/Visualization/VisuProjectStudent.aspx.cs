using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PIManager.Visualization
{
    public partial class VisuProjectStudent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            displayProjectList();
        }

        private void displayProjectList()
        {
            int rows = 10;
            int cols = 3;
            for (int row = 0; row < rows; row++)
            {
                TableRow newRow = new TableRow();
                ProjectTable.Controls.Add(newRow);
                for (int col = 0; col < cols; col++)
                {
                    TableCell newCell = new TableCell();
                    newCell.Text = "Cell " + row.ToString();
                    newCell.Text += "; Column " + col.ToString();
                    newCell.BorderStyle = BorderStyle.Solid;
                    newCell.BorderWidth = Unit.Pixel(1);
                    newCell.BorderColor = System.Drawing.Color.Black;
                    newRow.Controls.Add(newCell);
                }
            }
        }
    }
}