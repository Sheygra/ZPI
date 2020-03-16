using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZPI_Projekt_Anonimizator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            mojaTestowaFunkcja();


        }

        public void mojaTestowaFunkcja()
        {
            var jpg_gen = new ZPI_Projekt_Anonimizator.Generators.JPGGenerator();

            testTextBox.Text = jpg_gen.generateDocument("data");

            var jpg_parser = new ZPI_Projekt_Anonimizator.Parsers.JPGParser();

            var table = jpg_parser.parseDocument("path");

            testTextBox.Text += "\n ";
            foreach (DataRow r in table.Rows)
            {
                foreach (DataColumn c in table.Columns)
                {
                    testTextBox.Text += r[c];
                    testTextBox.Text += " ";
                }
            }

            var xml_gen = new ZPI_Projekt_Anonimizator.Generators.XMLGenerator(@"C:/Users/Artiom/Desktop/aninimizator/ZPI/ZPI Projekt Anonimizator/Generators/Files/XMLtest.txt");
            xml_gen.generateDocument("data");
        }
    }
}
