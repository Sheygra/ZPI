using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace ZPI.NUnitTests.AnonimizationTests
{

    [TestFixture]
    class KAnonimizationOneTest
    {

        ZPI_Projekt_Anonimizator.Algorithm.Anonymization anon;
        ZPI_Projekt_Anonimizator.Parsers.XMLParser XMLParser;
        string path;
        DataTable dataTable;
        DataTable anonDataTable;

        [SetUp]
        public void Setup()
        {
            anon = new ZPI_Projekt_Anonimizator.Algorithm.Anonymization();
            XMLParser = new ZPI_Projekt_Anonimizator.Parsers.XMLParser();
            
        }
        [TestCase]
        public void CanBeCanceledBy_FirstAnonimizationFor100Records_ReturnsWrongValue()
        {
            path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\resource\XML_files\test100.xml";
            dataTable = XMLParser.parseDocument(path);
            anonDataTable = anon.AnonymizeData(dataTable, 2);
            List<string> userArr = new List<string>();
            for (int i = 0; i < anonDataTable.Rows.Count; i++)
            {
                userArr.Add(anonDataTable.Rows[i][1].ToString() + anonDataTable.Rows[i][2].ToString() + anonDataTable.Rows[i][3].ToString() + anonDataTable.Rows[i][4].ToString() + anonDataTable.Rows[i][5].ToString() + anonDataTable.Rows[i][6].ToString());
            }
            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach (string item in userArr)
            {
                if (!dict.ContainsKey(item))
                {
                    dict.Add(item, 1);
                }
                else
                {
                    int count = 0;
                    dict.TryGetValue(item, out count);
                    dict.Remove(item);
                    dict.Add(item, count + 1);
                }
            }
            Assert.IsTrue(!dict.ContainsValue(1));

        }

        [TestCase]
        public void CanBeCanceledBy_FirstAnonimizationFor500Records_ReturnsWrongValue()
        {
            path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\resource\XML_files\test500.xml";
            dataTable = XMLParser.parseDocument(path);
            anonDataTable = anon.AnonymizeData(dataTable, 3);
            List<string> userArr = new List<string>();
            for (int i = 0; i < anonDataTable.Rows.Count; i++)
            {
                userArr.Add(anonDataTable.Rows[i][1].ToString() + anonDataTable.Rows[i][2].ToString() + anonDataTable.Rows[i][3].ToString() + anonDataTable.Rows[i][4].ToString() + anonDataTable.Rows[i][5].ToString() + anonDataTable.Rows[i][6].ToString());
            }
            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach (string item in userArr)
            {
                if (!dict.ContainsKey(item))
                {
                    dict.Add(item, 1);
                }
                else
                {
                    int count = 0;
                    dict.TryGetValue(item, out count);
                    dict.Remove(item);
                    dict.Add(item, count + 1);
                }
            }
            Assert.IsTrue(!dict.ContainsValue(1) && !dict.ContainsValue(2));

        }

        [TestCase]
        public void CanBeCanceledBy_FirstAnonimizationFor1000Records_ReturnsWrongValue()
        {
            path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\resource\XML_files\test1000.xml";
            dataTable = XMLParser.parseDocument(path);
            anonDataTable = anon.AnonymizeData(dataTable, 5);
            List<string> userArr = new List<string>();
            for (int i = 0; i < anonDataTable.Rows.Count; i++)
            {
                userArr.Add(anonDataTable.Rows[i][1].ToString() + anonDataTable.Rows[i][2].ToString() + anonDataTable.Rows[i][3].ToString() + anonDataTable.Rows[i][4].ToString() + anonDataTable.Rows[i][5].ToString() + anonDataTable.Rows[i][6].ToString());
            }
            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach (string item in userArr)
            {
                if (!dict.ContainsKey(item))
                {
                    dict.Add(item, 1);
                }
                else
                {
                    int count = 0;
                    dict.TryGetValue(item, out count);
                    dict.Remove(item);
                    dict.Add(item, count + 1);
                }
            }
            Assert.IsTrue(!dict.ContainsValue(1) && !dict.ContainsValue(2) && !dict.ContainsValue(3) && !dict.ContainsValue(4));

        }

        [TestCase]
        public void CanBeCanceledBy_FirstAnonimizationFor10000Records_ReturnsWrongValue()
        {
            path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\resource\XML_files\test10000.xml";
            dataTable = XMLParser.parseDocument(path);
            anonDataTable = anon.AnonymizeData(dataTable, 10);
            List<string> userArr = new List<string>();
            for (int i = 0; i < anonDataTable.Rows.Count; i++)
            {
                userArr.Add(anonDataTable.Rows[i][1].ToString() + anonDataTable.Rows[i][2].ToString() + anonDataTable.Rows[i][3].ToString() + anonDataTable.Rows[i][4].ToString() + anonDataTable.Rows[i][5].ToString() + anonDataTable.Rows[i][6].ToString());
            }
            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach (string item in userArr)
            {
                if (!dict.ContainsKey(item))
                {
                    dict.Add(item, 1);
                }
                else
                {
                    int count = 0;
                    dict.TryGetValue(item, out count);
                    dict.Remove(item);
                    dict.Add(item, count + 1);
                }
            }
            Assert.IsTrue(!dict.ContainsValue(1) && !dict.ContainsValue(2) && !dict.ContainsValue(3) && !dict.ContainsValue(4) 
                && !dict.ContainsValue(5) && !dict.ContainsValue(6)
                 && !dict.ContainsValue(7) && !dict.ContainsValue(8) && !dict.ContainsValue(9));

        }
    }
}
