#nullable enable
using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace gps_time_viewer
{
    /// <summary>
    /// GPS data model for storing data received from the GPS serial device. This class also stores the serial
    /// connection object for our hardware interface.
    /// </summary>
    internal class GpsModule : INotifyPropertyChanged
    {
        public SerialPort? Port;

        private DateTime _utcTime = DateTime.Now;

        public DateTime UtcTime
        {
            get => _utcTime;
            set
            {
                if (value == _utcTime) return;
                _utcTime = value;
                OnPropertyChanged();
            }
        }

        private bool _isConnected = false;

        public bool IsConnected
        {
            get => _isConnected;
            set
            {
                if (value == _isConnected) return;

                _isConnected = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}