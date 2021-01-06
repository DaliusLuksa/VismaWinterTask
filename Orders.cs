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
    public partial class Orders : Form
    {
        private const string _fileName = "orders.csv";
        private CSVReader _csvReader;

        private string _lastSelectedItem;

        public Orders()
        {
            InitializeComponent();
            _csvReader = new CSVReader(@_fileName, dataGridView1);
            // load data on the init
            DisplayData();
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            orderIdBox.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            orderDatePicker.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            orderMenuBox.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();

            _lastSelectedItem = $"{orderIdBox.Text},{orderDatePicker.Value},{orderMenuBox.Text}";
        }

        // insert button
        private void button1_Click(object sender, EventArgs e)
        {
            string[] products = orderMenuBox.Text.Split(new char[] { ' ' });

            if (AddNewOrder(
                int.Parse(orderIdBox.Text),
                orderDatePicker.Value,
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
            if (orderIdBox.Text != "" && orderMenuBox.Text != "")
            {
                string updatedItem = $"{orderIdBox.Text},{orderDatePicker.Value},{orderMenuBox.Text}";
                _csvReader.UpdateItem(_lastSelectedItem, updatedItem);
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
            if (orderIdBox.Text != "")
            {
                string item = $"{orderIdBox.Text},{orderDatePicker.Value},{orderMenuBox.Text}";
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
            orderIdBox.Text = "";
            orderDatePicker.Text = "";
            orderMenuBox.Text = "";
        }

        private bool AddNewOrder(int id, DateTime dateTime, string[] menuItems)
        {
            // check if any of the given inputs are empty or in the incorrect format
            if (id <= 0 || menuItems.Length == 0) { return false; }

            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append($"{id},{dateTime},");
                foreach (var product in menuItems)
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
