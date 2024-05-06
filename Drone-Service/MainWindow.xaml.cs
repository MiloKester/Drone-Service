using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
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

        // 6.5 6.5	Create a button method called “AddNewItem” that will add a new service item to a Queue<> based on the priority.
        // Use TextBoxes for the Client Name, Drone Model, Service Problem and Service Cost.
        // Use a numeric control for the Service Tag.
        // The new service item will be added to the appropriate Queue based on the Priority radio button.

        // try and code around needing this much nesting
        private void AddNewItem(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TextClient.Text) && !string.IsNullOrEmpty(TextModel.Text) && !string.IsNullOrEmpty(TextProblem.Text) && !string.IsNullOrEmpty(TextCost.Text) && (RadioRegular.IsChecked == true || RadioExpress.IsChecked == true) && !string.IsNullOrEmpty(UpDownTag.Text))
            {
                if (CheckCostValidity(TextCost.Text)) // if true (Valid)
                {
                    Drone newDrone = new Drone();

                    try
                    {
                        string priorityValue = GetServicePriority();
                        newDrone.SetClient(TextClient.Text);
                        newDrone.SetModel(TextModel.Text);
                        newDrone.SetProblem(TextProblem.Text);
                        newDrone.SetCost(double.Parse(TextCost.Text));
                        newDrone.SetTag(int.Parse(UpDownTag.Text)); // could use UpDownTag.Value

                        IncreaseTag(); // broken

                        if (priorityValue == "Regular")
                        {
                            RegularService.Enqueue(newDrone);
                            DisplayRegular();
                        }
                        else if (priorityValue == "Express")
                        {
                            // 6.6	Before a new service item is added to the Express Queue the service cost must be increased by 15%.
                            newDrone.SetCost(double.Parse(TextCost.Text) * 1.15); // spits out e.g. 114.9999999999999 instead of just 115.00 // reasonable outputs from the following: 1, 2, 4, 5, 8, 9, 10, 13, 15, 16, 18, 20
                            ExpressService.Enqueue(newDrone);
                            DisplayExpress();
                        }
                        else
                        {
                            StatusOutput.Content = "invalid priority. please try again";
                        }

                        ClearInputs();
                    }
                    catch
                    {
                        StatusOutput.Content = "shits fucked somewhere with data entry";
                    }
                }
                else
                {
                    StatusOutput.Content = "invalid cost. must be in the format of XX.XX / X.XX / XXX.XX";
                }
            }
            else
            {
                StatusOutput.Content = "Please Fill In All Fields";
            }  
        }

        // 6.7 Create a custom method called “GetServicePriority” which returns the value of the priority radio group. This method must be called inside the “AddNewItem” method before the new service item is added to a queue.
        private string GetServicePriority()
        {
            string radioText = "";
            if (RadioRegular.IsChecked == true)
            {
                radioText = (string)RadioRegular.Content;
            }
            else if (RadioExpress.IsChecked == true)
            {
                radioText = (string)RadioExpress.Content;
            }
            return radioText;
        }

        // 6.8	Create a custom method that will display all the elements in the RegularService queue. The display must use a List View and with appropriate column headers.
        private void DisplayRegular()
        {
            ListViewRegular.Items.Clear();

            foreach (Drone item in RegularService)
            {
                ListViewRegular.Items.Add(new
                {
                    Client = item.GetClient(),
                    Model = item.GetModel(),
                    Problem = item.GetProblem(),
                    Cost = item.GetCost(),
                    Tag = item.GetTag()
                });

            }
        }

        // 6.9	Create a custom method that will display all the elements in the ExpressService queue. The display must use a List View and with appropriate column headers.
        private void DisplayExpress()
        {
            ListViewExpress.Items.Clear();

            foreach (Drone item in ExpressService)
            {
                ListViewExpress.Items.Add(new
                {
                    Client = item.GetClient(),
                    Model = item.GetModel(),
                    Problem = item.GetProblem(),
                    Cost = item.GetCost(),
                    Tag = item.GetTag()
                });
            }
        }

        // 6.10	Create a custom method to ensure the Service Cost textbox can only accept a double value with two decimal point.
        private bool CheckCostValidity(string cost)
        {
            var pattern = @"[0-9]+\.[0-9][0-9]";

            if (Regex.IsMatch(cost, pattern))
            {
                return true; // is valid
            }
            else
            {
                return false;
            }
        }

        // 6.11	Create a custom method to increment the service tag control, this method must be called inside the “AddNewItem” method before the new service item is added to a queue.
        private void IncreaseTag()
        {
            
        }

        // 6.17	Create a custom method that will clear all the textboxes after each service item has been added.
        private void ClearInputs()
        {
            TextClient.Clear();
            TextModel.Clear();
            TextProblem.Clear();
            TextCost.Clear();
            RadioRegular.IsChecked = false;
            RadioExpress.IsChecked = false;
            // UpDownTag.Text = "";
        }

        private void ListViewRegular_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int currentIndex = ListViewRegular.SelectedIndex;
                if (currentIndex > -1)
                {
                    // have to get from listview directly

                }
            }
            catch
            {
                StatusOutput.Content = "invalid selection";
            }
        }
    }
}