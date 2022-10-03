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

using Microsoft.Xna.Framework.Graphics;

namespace Aristurtle.TinyEngine;

/// <summary>
///     Provieds the values for the <see cref="Graphics"/> instance when it is
///     created.
/// </summary>
/// <param name="SynchronizeWithVerticalRetrace">
///     Whether vsync should be used.
/// </param>
/// <param name="PreferMultiSampling">
///     Whether multi-sampling should be used for the backbuffer.
/// </param>
/// <param name="GraphicsProfile">
///     The graphics profile to use.
/// </param>
/// <param name="PreferredBackBufferFormat">
///     The desired surface format for the backbuffer.
/// </param>
/// <param name="PreferredDepthStencilFormat">
///     The desired depth stencil format.
/// </param>
/// <param name="AllowUserResizeWindow">
///     Whether users should be able to resize the game window.
/// </param>
/// <param name="IsMouseVisible">
///     Whether the mouse should be visible on the game window.
/// </param>
public record GraphicsOptions(bool SynchronizeWithVerticalRetrace = true,
                              bool PreferMultiSampling = false,
                              GraphicsProfile GraphicsProfile = GraphicsProfile.HiDef,
                              SurfaceFormat PreferredBackBufferFormat = SurfaceFormat.Color,
                              DepthFormat PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8,
                              bool AllowUserResizeWindow = false,
                              bool IsMouseVisible = true);
