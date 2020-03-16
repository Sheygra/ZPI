using System;
using System.Collections.Generic;
using System.Text;

namespace ZPI_Projekt_Anonimizator.entity
{
    class Patient
    {
        private string id;
        private string name;
        private string surName;
        private string phoneNumber;
        private string address;

        public Patient(string id, string name, string surName, string phoneNumber, string address)
        {
            this.Id = id;
            this.Name = name;
            this.SurName = surName;
            this.PhoneNumber = phoneNumber;
            this.Address = address;
        }

        public string SurName { get => surName; set => surName = value; }
        public string Name { get => name; set => name = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public string Address { get => address; set => address = value; }
        public string Id { get => id; set => id = value; }
    }
}
