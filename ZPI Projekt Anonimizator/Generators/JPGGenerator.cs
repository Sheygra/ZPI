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
            FileInfo originalImage = new FileInfo(resource_dir_path);

            BitmapDecoder decoder = null;
            BitmapFrame bitmapFrame = null;
            BitmapMetadata metadata = null;
            try
            {
                if (File.Exists(resource_file_path))
                {
                    using (Stream jpegStreamIn = File.Open(resource_file_path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    {

                        decoder = new JpegBitmapDecoder(jpegStreamIn, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                    }

                    bitmapFrame = decoder.Frames[0];

                    if (bitmapFrame != null)
                    {
                        BitmapMetadata meta_Data = (BitmapMetadata)bitmapFrame.Metadata.Clone(); //metadane przykładowego obrazu 

                        if (meta_Data != null)
                        { 
                            meta_Data.Comment = "NEW COMMENT";
                            
                            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(bitmapFrame, bitmapFrame.Thumbnail, meta_Data, bitmapFrame.ColorContexts));

                            using (Stream jpegStreamOut = File.Open(resource_dir_path + "new_img.jpg", FileMode.CreateNew, FileAccess.ReadWrite))
                            {
                                encoder.Save(jpegStreamOut);
                            }
                        
                        } 
                    }

                    return "EXISTS";
                }
                else return "DOESNT EXIST";
            }
            catch(Exception e)
            {
                return "EXCEPTION CAUGHT:" + e;
            }
        }
    }
}
