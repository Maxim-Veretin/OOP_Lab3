using GMap.NET;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Lab3Map.Classes
{
    class Car : MapObject
    {
        private PointLatLng point;

        public Car(string title, PointLatLng point) : base(title)
        {
            this.point = point;
        }

        public override double GetDistance(PointLatLng point2)
        {
            GeoCoordinate c1 = new GeoCoordinate(point.Lat, point.Lng);
            GeoCoordinate c2 = new GeoCoordinate(point2.Lat, point2.Lng);

            double distance = c1.GetDistanceTo(c2);

            return distance;
        }

        public override PointLatLng GetFocus()
        {
            return point;
        }

        public override GMapMarker GetMarker()
        {
            GMapMarker markerCar = new GMapMarker(point)
            {
                Shape = new Image
                {
                    Width = 30, // ширина маркера
                    Height = 30, // высота маркера
                    ToolTip = this.GetTitle(), // всплывающая подсказка
                    Source = new BitmapImage(new Uri("pack://application:,,,/Resources/car_icon.png")) // картинка
                },
                Position = point
            };

            return markerCar;
        }
    }
}