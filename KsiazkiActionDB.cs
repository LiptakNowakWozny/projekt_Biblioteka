using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Biblioteka.Models
{
    public class KsiazkiActionDB
    {
        List<ActionInMemory> Action = new List<ActionInMemory>();
        string fName = "Memory.dat";

        public void Save (ActionInMemory actionInMemory)
        {
            Ksiazki ksiazka = new Ksiazki();
            ksiazka.Tytul = actionInMemory.Tytul;
            ksiazka.Autor = actionInMemory.Autor;
            ksiazka.Liczba_Stron = actionInMemory.Liczba_Stron;

            switch (actionInMemory.ActionType)
            {
                case "insert":
                    Insert(ksiazka);
                    break;
                case "edit":
                    Edit(ksiazka);
                    break;
                case "delete":
                    Delete(ksiazka.Id);
                    break;
                default:
                    break;
            }
        }

        public void Insert (Ksiazki ksiazki)
        {
            KsiazkiEntities1 db = new KsiazkiEntities1();
            db.Ksiazki.Add(ksiazki);
            db.SaveChanges();
        }

        public void Delete (int? id)
        {
            KsiazkiEntities1 db = new KsiazkiEntities1();
            if (id != null && id > 0)
            {
                db.Ksiazki.Remove(db.Ksiazki.Find(id));
            }
            db.SaveChanges();
        }

        public void Edit(Ksiazki ksiazka)
        {
            KsiazkiEntities1 db = new KsiazkiEntities1();
            db.Ksiazki.Find(ksiazka.Id).Autor = ksiazka.Autor;
            db.Ksiazki.Find(ksiazka.Id).Tytul = ksiazka.Tytul;
            db.Ksiazki.Find(ksiazka.Id).Liczba_Stron = ksiazka.Liczba_Stron;
            db.SaveChanges();
        }

        public void InsertMemory (Ksiazki ksiazka)
        {
            Read();
            ActionInMemory actionInMemory = new ActionInMemory();
            actionInMemory.Tytul = ksiazka.Tytul;
            actionInMemory.Autor = ksiazka.Autor;
            actionInMemory.Liczba_Stron = ksiazka.Liczba_Stron;
            actionInMemory.ActionType = "insert";
            if(Action.Count == 5)
            {
                Save(Action.First());
                Action.RemoveAt(0);
            }
            Action.Add(actionInMemory);
            Write();
        }

        public void EditMemory(Ksiazki ksiazka)
        {
            Read();
            ActionInMemory actionInMemory = new ActionInMemory();
            actionInMemory.Id = ksiazka.Id;
            actionInMemory.Tytul = ksiazka.Tytul;
            actionInMemory.Autor = ksiazka.Autor;
            actionInMemory.Liczba_Stron = ksiazka.Liczba_Stron;
            actionInMemory.ActionType = "edit";
            if (Action.Count == 5)
            {
                Save(Action.First());
                Action.RemoveAt(0);
            }
            Action.Add(actionInMemory);
            Write();
        }

        public void DeleteMemory(int? id)
        {
            Read();
            ActionInMemory actionInMemory = new ActionInMemory();
            Ksiazki ksiazka = new Ksiazki();
            KsiazkiEntities1 db = new KsiazkiEntities1();
            if (id != null && id > 0)
            {
                ksiazka = db.Ksiazki.Find(id);
            }
            actionInMemory.Id = ksiazka.Id;
            actionInMemory.Tytul = ksiazka.Tytul;
            actionInMemory.Autor = ksiazka.Autor;
            actionInMemory.Liczba_Stron = ksiazka.Liczba_Stron;
            actionInMemory.ActionType = "delete";
            if (Action.Count == 5)
            {
                Save(Action.First());
                Action.RemoveAt(0);
            }
            Action.Add(actionInMemory);
            Write();
        }

        public void Write()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using(Stream fStream = new FileStream(fName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                binaryFormatter.Serialize(fStream, Action);
            }
        }

        public void Read()
        {
            Action.Clear();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using(Stream fStream = File.OpenRead(fName))
            {
                try{
                    Action = (List<ActionInMemory>)binaryFormatter.Deserialize(fStream);
                }
                catch(Exception ex) { };
            }
        }

        public List<ActionInMemory> ReadMemory()
        {
            Action.Clear();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (Stream fStream = File.OpenRead(fName))
            {
                try
                {
                    Action = (List<ActionInMemory>)binaryFormatter.Deserialize(fStream);
                }
                catch (Exception ex) { };
            }

            return Action;
        }
    }
}