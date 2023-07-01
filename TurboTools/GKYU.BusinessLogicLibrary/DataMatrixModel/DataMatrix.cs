using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.BusinessLogicLibrary.DataMatrixModel
{
    public class DataMatrix : IEnumerable
    {
        public List<MatrixColumn> Columns { get; set; }
        public List<object[]> Rows { get; set; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new GenericEnumerator(Rows.ToArray());
        }
    }

    public class MatrixColumn
    {
        public string Name { get; set; }
        public string StringFormat { get; set; }
    }
}
