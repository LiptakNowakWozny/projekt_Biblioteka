using Biblioteka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Biblioteka.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AllBooks()
        {
            KsiazkiEntities1 db = new KsiazkiEntities1();
            var ksiazki = db.Ksiazki;

            List<ActionInMemory> Action = new List<ActionInMemory>();
            KsiazkiActionDB ksiazkiActionDB = new KsiazkiActionDB();
            Action = ksiazkiActionDB.ReadMemory();
            List<KsiazkiDoWyswietlenia> doWyswietlenia = new List<KsiazkiDoWyswietlenia>();
            foreach(var ksiazka in ksiazki)
            {
                KsiazkiDoWyswietlenia ks = new KsiazkiDoWyswietlenia();
                ks.Id = ksiazka.Id;
                ks.Tytul = ksiazka.Tytul;
                ks.Autor = ksiazka.Autor;
                ks.Liczba_Stron = ksiazka.Liczba_Stron;
                doWyswietlenia.Add(ks);
            }
            foreach(var ksiazka in Action)
            {
                switch (ksiazka.ActionType)
                {
                    case "edit":
                        foreach(var item in doWyswietlenia)
                        {
                            if (item.Id == ksiazka.Id)
                            {
                                item.Tytul = ksiazka.Tytul;
                                item.Autor = ksiazka.Autor;
                                item.Liczba_Stron = ksiazka.Liczba_Stron;
                            }
                        }
                        break;
                    case "delete":
                        KsiazkiDoWyswietlenia NieDoWyswietlenia = new KsiazkiDoWyswietlenia();
                        foreach (var item in doWyswietlenia)
                        {
                            if (item.Id == ksiazka.Id)
                            {
                                NieDoWyswietlenia = item;
                            }
                        }
                        doWyswietlenia.Remove(NieDoWyswietlenia);
                        break;
                    case "insert":
                        KsiazkiDoWyswietlenia ks = new KsiazkiDoWyswietlenia();
                        ks.Id = ksiazka.Id;
                        ks.Tytul = ksiazka.Tytul;
                        ks.Autor = ksiazka.Autor;
                        ks.Liczba_Stron = ksiazka.Liczba_Stron;
                        doWyswietlenia.Add(ks);
                        break;
                    default:
                        break;
                }
            }

            return View(doWyswietlenia);

        }

        public ActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Insert(Ksiazki ksiazki)
        {

            KsiazkiActionDB ksiazkiActionDB = new KsiazkiActionDB();
            ksiazkiActionDB.InsertMemory(ksiazki);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {

            KsiazkiActionDB ksiazkiActionDB = new KsiazkiActionDB();
            ksiazkiActionDB.DeleteMemory(id);

            return RedirectToAction("AllBooks");
        }

        public ActionResult Edit (int? id)
        {
            //ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        public ActionResult Edit (Ksiazki ksiazka)
        {
            KsiazkiActionDB ksiazkiActionDB = new KsiazkiActionDB();
            ksiazkiActionDB.EditMemory(ksiazka);
            return RedirectToAction("AllBooks");
        }

        public ActionResult Back()
        {
            List<ActionInMemory> Action = new List<ActionInMemory>();
            KsiazkiActionDB ksiazkiActionDB = new KsiazkiActionDB();
            Action = ksiazkiActionDB.ReadMemory();

            return View(Action);
        }

        public ActionResult Cofnij (int id)
        {
            List<ActionInMemory> Action = new List<ActionInMemory>();
            KsiazkiActionDB ksiazkiActionDB = new KsiazkiActionDB();
            Action = ksiazkiActionDB.ReadMemory();
            ActionInMemory AkcjaDoUsuniecia = new ActionInMemory();
            foreach (var ksiazka in Action)
            {
                if(ksiazka.Id == id)
                {
                    AkcjaDoUsuniecia = ksiazka;
                }
            }
            Action.Remove(AkcjaDoUsuniecia);
            ksiazkiActionDB.Write();
            return RedirectToAction("Back");
        }
    }
}