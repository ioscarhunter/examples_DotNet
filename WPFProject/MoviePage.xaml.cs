﻿using System;
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

namespace WPFProject
{
    /// <summary>
    /// Interaction logic for MoviePage.xaml
    /// </summary>
    public partial class MoviePage : UserControl,ISwitchable
    {
        
        private int status = 0;
        public MoviePage()
        {
            InitializeComponent();
            media.Source = new Uri("C:/Users/Compark/Desktop/2.mp4");
            media.Play();
            
        }
        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        #region ISwitchable Members
        private void back_button1(object sender, System.Windows.RoutedEventArgs e)
        {
            Switcher.Switch(new MainMenu());
        }
        private void inc_button1(object sender, RoutedEventArgs e)
        {

        }
        private void dec_button1(object sender, RoutedEventArgs e)
        {

        }
        private void play_button(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            playing();
        }
        private void playing() {
            if (status == 0)
            {
                media.Pause();
                status = 1;
            }
            else
            {
                media.Play();
                status = 0;
            }
        }

        private void Up_volume_Click(object sender, RoutedEventArgs e)
        {
            inc_volume();
        }
        private void inc_volume() {
            if (media.Volume >= 0 && media.Volume < 1)
            {
                media.Volume += 0.25;
                Volume.Value += 0.25;
            }

            else if (media.Volume >= 1)
            {
                media.Volume = 1;
            }
        
        
        }


        private void Down_volume_Click(object sender, RoutedEventArgs e)
        {
            dec_volume();
        }
        private void dec_volume() {
            if (media.Volume > 0 && media.Volume <= 1)
            {
                media.Volume -= 0.25;
                Volume.Value -= 0.25;
            }
            else if (media.Volume <= 0)
            {
                media.Volume = 0;
            }
        
        
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            Switch_back();

        }
        private void Switch_back(){
            media.Stop();
            Switcher.Switch(new MainMenu());
        }

        public void sendcommand(int command)
        {
            switch (command)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
            }
        }


    }
}
