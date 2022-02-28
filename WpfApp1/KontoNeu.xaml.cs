using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.IO;

namespace WpfApp1
{
    /// <summary>
    /// Interaktionslogik für KontoNeu.xaml
    /// </summary>
    public partial class KontoNeu : Window
    {
        public KontoNeu()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        { 
            string path = @"konten.csv";
            string name = NameKonto.Text;
            string katigorie = KatiKonto.Text;
            bool enthalten = false;
            if (!File.Exists(path))
            {
                FileInfo FI = new FileInfo(path);
                FileStream FS = FI.Create();
                FS.Close();
            }

            if (name == "" || katigorie == "")
            {
                MessageBox.Show("Bitte füllen Sie alle Felder aus!!");
            }
            else
            {
                List<String> lines = new List<String>();
                if (File.Exists(path))
                {
                    using (StreamReader sr = new StreamReader(path))
                    {
                        string linecheck1 = sr.ReadLine();
                        if (linecheck1 != null)
                        {
                            string[] linecheck2 = linecheck1.Split("; ");
                            string compare;
                            for (int i = 0; i < linecheck2.Length; i++)
                            {
                                compare = linecheck2[i];
                                if (compare == name)
                                {
                                    enthalten = true;
                                    MessageBox.Show(("Das Konto \"" + name + "\" existiert bereits. Bitte geben Sie ihrem Konto einen anderen Namen."), "Kontename bereits vergeben!");
                                    break;
                                }
                                else
                                {
                                    enthalten = false;
                                }
                            }
                        }
                    }

                    if (enthalten == false)
                    {
                        using (StreamReader reader = new StreamReader(path))
                        {
                            string line1 = reader.ReadLine() +"; "+name;
                            string line2 = reader.ReadLine()+"; "+katigorie;
                            lines.Add(line1);
                            lines.Add(line2);
                        }

                        FileInfo Konto = new FileInfo(name +".csv");
                        FileStream Konto_FS = Konto.Create();
                        Konto_FS.Close();

                        using (StreamWriter writer = new StreamWriter(path, false))
                        {
                            foreach (String line in lines)
                            writer.WriteLine(line);
                        }

                        Close();
                    }
                }
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
