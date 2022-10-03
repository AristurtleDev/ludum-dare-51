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

public static class ColorUtilities
{
    public static Color Invert(this Color color)
    {
        return new Color(255 - color.R, 255 - color.G, 255 - color.B, color.A);
    }

    public static Color HexToRGBA(string hex)
    {
        const string HEX = "0123456789ABCDEF";
        byte toByte(char c) => (byte)HEX.IndexOf(char.ToUpper(c));

        if (hex[0] == '#')
        {
            hex = hex.Substring(1);
        }

        int len = hex.Length;

        float r, g, b, a;
        r = g = b = a = 1.0f;

        if (len == 3)
        {
            r = (toByte(hex[0]) * 16 + toByte(hex[0])) / 255.0f;
            g = (toByte(hex[1]) * 16 + toByte(hex[1])) / 255.0f;
            b = (toByte(hex[2]) * 16 + toByte(hex[2])) / 255.0f;
        }
        else if (len >= 6)
        {
            r = (toByte(hex[0]) * 16 + toByte(hex[1])) / 255.0f;
            g = (toByte(hex[2]) * 16 + toByte(hex[3])) / 255.0f;
            b = (toByte(hex[4]) * 16 + toByte(hex[5])) / 255.0f;

            if (len == 8)
            {
                a = (toByte(hex[6]) * 16 + toByte(hex[7])) / 255.0f;
            }
        }

        return new Color(r, g, b, a);
    }
}