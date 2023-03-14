using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace WordTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TestContainer test = null;

        public MainWindow()
        {
            InitializeComponent();
            brdVezerlo.IsEnabled = false;
        }

        private void btnLoadTest_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            try
            {
                test = new TestContainer(ofd.FileName);
            }
            catch (Exception hiba)
            {
                MessageBox.Show($"A program nem tudta a tesztet betölteni! {hiba.Message}");
                return;
            }

            //A státusz sort alul töltse fel értékekkel!!!

            lblFileName.Content = ofd.FileName.ToString();
            lblWordsNum.Content = test.WordPairs.Count.ToString();
            lblTestNumUK.Content = test.GetNumOfWordsTestedUK;
            lblTestNumHU.Content = test.GetNumOfWordsTestedHU;
           


            sliTestWordsNum.Minimum = 3;
            //További értékék? Value Maximum ?
            
            //Amíg nincs betöltés, addig legyen csak inaktív
            brdVezerlo.IsEnabled = true;
            btnStartTest.IsEnabled = true;
            btnDEMO.IsEnabled = true;
            btnListOfWords.IsEnabled = true;
        }

        private void sliTestWordsNum_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblChoisedNum.Content = Math.Round(sliTestWordsNum.Value) + " db";
           
        }

        private void btnStartTest_Click(object sender, RoutedEventArgs e)
        {
            //UK->HU vagy HU->UK teszt ?
            //most legyen UK->HU true
            bool UK_HU = rbUKHU.IsChecked.Value;

            //Mi alapján legyen a tesztlista elkészítve?
            //most legyen random 10db
            test.DoRandomListNumber((int)sliTestWordsNum.Value);

             
            TestWindow twin = new TestWindow(test.NarrowedWordPairs, UK_HU, chkIsCheckEveryWord.IsChecked == true);
            twin.ShowDialog();
            test.UpdateResultFromNarrowedList();
            test.WriteContainer();
            lblTestNumUK.Content = test.GetNumOfWordsTestedUK;
            lblTestNumHU.Content = test.GetNumOfWordsTestedHU;
        }


        private void btnListOfWords_Click(object sender, RoutedEventArgs e)
        {
            int num = (int)sliTestWordsNum.Value;
            if (rbRandom.IsChecked.Value)
            {
                test.DoRandomListNumber(num);
            }
            else if (rbLeastTested.IsChecked.Value)
            {
                if (rbHUUK.IsChecked.Value)
                {
                    test.DoLeastTestedHUList(num);
                }
                else
                    test.DoLeastTestedUKList(num);
            }
            else
            {
                if (rbHUUK.IsChecked.Value)
                {
                    test.DoLeastKnownHUList(num);
                }
                else
                    test.DoLeastKnownUKList(num);
            }
            //De mi a szűkített lista most?
            ListOfWords szoLista = new ListOfWords(test.NarrowedWordPairs, "Szavak listája");
            szoLista.ShowDialog();
        }

        private void btnDEMO_Click(object sender, RoutedEventArgs e)
        {
            DEMO listak = new DEMO(test);
            listak.ShowDialog();
        }

        private void rbRandom_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void rbLeastTested_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rbLestKnown_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
