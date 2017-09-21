using System;
using CoreLocation;
using MapKit;

namespace MagpieIOS
{
	public class AnnotationModel : MKAnnotation
	{

		private string _title;
		private string _subtitle;
		private int id;
		private bool claimed;
        public CLLocationCoordinate2D Coords;

		public AnnotationModel(CLLocationCoordinate2D coordinate, string title, int id, bool claimed, string sub)
		{
			this.Coords = coordinate;
			_title = title;
			this.id = id;
			this.claimed = claimed;
            this._subtitle = sub;
		}


		public override string Title { get { return _title; } }
        public override string Subtitle { get { return _subtitle;  } }

		public override CLLocationCoordinate2D Coordinate { get { return this.Coords; } }

        public int GetID() { return this.id; }

		public bool isClaimed()
		{
			return claimed;
		}
	}
}
