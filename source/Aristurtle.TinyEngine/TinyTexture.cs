/* ----------------------------------------------------------------------------

    This is based on the MTexture class from the Monocle Engine by Matt Thorson.
    Original repository URL was https://bitbucket.org/MattThorson/monocle-engine
    but no longer seems valid.

    Monocle is licensed under MIT License.

---------------------------------------------------------------------------- */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Aristurtle.TinyEngine;

public class TinyTexture : IDisposable
{
    /// <summary>
    ///     Gets a <see cref="bool"/> value indicating if this instance
    ///     has been disposed of.
    /// </summary>
    public bool IsDisposed { get; private set; }

    /// <summary>
    ///     Gets a <see cref="Texture2D"/> instance that is the base
    ///     texture used by this instance.
    /// </summary>
    public Texture2D? Texture { get; private set; }

    /// <summary>
    ///     Gets a <see cref="Rectangle"/> value that describes the bounds
    ///     within the <see cref="Texture"/> to use when rendeirng.
    /// </summary>
    public Rectangle SourceRectangle { get; }

    /// <summary>
    ///     Gets a <see cref="int"/> value that describes the width of this
    ///     <see cref="TinyTexture"/>.
    /// </summary>
    public int Width => SourceRectangle.Width;

    /// <summary>
    ///     Gets a <see cref="int"/> value that describes the height of this
    ///     <see cref="TinyTexture"/>.
    /// </summary>
    public int Height => SourceRectangle.Height;

    /// <summary>
    ///     Gets a <see cref="float"/> value that defines the top UV coordinate
    ///     of this <see cref="TinyTexture"/>.
    /// </summary>
    public float TopUV { get; private set; }

    /// <summary>
    ///     Gets a <see cref="float"/> value that defines the right UV coordinate
    ///     of this <see cref="TinyTexture"/>.
    /// </summary>
    public float RightUV { get; private set; }

    /// <summary>
    ///     Gets a <see cref="float"/> value that defines the bottom UV coordinate
    ///     of this <see cref="TinyTexture"/>.
    /// </summary>
    public float BottomUV { get; private set; }

    /// <summary>
    ///     Gets a <see cref="float"/> value that defines the left UV coordinate
    ///     of this <see cref="TinyTexture"/>.
    /// </summary>
    public float LeftUV { get; private set; }

    /// <summary>
    ///     Gets a <see cref="Vector2"/> value that defines the center of the
    ///     bounds of this <see cref="TinyTexture"/>.
    /// </summary>
    public Vector2 Center { get; private set; }

    /// <summary>
    ///     Creates a new <see cref="TinyTexture"/> instance.
    /// </summary>
    /// <remarks>
    ///     This constructor will create a new <see cref="Texture2D"/> instance with
    ///     every pixel set to the color provided. If you are looking to create a
    ///     <see cref="TinyTexture"/> instance from an existing <see cref="Texture2D"/>,
    ///     then use one of the other provided constructor methods.
    /// </remarks>
    /// <param name="device">
    ///     The <see cref="GraphicsDevice"/> instance used by the game for the
    ///     presentation of graphics.
    /// </param>
    /// <param name="width">
    ///     A <see cref="int"/> value that decribes the width, in pixels.
    /// </param>
    /// <param name="height">
    ///     A <see cref="int"/> value that describes the height, in pixels.
    /// </param>
    /// <param name="color">
    ///     A <see cref="Color"/> value that describes the color to set
    ///     each pixel to.
    /// </param>
    public TinyTexture(GraphicsDevice device, int width, int height, Color color)
    {
        Texture = new Texture2D(device, width, height);

        Color[] buffer = new Color[width * height];
        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = color;
        }

        Texture.SetData<Color>(buffer);

