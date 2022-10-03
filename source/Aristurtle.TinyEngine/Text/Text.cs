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

/// <summary>
///     A class that represents text to render using the spritebatch.
/// </summary>
public class Text
{
    //  The string value of this text instance.
    private string _value;

    //  The font used when rendering this text.
    // private PixelFontSize _font;
    private SpriteFont _font;

    //  The width and height size of the text disregarding
    //  the scale value given.
    private Vector2 _rawSize;

    //  The width and height size of the text when rendered.
    private Vector2 _size;

    //  Half the size of the text when rendered.
    private Vector2 _halfSize;

    //  The scale to render the text at.
    private Vector2 _scale;

    //  The position to render the text at.
    private Vector2 _position;

    //  The rectangular bounds of this text when rendered.
    private Rectangle _bounds;

    /// <summary>
    ///     Gets or Sets a <see cref="string"/> value that represents
    ///     the text to use when rendering.
    /// </summary>
    public string Value
    {
        get { return _value; }
        set
        {
            if (_value.Equals(value)) { return; }

            _value = value;
            SetSize();
        }
    }

    /// <summary>
    ///     Gets or Sets the <see cref="SpriteFont"/> value to use
    ///     when rendering.
    /// </summary>
    public SpriteFont Font
    {
        get { return _font; }
        set
        {
            if (_font.Equals(value)) { return; }

            _font = value;
            SetSize();
        }
    }

    /// <summary>
    ///     Gets or Sets a <see cref="Vector2"/> value that describes
    ///     the xy-coordinate position to render this at.
    /// </summary>
    public Vector2 Position
    {
        get { return _position; }
        set
        {
            _position = value;
            _position.Round();
            SetBounds();
        }
    }

    /// <summary>
    ///     Gets or Sets a <see cref="float"/> value that describes the
    ///     y-coordiante position to render this at.
    /// </summary>
    public float X
    {
        get { return _position.X; }
        set
        {
            _position.X = value;
            _position.Round();
            SetBounds();
        }
    }

    /// <summary>
    ///     Gets or Sets a <see cref="float"/> value that describes the
    ///     x-coordiante position to render this at.
    /// </summary>
    public float Y
    {
        get { return _position.Y; }
        set
        {
            _position.Y = value;
            _position.Round();
            SetBounds();
        }
    }

    /// <summary>
    ///     Gets a <see cref="Vector2"/> value that describes the width
    ///     and height, in pixels, of this text when rendered.
    /// </summary>
    public Vector2 Size => _size;

    /// <summary>
    ///     Gets a <see cref="Vector2"/> value that describes half the
    ///     width and height, in pixels, of this text when rendered.
    /// </summary>
    public Vector2 HalfMeasurement => _halfSize;

    /// <summary>
    ///     Gets a <see cref="Microsoft.Xna.Framework.Color"/> value that describes the color
    ///     of the text when rendered.
    /// </summary>
    public Color Color { get; set; }

    /// <summary>
    ///     Gets a <see cref="Microsoft.Xna.Framework.Color"/> value that describes the
    ///     color of the outline of the text when rendered.
    /// </summary>
    /// <remarks>
    ///     Outline color is only applied if <see cref="IsOutlined"/> is set to <c>true</c>.
    /// </remarks>
    public Color OutlineColor { get; set; }

    /// <summary>
    ///     Gets a <see cref="bool"/> value that defines if this text should be rendered with
    ///     an outline.
    /// </summary>
    public bool IsOutlined { get; set; }

    /// <summary>
    ///     Gets or Sets a <see cref="Vector2"/> value that desribes the
    ///     scale of the text when rendered on the x and y axes.
    /// </summary>
    public Vector2 Scale
    {
        get { return _scale; }
        set
        {
            if (_scale.Equals(value)) { return; }
            _scale = value;
            SetSize();
        }
    }

    /// <summary>
    ///     Gets or Sets a <see cref="Vector2"/> value that describes the
    ///     center of rotation when rendering this text.
    /// </summary>
    public Vector2 Origin { get; set; }

    /// <summary>
    ///     Gets or Sets a <see cref="float"/> that describes the angle of
    ///     rotation, in radians, to render the text at.
    /// </summary>
    public float Rotation { get; set; }

    /// <summary>
    ///     Gets or Sets a <see cref="SpriteEffects"/> value that describes
    ///     the horizontal and/or vertical flip effect to use when rendering
    ///     this text.
    /// </summary>
    public SpriteEffects Effect { get; set; }

    /// <summary>
    ///     Gets or Sets a <see cref="float"/> value that describes the z-buffer
    ///     depth of the text when rendered.
    /// </summary>
    public float LayerDepth { get; set; }

    /// <summary>
    ///     Gets a <see cref="Rectangle"/> value that describes the bounding area
    ///     of this text when rendered.
    /// </summary>
    public Rectangle Bounds => _bounds;

    /// <summary>
    ///     Creates a new <see cref="Text"/> instance.
    /// </summary>
    /// <param name="font">
    ///     A <see cref="PixelFontSize"/> value that represents the font to
    ///     use when rendering.
    /// </param>
    /// <param name="text">
    ///     A <see cref="string"/> value that represents the text to
    ///     use when rendering.
    /// </param>
    public Text(SpriteFont font, string text)
        : this(font, text, Vector2.Zero, Color.White, 0.0f, Vector2.Zero, Vector2.One) { }

