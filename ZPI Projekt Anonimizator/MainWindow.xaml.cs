using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Microsoft.Win32;
using ZPI_Projekt_Anonimizator.Algorithm;
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
                        xml_gen.generateDocument(filename, patientNumber);

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

        public void showJPGMetadata(String path)
         {
            try
            {
                var jpg_parser = new ZPI_Projekt_Anonimizator.Parsers.JPGParser();
                var table = jpg_parser.parseDocument(path);
                if (table == null) promptUser("An error ocurred, unable to open the jpg file." + path);
                else
                {
                    var values = table.Rows[0].ItemArray;
                    TextLine0.Text = table.Columns[0].ColumnName + ": " + values[0];
                    TextLine1.Text = table.Columns[1].ColumnName + ": " + values[1];
                    TextLine2.Text = table.Columns[2].ColumnName + ": " + values[2];
                    TextLine3.Text = table.Columns[3].ColumnName + ": " + values[3];
                    TextLine4.Text = table.Columns[4].ColumnName + ": " + values[4];
                    TextLine5.Text = "PATH: " + path;
                    MetadataDocumentView.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                promptUser("An error ocurred, unable to open the jpg file." + path);
            }
        }
        public void showDICOMMetadata(String path)
        {
            try
            {
                var dicomGenerator = new DICOMGenerator();
                path = dicomGenerator.generateDocument(new Patient("10", "ANNA", "KOWALSKA", "", "", "K", "prof", "krakow", "10/10/2010"));
                var dicom_parser = new ZPI_Projekt_Anonimizator.Parsers.DICOMParser();
                var table = dicom_parser.parseDocument(path);
                if (table == null) promptUser("An error ocurred, unable to open the jpg file." + path);
                else
                {
                    var values = table.Rows[0].ItemArray;
                    TextLine0.Text = table.Columns[0].ColumnName + ": " + values[0];
                    TextLine1.Text = table.Columns[1].ColumnName + ": " + values[1];
                    TextLine2.Text = table.Columns[2].ColumnName + ": " + values[2];
                    TextLine3.Text = table.Columns[3].ColumnName + ": " + values[3];
                    TextLine5.Text = "PATH: " + path;
                    MetadataDocumentView.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                promptUser("An error ocurred, unable to open the jpg file." + path);
            }
        }

        public void showDOCX(String path)
        {
            try
            {
                var docxGenerator = new DOCXGenerator();
                path = docxGenerator.generateDocument(new Patient("10", "ANNA", "KOWALSKA", "", "", "K", "prof", "krakow", "10/10/2010"));
                
                Process wordProcess = new Process();
                wordProcess.StartInfo.FileName = path;
                wordProcess.StartInfo.UseShellExecute = true;
                wordProcess.Start();
            }
            catch (Exception ex)
            {
                promptUser("An error ocurred, unable to open the docx file.");
            }
        }

        private void MetadataOpen_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                DataRowView context = (DataRowView)button.DataContext;
                var row = context.Row;
                var links = row["PathForFiles"].ToString().Split(";");
                //promptUser(links[0]);
                switch (button.Name)
                {
                    case "BtnJPG": showJPGMetadata(links[0]); break;
                    case "BtnDICOM": showDICOMMetadata(links[1]); break;
                    case "BtnDOCX": break;
                    default: promptUser("An error ocurred, no file can be opened."); break;
                }
            }
            catch (Exception ex)
            {
                promptUser("An error ocured while trying to read the file.");
            }
        }
        private void MetadataClose_Button_Click(object sender, RoutedEventArgs e)
        {
            TextLine0.Text = "";
            TextLine1.Text = "";
            TextLine2.Text = "";
            TextLine3.Text = "";
            TextLine4.Text = "";
            TextLine5.Text = "";
            MetadataDocumentView.Visibility = Visibility.Hidden;
        }
    }
}
