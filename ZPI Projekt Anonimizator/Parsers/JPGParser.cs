using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;

namespace ZPI_Projekt_Anonimizator.Parsers
{
    class JPGParser : DocumentParser
    {
        public DataTable parseDocument(String dir_path)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Comment", typeof(String));
            table.Columns.Add("AplicationName", typeof(String));
            table.Columns.Add("Copyright", typeof(String));
            table.Columns.Add("Subject", typeof(String));
            table.Columns.Add("Title", typeof(String));
            
            try
            {
                string[] files = Directory.GetFiles(dir_path, "*.jpg");

                foreach (String file_path in files)
                {
                    if (Path.GetFileName(file_path) != "lungs.jpg")
                    {
                        BitmapDecoder decoder = null;
                        using (Stream new_file_stream = File.Open(file_path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                        {
                            decoder = new JpegBitmapDecoder(new_file_stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                        }
                        BitmapFrame bitmapFrame = decoder.Frames[0];

                        if (bitmapFrame != null)
                        {
                            BitmapMetadata meta_Data = (BitmapMetadata)bitmapFrame.Metadata.Clone(); //metadane przykładowego obrazu                       

                            var row = table.NewRow();
                            row["Comment"] = meta_Data.Comment;
                            row["AplicationName"] = meta_Data.ApplicationName;
                            row["Copyright"] = meta_Data.Copyright;
                            row["Subject"] = meta_Data.Subject;
                            row["Title"] = meta_Data.Title;
                            table.Rows.Add(row);
                        }
                    }
                }
                return table;
            }
            catch(Exception e)
            {
                var row = table.NewRow();
                row["Comment"] = e.ToString();
                table.Rows.Add(row);
                return table;
            }
        }
    }
}
