using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace VismaWinterTask
{
    public class CSVReader
    {
        private string _filename;
        private List<string> _lines;

        public CSVReader(string filename)
        {
            _filename = filename;
        }

        public DataTable ReadFile()
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

                return dt;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error is " + ex.ToString());
                return null;
            }
        }

        public bool InsertItem(string[] param)
        {
            // check if any of the given inputs are empty or in the incorrect format
            // this doesn't check if correct amount of params was passed.
            foreach (var item in param)
            {
                if (item == null || item.Length == 0)
                {
                    return false;
                }
            }

            // check if the new item's id already exists in the list
            // if exists then stop and return false
            // if doesn't then continue
            if (IsIdExists(int.Parse(param[0])))
            {
                return false;
            }

            try
            {
                StringBuilder builder = new StringBuilder();
                foreach (var item in param)
                {
                    builder.Append($"{item},");
                }
                // remove the last ',' from the line
                builder.Remove(builder.Length - 1, 1);
                using (StreamWriter file = new StreamWriter(@_filename, true))
                {
                    file.WriteLine(builder);
                }

                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error is " + ex.ToString());
                return false;
            }
        }

        public bool UpdateItem(int index, string newLine)
        {
            // check if the index is not <= 0
            // or newLine is empty (checking for correct input on different files would be painful)
            if (index <= 0 || newLine == "") 
            {
                //MessageBox.Show($"Index - {index} cannot be <= 0.");
                return false;
            }

            // because of date being written differently on each machine
            // instead of looking for a old line in the list
            // we have to split each line and look for a given id
            for (int i = 1; i < _lines.Count; i++)
            {
                int id = int.Parse(_lines[i].Split(new char[] { ',' })[0]);
                
                if (id == index)
                {
                    // found the item in the list
                    // now we can update the line with new one
                    _lines[i] = newLine;

                    try
                    {
                        using (StreamWriter file = new StreamWriter(@_filename, false))
                        {
                            foreach (var line in _lines)
                            {
                                file.WriteLine(line);
                            }

                            file.Close();
                        }

                        // we updated the line so we can now return
                        return true;
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("Error is " + ex.ToString());
                        return false;
                    }
                }
            }

            // line which we wanted to update was not found
            //MessageBox.Show("The item you wish to update was not found.");
            return false;
        }

        public bool DeleteItem(int index)
        {
            // check if index is <= 0
            // return false
            if (index <= 0) { return false; }

            bool isDeleted = false;

            try
            {
                //_lines.Remove(lineToDelete);
                for (int i = 1; i < _lines.Count; i++)
                {
                    int id = int.Parse(_lines[i].Split(new char[] { ',' })[0]);

                    if (index == id)
                    {
                        // found the item we want to delete
                        // remove the line from the list
                        // and set the isDeleted to true
                        _lines.RemoveAt(i);
                        isDeleted = true;
                        // we don't need to continue searching
                        // so we breaking...
                        break;
                    }
                }

                // check if the line was found and deleted
                if (!isDeleted)
                {
                    return false;
                }

                using (StreamWriter file = new StreamWriter(@_filename, false))
                {
                    foreach (var line in _lines)
                    {
                        file.WriteLine(line);
                    }

                    file.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error is " + ex.ToString());
                return false;
            }
        }

        private bool IsIdExists(int index)
        {
            // check if _lines is not null
            // if it is then try to populate
            if (_lines == null)
            {
                ReadFile();
            }

            for (int i = 1; i < _lines.Count; i++)
            {
                int id = int.Parse(_lines[i].Split(new char[] { ',' })[0]);

                if (index == id)
                {
                    // id already exists in the list
                    return true;
                }
            }

            // id was not found in the list
            return false;
        }
    }
}