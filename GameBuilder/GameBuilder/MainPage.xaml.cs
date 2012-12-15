using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using GameBuilder.Definition;

namespace GameBuilder
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        // Controlador de eventos Click de botón simple que permite avanzar a la segunda página
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GamePage.Definition = new Definition.GameDefinition
            {
                Blocks = new List<Definition.BlockJson>(),
                InitialX = 2,
                InitialY = 10,
                Gravity = 70,
                Speed = 20,
                Acceleration = 30
            };

            for (int i = 0; i < 50; i++)
                GamePage.Definition.Blocks.Add(new BlockJson { Type = BlockType.Good, X = i, Y = 3 });
            for (int i = 50; i < 100; i++)
                GamePage.Definition.Blocks.Add(new BlockJson { Type = BlockType.Good, X = i, Y = 4 });
            for (int i = 100; i < 200; i++)
                GamePage.Definition.Blocks.Add(new BlockJson { Type = BlockType.Good, X = i, Y = 1 });
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }
    }
}