using AForge;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;
using GlyphRecognition;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCDM.Reconitions
{
    class GlyphImageProcessor
    {
        // glyph recognizer to use for glyph recognition in video
        private GlyphRecognizer recognizer = new GlyphRecognizer(5);
        // glyph tracker to track position of glyphs
        private GlyphTracker glyphTracker = new GlyphTracker();

        // quadrilateral transformation used to put image in place of glyph
        private BackwardQuadrilateralTransformation quadrilateralTransformation = new BackwardQuadrilateralTransformation();

        // default font to highlight glyphs
        private Font defaultFont = new Font(FontFamily.GenericSerif, 15, FontStyle.Bold);

        // object used for synchronization
        private object sync = new object();

        // Database of glyphs to recognize
        public GlyphDatabase GlyphDatabase
        {
            get { return recognizer.GlyphDatabase; }
            set
            {
                lock (sync)
                {
                    recognizer.GlyphDatabase = value;
                }
            }
        }
        // Glyphs' visualization type
        public VisualizationType VisualizationType
        {
            get { return visualizationType; }
            set
            {
                lock (sync)
                {
                    visualizationType = value;
                }
            }
        }
        // Effective focal length of camera
        private VisualizationType visualizationType = VisualizationType.Name;
        public float CameraFocalLength
        {
            get { return glyphTracker.CameraFocalLength; }
            set { glyphTracker.CameraFocalLength = value; }
        }
        public float GlyphSize
        {
            get { return glyphTracker.GlyphSize; }
            set { glyphTracker.GlyphSize = value; }
        }
        public List<ExtractedGlyphData> ProcessImage(Bitmap bitmap, out string name)
        {
            List<ExtractedGlyphData> glyphs = new List<ExtractedGlyphData>();
            name = "";
            lock (sync)
            {
                glyphTracker.ImageSize = bitmap.Size;

                // get list of recognized glyphs
                glyphs.AddRange(recognizer.FindGlyphs(bitmap));
                List<int> glyphIDs = glyphTracker.TrackGlyphs(glyphs);

                if (glyphs.Count > 0)
                {
                    if ((visualizationType == VisualizationType.BorderOnly) ||
                         (visualizationType == VisualizationType.Name))
                    {
                        Graphics g = Graphics.FromImage(bitmap);
                        int i = 0;

                        // highlight each found glyph
                        foreach (ExtractedGlyphData glyphData in glyphs)
                        {
                            List<IntPoint> glyphPoints = (glyphData.RecognizedGlyph == null) ?
                                glyphData.Quadrilateral : glyphData.RecognizedQuadrilateral;

                            Pen pen = new Pen(Color.Red, 3);

                            // highlight border
                            g.DrawPolygon(pen, ToPointsArray(glyphPoints));

                            string glyphTitle = null;

                            // prepare glyph's title
                            if ((visualizationType == VisualizationType.Name) && (glyphData.RecognizedGlyph != null))
                            {
                                glyphTitle = string.Format("{0}: {1}",
                                    glyphIDs[i], glyphData.RecognizedGlyph.Name);
                                name = glyphData.RecognizedGlyph.Name;


                            }
                            else
                            {
                                glyphTitle = string.Format("Mascota no idedntificada");

                            }



                            // show glyph's title
                            if (!string.IsNullOrEmpty(glyphTitle))
                            {
                                // get glyph's center point
                                IntPoint minXY, maxXY;
                                PointsCloud.GetBoundingRectangle(glyphPoints, out minXY, out maxXY);
                                IntPoint center = (minXY + maxXY) / 2;

                                // glyph's name size
                                SizeF nameSize = g.MeasureString(glyphTitle, defaultFont);

                                // paint the name
                                Brush brush = new SolidBrush(pen.Color);

                                g.DrawString(glyphTitle, defaultFont, brush,
                                    new System.Drawing.Point(center.X - (int)nameSize.Width / 2, center.Y - (int)nameSize.Height / 2));

                                brush.Dispose();
                            }

                            i++;
                            pen.Dispose();
                        }
                    }

                }
            }

            return glyphs;
        }
        public void Reset()
        {
            glyphTracker.Reset();
        }

        #region Helper methods
        // Convert list of AForge.NET framework's points to array of .NET's points
        private System.Drawing.Point[] ToPointsArray(List<IntPoint> points)
        {
            int count = points.Count;
            System.Drawing.Point[] pointsArray = new System.Drawing.Point[count];

            for (int i = 0; i < count; i++)
            {
                pointsArray[i] = new System.Drawing.Point(points[i].X, points[i].Y);
            }

            return pointsArray;
        }
        #endregion
    }
}
