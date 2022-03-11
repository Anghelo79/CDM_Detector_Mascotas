using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlyphRecognition
{
    public class GlyphDatabase :ICollection
    {

        public string CollectionName;
        private List<Glyph> glyphArray = new List<Glyph>();
        public Glyph this[int index]
        {
            get { return (Glyph)glyphArray[index]; }
        }
        public void CopyTo(Array a, int index)
        {
            glyphArray.CopyTo((Glyph[])a, index);
        }


        public int Size
        {

            get { return new byte[5, 5].GetLength(0); }
        }

        public int Count
        {
            get { return glyphArray.Count; }
        }
        public object SyncRoot
        {
            get { return this; }
        }
        public bool IsSynchronized
        {
            get { return false; }
        }
        public IEnumerator GetEnumerator()
        {
            return glyphArray.GetEnumerator();
        }

        public void Add(Glyph newGlyph)
        {
            glyphArray.Add(newGlyph);
        }

        public Glyph RecognizeGlyph(byte[,] rawGlyphData, out int rotation)
        {
            for (int i = 0; i < Count; i++)
            {
                if ((rotation = glyphArray[i].CheckForMatching(rawGlyphData)) != -1)
                {
                    return glyphArray[i];
                }
            }

            rotation = -1;
            return null;
        }
    }
}
