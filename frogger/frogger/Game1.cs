using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;


namespace frogger
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        int windowWidth = 600;
        int windowHeight = 600;
        Texture2D squareTexture;
        Texture2D carTexture;
        Texture2D frogTexture;
        Frog frogPlayer;
        KeyboardState oldKeyState;
        List<Car> cars = new List<Car>();
        Random rand = new Random();
        SpriteFont font;
        int timeToSpawnCar = 0;
        int maxCars = 15;
        
        class Frog
        {
            int posX;
            int posY;
            int size;
            Texture2D texture;
            Rectangle rectangleFrog;
            float elapsed;
            int frame;
            int rotation;
            public Frog(Texture2D _texture)
            {
                posX = 280;
                posY = 560;
                size = 40;
                texture = _texture;
                rectangleFrog = new Rectangle(posX, posY, size, size);
                elapsed = 0;
                frame = 0;
                rotation = 0;
            }

            public void DrawFrog(SpriteBatch spriteBatch)
            {
                if (elapsed >= 200)
                {
                    frame = 0;
                    elapsed = 0;
                }

                spriteBatch.Begin();
                /// nie dziala
                /// jak ma 
                spriteBatch.Draw(texture, new Rectangle(posX,posY+16,size,size), new Rectangle(64 * frame, 0, 64, 64), Color.White, 
                    MathHelper.ToRadians(90 * rotation),
                        new Vector2(32,32), SpriteEffects.None, 0);

                //spriteBatch.Draw(texture, rectangleFrog, Color.White);
                spriteBatch.End();
            }

            public void MoveFrog(string moveDirection)
            {
                elapsed = 0;
                frame = 1;
                if (moveDirection.Equals("up") && posY > size )
                {
                    rotation = 0;

                    posY -= size;
                }
                else if (moveDirection.Equals("down") && posY < 560)
                {
                    rotation = 2;
                    posY += size;
                }
                else if (moveDirection.Equals("left") && posX >= size)
                {
                    rotation = 3;
                    posX -= size;
                }
                else if (moveDirection.Equals("right") && posX < 560)
                {
                    posX += size;
                    rotation = 1;
                }
                rectangleFrog = new Rectangle(posX, posY, size, size);
            }

            public void SetStartPosition()
            {
                posX = 280;
                posY = 560;
                rectangleFrog = new Rectangle(posX, posY, size, size);
            }

            public void IncreaseElapsed(int time)
            {
                elapsed += time;
            }

            //seters
            public void SetPosX(int _posX)
            {
                posX = _posX;
            }

            public void SetPosY(int _posY)
            {
                posY = _posY;
            }

            //getters

            public Rectangle GetRectangleFrog()
            {
                return rectangleFrog;
            }





        }

        class Car
        {
            int posX;
            int posY;
            int size;
            Texture2D texture;
            Rectangle rectangleCar;
            bool moveRight;
            int speed;
            int frame;
            float elapsed;
            public Car(Texture2D _texture)
            {
                posX = -40;
                posY = 520;
                size = 40;
                texture = _texture;
                rectangleCar = new Rectangle(posX, posY, size, size);
                moveRight = true;
                speed = 1;
                frame = 0;
                elapsed = 0;
            }

            public Car(Texture2D _texture, Random rand )
            {
                int buff = rand.Next(5);
                size = 40;
                posY = 360 + (buff * size);                
                texture = _texture;
                if (buff == 0 || buff == 2)
                {
                    moveRight = true;
                    posX = -40;
                }
                else
                {
                    moveRight = false;
                    posX = 600;
                }
                rectangleCar = new Rectangle(posX, posY, size, size);
                speed = 3;
                frame = 0;
                elapsed = 0;
            }

            public void DrawCar(SpriteBatch spriteBatch)
            {
                spriteBatch.Begin();
                if (elapsed >= 40)
                {
                    if (frame >= 1)
                        frame = 0;
                    else
                        frame++;
                    elapsed = 0;
                }

                if (moveRight)
                {
                    spriteBatch.Draw(texture, rectangleCar, new Rectangle(64 * frame, 0, 64, 64), Color.White, MathHelper.ToRadians(180),
                        new Vector2(64, 64), SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.Draw(texture, rectangleCar, new Rectangle(64 * frame, 0, 64, 64), Color.White, 0,
                        new Vector2(0, 0), SpriteEffects.None, 0);
                }
                spriteBatch.End();
            }

            public void MoveCar()
            {
                if (moveRight)
                    posX += speed;
                else
                    posX -= speed;
                rectangleCar = new Rectangle(posX, posY, size, size);
            }

            public bool OutOfMap()
            {
                if (posX < -40 && posX > 640)
                    return true;
                else
                    return false;
            }

            public bool CanSpawnCar(List<Car> cars)
            {
                for (int i = 0; i < cars.Count; i++)
                {
                    if (cars[i].GetPosY() == posY)
                    {
                        int distance = cars[i].GetPosX() - posX;
                        if (distance > -160 && distance < 160 )
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

            public bool DestroyCar()
            {
                if (posX > 610 || posX < -50)
                    return true;
                else
                    return false;
            }

            public bool HitFrog(Frog frog)
            {
                if (rectangleCar.Intersects(frog.GetRectangleFrog()))
                {
                    frog.SetStartPosition();
                    return true;
                }
                else
                    return false;
            }

            public void IncreaseElapsed(int time)
            {
                elapsed += (float) time;
            }

            //getters
            public int GetPosX()
            {
                return posX;
            }

            public int GetPosY()
            {
                return posY;
            }



        }





        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.PreferredBackBufferHeight = windowHeight;
            graphics.ApplyChanges();
            


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
            squareTexture = Content.Load<Texture2D>("kwadrat");
            carTexture = Content.Load<Texture2D>("auto");
            frogTexture = Content.Load<Texture2D>("frog");
            frogPlayer = new Frog(frogTexture);
            
            //cars.Add(new Car(squareTexture, rand));
            font = Content.Load<SpriteFont>("napis");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            for (int i = cars.Count - 1; i >= 0; i--)
            {
                cars[i].IncreaseElapsed(gameTime.ElapsedGameTime.Milliseconds);
            }
           
            frogPlayer.IncreaseElapsed(gameTime.ElapsedGameTime.Milliseconds);
            KeyboardState keyState = Keyboard.GetState();
           
            if (keyState.IsKeyDown(Keys.Up) && oldKeyState.IsKeyUp(Keys.Up))
            {
                frogPlayer.MoveFrog("up");
                
            }
            else if (keyState.IsKeyDown(Keys.Down) && oldKeyState.IsKeyUp(Keys.Down))
            {
                frogPlayer.MoveFrog("down");
            }
            else if (keyState.IsKeyDown(Keys.Left) && oldKeyState.IsKeyUp(Keys.Left))
            {
                frogPlayer.MoveFrog("left");
            }
            else if(keyState.IsKeyDown(Keys.Right) && oldKeyState.IsKeyUp(Keys.Right))
            {
                frogPlayer.MoveFrog("right");
            }

            oldKeyState = keyState;

            //hit frog
            for (int i = cars.Count - 1; i >= 0; i--)
            {
                cars[i].HitFrog(frogPlayer);
            }

            //move cars
            for (int i = 0; i < cars.Count; i++)
            {
                cars[i].MoveCar();
            }

            //spawn Cars
            Car car = new Car(carTexture, rand);
            if (maxCars > cars.Count && car.CanSpawnCar(cars) && timeToSpawnCar <= 0)
                cars.Add(car);
            if (timeToSpawnCar <= 0)
                timeToSpawnCar = rand.Next(0, 30);
            timeToSpawnCar--;

            //destroy cars
            for (int i = cars.Count - 1; i >= 0; i--)
            {
                if (cars[i].DestroyCar())
                    cars.Remove(cars[i]);
            }

            
            // TODO: Add your update logic here

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


            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Liczba samochodow: " + cars.Count, Vector2.Zero, Color.Black);
            spriteBatch.Draw(squareTexture, new Rectangle(0, 360, 600, 200), Color.Black);

            spriteBatch.End();

            frogPlayer.DrawFrog(spriteBatch);
            for (int i = 0; i < cars.Count; i++)
            {
                cars[i].DrawCar(spriteBatch);
            }
            

            base.Draw(gameTime);
        }
    }
}
