using System;
using System.Collections.Generic;
using System.IO;
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

        // insert button (create new order)
        private void button1_Click(object sender, EventArgs e)
        {
            if (CheckInputs())
            {
                string[] param = { orderIdBox.Text, orderDatePicker.Value.ToString(), orderMenuBox.Text };

                // check if we can create a new order with given menu products
                if (CreateNewOrder(orderMenuBox.Text.Split(new char[] { ' ' })))
                {
                    // there's enough products for the order
                    // insert a new order into the orders.csv file
                    if (_csvReader.InsertItem(param))
                    {
                        // after adding new order update the table content
                        DisplayData();
                        ClearData();
                    }
                    else
                    {
                        MessageBox.Show("Order was not added.");
                    }
                }
                else
                {
                    MessageBox.Show("Order could not be completed. One of the menu's product was not found or the quantity was not enough.");
                }
            }
        }

        // NOT IN THE TASK
        // THAT'S WHAT HAPPEN WHEN I READ AFTER
        // I FINISHED WORKING...
        // update button
        // updates everything about the order but doesn't do
        // anything to the product's count (only visual)
        private void button2_Click(object sender, EventArgs e)
        {
            if (CheckInputs())
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
        }

        // NOT IN THE TASK
        // THAT'S WHAT HAPPEN WHEN I READ AFTER
        // I FINISHED WORKING...
        // delete button
        // doesn't return the products count back
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

        private bool CheckInputs()
        {
            // check if given id value is int
            // and is not null or empty
            try
            {
                if (orderIdBox.Text == null || orderIdBox.Text.Length == 0)
                {
                    MessageBox.Show("Id cannot be empty.");
                    return false;
                }

                int.Parse(orderIdBox.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Id can only be integers.");
                return false;
            }

            // check if given menu items value is int
            // and is not null or empty
            try
            {
                if (orderMenuBox.Text == null || orderMenuBox.Text.Length == 0)
                {
                    MessageBox.Show("Menu Items cannot be empty.");
                    return false;
                }

                var products = orderMenuBox.Text.Split(new char[] { ' ' });
                foreach (var product in products)
                {
                    int.Parse(product);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Menu Items can only be integers.");
                return false;
            }


            return true;
        }

        // display data in DataGridView
        private void DisplayData()
        {
            dataGridView1.DataSource = _csvReader.ReadFile();
        }

        // clear data  
        private void ClearData()
        {
            orderIdBox.Text = "";
            orderDatePicker.Text = "";
            orderMenuBox.Text = "";
        }

        private bool CreateNewOrder(string[] menuIds)
        {
            CSVReader menuCSV = new CSVReader("menu.csv");
            CSVReader stockCSV = new CSVReader("stock.csv");
            List<string> menuLines = menuCSV._lines;
            List<string> stockLines = stockCSV._lines;

            foreach (var menuID in menuIds)
            {
                if (!CheckMenu(int.Parse(menuID), menuLines, stockLines))
                {
                    // one of the menu products was not found
                    // or there wasn't enough

                    return false;
                }
            }

            // all menu products were found
            // update the stock.csv file
            using (StreamWriter file = new StreamWriter(@"stock.csv", false))
            {
                foreach (var line in stockLines)
                {
                    file.WriteLine(line);
                }

                file.Close();
            }

            return true;
        }

        private bool CheckMenu(int index, List<string> menuLines, List<string> stockLines)
        {
            for (int i = 1; i < menuLines.Count; i++)
            {
                var fields = menuLines[i].Split(new char[] { ',' });
                int id = int.Parse(fields[0]);
                if (id == index)
                {
                    // check if we have enough of this product
                    string[] products = fields[2].Split(new char[] { ' ' });
                    foreach (var productID in products)
                    {
                        if (!CheckStock(int.Parse(productID), stockLines))
                        {
                            // product was not found or was not enough
                            return false;
                        }
                    }
                }
            }

            // all products were found and there was enough
            return true;
        }

        private bool CheckStock(int index, List<string> stockLines)
        {
            for (int i = 1; i < stockLines.Count; i++)
            {
                var fields = stockLines[i].Split(new char[] { ',' });
                int id = int.Parse(fields[0]);
                if (id == index)
                {
                    // check if we have enough portions for the menu
                    int count = int.Parse(fields[2]);
                    if (count > 0)
                    {
                        // we have enough
                        // remove 1 from the count
                        // make a new line and update it
                        // without updating the file
                        // in case the order creation fails
                        string newLine = $"{fields[0]},{fields[1]},{count - 1},{fields[3]},{fields[4]}";
                        stockLines[i] = newLine;

                        return true;
                    }
                    else
                    {
                        // stock item was found but there was not enough
                        return false;
                    }
                }
            }

            // stock item was not found
            return false;
        }
    }
}