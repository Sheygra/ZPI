using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace ZPI_Projekt_Anonimizator.Generators
{
    class XMLGenerator : DocumentGenerator
    {

        List<ZPI_Projekt_Anonimizator.entity.Patient> dataBase;
        static Random random;
        private String data_generate_files = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\Generators\Files\";
        private String resource_dir_path_XML = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\resource\XML_files\";
        public XMLGenerator()
        {
            
            dataBase = new List<ZPI_Projekt_Anonimizator.entity.Patient>();
        }

        public string generateDocument(string patientData)
        {
            Random random = new Random();

            
            List<string> maleNames = fileToArray(data_generate_files + "MaleNames.txt");
            List<string> femaleNames = fileToArray(data_generate_files + "FemaleNames.txt");
            List<string> surnames = fileToArray(data_generate_files + "Surnames.txt");
            List<string> streets = fileToArray(data_generate_files + "Streets.txt");
            List<string> cities = fileToArray(data_generate_files + "Cities.txt");
            List<string> professions = fileToArray(data_generate_files + "Professions.txt");

            for (int i = 0; i< 10000; i++)
            {
                
                string name;
                string surname;
                string address;
                string phoneNumber;
                string gender;
                string city;
                string profession;
                string dateOfBirth;
                if(random.Next(0,100) > 50)
                {
                    name = choseRandomValueFromArray(maleNames);
                    surname = choseRandomValueFromArray(surnames);
                    gender = "M";
                } else
                {
                    name = choseRandomValueFromArray(femaleNames);
                    surname = choseRandomValueFromArray(surnames);
                    gender = "F";
                    if (surname.EndsWith('i')) surname = surname.Remove(surname.Length - 1, 1) + 'a';

                }
                address = generateRandomAdress(streets);
                phoneNumber = generateRandomPhoneNumber();
                city = generateRandomCity(cities);
                profession = generateRandomProfession(professions);
                dateOfBirth = generateRandomDateOfBirth();
                var patient = new ZPI_Projekt_Anonimizator.entity.Patient((i+1).ToString(),name,surname,phoneNumber,address, gender, profession, city, dateOfBirth);
                dataBase.Add(patient);
            }
           
            XDocument xdoc = new XDocument(
    new XDeclaration("1.0", "utf-8", "yes"),
        
        new XElement("Patients",
        from patient in dataBase
        select
            new XElement("Patient", new XElement("Id", patient.Id),
            new XElement("Name", patient.Name),
            new XElement("Surname", patient.SurName),
            new XElement("Gender", patient.Gender),
            new XElement("DateOfBirth", patient.DateOfBirth),
            new XElement("Profession", patient.Profession),
            new XElement("City", patient.City),
            new XElement("Address", patient.Address),
            new XElement("PhoneNumber", patient.PhoneNumber))));

            // Write the document to the file system            
            xdoc.Save(resource_dir_path_XML + "XMLtest" + dataBase.Count  + patientData +  ".xml");
            return "null";
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

        public string generateRandomPhoneNumber()
        {
            random = new Random();
            String number = "";
            number = random.Next(300, 999) + "-" + random.Next(100, 999) + "-" + random.Next(110, 999);
            return number;
        }

        public string generateRandomAdress(List<string> streets)
        {
            random = new Random();
            return choseRandomValueFromArray(streets) + " " + random.Next(0,200) + "/" + random.Next(1,100) + 
                ", " + random.Next(10,80) + "-" + random.Next(100,500);
        }

        public static String choseRandomValueFromArray(List<string> list)
        {
            random = new Random();
            return list[random.Next(list.Count)];
        }

        public string generateRandomCity(List<string> cities)
        {
            random = new Random();
            return choseRandomValueFromArray(cities);
        }

        public string generateRandomProfession(List<string> professions)
        {
            random = new Random();
            return choseRandomValueFromArray(professions);
        }

        public string generateRandomDateOfBirth()
        {
            DateTime start = new DateTime(1945, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(random.Next(range)).ToLongDateString();
        }
    }
}
