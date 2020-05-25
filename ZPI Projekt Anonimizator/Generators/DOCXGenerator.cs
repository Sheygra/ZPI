using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using ZPI_Projekt_Anonimizator.entity;

namespace ZPI_Projekt_Anonimizator.Generators
{
    class DOCXGenerator : DocumentGenerator
    {

        public string generateDocument(Patient patientData)
        {
            Random random = new Random();
            List<string> maleNames = fileToArray(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\Generators\Files\MaleNames.txt");
            List<string> femaleNames = fileToArray(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\Generators\Files\FemaleNames.txt");
            List<string> surnames = fileToArray(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\Generators\Files\Surnames.txt");
            List<string> streets = fileToArray(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\Generators\Files\Streets.txt");
            String projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            Directory.CreateDirectory(projectDirectory + @"\temporary");

            for (int i = 0; i < 1; i++)
            {
                string id;
                string name;
                string surname;
                string address;
                string phoneNumber;
                string pesel;
                if (random.Next(0, 100) > 50)
                {
                    name = choseRandomValueFromArray(maleNames);
                    surname = choseRandomValueFromArray(surnames);
                }
                else
                {
                    name = choseRandomValueFromArray(femaleNames);
                    surname = choseRandomValueFromArray(surnames);
                    if (surname.EndsWith('i')) surname = surname.Remove(surname.Length - 1, 1) + 'a';

                }
                id = generateRandomId();
                address = generateRandomAdress(streets);
                phoneNumber = generateRandomPhoneNumber();
                pesel = generateRandomPESEL();
                   
                XDocument xdoc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),

                new XElement("Patients",
                 new XElement("Patient", new XElement("Id", id),
                 new XElement("Name", name),
                 new XElement("Surname", surname),
                 new XElement("PESEL", pesel),
                 new XElement("Address", address),
                 new XElement("PhoneNumber", phoneNumber))));

                xdoc.Save(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\temporary\item1.xml");

                try
                {
                    String path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\resource\historia_choroby_wzor_1.docx";
                    FileInfo fileInfo = new FileInfo(path);
                    String newPath = projectDirectory + @"\temporary\" + fileInfo.Name + ".zip";
                    File.Copy(path, newPath, true);
                    Console.WriteLine(newPath);
                    using (ZipArchive archive = ZipFile.Open(newPath, ZipArchiveMode.Update))
                    {
                        ZipArchiveEntry entry = archive.GetEntry("customXml/item1.xml");
                        if (entry != null)
                        {
                            entry.Delete();
                        }
                        archive.CreateEntryFromFile(projectDirectory + @"\temporary\item1.xml", @"customXml\item1.xml");  
                    }

                    File.Move(newPath, projectDirectory + @"\Generators\Files\" + id + ".docx", true);

                }
                catch (Exception Ex)
                {
                    throw Ex;

                }

            }

            Directory.Delete(projectDirectory + @"\temporary", true);

            return "nowa sciezka";
        }

        public List<string> fileToArray(String filePath)
        {
            List<string> array = new List<string>();
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    while (!sr.EndOfStream)
                        array.Add(sr.ReadLine());
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex);
            }
            return array;
        }
        public string generateRandomId()
        {
            Random random = new Random();
            String id = "";
            id = DateTime.Now.ToFileTime() +"";
            return id;
        }
        public string generateRandomPhoneNumber()
        {
            Random random = new Random();
            String number = "";
            number = random.Next(0, 999) + "-" + random.Next(0, 999) + "-" + random.Next(0, 999);
            return number;
        }
        public string generateRandomPESEL()
        {
            Random random = new Random();
            String pesel = "";
            pesel = random.Next(50, 99) +"";
            int rand = random.Next(1, 12);
            if(rand < 10)
            {
                pesel += "0" + rand;
            }
            else
            {
                pesel += rand;
            }
            rand = random.Next(1, 31);
            if (rand < 10)
            {
                pesel += "0" + rand;
            }
            else
            {
                pesel += rand;
            }
            pesel += random.Next(0, 9);
            pesel += random.Next(0, 9);
            pesel += random.Next(0, 9);
            pesel += random.Next(0, 9);
            pesel += random.Next(0, 9);

            return pesel;
        }


        public string generateRandomAdress(List<string> streets)
        {
            Random random = new Random();
            return choseRandomValueFromArray(streets) + " " + random.Next(0, 200) + "/" + random.Next(1, 100) +
                ", " + random.Next(10, 80) + "-" + random.Next(100, 500);
        }

        public static String choseRandomValueFromArray(List<string> list)
        {
            Random random = new Random();
            return list[random.Next(list.Count)];
        }


    }
}
