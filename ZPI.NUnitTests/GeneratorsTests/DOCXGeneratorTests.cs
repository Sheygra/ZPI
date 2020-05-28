using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZPI.NUnitTests
{
    [TestFixture]
    class DOCXGeneratorTests
    {
        ZPI_Projekt_Anonimizator.entity.Patient patient;
        ZPI_Projekt_Anonimizator.Generators.DOCXGenerator DOCXGenerator;
        string s;

        [SetUp]
        public void Setup()
        {
            patient = new ZPI_Projekt_Anonimizator.entity.Patient("18922", "FFF", "XXX", "654728111", "Kwiatkowa 5", "K", "XD", "Wrocław", "00.00.2002");
            DOCXGenerator = new ZPI_Projekt_Anonimizator.Generators.DOCXGenerator();

        }

        [TestCase]
        public void CanBeCanseledBy_GenerateDocument_ReturnsNullValue()
        {
            s = DOCXGenerator.generateDocument(patient);
            Assert.IsNotNull(s);
            Assert.IsTrue(s.Contains("docx"));
            Assert.IsTrue(s.Contains("resource"));
            Assert.IsTrue(s.Contains("DOCX_files"));
        }

    }
}
