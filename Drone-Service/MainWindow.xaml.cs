using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Drone_Service
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // 6.2	Create a global List<T> of type Drone called “FinishedList”.
        List<Drone> FinishedList = new List<Drone>();
        // 6.3	Create a global Queue<T> of type Drone called “RegularService”.
        Queue<Drone> RegularService = new Queue<Drone>();
        // 6.4	Create a global Queue<T> of type Drone called “ExpressService”.
        Queue<Drone> ExpressService = new Queue<Drone>();
        
    }
}