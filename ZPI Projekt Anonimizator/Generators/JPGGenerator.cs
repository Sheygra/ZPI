using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;

namespace ZPI_Projekt_Anonimizator.Generators
{
    class JPGGenerator : DocumentGenerator
    {
        private String resource_file_path = @"C:\Users\kzakrzew\source\repos\ZPI Projekt Anonimizator\ZPI Projekt Anonimizator\resource\lungs.jpg";
 
        public String generateDocument(String patientData)
        {
            FileInfo originalImage = new FileInfo(resource_file_path);

            BitmapDecoder decoder = null;
            BitmapFrame bitmapFrame = null;
            BitmapMetadata metadata = null;

            if (File.Exists(resource_file_path))
            {
                using (Stream jpegStreamIn = File.Open(resource_file_path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    decoder = new JpegBitmapDecoder(jpegStreamIn, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                }
               
                metadata = (BitmapMetadata) decoder.Frames[0].Metadata;
                if(metadata != null)
                {

                }

                return "EXISTS";
            }
            else return "DOESNT EXIST";
        }
    }
}
