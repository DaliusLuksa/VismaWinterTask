using System;
using System.Windows.Forms;

namespace VismaWinterTask
{
    public partial class Orders : Form
    {
        private const string _fileName = "orders.csv";
        private CSVReader _csvReader;

        public Orders()
        {
            InitializeComponent();
            _csvReader = new CSVReader(@_fileName);
            // load data on the init
            DisplayData();
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            orderIdBox.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            orderDatePicker.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            orderMenuBox.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
        }

        // insert button
        private void button1_Click(object sender, EventArgs e)
        {
            string[] param = { orderIdBox.Text, orderDatePicker.Value.ToString(), orderMenuBox.Text };

            if (_csvReader.InsertItem(param))
            {
                // after adding new item update the table content
                DisplayData();
                ClearData();
            }
            else
            {
                MessageBox.Show("Order was not added.");
            }
        }

        // update button
        private void button2_Click(object sender, EventArgs e)
        {
            if (orderIdBox.Text != "" && orderMenuBox.Text != "")
            {
                string updatedItem = $"{orderIdBox.Text},{orderDatePicker.Value},{orderMenuBox.Text}";
                if (_csvReader.UpdateItem(int.Parse(orderIdBox.Text), updatedItem))
                {
                    ClearData();
                    DisplayData();
                }
                else
                {
                    MessageBox.Show("Order was not updated.");
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
            if (orderIdBox.Text != "")
            {
                if (_csvReader.DeleteItem(int.Parse(orderIdBox.Text)))
                {
                    ClearData();
                    DisplayData();
                }
                else
                {
                    MessageBox.Show("Order was not deleted.");
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
            orderIdBox.Text = "";
            orderDatePicker.Text = "";
            orderMenuBox.Text = "";
        }
    }
}