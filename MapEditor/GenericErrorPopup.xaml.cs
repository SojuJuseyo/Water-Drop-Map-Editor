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
using System.Windows.Shapes;

namespace MapEditor
{
    /// <summary>
    /// Logique d'interaction pour GenericErrorPopup.xaml
    /// </summary>
    public partial class GenericErrorPopup : Window
    {
        public GenericErrorPopup()
        {
            InitializeComponent();
        }

        public void setErrorMessage(string WindowTitle, string ErrorMessage)
        {
            this.Title = WindowTitle;
            errorMessageLabel.Content = ErrorMessage;
        }
    }
}
