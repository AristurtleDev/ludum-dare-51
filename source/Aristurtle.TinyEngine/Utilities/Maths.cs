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

using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Aristurtle.TinyEngine;

public static class Maths
{
    private static StringBuilder _tempBuilder = new();

    //  A Random instance that can be used globally for random value generation.
    private static readonly Random s_random = new Random();

    /// <summary>
    ///     Given a <see cref="int"/> value, determines if it is range of the
    ///     inclusive <paramref name="lower"/> and exclusive <paramref name="upper"/>
    ///     bounds.
    /// </summary>
    /// <param name="value">
    ///     A <see cref="int"/> value to check.,
    /// </param>
    /// <param name="lower">
    ///     A <see cref="int"/> value that defines the inclusive lower bounds.
    /// </param>
    /// <param name="upper">
    ///     A <see cref="int"/> value that defines the exclusive upper bounds.
    /// </param>
    /// <returns>
    ///     <c>true</c> if the <paramref name="value"/> is within range;
    ///     otherwise, <c>false</c>.
    /// </returns>
    public static bool IsInRange(int value, int lower, int upper) => value >= lower && value < upper;

    /// <summary>
    ///     Given a <see cref="float"/> value, determines if it is range of the
    ///     inclusive <paramref name="lower"/> and exclusive <paramref name="upper"/>
    ///     bounds.
    /// </summary>
    /// <param name="value">
    ///     A <see cref="float"/> value to check.,
    /// </param>
    /// <param name="lower">
    ///     A <see cref="float"/> value that defines the inclusive lower bounds.
    /// </param>
    /// <param name="upper">
    ///     A <see cref="float"/> value that defines the exclusive upper bounds.
    /// </param>
    /// <returns>
    ///     <c>true</c> if the <paramref name="value"/> is within range;
    ///     otherwise, <c>false</c>.
    /// </returns>
    public static bool IsInRange(float value, float lower, float upper) => value >= lower && value < upper;

    /// <summary>
    ///     Given a <see cref="double"/> value, determines if it is range of the
    ///     inclusive <paramref name="lower"/> and exclusive <paramref name="upper"/>
    ///     bounds.
    /// </summary>
    /// <param name="value">
    ///     A <see cref="double"/> value to check.,
    /// </param>
    /// <param name="lower">
    ///     A <see cref="double"/> value that defines the inclusive lower bounds.
    /// </param>
    /// <param name="upper">
    ///     A <see cref="double"/> value that defines the exclusive upper bounds.
    /// </param>
    /// <returns>
    ///     <c>true</c> if the <paramref name="value"/> is within range;
    ///     otherwise, <c>false</c>.
    /// </returns>
    public static bool IsInRange(double value, double lower, double upper) => value >= lower && value < upper;

    /// <summary>
    ///     Gets a <see cref="System.Random"/> instance that can be used globally for
    ///     random value generation.
    /// </summary>
    public static Random Random => s_random;

    /// <summary>
    ///     Returns a random <see cref="float"/> value that is greater than or equal 
    ///     to <c>0.0f</c> and less than <c>1.0f</c>.
    /// </summary>
    /// <param name="random">
    ///     A <see cref="System.Random"/> instance.
    /// </param>
    /// <returns>
    ///     A random <see cref="float"/> value that is greater than or equal to <c>0.0f</c> and
    ///     less than <c>1.0f</c>.
    /// </returns>
    public static float NextFloat(this Random random)
    {
        return (float)random.NextDouble();
    }

    /// <summary>
    ///     Returns a random <see cref="float"/> value that is greater than or equal to
    ///     the <paramref name="min"/> value and less than the <paramref name="max"/> value.
    /// </summary>
    /// <param name="random">
    ///     A <see cref="System.Random"/> instance.
    /// </param>
    /// <param name="min">
    ///     The inclusive lower bound of the random number returned.
    /// </param>
    /// <param name="max">
    ///     The exclusive upper bound of the random number returned.
    /// </param>
    /// <returns>
    ///     A random <see cref="float"/> value that is greater than or equal to the
    ///     <paramref name="min"/> value and less than the <paramref name="max"/> value.
    /// </returns>
    public static float NextFloat(this Random random, float min, float max)
    {
        return (float)random.NextDouble() * (max - min) + min;
    }

