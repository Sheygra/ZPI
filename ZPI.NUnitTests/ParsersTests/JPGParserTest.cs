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
        public void CanBeCanseledBy_JPGParser_ReturnsNullValue()
        {
            table = JPGParser.parseDocument(path);
            var values = table.Rows[0].ItemArray;
            Assert.IsNotNull(table);
            Assert.IsNotNull(values[0]);
            Assert.IsNotNull(values[1]);
            Assert.IsNotNull(values[2]);
            Assert.IsNotNull(values[3]);
            Assert.IsNotNull(values[4]);
        }
    }
}
