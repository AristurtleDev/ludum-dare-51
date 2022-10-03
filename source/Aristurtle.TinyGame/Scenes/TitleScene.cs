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
        //  The font used by hud elements.
        SpriteFont _hudFont;

        //  The font used for the title display.
        SpriteFont _titleFont;

        //  Text object that displays the title text.
        private Text _titleText;

        //  Text object that displays the press to start text.
        private Text _pressToStartText;
        

        //  Rect used to define the bounds of the container for the title text.
        Rectangle _titleTextContainerRect;


        //  Rect used to define the bounds for the bottom container
        Rectangle _bottomContainerRect;

        /// <summary>
        ///     Creates a new <see cref="TitleScene"/> instance..
        /// </summary>
        /// <param name="input">
        ///     The input profile for the game.
        /// </param>
        /// <returns></returns>
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

        /// <summary>
        ///     Initializes the text and container for the press to start text.
        /// </summary>
        private void InitializePressToStartText()
        {
            //  Create the container rect.
            _bottomContainerRect = new Rectangle();
            _bottomContainerRect.Width = Engine.Graphics.Resolution.X;
            _bottomContainerRect.Height = 80;
            _bottomContainerRect.X = 0;
            _bottomContainerRect.Y = Engine.Graphics.Resolution.Y - _bottomContainerRect.Height;

            //  Create the text object. Position it so it's center of the rect
            _pressToStartText = new(_hudFont, "Press Enter To Start");
            _pressToStartText.X = _bottomContainerRect.Center.X;
            _pressToStartText.Y = _bottomContainerRect.Center.Y;
            _pressToStartText.CenterOrigin();
        }

        /// <summary>
        ///     Initializes the text and container for the title banner.
        /// </summary>
        private void InitializeTitleBanner()
        {
            _titleTextContainerRect = new();
            _titleTextContainerRect.Width = 774;
            _titleTextContainerRect.Height = 214;
            _titleTextContainerRect.X = (Engine.Graphics.Resolution.X / 2) - (_titleTextContainerRect.Width / 2);
            _titleTextContainerRect.Y = ((Engine.Graphics.Resolution.Y - _bottomContainerRect.Height) / 2) - (_titleTextContainerRect.Height / 2);

            //  Create the title string, but wrap it so it wraps within the
            //  container
            string title = "It's Snake, But Every 10 Seconds Food Turns Into Walls";
            title = Maths.WordWrap(_titleFont, title, _titleTextContainerRect.Width - 5);

            //  Create the title text object. Position it so it's in the center
            //  of the container.
            _titleText = new Text(_titleFont, title, Color.Black);
            _titleText.X = _titleTextContainerRect.Center.X;
            _titleText.Y = _titleTextContainerRect.Center.Y;
            _titleText.CenterOrigin();
        }

        public override void LoadContent()
        {
            base.LoadContent();
            _hudFont = GameBase.EssentialGameAssets["font32"] as SpriteFont;
            _titleFont = GameBase.EssentialGameAssets["font64"] as SpriteFont;
        }

        /// <summary>
        ///     Unloads content references from this scene.
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();

            //  Remove references
            _hudFont = null;
            _titleFont = null;
        }

        /// <summary>
        ///     Updates this scene.
        /// </summary>
        public override void Update()
        {
            base.Update();

            //  If the user presses the start button, start the game.
            if (_input.TitleAction.Pressed)
            {
                _input.TitleAction.ConsumePress();
                ChangeScene(new PlayScene(_input), new FadeTransition(), new FadeTransition());
            }
        }

        /// <summary>
        ///     Draws this scene.
        /// </summary>
        public override void Draw()
        {
            //  Prepare the graphics
            Engine.Graphics.Device.SetRenderTarget(RenderTarget);
            Engine.Graphics.Device.Viewport = new Viewport(RenderTarget.Bounds);
            Engine.Graphics.Clear();

            DrawBackground();
            DrawHud();
            base.Draw();

            //  Alwoys derefernce the scenes render target when finished.
            Engine.Instance.GraphicsDevice.SetRenderTarget(null);
        }

        /// <summary>
        /// ///     Draws the hud.
        /// </summary>
        private void DrawHud()
        {
            Engine.Graphics.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            //  Draw the title banner
            Engine.Graphics.SpriteBatch.DrawRectangle(_titleTextContainerRect, new Color(238, 255, 204));
            Engine.Graphics.SpriteBatch.DrawHollowRectangle(_titleTextContainerRect, Color.Black);
            Engine.Graphics.SpriteBatch.DrawString(_titleText);

            //  Draw the press to start section.
            Engine.Graphics.SpriteBatch.DrawRectangle(_bottomContainerRect, Color.Black);
            Engine.Graphics.SpriteBatch.DrawString(_pressToStartText);

            Engine.Graphics.SpriteBatch.End();
        }
    }
}
