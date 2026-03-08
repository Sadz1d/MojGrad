using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Market.Application.Common.Helpers;

public static class ImageCompressor
{
    /// <summary>
    /// Resizes and compresses an image stream.
    /// Always outputs JPEG regardless of input format.
    /// </summary>
    /// <param name="inputStream">Original image stream</param>
    /// <param name="maxWidth">Max width in pixels (height scales proportionally)</param>
    /// <param name="maxHeight">Max height in pixels (width scales proportionally)</param>
    /// <param name="quality">JPEG quality 1-100</param>
    /// <returns>Compressed image as MemoryStream</returns>
    public static async Task<MemoryStream> CompressAsync(
        Stream inputStream,
        int maxWidth,
        int maxHeight,
        int quality = 80)
    {
        using var image = await Image.LoadAsync(inputStream);

        // Only downscale, never upscale
        if (image.Width > maxWidth || image.Height > maxHeight)
        {
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(maxWidth, maxHeight),
                Mode = ResizeMode.Max
            }));
        }

        var output = new MemoryStream();
        var encoder = new JpegEncoder { Quality = quality };
        await image.SaveAsync(output, encoder);
        output.Position = 0;
        return output;
    }
}