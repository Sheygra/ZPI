using System;
using ZPI_Projekt_Anonimizator.entity;

namespace ZPI_Projekt_Anonimizator.Generators
{
    public interface DocumentGenerator
    {
        public String generateDocument(Patient patientData);
    }
}
