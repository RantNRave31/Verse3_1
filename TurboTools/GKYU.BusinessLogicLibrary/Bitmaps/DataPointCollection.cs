using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.PresentationLogicLibrary.Models
{
    public class DataCollection
    {
        public ObservableCollection<DataPoint> DataPoints { set; get; }
        public DataCollection(int numPoints)
        {
            DataPoints = new ObservableCollection<DataPoint>(); //new DataRandomizer<DataPoint>(DataPoints, numPoints, Math.Min(1, numPoints / 100)) };
        }
    }
}
