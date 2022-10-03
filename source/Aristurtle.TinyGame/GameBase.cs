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
using Aristurtle.TinyEngine;
using Aristurtle.TinyGame.Scenes;
using System.Collections.Generic;

namespace Aristurtle.TinyGame;

/// <summary>
///     The base game instance.  It all starts here. Inherits from 
///     <see cref="Aristurtle.TinyEngine.Engine"/>.
/// </summary>
public class GameBase : Engine
{
    /// <summary>
    ///     The essential game assets that are reused throughout every scene.
    ///     Loaded once during the bootstrap and cached here.
    /// </summary>
    public static Dictionary<string, object> EssentialGameAssets { get; } = new();
    
    /// <summary>
    ///     The <see cref="Conductor"/> responsible for all audio.
    /// </summary>
    /// <returns></returns>
    public static Conductor Music { get; } = new();

    //  The input profile used throughout the game to determine playing input.
    private InputProfile _input;

    /// <summary>
    ///     Should the background movement be frozen?
    /// </summary>
    public bool FreezeBackground { get; set; }


    /// <summary>
    ///     Creates a new <see cref="GameBase"/> instance.
    /// </summary>
    /// <returns></returns>
    public GameBase() : base("It's Snae, But Every 10 Seconds The Food Becomes Walls", new())
    {
        _input = new();
        IsMouseVisible = true;
    }

    /// <summary>
    ///     Performs initial initializations.
    /// </summary>
    protected override void Initialize()
    {

        //  Initialize the graphics.
        Graphics.Initialize(1280, 720, 1280, 720, false);
        Graphics.PixelsPerUnit = 32;
        Graphics.ClearColor = Color.Transparent;

        //  User is allowed to resize window. 
        Window.AllowUserResizing = true;

        //  Input is initialized in base here, so register has to come after
        base.Initialize();
        _input.Register();

        //  Set the initial scene to the bootstrap seen.  This is where all
        //  iniital content is loaded.
        Scenes.ChangeScene(new Bootstrap(_input), null, new FadeTransition());
    }

    /// <summary>
    ///     Updates the game.
    /// </summary>
    /// <param name="gameTime">
    ///     A snapshot of the timeing values provided by the MonoGame framework.
    /// </param>
    protected override void Update(GameTime gameTime)
    {
        //  Ensure the base engine is updated
        base.Update(gameTime);

        //  Update the audio.
        Music.Update();
    }
}