    /// <summary>
    ///     Returns a random <see cref="double"/> value that is greater than or equal to
    ///     the <paramref name="min"/> value and less than the <paramref name="max"/> value.
    /// </summary>
    /// <param name="random">
    ///     A <see cref="System.Random"/> instance.
    /// </param>
    /// <param name="min">
    ///     The inclusive lower bound of the random number returned.
    /// </param>
    /// <param name="max">
    ///     The exclusive upper bound of the random number returned.
    /// </param>
    /// <returns>
    ///     A random <see cref="double"/> value that is greater than or equal to the
    ///     <paramref name="min"/> value and less than the <paramref name="max"/> value.
    /// </returns>
    public static double NextDouble(this Random random, double min, double max)
    {
        return random.NextDouble() * (max - min) + min;
    }

    /// <summary>
    ///     Given a series of <typeparamref name="T"/> values, chooses one at random and
    ///     returns it.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of values.
    /// </typeparam>
    /// <param name="random">
    ///     A <see cref="System.Random"/> instance.
    /// </param>
    /// <param name="values">
    ///     The values to choose from.
    /// </param>
    /// <returns>
    ///     A <typeparamref name="T"/> value randomly choosen from the ones given.
    /// </returns>
    public static T Next<T>(this Random random, params T[] values)
    {
        return values[random.Next(values.Length)];
    }

    /// <summary>
    ///     Given a list of <typeparamref name="T"/> values, chooses one at randome
    ///     and returns it
    /// </summary>
    /// <typeparam name="T">
    ///     The type of values.
    /// </typeparam>
    /// <param name="random">
    ///     A <see cref="System.Random"/> instance.
    /// </param>
    /// <param name="values">
    ///     The values to choose from.
    /// </param>
    /// <returns>
    ///     A <typeparamref name="T"/> value randomly choosen from the list of values given.
    /// </returns>
    public static T Next<T>(this Random random, List<T> values)
    {
        return values[random.Next(values.Count)];
    }

    /// <summary>
    ///     Given the x and y coordinate location of a vector, calculates
    ///     the angel, in radians, of the vector.
    /// </summary>
    /// <param name="x">
    ///     An <see cref="int"/> value representing the x-coordiante location
    ///     of the vector.
    /// </param>
    /// <param name="y">
    ///     An <see cref="int"/> value representing the y-coordinate location
    ///     of the vector.
    /// </param>
    /// <returns>
    ///     A <see cref="float"/> value which is the angel, in radians, of the
    ///     vector located at the x and y coordinates given.
    /// </returns>
    public static float Angle(int x, int y)
    {
        return (float)Math.Atan2(y, x);
    }

    /// <summary>
    ///     Given the x and y coordinate location of a vector, calculates
    ///     the angel, in radians, of the vector.
    /// </summary>
    /// <param name="x">
    ///     An <see cref="float"/> value representing the x-coordiante location
    ///     of the vector.
    /// </param>
    /// <param name="y">
    ///     An <see cref="float"/> value representing the y-coordinate location
    ///     of the vector.
    /// </param>
    /// <returns>
    ///     A <see cref="float"/> value which is the angel, in radians, of the
    ///     vector located at the x and y coordinates given.
    /// </returns>
    public static float Angle(float x, float y)
    {
        return (float)Math.Atan2(y, x);
    }

    /// <summary>
    ///     Calculates the angle, in radians, of a <see cref="Point"/> value.
    /// </summary>
    /// <param name="value">
    ///     The <see cref="Point"/> value to calculate the angle of.
    /// </param>
    /// <returns>
    ///     A <see cref="float"/> value which is the angle, in radians, of the
    ///     <see cref="Point"/> value given.
    /// </returns>
    public static float Angle(this Point value)
    {
        return (float)Math.Atan2(value.Y, value.X);
    }

    /// <summary>
    ///     Calculates the angle, in radians, of a <see cref="Vector2"/> value.
    /// </summary>
    /// <param name="value">
    ///     The <see cref="Vector2"/> value to calculate the angle of.
    /// </param>
    /// <returns>
    ///     A <see cref="float"/> value which is the angle, in radians, of the
    ///     <see cref="Vector2"/> value given.
    /// </returns>
    public static float Angle(this Vector2 value)
    {
        return (float)Math.Atan2(value.Y, value.X);
    }

