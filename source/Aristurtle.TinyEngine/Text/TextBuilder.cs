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

using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Aristurtle.TinyEngine;

public class TextBuilder
{
    private List<Text> _text;
    private Vector2 _position;
    private Vector2 _size;
    private Vector2 _origin;
    private Vector2 _renderPosition;

    public Vector2 Position
    {
        get { return _position; }
        set
        {
            if (_position.Equals(value)) { return; }
            _position = value;
            SetRenderPosition();
        }
    }

    public Vector2 Origin
    {
        get { return _origin; }
        set
        {
            if (_origin.Equals(value)) { return; }
            _origin = value;
            SetRenderPosition();
        }
    }

    public Vector2 Size => _size;

    public ReadOnlyCollection<Text> Text { get; private set; }

    public TextBuilder() : this(Vector2.Zero) { }

    public TextBuilder(Vector2 position)
    {
        _text = new List<Text>();
        Text = _text.AsReadOnly();

        _position = position;
        SetSize();
        SetRenderPosition();
    }

    public void Add(SpriteFont font, string text)
    {
        Add(font, text, Color.White);
    }

    public void Add(SpriteFont font, string text, Color color)
    {
        Text toAdd = new Text(font, text, color);

        _text.Add(toAdd);
        SetSize();
        SetRenderPosition();
    }

    public void Add(SpriteFont font, string text, Color color, float scale)
    {
        Text toAdd = new Text(font, text, color);
        toAdd.Scale = new Vector2(scale, scale);
        _text.Add(toAdd);
        SetSize();
        SetRenderPosition();
    }


    public void Clear()
    {
        _text.Clear();
        SetSize();
        SetRenderPosition();
    }

    private void SetRenderPosition()
    {
        _renderPosition.X = _position.X - _origin.X;
        _renderPosition.Y = _position.Y - _origin.Y;

        Vector2 nextPosition = Vector2.Zero;

        for (int i = 0; i < _text.Count; i++)
        {
            Text text = _text[i];

            if (i == 0)
            {
                text.Position = _renderPosition;
            }
            else
            {
                text.Position = nextPosition;
            }

            nextPosition = new Vector2
            {
                X = text.Position.X + text.Size.X,
                Y = _renderPosition.Y
            };
        }
    }

    private void SetSize()
    {
        _size = Vector2.Zero;
        for (int i = 0; i < _text.Count; i++)
        {
            _size.X += _text[i].Size.X;
            _size.Y = (float)Math.Max(_size.Y, _text[i].Size.Y);
        }
    }

}
