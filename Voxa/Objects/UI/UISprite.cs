using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using System.Drawing;
using Voxa.Objects.Renderer;

namespace Voxa.Objects.UI
{
    public class UISprite : UIElement
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
        public Sprite Sprite { get; private set; }
        public float Scale { get; private set; }
        public Vector2 Padding { get; private set; }

        private SpriteRenderer spriteRenderer;
        private GameObject gameObject;
        private double ratio = 1;
        private double initialRatio = -1;

        public UISprite(UIElement parent, Sprite sprite, float scale, AlignmentMode alignment, GameObject linkedObject)
        {
            this.gameObject = linkedObject;
            this.Sprite = sprite;
            this.Scale = scale;
            this.Position = Vector2.Zero;
            this.Padding = Vector2.Zero;
            this.SetParent(parent);
            this.initRenderer();
            this.SetAlignment(alignment);
        }

        public UISprite(Vector2 position, Sprite sprite, float scale, GameObject linkedObject)
        {
            this.gameObject = linkedObject;
            this.Sprite = sprite;
            this.Scale = scale;
            this.Position = position;
            this.Padding = Vector2.Zero;
            this.initRenderer();
        }

        private void initRenderer()
        {
            this.Sprite.Size = new Vector2(this.Scale, this.Scale);
            this.Sprite.Position = this.Position;
            this.spriteRenderer = new SpriteRenderer(this.Sprite, Engine.RenderingPool.UICamera, 100);
            this.gameObject.AttachComponent(this.spriteRenderer);
            this.spriteRenderer.Init();
        }

        public void SetScale(float scale)
        {
            this.Scale = scale;
            this.Sprite.Size = new Vector2(this.Scale, this.Scale);
            this.SetDirty();
        }

        public override void SetDirty()
        {
            base.SetDirty();
            if (this.Parent != null) {
                if (this.RescaleMode == ScaleMode.Ratio) {
                    float ratioDifference = (float)this.getRatioDifference();
                    if (ratioDifference != 0) {
                        float scale = this.Scale * (1 + ratioDifference);
                        this.Sprite.Size = new Vector2(scale, scale);
                    }
                } else if (this.RescaleMode == ScaleMode.Fill) {
                    ICanvas pCanvas = (ICanvas)(this.Parent);
                    Size pSize = pCanvas.Size;
                    this.spriteRenderer.CustomSize = pSize;
                }
                this.SetAlignment(this.Alignment);
            } else this.spriteRenderer.UpdateVertexBuffer();
            this.isDirty = false;
        }

        private double getRatioDifference()
        {
            if (this.Parent != null && this.Parent is ICanvas) {
                ICanvas pCanvas = (ICanvas)(this.Parent);
                Size pSize = pCanvas.Size;
                double spritePixelWidth = (double)(this.Sprite.Texture.Width * this.Scale);
                double spritePixelHeight = (double)(this.Sprite.Texture.Height * this.Scale);

                double wRatio = Math.Min(spritePixelWidth / (double)pSize.Width, 1);
                double hRatio = Math.Min(spritePixelHeight / (double)pSize.Height, 1);
                double ratio = (wRatio + hRatio) / 2;
                return Math.Round(this.initialRatio - ratio, 2);
            }
            return 0;
        }

        public void SetRescaleMode(ScaleMode scaleMode)
        {
            this.RescaleMode = scaleMode;
            if (this.Parent != null && this.Parent is ICanvas) {
                ICanvas pCanvas = (ICanvas)(this.Parent);
                Size pSize = pCanvas.Size;
                double spritePixelWidth = (double)(this.Sprite.Texture.Width * this.Scale);
                double spritePixelHeight = (double)(this.Sprite.Texture.Height * this.Scale);

                switch (scaleMode) {
                    case ScaleMode.Fixed: {
                        this.ratio = this.initialRatio;
                        this.Scale = 1;
                        this.Sprite.Size = new Vector2(this.Scale, this.Scale);
                        this.spriteRenderer.CustomSize = Size.Empty;
                        this.spriteRenderer.UpdateVertexBuffer();
                        break;
                    }
                    case ScaleMode.Ratio: {
                        double wRatio = Math.Min(spritePixelWidth / (double)pSize.Width, 1);
                        double hRatio = Math.Min(spritePixelHeight / (double)pSize.Height, 1);
                        this.spriteRenderer.CustomSize = Size.Empty;
                        double ratio = (wRatio + hRatio) / 2;
                        this.initialRatio = Math.Round(ratio, 2);
                        break;
                    }
                }
            }
            this.SetDirty();
        }

