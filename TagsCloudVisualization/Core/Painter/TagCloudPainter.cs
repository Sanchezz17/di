using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using TagsCloudGenerator.Core.Drawers;
using TagsCloudGenerator.Core.Translators;
using TagsCloudGenerator.Infrastructure;
using TagsCloudVisualization.Infrastructure.Common;

namespace TagsCloudVisualization.Core.Painter
{
    public class TagCloudPainter
    {
        public delegate TagCloudPainter Factory(TagCloudSettings tagCloudSettings);

        private readonly IImageHolder imageHolder;
        private readonly TagCloudSettings tagCloudSettings;
        private readonly ImageSettings imageSettings;
        private readonly Palette palette;

        public TagCloudPainter(IImageHolder imageHolder,
            TagCloudSettings tagCloudSettings, Palette palette, ImageSettings imageSettings)
        {
            this.imageHolder = imageHolder;
            this.tagCloudSettings = tagCloudSettings;
            this.palette = palette;
            this.imageSettings = imageSettings;
        }

        public void Paint()
        {
            using (var graphics = imageHolder.StartDrawing())
            {
                var translator = TextToTagsTranslatorFactory
                    .Create(tagCloudSettings.SpiralAlpha, tagCloudSettings.StepPhi);
                var tags = translator.TranslateTextToTags(
                    File.ReadLines(tagCloudSettings.TextFilename),
                    new HashSet<string>(),
                    tagCloudSettings.FontFamily,
                    tagCloudSettings.MinFontSize);

                var cloudDrawer =
                    new RectangleCloudDrawer(palette.BackgroundColor, new SolidBrush(palette.PrimaryColor));
                var bitmap = cloudDrawer.DrawCloud(tags.ToList());
                bitmap = ImageUtils.ResizeImage(bitmap, imageSettings.Width, imageSettings.Height);
                graphics.DrawImage(bitmap, 0, 0);
            }

            imageHolder.UpdateUi();
        }
    }
}