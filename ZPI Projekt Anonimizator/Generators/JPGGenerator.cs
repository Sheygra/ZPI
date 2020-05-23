using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media.Imaging;
using ZPI_Projekt_Anonimizator.entity;

namespace ZPI_Projekt_Anonimizator.Generators
{
    class JPGGenerator : DocumentGenerator
    {
        private String resource_dir_path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\resource\";
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
                        BitmapMetadata meta_Data = (BitmapMetadata)bitmapFrame.Metadata.Clone(); //metadane przykładowego obrazu 

                        if (meta_Data != null)
                        {
                            meta_Data.Comment = "date of birth: " + patientData.DateOfBirth;
                            meta_Data.DateTaken = DateTime.Now.ToString();
                            meta_Data.Subject = "Patient " + patientData.Name + " " + patientData.SurName;
                            meta_Data.Title = "Document patientID: " + patientData.Id;
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

        private String generateNewFileName()
        {
            Random r = new Random();
            string s = "abcdefghijklmnoprstuwxyz123456789_";
            return "new_file_" + r.Next(1, 1000000) + s[r.Next(0, s.Length-1)] + ".jpg";
        }
    }
}
