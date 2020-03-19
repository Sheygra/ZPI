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
        String resultDir;

        public XMLGenerator(String resultDir)
        {
            this.resultDir = resultDir;
            dataBase = new List<ZPI_Projekt_Anonimizator.entity.Patient>();
        }

        public string generateDocument(string patientData)
        {
            Random random = new Random();
            List<string> maleNames = fileToArray(@"C:/Users/Artiom/Desktop/aninimizator/ZPI/ZPI Projekt Anonimizator/Generators/Files/MaleNames.txt");
            List<string> femaleNames = fileToArray(@"C:/Users/Artiom/Desktop/aninimizator/ZPI/ZPI Projekt Anonimizator/Generators/Files/FemaleNames.txt");
            List<string> surnames = fileToArray(@"C:/Users/Artiom/Desktop/aninimizator/ZPI/ZPI Projekt Anonimizator/Generators/Files/Surnames.txt");
            List<string> streets = fileToArray(@"C:/Users/Artiom/Desktop/aninimizator/ZPI/ZPI Projekt Anonimizator/Generators/Files/Streets.txt");

            for (int i = 0; i< 10000; i++)
            {
                
                string name;
                string surname;
                string address;
                string phoneNumber;
                if(random.Next(0,100) > 50)
                {
                    name = choseRandomValueFromArray(maleNames);
                    surname = choseRandomValueFromArray(surnames);
                } else
                {
                    name = choseRandomValueFromArray(femaleNames);
                    surname = choseRandomValueFromArray(surnames);
                    if (surname.EndsWith('i')) surname = surname.Remove(surname.Length - 1, 1) + 'a';

                }
                address = generateRandomAdress(streets);
                phoneNumber = generateRandomPhoneNumber();
                var patient = new ZPI_Projekt_Anonimizator.entity.Patient((i+1).ToString(),name,surname,phoneNumber,address);
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
            new XElement("Address", patient.Address),
            new XElement("PhoneNumber", patient.PhoneNumber))));

            // Write the document to the file system            
            xdoc.Save(@"C:/Users/Artiom/Desktop/aninimizator/ZPI/ZPI Projekt Anonimizator/Generators/Files/XMLtest" + patientData + ".txt");
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
            Random random = new Random();
            String number = "";
            number = random.Next(0, 999) + "-" + random.Next(0, 999) + "-" + random.Next(0, 999);
            return number;
        }

        public string generateRandomAdress(List<string> streets)
        {
            Random random = new Random();
            return choseRandomValueFromArray(streets) + " " + random.Next(0,200) + "/" + random.Next(1,100) + 
                ", " + random.Next(10,80) + "-" + random.Next(100,500);
        }

        public static String choseRandomValueFromArray(List<string> list)
        {
            Random random = new Random();
            return list[random.Next(list.Count)];
        }

        
    }
}
