using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZPI.NUnitTests
{
    [TestFixture]
    class JPGGeneratorTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [TestCase]
        public void CanBeCanseledBy_GenerateDocument_ReturnsWrongValue()
        {
            var patient = new ZPI_Projekt_Anonimizator.entity.Patient("18922", "FFF", "XXX", "654728111", "Kwiatkowa 5", "K", "XD", "Wrocław", "00.00.2002");
            var JPG_gen = new ZPI_Projekt_Anonimizator.Generators.JPGGenerator();
            string s = JPG_gen.generateDocument(patient);
            Assert.IsNotNull(s);
            /*Assert.IsTrue(s.Contains("resource"));
            Assert.IsTrue(s.Contains("JPG_files"));*/
        }
    }
}
