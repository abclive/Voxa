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

        public enum ScaleMode
        {
            Fixed,
            Ratio,
            Fill
        }

        public AlignmentMode Alignment { get; private set; }
        public ScaleMode RescaleMode { get; private set; }
        public string Text { get; private set; }
        public Color4 Color { get; private set; }
        public Font Font { get; private set; }
        public double Scale { get; private set; }
        public Vector2 Padding { get; private set; }

        private TextRenderer textRenderer;
        private GameObject gameObject;
        private double ratio = 1;
        private double initialRatio = -1;

        public UIText(UIElement parent, AlignmentMode alignment, string text, Color4 color, Font font, float scale, GameObject linkedObject)
        {
            this.Text = text;
            this.Color = color;
            this.gameObject = linkedObject;
            this.Font = font;
            this.Scale = scale;
            this.Position = Vector2.Zero;
            this.Padding = Vector2.Zero;
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
            this.Padding = Vector2.Zero;
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

        public void SetPadding(Vector2 padding)
        {
            this.Padding = padding;
            this.SetDirty();
        }

        public override void SetDirty()
        {
            base.SetDirty();
            if (this.Parent != null) {
                if (this.RescaleMode == ScaleMode.Ratio) {
                    double ratioDifference = this.getRatioDifference();
                    if (ratioDifference != 0) {
                        this.Scale *= 1 + ratioDifference;
                        this.textRenderer.Scale = this.Scale;
                    }
                } else if (this.RescaleMode == ScaleMode.Fill) {
                    double fillRatio = this.getFillRatio();
                    if (fillRatio != 0) {
                        this.Scale *= 1 + fillRatio;
                        this.textRenderer.Scale = this.Scale;
                    }
                }
                this.SetAlignment(this.Alignment);
            }
            else this.textRenderer.UpdateVertexBuffer();
            this.isDirty = false;
        }

        private double getFillRatio()
        {
            if (this.Parent != null && this.Parent is ICanvas) {
                ICanvas pCanvas = (ICanvas)(this.Parent);
                System.Drawing.Size pSize = pCanvas.Size;
                double textPixelWidth = this.textRenderer.GetPixelWidth();

                double ratio = textPixelWidth / (double)pSize.Width;
                return Math.Round(1 - ratio, 2);
            }
            return 0;
        }

        private double getRatioDifference()
        {
            if (this.Parent != null && this.Parent is ICanvas) {
                ICanvas pCanvas = (ICanvas)(this.Parent);
                System.Drawing.Size pSize = pCanvas.Size;
                double textPixelWidth = this.textRenderer.GetPixelWidth();

                double ratio = Math.Min(textPixelWidth / (double)pSize.Width, 1);
                return Math.Round(this.initialRatio - ratio, 2);
            }
            return 0;
        }

        public void SetRescaleMode(ScaleMode scaleMode)
        {
            this.RescaleMode = scaleMode;
            if (this.Parent != null && this.Parent is ICanvas) {
                ICanvas pCanvas = (ICanvas)(this.Parent);
                System.Drawing.Size pSize = pCanvas.Size;
                double textPixelWidth = this.textRenderer.GetPixelWidth();

                switch (scaleMode) {
                    case ScaleMode.Fixed: {
                        this.ratio = this.initialRatio;
                        this.Scale = 1;
                        this.textRenderer.Scale = this.Scale;
                        this.textRenderer.UpdateVertexBuffer();
                        break;
                    } case ScaleMode.Ratio: {
                        double ratio = Math.Min(textPixelWidth / (double)pSize.Width, 1);
                        this.initialRatio = ratio;
                        break;
                    }
                }
            }
        }

        public void SetAlignment(AlignmentMode alignment)
        {
            this.Alignment = alignment;
            if (this.Parent != null && this.Parent is ICanvas) {
                Vector2 position = this.Position;
                Vector2 pPos = this.Parent.Position;
                ICanvas pCanvas = (ICanvas)(this.Parent);
                System.Drawing.Size pSize = pCanvas.Size;
                int textPixelWidth = (int)this.textRenderer.GetPixelWidth();
                int textPixelHeight = this.textRenderer.GetPixelHeight();
                if (this.RescaleMode == ScaleMode.Fill) textPixelWidth -= 1;

                switch (alignment) {
                    case AlignmentMode.Center: {
                        if (textPixelWidth <= pSize.Width) {
                            position.X = pPos.X + ((pSize.Width - textPixelWidth) / 2);
                        } else {
                            position.X = pPos.X + (pSize.Width / 2);
                        }
                        position.Y = pPos.Y + (pSize.Height / 2) - (textPixelHeight / 2);
                        break;
                    } case AlignmentMode.CenterLeft: {
                        if (textPixelWidth <= pSize.Width) {
                            position.X = pPos.X + this.Padding.X;
                        } else {
                            position.X = pPos.X;
                        }
                        position.Y = pPos.Y + (pSize.Height / 2) - (textPixelHeight / 2);
                        break;
                    } case AlignmentMode.CenterRight: {
                        if (textPixelWidth <= pSize.Width) {
                            position.X = pPos.X + (pSize.Width - textPixelWidth - this.Padding.X);
                        } else {
                            position.X = pPos.X + (pSize.Width - textPixelWidth);
                        }
                        position.Y = pPos.Y + (pSize.Height / 2) - (textPixelHeight / 2);
                        break;
                    } case AlignmentMode.BottomCenter: {
                        if (textPixelWidth <= pSize.Width) {
                            position.X = pPos.X + ((pSize.Width - textPixelWidth) / 2);
                        } else {
                            position.X = pPos.X + (pSize.Width / 2);
                        }
                        position.Y = pPos.Y + this.Padding.Y;
                        break;
                    } case AlignmentMode.BottomLeft: {
                        if (textPixelWidth <= pSize.Width) {
                            position.X = pPos.X + this.Padding.X;
                        } else {
                            position.X = pPos.X;
                        }
                        position.Y = pPos.Y + this.Padding.Y;
                        break;
                    } case AlignmentMode.BottomRight: {
                        if (textPixelWidth <= pSize.Width) {
                            position.X = pPos.X + (pSize.Width - textPixelWidth - this.Padding.X);
                        } else {
                            position.X = pPos.X + (pSize.Width - textPixelWidth);
                        }
                        position.Y = pPos.Y + this.Padding.Y;
                        break;
                    } case AlignmentMode.TopCenter: {
                        if (textPixelWidth <= pSize.Width) {
                            position.X = pPos.X + ((pSize.Width - textPixelWidth) / 2);
                        } else {
                            position.X = pPos.X + (pSize.Width / 2);
                        }
                        position.Y = pPos.Y + (pSize.Height - this.Padding.Y);
                        break;
                    } case AlignmentMode.TopLeft: {
                        if (textPixelWidth <= pSize.Width) {
                            position.X = pPos.X + this.Padding.X;
                        } else {
                            position.X = pPos.X;
                        }
                        position.Y = pPos.Y + (pSize.Height - this.Padding.Y);
                        break;
                    } case AlignmentMode.TopRight: {
                        if (textPixelWidth <= pSize.Width) {
                            position.X = pPos.X + (pSize.Width - textPixelWidth - this.Padding.X);
                        } else {
                            position.X = pPos.X + (pSize.Width - textPixelWidth);
                        }
                        position.Y = pPos.Y + (pSize.Height - this.Padding.Y);
                        break;
                    }
                }
                this.textRenderer.Position = position;
                this.textRenderer.UpdateVertexBuffer();
            }
        }
    }
}
