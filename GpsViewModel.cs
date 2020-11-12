#nullable enable
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
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

        // default baud rate in 9600 bps
        private int _serialBaudRate = 9600;

        public int BaudRate
        {
            get => _serialBaudRate;
            set
            {
                if (value == _serialBaudRate) return;

                _serialBaudRate = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> PortsOnComputer => new ObservableCollection<string>()
        {
            "COM1",
            "COM2"
        };

        public ICommand UpdateTime => new RelayCommand(p => { _gpsModule.UtcTime = DateTime.Now; });

        public ICommand ConnectToDevice => new RelayCommand(p =>
        {
            _gpsModule.IsConnected = !_gpsModule.IsConnected;
            MessageBox.Show(BaudRate.ToString(), "BaudRate");
        });

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}