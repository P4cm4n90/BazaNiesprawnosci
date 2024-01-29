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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Timers;
using System.Threading.Tasks;

namespace BazaNiesprawnosci
{
    /// <summary>
    /// Interaction logic for CPartControl.xaml
    /// </summary>
    public partial class CPartControl : UserControl, INotifyPropertyChanged
    {
        private bool copyPn = false;
        private bool pnCopy = false;
        private int fontSizeEdit = 15;
        private int fontSizeLabel = 15;
        private Timer enterCounter;
        private bool extendedVersion;
        public bool ExtendedVersion
        {
            get
            {
                return extendedVersion;
            }
            set
            {
                extendedVersion = value;
                NotifyPropertyChanged("ExtendedVersion");
                if (extendedVersion)
                    gExtItem.Visibility = Visibility.Visible;
                else
                    gExtItem.Visibility = Visibility.Collapsed;

            
            }
        }

        
        public bool CopyPn
        {
            get
            {
                return copyPn;
            }
            set
            {
                copyPn = value;
                NotifyPropertyChanged("CopyPn");
                
            }
        }

        public int FontSizeEdit
        {
            get
            {
                return fontSizeEdit;
            }
            set
            {
                fontSizeEdit = value;
                NotifyPropertyChanged("FontSizeEdit");
            }
        }

        public int FontSizeLabel
        {
            get
            {
                return fontSizeLabel;
            }
            set
            {
                fontSizeLabel = value;
                NotifyPropertyChanged("FontSizeLabel");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public CPartControl()
        {
            InitializeComponent();
            if (MainSettings.Default.UseExtendedVersion)
            {
                extendedVersion = true;
                gExtItem.Visibility = Visibility.Visible;
            }
            else
            {
                extendedVersion = false;
                gExtItem.Visibility = Visibility.Collapsed;
            }

            enterCounter = new Timer()
            {
                Interval = 1000
            };
            enterCounter.Elapsed += delegate(object sender, ElapsedEventArgs e)
            {
                copyPn = false;
            };

            

        }

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        public void MaintainPn(object sender, KeyboardEventArgs eargs)
        {
            if(sender.Equals(tbIdPartNumber))
            {
                tbInPartNumber.Text = tbIdPartNumber.Text;
            }
            else if(sender.Equals(tbInPartNumber))
            {
                tbIdPartNumber.Text = tbInPartNumber.Text;
            }
        }
        // experimental
        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {

                if (!enterCounter.Enabled) 
                    enterCounter.Start();
                else
                {
                    if (!pnCopy)
                    {
                        tbInPartNumber.Text = tbIdPartNumber.Text;
                        tbInPartNumber.KeyUp += MaintainPn;
                        tbIdPartNumber.KeyUp += MaintainPn;
                        pnCopy = true;
                        enterCounter.Stop();
                        tbIdPartNumber.Background = Brushes.LawnGreen;
                        tbInPartNumber.Background = Brushes.LawnGreen;
                    }
                    else
                    {
                        tbInPartNumber.KeyUp -= MaintainPn;
                        tbIdPartNumber.KeyUp -= MaintainPn;
                        pnCopy = false;
                        enterCounter.Stop();
                        tbIdPartNumber.Background = Brushes.White;
                        tbInPartNumber.Background = Brushes.White;
                    }
                }
            }
        }

        private void tbIdFlightHours_KeyUp(object sender, KeyEventArgs e)
        {
            string text = tbIdFlightHours.Text;
            int temp;
            if(!Int32.TryParse(text,out temp))
            {
                tbIdFlightHours.Text = "";
            }
        }
    }
}