        SourceRectangle = new Rectangle(0, 0, width, height);
        SetValues();
    }

    /// <summary>
    ///     Creates a new <see cref="TinyTexture"/> instance.
    /// </summary>
    /// <param name="texture">
    ///     An existing <see cref="Texture2D"/> instance.
    /// </param>
    public TinyTexture(Texture2D texture)
    {
        Texture = texture;
        SourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
        SetValues();
    }

    /// <summary>
    ///     Creates a new <see cref="TinyTexture"/> instance.
    /// </summary>
    /// <param name="parent">
    ///     An existing <see cref="TinyTexture"/> instance.
    /// </param>
    /// <param name="x">
    ///     A <see cref="int"/> value that defines the top-left x-coordinate of
    ///     the boundry within the <paramref name="parent"/> that this instance
    ///     represents.
    /// </param>
    /// <param name="y">
    ///     A <see cref="int"/> value that defines the top-left y-coordinate of
    ///     the boundry within the <paramref name="parent"/> that this instance
    ///     represents.
    /// </param>
    /// <param name="width">
    ///     A <see cref="int"/> value that defines the width of the boundry within
    ///     the <paramref name="parent"/> that this instance represents.
    /// </param>
    /// <param name="height">
    ///     A <see cref="int"/> value that defines the height of the boundry within
    ///     the <paramref name="parent"/> that this instance represents.
    /// </param>
    public TinyTexture(TinyTexture parent, int x, int y, int width, int height)
    {
        Texture = parent.Texture;
        SourceRectangle = parent.GetRelativeRect(x, y, width, height);

        SetValues();
    }

    /// <summary>
    ///     Creates a new <see cref="TinyTexture"/> instance.
    /// </summary>
    /// <param name="parent">
    ///     An existing <see cref="TinyTexture"/> instance.
    /// </param>
    /// <param name="sourceRectangle">
    ///     A <see cref="Rectangle"/> value that defines the boundry within the
    ///     <paramref name="parent"/> that this instance represents.
    /// </param>
    public TinyTexture(TinyTexture parent, Rectangle sourceRectangle)
        : this(parent, sourceRectangle.X, sourceRectangle.Y, sourceRectangle.Width, sourceRectangle.Height) { }

    //  Finalizer implementation to interally call Dispose passing false.
    ~TinyTexture() => Dispose(false);

    /// <summary>
    ///     Creates a new <see cref="TinyTexture"/> instance from a file.
    /// </summary>
    /// <param name="path">
    ///     A <see cref="string"/> value containing the fully qualified absolute
    ///     path to the image file to load.
    /// </param>
    /// <param name="premultiplyAlpha">
    ///     A <see cref="bool"/> value that indicates if the pixel color value of
    ///     the loaded texture should premultiply alpha.
    /// </param>
    /// <returns>
    ///     A <see cref="TinyTexture"/> instance.
    /// </returns>
    public static TinyTexture FromFile(string path, bool premultiplyAlpha)
    {
        Texture2D texture = TextureUtilities.FromFile(Engine.Graphics.Device, path, premultiplyAlpha);
        return new TinyTexture(texture);
    }

    private void SetValues()
    {
        if(Texture == null)
        {
            throw new InvalidOperationException("Underlying texture of TinyTexture instance is null");
        }
        
        Center = new Vector2(Width, Height) * 0.5f;
        TopUV = SourceRectangle.Top / (float)Texture.Height;
        RightUV = SourceRectangle.Right / (float)Texture.Width;
        BottomUV = SourceRectangle.Bottom / (float)Texture.Height;
        LeftUV = SourceRectangle.Left / (float)Texture.Width;
    }

    /// <summary>
    ///     Given a boundry within this <see cref="TinyTexture"/> instance, creates a new
    ///     <see cref="TinyTexture"/> instance from the boundry.
    /// </summary>
    /// <param name="x">
    ///     A <see cref="int"/> value that defines the top-left x-coordinate of
    ///     the boundry within this instance instance represents.
    /// </param>
    /// <param name="y">
    ///     A <see cref="int"/> value that defines the top-left y-coordinate of
    ///     the boundry within this instance represents.
    /// </param>
    /// <param name="width">
    ///     A <see cref="int"/> value that defines the width of the boundry within
    ///     this instance.
    /// </param>
    /// <param name="height">
    ///     A <see cref="int"/> value that defines the height of the boundry within
    ///     this instance represents.
    /// </param>
    /// <returns>
    ///     A <see cref="TinyTexture"/> instance.
    /// </returns>
    public TinyTexture GetSubtexture(int x, int y, int width, int height)
    {
        return new TinyTexture(this, x, y, width, height);
    }

    /// <summary>
    ///     Given a boundry within this <see cref="TinyTexture"/> instance, creates a new
    ///     <see cref="TinyTexture"/> instance from the boundry.
    /// </summary>
    /// <param name="relativeRect">
    ///     A <see cref="Rectangle"/> value that defines the boundry within
    ///    this instance.
    /// </param>
    /// <returns>
    ///     A <see cref="TinyTexture"/> instance.
    /// </returns>
    public TinyTexture GetSubteture(Rectangle relativeRect)
    {
        return new TinyTexture(this, relativeRect);
    }

    /// <summary>
    ///     Gets a <see cref="Rectangle"/> value that is relative to the boundry
    ///     of this instance realitve to the main texture.
    /// </summary>
    /// <param name="rect">
    ///     A <see cref="Rectangle"/> value that defines the relative boundry.
    /// </param>
    /// <returns>
    ///     A <see cref="Rectangle"/> value.
    /// </returns>
    public Rectangle GetRelativeRect(Rectangle rect)
    {
        return GetRelativeRect(rect.X, rect.Y, rect.Width, rect.Height);
    }

    /// <summary>
    ///     Gets a <see cref="Rectangle"/> value that is relative to the boundry
    ///     of this instance realitve to the main texture.
    /// </summary>
    /// <param name="x">
    ///     A <see cref="int"/> value that defines the top-left x-coordinate position
    ///     of the relative boundry.
    /// </param>
    /// <param name="y">
    ///     A <see cref="int"/> value that defines the top-left y-coordinate position
    ///     of the relative boundry.
    /// </param>
    /// <param name="width">
    ///     A <see cref="int"/> value that defines the width of the relative boundry.
    /// </param>
    /// <param name="height">
    ///     A <see cref="int"/> value that defines the height of the relative boundry.
    /// </param>
    /// <returns>
    ///     A <see cref="Rectangle"/> value.
    /// </returns>
    public Rectangle GetRelativeRect(int x, int y, int width, int height)
    {
        int atX = SourceRectangle.X + x;
        int atY = SourceRectangle.Y + y;

        int rX = (int)MathHelper.Clamp(atX, SourceRectangle.Left, SourceRectangle.Right);
        int rY = (int)MathHelper.Clamp(atY, SourceRectangle.Top, SourceRectangle.Bottom);
        int rW = Math.Max(0, Math.Min(atX + width, SourceRectangle.Right) - rX);
        int rH = Math.Max(0, Math.Min(atY + height, SourceRectangle.Bottom) - rY);

        return new Rectangle(rX, rY, rW, rH);
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
            if (Texture != null && !Texture.IsDisposed)
            {
                Texture.Dispose();
                Texture = null;
            }
        }

        IsDisposed = true;
    }

}
