using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media.Imaging;
using ZPI_Projekt_Anonimizator.entity;

namespace ZPI_Projekt_Anonimizator.Generators
{
    public class JPGGenerator : DocumentGenerator
    {
        private String resource_dir_path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\resource\JPG_files\";
        private String filename = "lungs.jpg";
        
        public String generateDocument(Patient patientData)
        {
            String resource_file_path = resource_dir_path + filename;
            String new_file_path = resource_dir_path + generateNewFileName();
            
            try
            {
                if (File.Exists(resource_file_path))
                {
                    File.Copy(resource_file_path, new_file_path, true);
                    BitmapDecoder decoder = null;
                    using (Stream new_file_stream = File.Open(new_file_path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    {
                         decoder = new JpegBitmapDecoder(new_file_stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                    }
                    BitmapFrame bitmapFrame = decoder.Frames[0];
                    
                    if (bitmapFrame != null)
                    {
                        BitmapMetadata meta_Data = (BitmapMetadata)bitmapFrame.Metadata.Clone();

                        if (meta_Data != null)
                        {
                            meta_Data.Comment = "Born " + patientData.DateOfBirth;
                            var data = getDateInCorrectFormat(patientData.DateOfBirth);
                            if (data != "")
                                meta_Data.DateTaken = data;
                            else meta_Data.DateTaken = "2100/01/01";
                            meta_Data.Subject = "Patient name - " + patientData.Name + " " + patientData.SurName;
                            meta_Data.Title = "The document for PatientID " + patientData.Id;
                            List<String> l = new List<string>{ patientData.Gender, patientData.Profession, patientData.City };
                            ReadOnlyCollection<String> keywords = new ReadOnlyCollection<String>(l);
                            meta_Data.Keywords = keywords;

                            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(bitmapFrame, bitmapFrame.Thumbnail, meta_Data, bitmapFrame.ColorContexts));
                            using (Stream new_file_stream = File.Open(new_file_path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                            {
                                new_file_stream.Flush();
                                encoder.Save(new_file_stream);
                            }
                        }
                    }
                    return new_file_path;
                }
                else return "RESOURSE FILE DOESNT EXIST";
            }
            catch(Exception e)
            {
                return "EXCEPTION CAUGHT:" + e;
            }
        }

        public String generateNewFileName()
        {
            Random r = new Random();
            string s = "abcdefghijklmnoprstuwxyz123456789_";
            return "new_file_" + r.Next(1, 1000000) + s[r.Next(0, s.Length-1)] + ".jpg";
        }
        
        private String getDateInCorrectFormat(String date)
        {
            var d = new DateTime();
            DateTime data = d;
            try
            {
                data = DateTime.Parse(date);
            }
            catch(Exception ex)
            {
                return "";
            }
            return d == data ? "" : data.ToString("yyyy/MM/dd").Replace('.','/');
        }
    }
}
