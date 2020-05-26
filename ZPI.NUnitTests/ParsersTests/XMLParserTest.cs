using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace ZPI.NUnitTests.ParsersTests
{
    [TestFixture]
    class XMLParserTest
    {
        ZPI_Projekt_Anonimizator.Parsers.XMLParser XMLParser;
        string path;
        DataTable table;

        [SetUp]
        public void Setup()
        {
            XMLParser = new ZPI_Projekt_Anonimizator.Parsers.XMLParser();
            path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\resource\XML_files\test.xml";
        }

        [TestCase]
        public void CanBeCanseledBy_XMLParer_ReturnsWrongValue()
        {
            table = XMLParser.parseDocument(path);
            DataRow[] dr = table.Select("Id= 1");
            var s = dr[0].ItemArray;
            Assert.IsNotNull(table);
            Assert.IsNotNull(dr);
            Assert.IsTrue(s[0].ToString().Equals("1"));
            Assert.IsTrue(s[1].ToString().Equals("Edyta"));
            Assert.IsTrue(s[2].ToString().Equals("Wójcik"));
            Assert.IsTrue(s[3].ToString().Equals("F"));
            Assert.IsTrue(s[4].ToString().Equals("Monday, June 30, 2008"));
            
            
            
        }
    }
}
