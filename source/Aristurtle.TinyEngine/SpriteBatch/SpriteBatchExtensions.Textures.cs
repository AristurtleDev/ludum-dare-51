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

namespace Aristurtle.TinyEngine;

public static partial class SpriteBatchExtensions
{
    /// <summary>
    ///     Draws a <see cref="TinyTexture"/> instnace
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="SpriteBatch"/> instance being used for rendering.
    /// </param>
    /// <param name="texture">
    ///     A <see cref="TinyTexture"/> instance containing the texture and values to
    ///     used for rendering.
    /// </param>
    /// <param name="position">
    ///     A <see cref="Vector2"/> value that defines the top-left xy-coordinate location
    ///     to draw the texture.
    /// </param>
    public static void Draw(this SpriteBatch spriteBatch, TinyTexture texture, Vector2 position)
    {
        spriteBatch.Draw(texture, position, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
    }

    /// <summary>
    ///     Draws a <see cref="TinyTexture"/> instnace
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="SpriteBatch"/> instance being used for rendering.
    /// </param>
    /// <param name="texture">
    ///     A <see cref="TinyTexture"/> instance containing the texture and values to
    ///     used for rendering.
    /// </param>
    /// <param name="position">
    ///     A <see cref="Vector2"/> value that defines the top-left xy-coordinate location
    ///     to draw the texture.
    /// </param>
    /// <param name="color">
    ///     A <see cref="Color"/> value that defines the color mask to apply to the
    ///     texture when drawing.
    /// </param>
    public static void Draw(this SpriteBatch spriteBatch, TinyTexture texture, Vector2 position, Color color)
    {
        spriteBatch.Draw(texture, position, color, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
    }

    /// <summary>
    ///     Draws a <see cref="TinyTexture"/> instnace
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="SpriteBatch"/> instance being used for rendering.
    /// </param>
    /// <param name="texture">
    ///     A <see cref="TinyTexture"/> instance containing the texture and values to
    ///     used for rendering.
    /// </param>
    /// <param name="position">
    ///     A <see cref="Vector2"/> value that defines the top-left xy-coordinate location
    ///     to draw the texture.
    /// </param>
    /// <param name="color">
    ///     A <see cref="Color"/> value that defines the color mask to apply to the
    ///     texture when drawing.
    /// </param>
    /// <param name="rotation">
    ///     A <see cref="float"/> value that defines the angle of rotation, in radians,
    ///     to use when rendering.
    /// </param>
    /// <param name="origin">
    ///     A <see cref="Vector2"/> value that defines the center of rotation when
    ///     drawing the texture.
    /// </param>
    public static void Draw(this SpriteBatch spriteBatch, TinyTexture texture, Vector2 position, Color color, float rotation, Vector2 origin)
    {
        spriteBatch.Draw(texture, position, color, rotation, origin, Vector2.One, SpriteEffects.None, 0.0f);
    }

    /// <summary>
    ///     Draws a <see cref="TinyTexture"/> instnace
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="SpriteBatch"/> instance being used for rendering.
    /// </param>
    /// <param name="texture">
    ///     A <see cref="TinyTexture"/> instance containing the texture and values to
    ///     used for rendering.
    /// </param>
    /// <param name="position">
    ///     A <see cref="Vector2"/> value that defines the top-left xy-coordinate location
    ///     to draw the texture.
    /// </param>
    /// <param name="color">
    ///     A <see cref="Color"/> value that defines the color mask to apply to the
    ///     texture when drawing.
    /// </param>
    /// <param name="rotation">
    ///     A <see cref="float"/> value that defines the angle of rotation, in radians,
    ///     to use when rendering.
    /// </param>
    /// <param name="origin">
    ///     A <see cref="Vector2"/> value that defines the center of rotation when
    ///     drawing the texture.
    /// </param>
    /// <param name="scale">
    ///     A <see cref="Vector2"/> value that defines the scale at which to draw
    ///     the texture on the x and y axes.
    /// </param>
    public static void Draw(this SpriteBatch spriteBatch, TinyTexture texture, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale)
    {
        spriteBatch.Draw(texture, position, color, rotation, origin, scale, SpriteEffects.None, 0.0f);
    }

    /// <summary>
    ///     Draws a <see cref="TinyTexture"/> instnace
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="SpriteBatch"/> instance being used for rendering.
    /// </param>
    /// <param name="texture">
    ///     A <see cref="TinyTexture"/> instance containing the texture and values to
    ///     used for rendering.
    /// </param>
    /// <param name="position">
    ///     A <see cref="Vector2"/> value that defines the top-left xy-coordinate location
    ///     to draw the texture.
    /// </param>
    /// <param name="color">
    ///     A <see cref="Color"/> value that defines the color mask to apply to the
    ///     texture when drawing.
    /// </param>
    /// <param name="rotation">
    ///     A <see cref="float"/> value that defines the angle of rotation, in radians,
    ///     to use when rendering.
    /// </param>
    /// <param name="origin">
    ///     A <see cref="Vector2"/> value that defines the center of rotation when
    ///     drawing the texture.
    /// </param>
    /// <param name="scale">
    ///     A <see cref="Vector2"/> value that defines the scale at which to draw
    ///     the texture on the x and y axes.
    /// </param>
    /// <param name="effects">
    ///     A <see cref="SpriteEffects"/> value that defines if the texture if flipped
    ///     on the horizontal and/or vertical axis when rendered.
    /// </param>
    public static void Draw(this SpriteBatch spriteBatch, TinyTexture texture, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects)
    {
        spriteBatch.Draw(texture, position, color, rotation, origin, scale, effects, 0.0f);
    }

    /// <summary>
    ///     Draws a <see cref="TinyTexture"/> instnace
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="SpriteBatch"/> instance being used for rendering.
    /// </param>
    /// <param name="texture">
    ///     A <see cref="TinyTexture"/> instance containing the texture and values to
    ///     used for rendering.
    /// </param>
    /// <param name="position">
    ///     A <see cref="Vector2"/> value that defines the top-left xy-coordinate location
    ///     to draw the texture.
    /// </param>
    /// <param name="color">
    ///     A <see cref="Color"/> value that defines the color mask to apply to the
    ///     texture when drawing.
    /// </param>
    /// <param name="rotation">
    ///     A <see cref="float"/> value that defines the angle of rotation, in radians,
    ///     to use when rendering.
    /// </param>
    /// <param name="origin">
    ///     A <see cref="Vector2"/> value that defines the center of rotation when
    ///     drawing the texture.
    /// </param>
    /// <param name="scale">
    ///     A <see cref="Vector2"/> value that defines the scale at which to draw
    ///     the texture on the x and y axes.
    /// </param>
    /// <param name="effects">
    ///     A <see cref="SpriteEffects"/> value that defines if the texture if flipped
    ///     on the horizontal and/or vertical axis when rendered.
    /// </param>
    /// <param name="layerDepth">
    ///     A <see cref="float"/> value that defines the z-buffer depth at which to
    ///     draw the texture.
    /// </param>
    public static void Draw(this SpriteBatch spriteBatch, TinyTexture texture, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
    {
        spriteBatch.Draw(texture.Texture, position, texture.SourceRectangle, color, rotation, origin, scale, effects, layerDepth);
    }
}
