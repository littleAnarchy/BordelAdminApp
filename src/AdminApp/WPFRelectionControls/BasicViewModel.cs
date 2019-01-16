using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPFRelectionControls
{
    public abstract class BasicViewModel : INotifyPropertyChanged
    {
        private bool _isBusy;

        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Маркер занятости view
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
            protected set
            {
                _isBusy = value;
                DoPropertyChanged(nameof(IsBusy));
            }
        }

        protected void DoAllPropertiesChanged()
        {
            DoPropertyChanged(string.Empty);
        }


        protected virtual void DoPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
