using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace ExportToExcelTools
{
    public class DataGridExcelTools
    {
        #region Attached Property

        /// <summary>
        /// Include current column in export report to excel
        /// </summary>
        public static readonly DependencyProperty IsExportedProperty = DependencyProperty.RegisterAttached("IsExported",
                                                                                        typeof(bool), typeof(DataGrid), new PropertyMetadata(true));

        /// <summary>
        /// Use custom header for report
        /// </summary>
        public static readonly DependencyProperty HeaderForExportProperty = DependencyProperty.RegisterAttached("HeaderForExport",
                                                                                        typeof(string), typeof(DataGrid), new PropertyMetadata(null));

        /// <summary>
        /// Use custom path to get value for report
        /// </summary>
        public static readonly DependencyProperty PathForExportProperty = DependencyProperty.RegisterAttached("PathForExport",
                                                                                        typeof(string), typeof(DataGrid), new PropertyMetadata(null));

        /// <summary>
        /// Use custom path to get value for report
        /// </summary>
        public static readonly DependencyProperty FormatForExportProperty = DependencyProperty.RegisterAttached("FormatForExport",
                                                                                        typeof(string), typeof(DataGrid), new PropertyMetadata(null));

        #region Attached properties helpers methods

        /* public void SetIsExported(DataGridColumn element, Boolean value)
        {

            element.SetValue(IsExportedProperty, value);
        }*/

        public Boolean GetIsExported(DataGridColumn element)
        {
            return (Boolean)element.GetValue(IsExportedProperty);
        }

        /* public void SetPathForExport(DataGridColumn element, string value)
         {
             element.SetValue(PathForExportProperty, value);
         }*/

        public string GetPathForExport(DataGridColumn element)
        {
            return (string)element.GetValue(PathForExportProperty);
        }

        /* public void SetHeaderForExport(DataGridColumn element, string value)
         {
             element.SetValue(HeaderForExportProperty, value);
         }*/

        public string GetHeaderForExport(DataGridColumn element)
        {
            return (string)element.GetValue(HeaderForExportProperty);
        }

        /* public void SetFormatForExport(DataGridColumn element, string value)
         {
             element.SetValue(FormatForExportProperty, value);
         }*/

        public string GetFormatForExport(DataGridColumn element)
        {
            return (string)element.GetValue(FormatForExportProperty);
        }

        #endregion

        #endregion

        /// <summary>
        /// Export data from <paramref name="grid"/> to Excel
        /// </summary>
        /// <param name="grid"></param>

        private int CursorPosition;
        private dynamic excel;
        private Workbooks workbook;
        private Worksheet worksheet;
        private Range rg;
        private Range rgHeader;
        public DataGridExcelTools()
        {
            CursorPosition = 1;
            excel = Microsoft.VisualBasic.Interaction.CreateObject("Excel.Application", string.Empty);
            //excel.ScreenUpdating = false;
            workbook = excel.workbooks;
            workbook.Add();
            worksheet = excel.ActiveSheet;
        }

        public void Clean()
        {
            // Show excel app
            excel.ScreenUpdating = true;
            excel.Visible = true;

            //Collect
            try
            {
                Marshal.ReleaseComObject(rg);
                Marshal.ReleaseComObject(rgHeader);
                Marshal.ReleaseComObject(worksheet);
                Marshal.ReleaseComObject(workbook);
                Marshal.ReleaseComObject(excel);
            }
            catch (Exception e)
            {

            }

            rg = null;
            rgHeader = null;
            worksheet = null;
            workbook = null;
            excel = null;
            GC.Collect();
        }
        /* public void ExportMatrixToExcel(DataGrid grid)
         {
             Thread thread = new Thread(StartExport);
             thread.Start(PrepareData(grid));
         }*/
        public void AddDataGrid(DataGrid grid)
        {
            //PrepareData(grid);
            ExportMatrixToExcel(PrepareData(grid) as object[,]);
            //Thread thread = new Thread(StartExport);
            //thread.Start(PrepareData(grid));
            //thread.Join();
        }
        public void AddText(String line)
        {
            ExportLineToExcel(line.Split('^'));
        }

        private void StartExport(object data)
        {
            ExportMatrixToExcel(data as object[,]);
        }

        private object[,] PrepareData(DataGrid grid)
        {
            // Get only columns which have binding or have custom binding
            //Filtering. is not emty and not null
            List<DataGridColumn> columns = grid.Columns.Where(x => (GetIsExported(x) && ((x is DataGridBoundColumn)
                                                 || (!string.IsNullOrEmpty(x.SortMemberPath))))).ToList();

            // Get list of items, bounded to grid
            List<object> list = grid.ItemsSource.Cast<object>().ToList();

            // Create data array (using array for data export optimization)
            object[,] data = new object[list.Count + 1, columns.Count];

            for (int columnIndex = 0; columnIndex < columns.Count; columnIndex++)
            {
                DataGridColumn gridColumn = columns[columnIndex];

                data[0, columnIndex] = GetHeader(gridColumn);

                string[] path = GetPath(gridColumn);

                string formatForExport = GetFormatForExport(gridColumn);

                if (path != null)
                {
                    // Fill data with values
                    for (int rowIndex = 1; rowIndex <= list.Count; rowIndex++)
                    {
                        object source = list[rowIndex - 1];
                        data[rowIndex, columnIndex] = GetValue(path, source, formatForExport);
                    }
                }
            }
            return data;
        }

        private string GetHeader(DataGridColumn column)
        {
            string headerForExport = GetHeaderForExport(column);
            if (headerForExport == null && column.Header != null)
                return column.Header.ToString();
            return headerForExport;
        }

        /// <summary>
        /// Get value of <paramref name="obj"/> by <paramref name="path"/>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="obj"></param>
        /// <param name="formatForExport"></param>
        /// <returns></returns>
        private object GetValue(string[] path, object obj, string formatForExport)
        {
            foreach (string pathStep in path)
            {
                if (obj == null)
                    return null;

                Type type = obj.GetType();
                PropertyInfo property = type.GetProperty(pathStep);

                if (property == null)
                {
                    Debug.WriteLine(string.Format("Couldn't find property '{0}' in type '{1}'", pathStep, type.Name));
                    return null;
                }

                obj = property.GetValue(obj, null);
            }

            if (!string.IsNullOrEmpty(formatForExport))
                return string.Format("{0:" + formatForExport + "}", obj);

            return obj;
        }

        /// <summary>
        /// Get path to get value. First try to get attached path value, then try to get path from binding
        /// </summary>
        /// <param name="gridColumn"></param>
        /// <returns></returns>
        private string[] GetPath(DataGridColumn gridColumn)
        {
            string path = GetPathForExport(gridColumn);

            if (string.IsNullOrEmpty(path))
            {
                if (gridColumn is DataGridBoundColumn)
                {
                    Binding binding = ((DataGridBoundColumn)gridColumn).Binding as Binding;
                    if (binding != null)                    
                        path = binding.Path.Path;                    
                }
                else                
                    path = gridColumn.SortMemberPath;                
            }

            return string.IsNullOrEmpty(path) ? null : path.Split('.');
        }
        public void ExportLineToExcel(object[] data)
        {
            const int left = 1;
            int top = CursorPosition;
            int height = 1;
            int width = data.GetLength(0);
            int bottom = top + height - 1;
            int right = left + width - 1;

            rg = worksheet.Range[worksheet.Cells[top, left], worksheet.Cells[bottom, right]];

            rg.Value = data;
            rg.Font.Bold = true;
            CursorPosition = (bottom + 2);
        }

        public void ExportMatrixToExcel(object[,] data)
        {
            const int left = 1;
            int top = CursorPosition;
            int height = data.GetLength(0);
            int width = data.GetLength(1);
            int bottom = top + height - 1;
            int right = left + width - 1;

            if (height == 0 || width == 0)
                return;

            rg = worksheet.Range[worksheet.Cells[top, left], worksheet.Cells[bottom, right]];

            rg.Value = data;

            // Set borders
            rg.Borders.LineStyle = XlLineStyle.xlContinuous;
            //rg.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            for (int i = 0; i < data.GetLength(1); i++)
            {
                (rg.Cells[1, i + 1] as Range).HorizontalAlignment = XlHAlign.xlHAlignCenter;
                for (int j = 1; j < data.GetLength(0); j++)
                {
                    if (j % 2 == 0)
                        (rg.Cells[j + 1, i + 1] as Range).Interior.Color = 255 * (int)Math.Pow(16, 4) + 255 * (int)Math.Pow(16, 2) + 255;
                    else
                        (rg.Cells[j + 1, i + 1] as Range).Interior.Color = 247 * (int)Math.Pow(16, 4) + 235 * (int)Math.Pow(16, 2) + 221;
                }

                switch (data[0, i].ToString()) { 
                    case "№": //if (data[0, i].ToString() == "№")
                        for (int j = 1; j < data.GetLength(0); j++)                    
                            (rg.Cells[j + 1, i + 1] as Range).HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    break;
                
                    case "Имя проекта":        //if (data[0, i].ToString() == "Имя проекта")
                        for (int j = 1; j < data.GetLength(0); j++)                    
                            (rg.Cells[j + 1, i + 1] as Range).HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    break;

                    case "Акционер": //if (data[0, i].ToString() == "Акционер")
                        for (int j = 1; j < data.GetLength(0); j++)                    
                            (rg.Cells[j + 1, i + 1] as Range).HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    break;
                    case "Сумма": // if (data[0, i].ToString() == "Сумма")                
                        for (int j = 1; j < data.GetLength(0); j++)                    
                            (rg.Cells[j + 1, i + 1] as Range).NumberFormat = "### ##0,00";
                        //(rg.Cells[j + 1, i + 1] as Range).Font.Size = 22;
                    break;
                
                    case "Сумма ($)": //if (data[0, i].ToString() == "Сумма ($)")                
                        for (int j = 1; j < data.GetLength(0); j++)                    
                            (rg.Cells[j + 1, i + 1] as Range).NumberFormat = "# ##0,00 \\$";
                    break;
                
                    case "Процент":  //if (data[0, i].ToString() == "Процент")
                    {
                        for (int j = 1; j < data.GetLength(0); j++)
                        {
                            (rg.Cells[j + 1, i + 1] as Range).HorizontalAlignment = XlHAlign.xlHAlignCenter;
                            (rg.Cells[j + 1, i + 1] as Range).NumberFormat = "# ##,00 \\%";
                        }
                    }

                    break;
                    case "Банк":
                    for (int j = 1; j < data.GetLength(0); j++)
                        (rg.Cells[j + 1, i + 1] as Range).HorizontalAlignment = XlHAlign.xlHAlignLeft;
                    break;

                    case "На кого получено":
                    for (int j = 1; j < data.GetLength(0); j++)
                        (rg.Cells[j + 1, i + 1] as Range).HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    break;

                    case "РКО":
                    for (int j = 1; j < data.GetLength(0); j++)
                        (rg.Cells[j + 1, i + 1] as Range).HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    break;

                    case "Дата транзакции":
                    for (int j = 1; j < data.GetLength(0); j++)
                        (rg.Cells[j + 1, i + 1] as Range).HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    break;

                    case "Валюта":
                    for (int j = 1; j < data.GetLength(0); j++)
                        (rg.Cells[j + 1, i + 1] as Range).HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    break;

                    case "Курс валют":
                    for (int j = 1; j < data.GetLength(0); j++)
                        (rg.Cells[j + 1, i + 1] as Range).HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    break;

                    case "Примечание":
                    for (int j = 1; j < data.GetLength(0); j++)
                        (rg.Cells[j + 1, i + 1] as Range).HorizontalAlignment = XlHAlign.xlHAlignLeft;
                    break;

                    case "Признак":
                    for (int j = 1; j < data.GetLength(0); j++)
                        (rg.Cells[j + 1, i + 1] as Range).HorizontalAlignment = XlHAlign.xlHAlignCenter ;
                    break;
            }
            }

            // Set auto columns width
            rg.EntireColumn.AutoFit();

            // Set header view
            rgHeader = worksheet.Range[worksheet.Cells[top, left], worksheet.Cells[top, right]];
            rgHeader.Font.Bold = true;
            rgHeader.Font.Color = 255 * (int)Math.Pow(16, 4) + 255 * (int)Math.Pow(16, 2) + 255;
            rgHeader.Interior.Color = 177 * (int)Math.Pow(16, 4) + 72 * (int)Math.Pow(16, 2) + 6; // #4E81BD            
            CursorPosition = (bottom + 2);
        }
    }
}
