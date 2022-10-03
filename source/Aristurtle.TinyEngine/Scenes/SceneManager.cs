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

using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Aristurtle.TinyEngine;

public class SceneManager
{
    public Scene? ActiveScene { get; private set; }
    public Scene? NextScene { get; private set; }
    public SceneTransition? TransitionOut { get; private set; }
    public SceneTransition? TransitionIn { get; private set; }
    public SceneTransition? ActiveTransition { get; private set; }

    internal SceneManager() { }

    internal void Update()
    {
        //  If there is an active transition, then we need to update that;
        //  otherwise if there is no active transition, but there is a next
        //  scene to switch too, switch to that scene instead.
        if (ActiveTransition != null && ActiveTransition.IsTransitioning)
        {
            ActiveTransition.Update();
        }
        else if (ActiveTransition == null && NextScene != null)
        {
            TransitionScene();
            ActiveScene.Begin();
        }

        ActiveScene?.Update();
    }

    internal void Render()
    {
        //  If there is an active scene, render it
        ActiveScene?.Draw();

        //  If there is an active transition happening, render the transition
        if (ActiveTransition != null && ActiveTransition.IsTransitioning)
        {
            ActiveTransition.Render();
        }
    }

    [MemberNotNull(nameof(NextScene))]
    public void ChangeScene([DisallowNull] Scene to)
    {
        if (ActiveScene == to)
        {
            throw new InvalidOperationException("A scene cannot change from itself to itself");
        }

        NextScene = to;
    }

    public void ChangeScene(Scene to, SceneTransition? tOut, SceneTransition? tIn)
    {
        if (ActiveScene == to)
        {
            throw new InvalidOperationException("A scene cannot change from itself to itself");
        }

        if (ActiveTransition != null && ActiveTransition.IsTransitioning)
        {
            return;
            // throw new InvalidOperationException("Cannot change scene in the middel of a transition");
        }

        NextScene = to;
        TransitionOut = tOut;

        if (TransitionOut != null)
        {
            TransitionOut.Kind = SceneTransitionKind.Out;
            TransitionOut.TransitionCompleted += TransitionOutCompleted;
        }

        TransitionIn = tIn;

        if (TransitionIn != null)
        {
            TransitionIn.Kind = SceneTransitionKind.In;
            TransitionIn.TransitionCompleted += TransitionInCompleted;
        }

        //  If we were given a out transition, set it as the active transition
        if(TransitionOut != null)
        {
            ActiveTransition = TransitionOut;

            //  If there is an active scene, start the out transition with the
            //  active scene's render target
            if(ActiveScene != null)
            {
                ActiveTransition.Start(ActiveScene.RenderTarget);
            }
        }
        else
        {
            //  There was no transition out given, was there a transition in
            //  given?
            if(TransitionIn != null)
            {
                //  Transition the scene immediatly
                TransitionScene();

                //  Set the active transition as the in transition
                ActiveTransition = TransitionIn;

                //  Start the active transition
                ActiveTransition.Start(ActiveScene.RenderTarget);
            }
        }
    }

    private void TransitionOutCompleted(object? sender, EventArgs e)
    {
        //  Unsubscribe from the event
        if(TransitionOut != null)
        {
            TransitionOut.TransitionCompleted -= TransitionOutCompleted;

            //  Dispose of the instance
            TransitionOut.Dispose();
            TransitionOut = null;
        }

        //  Out transition done, change the scene
        TransitionScene();

        //  Is there a transition in?
        if(TransitionIn != null)
        {
            ActiveTransition = TransitionIn;
            ActiveTransition.Start(ActiveScene.RenderTarget);
        }
        else
        {
            ActiveTransition = null;
            ActiveScene.Begin();
        }
    }

    private void TransitionInCompleted(object? sender, EventArgs e)
    {
        if(TransitionIn != null)
        {
            //  Unsubscribe from the event
            TransitionIn.TransitionCompleted -= TransitionInCompleted;

            //  Dispose of it
            TransitionIn.Dispose();
            TransitionIn = null;
        }

        //  There should be no more transitions after an in transiton completes.
        //  So we can null out the active
        ActiveTransition = null;

        //  Tell the active scene to beging
        ActiveScene?.Begin();
    }

    [MemberNotNull(nameof(ActiveScene))]
    internal void TransitionScene()
    {
        //  If there is an active scene, unload the content from it
        ActiveScene?.UnloadContent();

        //  Perform garbge collection
        GC.Collect();

        if (NextScene == null)
        {
            throw new InvalidOperationException("Attempting to transition scene but there is no scene to transition too");
        }

        //  Set the active scene to the next scene
        ActiveScene = NextScene;

        //  Reset the time rate
        Engine.Time.TimeRate = 1.0f;

        //  Null out the NextScene since we don't have one anymore
        NextScene = null;

        //  Initialize the active scene
        ActiveScene.Initialize();
    }

    internal void OnClientSizeChanged()
    {
        ActiveScene?.HandleClientSizeChanged();
        NextScene?.HandleClientSizeChanged();
        TransitionOut?.HandleClientSizeChanged();
        TransitionIn?.HandleClientSizeChanged();
    }

    internal void OnGraphicsDeviceCreated()
    {
        ActiveScene?.HandleGraphicsDeviceCreated();
        NextScene?.HandleGraphicsDeviceCreated();
        TransitionOut?.HandleGraphicsDeviceCreated();
        TransitionIn?.HandleGraphicsDeviceCreated();
    }

    internal void OnGraphicsDeviceReset()
    {
        ActiveScene?.HandleGraphicsDeviceReset();
        NextScene?.HandleGraphicsDeviceReset();
        TransitionOut?.HandleGraphicsDeviceReset();
        TransitionIn?.HandleGraphicsDeviceReset();
    }

}