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
    public partial class Form1 : Form
    {
        private const string _fileName = "stock.csv";
        private CSVReader _csvReader;

        public Form1()
        {
            InitializeComponent();
            _csvReader = new CSVReader(@_fileName, dataGridView1);
            // load data on the init
            DisplayData();
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            stockIdBox.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            stockNameBox.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            stockCountBox.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            stockUnitBox.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            stockSizeBox.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
        }

        // insert button
        private void button2_Click(object sender, EventArgs e)
        {
            if (AddNewStock(
                int.Parse(stockIdBox.Text),
                stockNameBox.Text,
                int.Parse(stockCountBox.Text),
                stockUnitBox.Text,
                double.Parse(stockSizeBox.Text)))
            {
                // after adding new item update the table content
                DisplayData();
                ClearData();
            }
        }

        // update button
        private void button3_Click(object sender, EventArgs e)
        {
            if (stockIdBox.Text != "" && stockNameBox.Text != "" && stockCountBox.Text != "" && stockUnitBox.Text != "" && stockSizeBox.Text != "")
            {
                string updatedItem = $"{stockIdBox.Text},{stockNameBox.Text},{stockCountBox.Text},{stockUnitBox.Text},{stockSizeBox.Text}";
                _csvReader.UpdateItem(int.Parse(stockIdBox.Text), updatedItem);
                ClearData();
            }
            else
            {
                MessageBox.Show("Please selet stock item to update and fill all boxes.");
            }
        }

        // delete button
        private void button1_Click(object sender, EventArgs e)
        {
            if (stockIdBox.Text != "")
            {
                string item = $"{stockIdBox.Text},{stockNameBox.Text},{stockCountBox.Text},{stockUnitBox.Text},{stockSizeBox.Text}";
                _csvReader.DeleteItem(item);
                ClearData();
            }
            else
            {
                MessageBox.Show("Please select stock item to delete.");
            }
        }

        // menu window button
        private void button4_Click(object sender, EventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
        }

        // orders window button
        private void button5_Click(object sender, EventArgs e)
        {
            Orders orders = new Orders();
            orders.Show();
        }

        // display data in DataGridView
        private void DisplayData()
        {
            _csvReader.ReadFile();
        }

        private bool AddNewStock(int id, string name, int count, string unit, double size)
        {
            // check if any of the given inputs are empty or in the incorrect format
            if (count <= 0 || id <= 0 || size <= 0 || name.Length == 0 || unit.Length == 0) { return false; }

            try
            {
                using (StreamWriter file = new StreamWriter(@"stock.csv", true))
                {
                    file.WriteLine($"{id},{name},{count},{unit},{size}");
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error is " + ex.ToString());
                throw;
            }
        }

        // clear Data  
        private void ClearData()
        {
            stockIdBox.Text = "";
            stockNameBox.Text = "";
            stockCountBox.Text = "";
            stockUnitBox.Text = "";
            stockSizeBox.Text = "";
        }
    }
}