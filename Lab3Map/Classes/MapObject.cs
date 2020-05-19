using GMap.NET;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3Map.Classes
{
    abstract class MapObject
    {
        private string title;

        private DateTime creationDate;

        protected MapObject(string title)
        {
            this.title = title;
            this.creationDate = new DateTime();
        }

        public string GetTitle()
        {
            return title;
        }

        public DateTime GetCreationDate()
        {
            return creationDate;
        }

        public abstract double GetDistance(PointLatLng point);
        public abstract PointLatLng GetFocus();
        public abstract GMapMarker GetMarker();
    }
}