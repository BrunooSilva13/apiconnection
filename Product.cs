using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace apiConnection
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public float Preco { get; set; }
        public bool Status {get; set; }
        

    }
}