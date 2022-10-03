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

using System.Runtime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Aristurtle.TinyEngine;

public class Engine : Game
{
    private readonly FPS _fps;

#pragma warning disable CS8618
    public static Engine Instance { get; private set; }
    public static Graphics Graphics { get; private set; }
    public static Time Time { get; private set; }
    public static SceneManager Scenes { get; private set; }
    public static ContentManager GlobalContent { get; private set; }
#pragma warning restore CS8618

    /// <summary>
    ///     Gets the <see cref="SpriteBatch"/> instance used for rendering.
    /// </summary>
    public SpriteBatch? SpriteBatch { get; private set; }

    /// <summary>
    ///     Gets a <see cref="string"/> value containing the title of the game.
    /// </summary>
    /// <remarks>
    ///     The title of the game is displayed in the title bar of the game window
    ///     when rendering in windowed mode.
    /// </remarks>
    public string Title { get; private set; }

    /// <summary>
    ///     Gets a <see cref="Version"/> instance that describes the version of
    ///     the game.
    /// </summary>
    public Version? Version { get; protected set; }

    /// <summary>
    ///     Creates a new <see cref="Engine"/> instance.
    /// </summary>
    /// <param name="title">
    ///     A <see cref="string"/> value containing the title of the game.
    /// </param>
    /// <param name="graphicsOptions">
    ///     A <see cref="GraphicsOptions"/> value containing the settings for
    ///     the <see cref="Tiny.Graphics"/> instance.
    /// </param>
    public Engine(string title, GraphicsOptions options)
    {
        Instance = this;
        GlobalContent = Content;

        if (string.IsNullOrEmpty(title))
        {
            title = "No Title";
        }

        Graphics = new(this, options);
        Graphics.ClientSizeChanged += OnClientSizeChanged;
        Graphics.GraphicsDeviceReset += OnGraphicsDeviceReset;
        Graphics.GraphicsDeviceCreated += OnGraphicsDeviceCreated;

        Time = new();
        Scenes = new();
        _fps = new();


        Title = title;

        //  Set the root directory for contnet
        Content.RootDirectory = @"Content";
        FileUtilities.Initialize(Content.RootDirectory);

        ////  Initialize the Scene system
        //Scene = new SceneManager(this);

        //  isFixedTimeStep is disabled by default in TinyEngine.  This can be overriden
        //  in the derived game by setting the value to true in the constructor
        IsFixedTimeStep = false;

        //  InactiveSleepTime is a value that determines hwo long the main thread will
        //  sleep for each update cycle when the game window is inactive.  TinyEngine sets
        //  this to 0 seconds by default.  This can be overridden in by the dervied game
        //  by setting the value manually in the constructor
        InactiveSleepTime = TimeSpan.Zero;

        //  Garbage collection is set to sustanted low latency in TinyEngine. This can
        //  be overridden by the dervied game class by setting the value manullay in 
        //  the constructor
        GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
    }

    /// <summary>
    ///     This will be called anytime the client game window size is changed. This
    ///     shoudl be overridden by the derived game class to handle any logic that
    ///     needs to be performed by the game.
    /// </summary>
    protected virtual void OnClientSizeChanged(object? sender, EventArgs e)
    {
        Scenes?.OnClientSizeChanged();
    }

    /// <summary>
    ///     This will be called anytime the <see cref="GraphicsDeviceManager.DeviceCreated"/>
    ///     event is triggered. This should be overritten by the derived game calss to handle
    ///     any logic that needs to be perforemd by the game.
    /// </summary>
    protected virtual void OnGraphicsDeviceCreated(object? sender, EventArgs e)
    {
        Scenes?.OnGraphicsDeviceCreated();
    }

    /// <summary>
    ///     This will be called anytime the <see cref="GraphicsDeviceManager.DeviceReset"/>
    ///     event is triggered.  When this happens, all contents of VRAM will be discarded
    ///     and things like RenderTargets will need to be recreated.  This shdould be 
    ///     overridden by the derived game class to handle this scenario.
    /// </summary>
    protected virtual void OnGraphicsDeviceReset(object? sender, EventArgs e)
    {
        Scenes?.OnGraphicsDeviceReset();
    }

