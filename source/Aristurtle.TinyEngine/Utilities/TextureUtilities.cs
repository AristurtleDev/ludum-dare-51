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

public static class TextureUtilities
{
    public static Texture2D FromFile(GraphicsDevice device, string path, bool preMultiplyAlpha = true)
    {
        Texture2D texture;

        using (Stream stream = FileUtilities.OpenStream(path))
        {
            texture = FromStream(device, stream, preMultiplyAlpha);
        }

        return texture;
    }


    public static void PremultiplyAlpha(this Texture2D texture)
    {
        Color[] buffer = new Color[texture.Width * texture.Height];
        texture.GetData<Color>(buffer);

        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = Color.FromNonPremultiplied(buffer[i].R, buffer[i].G, buffer[i].B, buffer[i].A);
        }

        texture.SetData<Color>(buffer);
    }

    public static Texture2D FromStream(GraphicsDevice device, Stream stream, bool preMultiplyAlpha = true)
    {

        Texture2D texture = Texture2D.FromStream(device, stream);

        if (preMultiplyAlpha)
        {
            Color[] buffer = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(buffer);

            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Color.FromNonPremultiplied(buffer[i].R, buffer[i].G, buffer[i].B, buffer[i].A);
            }



            texture.SetData<Color>(buffer);
        }

        return texture;
    }
}
