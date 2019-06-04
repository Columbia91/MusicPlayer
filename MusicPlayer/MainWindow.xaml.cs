using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace MusicPlayer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }
        
        string[] files;
        MediaPlayer mediaPlayer = new MediaPlayer();
        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.AddExtension = true;
            ofd.DefaultExt = "*.*";
            ofd.Filter = "All files (*.*)|*.*";
            ofd.Multiselect = true;
            ofd.ShowDialog();

            files = ofd.FileNames;

            foreach (string song in files)
            {
                if (!listBox.Items.Contains(song))
                {
                    listBox.Items.Add(song);
                }
            }

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            lblName.Content = (listBox.SelectedValue).ToString();
            mediaPlayer.Open(new Uri((listBox.SelectedValue).ToString()));
            mediaPlayer.Play();
        }

        private void listBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            lblName.Content = (listBox.SelectedValue).ToString();
            mediaPlayer.Open(new Uri((listBox.SelectedValue).ToString()));
            mediaPlayer.Play();
        }
    }
}