    /// <summary>
    ///     Called by the MonoGame framework after the <see cref="Game"/> instance
    ///     is created.  The derived game class should override this method to 
    ///     perform any initializations here, but ensure that base.Initialize() is 
    ///     still called first so TinyEngine can Initialize as well.
    /// </summary>
    protected override void Initialize()
    {
        base.Initialize();

        Input.Initialize();
    }

    /// <summary>
    ///     Called by the MonoGame framework during <see cref="Initialize"/> method 
    ///     execution.  The dervied game calss shoudl override this method to perform
    ///     any content loading here, ut ensure that base.LoadContent() is still called
    ///     so the TinyEngine can load its content as well.
    /// </summary>
    protected override void LoadContent()
    {
        base.LoadContent();

        SpriteBatch = new SpriteBatch(GraphicsDevice);
        SpriteBatchExtensions.Initialize(GraphicsDevice);
    }

    /// <summary>
    ///     Called by the MonoGame framework. All update logic for the game should be
    ///     perforemd here.  TinyEngine uses a scene system and will call the scene update
    ///     method from here. The derived game class shouldn't need to override this method.
    /// </summary>
    /// <param name="gameTime">
    ///     A <see cref="GameTime"/> instance containing a snapshot
    ///     of the timing values provided by the MonOGame framework during
    ///     an update cycle.
    /// </param>
    protected override void Update(GameTime gameTime)
    {
        //  Time should always be the first thing updates
        Time.Update(gameTime);

        _fps.Update();

        //  Update the input state
        Input.Update();

        //  Update the scenes
        Scenes?.Update();

        //  Always ensure we call base.Update() at the end of this.
        base.Update(gameTime);
    }

    /// <summary>
    ///     Renders the game.
    /// </summary>
    /// <param name="gameTime">
    ///     A <see cref="GameTime"/> instance containing a snapshot
    ///     of the timing values provided by the MonOGame framework during
    ///     an draw cycle.
    /// </param>
    protected override void Draw(GameTime gameTime)
    {
        if (Scenes == null)
        {
            throw new InvalidOperationException("Engine.Scenes is null");
        }

        //  Render the scenes.  Scenes render to their own internal 
        //  render targets.
        Scenes.Render();

        //  Prepare the graphics device for the screen render
        Graphics.SetViewport();
        Graphics.Clear();

        //  Derrived classes of Engine can specify something to always
        //  draw here
        DrawAlways();

        //  Being the sprite batch
        SpriteBatch?.Begin(blendState: BlendState.AlphaBlend,
                           samplerState: SamplerState.LinearClamp,
                           transformMatrix: Graphics.ScreenScaleMatrix);

        //  If there is an active transition, we draw it's render target;
        //  otherwise, we draw the render target of the active scene
        if (Scenes.ActiveTransition != null && Scenes.ActiveTransition.IsTransitioning)
        {
            SpriteBatch?.Draw(texture: Scenes.ActiveTransition.RenderTarget,
                              destinationRectangle: Scenes.ActiveTransition.RenderTarget.Bounds,
                              sourceRectangle: Scenes.ActiveTransition.RenderTarget.Bounds,
                              color: Color.White);
        }
        else if (Scenes.ActiveScene != null && Scenes.ActiveScene.RenderTarget != null)
        {
            SpriteBatch?.Draw(texture: Scenes.ActiveScene.RenderTarget,
                              destinationRectangle: Scenes.ActiveScene.RenderTarget.Bounds,
                              sourceRectangle: Scenes.ActiveScene.RenderTarget.Bounds,
                              color: Color.White);
        }

        //  End the sprite batch
        SpriteBatch?.End();

#if DEBUG
        //  Update the FPS counter
        _fps.UpdateCounter();

        if (_fps.HasUpdate)
        {
            float memoryUsage = GC.GetTotalMemory(false) / 1048576.0f;
            Window.Title = $"{Title} | {_fps.FrameRate}fps | {memoryUsage:F}MB";
        }
#endif

        //  Always ensure we call base.Render().
        base.Draw(gameTime);

    }

    protected virtual void DrawAlways() { }

}

