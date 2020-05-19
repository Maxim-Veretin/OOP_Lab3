using GMap.NET;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Lab3Map.Classes
{
    class Area : MapObject
    {
        private List<PointLatLng> points;

        public Area(string title, List<PointLatLng> points) : base(title)
        {
            this.points = new List<PointLatLng>();

            foreach (PointLatLng p in points)
            {
                this.points.Add(p);
            }
        }

        public override double GetDistance(PointLatLng p2)
        {
            double distance = double.MaxValue;

            foreach (PointLatLng p in points)
            {
                GeoCoordinate c1 = new GeoCoordinate(p.Lat, p.Lng);
                GeoCoordinate c2 = new GeoCoordinate(p2.Lat, p2.Lng);
                
                if (distance > c1.GetDistanceTo(c2))
                {
                    distance = c1.GetDistanceTo(c2);
                }
            }

            return distance;
        }

        public override PointLatLng GetFocus()
        {
            PointLatLng point = new PointLatLng();

            point.Lat = points.Sum(pts => pts.Lat)/points.Count;
            point.Lng = points.Sum(pts => pts.Lng)/points.Count;

            return point;
        }

        public override GMapMarker GetMarker()
        {
            GMapMarker markerArea = new GMapPolygon(points)
            {
                Shape = new Path
                {
                    Stroke = Brushes.Black,
                    Fill = Brushes.Violet,
                    Opacity = 0.7
                },
                Position = this.GetFocus()
            };

            return markerArea;
        }
    }
}