/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2022 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
---------------------------------------------------------------------------- */

using Aristurtle.TinyEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Aristurtle.TinyGame.Scenes
{
    public class PlayScene : BaseScene
    {
        //  Defines the values used for a body piece of a snake.
        private struct BodyPiece
        {
            public Point GridLocation;
            public Rectangle RenderRect;
            public Point Direction;
        }

        // ---------------------------------------------------------------------
        //
        //  Assets
        //
        // ---------------------------------------------------------------------

        //  Font used to render text to the scene.
        private SpriteFont _font;

        //  The texture to use when rendering a grid cell that's empty.
        private TinyTexture _gridCellTexture;

        // ---------------------------------------------------------------------
        //
        //  HUD Elements
        //
        // ---------------------------------------------------------------------

        //  The game over text.
        private Text _gameoverText;

        //  The Try Again text object.
        private Text _tryAgainText;

        //  The Score text object.
        private Text _scoreText;

        //  The next wave text object.
        private Text _nextWaveText;

        //  The begin countdown text object.
        private Text _beginCountdownText;

        //  The pause text object.
        private Text _pauseText;

        //  The bottom container on the screen where text such as score is
        //  displayed.
        private Rectangle _bottomContainer;

        //  The container that holds the game grid.
        private Rectangle _gridContainer;

        //  The container that hold the game over text.
        private Rectangle _gameoverContainer;

        //  The container that hold the countdown timer
        private Rectangle _beginCountdownContainer;

        // ---------------------------------------------------------------------
        //
        //  Timing and Timers
        //
        // ---------------------------------------------------------------------

        //  The amount of time to elapse before a tick occurs.
        private readonly TimeSpan _timeUntilTick = TimeSpan.FromSeconds(0.1);

        //  Tracks the amount of time remaining for a tick to occur.
        private TimeSpan _tickTimer;

        //  A 10 second timespan used for comparison.
        private readonly TimeSpan _tenSeconds = TimeSpan.FromSeconds(10);

        //  The amount of time to countdown before the game begins.
        private TimeSpan _beginTime = TimeSpan.FromSeconds(3);

        //  Used to track the every ten seconds event.
        private TimeSpan _tenSecondTimer;

        //  The timer used when a pulse/beat occurs from the music.
        private float _pulseTimer = 0.0f;

        //  The duration of a pulse. Should always match the crochet.
        private float _pulseDuration = GameBase.Music.Crochet;

        // ---------------------------------------------------------------------
        //
        //  Flags (state management. Should have used an enum probably)
        //
        // ---------------------------------------------------------------------

        //  Indicates that a game over has occured.
        private bool _gameover = false;

        //  Indicates that the game has been paused.
        private bool _gamePaused = false;

        //  Indicates that the game has begin after the initial countdown.
        private bool _hasBegun = false;

        //  Flag for when the snake has eaten food.
        private bool _hasEaten = false;

        //  Flag for if the tail should be rendered smoothly.
        private bool _smoothTail = false;

        // ---------------------------------------------------------------------
        //
        //  Directions
        //
        // ---------------------------------------------------------------------
        private readonly Point North = new Point(0, -1);
        private readonly Point South = new Point(0, 1);
        private readonly Point East = new Point(1, 0);
        private readonly Point West = new Point(-1, 0);

        // ---------------------------------------------------------------------
        //
        //  Grid Shenanigans   
        //
        // ---------------------------------------------------------------------

        //  A list of all empty cels in the grid.
        private List<Point> _emptyCells = new();

        //  A hash of all empty cells for fast lookup.
        private HashSet<Point> _emptyCellsHash = new();

        //  A list of all filled cells in the grid.
        private List<Point> _filledCells = new();

        //  A hash of all filled cells for fast lookup.
        private HashSet<Point> _filledCellsHash = new();

        //  The total number of columns in the play grid.
        private readonly int _columns = Engine.Graphics.TileCount.X;

        //  The total number of rows in the play grid.
        private readonly int _rows = 20;

        //  The player's score.
        private int _score;

        // ---------------------------------------------------------------------
        //
        //  Game Objects
        //
        // ---------------------------------------------------------------------

        //  Contains reference values of all snake body pieces in index order of
        //  head to tail.
        private List<BodyPiece> _snake;

        //  Collection of locations for all food.
        private List<Point> _food = new();

        //  Collection of locations for all walls
        private List<Point> _walls = new();

        //  The next direction the snake head will travel in after a tick has occured.
        private Point _nextDirection;

        /// <summary>
        ///     Creates a new <see cref="PlayScene"/> instance.
        /// </summary>
        /// <param name="input">
        ///     The input profile for the game.
        /// </param>
        public PlayScene(InputProfile input) : base(input)
        {
            GameBase.Music.OnBeat += OnBeatHandled;
        }

        /// <summary>
        ///     Initializes the scene.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            //  References to game assets we'll be using to initialize objects
            SpriteFont font32 = GameBase.EssentialGameAssets["font32"] as SpriteFont;
            SpriteFont font128 = GameBase.EssentialGameAssets["font128"] as SpriteFont;
            _gridCellTexture = GameBase.EssentialGameAssets["gridCelTex"] as TinyTexture;


            //  Initialize the timers
            _tickTimer = _timeUntilTick;
            _nextDirection = East;
            _tenSecondTimer = _tenSeconds;

            //  Initialize the score container. 
            _bottomContainer = new Rectangle();
            _bottomContainer.Height = 80;
            _bottomContainer.Width = Engine.Graphics.Resolution.X;
            _bottomContainer.X = 0;
            _bottomContainer.Y = Engine.Graphics.Resolution.Y - _bottomContainer.Height;

            //  Initialize the game over container.
            _gameoverContainer = new Rectangle();
            _gameoverContainer.Height = Engine.Graphics.Resolution.Y / 2;
            _gameoverContainer.Width = Engine.Graphics.Resolution.X / 2;
            _gameoverContainer.X = (Engine.Graphics.Resolution.X / 2) - (_gameoverContainer.Width / 2);
            _gameoverContainer.Y = (Engine.Graphics.Resolution.Y / 2) - (_gameoverContainer.Height / 2);

            //  Initialize the score text.
            _scoreText = new(font32, "Score: ");
            _scoreText.X = _scoreText.Size.Y / 2.0f;
            _scoreText.Y = _bottomContainer.Center.Y;
            _scoreText.CenterLeftOrigin();

            //  Initialize the next wave text
            _nextWaveText = new(font32, "Nex Wave In 10s");
            _nextWaveText.X = Engine.Graphics.Resolution.X - _nextWaveText.Size.X - 30;
            _nextWaveText.Y = _bottomContainer.Center.Y;
            _nextWaveText.CenterLeftOrigin();

            //  Initialize the game over text
            _gameoverText = new(font32, "Game Over");
            _gameoverText.X = (Engine.Graphics.Resolution.X / 2) - (_gameoverText.Size.X / 2);
            _gameoverText.Y = Engine.Graphics.Resolution.Y / 2;
            _gameoverText.CenterLeftOrigin();

            //  Initialize the try again text
            _tryAgainText = new(font32, "Press Enter To Try Again");
            _tryAgainText.X = (Engine.Graphics.Resolution.X / 2) - (_tryAgainText.Size.X / 2);
            _tryAgainText.Y = _bottomContainer.Y + (_bottomContainer.Height / 2);
            _tryAgainText.CenterLeftOrigin();

            //  Initilaize the pause text
            _pauseText = new(font32, "Paused!  ESC to unpause or Enter to restart");
            _pauseText.X = (Engine.Graphics.Resolution.X / 2);
            _pauseText.Y = _bottomContainer.Y + (_bottomContainer.Height / 2);
            _pauseText.CenterOrigin();

            //  Initialize the big countdown text
            _beginCountdownText = new(font128, "3");
            _beginCountdownText.OutlineColor = Color.Black;
            _beginCountdownText.IsOutlined = true;
            _beginCountdownText.X = Engine.Graphics.Resolution.X / 2;
            _beginCountdownText.Y = (Engine.Graphics.Resolution.Y / 2) - _bottomContainer.Height;
            _beginCountdownText.CenterOrigin();


            //  Initialize the grid container
            Point gridSize = new Point(_columns, _rows).Multiply(Engine.Graphics.PixelsPerUnit);

            //  The grid will always be centerd on the screen in the avaialble
            //  play area above the score container. So we need to first define
            //  the measurements of the play area.
            Point playArea = Engine.Graphics.Resolution;
            playArea.Y -= _bottomContainer.Height;

            //  Now get the grid location as the center of the play area
            Point gridLocation = playArea.HalfValue() - gridSize.HalfValue();

            //  And finally create the grid.
            _gridContainer = new Rectangle(gridLocation, gridSize);

            //  Initialize the empty cel collection.
            for (int column = 0; column < _columns; column++)
            {
                for (int row = 0; row < _rows; row++)
                {
                    EmptyCell(new Point(column, row));
                }
            }

            //  Create the snake
            _snake = new List<BodyPiece>();

            //  Snake starts with an initial length of 5
            for (int i = 0; i < 5; i++)
            {
                //  Calcualte the cell the snake body piece will be in
                //  Calcualte the body piece location.  Head starts in
                //  column 10 center row.
                Point cell = new Point(10 - i, (_rows / 2));

                //  Create the snake body piece and add it to the snake reference list.
                _snake.Add(new BodyPiece
                {
                    GridLocation = cell,
                    RenderRect = new Rectangle
                    {
                        X = _gridContainer.X + (cell.X * Engine.Graphics.PixelsPerUnit),
                        Y = _gridContainer.Y + (cell.Y * Engine.Graphics.PixelsPerUnit),
                        Width = Engine.Graphics.PixelsPerUnit,
                        Height = Engine.Graphics.PixelsPerUnit
                    },
                    Direction = _nextDirection
                });

                //  Update the fill cell references
                FillCell(cell);

            }

            //  Spawn the initial food
            SpawnFood();

            //  Set the initial scrore to 0
            IncreaseScore(0);
        }

        /// <summary>
        ///     Called when the scene first starts but before it begins.
        /// </summary>
        public override void Start()
        {
            //  When the scene starts, we want to freeze the background
            //  so that it doesn't start moving until the countdown timer
            //  has finished.
            GameAs<GameBase>().FreezeBackground = false;
        }

        /// <summary>
        ///     Event handler for when a beat occurs.
        /// </summary>
        private void OnBeatHandled(object sender, BeatEventArgs e)
        {
            //  On beat, reset the pulse timer.
            _pulseTimer = 0.0f;
        }



        // ---------------------------------------------------------------------
        //
        //  UPDATES
        //
        // ---------------------------------------------------------------------

        /// <summary>
        ///     Updates the scene
        /// </summary>
        public override void Update()
        {
            //  Alwoas update base first.
            base.Update();

            //  If the scene is paused, just return back. The scene pause is set
            //  by the base class and is set to false once the scene has
            //  finished transitioning in.
            if (ScenePaused) { return; }

            //  Check if the begin countdown has finished.  
            if (!_hasBegun)
            {
                UpdateCountdown();
            }
            else
            {
                UpdateCore();
            }
        }

        /// <summary>
        ///     Performs the update logic when the begin contdown is still
        ///     counting down.
        /// </summary>
        private void UpdateCountdown()
        {
            //  Decrement the timer
            _beginTime -= Engine.Time.ElapsedGameTime;

            //  Update the countdown text
            _beginCountdownText.Value = $"{_beginTime.Seconds + 1}";

            //  If the countdown is now finished, flag that is has
            if (_beginTime < TimeSpan.Zero)
            {
                _hasBegun = true;
            }
        }

        /// <summary>
        ///     Performs the core update logic after the begin countdown has
        ///     finished.
        /// </summary>
        private void UpdateCore()
        {
            //  Check if a game over has occured
            if (_gameover)
            {
                UpdateGameOver();
            }
            else
            {
                UpdateGameplay();
            }
        }

        /// <summary>
        ///     Performs the update logic when a game over has occured.
        /// </summary>
        private void UpdateGameOver()
        {
            //  If the user presses the retry button, then change to a new 
            //  instance of this scene.
            if (_input.Retry.Pressed)
            {
                Retry();;
            }
        }

        /// <summary>
        ///     Performs the update logic when the player is playing and there
        ///     is no game over.
        /// </summary>
        private void UpdateGameplay()
        {
            //  Increment the pulse timer
            _pulseTimer += Engine.Time.DeltaTime;

            //  Check if the game is paused.
            if (_gamePaused)
            {
                UpdateGameplayPaused();
            }
            else if(_input.Pause.Pressed)
            {
                _gamePaused = true;
            }
            else
            {
                //  Update the snakes direction based on the user input.
                UpdateDirectionBasedOnInput();

                //  Update the tick timer
                UpdateTickTimer();

                //  Update the head and tail rects draw percentages so that they
                //  appear to be drawing smoothly each frame as they move
                //  instead of the traditional snake jerky movement.
                float percentage = 1.0f - (float)(_tickTimer / _timeUntilTick);
                SmoothHeadRect(percentage);
                SmoothTailRect(percentage);

                //  Decrement the 10 second timer and execute if below zero
                _tenSecondTimer -= Engine.Time.ElapsedGameTime;
                _nextWaveText.Value = $"Next Wave in {_tenSecondTimer.Seconds + 1}s";

                if (_tenSecondTimer <= TimeSpan.Zero)
                {
                    _tenSecondTimer = _tenSeconds;
                    EveryTenSeconds();
                }
            }
        }

        /// <summary>
        ///     Performs the update logic when the gameplay is paused.
        /// </summary>
        private void UpdateGameplayPaused()
        {
            //  While the game is paused, the player can press the
            //  unpause button or they can press the retry button to
            //  restart the game.
            if (_input.Pause.Pressed)
            {
                _gamePaused = false;
            }
            else if (_input.Retry.Pressed)
            {
                Retry();
            }
        }

        /// <summary>
        ///     Updates the direction the snake is moving based on the player
        ///     input.
        /// </summary>
        private void UpdateDirectionBasedOnInput()
        {
            //  Get reference to the current direction the head is moving in
            Point currentDirection = _snake[0].Direction;

            if (currentDirection.Y == 0)
            {
                if (_input.MoveUp.Pressed)
                {
                    _nextDirection = new Point(0, -1);
                }
                else if (_input.MoveDown.Pressed)
                {
                    _nextDirection = new Point(0, 1);
                }
            }

            if (currentDirection.X == 0)
            {
                if (_input.MoveLeft.Pressed)
                {
                    _nextDirection = new Point(-1, 0);
                }
                else if (_input.MoveRight.Pressed)
                {
                    _nextDirection = new Point(1, 0);
                }
            }
        }

        /// <summary>
        ///     Performs the logic to update the underlying tick timer and 
        ///     performs a tick if the timer reaches zero.
        /// </summary>
        private void UpdateTickTimer()
        {
            //  Decrement the tick timer
            _tickTimer -= Engine.Time.ElapsedGameTime;

            //  If the timer is below zero, perform tick
            if (_tickTimer <= TimeSpan.Zero)
            {
                //  Reset the timer
                _tickTimer = _timeUntilTick;

                //  Perform tick
                Tick();
            }
        }

        /// <summary>
        ///     Performs the logic when a tick occurs. This is where the snake
        ///     moves, snake is updated, and collision is checked.
        /// </summary>
        private void Tick()
        {
            if (!_smoothTail)
            {
                _smoothTail = true;
            }

            //  We need to cache the value of the snake's head into two seperate
            //  instance. One will be a reference to what the head value was, 
            //  the other we'll use to update the head values
            BodyPiece previousHead = _snake[0];
            BodyPiece head = _snake[0];

            //  Check here for collision with food before doing the movement
            //  of the head and tail. We'll check head-to-body collision after
            //  movement.
            for (int i = _food.Count - 1; i >= 0; i--)
            {
                if (head.GridLocation == _food[i])
                {
                    _hasEaten = true;
                    _food.RemoveAt(i);
                    _smoothTail = false;
                    break;
                }
            }

            //  Check wall collision
            for (int i = 0; i < _walls.Count; i++)
            {
                if (head.GridLocation == _walls[i])
                {
                    GameOver();
                }
            }

            //  Set the current direction of the head body piece equal to the
            //  next direction value. We also set the value of the previous
            //  head direction so when it eventually becomes the tail piece, 
            //  it can smooth render in the correct direction.
            previousHead.Direction = head.Direction = _nextDirection;
            _bgOffsetDirection = _nextDirection;

            //  We need to reset the render rect of the previous head value due
            //  to an issue with it sometimes only being partially corrct due to
            //  the percentage calculations during the update part (yay floats)
            previousHead.RenderRect.Location = _gridContainer.Location + (previousHead.GridLocation.Multiply(Engine.Graphics.PixelsPerUnit));
            previousHead.RenderRect.Size = new Point(Engine.Graphics.PixelsPerUnit, Engine.Graphics.PixelsPerUnit);

            //  Now, remove the head from it's grid location
            EmptyCell(head.GridLocation);

            //  Move the head in the direction
            head.GridLocation += head.Direction;

            //  Check if the head should wrap around the grid
            if (head.GridLocation.X >= _columns)
            {
                head.GridLocation.X = 0;
            }
            else if (head.GridLocation.X < 0)
            {
                head.GridLocation.X = _columns - 1;
            }
            else if (head.GridLocation.Y >= _rows)
            {
                head.GridLocation.Y = 0;
            }
            else if (head.GridLocation.Y < 0)
            {
                head.GridLocation.Y = _rows - 1;
            }

            //  Update the render rect for the head
            head.RenderRect.X = _gridContainer.X + (head.GridLocation.X * Engine.Graphics.PixelsPerUnit);
            head.RenderRect.Y = _gridContainer.Y + (head.GridLocation.Y * Engine.Graphics.PixelsPerUnit);
            head.RenderRect.Width = Engine.Graphics.PixelsPerUnit;
            head.RenderRect.Height = Engine.Graphics.PixelsPerUnit;

            //  Update the local reference to the head
            _snake[0] = head;

            if (_hasEaten)
            {
                //  If we've eaten, then the new body piece that is created is 
                //  the same as what the previous piece was
                _hasEaten = false;
                _snake.Insert(1, previousHead);
                FillCell(previousHead.GridLocation);

                //  Increase the score by 100 points.
                IncreaseScore(100);
                GameBase.Music.PlaySoundEffect(SoundEffectName.Good);
            }
            else
            {
                //  If we didn't eat, then we need to move the tail to the 
                //  previous head position.

                //  Remove the tail
                EmptyCell(_snake[^1].GridLocation);
                _snake.RemoveAt(_snake.Count - 1);

                //  Reinsert tail
                _snake.Insert(1, previousHead);
                FillCell(previousHead.GridLocation);
            }

            //  Now that we've moved the head, we need to fill the cell its in
            //  now
            FillCell(head.GridLocation);


            //  Check head-to-body collision here. By doing it at the end, we
            //  prevent weird issues where collision occurs even though visually
            //  it doesn't look like it should have
            for (int i = 1; i < _snake.Count; i++)
            {
                if (head.GridLocation == _snake[i].GridLocation)
                {
                    //  Snake head collided with a body piece, so this is game 
                    //  over
                    GameOver();

                }
            }
        }

        /// <summary>
        ///     Smooths the size of the rect for the snake head based on the
        ///     percentage given.
        /// </summary>
        /// <param name="percentage">
        ///     The percentage to full rect.
        /// </param>
        private void SmoothHeadRect(float percentage)
        {
            //  Get reference to head
            //Point head = _snake[0];
            BodyPiece head = _snake[0];

            if (head.Direction == North)
            {
                head.RenderRect.Width = Engine.Graphics.PixelsPerUnit;
                head.RenderRect.Height = (int)Math.Ceiling(Engine.Graphics.PixelsPerUnit * percentage);
                head.RenderRect.X = _gridContainer.X + (head.GridLocation.X * Engine.Graphics.PixelsPerUnit);
                head.RenderRect.Y = _gridContainer.Y + (head.GridLocation.Y * Engine.Graphics.PixelsPerUnit) + (Engine.Graphics.PixelsPerUnit - head.RenderRect.Height);
            }
            else if (head.Direction == South)
            {
                head.RenderRect.Width = Engine.Graphics.PixelsPerUnit;
                head.RenderRect.Height = (int)Math.Ceiling(Engine.Graphics.PixelsPerUnit * percentage);
                head.RenderRect.X = _gridContainer.X + (head.GridLocation.X * Engine.Graphics.PixelsPerUnit);
                head.RenderRect.Y = _gridContainer.Y + (head.GridLocation.Y * Engine.Graphics.PixelsPerUnit);
            }
            else if (head.Direction == East)
            {
                head.RenderRect.Width = (int)Math.Ceiling(Engine.Graphics.PixelsPerUnit * percentage);
                head.RenderRect.Height = Engine.Graphics.PixelsPerUnit;
                head.RenderRect.X = _gridContainer.X + (head.GridLocation.X * Engine.Graphics.PixelsPerUnit);
                head.RenderRect.Y = _gridContainer.Y + (head.GridLocation.Y * Engine.Graphics.PixelsPerUnit);
            }
            else if (head.Direction == West)
            {
                head.RenderRect.Width = (int)Math.Ceiling(Engine.Graphics.PixelsPerUnit * percentage);
                head.RenderRect.Height = Engine.Graphics.PixelsPerUnit;
                head.RenderRect.X = _gridContainer.X + (head.GridLocation.X * Engine.Graphics.PixelsPerUnit) + (Engine.Graphics.PixelsPerUnit - head.RenderRect.Width);
                head.RenderRect.Y = _gridContainer.Y + (head.GridLocation.Y * Engine.Graphics.PixelsPerUnit);
            }

            _snake[0] = head;
        }

        /// <summary>
        ///     Smooths the size of the rect for the snake tail based on the
        ///     percentage given.
        /// </summary>
        /// <param name="percentage">
        ///     The percentage to full rect.
        /// </param>
        private void SmoothTailRect(float percentage)
        {
            if (!_smoothTail)
            {
                percentage = 1.0f;
            }
            //  Get reference to tail
            BodyPiece tail = _snake[^1];

            //Point tail = _snake[^1];

            if (tail.Direction == North)
            {
                tail.RenderRect.Width = Engine.Graphics.PixelsPerUnit;
                tail.RenderRect.Height = (int)Math.Ceiling(Engine.Graphics.PixelsPerUnit * (1.0f - percentage));
                tail.RenderRect.X = _gridContainer.X + (tail.GridLocation.X * Engine.Graphics.PixelsPerUnit);
                tail.RenderRect.Y = _gridContainer.Y + (tail.GridLocation.Y * Engine.Graphics.PixelsPerUnit);

            }
            else if (tail.Direction == South)
            {
                tail.RenderRect.Width = Engine.Graphics.PixelsPerUnit;
                tail.RenderRect.Height = (int)Math.Ceiling(Engine.Graphics.PixelsPerUnit * (1.0f - percentage));
                tail.RenderRect.X = _gridContainer.X + (tail.GridLocation.X * Engine.Graphics.PixelsPerUnit);
                tail.RenderRect.Y = _gridContainer.Y + (tail.GridLocation.Y * Engine.Graphics.PixelsPerUnit) + (Engine.Graphics.PixelsPerUnit - tail.RenderRect.Height);
            }
            else if (tail.Direction == East)
            {
                tail.RenderRect.Width = (int)Math.Ceiling(Engine.Graphics.PixelsPerUnit * (1.0f - percentage));
                tail.RenderRect.Height = Engine.Graphics.PixelsPerUnit;
                tail.RenderRect.X = _gridContainer.X + (tail.GridLocation.X * Engine.Graphics.PixelsPerUnit) + (Engine.Graphics.PixelsPerUnit - tail.RenderRect.Width);
                tail.RenderRect.Y = _gridContainer.Y + (tail.GridLocation.Y * Engine.Graphics.PixelsPerUnit);
            }
            else if (tail.Direction == West)
            {
                tail.RenderRect.Width = (int)Math.Ceiling(Engine.Graphics.PixelsPerUnit * (1.0f - percentage));
                tail.RenderRect.Height = Engine.Graphics.PixelsPerUnit;
                tail.RenderRect.X = _gridContainer.X + (tail.GridLocation.X * Engine.Graphics.PixelsPerUnit);
                tail.RenderRect.Y = _gridContainer.Y + (tail.GridLocation.Y * Engine.Graphics.PixelsPerUnit);
            }



            _snake[^1] = tail;
        }

        /// <summary>
        ///     Performs the logic that occurs ever ten seconds.
        /// </summary>
        private void EveryTenSeconds()
        {
            //  Remove all food from grid
            for (int i = 0; i < _food.Count; i++)
            {
                EmptyCell(_food[i]);
            }

            //  Remove all walls from grid
            for (int i = 0; i < _walls.Count; i++)
            {
                EmptyCell(_walls[i]);
            }

            //  Add all food remaining to the walls
            _walls.AddRange(_food);

            //  Clear out the food collection
            _food.Clear();

            //  Add all walls back to the grid
            for (int i = 0; i < _walls.Count; i++)
            {
                FillCell(_walls[i]);
            }

            //  Spawn 10 more food
            for (int i = 0; i < 10; i++)
            {
                Point cell = Maths.Random.Next<Point>(_emptyCells);
                _food.Add(cell);
                FillCell(cell);
            }
        }

        // ---------------------------------------------------------------------
        //
        //  DRAWING
        //
        // ---------------------------------------------------------------------

        /// <summary>
        ///     Draws the scene.
        /// </summary>
        public override void Draw()
        {
            //  Prepare the graphics for rendering
            Engine.Instance.GraphicsDevice.SetRenderTarget(RenderTarget);
            Engine.Instance.GraphicsDevice.Viewport = new Viewport(RenderTarget.Bounds);
            Engine.Instance.GraphicsDevice.Clear(Engine.Graphics.ClearColor);

            //  Draw calls in order.
            DrawBackground();
            DrawGrid();
            DrawObjects();
            DrawHud();

            base.Draw();

            //  Alwoys derefernce the scenes render target when finished.
            Engine.Instance.GraphicsDevice.SetRenderTarget(null);
        }

        /// <summary>
        ///     Draws the grid area.
        /// </summary>
        private void DrawGrid()
        {
            int shadowThick = 2;
            int halfShadowThick = 1;
            float alpha = 0.7f;

            Engine.Graphics.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null, null, null);

            //  Draw the grid container
            Engine.Graphics.SpriteBatch.DrawLine(_gridContainer.Right + halfShadowThick,
                                 _gridContainer.Top + shadowThick,
                                 _gridContainer.Right + halfShadowThick,
                                 _gridContainer.Bottom + shadowThick,
                                 Color.Black * alpha, shadowThick);
            Engine.Graphics.SpriteBatch.DrawLine(_gridContainer.Left + shadowThick,
                                 _gridContainer.Bottom + halfShadowThick,
                                 _gridContainer.Right + shadowThick,
                                 _gridContainer.Bottom + halfShadowThick
                                 , Color.Black * alpha, shadowThick);
            Engine.Graphics.SpriteBatch.DrawRectangle(_gridContainer, Color.Black * 0.9f);

            //  Draw the grid cel texture on top of the container.  This is
            //  wrapped.
            Engine.Graphics.SpriteBatch.Draw(_gridCellTexture.Texture, _gridContainer, new Rectangle(Point.Zero, _gridContainer.Size), Color.White * 0.5f);

            //  Draw the outline of the grid container.
            Engine.Graphics.SpriteBatch.DrawHollowRectangle(_gridContainer, Color.White);

            Engine.Graphics.SpriteBatch.End();
        }

        /// <summary>
        ///     Draws the food, wall, and snake objects
        /// </summary>
        private void DrawObjects()
        {
            Engine.Graphics.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            DrawFood();
            DrawWalls();
            DrawSnake();

            Engine.Graphics.SpriteBatch.End();
        }

        /// <summary>
        ///     Draws the food objects
        /// </summary>
        private void DrawFood()
        {
            //  Reusuable rect for each food object.
            Rectangle rect = Rectangle.Empty;

            //  Go food-by-food and draw them.
            for (int i = 0; i < _food.Count; i++)
            {
                //  The food values are stored as the column and row in the grid
                //  so we need to multiply each food's position by the 
                //  pixels per unit.
                rect.Location = _gridContainer.Location + _food[i].Multiply(Engine.Graphics.PixelsPerUnit);

                //  Set the size of the food to one graphical unit
                rect.Size = new Point(Engine.Graphics.PixelsPerUnit, Engine.Graphics.PixelsPerUnit);

                //  The food objects pulse to the beat of the music, so we need
                //  to scale them based on the beat by a factor of 4.
                rect = ScaleRectByBeat(rect, 4);

                //  Draw it.
                Engine.Graphics.SpriteBatch.DrawRectangle(rect, Color.Yellow);
            }
        }

        /// <summary>
        ///     Draws each wall object.
        /// </summary>
        private void DrawWalls()
        {
            //  Reusuable rect for each wall object.
            Rectangle rect = Rectangle.Empty;

            //  Unlike the food, the size of the walls are not changed based
            //  on the beat of the music.  So we an set the size once here
            //  for all food instead of for each food in the loop.
            rect.Size = new Point(Engine.Graphics.PixelsPerUnit, Engine.Graphics.PixelsPerUnit);


            //  Go wall-by-wall and draw them.
            for (int i = 0; i < _walls.Count; i++)
            {
                //  The wall values are stored as the column and row in the grid
                //  so we need to multiply each wall's position by the 
                //  pixels per unit.
                rect.Location = _gridContainer.Location + _walls[i].Multiply(Engine.Graphics.PixelsPerUnit);

                //  Draw the food
                Engine.Graphics.SpriteBatch.DrawRectangle(rect, Color.Gray);

                //  Draw an outline around the wall to make it easier to see
                Engine.Graphics.SpriteBatch.DrawHollowRectangle(rect, Color.White);
            }
        }

        /// <summary>
        ///     Draws the snake.
        /// </summary>
        private void DrawSnake()
        {
            for (int i = 0; i < _snake.Count; i++)
            {
                Engine.Graphics.SpriteBatch.DrawRectangle(_snake[i].RenderRect, Color.White);
            }
        }

        /// <summary>
        ///     Draws the HUD elements
        /// </summary>
        private void DrawHud()
        {
            Engine.Graphics.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            //  Draw the bottom container always.
            Engine.Graphics.SpriteBatch.DrawRectangle(_bottomContainer, Color.Black * 0.8f);
            Engine.Graphics.SpriteBatch.DrawHollowRectangle(_bottomContainer, Color.White);

            //  Check if countdown timer has finished.
            if (!_hasBegun)
            {
                //  Countdown hasn't finished, draw the grid container either
                //  way
                Engine.Graphics.SpriteBatch.DrawRectangle(_gridContainer, Color.Black * 0.4f);

                //  Only draw the actual countdown text if the scene transition
                //  has completed.
                if (!ScenePaused)
                {
                    Engine.Graphics.SpriteBatch.DrawString(_beginCountdownText);
                }
            }
            else
            {
                //  Is there a game over?
                if (_gameover)
                {
                    //  Draw the game over container
                    Engine.Graphics.SpriteBatch.DrawRectangle(_gameoverContainer, Color.Black * 0.8f);
                    Engine.Graphics.SpriteBatch.DrawHollowRectangle(_gameoverContainer, Color.White);

                    //  Draw the game over text
                    Engine.Graphics.SpriteBatch.DrawString(_gameoverText);

                    //  Draw the scroe text
                    Engine.Graphics.SpriteBatch.DrawString(_scoreText);

                    //  Draw the try again text.
                    Engine.Graphics.SpriteBatch.DrawString(_tryAgainText);
                }
                else if (_gamePaused)
                {
                    //  Not game over, but bame is paused. Draw the pause text.
                    Engine.Graphics.SpriteBatch.DrawString(_pauseText);
                }
                else
                {
                    //  Not game over, and game is not paused
                    //  Draw the sctore text.
                    Engine.Graphics.SpriteBatch.DrawString(_scoreText);

                    //  Draw the next wave text.
                    Engine.Graphics.SpriteBatch.DrawString(_nextWaveText);

                }
            }

            Engine.Graphics.SpriteBatch.End();
        }

        /// <summary>
        ///     Scales the <paramref name="rect"/> by the
        ///     <paramref name="factor"/> given based on the beat of the music.
        /// </summary>
        /// <param name="rect">
        ///     the rectangle value to scale.
        /// </param>
        /// <param name="factor">
        ///     The factor to scale by.
        /// </param>
        /// <returns>
        ///     A new rectangle value.
        /// </returns>
        private Rectangle ScaleRectByBeat(Rectangle rect, int factor)
        {
            int from = Engine.Graphics.PixelsPerUnit;
            int to = Engine.Graphics.PixelsPerUnit - factor;
            float per = _pulseTimer / _pulseDuration;
            int lerp = (int)Math.Ceiling(MathHelper.Lerp(from, to, per));

            Rectangle result;
            result.X = rect.X + lerp;
            result.Y = rect.Y + lerp;
            result.Width = rect.Width - (lerp * 2);
            result.Height = rect.Height - (lerp * 2);

            return result;
        }

        // ---------------------------------------------------------------------
        //
        //  UTILITY METHODS
        //
        // ---------------------------------------------------------------------

        /// <summary>
        ///     Performs logic when a game over occurs.
        /// </summary>
        private void GameOver()
        {
            //  Flag that the game over happend.
            _gameover = true;

            GameBase.Music.PlaySoundEffect(SoundEffectName.Bad);

            //  Move the scroe text so that it will appear inside the game over
            //  container.
            _scoreText.X = (Engine.Graphics.Resolution.X / 2) - (_scoreText.Size.X / 2);
            _scoreText.Y = _gameoverContainer.Bottom - _scoreText.Size.Y;
        }

        /// <summary>
        ///     Spanws new food on the grid.
        /// </summary>
        private void SpawnFood()
        {
            //  CLear out the remaining food.
            _food.Clear();

            //  Spawn 10 more food.
            for (int i = 0; i < 10; i++)
            {
                //  Choose a random cell from the empty cells.
                Point cell = Maths.Random.Next<Point>(_emptyCells);

                //  Add to collection
                _food.Add(cell);

                //  Fill the cell.
                FillCell(cell);
            }
        }

        /// <summary>
        ///     Emptys the cell give.
        /// </summary>
        /// <param name="cell">
        ///     The cell to empty
        /// </param>
        private void EmptyCell(Point cell)
        {
            //  Check if the cell is already empty or not.  We can't empty a
            //  cel that is already empty, this should never occur.
            if (!_emptyCellsHash.Contains(cell))
            {
                _emptyCells.Add(cell);
                _emptyCellsHash.Add(cell);
            }
            else
            {
                throw new Exception("Told to empty cell, but cell already empty");
            }

            //  Remove the cell from the filled collection.
            if (_filledCellsHash.Contains(cell))
            {
                _filledCells.Remove(cell);
                _filledCellsHash.Remove(cell);
            }
        }

        /// <summary>
        ///     Fills the cell given.
        /// </summary>
        /// <param name="cell"></param>
        private void FillCell(Point cell)
        {
            //  Remove the cell from the empty collection
            if (_emptyCellsHash.Contains(cell))
            {
                _emptyCells.Remove(cell);
                _emptyCellsHash.Remove(cell);
            }

            //  Add the cell to the filled collection.
            if (!_filledCellsHash.Contains(cell))
            {
                _filledCells.Add(cell);
                _filledCellsHash.Add(cell);
            }
        }

        /// <summary>
        ///     Increases the player sctore by the amount given.
        /// </summary>
        /// <param name="increaseBy">
        ///     The amount to increase the score by.
        /// </param>
        private void IncreaseScore(int increaseBy)
        {
            _score += increaseBy;
            _scoreText.Value = $"Score: {_score:000000000}";
        }

        /// <summary>
        ///     Performs the logic when the player requests to retry the level.
        /// </summary>
        private void Retry()
        {
            //  When a retry occurs, we just change the scene to a new instance
            // of this play scene
            ChangeScene(new PlayScene(_input),
                new EvenOddTileTransition(Engine.Graphics.Resolution.X / 4, TimeSpan.FromSeconds(1)),
                new EvenOddTileTransition(Engine.Graphics.Resolution.X / 4, TimeSpan.FromSeconds(1)));
        }
    }
}
