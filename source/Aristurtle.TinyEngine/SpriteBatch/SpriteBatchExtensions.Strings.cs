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
    ///     Draws a <see cref="Text"/> instance.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="SpriteBatch"/> instance being used for rendering.
    /// </param>
    /// <param name="text">
    ///     A <see cref="Text"/> instance that defines the value and properties of
    ///     the text to be rendered.
    /// </param>
    public static void DrawString(this SpriteBatch spriteBatch, Text text)
    {
        if (text.IsOutlined)
        {
            spriteBatch.DrawStringOutlined(text.Font, text.Value, text.Position, text.Color, text.OutlineColor, text.Origin, text.Scale, text.LayerDepth);
        }
        else
        {
            spriteBatch.DrawString(text.Font, text.Value, text.Position, text.Color, text.Rotation, text.Origin, text.Scale, text.Effect, text.LayerDepth);
            // spriteBatch.DrawString(text.Font, text.Value, text.Position, text.Color, text.Origin, text.Scale, text.LayerDepth);
        }
    }

    /// <summary>
    ///     Draws a <see cref="TextBuilder"/> instance.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="SpriteBatch"/> instance being used for rendering.
    /// </param>
    /// <param name="textBuilder">
    ///     A <see cref="TextBuilder"/> instance that defines the text and properties
    ///     of each text to be rendered.
    /// </param>
    public static void DrawString(this SpriteBatch spriteBatch, TextBuilder textBuilder)
    {
        for (int i = 0; i < textBuilder.Text.Count; i++)
        {
            spriteBatch.DrawString(textBuilder.Text[i]);
        }
    }

    /// <summary>
    ///     Draws a <see cref="string"/>
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="SpriteBatch"/> instance being used for rendering.
    /// </param>
    /// <param name="font">
    ///     A <see cref="SpriteFont"/> instance containing the font information used
    ///     when rendering.
    /// </param>
    /// <param name="text">
    ///     A <see cref="string"/> value that contains the text to draw.
    /// </param>
    /// <param name="position">
    ///     A <see cref="Vector2"/> value that defines the xy-coordinate location
    ///     to draw the text.
    /// </param>
    public static void DrawStringOutlined(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position)
    {
        spriteBatch.DrawStringOutlined(font, text, position, Color.White, Color.Black, Vector2.Zero, Vector2.One, 0.0f);
    }

    /// <summary>
    ///     Draws a <see cref="string"/>
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="SpriteBatch"/> instance being used for rendering.
    /// </param>
    /// <param name="font">
    ///     A <see cref="SpriteFont"/> instance containing the font information used
    ///     when rendering.
    /// </param>
    /// <param name="text">
    ///     A <see cref="string"/> value that contains the text to draw.
    /// </param>
    /// <param name="position">
    ///     A <see cref="Vector2"/> value that defines the xy-coordinate location
    ///     to draw the text.
    /// </param>
    /// <param name="color">
    ///     A <see cref="Color"/> value that defines the color mask to apply to the
    ///     text when drawing.
    /// </param>
    /// <param name="outlineColor">
    ///     A <see cref="Color"/> value that defines the color mask to apply to the
    ///     outline of the text when drawing.
    /// </param>
    public static void DrawStringOutlined(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, Color outlineColor)
    {
        spriteBatch.DrawStringOutlined(font, text, position, color, outlineColor, Vector2.Zero, Vector2.One, 0.0f);
    }

    /// <summary>
    ///     Draws a <see cref="string"/>
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="SpriteBatch"/> instance being used for rendering.
    /// </param>
    /// <param name="font">
    ///     A <see cref="SpriteFont"/> instance containing the font information used
    ///     when rendering.
    /// </param>
    /// <param name="text">
    ///     A <see cref="string"/> value that contains the text to draw.
    /// </param>
    /// <param name="position">
    ///     A <see cref="Vector2"/> value that defines the xy-coordinate location
    ///     to draw the text.
    /// </param>
    /// <param name="color">
    ///     A <see cref="Color"/> value that defines the color mask to apply to the
    ///     text when drawing.
    /// </param>
    /// <param name="outlineColor">
    ///     A <see cref="Color"/> value that defines the color mask to apply to the
    ///     outline of the text when drawing.
    /// </param>
    /// <param name="origin">
    ///     A <see cref="Vector2"/> value that defines the center of rotation when
    ///     drawing the text.
    /// </param>
    public static void DrawStringOutlined(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, Color outlineColor, Vector2 origin)
    {
        spriteBatch.DrawStringOutlined(font, text, position, color, outlineColor, origin, Vector2.One, 0.0f);
    }

    /// <summary>
    ///     Draws a <see cref="string"/>
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="SpriteBatch"/> instance being used for rendering.
    /// </param>
    /// <param name="font">
    ///     A <see cref="SpriteFont"/> instance containing the font information used
    ///     when rendering.
    /// </param>
    /// <param name="text">
    ///     A <see cref="string"/> value that contains the text to draw.
    /// </param>
    /// <param name="position">
    ///     A <see cref="Vector2"/> value that defines the xy-coordinate location
    ///     to draw the text.
    /// </param>
    /// <param name="color">
    ///     A <see cref="Color"/> value that defines the color mask to apply to the
    ///     text when drawing.
    /// </param>
    /// <param name="outlineColor">
    ///     A <see cref="Color"/> value that defines the color mask to apply to the
    ///     outline of the text when drawing.
    /// </param>
    /// <param name="origin">
    ///     A <see cref="Vector2"/> value that defines the center of rotation when
    ///     drawing the text.
    /// </param>
    /// <param name="scale">
    ///     A <see cref="Vector2"/> value that defines the scale at which to draw
    ///     the text on the x and y axes.
    /// </param>
    public static void DrawStringOutlined(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, Color outlineColor, Vector2 origin, Vector2 scale)
    {
        spriteBatch.DrawStringOutlined(font, text, position, color, outlineColor, origin, scale, 0.0f);
    }

    /// <summary>
    ///     Draws a <see cref="string"/>
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="SpriteBatch"/> instance being used for rendering.
    /// </param>
    /// <param name="font">
    ///     A <see cref="SpriteFont"/> instance containing the font information used
    ///     when rendering.
    /// </param>
    /// <param name="text">
    ///     A <see cref="string"/> value that contains the text to draw.
    /// </param>
    /// <param name="position">
    ///     A <see cref="Vector2"/> value that defines the xy-coordinate location
    ///     to draw the text.
    /// </param>
    /// <param name="color">
    ///     A <see cref="Color"/> value that defines the color mask to apply to the
    ///     text when drawing.
    /// </param>
    /// <param name="outlineColor">
    ///     A <see cref="Color"/> value that defines the color mask to apply to the
    ///     outline of the text when drawing.
    /// </param>
    /// <param name="origin">
    ///     A <see cref="Vector2"/> value that defines the center of rotation when
    ///     drawing the text.
    /// </param>
    /// <param name="scale">
    ///     A <see cref="Vector2"/> value that defines the scale at which to draw
    ///     the text on the x and y axes.
    /// </param>
    /// <param name="layerDepth">
    ///     A <see cref="float"/> value that defines the z-buffer depth at which to
    ///     draw the text.
    /// </param>
    public static void DrawStringOutlined(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, Color outlineColor, Vector2 origin, Vector2 scale, float layerDepth)
    {
        //  To prevent artificats/distortions when rendering, the position
        //  of the text will be floored here to an int value. Atleast this is how I'm
        //  handling this.
        position.Floor();

        //  A reusable Vector2 struct for the position of the outline
        Vector2 outlinePos;

        for (int x = -1; x < 2; x += 2)
        {
            for (int y = -1; y < 2; y += 2)
            {
                outlinePos = new Vector2(x, y) + position;
                outlinePos.Floor(); //  Incase some weird floating point issues.
                spriteBatch.DrawString(font, text, outlinePos, outlineColor, 0.0f, origin, scale, SpriteEffects.None, layerDepth);
            }
        }

        //  Finally, draw the actual text on top of the outline renders
        spriteBatch.DrawString(font, text, position, color, 0.0f, origin, scale, SpriteEffects.None, layerDepth);
    }
}
