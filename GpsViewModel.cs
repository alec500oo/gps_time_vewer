#nullable enable
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using JetBrains.Annotations;

namespace gps_time_viewer
{
    internal class GpsViewModel : INotifyPropertyChanged
    {
        private GpsModule _gpsModule = new GpsModule();

        public GpsModule GpsModule
        {
            get => _gpsModule;
            set
            {
                if (value == _gpsModule) return;

                _gpsModule = value;
                OnPropertyChanged();
            }
        }

        public ICommand UpdateTime => new RelayCommand(p => { _gpsModule.UtcTime = DateTime.Now; });

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
