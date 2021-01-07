using NUnit.Framework;
using System.Data;
using System.IO;
using System.Reflection;
using VismaWinterTask;

namespace NUnitTests
{
    public class CSVReaderTests
    {
        private string _filename = "stock.csv";
        private CSVReader _csvReader;

        [SetUp]
        public void Setup()
        {
             _csvReader = new CSVReader(_filename);
        }

        // INSERT TESTS

        [Test]
        public void Insert_WithInvalidInputs_ReturnFalse()
        {
            // put at least one wrong input
            string id = null;
            string[] param = { id, "", "10", "Kg", "1.1" };

            Assert.IsFalse(_csvReader.InsertItem(param));
        }

        [Test]
        public void Insert_WithNegativeId_ReturnFalse()
        {
            // set id to negative number
            string id = "-10";
            string[] param = { id, "", "10", "Kg", "1.1" };

            Assert.IsFalse(_csvReader.InsertItem(param));
        }

        [Test]
        public void Insert_WithExistingId_ReturnFalse()
        {
            // set id to already existing one
            string id = "2";
            string[] param = { id, "Chicken", "10", "Kg", "1.1" };

            Assert.IsFalse(_csvReader.InsertItem(param));
        }

        [Test]
        public void Insert_WithValidInputs_ReturnTrue()
        {
            // set id to non existant one
            string id = "500";
            string[] param = { id, "Chicken", "10", "Kg", "1.1" };

            Assert.IsTrue(_csvReader.InsertItem(param));
        }

        // UPDATE TESTS

        [Test]
        public void Update_WithInvalidId_ReturnFalse()
        {
            // to populate _lines list
            _csvReader.ReadFile();

            // id of the stock item that doesn't exist
            int id = 9123;
            string updatedItem = $"1,Name,20,Kg,0.5";

            Assert.IsFalse(_csvReader.UpdateItem(id, updatedItem));
        }

        [Test]
        public void Update_WithNegativeId_ReturnFalse()
        {
            // negative id
            int id = -10;
            string updatedItem = $"1,Name,20,Kg,0.5";

            Assert.IsFalse(_csvReader.UpdateItem(id, updatedItem));
        }

        [Test]
        public void Update_WithEmptyStringInput_ReturnFalse()
        {
            int id = 1;
            // empty string to pass
            string updatedItem = "";

            Assert.IsFalse(_csvReader.UpdateItem(id, updatedItem));
        }

        [Test]
        public void Update_WithValidInputs_ReturnTrue()
        {
            // to populate _lines list
            _csvReader.ReadFile();

            // any id that exists in the file
            int id = 500;
            string updatedItem = $"{id},Updated name,200,g,500.500";

            Assert.IsTrue(_csvReader.UpdateItem(id, updatedItem));
        }

        // DELETE TESTS

        [Test]
        public void Delete_WithInvalidId_ReturnFalse()
        {
            // to populate _lines list
            _csvReader.ReadFile();

            // id of the stock item that doesn't exist
            int id = 9123;

            Assert.IsFalse(_csvReader.DeleteItem(id));
        }

        [Test]
        public void Delete_WithNegativeId_ReturnFalse()
        {
            // negative id
            int id = -10;

            Assert.IsFalse(_csvReader.DeleteItem(id));
        }

        [Test]
        public void Delete_WithValidId_ReturnTrue()
        {
            // to populate _lines list
            _csvReader.ReadFile();

            // any id that exists in the file
            int id = 500;

            Assert.IsTrue(_csvReader.DeleteItem(id));
        }
    }
}