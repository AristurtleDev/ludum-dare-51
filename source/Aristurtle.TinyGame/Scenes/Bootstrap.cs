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
using System.Threading;

namespace Aristurtle.TinyGame.Scenes
{
    public class Bootstrap : Scene
    {
        //  Indicates if the content loading has finished;
        private bool _loaded;

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

        public Bootstrap(InputProfile input) : base()
        {
            _input = input;
            _skipped = false;
            _textColor = Color.White;
        }

        public override void Begin()
        {
            base.Begin();

            _present = PresentScreen();
        }

        public override void Initialize()
        {
            base.Initialize();
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
        }
        public override void LoadContent()
        {
            base.LoadContent();

            SpriteFont font = Engine.GlobalContent.Load<SpriteFont>("RussoOne64");
            _text = new(font, "Created By AristurtleDev", _textColor * 0.0f);
            _text.Position = Engine.Graphics.Resolution.HalfValue().ToVector2();
            _text.CenterOrigin();

            _backgroundPattern = new(Engine.GlobalContent.Load<Texture2D>("background_pattern"));
            GameBase.Music.PlaySong(SongTitle.Realizer);

        }


        private IEnumerator PresentScreen()
        {
            TimeSpan halfSecond = TimeSpan.FromSeconds(0.5);
            TimeSpan oneSecond = TimeSpan.FromSeconds(1);

            TimeSpan timer = halfSecond;

            //  Fade text in
            while (timer > TimeSpan.Zero && !_skipped)
            {
                timer -= Engine.Time.ElapsedGameTime;
                _alpha = MathHelper.LerpPrecise(0.0f, 1.0f, 1.0f - (float)(timer / halfSecond));
                yield return null;
            }

            timer = oneSecond;

            //  Present text for one second
            while (timer > TimeSpan.Zero && !_skipped)
            {
                timer -= Engine.Time.ElapsedGameTime;
                yield return null;
            }

            timer = halfSecond;

            //  Fade text out
            while (timer > TimeSpan.Zero && !_skipped)
            {
                timer -= Engine.Time.ElapsedGameTime;
                _alpha = MathHelper.LerpPrecise(1.0f, 0.0f, 1.0f - (float)(timer / halfSecond));
                yield return null;
            }


            //  End the present text, show the loading text
            _text.Value = "Loading...";
            _text.CenterOrigin();
            _text.Color = Color.White;

        }

        public override void Update()
        {
            if (_loaded)
            {
                ChangeScene(new TitleScene(_input), new EvenOddTileTransition(), new EvenOddTileTransition());
            }
            else
            {

                if (_present != null && _present.MoveNext())
                {
                    _text.Color = _textColor * _alpha;

                    if (_input.AnyButtonPressed)
                    {
                        _skipped = true;
                    }
                }
                else if (_present != null)
                {
                    _present = null;
                    _loaded = true;
                }
            }

            _backgroundSource.X = (_backgroundSource.X + 1) % _backgroundPattern.Width;
            _backgroundSource.Y = (_backgroundSource.Y - 1) % _backgroundPattern.Height;
        }

        public override void Draw()
        {
            base.Draw();

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

            Engine.Graphics.Device.SetRenderTarget(null);
        }


    }
}