        public void SetAlignment(AlignmentMode alignment)
        {
            this.Alignment = alignment;
            if (this.Parent != null && this.Parent is ICanvas) {
                Vector2 position = this.Position;
                Vector2 pPos = this.Parent.Position;
                ICanvas pCanvas = (ICanvas)(this.Parent);
                Size pSize = pCanvas.Size;
                int spritePixelWidth = (int)(this.Sprite.Texture.Width * this.Sprite.Size.X);
                int spritePixelHeight = (int)(this.Sprite.Texture.Height * this.Sprite.Size.Y);

                if (this.RescaleMode != ScaleMode.Fill) {
                    switch (alignment) {
                        case AlignmentMode.Center: {
                            if (spritePixelWidth <= pSize.Width) {
                                position.X = pPos.X + ((pSize.Width - spritePixelWidth) / 2);
                            } else {
                                position.X = pPos.X + (pSize.Width / 2);
                            }
                            position.Y = (pPos.Y + (pSize.Height / 2)) - (spritePixelHeight / 2);
                            break;
                        }
                        case AlignmentMode.CenterLeft: {
                            if (spritePixelWidth <= pSize.Width) {
                                position.X = pPos.X + this.Padding.X;
                            } else {
                                position.X = pPos.X;
                            }
                            position.Y = (pPos.Y + (pSize.Height / 2)) - (spritePixelHeight / 2);
                            break;
                        }
                        case AlignmentMode.CenterRight: {
                            if (spritePixelWidth <= pSize.Width) {
                                position.X = pPos.X + (pSize.Width - spritePixelWidth - this.Padding.X);
                            } else {
                                position.X = pPos.X + (pSize.Width - spritePixelWidth);
                            }
                            position.Y = (pPos.Y + (pSize.Height / 2)) - (spritePixelHeight / 2);
                            break;
                        }
                        case AlignmentMode.BottomCenter: {
                            if (spritePixelWidth <= pSize.Width) {
                                position.X = pPos.X + ((pSize.Width - spritePixelWidth) / 2);
                            } else {
                                position.X = pPos.X + (pSize.Width / 2);
                            }
                            position.Y = pPos.Y + this.Padding.Y;
                            break;
                        }
                        case AlignmentMode.BottomLeft: {
                            if (spritePixelWidth <= pSize.Width) {
                                position.X = pPos.X + this.Padding.X;
                            } else {
                                position.X = pPos.X;
                            }
                            position.Y = pPos.Y + this.Padding.Y;
                            break;
                        }
                        case AlignmentMode.BottomRight: {
                            if (spritePixelWidth <= pSize.Width) {
                                position.X = pPos.X + (pSize.Width - spritePixelWidth - this.Padding.X);
                            } else {
                                position.X = pPos.X + (pSize.Width - spritePixelWidth);
                            }
                            position.Y = pPos.Y + this.Padding.Y;
                            break;
                        }
                        case AlignmentMode.TopCenter: {
                            if (spritePixelWidth <= pSize.Width) {
                                position.X = pPos.X + ((pSize.Width - spritePixelWidth) / 2);
                            } else {
                                position.X = pPos.X + (pSize.Width / 2);
                            }
                            position.Y = pPos.Y + (pSize.Height - this.Padding.Y);
                            break;
                        }
                        case AlignmentMode.TopLeft: {
                            if (spritePixelWidth <= pSize.Width) {
                                position.X = pPos.X + this.Padding.X;
                            } else {
                                position.X = pPos.X;
                            }
                            position.Y = pPos.Y + (pSize.Height - this.Padding.Y);
                            break;
                        }
                        case AlignmentMode.TopRight: {
                            if (spritePixelWidth <= pSize.Width) {
                                position.X = pPos.X + (pSize.Width - spritePixelWidth - this.Padding.X);
                            } else {
                                position.X = pPos.X + (pSize.Width - spritePixelWidth);
                            }
                            position.Y = pPos.Y + (pSize.Height - this.Padding.Y);
                            break;
                        }
                    }
                } else {
                    position = pPos;
                }
                this.Position = position;
                this.Sprite.Position = position;
                this.spriteRenderer.UpdateVertexBuffer();
            }
        }

        public void Destroy()
        {
            this.spriteRenderer = null;
            this.Sprite = null;
        }
    }
}
