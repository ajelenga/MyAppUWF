using MyApp.Service;
using MyApp.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyApp.PageBtn
{
    /// <summary>
    /// Logique d'interaction pour PageBtn.xaml
    /// </summary>
    public partial class PageBtn : Page
    {
        public PageBtn()
        {
            InitializeComponent();
            DataContext = new ChiffreViewModel();
        }
    }
}
