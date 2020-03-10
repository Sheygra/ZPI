using System;
using System.Collections.Generic;
using System.Text;

namespace ZPI_Projekt_Anonimizator.Generators
{
    interface DocumentGenerator
    {
        public String generateDocument(String patientData); // bierze dane pacjenta, generuje dokument i zwraca sciezke do niego
        
    }
}
