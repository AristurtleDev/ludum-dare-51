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
using System.Collections;

namespace Aristurtle.TinyGame.Scenes
{
    /// <summary>
    ///     Scene responsible for bootstraping the game, including loading
    ///     essential game assets, starting the audio, and prsenting the
    ///     created by text.
    /// </summary>
    public class Bootstrap : Scene
    {
        //  Indicates if we are finished with the bootstrap scene and should
        //  change scenes.
        private bool _changeScene;

        //  Indicates if the user presses an input to skip the present text.
        private bool _skipped;

        //  Text used to display text on the screen.
        private Text _text;

        //  Input profile for detecting player input.
        private InputProfile _input;

        //  Used to incrementally execute presenation text logic.
        private IEnumerator _present;

        //  The alpha value of the text rendered
        private float _alpha;

        //  The destiniation rectangle used for the background render.
        private Rectangle _backgroundDestination;

        //  The source rectangle used for the background render.
        private Rectangle _backgroundSource;

        private TinyTexture _backgroundPattern;

        private Color _textColor;

        /// <summary>
        ///     Creats a new <see cref="Bootstrap"/> scene.
        /// </summary>
        /// <param name="input">
        ///     The input profile for the game.
        /// </param>
        public Bootstrap(InputProfile input) : base()
        {
            _input = input;
            _skipped = false;
            _textColor = Color.White;
        }

        /// <summary>
        ///     Called after the transition in for the scene has completed.
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            //  Set the present enumerator.
            _present = PresentScreen();
        }

        /// <summary>
        ///     Initialize the scene.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            //  Initialize the rect used for the backgorund.
            _backgroundDestination = new Rectangle
            {
                X = 0,
                Y = 0,
                Width = Engine.Graphics.Resolution.X,
                Height = Engine.Graphics.Resolution.Y
            };

            _backgroundSource = new Rectangle
            {
                X = 0,
                Y = 0,
                Width = Engine.Graphics.Resolution.X,
                Height = Engine.Graphics.Resolution.Y
            };

            //  Create the text to show on screen.
            SpriteFont font = GameBase.EssentialGameAssets["font64"] as SpriteFont;
            _text = new(font, "Created By AristurtleDev", _textColor * 0.0f);
            _text.Position = Engine.Graphics.Resolution.HalfValue().ToVector2();
            _text.CenterOrigin();

            //  Initialize the background pattern.
            _backgroundPattern = GameBase.EssentialGameAssets["backgroundTex"] as TinyTexture;
        }

        /// <summary>
        ///     Load all initial game content.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            //  Start the music
            GameBase.Music.PlaySong(SongTitle.Realizer);

            //  Load fonts
            SpriteFont font32 = Engine.GlobalContent.Load<SpriteFont>("RussoOne32");
            SpriteFont font64 = Engine.GlobalContent.Load<SpriteFont>("RussoOne64");
            SpriteFont font128 = Engine.GlobalContent.Load<SpriteFont>("RussoOne128");

            //  Load textures
            TinyTexture backgroundTex = new(Engine.GlobalContent.Load<Texture2D>("background_pattern"));
            TinyTexture gridCelTex = new(Engine.GlobalContent.Load<Texture2D>("grid_cell"));

            //  Put the content in the essentials dictionary
            GameBase.EssentialGameAssets.Add(nameof(font32), font32);
            GameBase.EssentialGameAssets.Add(nameof(font64), font64);
            GameBase.EssentialGameAssets.Add(nameof(font128), font128);
            GameBase.EssentialGameAssets.Add(nameof(backgroundTex), backgroundTex);
            GameBase.EssentialGameAssets.Add(nameof(gridCelTex), gridCelTex);

            GameBase.Music.LoadEffects();
        }


        /// <summary>
        ///     An enumeration that is used to displaythe Created By text on 
        ///     the screen, fading it in, then fading it back out
        /// </summary>
        /// <returns></returns>
        private IEnumerator PresentScreen()
        {
            //  A 0.5 second timespan.
            TimeSpan halfSecond = TimeSpan.FromSeconds(0.5);

            //  A 1 second timespan.
            TimeSpan oneSecond = TimeSpan.FromSeconds(1);

            //  Set the initial timer to 0.5 seconds.
            TimeSpan timer = halfSecond;

            //  Fade text in
            while (timer > TimeSpan.Zero && !_skipped)
            {
                timer -= Engine.Time.ElapsedGameTime;
                _alpha = MathHelper.LerpPrecise(0.0f, 1.0f, 1.0f - (float)(timer / halfSecond));
                yield return null;
            }

            //  Set the timer to 1 second, this is used to pause the fade on
            //  the text by 1 second before fading out.
            timer = oneSecond;

            //  Present text for one second
            while (timer > TimeSpan.Zero && !_skipped)
            {
                timer -= Engine.Time.ElapsedGameTime;
                yield return null;
            }

            //  Set the timer back to 0.5 seconds to fade it out.
            timer = halfSecond;

            //  Fade text out
            while (timer > TimeSpan.Zero && !_skipped)
            {
                timer -= Engine.Time.ElapsedGameTime;
                _alpha = MathHelper.LerpPrecise(1.0f, 0.0f, 1.0f - (float)(timer / halfSecond));
                yield return null;
            }
        }

        /// <summary>
        ///     Update the scene.
        /// </summary>
        public override void Update()
        {
            if (_changeScene)
            {
                ChangeScene(new TitleScene(_input), new EvenOddTileTransition(), new EvenOddTileTransition());
            }
            else
            {
                //  As long as we are presenting, continue to move to the next
                //  iteration of the presenter.
                if (_present != null && _present.MoveNext())
                {
                    //  Adjust the color fo the text by the alpha value that
                    //  is calculated in the presenter enumeration.  
                    _text.Color = _textColor * _alpha;

                    //  If the user presses any button, then they can skip the
                    //  presentation screen and move on immediatly to the
                    //  next scene.
                    if (_input.AnyButtonPressed)
                    {
                        _changeScene = true;
                    }
                }
                else if (_present != null)
                {
                    //  This will only occur when the presenter finished, so
                    //  we can flag now to change scene.
                    _present = null;
                    _changeScene = true;
                }
            }

            //  Update the background source rect so it is moving.
            _backgroundSource.X = (_backgroundSource.X + 1) % _backgroundPattern.Width;
            _backgroundSource.Y = (_backgroundSource.Y - 1) % _backgroundPattern.Height;
        }

        /// <summary>
        ///     Draws this scene.
        /// </summary>
        public override void Draw()
        {
            base.Draw();

            //  Prepare the graphics for rendering.
            Engine.Graphics.Device.SetRenderTarget(RenderTarget);
            Engine.Graphics.Device.Viewport = new Viewport(RenderTarget.Bounds);
            Engine.Graphics.Clear();

            //  Drawing the background pattern
            Engine.Graphics.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null, null, null);
            Engine.Graphics.SpriteBatch.Draw(_backgroundPattern.Texture, _backgroundDestination, _backgroundSource, Color.White * 0.7f);
            Engine.Graphics.SpriteBatch.End();

            //  Drawing the message on screen
            Engine.Graphics.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            Engine.Graphics.SpriteBatch.DrawString(_text);
            Engine.Graphics.SpriteBatch.End();

            //  Alwoys derefernce the scenes render target when finished.
            Engine.Graphics.Device.SetRenderTarget(null);
        }


    }
}
