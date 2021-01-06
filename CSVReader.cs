using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VismaWinterTask
{
    class CSVReader
    {
        private string _filename;
        private List<string> _lines;
        private DataGridView _dataTable;

        public CSVReader(string filename, DataGridView dataTable)
        {
            _filename = filename;
            _dataTable = dataTable;
        }

        public void ReadFile()
        {
            try
            {
                string CSVFilePathName = @_filename;
                string[] Lines = File.ReadAllLines(CSVFilePathName);
                _lines = new List<string>();
                foreach (var line in Lines)
                {
                    _lines.Add(line);
                }
                string[] Fields;
                Fields = Lines[0].Split(new char[] { ',' });
                int Cols = Fields.GetLength(0);
                DataTable dt = new DataTable();
                //1st row must be column names; force lower case to ensure matching later on.
                for (int i = 0; i < Cols; i++)
                    dt.Columns.Add(Fields[i], typeof(string));
                DataRow Row;
                for (int i = 1; i < Lines.GetLength(0); i++)
                {
                    Fields = Lines[i].Split(new char[] { ',' });
                    Row = dt.NewRow();
                    for (int f = 0; f < Cols; f++)
                        Row[f] = Fields[f];
                    dt.Rows.Add(Row);
                }
                _dataTable.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error is " + ex.ToString());
                throw;
            }
        }

        public void UpdateItem(string oldLine, string newLine)
        {
            if (_lines.FindIndex(o => o == oldLine) == -1)
            {
                MessageBox.Show("The item you wish to update was not found.");
            }
            else
            {
                _lines[_lines.FindIndex(o => o == oldLine)] = newLine;
            }
            

            try
            {
                using (StreamWriter file = new StreamWriter(@_filename, false))
                {
                    foreach(var line in _lines)
                    {
                        file.WriteLine(line);
                    }

                    file.Close();
                }

                ReadFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error is " + ex.ToString());
                throw;
            }
        }

        public void DeleteItem(string lineToDelete)
        {
            try
            {
                _lines.Remove(lineToDelete);

                using (StreamWriter file = new StreamWriter(@_filename, false))
                {
                    foreach (var line in _lines)
                    {
                        file.WriteLine(line);
                    }

                    file.Close();
                }

                ReadFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error is " + ex.ToString());
                throw;
            }
        }
    }
}