    /// <summary>
    ///     Calculates the angle in radians between two points.
    /// </summary>
    /// <param name="x1">
    ///     A <see cref="int"/> value representing the x-coordinate position
    ///     of the starting point.
    /// </param>
    /// <param name="y1">
    ///     A <see cref="int"/> value representing the y-coordinate position
    ///     of the starting point.
    /// </param>
    /// <param name="x2">
    ///     A <see cref="int"/> value representing the x-coordinate position
    ///     of the ending point.
    /// </param>
    /// <param name="y2">
    ///     A <see cref="int"/> value representing the y-coordinate position
    ///     of the ending point.
    /// </param>
    /// <returns>
    ///     A <see cref="float"/> value containing the angle, in radians, of
    ///     the given points.
    /// </returns>
    public static float Angle(int x1, int y1, int x2, int y2)
    {
        return (float)Math.Atan2(y2 - y1, x2 - x1);
    }

    /// <summary>
    ///     Calculates the angle in radians between two points.
    /// </summary>
    /// <param name="x1">
    ///     A <see cref="float"/> value representing the x-coordinate position
    ///     of the starting point.
    /// </param>
    /// <param name="y1">
    ///     A <see cref="float"/> value representing the y-coordinate position
    ///     of the starting point.
    /// </param>
    /// <param name="x2">
    ///     A <see cref="float"/> value representing the x-coordinate position
    ///     of the ending point.
    /// </param>
    /// <param name="y2">
    ///     A <see cref="float"/> value representing the y-coordinate position
    ///     of the ending point.
    /// </param>
    /// <returns>
    ///     A <see cref="float"/> value containing the angle, in radians, of
    ///     the given points.
    /// </returns>
    public static float Angle(float x1, float y1, float x2, float y2)
    {
        return (float)Math.Atan2(y2 - y1, x2 - x1);
    }

    /// <summary>
    ///     Calculates the angle in radians between two points.
    /// </summary>
    /// <param name="from">
    ///     A <see cref="Point"/> value reperesnting the starting xy-coordinate
    ///     position of the starting point.
    /// </param>
    /// <param name="to">
    ///     A <see cref="Point"/> value reperesnting the starting xy-coordinate
    ///     position of the ending point.
    /// </param>
    /// <returns>
    ///     A <see cref="float"/> value containing the angle, in radians, of
    ///     the given points.
    /// </returns>
    public static float Angle(Point from, Point to)
    {
        return Angle(from.X, from.Y, to.X, to.Y);
    }
    /// <summary>
    ///     Calculates the angle in radians between two points.
    /// </summary>
    /// <param name="from">
    ///     A <see cref="Vector2"/> value reperesnting the starting xy-coordinate
    ///     position of the starting point.
    /// </param>
    /// <param name="to">
    ///     A <see cref="Vector2"/> value reperesnting the starting xy-coordinate
    ///     position of the ending point.
    /// </param>
    /// <returns>
    ///     A <see cref="float"/> value containing the angle, in radians, of
    ///     the given points.
    /// </returns>
    public static float Angle(Vector2 from, Vector2 to)
    {
        return Angle(from.X, from.Y, to.X, to.Y);
    }

    /// <summary>
    ///     Calcualtes a <see cref="Vector2"/> value based the angle, in radians,
    ///     and length of the vector.
    /// </summary>
    /// <param name="radians">
    ///     A <see cref="float"/> value representing the angle of the vector, in radians.
    /// </param>
    /// <param name="length">
    ///     A <see cref="float"/> value representing the length of the vector.
    /// </param>
    /// <returns></returns>
    public static Vector2 AngleToVector(float radians, float length)
    {
        float x = (float)Math.Cos(radians) * length;
        float y = (float)Math.Sin(radians) * length;
        return new Vector2(x, y);
    }

    /// <summary>
    ///     Calcualtes the perpendicular xy-coordiante position of a <see cref="Vector2"/>
    ///     value.
    /// </summary>
    /// <param name="vector2">
    ///     The <see cref="Vector2"/> value to calculate hte perpendicular position of.
    /// </param>
    /// <returns>
    ///     A <see cref="Vector2"/> value.
    /// </returns>
    public static Vector2 Perpendicular(Vector2 vector2)
    {
        return new Vector2(-vector2.Y, vector2.X);
    }

    /// <summary>
    ///     Given a vector and number of slices, calculates a new vector that is
    ///     snapped to the angle of a slice within a unit circle.
    /// </summary>
    /// <param name="vector">
    ///     A <see cref="Vector2"/> value to snap.
    /// </param>
    /// <param name="slices">
    ///     A <see cref="float"/> value indicating the number of times to evenly
    ///     slice a unit circle to create the angles to snap to.
    /// </param>
    /// <returns>
    ///     A <see cref="Vector2"/> value.
    /// </returns>
    public static Vector2 Snap(this Vector2 vector, float slices)
    {
        float dividers = MathHelper.TwoPi / slices;
        float angle = vector.Angle();
        float snappedAngle = (float)Math.Floor((angle + dividers / 2.0f) / dividers) * dividers;
        return AngleToVector(snappedAngle, vector.Length());
    }

