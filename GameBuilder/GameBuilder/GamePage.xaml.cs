using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameBuilder
{
    public partial class GamePage : PhoneApplicationPage
    {
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;

        public GamePage()
        {
            InitializeComponent();

            // Obtenga el administrador de contenido de la aplicación
            contentManager = (Application.Current as App).Content;

            // Crear un temporizador para esta página
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromTicks(333333);
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Establecer el modo de uso compartido del dispositivo gráfico para que active una representación de XNA
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

            // Cree un nuevo objeto SpriteBatch, que puede usarse para dibujar texturas.
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);
         
            // TODO: usar este contenido para cargar el contenido del juego aquí

            // Iniciar el temporizador
            timer.Start();

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Detener el temporizador
            timer.Stop();

            // Establecer el modo de uso compartido del dispositivo gráfico para que desactive una representación de XNA
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);

            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// Permite que la página ejecute la lógica para, por ejemplo, actualizar el entorno,
        /// comprobar colisiones, recopilar datos de entrada y reproducir audio.
        /// </summary>
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {
            // TODO: Agregar aquí la lógica de actualización
        }

        /// <summary>
        /// Permite a la página dibujarse a sí misma.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Agregar aquí el código de dibujo
        }
    }
}