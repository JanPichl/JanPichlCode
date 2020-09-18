//-----------------------------------------------------------------------
// <copyright file="MainWindow.cs" company="I&C Energo a.s.">
//     Copyright (c) 
// </copyright>
// <author>
//     Jan Pichl
// </author>
//-----------------------------------------------------------------------

namespace JanPichlCode
{
    using JanPichlCode.Configuration;
    using JanPichlCode.ViewModel;
    using log4net;
    using System;
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(MainWindow));

        private readonly FurnaceBoardViewModel _furnaceBoardViewModel;

        public MainWindow()
        {
            try
            {
                InitializeComponent();

                FurnaceBoardSection section = FurnaceBoardSection.GetSection();

                int? checkInterval = section.CheckIntervalInSeconds;

                if (checkInterval == null)
                {
                    checkInterval = 20;
                }

                _furnaceBoardViewModel = new FurnaceBoardViewModel((int)checkInterval, section);

                this.Content = _furnaceBoardViewModel;
            }
            catch(Exception ex)
            {
                _log.Error(nameof(MainWindow), ex);
            }
        }

    }
}
