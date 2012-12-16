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
using Newtonsoft.Json;
using System.IO;
using System.Text;
using RestSharp;

namespace GameBuilder
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            GamePage.Definition = new Definition.GameDefinition
            {
                Blocks = new List<Definition.BlockJson>(),
                InitialX = 2,
                InitialY = 10,
                Gravity = 120,
                Speed = 50,
                Acceleration = 40,
                FinalX = 100
            };

            for (int i = 0; i < 20; i++)
                GamePage.Definition.Blocks.Add(new BlockJson { Type = BlockType.Good, X = i, Y = 3 });
            for (int i = 20; i < 70; i++)
                GamePage.Definition.Blocks.Add(new BlockJson { Type = BlockType.Good, X = i, Y = 4 });
            for (int i = 70; i < 100; i++)
                GamePage.Definition.Blocks.Add(new BlockJson { Type = BlockType.Good, X = i, Y = 1 });

            BuilderPage.Definition = GamePage.Definition;
        }

        // Controlador de eventos Click de botón simple que permite avanzar a la segunda página
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/BuilderPage.xaml", UriKind.Relative));
        }

        

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var serializer = new JsonSerializer();

            var sb = new StringBuilder();

            using (var memStream = new StringWriter(sb))
            {
                using (var jsonWriter = new JsonTextWriter(memStream))
                {
                    serializer.Serialize(jsonWriter, GamePage.Definition);
                }
            }

            var client = new RestClient();
            var request = new RestRequest("http://hackaya.com/api/addGame");
            request.Method = Method.POST;
            request.AddParameter("name", "asdasd");
            request.AddParameter("description", "Ultracool");
            request.AddParameter("data", sb.ToString());
            request.AddParameter("author", "Me");
            
            client.ExecuteAsync(request, (resp, handle) =>
            {
                if (resp.StatusCode == HttpStatusCode.OK)
                    Dispatcher.BeginInvoke(() => MessageBox.Show("Sent!"));
                else
                    Dispatcher.BeginInvoke(() => MessageBox.Show("Request failed. PS: This is demo effect. It worked on our PC."));
            });
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var client = new RestClient();
            var request = new RestRequest("http://hackaya.com/api/games");
            request.Method = Method.GET;

            client.ExecuteAsync(request, (resp, handle) =>
            {
                if (resp.StatusCode != HttpStatusCode.OK)
                {
                    Dispatcher.BeginInvoke(() => MessageBox.Show("Request failed. PS: This is demo effect. It worked on our PC."));
                    return;
                }

                var serializer = new JsonSerializer();
                IEnumerable<ServerData> games;
                using(var memStream = new StringReader(resp.Content))
                {
                    using (var reader = new JsonTextReader(memStream))
                    {
                        try
                        {
                            games = serializer.Deserialize<IEnumerable<ServerData>>(reader);
                        }
                        catch (Exception x)
                        {
                            Dispatcher.BeginInvoke(() => MessageBox.Show("Deserialization failed. PS: This is demo effect. It worked on our PC."));
                            return;
                        }
                    }
                }

                if (games.Any())
                {
                    GameDefinition game;
                    var serverData = games.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.data));

                    if (serverData == null)
                    {
                        Dispatcher.BeginInvoke(() => MessageBox.Show("no data received."));
                        return;
                    }

                    using (var memStream = new StringReader(serverData.data))
                    {
                        using (var reader = new JsonTextReader(memStream))
                        {
                            try
                            {
                                game = serializer.Deserialize<GameDefinition>(reader);
                                GamePage.Definition = game;
                                BuilderPage.Definition = game;
                                Dispatcher.BeginInvoke(() => MessageBox.Show("Downloaded!"));
                            }
                            catch (Exception x)
                            {
                                Dispatcher.BeginInvoke(() => MessageBox.Show("Deserialization failed. PS: This is demo effect. It worked on our PC."));
                                return;
                            }
                        }
                    }
                }
                else
                {
                    Dispatcher.BeginInvoke(() => MessageBox.Show("No games received :("));
                }

            });
        }
    }

    public class  ServerData
        {
            public string name;
            public string description;
            public string data;
            public string author;
        }
}