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

namespace Aristurtle.TinyEngine;

public partial class VirtualButton
{
    public partial class Mouse
    {
        public class RightButton : Node
        {
            /// <summary>
            ///     Gets a <see cref="bool"/> value indicating if this
            ///     <see cref="RightButton"/> node is pressed down.
            /// </summary>
            public override bool Check => Input.Mouse.ButtonCheck(MouseButton.Right);

            /// <summary>
            ///     Gets a <see cref="bool"/> value indicating if this
            ///     <see cref="RightButton"/> node was just prssed down on the current
            ///     frame only.
            /// </summary>
            public override bool Pressed => Input.Mouse.ButtonPressed(MouseButton.Right);

            /// <summary>
            ///     Gets a <see cref="bool"/> value indicating if this
            ///     <see cref="RightButton"/> node was just released on the current frame
            ///     only.
            /// </summary>
            public override bool Released => Input.Mouse.ButtonReleased(MouseButton.Right);
        }
    }
}

