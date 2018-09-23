using System;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace ConsoleApp1
{
        [Serializable]
        public class Capability
        {
            public Layer Layer { get; set; }

            public Capability(){}
        }

        [Serializable]
        public class Layer
        {
            public string Name { get; set; }
            public string Title { get; set; }
            [XmlElement("Layer")]
            public Layer[] SubLayers { get; set; }
            public Attribute[] Attributes { get; set; }

            public Layer() { }

        }

        [Serializable]
        public class Attribute
        {
            [XmlAttribute("name")]
            public string Name { get; set; }
            [XmlAttribute("type")]
            public string Type { get; set; }

            public Attribute(){}
        }

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Не указан путь до файла");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Write input filename with extension: ");
            string inputFileName = Console.ReadLine();
            Console.WriteLine("Write output filename with extension: ");
            string outputFileName = Console.ReadLine();
            string jsonText = string.Empty;

            try
            {
                using (FileStream sr = new FileStream(args[0] + inputFileName, FileMode.OpenOrCreate))
                {
                    XmlSerializer formatter = new XmlSerializer(typeof(Capability));
                    var xmlText = formatter.Deserialize(sr);
                    jsonText = JsonConvert.SerializeObject(xmlText, Formatting.Indented,
                        new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
                    Console.WriteLine(jsonText);
                }

                using (FileStream fstream = new FileStream($"{args[0]}{outputFileName}", FileMode.OpenOrCreate))
                {
                    byte[] array = System.Text.Encoding.Default.GetBytes(jsonText);
                    fstream.Write(array, 0, array.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR " + ex.Message);
                Console.ReadLine();
            }
            Console.ReadLine();
        }
    }
}