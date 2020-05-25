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
        public DataTable parseDocument(string path)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Title", typeof(String));
            table.Columns.Add("Subject", typeof(String));
            table.Columns.Add("Keywords", typeof(String));
            table.Columns.Add("Comment", typeof(String));
     
            try
            {
                BitmapDecoder decoder = null;
                using (Stream new_file_stream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    decoder = new JpegBitmapDecoder(new_file_stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                }
                BitmapFrame bitmapFrame = decoder.Frames[0];

                if (bitmapFrame != null)
                {
                    BitmapMetadata meta_Data = (BitmapMetadata)bitmapFrame.Metadata.Clone(); //metadane przykładowego obrazu                       

                    var row = table.NewRow();
                    row["Title"] = meta_Data.Title == null ? "" : meta_Data.Title;
                    row["Subject"] = meta_Data.Subject == null ? "" : meta_Data.Subject;
                    string s = "";
                    foreach(var keyword in meta_Data.Keywords)
                    {
                        s += keyword + " ";
                    }
                    row["Keywords"] = meta_Data.Keywords == null ? "" : s;
                    row["Comment"] = meta_Data.Comment == null ? "" : meta_Data.Comment;
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
