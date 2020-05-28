using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ZPI_Projekt_Anonimizator.Parsers;

namespace ZPI_Projekt_Anonimizator.Algorithm
{
    public class Anonymization
    {
        private string[] professionsMedicine;
        private string[] professionsEducation;
        private string[] professionsTransport;
        private string[] professionsSport;
        private string[] professionsTechnic;
        private string[] cities;
        private string[] regions;

        //////////////////////////////////Init Generalization Groups
        public Anonymization()
        {
            professionsMedicine = new string[] { "Ortopeda", "Chirurg", "Pielęgniarz", "Farmaceuta", "Kosmetolog", "Masażysta", "Fizjoterapeuta", "Optyk", "Weterynarz" };
            professionsEducation = new string[] { "Nauczyciel","Pedagog", "Profesor", "Wykładowca", "Bibliotekarz", "Opiekun", "Logopeda", "Dyrektor szkoły" };
            professionsTransport = new string[] { "Taksówkarz", "Kierowca", "Pilot", "Steward", "Dyspozytor", "Kurier", "Listonosz", "Motorniczy", "Dźwigowiec", "Konduktor" };
            professionsSport = new string[] { "Piłkarz", "Ciężarowiec", "Kulturysta", "Trener", "Siatkarz", "Koszykarz" };
            professionsTechnic = new string[] { "Tynkarz", "Architekt", "Hydraulik", "Informatyk", "Elektryk", "Stolarz", "Geodeta", "Murarz", "Dekarz", "Ślusarz" };
            cities = new string[] { "Wroclaw", "Olawa", "Jelenia Gora", "Olesnica", "Boleslawiec", "Warszawa", "Radom", "Plock", "Bielsko Biala", "Katowice", "Opole", "Bytom", "Leszno", "Poznan", "Kalisz", "Konin" };
            regions = new string[] { "Dolnoslaskie", "Dolnoslaskie", "Dolnoslaskie", "Dolnoslaskie", "Dolnoslaskie", "Mazowieckie", "Mazowieckie", "Mazowieckie", "Slaskie", "Slaskie", "Slaskie", "Slaskie", "Wielkopolskie", "Wielkopolskie", "Wielkopolskie", "Wielkopolskie" };
        }


        //////////////////////////////////Generalize Methods
        private String generalizeProfession(String profession)
        {
            if (profession.Equals("*") || profession.Equals("Medycyna") || profession.Equals("Edukacja") || profession.Equals("Transport") || profession.Equals("Sport") || profession.Equals("Technika") || profession.Equals("Unknown")) return "*";

            String generalizedProfession = "Unknown";
            if (professionsMedicine.Contains(profession)){
                generalizedProfession = "Medycyna";
            }else if(professionsEducation.Contains(profession)){
                generalizedProfession = "Edukacja";
            }else if (professionsTransport.Contains(profession)){
                generalizedProfession = "Transport";
            }else if (professionsSport.Contains(profession)){
                generalizedProfession = "Sport";
            }else if (professionsTechnic.Contains(profession)){
                generalizedProfession = "Technika";
            }  
            return generalizedProfession;
        }

        private String generalizeCity(String city)
        {
            if (regions.Contains(city) || city.Equals("*")) return "*";

            for (int i=0; i<cities.Length; i++)
            {
                if (cities[i].Equals(city))
                {
                    return regions[i];
                }
            }
            return "Unknown";
        }

        private String generalizeBirthDate(String birthDate)
        {
            if (birthDate.Equals("*") || birthDate.Equals("Dziecko") || birthDate.Equals("Mlodziez") || birthDate.Equals("Dorosly") || birthDate.Equals("Senior")) return "*";


            int personYear = Convert.ToInt16(birthDate.Substring(birthDate.Length - 4, 4));
            int age = DateTime.Now.Year - personYear;

            String ageGroup = "Unknown";

            if (age < 8){
                ageGroup = "Dziecko";}
            else if(age < 19){
                ageGroup = "Mlodziez";}
            else if(age < 61){
                ageGroup = "Dorosly";}
            else if(age >= 61){
                ageGroup = "Senior";}

            return ageGroup;
        }

