using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VismaWinterTask
{
    public partial class Menu : Form
    {
        private const string _fileName = "menu.csv";
        private CSVReader _csvReader;

        public Menu()
        {
            InitializeComponent();
            _csvReader = new CSVReader(@_fileName, dataGridView1);
            // load data on the init
            DisplayData();
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            menuIdBox.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            menuNameBox.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            menuProductsBox.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
        }

        // insert button
        private void button1_Click(object sender, EventArgs e)
        {
            string[] products = menuProductsBox.Text.Split(new char[] { ' ' });

            if (AddNewMenu(
                int.Parse(menuIdBox.Text),
                menuNameBox.Text,
                products
                ))
            {
                // after adding new item update the table content
                DisplayData();
                ClearData();
            }
        }

        // update button
        private void button2_Click(object sender, EventArgs e)
        {
            if (menuIdBox.Text != "" && menuNameBox.Text != "" && menuProductsBox.Text != "")
            {
                string updatedItem = $"{menuIdBox.Text},{menuNameBox.Text},{menuProductsBox.Text}";
                _csvReader.UpdateItem(int.Parse(menuIdBox.Text), updatedItem);
                ClearData();
            }
            else
            {
                MessageBox.Show("Please selet stock item to update and fill all boxes.");
            }
        }

        // delete button
        private void button3_Click(object sender, EventArgs e)
        {
            if (menuIdBox.Text != "")
            {
                string item = $"{menuIdBox.Text},{menuNameBox.Text},{menuProductsBox.Text}";
                _csvReader.DeleteItem(item);
                ClearData();
            }
            else
            {
                MessageBox.Show("Please select stock item to delete.");
            }
        }

        // display data in DataGridView
        private void DisplayData()
        {
            _csvReader.ReadFile();
        }

        // clear Data  
        private void ClearData()
        {
            menuIdBox.Text = "";
            menuNameBox.Text = "";
            menuProductsBox.Text = "";
        }

        private bool AddNewMenu(int id, string name, string[] products)
        {
            // check if any of the given inputs are empty or in the incorrect format
            if (id <= 0 || name.Length == 0 || products.Length == 0) { return false; }

            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append($"{id},{name},");
                foreach (var product in products)
                {
                    builder.Append($"{product} ");
                }

                using (StreamWriter file = new StreamWriter(@_fileName, true))
                {
                    file.WriteLine(builder.ToString());
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error is " + ex.ToString());
                throw;
            }
        }

        
    }
}
