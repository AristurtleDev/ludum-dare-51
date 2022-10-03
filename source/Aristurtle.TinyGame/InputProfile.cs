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

using Aristurtle.TinyEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Aristurtle.TinyGame;

public class InputProfile
{
    public VirtualButton MoveUp { get; private set; }
    public VirtualButton MoveDown { get; private set; }
    public VirtualButton MoveLeft { get; private set; }
    public VirtualButton MoveRight { get; private set; }

    public VirtualButton VolumeUp { get; private set; }
    public VirtualButton VolumeDown { get; private set; }

    public VirtualButton Pause { get; private set; }
    public VirtualButton Retry { get; private set; }

    public VirtualButton TitleAction { get; private set; }

    public bool AnyButtonPressed => Input.Keyboard.AnyKeyPressed || Input.GamePads[0].AnyButtonPressed();


    public InputProfile()
    {
        TitleAction = new VirtualButton();
        TitleAction.Nodes.Add(new VirtualButton.Keyboard.Key(Keys.Enter));
        TitleAction.Nodes.Add(new VirtualButton.GamePad.Button(PlayerIndex.One, Buttons.Start));

        Retry = new VirtualButton();
        Retry.Nodes.Add(new VirtualButton.Keyboard.Key(Keys.Enter));
        Retry.Nodes.Add(new VirtualButton.GamePad.Button(PlayerIndex.One, Buttons.Back));

        VolumeUp = new VirtualButton();
        VolumeUp.Nodes.Add(new VirtualButton.Keyboard.Key(Keys.OemPlus));

        VolumeDown = new VirtualButton();
        VolumeDown.Nodes.Add(new VirtualButton.Keyboard.Key(Keys.OemMinus));

        Pause = new VirtualButton();
        Pause.Nodes.Add(new VirtualButton.Keyboard.Key(Keys.Escape));
        Pause.Nodes.Add(new VirtualButton.GamePad.Button(PlayerIndex.One, Buttons.Start));

        MoveUp = new VirtualButton();
        MoveUp.Nodes.Add(new VirtualButton.Keyboard.Key(Keys.W));
        MoveUp.Nodes.Add(new VirtualButton.Keyboard.Key(Keys.Up));
        MoveUp.Nodes.Add(new VirtualButton.GamePad.Button(PlayerIndex.One, Buttons.DPadUp));
        MoveUp.Nodes.Add(new VirtualButton.GamePad.Button(PlayerIndex.One, Buttons.Y));
        MoveUp.Nodes.Add(new VirtualButton.GamePad.LeftStickUp(PlayerIndex.One));
        MoveUp.Nodes.Add(new VirtualButton.GamePad.RightStickUp(PlayerIndex.One));

        MoveDown = new VirtualButton();
        MoveDown.Nodes.Add(new VirtualButton.Keyboard.Key(Keys.S));
        MoveDown.Nodes.Add(new VirtualButton.Keyboard.Key(Keys.Down));
        MoveDown.Nodes.Add(new VirtualButton.GamePad.Button(PlayerIndex.One, Buttons.DPadDown));
        MoveDown.Nodes.Add(new VirtualButton.GamePad.Button(PlayerIndex.One, Buttons.A));
        MoveDown.Nodes.Add(new VirtualButton.GamePad.LeftStickDown(PlayerIndex.One));
        MoveDown.Nodes.Add(new VirtualButton.GamePad.RightStickDown(PlayerIndex.One));

        MoveLeft = new VirtualButton();
        MoveLeft.Nodes.Add(new VirtualButton.Keyboard.Key(Keys.A));
        MoveLeft.Nodes.Add(new VirtualButton.Keyboard.Key(Keys.Left));
        MoveLeft.Nodes.Add(new VirtualButton.GamePad.Button(PlayerIndex.One, Buttons.DPadLeft));
        MoveLeft.Nodes.Add(new VirtualButton.GamePad.Button(PlayerIndex.One, Buttons.X));
        MoveLeft.Nodes.Add(new VirtualButton.GamePad.LeftStickLeft(PlayerIndex.One));
        MoveLeft.Nodes.Add(new VirtualButton.GamePad.RightStickLeft(PlayerIndex.One));

        MoveRight = new VirtualButton();
        MoveRight.Nodes.Add(new VirtualButton.Keyboard.Key(Keys.D));
        MoveRight.Nodes.Add(new VirtualButton.Keyboard.Key(Keys.Right));
        MoveRight.Nodes.Add(new VirtualButton.GamePad.Button(PlayerIndex.One, Buttons.DPadRight));
        MoveRight.Nodes.Add(new VirtualButton.GamePad.Button(PlayerIndex.One, Buttons.B));
        MoveRight.Nodes.Add(new VirtualButton.GamePad.LeftStickRight(PlayerIndex.One));
        MoveRight.Nodes.Add(new VirtualButton.GamePad.RightStickRight(PlayerIndex.One));
    }

    public void Register()
    {
        MoveUp.Register();
        MoveDown.Register();
        MoveLeft.Register();
        MoveRight.Register();
        VolumeUp.Register();
        VolumeDown.Register();
        Pause.Register();
        Retry.Register();
        TitleAction.Register();
    }

    public void Deregister()
    {
        MoveUp.Deregister();
        MoveDown.Deregister();
        MoveLeft.Deregister();
        MoveRight.Deregister();
        VolumeUp.Deregister();
        VolumeDown.Deregister();
        Pause.Deregister();
        Retry.Deregister();
        TitleAction.Deregister();
    }
}

