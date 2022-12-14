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

public partial class VirtualJoystick
{
    public partial class GamePad
    {
        public class Buttons : Node
        {
            //  The index of the gamepad.
            private int _index;

            //  The value of this node.
            private Vector2 _value;

            /// <summary>
            ///     Gets a <see cref="Vector2"/> value where <see cref="Value.X"/> is
            ///     the value of this node on its x-axis and <see cref="Value.Y"/> is
            ///     the value of this node on its y-axis
            /// </summary>
            public override Vector2 Value => _value;

            /// <summary>
            ///     Gets or Sets the <see cref="Tiny.OverlapBehavior"/> value to use
            ///     when two or more inputs are detected.
            /// </summary>
            public OverlapBehavior OverlapBehavior { get; set; }

            /// <summary>
            ///     Gets or Sets the <see cref="Microsoft.Xna.Framework.Input.Buttons"/> value that represents pushing
            ///     the <see cref="VirtualJoystick"/> upwards.
            /// </summary>
            public Microsoft.Xna.Framework.Input.Buttons Up { get; set; }

            /// <summary>
            ///     Gets or Sets the <see cref="Microsoft.Xna.Framework.Input.Buttons"/> value that represetns pushing
            ///     the <see cref="VirtualJoystick"/> downwards.
            /// </summary>
            public Microsoft.Xna.Framework.Input.Buttons Down { get; set; }

            /// <summary>
            ///     Gets or Sets the <see cref="Microsoft.Xna.Framework.Input.Buttons"/> value that represents pushing
            ///     the <see cref="VirtualJoystick"/> left.
            /// </summary>
            public Microsoft.Xna.Framework.Input.Buttons Left { get; set; }

            /// <summary>
            ///     Gets or Sets the <see cref="Microsoft.Xna.Framework.Input.Buttons"/> value that represents pushing
            ///     the <see cref="VirtualJoystick"/> right.
            /// </summary>
            public Microsoft.Xna.Framework.Input.Buttons Right { get; set; }

            /// <summary>
            ///     Creates a new <see cref="Buttons"/> instance.
            /// </summary>
            /// <param name="index">
            ///     An <see cref="PlayerIndex"/> value representing the
            ///     index of the gamepad this will pull values from.
            /// </param>
            /// <param name="behavior">
            ///     A <see cref="Tiny.OverlapBehavior"/> value to use when two or
            ///     more inputs are detected at the same time.
            /// </param>
            /// <param name="up">
            ///     A <see cref="Microsoft.Xna.Framework.Input.Buttons"/> value
            ///     that represents pushing the <see cref="VirtualJoystick"/>
            ///     upwards.
            /// </param>
            /// <param name="down">
            ///     A <see cref="Microsoft.Xna.Framework.Input.Buttons"/> value
            ///     that represents pushing the <see cref="VirtualJoystick"/>
            ///     downwards.
            /// </param>
            /// <param name="left">
            ///     A <see cref="Microsoft.Xna.Framework.Input.Buttons"/> value
            ///     that represents pushing the <see cref="VirtualJoystick"/>
            ///     left.
            /// </param>
            /// <param name="right">
            ///     A <see cref="Microsoft.Xna.Framework.Input.Buttons"/> value
            ///     that represents pushing the <see cref="VirtualJoystick"/>
            ///     right.
            /// </param>
            public Buttons(PlayerIndex index,
                          OverlapBehavior behavior,
                          Microsoft.Xna.Framework.Input.Buttons up,
                          Microsoft.Xna.Framework.Input.Buttons down,
                          Microsoft.Xna.Framework.Input.Buttons left,
                          Microsoft.Xna.Framework.Input.Buttons right)
                : this((int)index, behavior, up, down, left, right) { }

            /// <summary>
            ///     Creates a new <see cref="Buttons"/> instance.
            /// </summary>
            /// <param name="index">
            ///     An <see cref="int"/> value representing the index of the
            ///     gamepad this will pull values from.
            /// </param>
            /// <param name="behavior">
            ///     A <see cref="Tiny.OverlapBehavior"/> value to use when two or
            ///     more inputs are detected at the same time.
            /// </param>
            /// <param name="up">
            ///     A <see cref="Microsoft.Xna.Framework.Input.Buttons"/> value
            ///     that represents pushing the <see cref="VirtualJoystick"/>
            ///     upwards.
            /// </param>
            /// <param name="down">
            ///     A <see cref="Microsoft.Xna.Framework.Input.Buttons"/> value
            ///     that represents pushing the <see cref="VirtualJoystick"/>
            ///     downwards.
            /// </param>
            /// <param name="left">
            ///     A <see cref="Microsoft.Xna.Framework.Input.Buttons"/> value
            ///     that represents pushing the <see cref="VirtualJoystick"/>
            ///     left.
            /// </param>
            /// <param name="right">
            ///     A <see cref="Microsoft.Xna.Framework.Input.Buttons"/> value
            ///     that represents pushing the <see cref="VirtualJoystick"/>
            ///     right.
            /// </param>
            public Buttons(int index,
                          OverlapBehavior behavior,
                          Microsoft.Xna.Framework.Input.Buttons up,
                          Microsoft.Xna.Framework.Input.Buttons down,
                          Microsoft.Xna.Framework.Input.Buttons left,
                          Microsoft.Xna.Framework.Input.Buttons right)
            {
                _index = index;
                OverlapBehavior = behavior;
                Up = up;
                Down = down;
                Left = left;
                Right = right;
            }

            /// <summary>
            ///     Updates the value of this <see cref="Buttons"/> instance.
            /// </summary>
            public override void Update()
            {
                bool isUp = Input.GamePads[_index].ButtonCheck(Up);
                bool isDown = Input.GamePads[_index].ButtonCheck(Down);
                bool isLeft = Input.GamePads[_index].ButtonCheck(Left);
                bool isRight = Input.GamePads[_index].ButtonCheck(Right);

                if (isUp)
                {
                    if (isDown)
                    {
                        //  Both Up and Down are pressed so the value is determiend
                        //  by the overlap behavior.
                        switch (OverlapBehavior)
                        {
                            default:
                            case OverlapBehavior.Cancel:
                                _value.Y = 0;
                                break;
                            case OverlapBehavior.Positive:
                                _value.Y = 1;
                                break;
                            case OverlapBehavior.Negative:
                                _value.Y = -1;
                                break;
                        }
                    }
                    else
                    {
                        _value.Y = 1;
                    }
                }
                else if (isDown)
                {
                    _value.Y = -1;
                }
                else
                {
                    _value.Y = 0;
                }


                if (isLeft)
                {
                    if (isRight)
                    {
                        //  Both Left and Right are pressed so the value is determiend
                        //  by the overlap behavior.
                        switch (OverlapBehavior)
                        {
                            default:
                            case OverlapBehavior.Cancel:
                                _value.X = 0;
                                break;
                            case OverlapBehavior.Positive:
                                _value.X = 1;
                                break;
                            case OverlapBehavior.Negative:
                                _value.X = -1;
                                break;

                        }
                    }
                    else
                    {
                        _value.X = -1;
                    }
                }
                else if (isRight)
                {
                    _value.X = 1;
                }
                else
                {
                    _value.X = 0;
                }
            }

        }
    }
}

