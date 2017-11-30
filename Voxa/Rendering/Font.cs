using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SharpFont;

namespace Voxa.Rendering
{
    public class Font
    {
        public struct Character
        {
            public int     TextureId;
            public Size    Size;
            public Vector2 Bearing;
            public int     Advance;
        }

        public Dictionary<char, Character> Characters { get; private set; }
        public Face Face { get; private set; }
        public uint Size { get; private set; }

        public Font(string path)
        {
            this.Face = new Face(Engine.FontLibrary, path);
            this.Size = 10;
            this.Face.SetPixelSizes(0, this.Size);
            this.createCharacterMap();
        }

        public Font(string path, uint size)
        {
            this.Face = new Face(Engine.FontLibrary, path);
            this.Size = size;
            this.Face.SetPixelSizes(0, this.Size);
            this.createCharacterMap();
        }

        private void createCharacterMap()
        {
            this.Characters = new Dictionary<char, Character>();
            for (uint charCode = 0; charCode < 128; charCode++) {
                this.Face.LoadChar(charCode, LoadFlags.Render, LoadTarget.Normal);

                int textureId = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, textureId);
                GL.TexImage2D(
                    TextureTarget.Texture2D,
                    0,
                    PixelInternalFormat.CompressedRed,
                    this.Face.Glyph.Bitmap.Width,
                    this.Face.Glyph.Bitmap.Rows,
                    0,
                    PixelFormat.Red,
                    PixelType.UnsignedByte,
                    this.Face.Glyph.Bitmap.Buffer
                );
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)All.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);

                Character character = new Character() {
                    TextureId = textureId,
                    Size = new Size(this.Face.Glyph.Bitmap.Width, this.Face.Glyph.Bitmap.Rows),
                    Bearing = new Vector2(this.Face.Glyph.BitmapLeft, this.Face.Glyph.BitmapTop),
                    Advance = this.Face.Glyph.Advance.X.ToInt32()
                };

                this.Characters.Add((char)charCode, character);
            }
        }
    }
}
