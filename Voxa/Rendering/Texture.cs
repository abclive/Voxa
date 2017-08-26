using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using Voxa.Utils;

namespace Voxa.Rendering
{
    class Texture
    {
        public enum FilteringMode
        {
            NEAREST_NEIGHBOR = All.Nearest,
            LINEAR = All.Linear
        }

        public enum RepeatMode
        {
            REPEAT = All.Repeat,
            MIRRORED_REPEAT = All.MirroredRepeat,
            CLAMP_TO_EDGE = All.ClampToEdge,
            CLAMP_TO_BORDER = All.ClampToBorder
        }

        public string ResourcePath;

        public bool FlipY = false;
        public FilteringMode Filtering = FilteringMode.LINEAR;
        public RepeatMode Repeat = RepeatMode.REPEAT;
        public TextureUnit Unit = TextureUnit.Texture0;

        private Bitmap textureBitmap;
        private BitmapData textureData;
        private int textureId;

        public Texture(string resourcePath)
        {
            this.ResourcePath = resourcePath;
        }

        public void Bind()
        {
            GL.ActiveTexture(this.Unit);
            GL.BindTexture(TextureTarget.Texture2D, this.textureId);
        }

        public void Load()
        {
            this.textureBitmap = new Bitmap(ResourceManager.GetFileStream(this.ResourcePath));

            if (this.FlipY)
                this.textureBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);

            this.textureData = this.textureBitmap.LockBits(new Rectangle(0, 0, this.textureBitmap.Width, this.textureBitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            this.textureId = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, this.textureId);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)this.Filtering);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)this.Filtering);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)this.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)this.Repeat);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, this.textureBitmap.Width, this.textureBitmap.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, this.textureBitmap.Width, this.textureBitmap.Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, this.textureData.Scan0);

            GL.BindTexture(TextureTarget.Texture2D, 0);

            this.textureBitmap.UnlockBits(this.textureData);
            this.textureBitmap.Dispose();
        }
    }
}
