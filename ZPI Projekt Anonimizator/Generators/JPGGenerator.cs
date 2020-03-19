using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;

namespace ZPI_Projekt_Anonimizator.Generators
{
    class JPGGenerator : DocumentGenerator
    {
        private String resource_dir_path = @"C:\Users\kzakrzew\source\repos\ZPI Projekt Anonimizator\ZPI Projekt Anonimizator\resource\";
        private String filename = "lungs.jpg";
        
        public String generateDocument(String patientData)
        {
            String resource_file_path = resource_dir_path + filename;
            String new_file_path = resource_dir_path + "new_img.jpg";
            FileInfo originalImage = new FileInfo(resource_dir_path);
            
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
                            meta_Data.Comment = "NEW COMMENT";

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
    }
}
