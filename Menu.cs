using System;
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
            _csvReader = new CSVReader(@_fileName);
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
            string[] param = { menuIdBox.Text, menuNameBox.Text, menuProductsBox.Text };

            if (_csvReader.InsertItem(param))
            {
                // after adding new item update the table content
                DisplayData();
                ClearData();
            }
            else
            {
                MessageBox.Show("Menu was not added.");
            }
        }

        // update button
        private void button2_Click(object sender, EventArgs e)
        {
            if (menuIdBox.Text != "" && menuNameBox.Text != "" && menuProductsBox.Text != "")
            {
                string updatedItem = $"{menuIdBox.Text},{menuNameBox.Text},{menuProductsBox.Text}";
                if (_csvReader.UpdateItem(int.Parse(menuIdBox.Text), updatedItem))
                {
                    ClearData();
                    DisplayData();
                }
                else
                {
                    MessageBox.Show("Menu was not updated.");
                }
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
                if (_csvReader.DeleteItem(int.Parse(menuIdBox.Text)))
                {
                    ClearData();
                    DisplayData();
                }
                else
                {
                    MessageBox.Show("Menu was not deleted.");
                }
            }
            else
            {
                MessageBox.Show("Please select stock item to delete.");
            }
        }

        // display data in DataGridView
        private void DisplayData()
        {
            dataGridView1.DataSource = _csvReader.ReadFile();
        }

        // clear Data  
        private void ClearData()
        {
            menuIdBox.Text = "";
            menuNameBox.Text = "";
            menuProductsBox.Text = "";
        }
    }
}