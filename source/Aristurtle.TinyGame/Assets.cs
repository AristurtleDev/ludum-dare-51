// using System.Collections.Generic;
// using Aristurtle.TinyEngine;

// namespace Aristurtle.TinyGame;

// public static class Assets
// {
//     private static Dictionary<string, TinyTexture> _sprites = new();
//     private static Dictionary<string, PixelFont> _fonts = new();
//     public static SpriteSheet ControllerIcons;

//     public static void LoadEssential()
//     {
//         _sprites.Add("background_pattern", TinyTexture.FromFile(@"sprites/background_pattern.png", true));

//         PixelFont russoOneFont = new("Russo One");
//         russoOneFont.AddFontSize(@"fonts/russoOne64.fnt");
//         russoOneFont.AddFontSize(@"font/russoOne32.fnt");
//         _fonts.Add(russoOneFont.Face, russoOneFont);
//     }

//     public static void Load()
//     {
//         LoadSprites();
//         LoadSpriteSheets();
//         LoadFonts();
//     }

//     private static void LoadSprites()
//     {
//         _sprites.Add("grid_cell", TinyTexture.FromFile(@"sprites/grid_cell.png", false));
//     }

//     private static void LoadFonts()
//     {
//         SpriteSheet controllerSheet = new(string.Empty);
//         controllerSheet.Load("@spritesheets/x360_icons.xml");

//         PixelFont controllerFont = new("Controller");
//         PixelFontSize size = new(new() { controllerSheet.Texture })
//         {
//             LineHeight = 64,
//             Size = 64
//         };

//         controllerSheet.TryGetSprite("button_a", out TinyTexture buttonA);
//         controllerSheet.TryGetSprite("button_b", out TinyTexture buttonB);
//         controllerSheet.TryGetSprite("button_x", out TinyTexture buttonX);
//         controllerSheet.TryGetSprite("button_y", out TinyTexture buttonY);
//         controllerSheet.TryGetSprite("dpad", out TinyTexture dpad);
//         controllerSheet.TryGetSprite("dpad_down", out TinyTexture dpad_down);
//         controllerSheet.TryGetSprite("dpad_left", out TinyTexture dpad_left);
//         controllerSheet.TryGetSprite("dpad_right", out TinyTexture dpad_right);
//         controllerSheet.TryGetSprite("dpad_up", out TinyTexture dpad_up);
//         controllerSheet.TryGetSprite("left_thumbstick", out TinyTexture left_thumbstick);
//         controllerSheet.TryGetSprite("left_thumbstick_down", out TinyTexture left_thumbstick_down);
//         controllerSheet.TryGetSprite("left_thumbstick_left", out TinyTexture left_thumbstick_left);
//         controllerSheet.TryGetSprite("left_thumbstick_right", out TinyTexture left_thumbstick_right);
//         controllerSheet.TryGetSprite("left_thumbstick_up", out TinyTexture left_thumbstick_up);
//         controllerSheet.TryGetSprite("right_thumbstick", out TinyTexture right_thumbstick);
//         controllerSheet.TryGetSprite("right_thumbstick_down", out TinyTexture right_thumbstick_down);
//         controllerSheet.TryGetSprite("right_thumbstick_left", out TinyTexture right_thumbstick_left);
//         controllerSheet.TryGetSprite("right_thumbstick_right", out TinyTexture right_thumbstick_right);
//         controllerSheet.TryGetSprite("right_thumbstick_up", out TinyTexture right_thumbstick_up);

//         size.AddCharacter(new PixelFontCharacter('a', buttonA, 0, 0, 64));
//         size.AddCharacter(new PixelFontCharacter('b', buttonB, 0, 0, 64));
//         size.AddCharacter(new PixelFontCharacter('x', buttonX, 0, 0, 64));
//         size.AddCharacter(new PixelFontCharacter('y', buttonY, 0, 0, 64));

//         size.AddCharacter(new PixelFontCharacter('s', dpad, 0, 0, 64));
//         size.AddCharacter(new PixelFontCharacter('w', dpad_up, 0, 0, 64));
//         size.AddCharacter(new PixelFontCharacter('x', dpad_down, 0, 0, 64));
//         size.AddCharacter(new PixelFontCharacter('a', dpad_left, 0, 0, 64));
//         size.AddCharacter(new PixelFontCharacter('d', dpad_right, 0, 0, 64));

//         size.AddCharacter(new PixelFontCharacter('k', left_thumbstick, 0, 0, 64));
//         size.AddCharacter(new PixelFontCharacter('i', left_thumbstick_up, 0, 0, 64));
//         size.AddCharacter(new PixelFontCharacter('m', left_thumbstick_down, 0, 0, 64));
//         size.AddCharacter(new PixelFontCharacter('j', left_thumbstick_left, 0, 0, 64));
//         size.AddCharacter(new PixelFontCharacter('l', left_thumbstick_right, 0, 0, 64));

//         size.AddCharacter(new PixelFontCharacter('5', right_thumbstick, 0, 0, 64));
//         size.AddCharacter(new PixelFontCharacter('8', right_thumbstick_up, 0, 0, 64));
//         size.AddCharacter(new PixelFontCharacter('2', right_thumbstick_down, 0, 0, 64));
//         size.AddCharacter(new PixelFontCharacter('4', right_thumbstick_left, 0, 0, 64));
//         size.AddCharacter(new PixelFontCharacter('6', right_thumbstick_right, 0, 0, 64));

//         controllerFont.AddFontSize(size);

//         _fonts.Add(controllerFont.Face, controllerFont);
//     }

//     private static void LoadSpriteSheets()
//     {
//         ControllerIcons = new SpriteSheet("Xbox 360 Icons");
//         ControllerIcons.Load(@"spritesheets/x360_icons.xml");
//     }

//     public static bool TryGetTexture(string name, out TinyTexture texture)
//     {
//         return _sprites.TryGetValue(name, out texture);
//     }

//     public static bool TryGetFont(string name, float size, out PixelFontSize font)
//     {
//         if (_fonts.TryGetValue(name, out PixelFont pixelFont))
//         {
//             font = pixelFont.Get(size);
//             return true;
//         }
//         else
//         {
//             font = null;
//             return false;
//         }
//     }
// }