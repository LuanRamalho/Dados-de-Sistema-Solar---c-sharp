using System;

namespace SistemaSolarApp
{
    public class Planeta
    {
        public string Id { get; set; } = Guid.NewGuid().ToString().Substring(0, 8);
        public string Nome { get; set; }
        public string Rotacao { get; set; }
        public string Translacao { get; set; }
        public string Diametro { get; set; }
        public string Temperatura { get; set; }
        public string Distancia { get; set; }
        public string Imagem { get; set; }
    }
}