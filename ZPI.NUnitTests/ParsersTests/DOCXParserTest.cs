using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace ZPI.NUnitTests.ParsersTests
{
    [TestFixture]
    class DOCXParserTest
    {

        ZPI_Projekt_Anonimizator.Parsers.DOCXParser DOCXParser;
        string path;
        DataTable table;

        [SetUp]
        public void Setup()
        {
            DOCXParser = new ZPI_Projekt_Anonimizator.Parsers.DOCXParser();
            path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\resource\DOCX_files\test.docx";
        }

        [TestCase]
        public void CanBeCanseledBy_DOCXParser_ReturnsWrongValue()
        {
            table = DOCXParser.parseDocument(path);
            var values = table.Rows[0].ItemArray;
            Assert.IsNotNull(table);
            Assert.IsTrue(values[0].Equals("1"));
            Assert.IsTrue(values[1].Equals("Renata"));
            Assert.IsTrue(values[2].Equals("Jankowska"));
            Assert.IsTrue(values[3].Equals("Kobieta"));
            Assert.IsTrue(values[4].Equals("Pani"));
            Assert.IsTrue(values[9].Equals("933-877-938"));
            Assert.IsTrue(values[5].Equals("Friday, October 3, 1952"));
            Assert.IsTrue(values[6].Equals("sprzedawca"));
            Assert.IsTrue(values[7].Equals("Olawa"));
        }
    }
}
