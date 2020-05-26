using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZPI.NUnitTests
{
    [TestFixture]
    class JPGGeneratorTests
    {
        ZPI_Projekt_Anonimizator.entity.Patient patient;
        ZPI_Projekt_Anonimizator.Generators.JPGGenerator JPG_gen;
        string s;

        [SetUp]
        public void Setup()
        {
            patient = new ZPI_Projekt_Anonimizator.entity.Patient("18922", "FFF", "XXX", "654728111", "Kwiatkowa 5", "K", "XD", "Wrocław", "00.00.2002");
            JPG_gen = new ZPI_Projekt_Anonimizator.Generators.JPGGenerator();
            
        }

        [TestCase]
        public void CanBeCanseledBy_GenerateDocument_ReturnsNullValue()
        {
            s = JPG_gen.generateDocument(patient);
            Assert.IsNotNull(s);
            Assert.IsTrue(s.Contains("jpg"));
            Assert.IsTrue(s.Contains("resource"));
            Assert.IsTrue(s.Contains("JPG_files"));
        }

        [TestCase]
        public void CanBeCanceledBy_GenerateFileName_WrongValue()
        {
            s = JPG_gen.generateNewFileName();
            Assert.IsTrue(s.Contains("jpg"));
            Assert.IsTrue(s.Contains("new"));
            Assert.IsTrue(s.Contains("file"));
        }
    }
}
