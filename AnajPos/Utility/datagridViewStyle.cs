using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnajPos.Utility
{
    public static class datagridViewStyle
    {
        public static void SetDesignToDG(DataGridView gv)
        {
            // Set general properties
            gv.AllowUserToAddRows = false;
            gv.BackgroundColor = System.Drawing.Color.FromArgb(248, 246, 255);
            gv.ForeColor = System.Drawing.Color.Gray;
            gv.Anchor = System.Windows.Forms.AnchorStyles.Top |
                                   System.Windows.Forms.AnchorStyles.Bottom |
                                   System.Windows.Forms.AnchorStyles.Left |
                                   System.Windows.Forms.AnchorStyles.Right;

            // Set column styles
            DataGridViewCellStyle headerCellStyle = new DataGridViewCellStyle
            {
                BackColor = System.Drawing.Color.FromArgb(65, 177, 225), // Adjust the color to your preference
                Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0), // Increase the font size
                ForeColor = System.Drawing.Color.White,
                SelectionBackColor = System.Drawing.Color.FromArgb(65, 177, 225),
                SelectionForeColor = System.Drawing.Color.White,
                WrapMode = System.Windows.Forms.DataGridViewTriState.True,
            };

            gv.ColumnHeadersDefaultCellStyle = headerCellStyle;
            gv.EnableHeadersVisualStyles = false;

            // Adjust header padding to remove the border
            gv.ColumnHeadersDefaultCellStyle.Padding = new Padding(0);

            // Set row styles
            DataGridViewCellStyle defaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0), // Increase the font size
                ForeColor = System.Drawing.Color.Black,
                SelectionBackColor = System.Drawing.Color.FromArgb(229, 243, 255),
                SelectionForeColor = System.Drawing.Color.Gray,
                WrapMode = System.Windows.Forms.DataGridViewTriState.False
            };

            DataGridViewCellStyle alternateCellStyle = new DataGridViewCellStyle
            {
                BackColor = System.Drawing.Color.FromArgb(240, 240, 240),
                Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0), // Increase the font size
                ForeColor = System.Drawing.Color.Black,
                SelectionBackColor = System.Drawing.Color.FromArgb(229, 243, 255),
                SelectionForeColor = System.Drawing.Color.Gray,
                WrapMode = System.Windows.Forms.DataGridViewTriState.False
            };

            gv.RowsDefaultCellStyle = defaultCellStyle;
            gv.AlternatingRowsDefaultCellStyle = alternateCellStyle;

            // Set other properties
            gv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            gv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            gv.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            gv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gv.MultiSelect = false;
            gv.ReadOnly = true;
            gv.RowHeadersVisible = false;
            gv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            gv.GridColor = System.Drawing.Color.FromArgb(209, 209, 209); // Set the grid line color
            gv.TabIndex = 101;
        }

        public static void SetColumnWidths(DataGridView gv, List<string> columnNames, List<int> columnWidths)
        {

            if (columnNames.Count != columnWidths.Count)
            {
                throw new ArgumentException("Number of column names must match the number of column widths.");
            }

            for (int i = 0; i < columnNames.Count; i++)
            {
                string columnName = columnNames[i];

                if (gv.Columns.Contains(columnName))
                {
                    DataGridViewColumn column = gv.Columns[columnName];
                    column.Width = columnWidths[i];
                }
            }
        }

        public static void SetColumnWidthsByIndex(DataGridView gv, List<int> columnIndexes, List<int> columnWidths)
        {
            if (columnIndexes.Count != columnWidths.Count)
            {
                throw new ArgumentException("Number of column indexes must match the number of column widths.");
            }

            for (int i = 0; i < columnIndexes.Count; i++)
            {
                int columnIndex = columnIndexes[i];

                if (columnIndex >= 0 && columnIndex < gv.Columns.Count)
                {
                    DataGridViewColumn column = gv.Columns[columnIndex];
                    column.Width = columnWidths[i];
                }
            }
        }
    }
}
