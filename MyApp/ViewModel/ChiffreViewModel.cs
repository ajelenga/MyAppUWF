using MyApp.Model;
using MyApp.Service;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MyApp.ViewModel
{
    public class ChiffreViewModel : INotifyPropertyChanged
    {
        private ChiffreService _chiffreService;
        private int _entier;

        public ChiffreViewModel()
        {
            _chiffreService = new ChiffreService();
            GenerationCommand = new RelayCommand(Generation);
            IncrementationCommand = new RelayCommand(Incrementation);
        }

        public int Entier
        {
            get { return _entier; }
            set
            {
                if (_entier != value)
                {
                    _entier = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand GenerationCommand { get; }
        public ICommand IncrementationCommand { get; }

        private void Generation()
        {
            Entier = _chiffreService.generation().entier;
        }

        private void Incrementation()
        {
            Entier++;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
