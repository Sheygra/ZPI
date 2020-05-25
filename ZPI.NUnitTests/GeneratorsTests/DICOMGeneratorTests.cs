using NUnit.Framework;

namespace ZPI.NUnitTests
{
    [TestFixture]
    public class DICOMGeneratorTests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [TestCase]
        public void CanBeCanseledBy_GenerateDocument_ReturnsWrongValue()
        {
            var patient = new ZPI_Projekt_Anonimizator.entity.Patient("18922", "FFF", "XXX", "654728111", "Kwiatkowa 5", "K", "XD", "Wrocław", "00.00.2002");
            var DICOM_gen = new ZPI_Projekt_Anonimizator.Generators.DICOMGenerator();
            string s = DICOM_gen.generateDocument(patient);  
            Assert.IsNotNull(s);
           /* Assert.IsTrue(s.Contains("resourse"));
            Assert.IsTrue(s.Contains("DICOM_files"));*/
        }
    }
}