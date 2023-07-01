using GKYU.BusinessLogicLibrary.Bitmaps;
using GKYU.BusinessLogicLibrary.DataMatrixModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GKYU.PresentationLogicLibrary.ViewModels
{
    public class MonthlyOrderCount
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int OrderCount { get; set; }
    }
    public class DataMatrixViewModel
        : FileViewModel
    {
        protected DataMatrix _dataMatrix;
        public DataMatrix DataMatrix
        {
            get { return _dataMatrix; }
            set { _dataMatrix = value; }
        }
        public DataMatrixViewModel(string name, FileModel fileModel) : base(name, fileModel) { }
        private static DataMatrix SumByMonth(IEnumerable<MonthlyOrderCount> orders)
        {
            DataMatrix result = new DataMatrix
            {
                Columns = new List<MatrixColumn>(),
                Rows = new List<object[]>()
            };
            result.Columns.Add(new MatrixColumn() { Name = "Year" });
            for (int i = 0; i < 12; i++)
                result.Columns.Add(new MatrixColumn()
                {
                    Name = string.Format("{0:MMM}", new DateTime(2009, i + 1, 1))
                });
            for (int i = 1996; i < 1999; i++)
            {
                object[] row = new object[13];
                row[0] = i;
                for (int j = 1; j <= 12; j++)
                {
                    int count = (from o in orders
                                 where o.Year == i && o.Month == j
                                 select o.OrderCount).Sum();
                    row[j] = count;
                }
                result.Rows.Add(row);
            }
            return result;
        }
        public static DataMatrix CreateDateRange(DateTime startDate, DateTime endDate)
        {
            //var orders = from o in db.Orders
            //             group o by new { o.OrderDate.Value.Year, o.OrderDate.Value.Month }
            //            into g
            //             select new MonthlyOrderCount()
            //             {
            //                 Year = g.Key.Year,
            //                 Month = g.Key.Month,
            //                 NumberOfOrders = g.Count()
            //             };
            List<MonthlyOrderCount> monthlyOrders = new List<MonthlyOrderCount>();
            Random rand = new Random();
            foreach (int year in Enumerable.Range(startDate.Year, endDate.Year))
            {
                foreach(int month in Enumerable.Range(startDate.Month, endDate.Month))
                {
                    foreach(int day in Enumerable.Range(startDate.Day, endDate.Day))
                    {
                        monthlyOrders.Add(new MonthlyOrderCount() { Year=year, Month = month, OrderCount = rand.Next(1000) });
                    }
                }
            }
            DataMatrix result = SumByMonth(monthlyOrders);
            return result;
        }
    }
}
