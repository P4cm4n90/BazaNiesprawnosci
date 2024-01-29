using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.ComponentModel;

namespace BazaNiesprawnosci
   
{
    /// <summary>
    /// Interaction logic for WPasswdBox.xaml
    /// </summary>
    public partial class WPasswdBox : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public delegate void DataBaseStatusHandler(object sender, TextEventArgs eargs);
        public event DataBaseStatusHandler OnStatusChange;

        private string password = "muminki";
        private string typePassword = "";
        public bool result = false;

        public string TypePassword { get => typePassword; set { typePassword = value;
                NotifyPropertyChanged("TypePassword"); } }

        public WPasswdBox()
        {
            InitializeComponent();
            tbPassword.DataContext = this;
            //this.DataContext = this;
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        public void NotifyStatusChanged(string text)
        {
            OnStatusChange?.Invoke(this, new TextEventArgs(text));
        }

        private void tbPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if(string.Compare(password,TypePassword) == 0)
                {
                    result = true;
                    tbPassword.Password = "";
                    TypePassword = "";
                    this.Close();
                }
                else
                {
                    NotifyStatusChanged(Properties.Resources.PasswordBoxBadPassword);
                    tbPassword.Password = "";
                    TypePassword = "";
                    
                    
                }
            }
        }

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            TypePassword = tbPassword.Password;
        }
    }
}
