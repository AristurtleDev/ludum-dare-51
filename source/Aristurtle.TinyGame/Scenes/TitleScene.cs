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

namespace Aristurtle.TinyGame.Scenes
{
    public class TitleScene : BaseScene
    {
        SpriteFont _hudFont;
        SpriteFont _titleFont;

        private Text _titleText;
        private TextBuilder _pressToStartText;
        Rectangle _titleTextContainer;

        //  A rectangle value used to render a container rectangle for the 
        //  _pressToStartText text.
        Rectangle _containerRect;

        public TitleScene(InputProfile input) : base(input)
        {
            _bgOffsetDirection = new Point(-1, -1);
        }

        public override void Initialize()
        {
            base.Initialize();
            InitializePressToStartText();
            InitializeTitleBanner();
        }

        private void InitializePressToStartText()
        {
            _pressToStartText = new TextBuilder();
            _pressToStartText.Add(_hudFont, "Press Enter to start ", Color.White);
            // _pressToStartText.Add(_controllerFont, "a", Color.White, 0.5f);
            // _pressToStartText.Add(_hudFont, " to start ", Color.White);

            Vector2 margin = new Vector2(20, 20);

            _containerRect = new Rectangle();
            _containerRect.X = 0;
            _containerRect.Height = (int)_pressToStartText.Size.Y + ((int)margin.Y * 2);
            _containerRect.Width = Engine.Graphics.Resolution.X;
            _containerRect.Y = Engine.Graphics.Resolution.Y - _containerRect.Height;


            _pressToStartText.Position = new Vector2
            {
                X = _containerRect.Center.X,
                Y = _containerRect.Center.Y
            };
            _pressToStartText.Origin = new Vector2(_pressToStartText.Size.X * 0.5f, _pressToStartText.Size.Y * 0.5f);
        }

        private void InitializeTitleBanner()
        {
            int rectWidth = 774;
            int rectHeight = 214;
            _titleTextContainer = new Rectangle()
            {
                X = (Engine.Graphics.Resolution.X / 2) - (rectWidth / 2),
                Y = ((Engine.Graphics.Resolution.Y - _containerRect.Height) / 2) - (rectHeight / 2),
                Width = rectWidth,
                Height = rectHeight
            };

            string title = "It's Snake, But Every 10 Seconds Food Turns Into Walls";

            title = Maths.WordWrap(_titleFont, title, _titleTextContainer.Width - 5);

            // _titleText = new Text(_titleFont, "Tiny Snake", Color.Black);
            _titleText = new Text(_titleFont, title, Color.Black);

            _titleText.Position = new Vector2
            {
                X = Engine.Graphics.Resolution.X * 0.5f,
                Y = (Engine.Graphics.Resolution.Y - _containerRect.Height) * 0.5f
            };
            _titleText.CenterOrigin();
        }

        TinyTexture _abutton;
        public override void LoadContent()
        {
            base.LoadContent();
            _hudFont = Engine.GlobalContent.Load<SpriteFont>("RussoOne32");
            _titleFont = Engine.GlobalContent.Load<SpriteFont>("russoOne64");

            // Assets.TryGetFont("Controller", 64.0f, out _controllerFont);
            // Assets.ControllerIcons.TryGetSprite("left_thumbstick_down", out _abutton);

        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            //  Remove references
            _hudFont = null;
            _titleFont = null;
        }

        public override void Update()
        {
            base.Update();
            if (_input.TitleStart.Pressed)
            {
                _input.TitleStart.ConsumePress();
                ChangeScene(new PlayScene(_input), new FadeTransition(), new FadeTransition());
            }

        }


        public override void Draw()
        {
            Engine.Graphics.Device.SetRenderTarget(RenderTarget);
            Engine.Graphics.Device.Viewport = new Viewport(RenderTarget.Bounds);
            Engine.Graphics.Clear();

            DrawBackground();

            Engine.Graphics.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            DrawTitleBanner();
            Engine.Graphics.SpriteBatch.End();

            Engine.Graphics.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearClamp, null, null, null, null);
            DrawPressAnyKey();
            Engine.Graphics.SpriteBatch.End();

            base.Draw();




            Engine.Instance.GraphicsDevice.SetRenderTarget(null);
        }

        private void DrawTitleBanner()
        {
            Engine.Graphics.SpriteBatch.DrawRectangle(_titleTextContainer, new Color(238, 255, 204));
            Engine.Graphics.SpriteBatch.DrawHollowRectangle(_titleTextContainer, Color.Black);
            Engine.Graphics.SpriteBatch.DrawString(_titleText);
        }

        private void DrawPressAnyKey()
        {

            Engine.Graphics.SpriteBatch.DrawRectangle(_containerRect, Color.Black);
            Engine.Graphics.SpriteBatch.DrawString(_pressToStartText);
        }
    }
}
