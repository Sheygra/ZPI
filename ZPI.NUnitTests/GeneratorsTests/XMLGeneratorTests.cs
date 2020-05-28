using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZPI.NUnitTests
{
    [TestFixture]
    class XMLGeneratorTests
    {

        ZPI_Projekt_Anonimizator.Generators.XMLGenerator XML_gen;
        string s;

        [SetUp]
        public void Setup()
        {        
            XML_gen = new ZPI_Projekt_Anonimizator.Generators.XMLGenerator();
        }


        [TestCase]
        public void CanBeCanseledBy_GenerateDocument_ReturnsNullValue()
        {
            s = XML_gen.generateDocument("test100", 100);
            Assert.IsNotNull(s);
            Assert.IsTrue(s.Contains("xml"));
            Assert.IsTrue(s.Contains("100"));
            Assert.IsTrue(s.Contains("resource"));
            Assert.IsTrue(s.Contains("XML_files"));
        }

        [TestCase]
        public void CanBeCanseledBy_GenerateRandomPhoneNumber_WrongValue()
        {
            s = XML_gen.generateRandomPhoneNumber();
            Assert.IsNotNull(s);
            Assert.IsTrue(s.ToCharArray().Length == 11);
        }

        [TestCase]
        public void CanBeCanceledBy_GenerateRandomAddress_WrongValue()
        {
            List<String> ulicy = new List<string>();
            ulicy.Add("Kalwadorska");
            s = XML_gen.generateRandomAdress(ulicy);
            Assert.IsNotNull(s);
            Assert.IsTrue(s.Contains("Kalwadorska"));
        }

        [TestCase]
        public void CanBeCanceledBy_GenerateRandomCyti_WrongValue()
        {
            List<String> miasta = new List<string>();
            miasta.Add("Olsztyn");
            s = XML_gen.generateRandomCity(miasta);
            Assert.IsNotNull(s);
            Assert.IsTrue(s.Contains("Olsztyn"));
        }

        [TestCase]
        public void CanBeCanceledBy_GenerateRandomProfession_WrongValue()
        {
            List<String> professions = new List<string>();
            professions.Add("stolarz");
            s = XML_gen.generateRandomProfession(professions);
            Assert.IsNotNull(s);
            Assert.IsTrue(s.Contains("stolarz"));
        }
    }
}
