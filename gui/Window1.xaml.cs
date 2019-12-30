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

namespace gui
{
    /// <summary>
    /// Interaktionslogik für Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        String DataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
               "\\sb0t\\" + AppDomain.CurrentDomain.FriendlyName + "\\Scripting\\";

        public Window1()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                List<String> list = new List<String>();

                for (int i = 0; i < tb.LineCount; i++)//tb=textbox
                {
                    string text = tb.GetLineText(i).Replace("\r\n","") + ".js";
                    list.Add(text);
                }
                String path = System.IO.Path.Combine(DataPath, "autorun.dat");
                File.WriteAllLines(path, list.ToArray());
            }
            catch { }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.tb.AcceptsReturn = true;
            String path = System.IO.Path.Combine(DataPath, "autorun.dat");
            if (File.Exists(path))
            {
                String[] lines = File.ReadAllLines(path);
                foreach (String str in lines)
                {
                    if (lines.Last() == str)
                    {
                        this.tb.AppendText(str.Replace(".js", ""));
                    }
                    else
                    {
                        this.tb.AppendText(str.Replace(".js", "") + Environment.NewLine);
                    }
                }
            }
        }
    }
}
