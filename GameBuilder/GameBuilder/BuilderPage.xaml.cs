using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameBuilder.Definition;
using GameBuilder.Helpers;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input.Touch;

namespace GameBuilder
{
    public partial class BuilderPage : PhoneApplicationPage
    {
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;
        public static GameDefinition Definition { get; set; }

        public BuilderPage()
        {
            InitializeComponent();

            // Obtenga el administrador de contenido de la aplicación
            contentManager = (Application.Current as App).Content;

            // Crear un temporizador para esta página
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Establecer el modo de uso compartido del dispositivo gráfico para que active una representación de XNA
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

            // Cree un nuevo objeto SpriteBatch, que puede usarse para dibujar texturas.
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

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
        }

        /// <summary>
        /// Permite a la página dibujarse a sí misma.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {  
            spriteBatch.Begin();
           
            spriteBatch.End();
        }
    }
}