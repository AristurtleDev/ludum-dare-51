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

using System;
using Aristurtle.TinyEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Aristurtle.TinyGame;

/// <summary>
///     The base scene used by other scenes. 
/// </summary>
public class BaseScene : Scene
{
    //  The input profile used by the game.
    protected InputProfile _input;

    //  The backgroudn texture used to render the background.
    TinyTexture _backgroundTexture;

    //  The destination rectangle used when rendering the background with
    //  wrapping.
    private Rectangle _backgroundDestination;

    //  The source rectangle used when rendring the background with warpping.
    private Rectangle _backgroundSource;

    //  The offset of the background wrapping.
    protected int _bgOffset;

    //  The direction of the background warpping.
    protected Point _bgOffsetDirection;

    //  The offset of the background warpping on the x axis.
    private int _bgOffsetX;

    //  The offset of the background warpping on the y axis.
    private int _bgOffsetY;

    //  Used to track the time to dispaly the volume adjustment text.
    private TimeSpan _volumeTimer;

    //  The volume adjustment text.
    private Text _volumeText;

    //  Flags if the volume adjustment text is shown.
    private bool _volumeShown;

    /// <summary>
    ///     Initializes a new <see cref="BaseScene"/>.
    /// </summary>
    /// <param name="input">
    ///     The input profile used by the game.
    /// </param>
    /// <returns></returns>
    public BaseScene(InputProfile input) : base()
    {
        _input = input;
        _bgOffsetDirection = new Point(0, 0);
    }

    /// <summary>
    ///     Initializes the base scene.
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();

        //  Initialzie the destination rect for the background
        _backgroundDestination = new();
        _backgroundDestination.X = 0;
        _backgroundDestination.Y = 0;
        _backgroundDestination.Width = Engine.Graphics.Resolution.X;
        _backgroundDestination.Height = Engine.Graphics.Resolution.Y;

        //  Initialize the background source rect
        _backgroundSource = new();
        _backgroundSource.X = _bgOffset;
        _backgroundSource.Y = _bgOffset;
        _backgroundSource.Width = Engine.Graphics.Resolution.X;
        _backgroundSource.Height = Engine.Graphics.Resolution.Y;

        //  Initialize the background texture
        _backgroundTexture = GameBase.EssentialGameAssets["backgroundTex"] as TinyTexture;

        //  Initialize the volume text
        SpriteFont font32 = GameBase.EssentialGameAssets["font32"] as SpriteFont;
        _volumeText = new(font32, "Volume 000");
        _volumeText.Color = Color.White * 0.5f;
        _volumeText.OutlineColor = Color.Black * 0.5f;
        _volumeText.IsOutlined = true;
        _volumeText.X = Engine.Graphics.Resolution.X - _volumeText.Size.X - 5;
        _volumeText.Y = 5;
    }

    /// <summary>
    ///     Updates the base scene.
    /// </summary>
    public override void Update()
    {
        //  Adjust the offset of the background wrapping
        _bgOffsetX = (_bgOffsetX + _bgOffsetDirection.X) % _backgroundTexture.Width;
        _bgOffsetY = (_bgOffsetY + _bgOffsetDirection.Y) % _backgroundTexture.Height;

        //  Adjust the background source rect
        _backgroundSource.X = _bgOffsetX;
        _backgroundSource.Y = _bgOffsetY;


        //  If the volume up or down buttons were pressed, adjust volume.
        if (_input.VolumeDown.Pressed)
        {
            GameBase.Music.DecreaseVolume();
            _volumeShown = true;
            _volumeTimer = TimeSpan.FromSeconds(1);            
            _volumeText.Value = $"Volume: {GameBase.Music.Volume}";
        }
        else if (_input.VolumeUp.Pressed)
        {
            GameBase.Music.IncreaseVolume();
            _volumeShown = true;
            _volumeTimer = TimeSpan.FromSeconds(1);
            _volumeText.Value = $"Volume: {GameBase.Music.Volume}";
        }

        //  If volume was adjusted, update the timer.
        if(_volumeShown)
        {
            _volumeTimer -= Engine.Time.ElapsedGameTime;

            if(_volumeTimer <= TimeSpan.Zero)
            {
                _volumeShown = false;
            }
        }
    }

    /// <summary>
    ///     Unloads content references used by this scene.
    /// </summary>
    public override void UnloadContent()
    {
        base.UnloadContent();
        _backgroundTexture = null;
    }

    /// <summary>
    ///     Draws this scene.
    /// </summary>
    public override void Draw()
    {
        base.Draw();

        if (_volumeShown)
        {
            Engine.Graphics.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            Engine.Graphics.SpriteBatch.DrawString(_volumeText);
            Engine.Graphics.SpriteBatch.End();
        }
    }

    /// <summary>
    ///     When called by a derived scene class, draws the background.
    /// </summary>
    protected void DrawBackground()
    {
        Engine.Graphics.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null, null, null);
        Engine.Graphics.SpriteBatch.Draw(_backgroundTexture.Texture, _backgroundDestination, _backgroundSource, Color.White * 0.7f);
        Engine.Graphics.SpriteBatch.End();
    }
}