    /// <summary>
    ///     Creates a new <see cref="Text"/> instance.
    /// </summary>
    /// <param name="font">
    ///     A <see cref="PixelFontSize"/> value that represents the font to
    ///     use when rendering.
    /// </param>
    /// <param name="text">
    ///     A <see cref="string"/> value that represents the text to
    ///     use when rendering.
    /// </param>
    /// <param name="color">
    ///     A <see cref="Color"/> value that describes the color of the
    ///     text when rendered.
    /// </param>
    public Text(SpriteFont font, string text, Color color)
        : this(font, text, Vector2.Zero, color, 0.0f, Vector2.Zero, Vector2.One) { }

    /// <summary>
    ///     Creates a new <see cref="Text"/> instance.
    /// </summary>
    /// <param name="font">
    ///     A <see cref="PixelFontSize"/> value that represents the font to
    ///     use when rendering.
    /// </param>
    /// <param name="text">
    ///     A <see cref="string"/> value that represents the text to
    ///     use when rendering.
    /// </param>
    /// <param name="position">
    ///     A <see cref="Vector2"/> value that describes the xy-coordinate
    ///     position to render this at.
    /// </param>
    public Text(SpriteFont font, string text, Vector2 position)
        : this(font, text, position, Color.White, 0.0f, Vector2.Zero, Vector2.One) { }

    /// <summary>
    ///     Creates a new <see cref="Text"/> instance.
    /// </summary>
    /// <param name="font">
    ///     A <see cref="PixelFontSize"/> value that represents the font to
    ///     use when rendering.
    /// </param>
    /// <param name="text">
    ///     A <see cref="string"/> value that represents the text to
    ///     use when rendering.
    /// </param>
    /// <param name="position">
    ///     A <see cref="Vector2"/> value that describes the xy-coordinate
    ///     position to render this at.
    /// </param>
    public Text(SpriteFont font, string text, Vector2 position, Color color)
        : this(font, text, position, color, 0.0f, Vector2.Zero, Vector2.One) { }

    /// <summary>
    ///     Creates a new <see cref="Text"/> instance.
    /// </summary>
    /// <param name="font">
    ///     A <see cref="PixelFontSize"/> value that represents the font to
    ///     use when rendering.
    /// </param>
    /// <param name="text">
    ///     A <see cref="string"/> value that represents the text to
    ///     use when rendering.
    /// </param>
    /// <param name="position">
    ///     A <see cref="Vector2"/> value that describes the xy-coordinate
    ///     position to render this at.
    /// </param>
    /// <param name="color">
    ///     A <see cref="Color"/> value that describes the color of the
    ///     text when rendered.
    /// </param>
    /// <param name="rotation">
    ///     A <see cref="float"/> that describes the angle of rotation,
    ///     in radians, to render the text at.
    /// </param>
    /// <param name="origin">
    ///     A <see cref="Vector2"/> value that describes the center of
    ///     rotation when rendering this text.
    /// </param>
    /// <param name="scale">
    ///     A <see cref="float"/> value that desribes the scale of the
    ///     text when rendered on the x and y axes.
    /// </param>
    public Text(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale)
        : this(font, text, position, color, rotation, origin, new Vector2(scale, scale)) { }

    /// <summary>
    ///     Creates a new <see cref="Text"/> instance.
    /// </summary>
    /// <param name="font">
    ///     A <see cref="PixelFontSize"/> value that represents the font to
    ///     use when rendering.
    /// </param>
    /// <param name="text">
    ///     A <see cref="string"/> value that represents the text to
    ///     use when rendering.
    /// </param>
    /// <param name="position">
    ///     A <see cref="Vector2"/> value that describes the xy-coordinate
    ///     position to render this at.
    /// </param>
    /// <param name="color">
    ///     A <see cref="Color"/> value that describes the color of the
    ///     text when rendered.
    /// </param>
    /// <param name="rotation">
    ///     A <see cref="float"/> that describes the angle of rotation,
    ///     in radians, to render the text at.
    /// </param>
    /// <param name="origin">
    ///     A <see cref="Vector2"/> value that describes the center of
    ///     rotation when rendering this text.
    /// </param>
    /// <param name="scale">
    ///     A <see cref="Vector2"/> value that desribes the scale of the
    ///     text when rendered on the x and y axes.
    /// </param>
    public Text(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale)
    {
        _font = font;
        _value = text;
        Position = position;
        Color = color;
        Rotation = rotation;
        Origin = origin;
        _scale = scale;

        SetSize();
    }

    /// <summary>
    ///     Sets the size and half size measurements of this <see cref="Text"/>.
    /// </summary>
    private void SetSize()
    {
        _rawSize = _font.MeasureString(_value);
        _size = _rawSize * _scale;
        _halfSize = _size * 0.5f;
        SetBounds();
    }

    /// <summary>
    ///     Sets the bounds of this text when rendered.
    /// </summary>
    private void SetBounds()
    {
        _bounds = new Rectangle
        {
            X = (int)Math.Floor(_position.X - (Origin.X * _scale.X)),
            Y = (int)Math.Floor(_position.Y - (Origin.Y * _scale.Y)),
            Width = (int)Math.Floor(_size.X),
            Height = (int)Math.Floor(_size.Y)
        };
    }

    /// <summary>
    ///     Center the <see cref="Origin"/> point of the text.
    /// </summary>
    public void CenterOrigin()
    {
        Origin = _rawSize * 0.5f;
        SetBounds();
    }

    public void CenterLeftOrigin()
    {
        Origin = new Vector2(0, _rawSize.Y) * 0.5f;
        SetBounds();
    }
}
