using System;
using System.Collections.Generic;
using System.Text;

namespace MathForGames
{
    class UIText : Actor
    {
        private string _text;
        private int _width;
        private int _height;

        /// <summary>
        /// The text that will appear inside the text box
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// The width of the text box. If the cursor is outside the max
        /// x position the text will wrap around.
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// The height of the text box. If the cursor is outside the max
        /// y position the text is truncated.
        /// </summary>
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        /// <summary>
        /// Sets the starting values for the text box
        /// </summary>
        /// <param name="x">The x position of the text box</param>
        /// <param name="y">The y position of the text box</param>
        /// <param name="name">The name of the text box</param>
        /// <param name="color">The color of the text box</param>
        /// <param name="width">How wide the text box is</param>
        /// <param name="height">How tall the text box is</param>
        /// <param name="text">The text that will be displayed</param>
        public UIText(float x, float y, string name, ConsoleColor color, int width, int height, string text = "")
            : base('\0', x, y, name, color)
        {
            Text = text;
            Width = width;
            Height = height;
        }

        public override void Draw()
        {
            int cursorPosX = (int)Position.X;
            int cursorPosY = (int)Position.Y;

            Icon currentLetter = new Icon { color = Icon.color };

            char[] textChars = Text.ToCharArray();

            //Iterate through all character in the string
            for (int i = 0; i < textChars.Length; i++)
            {
                //Set the icon symbol to be the current character in the array
                currentLetter.Symbol = textChars[i];

                if (currentLetter.Symbol == '\n')
                {
                    cursorPosX = (int)Position.X;
                    cursorPosY++;
                    continue;
                }

                //Add the current character to the buffer
                Engine.Render(currentLetter, new MathLibrary.Vector2 { X = cursorPosX, Y = cursorPosY });

                //Increment the cursor position so th eletters are set side by side
                cursorPosX++;

                if (cursorPosX - (int)Position.X > Width)
                {
                    cursorPosX = (int)Position.X;
                    cursorPosY++;
                }

                if (cursorPosY - (int)Position.Y > Height)
                {
                    break;
                }
            }
        }
    }
}
