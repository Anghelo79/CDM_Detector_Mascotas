using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlyphRecognition
{
   public class Glyph
    {

        public string Name { set; get; }
        public string Data { set; get; }



        public Glyph()
        {

        }
        public Glyph(string name, byte[,] data)
        {


            Name = name;
            StringBuilder sb = new StringBuilder();
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    sb.Append(data[x, y]);
                }
            }
            Data = sb.ToString();

        }

        public object Clone()
        {
            Glyph clone = new Glyph(Name, (byte[,])GlyphDataFromString().Clone());



            return clone;
        }
        public byte[,] GlyphDataFromString()
        {
            byte[,] glyphData = new byte[5, 5];

            for (int i = 0, k = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++, k++)
                {
                    glyphData[i, j] = (byte)((Data[k] == '0') ? 0 : 1);
                }
            }

            return glyphData;
        }


        public int CheckForMatching(byte[,] rawGlyphData)
        {
            int size = rawGlyphData.GetLength(0);
            byte[,] data = GlyphDataFromString();
            int sizeM1 = size - 1;

            bool match1 = true;
            bool match2 = true;
            bool match3 = true;
            bool match4 = true;

            for (int i = 0; i < rawGlyphData.GetLength(0); i++)
            {
                for (int j = 0; j < rawGlyphData.GetLength(1); j++)
                {
                    byte value = rawGlyphData[i, j];

                    // no rotation
                    match1 &= (value == data[i, j]);
                    // 180 deg
                    match2 &= (value == data[sizeM1 - i, sizeM1 - j]);
                    // 90 deg
                    match3 &= (value == data[sizeM1 - j, i]);
                    // 270 deg
                    match4 &= (value == data[j, sizeM1 - i]);
                }
            }

            if (match1)
                return 0;
            else if (match2)
                return 180;
            else if (match3)
                return 90;
            else if (match4)
                return 270;

            return -1;
        }


        public static int CheckForMatching(byte[,] sourceGlyph, byte[,] targetGlyph)
        {
            int size = sourceGlyph.GetLength(0);

            if (size != sourceGlyph.GetLength(1))
            {
                throw new ArgumentException("Invalid glyph data array - must be square.", "rawGlyphData");
            }

            if (targetGlyph.GetLength(0) != targetGlyph.GetLength(1))
            {
                throw new ArgumentException("Invalid glyph data array - must be square.", "targetGlyph");
            }

            if (size != targetGlyph.GetLength(0))
                return -1;

            int sizeM1 = size - 1;

            bool match1 = true;
            bool match2 = true;
            bool match3 = true;
            bool match4 = true;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    byte value = sourceGlyph[i, j];

                    // no rotation
                    match1 &= (value == targetGlyph[i, j]);
                    // 180 deg
                    match2 &= (value == targetGlyph[sizeM1 - i, sizeM1 - j]);
                    // 90 deg
                    match3 &= (value == targetGlyph[sizeM1 - j, i]);
                    // 270 deg
                    match4 &= (value == targetGlyph[j, sizeM1 - i]);
                }
            }

            if (match1)
                return 0;
            else if (match2)
                return 180;
            else if (match3)
                return 90;
            else if (match4)
                return 270;

            return -1;
        }

        public static bool CheckIfRotationInvariant(byte[,] rawGlyphData)
        {
            int size = rawGlyphData.GetLength(0);

            if (size != rawGlyphData.GetLength(1))
            {
                throw new ArgumentException("Invalid glyph data array - must be square.");
            }

            int sizeM1 = size - 1;

            bool match2 = true;
            bool match3 = true;
            bool match4 = true;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    byte value = rawGlyphData[i, j];

                    // 180 deg
                    match2 &= (value == rawGlyphData[sizeM1 - i, sizeM1 - j]);
                    // 90 deg
                    match3 &= (value == rawGlyphData[sizeM1 - j, i]);
                    // 270 deg
                    match4 &= (value == rawGlyphData[j, sizeM1 - i]);
                }
            }

            return (match2 || match3 || match4);
        }


        public static bool CheckIfEveryRowColumnHasValue(byte[,] rawGlyphData)
        {
            int size = rawGlyphData.GetLength(0);

            if (size != rawGlyphData.GetLength(1))
            {
                throw new ArgumentException("Invalid glyph data array - must be square.");
            }

            int sizeM1 = size - 1;

            byte[] rows = new byte[sizeM1];
            byte[] cols = new byte[sizeM1];

            for (int i = 1; i < sizeM1; i++)
            {
                for (int j = 1; j < sizeM1; j++)
                {
                    byte value = rawGlyphData[i, j];

                    rows[i] |= value;
                    cols[j] |= value;
                }
            }

            for (int i = 1; i < sizeM1; i++)
            {
                if ((rows[i] == 0) || (cols[i] == 0))
                    return false;
            }

            return true;
        }


        public static bool CheckIfGlyphHasBorder(byte[,] rawGlyphData)
        {
            int size = rawGlyphData.GetLength(0);

            if (size != rawGlyphData.GetLength(1))
            {
                throw new ArgumentException("Invalid glyph data array - must be square.");
            }

            int sizeM1 = size - 1;

            for (int i = 0; i <= sizeM1; i++)
            {
                if (rawGlyphData[0, i] == 1)
                    return false;
                if (rawGlyphData[sizeM1, i] == 1)
                    return false;

                if (rawGlyphData[i, 0] == 1)
                    return false;
                if (rawGlyphData[i, sizeM1] == 1)
                    return false;
            }

            return true;
        }
    }
}