        private String generalizeString(String text)
        {
            if (text.Equals("*")) return "*";

            String trimmedText = text;
            if (text[text.Length - 1].Equals('*'))
            {
                trimmedText = text.Substring(0, text.Length - 2) + "*";
            }
            else
            {
                trimmedText = text.Substring(0,text.Length-1)+"*";
            }
            return trimmedText;
        }

        private String generalizeAddress(String address)
        {
            if (address.Equals("*")) return "*";

            String trimmedAddress = address;

            if (address[address.Length - 1].Equals('*'))
            {
                if (address.Contains("/"))
                {
                    trimmedAddress = address.Substring(0, address.Length - 7) + "*";
                }
                else
                {
                    trimmedAddress = address.Substring(0, address.Length - 2) + "*";
                }
            }
            else
            {
                trimmedAddress = address.Substring(0, address.Length - 8) + "*";
            }
            
            return trimmedAddress;
        }

        private String generalizePhone(String number)
        {
            if (number.Equals("*")) return "*";

            String trimmedNumber = number;

            if (number[number.Length - 1].Equals('*'))
            {
                if (number.Contains("-"))
                {
                    trimmedNumber = number.Substring(0, number.Length - 5) + "*";
                }
                else
                {
                    trimmedNumber = number.Substring(0, number.Length - 2) + "*";
                }
            }
            else
            {
                trimmedNumber = number.Substring(0, number.Length - 4) + "*";
            }

            return trimmedNumber;
        }


        //////////////////////////////////Main Algorithm
        public DataTable AnonymizeData(DataTable DataTableFile, int k)
        {
            DataTable dataCopy = DataTableFile.Copy();

            List<(String, String, String, String, String, String, String, String)> representative = new List<(string, string, string, string, string, string, string, string)>();
            List<int> occurs = new List<int>();
            List<List<int>> originalIndexes = new List<List<int>>();
            
            //initial fill
            foreach (DataRow row in dataCopy.Rows)
            {
                var values = row.ItemArray;
                    representative.Add((values[1] + "", values[2] + "", values[3] + "", values[4] + "", values[5] + "", values[6] + "", values[7] + "", values[8] + ""));
                    occurs.Add(1);
                    List<int> init = new List<int>();
                    init.Add(Convert.ToInt16(values[0]) - 1);
                    originalIndexes.Add(init);
            }

            int a, b;

            do
            {
                int column = getColToGeneralize(representative, occurs, k);
                representative = generalizeValues(representative, occurs, k, column);
                (representative, occurs, originalIndexes) = designateRep(representative, occurs, originalIndexes);

                (a, b) = getMinOccur(occurs, k);
            } while (a>k || b>=k);

            return saveToDataTable(representative, originalIndexes, dataCopy);
        }


        //////////////////////////////////Helping Functions
        private (List<(String, String, String, String, String, String, String, String)>, List<int>, List<List<int>>) designateRep(List<(String, String, String, String, String, String, String, String)> representative, List<int> occurs, List<List<int>> originalIndexes){
            
            List<(String, String, String, String, String, String, String, String)> newRepresentative = new List<(string, string, string, string, string, string, string, string)>();
            List<int> newOccurs = new List<int>();
            List<List<int>> newOriginalIndexes = new List<List<int>>();

            int x = 0;
            int y = 0;

            foreach (var tuple in representative)
            {
                foreach(var newTuple in newRepresentative)
                {
                    if (newTuple.Equals(tuple))
                    {
                        newOccurs[y]++;
                        newOriginalIndexes[y].Add(x);
                    }
                    y++;
                }
                y = 0;
                if (!newRepresentative.Contains(tuple))
                {
                    newRepresentative.Add(tuple);
                    newOccurs.Add(occurs[x]);
                    newOriginalIndexes.Add(originalIndexes[x]);
                }
                x++;
            }

            return (newRepresentative, newOccurs, newOriginalIndexes);
        }

