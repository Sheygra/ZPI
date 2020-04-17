using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Text;


namespace ZPI_Projekt_Anonimizator.Parsers
{

    class DOCXParser : DocumentParser
    {
        public DataTable parseDocument(string path)
        {
            String projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

            DataTable objDataTable;
            DataSet ds;
            try
            {
                objDataTable = new DataTable();
                FileInfo fileInfo = new FileInfo(path);
                String newPath = projectDirectory + @"\temporary\" + fileInfo.Name + ".zip";
                Directory.CreateDirectory(projectDirectory + @"\temporary"); 
                File.Copy(path, newPath, true);
                ds = new DataSet();

                using (ZipArchive archive = ZipFile.Open(newPath, ZipArchiveMode.Update))
                {
                    archive.ExtractToDirectory(projectDirectory + @"\temporary\extract", true);
                    File.Copy(projectDirectory + @"\temporary\extract\customXml\item1.xml", projectDirectory + @"\temporary\item1.xml", true);

                    //podmiana pliku item1.xml
                    //if(archive.GetEntry(@"customXml\item1.xml") !=null)
                    //    archive.GetEntry(@"customXml\item1.xml").Delete();
                    //archive.CreateEntryFromFile(projectDirectory + @"\temporary\item1.xml", @"customXml\item1.xml");                    
                }

                //File.Move(newPath, projectDirectory + @"\temporary\" + fileInfo.Name.Remove(fileInfo.Name.Length-5)+ "_anonymised.docx", true);

                ds.ReadXml(projectDirectory + @"\temporary\item1.xml");
                objDataTable = ds.Tables[0];
                Directory.Delete(projectDirectory + @"\temporary", true);



            }
            catch (Exception Ex)
            {
                throw Ex;
                
            }
            return objDataTable;
        }
    }
}
