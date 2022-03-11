using ProyectoCDM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCDM.MVM.VieModel
{
    class MainViewModel: ObservableObject
    {
        public RelayCommand HomeViewCommmand { set; get; }
        public RelayCommand PetRegistroViewCommmand { set; get; }
        public RelayCommand HomeRegisterViewCommmand { set; get; }
        public RelayCommand ViewSecurityViewCommmand { set; get; }
        public RelayCommand MoveDetectionVielCommmand { set; get; }
        public HomeViewModel Homevm { set; get; }
        public PetRegisterModel PetRegister { set; get; }

        public HomeReginsterViewModel HomeReginstervm { set; get; }
        public MoveDetectionVielModel MoveDetectionVielvm { set; get; }
        public ViewSecurityViewModel ViewSecuritivm { set; get; }

        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        public MainViewModel()
        {
            Homevm = new HomeViewModel();
            PetRegister = new PetRegisterModel();
            HomeReginstervm = new HomeReginsterViewModel();
            MoveDetectionVielvm = new MoveDetectionVielModel();
            ViewSecuritivm = new ViewSecurityViewModel();
            CurrentView = Homevm;

            HomeViewCommmand = new RelayCommand(o =>
            {
                CurrentView = Homevm;
            });
            PetRegistroViewCommmand = new RelayCommand(o =>
            {
                CurrentView = PetRegister;
            });
            HomeRegisterViewCommmand = new RelayCommand(o =>
            {
                CurrentView = HomeReginstervm;

            });
            ViewSecurityViewCommmand = new RelayCommand(o =>
            {
                CurrentView = ViewSecuritivm;
            });
            MoveDetectionVielCommmand = new RelayCommand(o =>
            {
                CurrentView = MoveDetectionVielvm;
            });

        }
    }
}
