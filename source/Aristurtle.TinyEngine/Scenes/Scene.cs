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
using Microsoft.Xna.Framework.Content;

namespace Aristurtle.TinyEngine;

/// <summary>
///     An abstract class used to create scenes within the game.
/// </summary>
public abstract class Scene
{

    private TinyRenderTarget? _renderTarget;
    private ContentManager? _content;

    /// <summary>
    ///     Gets the <see cref="TinyRenderTarget"/> instance that the scene
    ///     renders to.
    /// </summary>
    public TinyRenderTarget RenderTarget
    {
        get
        {
            if(_renderTarget == null || _renderTarget.IsDisposed)
            {
                throw new InvalidOperationException("Scene rendertarget is null");
            }

            return _renderTarget;
        }
    }


    /// <summary>
    ///     Gets or Sets a <see cref="bool"/> value indicating if this
    ///     scene is paused. 
    /// </summary>
    /// <remarks>
    ///     When paused, the update method will be skipped for this scene.
    /// </remarks>
    public bool ScenePaused { get; set; }

    /// <summary>
    ///     Gets the <see cref="ContentManager"/> instance used to load content
    ///     specific for this scene. 
    /// </summary>
    /// <remarks>
    ///     Any content loaded through this <see cref="ContentManager"/> will
    ///     be unloaded when switch from this scene to another scene.s
    /// </remarks>
    public ContentManager Content
    {
        get
        {
            if(_content == null)
            {
                throw new InvalidOperationException("Scene.Content is null");
            }

            return _content;
        }
    }

    /// <summary>
    ///     Creates a new <see cref="Scene"/> instance.
    /// </summary>
    public Scene()
    {
        _content = new(Engine.GlobalContent.ServiceProvider);
        Content.RootDirectory = Engine.GlobalContent.RootDirectory;
        ScenePaused = true;
    }

    /// <summary>
    ///     Called internally by TinyEngine on the first update frame
    ///     after the scene has been fully transitioned into.
    /// </summary>
    public virtual void Begin()
    {
        ScenePaused = false;
        Start();
    }

    /// <summary>
    ///     Perform any logic that should occur at the start of the scene here.
    /// </summary>
    /// <remarks>
    ///     This is called only once, on the first frame after the scene has been
    ///     fully transitioned into, but before the first update for the scene
    ///     has occured.
    /// </remarks>
    public virtual void Start() { }

    /// <summary>
    ///     Perform all scene initilizations here. 
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         When overriding this method, ensure that <c>base.Initialize()</c> is called first.
    ///     </para>
    ///     <para>
    ///         This is called only once, immediatly after the scene becomes the active scene,
    ///         and before the first update is called for the scene.
    ///     </para>
    /// </remarks>
    public virtual void Initialize()
    {
        LoadContent();
    }

    /// <summary>
    ///     Perform any loading of scene specific content here.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         When overriding this method, ensure that <c>base.LoadContent()</c> is still called.
    ///         This is to allow the <see cref="TinyRenderTarget"/> value to instantiate.
    ///     </para>
    ///     <para>
    ///         This is called only once, immediately at the end of the 
    ///         <see cref="Initialize"/> method.
    ///     </para>
    /// </remarks>
    public virtual void LoadContent()
    {
        _renderTarget = new TinyRenderTarget(width: Engine.Graphics.Resolution.X,
                                            height: Engine.Graphics.Resolution.Y,
                                            multiSampleCount: 0,
                                            depth: true,
                                            preserve: true);
    }

    /// <summary>
    ///     Perform any unloading of scene specific content here.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         When overriding this method, ensure that <c>base.UnloadContent()</c> is still
    ///         called so that the content manager for the scene can be unloaded.
    ///     </para>
    ///     <para>
    ///         This is called only once, immediatly after TinyEngine switches to a new scene
    ///         from this scene. 
    ///     </para>
    /// </remarks>
    public virtual void UnloadContent()
    {
        _content?.Dispose();
        _content = null;

        //  Dispose of the render target if it is not already dispoed
        if (_renderTarget != null && !_renderTarget.IsDisposed)
        {
            _renderTarget.Dispose();
            _renderTarget = null;
        }
    }

    /// <summary>
    ///     Perform any update logic for the scene here.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method is called each update cycle, and only once per.
    ///     </para>
    /// </remarks>
    public virtual void Update() { }

    /// <summary>
    ///     Perofrm all drawing for the scene here.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method is called each draw cycle, and only once per.
    ///     </para>
    /// </remarks>
    public virtual void Draw() { }

    /// <summary>
    ///     Gets the <see cref="Game"/> instance as <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">
    ///     The derived <see cref="Game"/> type.
    /// </typeparam>
    /// <returns>
    ///     The <see cref="Game"/> instance as <typeparamref name="T"/>
    /// </returns>
    public T? GameAs<T>() where T : Game
    {
        return Engine.Instance as T;
    }

    /// <summary>
    ///     Changes the current active <see cref="Scene"/> to the one provided.
    /// </summary>
    /// <param name="to">
    ///     The <see cref="Scene"/> instance to change to.
    /// </param>
    public void ChangeScene(Scene to)
    {
        Engine.Scenes.ChangeScene(to);
    }

    /// <summary>
    ///     Changes the current active <see cref="Scene"/> to the one provided using
    ///     the <see cref="SceneTransition"/> instances given to transition the scenes
    ///     in and out.
    /// </summary>
    /// <param name="to">
    ///     The <see cref="Scene"/> instance to change to.
    /// </param>
    /// <param name="transitionOut">
    ///     A <see cref="SceneTransition"/> instance that is used to transition the current
    ///     active <see cref="Scene"/> out.
    /// </param>
    /// <param name="transitionIn">
    ///     A <see cref="SceneTransition"/> instance that is used to transition next
    ///     <see cref="Scene"/> in.
    /// </param>
    public void ChangeScene(Scene to, SceneTransition transitionOut, SceneTransition transitionIn)
    {
        Engine.Scenes.ChangeScene(to, transitionOut, transitionIn);
    }

    /// <summary>
    ///     Handles the <see cref="GraphicsDeviceManager.DeviceCreated"/> event for the
    ///     scene.
    /// </summary>
    public virtual void HandleGraphicsDeviceCreated()
    {
        RenderTarget.Reload();
    }

    /// <summary>
    ///     Handles the <see cref="GraphicsDeviceManager.DeviceReset"/> event for the
    ///     scene.
    /// </summary>
    public virtual void HandleGraphicsDeviceReset()
    {
        RenderTarget.Reload();
    }

    /// <summary>
    ///     Handles the <see cref="GameWindow.ClientSizeChanged"/> event for the
    ///     scene.
    /// </summary>
    public virtual void HandleClientSizeChanged() { }
}
