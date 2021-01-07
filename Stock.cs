using System;
using System.Windows.Forms;

namespace VismaWinterTask
{
    public partial class Stock : Form
    {
        private const string _fileName = "stock.csv";
        private CSVReader _csvReader;

        public Stock()
        {
            InitializeComponent();
            _csvReader = new CSVReader(@_fileName);
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
            string[] param = { stockIdBox.Text, stockNameBox.Text, stockCountBox.Text, stockUnitBox.Text, stockSizeBox.Text };
            if (_csvReader.InsertItem(param))
            {
                // item was successfully added
                // refresh table content
                // and clear input fields
                DisplayData();
                ClearData();
            }
            else
            {
                // item was not added
                // show error message
                MessageBox.Show("Item was not added. Please fill all the boxes.");
            }
        }

        // update button
        private void button3_Click(object sender, EventArgs e)
        {
            if (stockIdBox.Text != "" && stockNameBox.Text != "" && stockCountBox.Text != "" && stockUnitBox.Text != "" && stockSizeBox.Text != "")
            {
                string updatedItem = $"{stockIdBox.Text},{stockNameBox.Text},{stockCountBox.Text},{stockUnitBox.Text},{stockSizeBox.Text}";
                if (_csvReader.UpdateItem(int.Parse(stockIdBox.Text), updatedItem))
                {
                    ClearData();
                    DisplayData();
                }
                else
                {
                    MessageBox.Show("Stock item was not updated.");
                }
                
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
                if (_csvReader.DeleteItem(int.Parse(stockIdBox.Text)))
                {
                    ClearData();
                    DisplayData();
                }
                else
                {
                    MessageBox.Show("Stock item was not deleted.");
                }
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
            dataGridView1.DataSource = _csvReader.ReadFile();
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