    /// <summary>
    ///     Given a vector and number of slices, calculates a new vector that is
    ///     normalized and snapped to the angle of a slice within a unit circle
    /// </summary>
    /// <param name="vector">
    ///     A <see cref="Vector2"/> value to normalize and snap.
    /// </param>
    /// <param name="slices">
    ///     A <see cref="float"/> value indicating the number of times to evenly
    ///     slice a unit circle to create the angles to snap to.
    /// </param>
    /// <returns>
    ///     A <see cref="Vector2"/> value.
    /// </returns>
    public static Vector2 SnapNormal(this Vector2 vector, float slices)
    {
        float dividers = MathHelper.TwoPi / slices;
        float angle = vector.Angle();
        float snappedAngle = (float)Math.Floor((angle + dividers / 2.0f) / dividers) * dividers;
        return AngleToVector(snappedAngle, 1.0f);
    }

    /// <summary>
    ///     Gets half the <see cref="Vector2"/> value given.
    /// </summary>
    /// <param name="vector">
    ///     A <see cref="Vector2"/> value to half.
    /// </param>
    /// <returns>
    ///     A <see cref="Vector2"/> value that is half the original.
    /// </returns>
    public static Vector2 HalfValue(this Vector2 vector)
    {
        return vector * 0.5f;
    }

    /// <summary>
    ///     Gets half the <see cref="Point"/> value given.
    /// </summary>
    /// <param name="point">
    ///     A <see cref="Point"/> value to half.
    /// </param>
    /// <returns>
    ///     A <see cref="Point"/> value that is half the original.
    /// </returns>
    public static Point HalfValue(this Point point)
    {
        return new Point(point.X / 2, point.Y / 2);
    }

    /// <summary>
    ///     Multiples a <see cref="Point"/> value's components by the
    ///     <paramref name="multiplier"/> value.
    /// </summary>
    /// <param name="point">
    ///     The <see cref="Point"/> value to multiply.
    /// </param>
    /// <param name="multiplier">
    ///     A <see cref="int"/> value to multiply the components by.
    /// </param>
    /// <returns>
    ///     A <see cref="Point"/> value contining the multiplication result.
    /// </returns>
    public static Point Multiply(this Point point, int multiplier)
    {
        return new Point(point.X * multiplier, point.Y * multiplier);
    }

    /// <summary>
    ///     Divides a <see cref="Point"/> value's components by the
    ///     <paramref name="divisor"/> value.
    /// </summary>
    /// <param name="point">
    ///     The <see cref="Point"/> value to multiply.
    /// </param>
    /// <param name="divisor">
    ///     A <see cref="int"/> value to divide the components by.
    /// </param>
    /// <returns>
    ///     A <see cref="Point"/> value contining the divsion result.
    /// </returns>
    public static Point Divide(this Point point, int divisor)
    {
        return new Point(point.X / divisor, point.Y / divisor);
    }

    public static string WordWrap(SpriteFont font, string text, int maxWidth)
    {
        return WordWrap(text, maxWidth, font.MeasureString);
    }

    private static string WordWrap(string text, int width, Func<string, Vector2> measureFunc)
    {
        //  Return immediatly if the text is empty.
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        _tempBuilder.Clear();

        //  Split on all spaces
        string[] words = Regex.Split(text, @"(\s)");

        //  Used to track the current width of the line
        float lineWidth = 0.0f;

        for (int w = 0; w < words.Length; w++)
        {
            //  Get the width of the word
            float wordWith = measureFunc(words[w]).X;

            //  If we add the width of the word to the current line width
            //  and we go over the alloted width, then we need to insert a
            //  new line
            if (wordWith + lineWidth > width)
            {
                _tempBuilder.Append('\n');
                lineWidth = 0.0f;

                //  If the current word is just a space, then we continue so
                //  we don't put a space at the beginning of the next line.
                if (words[w].Equals(" "))
                {
                    continue;
                }
            }

            //  Check if the word is longer than the alloted width. If it is,
            //  fit as much of the word in as possible before splitting it.
            if (wordWith > width)
            {
                int start = 0;
                for (int i = 0; w < words[w].Length; w++)
                {
                    if (i - start > 1 && measureFunc(words[w].Substring(start, i - start - 1)).X > width)
                    {
                        _tempBuilder.Append(words[w].Substring(start, i - start - 1));
                        _tempBuilder.Append('\n');
                        start = i - 1;
                    }
                }

                string remaining = words[w].Substring(start, words[w].Length - start);
                _tempBuilder.Append(remaining);
                lineWidth += measureFunc(remaining).X;
            }
            else
            {
                //  Word fits on the current line, so just add it
                lineWidth += wordWith;
                _tempBuilder.Append(words[w]);
            }
        }

        return _tempBuilder.ToString();
    }

