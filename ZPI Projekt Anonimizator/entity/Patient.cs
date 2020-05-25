using System;
using System.Collections.Generic;
using System.Text;

namespace ZPI_Projekt_Anonimizator.entity
{
    public class Patient
    {
        private string id;
        private string name;
        private string surName;
        private string phoneNumber;
        private string address;
        private string gender;
        private string profession;
        private string city;
        private string dateOfBirth;
       
        public Patient(string id, string name, string surName, string phoneNumber, string address, string gender, string profession, string city, 
            string dateOfBirth)
        {
            this.Id = id;
            this.Name = name;
            this.SurName = surName;
            this.PhoneNumber = phoneNumber;
            this.Address = address;
            this.Gender = gender;
            this.Profession = profession;
            this.City = city;
            this.DateOfBirth = dateOfBirth;
        }



        public string SurName { get => surName; set => surName = value; }
        public string Name { get => name; set => name = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public string Address { get => address; set => address = value; }
        public string Id { get => id; set => id = value; }
        public string Gender { get => gender; set => gender = value; }
        public string Profession { get => profession; set => profession = value; }
        public string City { get => city; set => city = value; }
        public string DateOfBirth { get => dateOfBirth; set => dateOfBirth = value; }
        


    }
}
