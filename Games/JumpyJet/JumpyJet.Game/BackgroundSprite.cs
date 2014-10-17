﻿using SiliconStudio.Core.Mathematics;
using SiliconStudio.Paradox.Graphics;

namespace JumpyJet
{
    /// <summary>
    /// A custom sprite displays scrolling background image.
    /// </summary>
    public class BackgroundSprite
    {
        private readonly float depth;
        private readonly Int2 screenResolution;
        private readonly Vector2 screenCenter;

        // Texture
        private Texture texture;
        private Rectangle textureRegion;

        // First quad parameters
        private readonly Vector2 firstQuadPos;
        private Vector2 firstQuadOrigin;
        private Rectangle firstQuadRegion;

        // Second quad parameters
        private Vector2 secondQuadPos;
        private Vector2 secondQuadOrigin;
        private Rectangle secondQuadRegion;

        public bool IsUpdating { get; set; }
        public bool IsRunning { get; protected set; }
        public bool IsVisible { get; protected set; }
        public int ScrollPos { get; protected set; }
        public float ScrollWidth { get; protected set; }
        public float ScrollSpeed { get; protected set; }

        public BackgroundSprite(Sprite backgroundSprite, Vector3 screenVirtualResolution, float scrollSpeed, float depth, Vector2 startPos = default(Vector2))
        {
            screenResolution = new Int2((int)screenVirtualResolution.X, (int)screenVirtualResolution.Y);
            screenCenter = new Vector2(screenVirtualResolution.X / 2, screenVirtualResolution.Y /2);

            this.depth = depth;
            firstQuadPos = startPos;
            secondQuadPos = startPos;

            ScrollSpeed = scrollSpeed;
            ScrollPos = 0;

            CreateBackground(backgroundSprite.Texture, backgroundSprite.Region);
        }

        public void Update(float elapsedTime)
        {
            if (!IsUpdating)
                return;

            // Update Scroll position
            if (ScrollPos > textureRegion.Width)
                ScrollPos = 0;

            ScrollPos += (int)(elapsedTime * ScrollSpeed);

            UpdateSpriteQuads();
        }

        public void DrawSprite(SpriteBatch spriteBatch)
        {
            // DrawBackgroundParallax the first quad
            spriteBatch.Draw(texture, firstQuadPos + screenCenter, firstQuadRegion, Color.White, 0f, firstQuadOrigin, 1f, SpriteEffects.None, ImageOrientation.AsIs, depth);
            
            if (secondQuadRegion.Width > 0)
            {
                // DrawBackgroundParallax the second quad
                spriteBatch.Draw(texture, secondQuadPos + screenCenter, secondQuadRegion, Color.White, 0f, secondQuadOrigin, 1f, SpriteEffects.None, ImageOrientation.AsIs, depth);
            }
        }

        private void CreateBackground(Texture2D bgTexture, Rectangle texReg)
        {
            texture = bgTexture;
            textureRegion = texReg;

            // Set offset to rectangle
            firstQuadRegion.X = textureRegion.X;
            firstQuadRegion.Y = textureRegion.Y;

            firstQuadRegion.Width = (textureRegion.Width > screenResolution.X) ? screenResolution.X : textureRegion.Width;
            firstQuadRegion.Height = (textureRegion.Height > screenResolution.Y) ? screenResolution.Y : textureRegion.Height;

            // Centering the content
            firstQuadOrigin.X = 0.5f * firstQuadRegion.Width;
            firstQuadOrigin.Y = 0.5f * firstQuadRegion.Height;

            // Copy data from first quad to second one
            secondQuadRegion = firstQuadRegion;
            secondQuadOrigin = firstQuadOrigin;
        }

        private void UpdateSpriteQuads()
        {
            // Update first Quad
            var firstQuadNewWidth = textureRegion.Width - ScrollPos;
            firstQuadRegion.Width = firstQuadNewWidth;
            // Update X position of the first Quad
            firstQuadRegion.X = ScrollPos;

            // Update second Quad
            // Calculate new X position and width of the second quad
            var secondQuadNewWidth = (ScrollPos + screenResolution.X) - textureRegion.Width;
            var secondQuadNewXPosition = (screenResolution .X/ 2 - secondQuadNewWidth) + secondQuadNewWidth / 2;

            secondQuadRegion.Width = secondQuadNewWidth;
            secondQuadPos.X = secondQuadNewXPosition;
            secondQuadOrigin.X = secondQuadNewWidth / 2f;
        }
    }
}
