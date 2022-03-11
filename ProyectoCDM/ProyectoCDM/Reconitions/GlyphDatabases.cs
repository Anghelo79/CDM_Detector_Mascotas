using GlyphRecognition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProyectoCDM.Reconitions
{
    public class GlyphDatabases
    {
        static string Archivo = System.IO.Path.Combine(Environment.GetFolderPath
           (Environment.SpecialFolder.ApplicationData), "Codigo_CDM_Mascotas ", "DataMascota.xml");
        GlyphDatabase listaDataGlyph;

        public GlyphDatabases()
        {
            if (File.Exists(Archivo))
            {
                XmlSerializer xml = new XmlSerializer(typeof(GlyphDatabase));
                using (StreamReader sr = new StreamReader(Archivo))
                {
                    listaDataGlyph = (GlyphDatabase)xml.Deserialize(sr);

                }

            }
            else
            {
                listaDataGlyph = new GlyphDatabase();
                listaDataGlyph.CollectionName = "Glyphs";
            }
        }
        public GlyphDatabase GlyphDatabase { get { return listaDataGlyph; } }

        public void AddGlyph(string name, byte[,] matris)
        {
            XmlSerializer xml = new XmlSerializer(typeof(GlyphDatabase));
            listaDataGlyph.Add(new Glyph(name, matris));
            using (StreamWriter writer = new StreamWriter(Archivo))
            {
                xml.Serialize(writer, listaDataGlyph);

            }
            loadGlyphs();

        }

        public void loadGlyphs()
        {
            XmlSerializer xml = new XmlSerializer(typeof(GlyphDatabase));
            using (StreamReader sr = new StreamReader(Archivo))
            {
                listaDataGlyph = (GlyphDatabase)xml.Deserialize(sr);

            }

        }
    }
}
