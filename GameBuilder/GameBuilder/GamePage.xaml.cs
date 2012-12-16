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
    public static class IntExtensions
    {
        public static bool Between(this long num, long left, long right)
        {
            return num >= left && num <= right;
        }
    }

    public partial class GamePage : PhoneApplicationPage
    {
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;
        public static GameDefinition Definition { get; set; }

        const int blockSize = 30;
        int rows;
        int columns;
        int currentX;
        int currentY = 0;

        int height;
        int width;

        Texture2D GoodBlock;
        Texture2D BadBlock;
        Player player;
        List<GameBuilder.Definition.Block> Blocks;
        double playerYdiff = 0;
        double flyingStart;
        bool isFlying;
        double jumpSpeed = 200;
        int isJump = 0;
        bool dead;
        bool justNavigated = false;
        double startTime;
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
            justNavigated = true;

            // Establecer el modo de uso compartido del dispositivo gráfico para que active una representación de XNA
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

            // Cree un nuevo objeto SpriteBatch, que puede usarse para dibujar texturas.
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

            if (Definition == null)
            {
                MessageBox.Show("Couldn't load the game.");
                return;
            }

            height = 480;
            width = 800;

            rows = height / blockSize;
            columns = width / blockSize;
            player = new Player(SharedGraphicsDeviceManager.Current.GraphicsDevice, blockSize);
            player.X = Definition.InitialX;
            player.Y = Definition.InitialY;

            currentX = 0;

            GoodBlock = ColorTextureCreator.Create(SharedGraphicsDeviceManager.Current.GraphicsDevice,blockSize, blockSize, Color.Black);
            BadBlock = ColorTextureCreator.Create(SharedGraphicsDeviceManager.Current.GraphicsDevice,blockSize, blockSize, Color.Red);

            Blocks = Definition.Blocks.Select(x => new GameBuilder.Definition.Block(x)).ToList();
            isFlying = false;
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

        bool InViewport(GameObject x)
        {
            return (x.X * blockSize).Between(currentX - blockSize, currentX + width + blockSize)
                && (x.Y * blockSize).Between(currentY, currentY + height + blockSize);
        }

        void UpdateX(GameTimerEventArgs e)
        {
            double x;
            double ellapsed = Runtime(e) / 1000;

            x = Definition.Speed * ellapsed + Math.Pow(ellapsed, 2) * Definition.Acceleration / 2;

            currentX = (int)x;
        }

        double Runtime(GameTimerEventArgs e)
        {
            return e.TotalTime.TotalMilliseconds - startTime;
        }

        bool jumpStart;

        /// <summary>
        /// Permite que la página ejecute la lógica para, por ejemplo, actualizar el entorno,
        /// comprobar colisiones, recopilar datos de entrada y reproducir audio.
        /// </summary>
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {
            if (justNavigated)
            {
                justNavigated = false;
                startTime = e.TotalTime.TotalMilliseconds;
            }


            if (dead)
                return;

            var touches = TouchPanel.GetState();

            if (!isFlying && touches.Any(x => x.Position.X >= width / 2))
            {
                isFlying = true;
                jumpStart = true;
                isJump = 1;
            }

            UpdateX(e);

            if (currentX > Definition.FinalX * blockSize)
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("You won!!!!!"));
                timer.Stop();
                dead = true;
                Dispatcher.BeginInvoke(() => NavigationService.GoBack());
                return;
            }

            if (CheckCollision())
            {
                dead = true;
                return;
            }

            if (isFlying)
            {
                double flyingTime = Runtime(e) -flyingStart;

                if(jumpStart)
                    jumpStart = playerYdiff / blockSize < 1;

                if (!jumpStart && BlockUnderPlayer())
                {
                    player.Y += (int)Math.Round(playerYdiff / blockSize);
                    playerYdiff = 0;
                    isFlying = false;
                    isJump = 0;
                }
                else
                    playerYdiff = isJump * jumpSpeed * flyingTime / 1000 - Math.Pow(flyingTime / 1000, 2) * Definition.Gravity / 2;
            }
            else
            {
                if (!BlockUnderPlayer())
                {
                    isJump = 0;
                    isFlying = true;
                    flyingStart = Runtime(e);
                }
            }

            if (!(player.Y * blockSize + playerYdiff < currentY + height && player.Y * blockSize + playerYdiff > currentY))
                dead = true;

            if (dead)
            {
                MessageBox.Show("Has muerto.");
                Dispatcher.BeginInvoke(NavigationService.GoBack);
                timer.Stop();
            }
        }

        bool BlockUnderPlayer()
        {
            var block = Blocks.OrderBy(x => x.X).FirstOrDefault(x => x.X - (long) (currentX / blockSize) == player.X);

            return block != null && block.Y == (int)(player.Y + playerYdiff / blockSize);
        }

        private bool CheckCollision()
        {
            bool dies = false;

            var block = Blocks.OrderBy(x => x.X).FirstOrDefault(x => x.X * blockSize - currentX == (player.X + 1) * blockSize + playerYdiff);

            if (block != null && block.Y == player.Y)
                dies = true;

            if (dies)
            {
                MessageBox.Show("Has muerto.");
                Dispatcher.BeginInvoke(NavigationService.GoBack);
                timer.Stop();
            }

            return dies;
        }

        /// <summary>
        /// Permite a la página dibujarse a sí misma.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            Vector2 vPlayer = new Vector2(player.X * blockSize, (float)(height - ((player.Y + 1) * blockSize + playerYdiff)));
            
            spriteBatch.Begin();
            spriteBatch.Draw(player.Texture, new Microsoft.Xna.Framework.Rectangle((int)vPlayer.X, (int)vPlayer.Y, blockSize, blockSize), Color.White);

            foreach (var block in Blocks.Where(x => InViewport(x)))
            {
                Vector2 vBlock = new Vector2(block.X * blockSize - currentX, height - ((block.Y + 1) * blockSize - currentY));
                if (block.Type == BlockType.Good)
                    spriteBatch.Draw(GoodBlock, vBlock, Color.White);
                else
                    spriteBatch.Draw(BadBlock, vBlock, Color.White);
            }

            spriteBatch.End();
        }
    }
}