        private (int, int) getMinOccur(List<int> occurs, int k)
        {
            int count = 0;
            int count2 = 0;
            foreach(int x in occurs)
            {
                if (x < k)
                {
                    count++;
                    count2 += x;
                }
            }
            return (count, count2);
        }

        private int getColToGeneralize(List<(String, String, String, String, String, String, String, String)> representative, List<int> occurs, int k)
        {
            int column = 0;

            List<String> item1 = new List<String>();
            List<String> item2 = new List<String>();
            List<String> item3 = new List<String>();
            List<String> item4 = new List<String>();
            List<String> item5 = new List<String>();
            List<String> item6 = new List<String>();
            List<String> item7 = new List<String>();
            List<String> item8 = new List<String>();

            int i = 0;
            foreach(var tuple in representative)
            {
                if (occurs[i] < k)
                {
                    if (!item1.Contains(tuple.Item1)) { item1.Add(tuple.Item1); }
                    if (!item2.Contains(tuple.Item2)) { item2.Add(tuple.Item2); }
                    if (!item3.Contains(tuple.Item3)) { item3.Add(tuple.Item3); }
                    if (!item4.Contains(tuple.Item4)) { item4.Add(tuple.Item4); }
                    if (!item5.Contains(tuple.Item5)) { item5.Add(tuple.Item5); }
                    if (!item6.Contains(tuple.Item6)) { item6.Add(tuple.Item6); }
                    if (!item7.Contains(tuple.Item7)) { item7.Add(tuple.Item7); }
                    if (!item8.Contains(tuple.Item8)) { item8.Add(tuple.Item8); }
                }
            }

            int max = 0;
            if (item1.Count() > max) { max = item1.Count(); column = 1; }
            if (item2.Count() > max) { max = item2.Count(); column = 2; }
            if (item3.Count() > max) { max = item3.Count(); column = 3; }
            if (item4.Count() > max) { max = item4.Count(); column = 4; }
            if (item5.Count() > max) { max = item5.Count(); column = 5; }
            if (item6.Count() > max) { max = item6.Count(); column = 6; }
            if (item7.Count() > max) { max = item7.Count(); column = 7; }
            if (item8.Count() > max) { column = 8; }

            return column;
        }

