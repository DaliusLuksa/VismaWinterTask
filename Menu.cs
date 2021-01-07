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
            if (CheckInputs())
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
        }

        // update button
        private void button2_Click(object sender, EventArgs e)
        {
            if (CheckInputs())
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

        private bool CheckInputs()
        {
            // check if given id value is int
            // and is not null or empty
            try
            {
                if (menuIdBox.Text == null || menuIdBox.Text.Length == 0)
                {
                    MessageBox.Show("Id cannot be empty.");
                    return false;
                }

                if (int.Parse(menuIdBox.Text) <= 0)
                {
                    MessageBox.Show("Id cannot be negative or 0.");
                    return false;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Id can only be integer.");
                return false;
            }

            // check if given name value is not empty or null
            if (menuNameBox.Text == null || menuNameBox.Text.Length == 0)
            {
                MessageBox.Show("Name cannot be empty.");
                return false;
            }

            // check if given products value is int
            // and is not null or empty
            try
            {
                if (menuProductsBox.Text == null || menuProductsBox.Text.Length == 0)
                {
                    MessageBox.Show("Products cannot be empty.");
                    return false;
                }

                var products = menuProductsBox.Text.Split(new char[] { ' ' });
                foreach (var product in products)
                {
                    if (int.Parse(product) <= 0)
                    {
                        MessageBox.Show("Products cannot be negative or 0.");
                        return false;
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Products can only be integers.");
                return false;
            }

            
            return true;
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