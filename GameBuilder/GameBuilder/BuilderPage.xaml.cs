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
        BlockType SelectedType;
        int blockSize = 30;
        List<Block> Blocks = new List<Block>();
        Texture2D GoodBlockTexture;
        Texture2D BadBlockTexture;
        UIElementRenderer elementRenderer;

        public BuilderPage()
        {
            InitializeComponent();

            // Obtenga el administrador de contenido de la aplicación
            contentManager = (Application.Current as App).Content;
            LayoutUpdated += new EventHandler(XNARendering_LayoutUpdated);
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

            // Iniciar el temporizador
            timer.Start();

            Blocks = Definition.Blocks.Select(x => new GameBuilder.Definition.Block(x)).ToList(); 

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
            var touches = TouchPanel.GetState();

            if (touches.Count == 1)
            {
                var touch = touches[0];

                if (touch.State != TouchLocationState.Invalid)
                {
                    var block = new BlockJson();
                    block.Type = SelectedType;
                    block.X = (long)(touch.Position.X / blockSize);
                    block.Y = (long)((480 - touch.Position.Y) / blockSize);

                    Definition.Blocks.Add(block);
                    Blocks.Add(new Block(block));
                }
            }
        }

        /// <summary>
        /// Permite a la página dibujarse a sí misma.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            spriteBatch.Begin();
            GoodBlockTexture = ColorTextureCreator.Create(SharedGraphicsDeviceManager.Current.GraphicsDevice, blockSize, blockSize, Color.Black);
            BadBlockTexture = ColorTextureCreator.Create(SharedGraphicsDeviceManager.Current.GraphicsDevice, blockSize, blockSize, Color.Red);

            foreach (var block in Blocks.Where(x => x.X*blockSize < 800))
            {
                Vector2 vBlock = new Vector2(block.X * blockSize, 480 - ((block.Y + 1) * blockSize));
                if (block.Type == BlockType.Good)
                    spriteBatch.Draw(GoodBlockTexture, vBlock, Color.White);
                else
                    spriteBatch.Draw(BadBlockTexture, vBlock, Color.White);
            }

            spriteBatch.End();

        }

        private void Bad_Click_1(object sender, RoutedEventArgs e)
        {
            SelectedType = BlockType.Bad;
        }

        private void GoodBlock_Click_1(object sender, RoutedEventArgs e)
        {
            SelectedType = BlockType.Good;
        }

        private void Done_Click_1(object sender, RoutedEventArgs e)
        {
            GamePage.Definition = Definition;
            NavigationService.GoBack();
        }

        void XNARendering_LayoutUpdated(object sender, EventArgs e)
        {
            // make sure page size is valid
            if (ActualWidth == 0 || ActualHeight == 0)
                return;

            // see if we already have the right sized renderer
            if (elementRenderer != null &&
                elementRenderer.Texture != null &&
                elementRenderer.Texture.Width == (int)ActualWidth &&
                elementRenderer.Texture.Height == (int)ActualHeight)
            {
                return;
            }

            // dispose the current renderer
            if (elementRenderer != null)
                elementRenderer.Dispose();

            // create the renderer
            elementRenderer = new UIElementRenderer(this, (int)ActualWidth, (int)ActualHeight);
        }

    }
}