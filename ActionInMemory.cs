using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biblioteka.Models
{
    [Serializable]
    public class ActionInMemory
    {
        public int Id { get; set; }
        public string Tytul { get; set; }
        public string Autor { get; set; }
        public int? Liczba_Stron { get; set; }
        public string ActionType { get; set; }
    }
}