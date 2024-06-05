using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MpDemo.Graphics;

public class BinaryFontImporter
{
    private ContentManager _content;

    public ContentManager Content { get => _content; set => _content = value; }

    public BinaryFontImporter(ContentManager contentManager)
    {
        this._content = contentManager;
    }

    public SpriteFont Import(string name)
    {
        byte[] bytes;

        try
        {
            using(var stream = new BinaryReader(TitleContainer.OpenStream("Content/Fonts/" + name + ".bft")))
                bytes = stream.ReadBytes((int)stream.BaseStream.Length);
            if(bytes.Length < 8)
                throw new EndOfStreamException("The file \"Fonts/" + name + "\" is too short to be read properly");
        }
        catch(Exception exception)
        {
            Console.WriteLine(exception.ToString());
            return null;
        }

        int position = 4;

        int firstChar = bytes[0];
        int lastChar = bytes[1];
        int cellWidth = bytes[2];
        int cellHeight = bytes[3];

        int charCount = lastChar - firstChar + 1;
        int currentCharCount = 0;

        List<Rectangle> bounds = new(charCount);
        List<Rectangle> cropping = new(charCount);
        List<char> characters = new(charCount);
        List<Vector3> kerning = new(charCount);

        while(position < bytes.Length)
        {
            if(position + 4 >= bytes.Length) break;

            Point pos = AsPoint(bytes[position]);
            Point size = AsPoint(bytes[position + 1]);
            Point kern = AsPoint(bytes[position + 2]);
            Point offset = AsPoint(bytes[position + 3]);

            bounds.Add(new Rectangle(pos.X * cellWidth, pos.Y * cellHeight, cellWidth, cellHeight));
            cropping.Add(new Rectangle(Point.Zero, bounds[currentCharCount].Size));
            characters.Add((char)(currentCharCount + firstChar));
            kerning.Add(new Vector3(kern.X, 0, kern.Y));

            position += 4;
            currentCharCount++;
        }

        return new SpriteFont(_content.Load<Texture2D>("Images/Fonts/" + name), bounds, cropping, characters, cellHeight, 1, kerning, null);
    }

    static Point AsPoint(byte value)
    {
        return new Point((value & 0xF0) >> 8, value & 0x0F);
    }
}
