using System.ComponentModel;

namespace Core.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raises the 'PropertyChanged' event when the value of a property of the data model has changed.
        /// </summary>
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// 'PropertyChanged' event that is raised when the value of a property of the data model has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructors


        protected ViewModelBase() : base()
        {
        }

        #endregion

        #region Properties



        #endregion


    }
}
