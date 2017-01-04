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

namespace MachPlotNamespace
{
    /// <summary>
    /// Interaction logic for ModuleChooseWindow.xaml
    /// </summary>
    public partial class ModuleChooseWindow : Window
    {
        public ModuleChooseWindow()
        {
            InitializeComponent();
            moduleChangeListbox.Items.Add("Maszyna asynchroniczna");
        }

        private void chooseButton_Click(object sender, RoutedEventArgs e)
        {
            if(moduleChangeListbox.SelectedIndex != -1)
            {
                switch (moduleChangeListbox.SelectedItem as string)
                {
                    case "Maszyna asynchroniczna":

                        break;
                }
            }
        }
    }
}
