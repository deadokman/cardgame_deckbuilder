using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace OggSharp
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class OggSharpGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        OggSong current;
        OggSong song1;
        OggSong song2;
        SpriteFont font;

        public OggSharpGame()
        {
            graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Font");
            song1 = new OggSong(TitleContainer.OpenStream("OggTest.ogg"), false);
            song2 = new OggSong(TitleContainer.OpenStream("Phasor.ogg"), false);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            KeyboardState keyState = Keyboard.GetState();

            // TODO: Add your update logic here
            if (keyState.IsKeyDown(Keys.D1))
            {
                if (current != null && current != song1)
                {
                    current.Stop();
                }
                current = song1;
                current.Play();
            }
            else if (keyState.IsKeyDown(Keys.D2))
            {
                song2.Play();
            }
            else if (keyState.IsKeyDown(Keys.S))
            {
                if (current != null)
                {
                    current.Stop();
                }
            }
            else if (keyState.IsKeyDown(Keys.G))
            {
                if (current != null)
                {
                    current.Play();
                }
            }
            else if (keyState.IsKeyDown(Keys.P))
            {
                if (current != null)
                {
                    current.Pause();
                }
            }
            else if (keyState.IsKeyDown(Keys.R))
            {
                if (current != null)
                {
                    current.Resume();
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            string pos = "Song1 Pos: " + song1.Position.ToString("0.00") + " / " + song1.Length.ToString("0.00") + "\nSong2 Pos: " + song2.Position.ToString("0.00") + " / " + song2.Length.ToString("0.00");
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "1 - Sound 1\n2 - Sound 2\nS - Stop sound 1\nG - Start sound 1\nP - Pause sound 1\nR - Resume sound 1\n" + pos, new Vector2(50, 50), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

#if WINDOWS || XBOX
        static class Program
        {
            /// <summary>
            /// The main entry point for the application.
            /// </summary>
            static void Main(string[] args)
            {
                using (OggSharpGame game = new OggSharpGame())
                {
                    game.Run();
                }
            }
        }
#endif
    }
}
