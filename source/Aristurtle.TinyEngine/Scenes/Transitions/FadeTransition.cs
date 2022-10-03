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

namespace Aristurtle.TinyEngine;

/// <summary>
///     A transition that fades the scene out/in.
/// </summary>
public class FadeTransition : SceneTransition
{
    /// <summary>
    ///     Creates a new <see cref="FadeTransition"/> instance with with
    ///     a default time of 1 second.
    /// </summary>
    public FadeTransition() : this(TimeSpan.FromSeconds(1)) { }

    /// <summary>
    ///     Creates a new <see cref="FadeTransition"/> instance.
    /// </summary>
    /// <param name="transitionTime">
    ///     A <see cref="TimeSpan"/> value that represents the total amount of
    ///     time this transition should take to complete.
    /// </param>
    public FadeTransition(TimeSpan transitionTime) : base(transitionTime) { }

    /// <summary>
    ///     Draws this transition.
    /// </summary>
    protected override void Draw()
    {
        Engine.Graphics.SpriteBatch.Draw(texture: SceneRenderTarget,
                                         destinationRectangle: SceneRenderTarget.Bounds,
                                         sourceRectangle: SceneRenderTarget.Bounds,
                                         color: Color.White * GetAlpha());
    }

    /// <summary>
    ///     Gets the alpha value to use for the color mask when rendering.
    /// </summary>
    /// <returns>
    ///     The value to use for the color mask alpha
    /// </returns>
    private float GetAlpha()
    {
        double timeLeft = TransitionTimeRemaining.TotalSeconds;

        if (Kind == SceneTransitionKind.Out)
        {
            return (float)(timeLeft / TransitionTime.TotalSeconds);
        }
        else
        {
            return (float)(1.0 - (timeLeft / TransitionTime.TotalSeconds));
        }
    }
}
