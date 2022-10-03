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
        //  The amount of time to elapse before a tick occurs.
        private readonly TimeSpan _timeUntilTick = TimeSpan.FromSeconds(0.1);

        //  A 10 second timespan used for comparison.
        private readonly TimeSpan _tenSeconds = TimeSpan.FromSeconds(10);

        private TimeSpan _beginTime = TimeSpan.FromSeconds(3);

        //  Easy to use for comparison directions.
        private readonly Point North = new Point(0, -1);
        private readonly Point South = new Point(0, 1);
        private readonly Point East = new Point(1, 0);
        private readonly Point West = new Point(-1, 0);

        private bool _gameover = false;
        private bool _gamePaused = false;
        private bool _hasBegun = false;

        //  HUD Elements
        private Text _gameoverText;
        private Text _tryAgainText;
        private Text _scoreText;
        private Text _timerText;
        private Text _beginText;
        private Text _pauseText;





        //  Used to track the every ten seconds event.
        private TimeSpan _tenSecondTimer;

        //  Flag used to block Events processing in the middle of a draw call.
        private bool _isDrawing;

        //  The timer used when a pulse/beat occurs from the music.
        private float _pulseTimer = 0.0f;

        //  The duration of a pulse. Should always match the crochet.
        private float _pulseDuration = GameBase.Music.Crochet;

        //  Font used to render text to the scene.
        private SpriteFont _font;

        //  A list of all empty cels in the grid.
        private List<Point> _emptyCells;

        //  A hash of all empty cells for fast lookup.
        private HashSet<Point> _emptyCellsHash;

        //  A list of all filled cells in the grid.
        private List<Point> _filledCells;

        //  A hash of all filled cells for fast lookup.
        private HashSet<Point> _filledCellsHash;

        //  Flag for when the snake has eaten food.
        private bool _hasEaten;

        //  Flag for if the tail should be rendered smoothly. Used for debug.
        private bool _smoothTail;

        //  The total number of columns in the play grid.
        private int _columns;

        //  The total number of rows in the play grid.
        private int _rows;

        //  The player's score.
        private int _score;


        //  The visual container that is drawn to the screen and contains the
        //  score.
        private Rectangle _scoreContainer;

        //  Defines the values used for a body piece of a snake.
        private struct BodyPiece
        {
            public Point GridLocation;
            public Rectangle RenderRect;
            public Point Direction;
        }


        //  The next direction the snake head will travel in after a tick has occured.
        private Point _nextDirection;

        //  The texture to use when rendering a grid cell that's empty.
        private TinyTexture _gridCellTexture;

        //  A rectangle that defines the boundry of the grid area.
        private Rectangle _gridRect;

        //  Tracks the amount of time remaining for a tick to occur.
        private TimeSpan _tickTimer;

        //  Contains reference values of all snake body pieces in index order of
        //  head to tail.
        private List<BodyPiece> _snake;

        //  The point location of the food piece.
        // private Point _food;

        private List<Point> _food = new();
        private List<Point> _walls = new();

        public PlayScene(InputProfile input) : base(input)
        {
            _hasEaten = false;
            _smoothTail = false;
            _columns = Engine.Graphics.TileCount.X;
            _rows = 20;

            _emptyCells = new List<Point>();
            _emptyCellsHash = new HashSet<Point>();
            _filledCells = new List<Point>();
            _filledCellsHash = new HashSet<Point>();

            _tickTimer = _timeUntilTick;
            _nextDirection = East;
            _tenSecondTimer = _tenSeconds;

            GameBase.Music.OnBeat += OnBeatHandled;

        }

        /// <summary>
        ///     Event handler for when a beat occurs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBeatHandled(object sender, BeatEventArgs e)
        {
            if (!_isDrawing)
            {
                _pulseTimer = 0.0f;
            }
        }

        /// <summary>
        ///     Called when the scene first starts but before it begins.
        /// </summary>
        public override void Start()
        {
            GameAs<GameBase>().FreezeBackground = false;
        }


        private Rectangle _gameoverContainer;
        private Rectangle _beginTimeContainer;

        /// <summary>
        ///     Initializes the scene.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            //  Initialize the score container first. 
            _scoreContainer = new Rectangle();
            _scoreContainer.Height = 80; /* (int)(Math.Ceiling(_scoreText.Size.Y * 2));*/
            _scoreContainer.Width = Engine.Graphics.Resolution.X;
            _scoreContainer.X = 0;
            _scoreContainer.Y = Engine.Graphics.Resolution.Y - _scoreContainer.Height;

            //  Initialize the score text.
            _scoreText.X = _scoreText.Size.Y / 2.0f;
            _scoreText.Y = _scoreContainer.Center.Y;
            _scoreText.CenterLeftOrigin();

            _timerText.X = Engine.Graphics.Resolution.X - _timerText.Size.X - 20;
            _timerText.Y = _scoreContainer.Center.Y;
            _timerText.CenterLeftOrigin();


            _gameoverContainer = new Rectangle();
            _gameoverContainer.Height = Engine.Graphics.Resolution.Y / 2;
            _gameoverContainer.Width = Engine.Graphics.Resolution.X / 2;
            _gameoverContainer.X = (Engine.Graphics.Resolution.X / 2) - (_gameoverContainer.Width / 2);
            _gameoverContainer.Y = (Engine.Graphics.Resolution.Y / 2) - (_gameoverContainer.Height / 2);

            _gameoverText.X = (Engine.Graphics.Resolution.X / 2) - (_gameoverText.Size.X / 2);
            _gameoverText.Y = Engine.Graphics.Resolution.Y / 2;
            _gameoverText.CenterLeftOrigin();

            _tryAgainText.X = (Engine.Graphics.Resolution.X / 2) - (_tryAgainText.Size.X / 2);
            _tryAgainText.Y = _scoreContainer.Y + (_scoreContainer.Height / 2);
            _tryAgainText.CenterLeftOrigin();

            _pauseText.X = (Engine.Graphics.Resolution.X / 2);
            _pauseText.Y = _scoreContainer.Y + (_scoreContainer.Height / 2);
            _pauseText.CenterOrigin();








            //  Initialize the size of the grid.
            Point gridSize = new Point(_columns, _rows).Multiply(Engine.Graphics.PixelsPerUnit);

            //  The grid will always be centerd on the screen in the avaialble play area above the
            //  score container. So we need to first define the measurements of the play area.
            Point playArea = Engine.Graphics.Resolution;
            playArea.Y -= _scoreContainer.Height;

            //  Now get the grid location as the center of the play area
            Point gridLocation = playArea.HalfValue() - gridSize.HalfValue();

            //  And finally create the grid.
            _gridRect = new Rectangle(gridLocation, gridSize);


            _beginText.Position = _gridRect.Center.ToVector2();
            _beginText.CenterLeftOrigin();



            //  Fill the initial empty cell list
            for (int column = 0; column < _columns; column++)
            {
                for (int row = 0; row < _rows; row++)
                {
                    EmptyCell(new Point(column, row));
                }
            }

            //  Create the snake
            _snake = new List<BodyPiece>();

            for (int i = 0; i < 5; i++)
            {
                //  Calcualte the cell the snake body piece will be in
                Point cell = new Point((_columns / 2) - i, (_rows / 2));

                //  Create the snake body piece and add it to the snake reference list.
                _snake.Add(new BodyPiece
                {
                    GridLocation = cell,
                    RenderRect = new Rectangle
                    {
                        X = _gridRect.X + (cell.X * Engine.Graphics.PixelsPerUnit),
                        Y = _gridRect.Y + (cell.Y * Engine.Graphics.PixelsPerUnit),
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
        }

        /// <summary>
        ///     Loads the assets for the scene.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            _gridCellTexture = new(Engine.GlobalContent.Load<Texture2D>("grid_cell"));
            _font = Engine.GlobalContent.Load<SpriteFont>("RussoOne64");
            SpriteFont bigFont = Engine.GlobalContent.Load<SpriteFont>("RussoOne128");
            _scoreText = new(_font, $"Score: ");
            _timerText = new(_font, $"Next Wave 10s");
            _gameoverText = new(_font, "Game Over");
            _tryAgainText = new(_font, "Press Enter To Try Again");
            _beginText = new(bigFont, "3");
            _beginText.OutlineColor = Color.Black;
            _beginText.IsOutlined = true;
            _pauseText = new(_font, "Paused! ESC to unpause or Enter to restart");


            IncreaseScore(0);
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
            base.Update();

            if (ScenePaused) { return; }

            if (!_hasBegun)
            {
                _beginTime -= Engine.Time.ElapsedGameTime;
                _beginText.Value = $"{_beginTime.Seconds + 1}";

                if (_beginTime < TimeSpan.Zero)
                {
                    _hasBegun = true;
                }
            }
            else
            {

                if (_gameover)
                {
                    UpdateGameOver();
                }
                else
                {
                    UpdateGameplay();
                }
            }
        }

        private void UpdateGameOver()
        {
            if (_input.AnyButtonPressed)
            {
                ChangeScene(new PlayScene(_input),
                new EvenOddTileTransition(Engine.Graphics.Resolution.X / 4, TimeSpan.FromSeconds(1)),
                new EvenOddTileTransition(Engine.Graphics.Resolution.X / 4, TimeSpan.FromSeconds(1)));
            }
        }

        private void UpdateGameplay()
        {
            //  Increment the pulse timer
            _pulseTimer += Engine.Time.DeltaTime;

            if (!_gamePaused && _input.Pause.Pressed)
            {
                _gamePaused = true;
            }
            else if (_gamePaused && _input.Pause.Pressed)
            {
                _gamePaused = false;
            }
            else if(_gamePaused && _input.Retry.Pressed)
            {
                ChangeScene(new PlayScene(_input),
                new EvenOddTileTransition(Engine.Graphics.Resolution.X / 4, TimeSpan.FromSeconds(1)),
                new EvenOddTileTransition(Engine.Graphics.Resolution.X / 4, TimeSpan.FromSeconds(1)));
            }

            if (!_gamePaused)
            {
                UpdateInput();

                //  Decrement the tick timer and execute tick if below zero
                _tickTimer -= Engine.Time.ElapsedGameTime;

                if (_tickTimer <= TimeSpan.Zero)
                {
                    _tickTimer = _timeUntilTick;
                    Tick();
                }

                //  Update the head and tail rects draw percentages.
                float percentage = 1.0f - (float)(_tickTimer / _timeUntilTick);
                UpdateHeadRect(percentage);
                UpdateTailRect(percentage);

                //  Decrement the 10 second timer and execute if below zero
                _tenSecondTimer -= Engine.Time.ElapsedGameTime;
                _timerText.Value = $"Next Wave: {_tenSecondTimer.Seconds + 1}s";

                if (_tenSecondTimer <= TimeSpan.Zero)
                {
                    _tenSecondTimer = _tenSeconds;
                    EveryTenSeconds();
                }
            }
            else
            {
            }
        }

        private void UpdateInput()
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

        private void UpdateHeadRect(float percentage)
        {
            //  Get reference to head
            //Point head = _snake[0];
            BodyPiece head = _snake[0];

            if (head.Direction == North)
            {
                head.RenderRect.Width = Engine.Graphics.PixelsPerUnit;
                head.RenderRect.Height = (int)Math.Ceiling(Engine.Graphics.PixelsPerUnit * percentage);
                head.RenderRect.X = _gridRect.X + (head.GridLocation.X * Engine.Graphics.PixelsPerUnit);
                head.RenderRect.Y = _gridRect.Y + (head.GridLocation.Y * Engine.Graphics.PixelsPerUnit) + (Engine.Graphics.PixelsPerUnit - head.RenderRect.Height);
            }
            else if (head.Direction == South)
            {
                head.RenderRect.Width = Engine.Graphics.PixelsPerUnit;
                head.RenderRect.Height = (int)Math.Ceiling(Engine.Graphics.PixelsPerUnit * percentage);
                head.RenderRect.X = _gridRect.X + (head.GridLocation.X * Engine.Graphics.PixelsPerUnit);
                head.RenderRect.Y = _gridRect.Y + (head.GridLocation.Y * Engine.Graphics.PixelsPerUnit);
            }
            else if (head.Direction == East)
            {
                head.RenderRect.Width = (int)Math.Ceiling(Engine.Graphics.PixelsPerUnit * percentage);
                head.RenderRect.Height = Engine.Graphics.PixelsPerUnit;
                head.RenderRect.X = _gridRect.X + (head.GridLocation.X * Engine.Graphics.PixelsPerUnit);
                head.RenderRect.Y = _gridRect.Y + (head.GridLocation.Y * Engine.Graphics.PixelsPerUnit);
            }
            else if (head.Direction == West)
            {
                head.RenderRect.Width = (int)Math.Ceiling(Engine.Graphics.PixelsPerUnit * percentage);
                head.RenderRect.Height = Engine.Graphics.PixelsPerUnit;
                head.RenderRect.X = _gridRect.X + (head.GridLocation.X * Engine.Graphics.PixelsPerUnit) + (Engine.Graphics.PixelsPerUnit - head.RenderRect.Width);
                head.RenderRect.Y = _gridRect.Y + (head.GridLocation.Y * Engine.Graphics.PixelsPerUnit);
            }

            _snake[0] = head;
        }

        private void UpdateTailRect(float percentage)
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
                tail.RenderRect.X = _gridRect.X + (tail.GridLocation.X * Engine.Graphics.PixelsPerUnit);
                tail.RenderRect.Y = _gridRect.Y + (tail.GridLocation.Y * Engine.Graphics.PixelsPerUnit);

            }
            else if (tail.Direction == South)
            {
                tail.RenderRect.Width = Engine.Graphics.PixelsPerUnit;
                tail.RenderRect.Height = (int)Math.Ceiling(Engine.Graphics.PixelsPerUnit * (1.0f - percentage));
                tail.RenderRect.X = _gridRect.X + (tail.GridLocation.X * Engine.Graphics.PixelsPerUnit);
                tail.RenderRect.Y = _gridRect.Y + (tail.GridLocation.Y * Engine.Graphics.PixelsPerUnit) + (Engine.Graphics.PixelsPerUnit - tail.RenderRect.Height);
            }
            else if (tail.Direction == East)
            {
                tail.RenderRect.Width = (int)Math.Ceiling(Engine.Graphics.PixelsPerUnit * (1.0f - percentage));
                tail.RenderRect.Height = Engine.Graphics.PixelsPerUnit;
                tail.RenderRect.X = _gridRect.X + (tail.GridLocation.X * Engine.Graphics.PixelsPerUnit) + (Engine.Graphics.PixelsPerUnit - tail.RenderRect.Width);
                tail.RenderRect.Y = _gridRect.Y + (tail.GridLocation.Y * Engine.Graphics.PixelsPerUnit);
            }
            else if (tail.Direction == West)
            {
                tail.RenderRect.Width = (int)Math.Ceiling(Engine.Graphics.PixelsPerUnit * (1.0f - percentage));
                tail.RenderRect.Height = Engine.Graphics.PixelsPerUnit;
                tail.RenderRect.X = _gridRect.X + (tail.GridLocation.X * Engine.Graphics.PixelsPerUnit);
                tail.RenderRect.Y = _gridRect.Y + (tail.GridLocation.Y * Engine.Graphics.PixelsPerUnit);
            }



            _snake[^1] = tail;
        }

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

        private void CreateWalls()
        {
        }

        public override void Draw()
        {
            _isDrawing = true;
            Engine.Instance.GraphicsDevice.SetRenderTarget(RenderTarget);
            Engine.Instance.GraphicsDevice.Viewport = new Viewport(RenderTarget.Bounds);
            Engine.Instance.GraphicsDevice.Clear(Engine.Graphics.ClearColor);

            DrawBackground();
            DrawGrid();
            DrawObjects();
            DrawHud();

            //SpriteBatch.Begin();
            //SpriteBatch.DrawString(_font, $"Empty: {_emptyCells.Count}  |  Filled: {_filledCells.Count}\n Snake: {_snake.Count}", Vector2.Zero, Color.White);
            //SpriteBatch.End();

            base.Draw();

            Engine.Instance.GraphicsDevice.SetRenderTarget(null);

            _isDrawing = false;
        }

        private void DrawGrid()
        {
            int shadowThick = 2;
            int halfShadowThick = 1;
            float alpha = 0.7f;
            Engine.Graphics.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null, null, null);
            Engine.Graphics.SpriteBatch.DrawLine(_gridRect.Right + halfShadowThick,
                                 _gridRect.Top + shadowThick,
                                 _gridRect.Right + halfShadowThick,
                                 _gridRect.Bottom + shadowThick,
                                 Color.Black * alpha, shadowThick);
            Engine.Graphics.SpriteBatch.DrawLine(_gridRect.Left + shadowThick,
                                 _gridRect.Bottom + halfShadowThick,
                                 _gridRect.Right + shadowThick,
                                 _gridRect.Bottom + halfShadowThick
                                 , Color.Black * alpha, shadowThick);
            Engine.Graphics.SpriteBatch.DrawRectangle(_gridRect, Color.Black * 0.9f);
            Engine.Graphics.SpriteBatch.Draw(_gridCellTexture.Texture, _gridRect, new Rectangle(Point.Zero, _gridRect.Size), Color.White * 0.5f);
            Engine.Graphics.SpriteBatch.DrawHollowRectangle(_gridRect, Color.White);
            Engine.Graphics.SpriteBatch.End();
        }

        private void DrawFood()
        {
            Rectangle rect = Rectangle.Empty;
            for (int i = 0; i < _food.Count; i++)
            {
                rect.Location = _gridRect.Location + _food[i].Multiply(Engine.Graphics.PixelsPerUnit);
                rect.Size = new Point(Engine.Graphics.PixelsPerUnit, Engine.Graphics.PixelsPerUnit);
                rect = ScaleRectByBeat(rect, 4);
                Engine.Graphics.SpriteBatch.DrawRectangle(rect, Color.Yellow);
            }
        }

        private void DrawWalls()
        {
            Rectangle rect = Rectangle.Empty;
            for (int i = 0; i < _walls.Count; i++)
            {
                rect.Location = _gridRect.Location + _walls[i].Multiply(Engine.Graphics.PixelsPerUnit);
                rect.Size = new Point(Engine.Graphics.PixelsPerUnit, Engine.Graphics.PixelsPerUnit);
                // rect = ScaleRectByBeat(rect, 12);
                Engine.Graphics.SpriteBatch.DrawRectangle(rect, Color.Orange);
            }
        }

        private void DrawObjects()
        {
            Rectangle rect = Rectangle.Empty;

            Engine.Graphics.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            DrawFood();
            DrawWalls();
            // //  Draw the food
            // rect.Location = _gridRect.Location + _food.Multiply(Engine.Graphics.PixelsPerUnit);
            // rect.Size = new Point(Engine.Graphics.PixelsPerUnit, Engine.Graphics.PixelsPerUnit);
            // // Engine.Graphics.SpriteBatch.DrawRectangle(rect, Color.Yellow);
            // Engine.Graphics.SpriteBatch.DrawRectangle(ScaleRectByBeat(rect, 12), Color.Yellow);

            //  Draw the snake head
            for (int i = 0; i < _snake.Count; i++)
            {
                Engine.Graphics.SpriteBatch.DrawRectangle(_snake[i].RenderRect, Color.White);
            }

            Engine.Graphics.SpriteBatch.End();
        }

        private Rectangle ScaleRectByBeat(Rectangle rect, int factor)
        {
            int from = Engine.Graphics.PixelsPerUnit;
            int to = Engine.Graphics.PixelsPerUnit - factor;
            float per = _pulseTimer / _pulseDuration;
            int lerp = (int)Math.Ceiling(MathHelper.Lerp(from, to, per));

            Rectangle result;
            //  Reduce the original rectangle by 12
            result.X = rect.X + lerp;
            result.Y = rect.Y + lerp;
            result.Width = rect.Width - (lerp * 2);
            result.Height = rect.Height - (lerp * 2);

            // //  Calculate the percentage into the beat we are
            // float percentage = _beatTimer / GameBase.Music.Crochet;
            // Console.WriteLine($"{_beatTimer} -- {percentage}");

            // //  Determine the scale based on the beat percentage

            // int scaleBy = percentage switch
            // {
            //     float p when p >= 0.0f && p < 25.0f => 12,
            //     float p when p >= 25.0f && p < 50.0f => 8,
            //     float p when p >= 50.0f && p < 75.0f => 4,
            //     _ => 0
            // };

            // result.X = rect.X + scaleBy;
            // result.Y = rect.Y + scaleBy;
            // result.Width = rect.Width - (scaleBy * 2);
            // result.Height = rect.Height - (scaleBy * 2);

            // Console.WriteLine($"{rect} -- {result}");

            //  Apply the scale
            // rect.X -= scaleBy;
            // rect.Y -= scaleBy;
            // rect.Width += (scaleBy * 2);
            // rect.Height += (scaleBy * 2);

            return result;
        }

        private void DrawHud()
        {
            Engine.Graphics.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            Engine.Graphics.SpriteBatch.DrawRectangle(_scoreContainer, Color.Black * 0.8f);
            Engine.Graphics.SpriteBatch.DrawHollowRectangle(_scoreContainer, Color.White);

            if (!_hasBegun)
            {
                Engine.Graphics.SpriteBatch.DrawRectangle(_gridRect, Color.Black * 0.4f);

                if (!ScenePaused)
                {
                    Engine.Graphics.SpriteBatch.DrawString(_beginText);
                }
            }
            else
            {

                if (_gameover)
                {
                    Engine.Graphics.SpriteBatch.DrawRectangle(_gameoverContainer, Color.Black * 0.8f);
                    Engine.Graphics.SpriteBatch.DrawHollowRectangle(_gameoverContainer, Color.White);
                    Engine.Graphics.SpriteBatch.DrawString(_gameoverText);
                    Engine.Graphics.SpriteBatch.DrawString(_scoreText);

                    Engine.Graphics.SpriteBatch.DrawString(_tryAgainText);
                }
                else if (_gamePaused)
                {
                    Engine.Graphics.SpriteBatch.DrawString(_pauseText);
                }
                else
                {
                    Engine.Graphics.SpriteBatch.DrawString(_scoreText);
                    Engine.Graphics.SpriteBatch.DrawString(_timerText);

                }
            }
            Engine.Graphics.SpriteBatch.End();

        }

        private void Tick()
        {
            if (!_smoothTail)
            {
                _smoothTail = true;
            }

            //  Check collision first
            //CheckCollision();

            //  We need to cache the value of the snake's head into two seperate
            //  instance. One will be a reference to what the head value was, the 
            //  other we'll use to update the head values
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

            //  Check for body collision
            for (int i = 1; i < _snake.Count; i++)
            {
                if (head.GridLocation == _snake[i].GridLocation)
                {
                    GameOver();
                }
            }

            // if (head.GridLocation == _food)
            // {
            //     _hasEaten = true;
            //     _smoothTail = false;
            // }



            //  Set the current direction of the head body piece equal to the next direction value.
            //  We also set the value of the previous head direction so when it eventually becomes the
            //  tail piece, it can smooth render in the correct direction.
            previousHead.Direction = head.Direction = _nextDirection;
            _bgOffsetDirection = _nextDirection;
            //_bgOffsetDirection.Y *= -1;



            //  We need to reset the render rect of the previous head value due to an issue
            //  with it sometimes only being partially corrct due to the percentage calculations during
            //  the update part (yay floats)
            previousHead.RenderRect.Location = _gridRect.Location + (previousHead.GridLocation.Multiply(Engine.Graphics.PixelsPerUnit));
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
            head.RenderRect.X = _gridRect.X + (head.GridLocation.X * Engine.Graphics.PixelsPerUnit);
            head.RenderRect.Y = _gridRect.Y + (head.GridLocation.Y * Engine.Graphics.PixelsPerUnit);
            head.RenderRect.Width = Engine.Graphics.PixelsPerUnit;
            head.RenderRect.Height = Engine.Graphics.PixelsPerUnit;

            //  Update the local reference to the head
            _snake[0] = head;

            if (_hasEaten)
            {
                //  If we've eaten, then the new body piece that is created is the same as
                //  what the previous piece was
                _hasEaten = false;
                _snake.Insert(1, previousHead);
                FillCell(previousHead.GridLocation);

                //  Spawn new food
                // SpawnFood();


                IncreaseScore(100);
            }
            else
            {
                //  If we didn't eat, then we need to move the tail to the previous head 
                //  position.

                //  Remove the tail
                EmptyCell(_snake[^1].GridLocation);
                _snake.RemoveAt(_snake.Count - 1);


                //  Reinsert tail
                _snake.Insert(1, previousHead);
                FillCell(previousHead.GridLocation);
            }

            //  Now that we've moved the head, we need to fill the cell its in ow
            FillCell(head.GridLocation);


            //  Check head-to-body collision here. By doing it at the end, we prevent
            //  weird issues where collision occurs even though visually it doesn't look
            //  like it should have
            for (int i = 1; i < _snake.Count; i++)
            {
                if (head.GridLocation == _snake[i].GridLocation)
                {
                    //  Snake head collided with a body piece, so this is game over

                }
            }
        }


        private void GameOver()
        {
            _gameover = true;
            _scoreText.X = (Engine.Graphics.Resolution.X / 2) - (_scoreText.Size.X / 2);
            _scoreText.Y = _gameoverContainer.Bottom - _scoreText.Size.Y;
        }

        private void SpawnFood()
        {
            _food.Clear();

            for (int i = 0; i < 10; i++)
            {
                Point cell = Maths.Random.Next<Point>(_emptyCells);
                _food.Add(cell);
                FillCell(cell);
            }
            //     //  Choose a random emtpy cell from the empty cell list
            //     Point cell = Maths.Random.Next<Point>(_emptyCells);

            // //  Update the food refrence
            // _food = cell;

            // //  Update the fillded cell refrence.
            // FillCell(cell);
        }

        private void EmptyCell(Point cell)
        {
            if (!_emptyCellsHash.Contains(cell))
            {
                _emptyCells.Add(cell);
                _emptyCellsHash.Add(cell);
            }
            else
            {
                throw new Exception("Told to empty cell, but cell already empty");
            }

            if (_filledCellsHash.Contains(cell))
            {
                _filledCells.Remove(cell);
                _filledCellsHash.Remove(cell);
            }


        }

        /// <summary>
        ///     Adjust the Empty and FIlled cell 
        /// </summary>
        /// <param name="cell"></param>
        private void FillCell(Point cell)
        {
            if (_emptyCellsHash.Contains(cell))
            {
                _emptyCells.Remove(cell);
                _emptyCellsHash.Remove(cell);
            }

            if (!_filledCellsHash.Contains(cell))
            {
                _filledCells.Add(cell);
                _filledCellsHash.Add(cell);
            }
        }

        private void IncreaseScore(int increaseB)
        {
            _score += increaseB;
            _scoreText.Value = $"Score: {_score:000000000}";
        }
    }
}
