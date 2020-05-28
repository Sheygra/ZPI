using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace ZPI.NUnitTests.ParsersTests
{
    [TestFixture]
    class DICOMParserTest
    {

        ZPI_Projekt_Anonimizator.Parsers.DICOMParser DICOMParser;
        string path;
        DataTable table;

        [SetUp]
        public void Setup()
        {
            DICOMParser = new ZPI_Projekt_Anonimizator.Parsers.DICOMParser();
            path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\resource\DICOM_files\test.dcm";
        }

        [TestCase]
        public void CanBeCanseledBy_DOCXParser_ReturnsNullValue()
        {
            table = DICOMParser.parseDocument(path);
            var values = table.Rows[0].ItemArray;
            Assert.IsNotNull(table);
            Assert.IsTrue(values[0].ToString().Contains("Katarzyna"));
            Assert.IsTrue(values[1].Equals("Monday, February 7, 1977"));
            Assert.IsTrue(values[2].Equals("F"));
            Assert.IsTrue(values[3].Equals("148"));        
        }
    }
}
