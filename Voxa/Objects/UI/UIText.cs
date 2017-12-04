using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using Voxa.Objects.Renderer;
using Voxa.Rendering;

namespace Voxa.Objects.UI
{
    public class UIText : UIElement
    {
        public enum AlignmentMode
        {
            Center,
            CenterLeft,
            CenterRight,
            TopCenter,
            TopLeft,
            TopRight,
            BottomCenter,
            BottomLeft,
            BottomRight
        }

        public AlignmentMode Alignment { get; private set; }
        public string Text { get; private set; }
        public Color4 Color { get; private set; }
        public Font Font { get; private set; }
        public float Scale { get; private set; }

        private TextRenderer textRenderer;
        private GameObject gameObject;

        public UIText(UIElement parent, AlignmentMode alignment, string text, Color4 color, Font font, float scale, GameObject linkedObject)
        {
            this.Text = text;
            this.Color = color;
            this.gameObject = linkedObject;
            this.Font = font;
            this.Scale = scale;
            this.Position = Vector2.Zero;
            this.initRenderer();
            this.SetParent(parent);
            this.SetAlignment(alignment);
        }

        public UIText(Vector2 position, string text, Color4 color, Font font, float scale, GameObject linkedObject)
        {
            this.Text = text;
            this.Color = color;
            this.gameObject = linkedObject;
            this.Font = font;
            this.Scale = scale;
            this.Position = position;
            this.initRenderer();
        }

        private void initRenderer()
        {
            this.textRenderer = new TextRenderer(this.Text, this.Position, this.Color, this.Font, this.Scale);
            this.gameObject.AttachComponent(this.textRenderer);
            this.textRenderer.Init();
        }

        public void SetColor(Color4 color)
        {
            this.Color = color;
            this.textRenderer.Color = color;
            this.textRenderer.UpdateVertexBuffer();
        }

        public void SetScale(float scale)
        {
            this.Scale = scale;
            this.textRenderer.Scale = this.Scale;
            this.SetDirty();
        }

        public void SetText(string text)
        {
            this.Text = text;
            this.textRenderer.Text = this.Text;
            this.SetDirty();
        }

        public void SetPosition(Vector2 position)
        {
            if (this.Parent == null) {
                this.Position = position;
                this.textRenderer.Position = this.Position;
                this.SetDirty();
            }
        }

        public override void SetDirty()
        {
            base.SetDirty();
            if (this.Parent != null) this.SetAlignment(this.Alignment);
            else this.textRenderer.UpdateVertexBuffer();
            this.isDirty = false;
        }

        public void SetAlignment(AlignmentMode alignment)
        {
            if (this.Parent != null && this.Parent is ICanvas) {
                Vector2 position = this.Position;
                Vector2 pPos = this.Parent.Position;
                ICanvas pCanvas = (ICanvas)(this.Parent);
                System.Drawing.Size pSize = pCanvas.Size;
                int textPixelWidth = this.textRenderer.GetPixelWidth();

                switch (alignment) {
                    case AlignmentMode.Center: {
                        if (textPixelWidth <= pSize.Width) {
                            position.X = pPos.X + ((pSize.Width - textPixelWidth) / 2);
                            position.Y = pPos.Y + (pSize.Height / 2);
                        }
                        break;
                    }
                }
                this.textRenderer.Position = position;
                this.textRenderer.UpdateVertexBuffer();
            }
        }
    }
}
