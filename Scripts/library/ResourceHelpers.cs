using Godot;
using System;

namespace ResourceHelpers
{
    public partial class ImageLoader
    {
        public ImageTexture LoadImageToTexture(string ImagePath, int StellarObjectSize)
        {
            ImageTexture setImageTexture = new ImageTexture();
            Texture2D texture2D = ResourceLoader.Load<Texture2D>(ImagePath);
            Image img = texture2D.GetImage();

            img.Resize(StellarObjectSize, StellarObjectSize, Image.Interpolation.Bilinear);

            setImageTexture.SetImage(img);

            return setImageTexture;
        }
    }
}
