using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Dicom;

namespace ZPI_Projekt_Anonimizator.Parsers
{
    class DICOMParser : DocumentParser
    {
        public DataTable parseDocument(String dir_path)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Name", typeof(String));
            table.Columns.Add("BirthDate", typeof(String));
            table.Columns.Add("Sex", typeof(String));
            table.Columns.Add("Weight", typeof(String));

            try
            {
                string[] files = Directory.GetFiles(dir_path, "*.dcm");

                foreach (String file_path in files)
                {
                    var file = DicomFile.Open(file_path);
                    var row = table.NewRow();

                    row["Name"] = file.Dataset.GetSingleValue<string>(DicomTag.PatientName);
                    row["BirthDate"] = file.Dataset.GetSingleValue<string>(DicomTag.PatientBirthDate);
                    row["Sex"] = file.Dataset.GetSingleValue<string>(DicomTag.PatientSex);
                    row["Weight"] = file.Dataset.GetSingleValue<string>(DicomTag.PatientWeight);

                    table.Rows.Add(row);
                }

                return table;
            }
            catch (Exception e)
            {
                var row = table.NewRow();
                row["Comment"] = e.ToString();
                table.Rows.Add(row);
                return table;
            }
        }
    }
}
