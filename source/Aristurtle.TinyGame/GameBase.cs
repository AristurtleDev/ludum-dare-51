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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Aristurtle.TinyEngine;
using Aristurtle.TinyGame.Scenes;

namespace Aristurtle.TinyGame;

public class GameBase : Engine
{
    public static Conductor Music { get; } = new();
    private InputProfile _input;

    public bool FreezeBackground { get; set; }


    public GameBase() : base("Tiny Game", new())
    {
        _input = new();
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {

        Graphics.Initialize(1280, 720, 1280, 720, false);
        Graphics.PixelsPerUnit = 32;
        Graphics.ClearColor = Color.Transparent;

        Window.AllowUserResizing = true;

        //  Input is initialized in base here, so register has to come after
        base.Initialize();
        _input.Register();


        Scenes.ChangeScene(new Bootstrap(_input), null, new FadeTransition());
    }

    protected override void LoadContent()
    {
        base.LoadContent();
        // Assets.LoadEssential();
        Window.Title = "It's Snake, But Every 10 Seconds The Food Turns Into Walls";
    }

    protected override void Update(GameTime gameTime)
    {
        Music.Update();
        base.Update(gameTime);
    }
}
