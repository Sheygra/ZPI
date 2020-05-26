using NUnit.Framework;

namespace ZPI.NUnitTests
{
    [TestFixture]
    public class DICOMGeneratorTests
    {
        ZPI_Projekt_Anonimizator.entity.Patient patient;
        ZPI_Projekt_Anonimizator.Generators.DICOMGenerator DICOM_gen;
        string s;

        [SetUp]
        public void Setup()
        {
            patient = new ZPI_Projekt_Anonimizator.entity.Patient("18922", "FFF", "XXX", "654728111", "Kwiatkowa 5", "K", "XD", "Wrocław", "00.00.2002");
            DICOM_gen = new ZPI_Projekt_Anonimizator.Generators.DICOMGenerator();

        }

        [TestCase]
        public void CanBeCanseledBy_GenerateDocument_ReturnsNullValue()
        {
            s = DICOM_gen.generateDocument(patient);
            Assert.IsNotNull(s);
            Assert.IsTrue(s.Contains("DCM"));
            Assert.IsTrue(s.Contains("resource"));
            Assert.IsTrue(s.Contains("DICOM_files"));
        }

        [TestCase]
        public void CanBeCanceledBy_GenerateFileName_WrongValue()
        {
            s = DICOM_gen.generateNewFileName();
            Assert.IsTrue(s.Contains("DCM"));
            Assert.IsTrue(s.Contains("new"));
            Assert.IsTrue(s.Contains("file"));
        }

        [TestCase]
        public void CanBeCanceledBy_RandomSex_WrongValue()
        {
            s = DICOM_gen.RandomSex();
            Assert.IsTrue(s.Equals("M") || s.Equals("F") || s.Equals(""));
            
        }
    }
}