        private List<(String, String, String, String, String, String, String, String)> generalizeValues(List<(String, String, String, String, String, String, String, String)> representative, List<int> occurs, int k, int columnToGeneralize)
        {

            List<(String, String, String, String, String, String, String, String)> copyRepresentative = new List<(string, string, string, string, string, string, string, string)>();
            switch (columnToGeneralize)
            {
                case 1:
                    for(int i = 0; i<representative.Count(); i++)
                    {
                        if (occurs[i] < k)
                        {
                            copyRepresentative.Add((generalizeString(representative[i].Item1), representative[i].Item2, representative[i].Item3, representative[i].Item4, representative[i].Item5, representative[i].Item6, representative[i].Item7, representative[i].Item8));
                        }
                        else
                        {
                            copyRepresentative.Add(representative[i]);
                        }
                    }
                    break;
                case 2:
                    for (int i = 0; i < representative.Count(); i++)
                    {
                        if (occurs[i] < k)
                        {
                            copyRepresentative.Add((representative[i].Item1, generalizeString(representative[i].Item2), representative[i].Item3, representative[i].Item4, representative[i].Item5, representative[i].Item6, representative[i].Item7, representative[i].Item8));
                        }
                        else
                        {
                            copyRepresentative.Add(representative[i]);
                        }
                    }
                    break;
                case 3:
                    for (int i = 0; i < representative.Count(); i++)
                    {
                        if (occurs[i] < k)
                        {
                            copyRepresentative.Add((representative[i].Item1, representative[i].Item2, generalizeString(representative[i].Item3), representative[i].Item4, representative[i].Item5, representative[i].Item6, representative[i].Item7, representative[i].Item8));
                        }
                        else
                        {
                            copyRepresentative.Add(representative[i]);
                        }
                    }
                    break;
                case 4:
                    for (int i = 0; i < representative.Count(); i++)
                    {
                        if (occurs[i] < k)
                        {
                            copyRepresentative.Add((representative[i].Item1, representative[i].Item2, representative[i].Item3, generalizeBirthDate(representative[i].Item4), representative[i].Item5, representative[i].Item6, representative[i].Item7, representative[i].Item8));
                        }
                        else
                        {
                            copyRepresentative.Add(representative[i]);
                        }
                    }
                    break;
                case 5:
                    for (int i = 0; i < representative.Count(); i++)
                    {
                        if (occurs[i] < k)
                        {
                            copyRepresentative.Add((representative[i].Item1, representative[i].Item2, representative[i].Item3, representative[i].Item4, generalizeProfession(representative[i].Item5), representative[i].Item6, representative[i].Item7, representative[i].Item8));
                        }
                        else
                        {
                            copyRepresentative.Add(representative[i]);
                        }
                    }
                    break;
                case 6:
                    for (int i = 0; i < representative.Count(); i++)
                    {
                        if (occurs[i] < k)
                        {
                            copyRepresentative.Add((representative[i].Item1, representative[i].Item2, representative[i].Item3, representative[i].Item4, representative[i].Item5, generalizeCity(representative[i].Item6), representative[i].Item7, representative[i].Item8));
                        }
                        else
                        {
                            copyRepresentative.Add(representative[i]);
                        }
                    }
                    break;
                case 7:
                    for (int i = 0; i < representative.Count(); i++)
                    {
                        if (occurs[i] < k)
                        {
                            copyRepresentative.Add((representative[i].Item1, representative[i].Item2, representative[i].Item3, representative[i].Item4, representative[i].Item5, representative[i].Item6, generalizeAddress(representative[i].Item7), representative[i].Item8));
                        }
                        else
                        {
                            copyRepresentative.Add(representative[i]);
                        }
                    }
                    break;
                case 8:
                    for (int i = 0; i < representative.Count(); i++)
                    {
                        if (occurs[i] < k)
                        {
                            copyRepresentative.Add((representative[i].Item1, representative[i].Item2, representative[i].Item3, representative[i].Item4, representative[i].Item5, representative[i].Item6, representative[i].Item7, generalizePhone(representative[i].Item8)));
                        }
                        else
                        {
                            copyRepresentative.Add(representative[i]);
                        }
                    }
                    break;
            }


            return copyRepresentative;
        }

        private DataTable saveToDataTable(List<(String, String, String, String, String, String, String, String)> representative, List<List<int>> originalIndexes, DataTable originalData)
        {

            DataTable newData = originalData.Copy();

            for(int i=0; i< representative.Count(); i++) {

                foreach(int id in originalIndexes[i])
                {
                    newData.Rows[id].BeginEdit();
                    newData.Rows[id][1] = representative[i].Item1;
                    newData.Rows[id][2] = representative[i].Item2;
                    newData.Rows[id][3] = representative[i].Item3;
                    newData.Rows[id][4] = representative[i].Item4;
                    newData.Rows[id][5] = representative[i].Item5;
                    newData.Rows[id][6] = representative[i].Item6;
                    newData.Rows[id][7] = representative[i].Item7;
                    newData.Rows[id][8] = representative[i].Item8;
                }
            }
            newData.AcceptChanges();

            return newData;
        }




        public String Test()
        {
            XMLParser parser = new XMLParser();
            DataTable testowyplik = parser.parseDocument(@"C:\Users\Michal\source\repos\ZPI2\ZPI Projekt Anonimizator\resource\XML_files\mojtest2.xml");
            
            return AnonymizeData(testowyplik, 9).Rows.Count+"";
        }






    }
}
