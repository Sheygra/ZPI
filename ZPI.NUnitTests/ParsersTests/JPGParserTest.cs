using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace ZPI.NUnitTests.ParsersTests
{
    [TestFixture]
    class JPGParserTest
    {
        ZPI_Projekt_Anonimizator.Parsers.JPGParser JPGParser;
        string path;
        DataTable table;

        [SetUp]
        public void Setup()
        {
            JPGParser = new ZPI_Projekt_Anonimizator.Parsers.JPGParser();
            path =  Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\resource\JPG_files\test.jpg";
        }

        [TestCase]
        public void CanBeCanseledBy_JPGParser_ReturnsWrongValue()
        {
            table = JPGParser.parseDocument(path);
            var values = table.Rows[0].ItemArray;
            Assert.IsNotNull(table);
            Assert.IsTrue(values[0].Equals("The document for PatientID 1"));
            Assert.IsTrue(values[1].Equals("Patient name - Jadwiga Jabłońska"));
            Assert.IsTrue(values[2].Equals("F kierowca Poznan "));
            Assert.IsTrue(values[3].Equals("Born Saturday, February 14, 1970"));
            Assert.IsTrue(values[4].Equals("5/23/2020 12:00:00 AM"));        
        }
    }
}
