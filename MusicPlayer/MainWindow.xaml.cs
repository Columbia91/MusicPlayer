using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MusicPlayer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MediaPlayer mediaPlayer = new MediaPlayer();
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (mediaPlayer.Source != null)
            {
                timeLabel.Content = String.Format("{0} / {1}",
                    mediaPlayer.Position.ToString(@"mm\:ss"), mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
                progressSlider.Minimum = 0;
                progressSlider.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                progressSlider.Value = mediaPlayer.Position.TotalSeconds;
            }
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(delegate ()
            {
                Action action = () =>
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
                    if (openFileDialog.ShowDialog() == true)
                    {
                        mediaPlayer.Open(new Uri(openFileDialog.FileName));
                        musicName.Content = Path.GetFileName(openFileDialog.FileName);
                    }
                };

                if (!Dispatcher.CheckAccess())
                    Dispatcher.Invoke(action);
                else
                    action();
            })
            {
                IsBackground = false
            };
            thread.Start();
        }
        
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Play();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
        }
        
        private void ProgressSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            mediaPlayer.Position = TimeSpan.FromSeconds(progressSlider.Value);
        }
        public void SaveNotes()
        {
            using(var writer = new StreamWriter("file.txt", false, Encoding.Default))
            {
                writer.WriteLine(notesTextBox.Text);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Thread thread = new Thread(delegate ()
            {
                Action action = () =>
                {
                    SaveNotes();
                };

                if (!Dispatcher.CheckAccess())
                    Dispatcher.Invoke(action);
                else
                    action();
            });
            thread.Start();
        }
    }
}