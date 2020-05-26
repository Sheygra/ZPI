using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Xml.Linq;
using ZPI_Projekt_Anonimizator.entity;

namespace ZPI_Projekt_Anonimizator.Generators
{
    public class DOCXGenerator : DocumentGenerator
    {

        public string generateDocument(Patient patientData)
        {   
            String projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            Directory.CreateDirectory(projectDirectory + @"\resource\temporary");
            String finalPath = "";

            String patientDisease = "";
            
            Random random = new Random();
            int rand = random.Next(5);
            try
            {
                using (StreamReader sr = new StreamReader(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\Generators\Files\Diseases.txt"))
                {
                    int i = 0;
                    while (!sr.EndOfStream)
                    {
                        if (i == rand)
                        {
                            patientDisease = sr.ReadLine();
                            break;
                        }
                        else
                        {
                            sr.ReadLine();
                            i++;
                        }
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex);
            }


            XDocument xdoc = new XDocument(
            new XDeclaration("1.0", "utf-8", "yes"),
            new XElement("Patient",
                new XElement("Id", patientData.Id),
                new XElement("Name", patientData.Name),
                new XElement("Surname", patientData.SurName),
                new XElement("Gender", patientData.Gender == "M" ? "Mężczyzna" : (patientData.Gender == "F" ? "Kobieta" : "Nieznana")),
                new XElement("Title", patientData.Gender=="M"?"Pan": (patientData.Gender == "F"?"Pani":"Pan/Pani")),
                new XElement("DateOfBirth", patientData.DateOfBirth),
                new XElement("Profession", patientData.Profession),
                new XElement("City", patientData.City),
                new XElement("Disease", patientDisease),
                new XElement("PhoneNumber", patientData.PhoneNumber)
            ));
                
            xdoc.Save(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\resource\temporary\item1.xml");

            try
            {
                String path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\resource\DOCX_files\historia_choroby_wzor_1.docx";
                FileInfo fileInfo = new FileInfo(path);
                String newPath = projectDirectory + @"\resource\temporary\" + fileInfo.Name + ".zip";
                File.Copy(path, newPath, true);
                using (ZipArchive archive = ZipFile.Open(newPath, ZipArchiveMode.Update))
                {
                    ZipArchiveEntry entry = archive.GetEntry("customXml/item1.xml");
                    if (entry != null)
                    {
                        entry.Delete();
                    }
                    archive.CreateEntryFromFile(projectDirectory + @"\resource\temporary\item1.xml", @"customXml\item1.xml");
                }
                finalPath = projectDirectory + @"\resource\DOCX_files\" + patientData.Id + "-" + DateTime.Now.ToFileTime() + ".docx";
                File.Move(newPath, finalPath, true);

            }
            catch (Exception Ex)
            {
                throw Ex;

            }

            Directory.Delete(projectDirectory + @"\resource\temporary", true);

            return finalPath;
        }
    }
}
