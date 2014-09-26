using System;
using System.IO;
using System.Web.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
//using Microsoft.Office.Interop.Excel;
using Models;
using OfficeOpenXml;
using System.Data;


namespace BusinessLogic.General
{
    public sealed class ExportaExcel
    {
        public string pathFileName = "";
        public DbQueryResult status;
        public DbQueryResult SetDataTable_To_Excel(String nombre, System.Data.DataTable dtTable, System.Web.HttpResponse Response)
        {
            status = new DbQueryResult();
            status.Success = false;
            
            try
            {             
                    var pck = new OfficeOpenXml.ExcelPackage();
                    var ws = pck.Workbook.Worksheets.Add(nombre);

                    for (int c = 0; c < dtTable.Columns.Count; c++)
                    {
                        ws.Cells[1, (c + 1)].Value = dtTable.Columns[c].ColumnName;

                    }

                    int col = 1;
                    int row = 2;
                    foreach (DataRow rw in dtTable.Rows)
                    {
                        foreach (DataColumn cl in dtTable.Columns)
                        {
                            if (rw[cl.ColumnName] != DBNull.Value)
                            {
                                    ws.Cells[row, col].Value = rw[cl.ColumnName].ToString();                                                                
                            }
                            col++;
                        }
                        row++;
                        col = 1;
                    }
                   
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename="+nombre+".xlsx");
                    Response.BinaryWrite(pck.GetAsByteArray());
                    Response.End();   
            }
            catch (Exception c)
            {
                status.ErrorMessage = c.Message;
            }          
            return status;
        }
    }
}
