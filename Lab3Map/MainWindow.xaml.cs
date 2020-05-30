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
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System.Device.Location;
using Lab3Map.Classes;

namespace Lab3Map
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            comboBox.SelectedIndex = 0;
        }

        List<MapObject> mapObjects = new List<MapObject>();
        List<PointLatLng> activePoints = new List<PointLatLng>();

        private void Map_Loaded(object sender, RoutedEventArgs e)
        {
            // настройка доступа к данным
            GMaps.Instance.Mode = AccessMode.ServerAndCache;

            // установка провайдера карт
            Map.MapProvider = OpenStreetMapProvider.Instance;

            // установка зума карты
            Map.MinZoom = 2;
            Map.MaxZoom = 17;
            Map.Zoom = 15;

            // установка фокуса карты
            Map.Position = new PointLatLng(55.012823, 82.950359);

            // настройка взаимодействия с картой
            Map.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            Map.CanDragMap = true;
            Map.DragButton = MouseButton.Left;
        }

        private void Map_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PointLatLng point = Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y);
            activePoints.Add(point);

            listResults.Items.Clear();

            if (buttonSearchMode.IsChecked == true && mapObjects.Count != 0)
            {
                mapObjects.Sort((obj1, obj2) => obj1.GetDistance(point).CompareTo(obj2.GetDistance(point)));

                foreach (MapObject obj in mapObjects)
                {
                    listResults.Items.Add(obj.GetTitle() + " " + Math.Round(obj.GetDistance(point), 2));
                }
            }
        }

        private void butOKAdd_Click(object sender, RoutedEventArgs e)
        {
            if (buttonCreationMode.IsChecked == true)
            {
                if (textNameAdd.Text.ToString() != "Enter name..." || textNameAdd.Text.ToString() != "")
                    CreateMapObject();
                else
                    MessageBox.Show("Enter name of object");
            }
            else
                MessageBox.Show("Pick 'Creation of objects' Cursor Mode");
        }

        private void CreateMapObject()
        {
            switch (comboBox.SelectedIndex)
            {
                case 0:
                    if (activePoints.Count >= 3)
                    {
                        Area area = new Area(textNameAdd.Text.ToString(), activePoints);
                        mapObjects.Add(area);
                        Map.Markers.Add(area.GetMarker());
                        activePoints.Clear();
                    }
                    else if (activePoints.Count < 3)
                        MessageBox.Show("Count of active points must be bigger two");
                        activePoints.Clear();
                    break;

                case 1:
                    if (activePoints.Count > 0)
                    {
                        Car car = new Car(textNameAdd.Text.ToString(), activePoints.Last());
                        mapObjects.Add(car);
                        Map.Markers.Add(car.GetMarker());
                        activePoints.Clear();
                    }
                    else if (activePoints.Count < 1)
                    {
                        MessageBox.Show("Count of active points must be bigger zero");
                        activePoints.Clear();
                    }
                    break;

                case 2:
                    if (activePoints.Count >= 2)
                    {
                        Classes.Route route = new Classes.Route(textNameAdd.Text.ToString(), activePoints);
                        mapObjects.Add(route);
                        Map.Markers.Add(route.GetMarker());
                        activePoints.Clear();
                    }
                    else if (activePoints.Count < 2 )
                        MessageBox.Show("Count of active points must be bigger one");
                        activePoints.Clear();
                    break;

                case 3:
                    if (activePoints.Count > 0)
                    {
                        Human human = new Human(textNameAdd.Text.ToString(), activePoints.Last());
                        mapObjects.Add(human);
                        Map.Markers.Add(human.GetMarker());
                        activePoints.Clear();
                    }
                    else if (activePoints.Count != 1)
                    {
                        MessageBox.Show("Count of active points must be bigger zero");
                        activePoints.Clear();
                    }
                    break;

                case 4:
                    if (activePoints.Count > 0)
                    {
                        Classes.Location location = new Classes.Location(textNameAdd.Text.ToString(), activePoints.Last());
                        mapObjects.Add(location);
                        Map.Markers.Add(location.GetMarker());
                        activePoints.Clear();
                    }
                    else if (activePoints.Count != 1)
                    {
                        MessageBox.Show("Count of active points must be bigger zero");
                        activePoints.Clear();
                    }
                    break;
            }
        }

        private void butReset_Click(object sender, RoutedEventArgs e) => activePoints.Clear();

        private void butOKSearch_Click(object sender, RoutedEventArgs e)
        {
            listResults.Items.Clear();

            if (textNameSearch.Text.ToString() == "..." || textNameSearch.Text.ToString() == "")
                MessageBox.Show("Введите имя искомого объекта");
            else
            {
                foreach (MapObject obj in mapObjects)
                {
                    if (obj.GetTitle().Contains(textNameSearch.Text.ToString()))
                    {
                        listResults.Items.Add(obj.GetTitle().ToString());
                    }
                }
            }
        }

        private void listResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listResults.SelectedItem.ToString().Contains(" "))
            {
                char srch = ' ';
                string str = listResults.SelectedItem.ToString();
                int ind = str.IndexOf(srch);
                str.Remove(ind);
                
                GetFocus(str);
            }
            else
                GetFocus(listResults.SelectedItem);
        }

        private void GetFocus(object selectedItem)
        {
            foreach (MapObject obj in mapObjects)
            {
                try
                {
                    if (obj.GetTitle() == selectedItem.ToString())
                        Map.Position = obj.GetFocus();
                }
                catch(NullReferenceException)
                {

                }
            }
        }

        private void butClear_Click(object sender, RoutedEventArgs e) => Map.Markers.Clear();
    }
}