    /// <summary>
    ///     Returns a value indictating if the grid cell at the 
    ///     <paramref name="column"/> and <paramref name="row"/> values provided
    ///     is considered an odd cell.  
    /// </summary>
    /// <remarks>
    ///     A "odd" cell is one where the row and column are both even or where the
    ///     row and column are both odd.
    /// </remarks>
    /// <param name="column">
    ///     An <see cref="int"/> value representing the column number of the grid cell.
    /// </param>
    /// <param name="row">
    ///     An <see cref="int"/> value representing the row number of the grid cell.
    /// </param>
    /// <returns>
    ///     <c>true</c> if the grid cell at the row and column given is odd;
    ///     otherwise, <c>false</c
    /// </returns>
    public static bool IsOdd(int column, int row)
    {
        return ((column % 2 == 0 && row % 2 == 0) || (column % 2 != 0 && row % 2 != 0));
    }

    /// <summary>
    ///     Given the <paramref name="column"/> and <paramref name="row"/> number of a
    ///     grid cell, returns the index of the cell within the grid.
    /// </summary>
    /// <param name="column">
    ///     An <see cref="int"/> value representing the column number of the grid cell.
    /// </param>
    /// <param name="row">
    ///     An <see cref="int"/> value representing the row number of the grid cell.
    /// </param>
    /// <param name="totalColumns">
    ///     An <see cref="int"/> value representing the total number of columns in
    ///     the grid.
    /// </param>
    /// <returns>
    ///     An <see cref="int"/> value whos value is the index of the grid cell
    ///     within its grid.
    /// </returns>
    public static int GetGridIndex(int column, int row, int totalColumns)
    {
        return (row * totalColumns) + column;
    }

    /// <summary>
    ///     Given then index of a gird cell within a grid, calculates the <paramref name="row"/>
    ///     and <paramref name="column"/> of the grid cell.
    /// </summary>
    /// <param name="gridindex">
    ///     An <see cref="int"/> value representing the index of the grid cell.
    /// </param>
    /// <param name="totalColumns">
    ///     An <see cref="int"/> value representing the total number of columns in
    ///     the grid.
    /// </param>
    /// <param name="column">
    ///     When this method returns, contains the column number of the grid cell as
    ///     an <see cref="int"/> value.
    /// </param>
    /// <param name="row">
    ///     When this method returns, contains the row number of the grid cell as
    ///     an <see cref="int"/> value.
    /// </param>
    public static void GetColumnAndRow(int gridindex, int totalColumns, out int column, out int row)
    {
        row = gridindex / totalColumns;
        column = gridindex % totalColumns;
    }

    /// <summary>
    ///     Inverts the RGB values of a <see cref="Color"/> value.
    /// </summary>
    /// <param name="color">
    ///     The <see cref="Color"/> value to invert.
    /// </param>
    /// <returns>
    ///     A <see cref="Color"/> value that is the inversion of the
    ///     original provided.
    /// </returns>
    public static Color Invert(this Color color)
    {
        return new Color(255 - color.R, 255 - color.G, 255 - color.B, color.A);
    }

    /// <summary>
    ///     Given a <see cref="string"/> whos value is a valid hex color value,
    ///     covnert its to a <see cref="Color"/> value
    /// </summary>
    /// <remarks>
    ///     It is not a requiremnt that the hex color value begin with a <c>#</c>.
    ///     However it does need to be in a <c>RGB</c>, <c>RRGGBB</c>, or
    ///     <c>RRGGBBAA</c> format.
    /// </remarks>
    /// <param name="hex">
    ///     A <see cref="string"/> containing a valid hex value.
    /// </param>
    /// <returns>
    ///     A <see cref="Color"/> value representation of the hex string given.
    /// </returns>
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
