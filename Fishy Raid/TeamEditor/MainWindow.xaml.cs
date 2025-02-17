﻿using System;
using System.Collections.Generic;
using System.IO;
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

namespace TeamEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            #region Geting Background img path
            string imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "background.jpg");
            ImageBrush backgroundBrush = new ImageBrush();
            Image image = new Image();
            image.Source = new BitmapImage(
                new Uri(imgPath));
            backgroundBrush.ImageSource = image.Source;
            #endregion

            #region Geting Button img path

            string buttonPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "gomb.png");
            ImageBrush buttonBrush = new ImageBrush();
            Image image2 = new Image();
            image2.Source = new BitmapImage(
                new Uri(buttonPath));
            buttonBrush.ImageSource = image2.Source;

            #endregion            

            InitializeComponent();

            myGrid.Background = backgroundBrush;
            
        }
       
    }
}
