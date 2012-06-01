﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Surface.Presentation.Controls;

namespace OpenAttractor
{
    /// <summary>
    /// Interaction logic for VideoPlayer.xaml
    /// </summary>
    public partial class VideoPlayer : UserControl
    {
        public string Source { get { return (string)GetValue(_sourceProperty); } set { SetValue(_sourceProperty, value); } }
        public static readonly DependencyProperty _sourceProperty = DependencyProperty.Register("Source", typeof(string), typeof(VideoPlayer), new FrameworkPropertyMetadata(String.Empty));

        public double CurrentVideoProgress { get { return (double)GetValue(_currentVideoProgress); } set { SetValue(_currentVideoProgress, value); } }
        public static readonly DependencyProperty _currentVideoProgress = DependencyProperty.Register("CurrentVideoProgress", typeof(double), typeof(VideoPlayer), new FrameworkPropertyMetadata((double)0));

		public bool VideoIsPlaying { get { return (bool)GetValue(_videoIsPlaying); } set { SetValue(_videoIsPlaying, value); } }
		public static readonly DependencyProperty _videoIsPlaying = DependencyProperty.Register("VideoIsPlaying", typeof(bool), typeof(VideoPlayer), new FrameworkPropertyMetadata(false));

        private Timer _playTimer;

        public VideoPlayer()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(VideoPlayer_Loaded);
        }

        void VideoPlayer_Loaded(object sender, RoutedEventArgs e)
        {
            videoPlayer.MediaEnded += delegate(object o, RoutedEventArgs args)
            {
                videoPlayer.Position = new TimeSpan(0, 0, 0, 0);
                videoPlayer.Play();
            };

            _playTimer = new Timer {Interval = 300};
            _playTimer.Elapsed += delegate(object o, ElapsedEventArgs args)
                                      {
                                          Application.Current.Dispatcher.BeginInvoke(
                                              DispatcherPriority.Background,
                                              new Action(() => CurrentVideoProgress =
                                                               videoPlayer.Position.TotalMilliseconds/
                                                               videoPlayer.NaturalDuration.TimeSpan.TotalMilliseconds));
                                      };
        }


        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            videoPlayer.Play();
            _playTimer.Start();
        	VideoIsPlaying = true;
        }

        private void RewindButton_Click(object sender, RoutedEventArgs e)
        {
            videoPlayer.Position = new TimeSpan(0, 0, 0, 0);
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            videoPlayer.Pause();
            _playTimer.Stop();
			VideoIsPlaying = false;
        }

        private void videoPlayer_Loaded(object sender, RoutedEventArgs e)
        {
            ((MediaElement)sender).Play();
			((MediaElement)sender).Position = new TimeSpan(0, 0, 0, 1);
            ((MediaElement)sender).Pause();
        }

        private void PlayButtonSmall_Click(object sender, RoutedEventArgs e)
        {
            videoPlayer.Play();
            _playTimer.Start();
			VideoIsPlaying = true;
        }
    }
}
