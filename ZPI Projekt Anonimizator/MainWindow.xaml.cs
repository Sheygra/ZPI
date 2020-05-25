using System;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Microsoft.Win32;
using ZPI_Projekt_Anonimizator.entity;
using ZPI_Projekt_Anonimizator.Generators;

namespace ZPI_Projekt_Anonimizator
{
    public partial class MainWindow : Window
    {
        DataTable patientDataGenerated = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void bindXMLBeforeGridData()
        {
            string filePath = "";
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Filter = "XML file|*.xml";
            fileDialog.DefaultExt = ".xml";
            Nullable<bool> dialogOK = fileDialog.ShowDialog();
            if (dialogOK == true)
            {
                filePath = fileDialog.FileName;
            }
            try
            {
                BrowseTextBlockPath.Text = filePath.Substring(0,3) + "(...)" + filePath.Substring(filePath.Length-20, 20);
                BrowseTextBlockPath.Visibility = Visibility.Visible;
                var xml_reader = new ZPI_Projekt_Anonimizator.Parsers.XMLParser();

                patientDataGenerated = xml_reader.parseDocument(filePath);

                XMLbeforeDataGrid.DataContext = patientDataGenerated.DefaultView;
                XMLbeforeDataGrid.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                promptUser("Unable to retrieve patient data. Please try again.");
            }
        }

        private void Browse_Button_Click(object sender, RoutedEventArgs e)
        {
            bindXMLBeforeGridData();
        }

        private void Generate_Button_Click(object sender, RoutedEventArgs e)
        {
            var filename = OutputFileNameInput.Text;
            int patientNumber = 0;
            if (filename == null || filename.Length == 0)
            {
                promptUser("File name is incorrect. Name has to be at least one character.");
            }
            else
            {
                Int32.TryParse(PatientNumberInput.Text, out patientNumber);
                if (patientNumber != 0)
                {
                    try
                    {
                        var xml_gen = new ZPI_Projekt_Anonimizator.Generators.XMLGenerator();
                        xml_gen.generateDocument(filename); //generate XML with given parameters

                        OutputFileNameInput.Text = "";
                        PatientNumberInput.Text = "";

                        promptUser("Successfully generated patient data file: " + filename + ".xml");
                    }
                    catch (Exception ex)
                    {
                        promptUser("Unable to generate patient data. An error occured.");
                    }
                }
                else
                {
                    promptUser("Patient number is incorrect. Enter an integer.");
                }
            }
        }

        private void RunAlgorithm_Button_Click(object sender, RoutedEventArgs e)
        {
            bool k_anonimization = RButton1KAnoAlgorithm.IsChecked == null ? false : RButton1KAnoAlgorithm.IsChecked == true ? true : false;
            bool k_alfa_anonimization = RButton1KAlfaAnoAlgorithm.IsChecked == null ? false : RButton1KAlfaAnoAlgorithm.IsChecked == true ? true : false;

            if(patientDataGenerated == null || patientDataGenerated.Rows.Count == 0)
            {
                promptUser("Patient data is empty.");
            }
            else if (k_anonimization)
            {
                promptUser("K-anonimization algoritm.");
                try
                {
                    XMLAfterAnonimizationGrid.DataContext = patientDataGenerated.DefaultView; //tutaj wynik anonimizacji
                    XMLAfterAnonimizationGrid.Visibility = Visibility.Visible;
                }
                catch(Exception ex)
                {
                    promptUser("Algoritm execution resulted in an error.");
                }
            }
            else if(k_alfa_anonimization)
            {
                promptUser("K-alfa-anonimization algoritm.");
                try
                {
                    XMLAfterAnonimizationGrid.DataContext = patientDataGenerated.DefaultView; //tutaj wynik anonimizacji
                    XMLAfterAnonimizationGrid.Visibility = Visibility.Visible;
                }
                catch (Exception ex)
                {
                    promptUser("Algoritm execution resulted in an error.");
                }
            }
            else
            {
                promptUser("One of algorithms must be selected.");
            }
            
        }
        
        private void promptUser(String msg)
        {
            InfoBoxPrompt.Text = msg;
            ((Storyboard)FindResource("animate")).Begin(Prompt);
        }

        public void mojaTestowaFunkcja()
         {
            try
            {
                var jpg_gen = new ZPI_Projekt_Anonimizator.Generators.JPGGenerator();
                Patient p = new Patient("18922", "FFF", "XXX", "654728111", "Kwiatkowa 5", "K", "XD", "Wrocław", "00.00.2002");
                var path = jpg_gen.generateDocument(p);
                

                var jpg_parser = new ZPI_Projekt_Anonimizator.Parsers.JPGParser();
                var table = jpg_parser.parseDocument(path);
                if (table == null) promptUser("NULL");
                else
                {
                    var values = table.Rows[0].ItemArray;
                    TextLine0.Text = table.Columns[0].ColumnName + ": " + values[0];
                    TextLine1.Text = table.Columns[1].ColumnName + ": " + values[1];
                    TextLine2.Text = table.Columns[2].ColumnName + ": " + values[2];
                    TextLine3.Text = table.Columns[3].ColumnName + ": " + values[3];
                    TextLine4.Text = table.Columns[4].ColumnName + ": " + values[4];
                    TextLine5.Text = "PATH: " + path;
                }
            }
            catch (Exception ex)
            {
                promptUser(ex.ToString());
            }

        }
        /*
        public void mojaBardziejTestowaFunkcja()
        {
            Patient p = new Patient("18922", "FFF", "XXX", "654728111", "Kwiatkowa 5", "K", "XD", "Wrocław", "00.00.2002");
            var docx_parser = new ZPI_Projekt_Anonimizator.Parsers.DOCXParser();
            DataTable dt = docx_parser.parseDocument(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\resource\historia_choroby_wzor_1.docx");
            string s = "";

            foreach (DataRow dr in dt.Rows)
            {
                s = s + dr["Id"].ToString() + ".  " + dr["Name"].ToString() + "  " +
                    dr["Surname"].ToString() + " | " + dr["PESEL"].ToString() + "  " + " | " + dr["Address"].ToString() + "  (" + dr["PhoneNumber"].ToString() + ")\n";
            }
            testTextBox2.Text = s;

            var docx_generator = new ZPI_Projekt_Anonimizator.Generators.DOCXGenerator();

            docx_generator.generateDocument(p);
        }
        private void btnOpenClick(Object sender, RoutedEventArgs rea)
        {
            string filePath ="";
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Filter = "XML file|*.xml";
            fileDialog.DefaultExt = ".xml";
            Nullable<bool> dialogOK = fileDialog.ShowDialog();
            if(dialogOK == true)
            {
                filePath = fileDialog.FileName;

            }
            try
            {
                var xml_reader = new ZPI_Projekt_Anonimizator.Parsers.XMLParser();

                DataTable dt = xml_reader.parseDocument(filePath);
                string s = "";
                foreach (DataRow dr in dt.Rows)
                {
                    s = s + dr["id"].ToString() + ".  " + dr["Name"].ToString() + "  " +
                        dr["Surname"].ToString() + dr["Profession"].ToString() + " |  " + dr["City"].ToString() + dr["Address"].ToString() + "  (" + dr["PhoneNumber"].ToString() + ")\n";
                }
                testTextBox1.Text = s;
            }
            catch (Exception ex)
            {
                testTextBox1.Text = "Blędna sciezka!!\n" + ex;
            }

        }

        private void testTextBox3_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void testTextBox4_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void testTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }*/
    }
}
