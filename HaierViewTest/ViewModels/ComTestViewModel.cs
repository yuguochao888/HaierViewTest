using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;

namespace HaierViewTest.ViewModels
{
    [POCOViewModel]
  public  class ComTestViewModel
    {
        public virtual bool CanExecuteSaveCommand { get; set; } = true;


        DelegateCommand _SaveCommand;
        public DelegateCommand SaveCommand
        {
            get
            {
                return _SaveCommand ??
                       (_SaveCommand = new DelegateCommand(Save, CanSave));
            }
        }

        DelegateCommand<string> _OpenCommand;
        public DelegateCommand<string> OpenCommand
        {
            get
            {
                return _OpenCommand ??
                       (_OpenCommand = new DelegateCommand<string>(Open, CanOpen));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void Save()
        {
            IsOpen = !IsOpen;
            MessageBox.Show("Action: Save");

        }
        public bool CanSave()
        {
            return CanExecuteSaveCommand;

        }

        public virtual bool IsOpen { set; get; }

        public virtual string FileName { get; set; }
        public void Open(string fileName)
        {
            //MessageBox.Show(string.Format("Action: Open {0}", fileName));
        }
        public bool CanOpen(string fileName)
        {
            return !string.IsNullOrEmpty(fileName);
        }


        private void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
