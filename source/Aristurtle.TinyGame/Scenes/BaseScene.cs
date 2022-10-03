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

public class BaseScene : Scene
{
    protected InputProfile _input;
    TinyTexture _backgroundTexture;
    private Rectangle _backgroundDestination;
    private Rectangle _backgroundSource;
    protected int _bgOffset;

    protected Point _bgOffsetDirection;
    private int _bgOffsetX;
    private int _bgOffsetY;

    private TimeSpan _volumeTimer;
    private Text _volumeText;
    private bool _volumeShown;


    public BaseScene(InputProfile input) : base()
    {
        _input = input;
        _bgOffsetDirection = new Point(0, 0);
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
            X = _bgOffset,
            Y = _bgOffset,
            Width = Engine.Graphics.Resolution.X,
            Height = Engine.Graphics.Resolution.Y
        };

        _volumeText.X = Engine.Graphics.Resolution.X - _volumeText.Size.X - 5;
        _volumeText.Y = 5;


    }

    public override void Update()
    {
        _bgOffsetX = (_bgOffsetX + _bgOffsetDirection.X) % _backgroundTexture.Width;
        _bgOffsetY = (_bgOffsetY + _bgOffsetDirection.Y) % _backgroundTexture.Height;

        _backgroundSource = new Rectangle
        {
            X = _bgOffsetX,
            Y = _bgOffsetY,
            Width = Engine.Graphics.Resolution.X,
            Height = Engine.Graphics.Resolution.Y
        };

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

        if(_volumeShown)
        {
            _volumeTimer -= Engine.Time.ElapsedGameTime;

            if(_volumeTimer <= TimeSpan.Zero)
            {
                _volumeShown = false;
            }
        }



    }

    public override void LoadContent()
    {
        base.LoadContent();
        _backgroundTexture = new(Engine.GlobalContent.Load<Texture2D>("background_pattern"));
        SpriteFont font = Engine.GlobalContent.Load<SpriteFont>("RussoOne32");
        _volumeText = new(font, "Volume 000");
        _volumeText.Color = Color.White * 0.5f;
    }

    public override void UnloadContent()
    {
        base.UnloadContent();
        _backgroundTexture = null;
    }

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

    protected void DrawBackground()
    {
        Engine.Graphics.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null, null, null);
        Engine.Graphics.SpriteBatch.Draw(_backgroundTexture.Texture, _backgroundDestination, _backgroundSource, Color.White * 0.7f);
        Engine.Graphics.SpriteBatch.End();
    }
}

