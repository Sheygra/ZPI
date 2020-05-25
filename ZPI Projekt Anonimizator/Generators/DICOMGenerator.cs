using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Dicom;
using ZPI_Projekt_Anonimizator.entity;

namespace ZPI_Projekt_Anonimizator.Generators
{
    public class DICOMGenerator : DocumentGenerator
    {
        private String resource_dir_path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\resource\";
        private Random gen = new Random();
        private DateTime date_start = new DateTime(1920, 1, 1);

        public String generateDocument(Patient patientData)
        {
            String resource_file_path = resource_dir_path + "MRBRAIN.DCM";
            String new_file_path = resource_dir_path + generateNewFileName();

            try
            {
                var source_file = DicomFile.Open(resource_file_path);
                var output_file = source_file.Clone();
                output_file.Dataset.AddOrUpdate(DicomTag.PatientName, "Artur");
                output_file.Dataset.AddOrUpdate(DicomTag.PatientBirthDate, RandomDay().ToString());
                output_file.Dataset.AddOrUpdate(DicomTag.PatientSex, RandomSex());
                output_file.Dataset.AddOrUpdate(DicomTag.PatientWeight, gen.Next(200));
                output_file.Save(new_file_path);

                return new_file_path;
            }
            catch (Exception e)
            {
                return "EXCEPTION CAUGHT:" + e;
            }
        }
        private String generateNewFileName()
        {
            Random r = new Random();
            string s = "DicomFile_";

            return "new_file_" + r.Next(1, 1000000) + s[r.Next(0, s.Length - 1)] + ".DCM";
        }
        private DateTime RandomDay()
        {
            int range = (DateTime.Today - date_start).Days;
            return date_start.AddDays(gen.Next(range));
        }

        private String RandomSex()
        {
            String sex = "";
            int number = gen.Next(3);
            if (number == 0)
            {
                sex = "M";
            }
            else if (number == 1)
            {
                sex = "F";
            }
            return sex;
        }
    }
}
