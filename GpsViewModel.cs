#nullable enable
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
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

        private int _selectedIndex;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (value == _selectedIndex) return;

                _selectedIndex = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> PortsOnComputer { get; }

        public GpsViewModel()
        {
            PortsOnComputer = new ObservableCollection<string>(SerialPort.GetPortNames());
        }

        public ICommand ToggleConnection => new RelayCommand(p =>
        {
            if (!_gpsModule.IsConnected) ConnectToDevice(PortsOnComputer[SelectedIndex], BaudRate);
            else DisconnectFromDevice();
        });

        private void OnNewSerialData(object s, SerialDataReceivedEventArgs e)
        {
            if (!(s is SerialPort port)) return;

            string line = port.ReadLine();
            Debug.WriteLine(line);
        }

        private void ConnectToDevice(string id, int baudRate)
        {
            _gpsModule.Port = new SerialPort(PortsOnComputer[SelectedIndex], BaudRate);
            _gpsModule.Port.Open();
            _gpsModule.IsConnected = _gpsModule.Port.IsOpen;

            _gpsModule.Port.DataReceived += OnNewSerialData;
        }

        private void DisconnectFromDevice()
        {
            Debug.Assert(_gpsModule.Port != null, "_gpsModule.Port != null");
            _gpsModule.Port.Close();
            _gpsModule.IsConnected = _gpsModule.Port.IsOpen;
            _gpsModule.Port.DataReceived -= OnNewSerialData;
            _gpsModule.Port = null;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}