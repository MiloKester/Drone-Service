using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

// Milo Kester 30063179
// 2024-05-XX
// Drone Service
// AT3 - Application to log drones for service and repair

namespace Drone_Service
{   
    public partial class MainWindow : Window
    {
        #region SetUp

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

        #endregion

        #region Buttons
        
        // 6.5	Create a button method called “AddNewItem” that will add a new service item to a Queue<> based on the priority.Use TextBoxes for the Client Name, Drone Model, Service Problem and Service Cost.Use a numeric control for the Service Tag.The new service item will be added to the appropriate Queue based on the Priority radio button.
        private void AddNewItem(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextClient.Text) || string.IsNullOrEmpty(TextModel.Text) || string.IsNullOrEmpty(TextProblem.Text) || string.IsNullOrEmpty(TextCost.Text) || (RadioRegular.IsChecked == false && RadioExpress.IsChecked == false) || string.IsNullOrEmpty(UpDownTag.Text))
            {
                StatusOutput.Content = "Please Fill In All Fields."; 
                return;
            }
            
            if (!CheckCostValidity(TextCost.Text))
            {
                StatusOutput.Content = "Invalid Cost. Must be in the format of X.XX";
                return;
            }

            Drone newDrone = new Drone();

            try
            {
                string priorityValue = GetServicePriority();
                newDrone.SetClient(TextClient.Text);
                newDrone.SetModel(TextModel.Text);
                newDrone.SetProblem(TextProblem.Text);
                newDrone.SetCost(double.Parse(TextCost.Text));
                newDrone.SetTag(int.Parse(UpDownTag.Text));

                IncreaseTag();

                switch (priorityValue)
                {
                    case "Regular":
                        RegularService.Enqueue(newDrone);
                        DisplayRegular();
                        break;
                    case "Express":
                        // 6.6	Before a new service item is added to the Express Queue the service cost must be increased by 15%.
                        newDrone.SetCost(Math.Round(double.Parse(TextCost.Text) * 1.15, 2)); // gets value as a string, parses to double, multiples by 1.15, rounds to 2dp
                        ExpressService.Enqueue(newDrone);
                        DisplayExpress();
                        break;
                    default:
                        StatusOutput.Content = "Invalid Priority. Please Try Again.";
                        break;
                }

                ClearInputs();
                TextClient.Focus();
            }
            catch
            {
                StatusOutput.Content = "broken";
            }
        }

        // 6.14	Create a button click method that will remove a service item from the regular ListView and dequeue the regular service Queue<T> data structure. The dequeued item must be added to the List<T> and displayed in the ListBox for finished service items.
        private void CompleteRegular_Click(object sender, RoutedEventArgs e)
        {
            FinishedList.Add(RegularService.Peek()); // add first in queue to list
            RegularService.Dequeue(); // remove first from queue
            DisplayRegular(); // redisplay
            DisplayFinished();
        }

        // 6.15	Create a button click method that will remove a service item from the express ListView and dequeue the express service Queue<T> data structure. The dequeued item must be added to the List<T> and displayed in the ListBox for finished service items.
        private void CompleteExpress_Click(object sender, RoutedEventArgs e)
        {
            FinishedList.Add(ExpressService.Peek());
            ExpressService.Dequeue();
            DisplayExpress();
            DisplayFinished();
        }

        #endregion

        #region Displaying

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

        private void DisplayFinished()
        {
            ListBoxFinished.Items.Clear();

            foreach (Drone item in FinishedList)
            {
                ListBoxFinished.Items.Add(item.GetClient() + " - " + item.GetCost().ToString());
            }
        }

        #endregion

        #region Input Boxes

        // 6.17	Create a custom method that will clear all the textboxes after each service item has been added.
        private void ClearInputs()
        {
            TextClient.Foreground = Brushes.Gray; TextClient.Text = "Client Name";
            TextModel.Foreground = Brushes.Gray; TextModel.Text = "Drone Model";
            TextProblem.Foreground = Brushes.Gray; TextProblem.Text = "Service Problem";
            TextCost.Foreground = Brushes.Gray; TextCost.Text = "Service Cost";
            RadioRegular.IsChecked = false;
            RadioExpress.IsChecked = false;
        }

        // Textbox Formatting
        private void TextClient_GotFocus(object sender, RoutedEventArgs e)
        {
            TextClient.Clear(); TextClient.Foreground = Brushes.Black;
        }

        private void TextModel_GotFocus(object sender, RoutedEventArgs e)
        {
            TextModel.Clear(); TextModel.Foreground = Brushes.Black;
        }

        private void TextProblem_GotFocus(object sender, RoutedEventArgs e)
        {
            TextProblem.Clear(); TextProblem.Foreground = Brushes.Black;
        }

        private void TextCost_GotFocus(object sender, RoutedEventArgs e)
        {
            TextCost.Clear(); TextCost.Foreground = Brushes.Black;
        }

        #endregion

        #region Selection Events

        // 6.12	Create a mouse click method for the regular service ListView that will display the Client Name and Service Problem in the related textboxes.
        private void ListViewRegular_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int indx = ListViewRegular.SelectedIndex;
                if (indx > -1)
                {
                    var item = (dynamic)ListViewRegular.SelectedItems[indx];

                    TextClient.Text = item.Client;
                    TextProblem.Text = item.Problem;
                }
            }
            catch
            {
                StatusOutput.Content = "invalid selection";
            }
        }

        // 6.13	Create a mouse click method for the express service ListView that will display the Client Name and Service Problem in the related textboxes.
        private void ListViewExpress_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int indx = ListViewExpress.SelectedIndex;
                if (indx > -1)
                {
                    var item = (dynamic)ListViewExpress.SelectedItems[indx];

                    TextClient.Text = item.Client;
                    TextProblem.Text = item.Problem;
                }
            }
            catch
            {
                StatusOutput.Content = "invalid selection";
            }
        }

        // 6.16	Create a double mouse click method that will delete a service item from the finished listbox and remove the same item from the List<T>.
        private void ListBoxFinished_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FinishedList.RemoveAt(ListBoxFinished.SelectedIndex);
            DisplayFinished();
        }

        #endregion

        #region Checks & Misc

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
            UpDownTag.Value += 10;
        }

        #endregion
    }
}

// might just end up replacing temp text with captions cause its easier to understand for everyone involved
// also check for way to set column widths since it just autos to the width of the very first entry put in 