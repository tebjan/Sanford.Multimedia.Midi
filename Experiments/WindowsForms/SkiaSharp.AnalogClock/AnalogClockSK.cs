

namespace SkiaSharp.AnalogClock
{
    public class AnalogClockSK : Control
    {
        // Configuration for blackFillPaint
        SKPaint blackFillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Black,
            IsAntialias = true
        };

        // Configuration for whiteStrokePaint
        SKPaint whiteStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.White,
            StrokeWidth = 2,
            StrokeCap = SKStrokeCap.Round,
            IsAntialias = true
        };

        // Configuration for blackStrokePaint
        SKPaint blackStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Black,
            StrokeWidth = 2,
            StrokeCap = SKStrokeCap.Round,
            IsAntialias = true
        };

        // Configuration for grayStrokePaint
        SKPaint grayStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Gray,
            StrokeWidth = 2,
            StrokeCap = SKStrokeCap.Round,
            IsAntialias = true
        };

        // Configuration for redStrokePaint
        SKPaint redStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Red,
            StrokeWidth = 6,
            StrokeCap = SKStrokeCap.Round,
            IsAntialias = true
        };

        // Configuration for whiteFillPaint
        SKPaint whiteFillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.White,
            IsAntialias = true
        };

        // Configuration for grayFillPaint
        SKPaint grayFillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Gray,
            IsAntialias = true
        };

        // Configuration for blackText
        SKPaint blackText = new SKPaint
        {
            Style = SKPaintStyle.StrokeAndFill,
            Color = SKColors.Black,
            TextSize = 25,
            Typeface = SKTypeface.FromFamilyName(
                "Calibri",
                SKFontStyleWeight.Normal,
                SKFontStyleWidth.Normal,
                SKFontStyleSlant.Upright),
            IsAntialias = true
        };

        // Configurations for hourHandPath and minuteHandPath drawings
        SKPath hourHandPath = SKPath.ParseSvgPathData(
            "M 0 -60 C 0 -30 20 -30 5 -20 L 5 0 C 5 7.5 -5 7.5 -5 0 L -5 -20 C -20 -30 0 -30 0 -60");
        SKPath minuteHandPath = SKPath.ParseSvgPathData(
            "M 0 -80 C 0 -75 0 -70 2.5 -60 L 2.5 0 C 2.5 5 -2.5 5 -2.5 0 L -2.5 -60 C 0 -70 0 -75 0 -80");

        
        
        private void canvasView_PaintSurface(object sender, EventArgs e, PictureBox PictureBoxClockSK)
        {
            SKImageInfo ImgInfo = new SKImageInfo(PictureBoxClockSK.Size.Width, PictureBoxClockSK.Size.Height);
            SKSurface surface = SKSurface.Create(ImgInfo);

            SKCanvas canvas = surface.Canvas;

            canvas.Clear(SKColors.CornflowerBlue);


            // Set transforms
            canvas.Translate(2, 2);
            canvas.Scale(300f);

            // Get DateTime
            DateTime dateTime = DateTime.Now;

            // Clock background
            canvas.DrawCircle(0, 0, 100, whiteFillPaint);

            // Hour and minute marks
            for (int angle = 0; angle < 360; angle += 6)
            {
                canvas.DrawCircle(0, -90, angle % 30 == 0 ? 4 : 2, blackFillPaint);
                canvas.RotateDegrees(6);
            }

            // Hour numbers
            for (int angle = 0; angle < 360; angle += 360)
            {
                canvas.DrawText("12", -12, -65, blackText);
                canvas.DrawText("1", 32, -52, blackText);
                canvas.DrawText("2", 58, -25, blackText);
                canvas.DrawText("3", 70, 8, blackText);
                canvas.DrawText("4", 58, 42, blackText);
                canvas.DrawText("5", 34, 68, blackText);
                canvas.DrawText("6", -6, 81, blackText);
                canvas.DrawText("7", -45, 68, blackText);
                canvas.DrawText("8", -70, 42, blackText);
                canvas.DrawText("9", -82, 8, blackText);
                canvas.DrawText("10", -75, -25, blackText);
                canvas.DrawText("11", -50, -52, blackText);
            }

            // Hour hand
            canvas.Save();
            canvas.RotateDegrees(30 * dateTime.Hour + dateTime.Minute / 2f + dateTime.Second / 120f);
            canvas.DrawPath(hourHandPath, blackFillPaint);
            canvas.DrawPath(hourHandPath, grayStrokePaint);
            //     whiteStrokePaint.StrokeWidth = 15;
            //     canvas.DrawLine(0, 0, 0, -50, whiteStrokePaint);
            canvas.Restore();

            // Minute hand
            canvas.Save();
            canvas.RotateDegrees(6 * dateTime.Minute + dateTime.Second / 10f);
            canvas.DrawPath(minuteHandPath, blackFillPaint);
            canvas.DrawPath(minuteHandPath, grayStrokePaint);
            //     whiteStrokePaint.StrokeWidth = 10;
            //     canvas.DrawLine(0, 0, 0, -70, whiteStrokePaint);
            canvas.Restore();

            // Second hand
            canvas.Save();
            float seconds = dateTime.Second + dateTime.Millisecond / 100000f;
            canvas.RotateDegrees(6 * seconds);
            //     whiteStrokePaint.StrokeWidth = 2;
            canvas.DrawLine(0, 10, 0, -80, blackStrokePaint);
            canvas.DrawLine(0, 0, 0, 0, redStrokePaint);
            canvas.Restore();

            // Image display code
            using (SKImage Img = surface.Snapshot())
            using (SKData data = Img.Encode(SKEncodedImageFormat.Png, 100))

            // Windows Form code (PictureBox is required, no methods have been found that don't rely on it, unfortunately)
            using (MemoryStream mStream = new MemoryStream(data.ToArray()))
            {
                Bitmap bm = new Bitmap(mStream, false);
                PictureBoxClockSK.Image = bm;
            }
        }
    }
}