using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Biblioteka.Models
{
    public class KsiazkiDoWyswietlenia
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        public string Tytul { get; set; }
        public string Autor { get; set; }
        public int? Liczba_Stron { get; set; }
    }
}