using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using TagsCloudGenerator.Infrastructure;
using TagsCloudVisualization.Infrastructure.Common;

namespace TagsCloudVisualization.Core
{
    public class PictureBoxImageHolder : PictureBox, IImageHolder
    {
        public Size GetImageSize()
        {
            FailIfNotInitialized();
            return Image.Size;
        }

        public Graphics StartDrawing()
        {
            FailIfNotInitialized();
            return Graphics.FromImage(Image);
        }

        private void FailIfNotInitialized()
        {
            if (Image == null)
                throw new InvalidOperationException(
                    "Call PictureBoxImageHolder.RecreateImage before other method call!");
        }

        public void UpdateUi()
        {
            Refresh();
            Application.DoEvents();
        }

        public void RecreateImage(ImageSettings imageSettings)
        {
            Image = new Bitmap(imageSettings.Width, imageSettings.Height, PixelFormat.Format24bppRgb);
        }

        public void ResizeImage(ImageSettings imageSettings)
        {
            Image = ImageUtils.ResizeImage(Image, imageSettings.Width, imageSettings.Height);
        }

        public void SaveImage(string fileName, ImageFormat format)
        {
            FailIfNotInitialized();
            Image.Save(fileName, format);
        }
    }
}