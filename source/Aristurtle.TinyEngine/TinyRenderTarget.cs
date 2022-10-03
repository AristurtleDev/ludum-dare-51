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

/// <summary>
///     Represents a virtual render target that can be used for rendering to.
/// </summary>
public class TinyRenderTarget : IDisposable
{
    //  The Rendertarget2D instance represented
    private RenderTarget2D? _renderTarget;

    /// <summary>
    ///     Gets a <see cref="bool"/> value indicating if this instance
    ///     has been disposed of.
    /// </summary>
    public bool IsDisposed { get; private set; }

    /// <summary>
    ///     Gets a <see cref="int"/> value that describes the width,
    ///     in pixels, of this render target.
    /// </summary>
    public int Width { get; }

    /// <summary>
    ///     Gets a <see cref="int"/> value that describes the height,
    ///     in pixels, of this render target.
    /// </summary>
    public int Height { get; }

    /// <summary>
    ///     Gets a <see cref="int"/> value that describes the number
    ///     of sample counts to perform on the texture for MSAA
    /// </summary>
    public int MultiSampleCount { get; }

    /// <summary>
    ///     Gets a <see cref="bool"/> value that indicates if depth
    ///     is enabled for this target.
    /// </summary>
    public bool DepthEnabled { get; }

    /// <summary>
    ///     Gets a <see cref="bool"/> value that indicates if the
    ///     contents of the target should be preserved.
    /// </summary>
    public bool Preserve { get; }

    /// <summary>
    ///     Gets a <see cref="Rectangle"/> value that describes the
    ///     bounds of this target.
    /// </summary>
    public Rectangle Bounds => _renderTarget?.Bounds ?? Rectangle.Empty;

    /// <summary>
    ///     Creates a new <see cref="TinyRenderTarget"/> instance.
    /// </summary>
    /// <param name="width">
    ///     A <see cref="int"/> value that describes the width,
    ///     in pixels, of this render target.
    /// </param>
    /// <param name="height">
    ///     A <see cref="int"/> value that describes the height,
    ///     in pixels, of this render target.
    /// </param>
    /// <param name="multiSampleCount">
    ///     A <see cref="int"/> value that describes the number
    ///     of sample counts to perform on the texture for MSAA
    /// </param>
    /// <param name="depth">
    ///     A <see cref="bool"/> value that indicates if depth
    ///     is enabled for this target.
    /// </param>
    /// <param name="preserve">
    ///     A <see cref="bool"/> value that indicates if the
    ///     contents of the target should be preserved.
    /// </param>
    public TinyRenderTarget(int width, int height, int multiSampleCount, bool depth, bool preserve)
    {
        Width = width;
        Height = height;
        MultiSampleCount = multiSampleCount;
        DepthEnabled = depth;
        Preserve = preserve;
        CreateRenderTarget();
    }

    //  Finalizer implementation to internally call Dipose passing false.
    ~TinyRenderTarget() => Dispose(false);

    [MemberNotNull(nameof(_renderTarget))]
    private void CreateRenderTarget()
    {
        _renderTarget = new RenderTarget2D(graphicsDevice: Engine.Graphics.Device,
                                           width: Width,
                                           height: Height,
                                           mipMap: false,
                                           preferredFormat: SurfaceFormat.Color,
                                           preferredDepthFormat: DepthEnabled ? DepthFormat.Depth24Stencil8 : DepthFormat.None,
                                           preferredMultiSampleCount: MultiSampleCount,
                                           usage: Preserve ? RenderTargetUsage.PreserveContents : RenderTargetUsage.DiscardContents);
    }

    /// <summary>
    ///     Reloads this <see cref="TinyRenderTarget"/>. This should be called whenever the
    ///     contents of VRAM are discarded and the target needs to be recreated.
    /// </summary>
    [MemberNotNull(nameof(_renderTarget))]
    public void Reload()
    {
        Unload();
        CreateRenderTarget();
    }

    /// <summary>
    ///     Unloads the <see cref="TinyRenderTarget"/> by disposing of the base
    ///     render target instance.
    /// </summary>
    public void Unload()
    {
        if (_renderTarget != null && !_renderTarget.IsDisposed)
        {
            _renderTarget.Dispose();
            _renderTarget = null;
        }
    }

    /// <summary>
    ///     Diposes of resources managed by this instance.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Disposes of resources managed by this instance.
    /// </summary>
    /// <param name="isDisposing">
    ///     A <see cref="bool"/> value that indicates if resources should
    ///     be disposed of.
    /// </param>
    private void Dispose(bool isDisposing)
    {
        if (IsDisposed)
        {
            return;
        }

        if (isDisposing)
        {
            if (_renderTarget != null && !_renderTarget.IsDisposed)
            {
                _renderTarget.Dispose();
                _renderTarget = null;
            }
        }

        IsDisposed = true;
    }

    /// <summary>
    ///     Allows implicit conversion of a <see cref="TinyRenderTarget"/> to
    ///     a <see cref="RenderTarget2D"/> instance.
    /// </summary>
    /// <param name="target">
    ///     The <see cref="TinyRenderTarget"/> instnace to convert.
    /// </param>
    public static implicit operator RenderTarget2D(TinyRenderTarget target)
    {
        if (target._renderTarget == null)
        {
            throw new InvalidOperationException("Underlying render target of TinyRenderTarget is null");
        }
        
        return target._renderTarget;
    }
}

