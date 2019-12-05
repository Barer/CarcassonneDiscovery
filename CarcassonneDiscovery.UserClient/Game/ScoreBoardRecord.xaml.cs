﻿namespace CarcassonneDiscovery.UserClient
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Interakční logika pro ScoreBoardRecord.xaml
    /// </summary>
    public partial class ScoreBoardRecord : UserControl
    {
        /// <summary>
        /// Score of the player.
        /// </summary>
        private int _Score;

        /// <summary>
        /// Number of unoccupied followers of the player.
        /// </summary>
        private int _Followers;

        /// <summary>
        /// Is player on move?
        /// </summary>
        private bool _OnMove;

        /// <summary>
        /// Score of the player.
        /// </summary>
        public int Score
        {
            get { return _Score; }
            set
            {
                _Score = value;

                Dispatcher.Invoke(new Action(() => ScoreLabel.Content = _Score.ToString()));
            }
        }

        /// <summary>
        /// Number of unoccupied followers of the player.
        /// </summary>
        public int Followers
        {
            get { return _Followers; }
            set
            {
                _Followers = value;

                Dispatcher.Invoke(new Action(() => FollowersLabel.Content = $"({_Followers})"));
            }
        }

        /// <summary>
        /// Is player on move?
        /// </summary>
        public bool OnMove
        {
            get { return _OnMove; }
            set
            {
                _OnMove = value;

                Dispatcher.Invoke(new Action(() => OnMoveEllipse.Visibility = (value ? Visibility.Visible : Visibility.Hidden)));
            }
        }

        public ScoreBoardRecord(string name, PlayerColor color)
        {
            InitializeComponent();

            Brush backgroundBrush = null;
            Brush foregroundBrush = null;

            switch(color)
            {
                case PlayerColor.Black:
                    backgroundBrush = Brushes.Black;
                    foregroundBrush = Brushes.White;
                    break;
                case PlayerColor.Red:
                    backgroundBrush = Brushes.Red;
                    foregroundBrush = Brushes.Black;
                    break;
                case PlayerColor.Green:
                    backgroundBrush = Brushes.Green;
                    foregroundBrush = Brushes.Black;
                    break;
                case PlayerColor.Blue:
                    backgroundBrush = Brushes.Blue;
                    foregroundBrush = Brushes.Black;
                    break;
                case PlayerColor.Yellow:
                    backgroundBrush = Brushes.Yellow;
                    foregroundBrush = Brushes.Black;
                    break;
            }

            NameLabel.Content = name;
            Score = 0;
            Followers = 0;
            OnMove = false;

            NameLabel.Foreground = foregroundBrush;
            ScoreLabel.Foreground = foregroundBrush;
            FollowersLabel.Foreground = foregroundBrush;
            OnMoveEllipse.Fill = foregroundBrush;
            Background = backgroundBrush;
        }
    }
}
