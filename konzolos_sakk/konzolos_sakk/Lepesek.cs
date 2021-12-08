using System;
using System.Collections.Generic;
using System.Linq;

namespace konzolos_sakk
{
	public class Lepesek
	{
        public Babu Babu
        {
            private set;
            get;
        }

        public int X
        {
            private set;
            get;
        }

        public int Y
        {
            private set;
            get;
        }

        private List<Tabla.Cella> LehetsegesLepesek;

        public IEnumerable<Tabla.Cella> LehetsegesLepesekBe(bool UthetoEllenseg = true)
        {
            if (LehetsegesLepesek.Count == 0)
                yield break;

            for (int i = 0; i < LehetsegesLepesek.Count - 1; i++)
            {
                yield return LehetsegesLepesek[i];
            }

            if (LehetsegesLepesek.Last().Babu == null)
                yield return LehetsegesLepesek.Last();
            else if (UthetoEllenseg && LehetsegesLepesek.Last().Babu.Szin != Babu.Szin)
                yield return LehetsegesLepesek.Last();
        }

        public int LehetsegesLepesekBe(bool UthetoEllenseg = true)
        {
            if (LehetsegesLepesek.Count == 0)
                return 0;

            if (LehetsegesLepesek.Last().Babu == null)
                return LehetsegesLepesek.Count;
            else if (!UthetoEllenseg || LehetsegesLepesek.Last().Babu.Szin == Babu.Szin)
                return LehetsegesLepesek.Count - 1;
            else
                return LehetsegesLepesek.Count;
        }

        public int Lehetosegek
        {
            private set;
            get;
        }

        private bool utesFrissites;

        public Lepesek(Babu babu, int x, int y, int Lehetosegek = 8, bool utesFrissites = true)
        {
            Babu = babu;
            X = x;
            Y = y;
            Utesfrissites = utesFrissites;
            this.utesFrissites = utesFrissites;

            LehetsegesLepesek = new List<Tabla.Cella>();
            LehetsegesLepesek.AddRange(babu.Szulo.OpenLineOfSight(x, y, Lehetosegek));

            foreach (Tabla.Cella lepj in LehetsegesLepesek)
            {
                if (utesFrissites)
                    lepj.UtesCellara.Add(Babu);
            }
        }

        public bool Blokkolte(Tabla.Cella honnan, Tabla.Cella hova, Tabla.Cella blokk)
        {
            if (LehetsegesLepesek.Contains(blokk) && !LehetsegesLepesek.Contains(hova))
            {
                return false;
            }
            else if (LehetsegesLepesek.Contains(honnan))
            {
                int szam = LehetsegesLepesek.IndexOf(hova);
                if (0 <= szam && szam < LehetsegesLepesek.Count - 1)
                    return true;
                else
                {
                    foreach (Tabla.Cella lepj in honnan.OpenLineOfSight(X, Y, Lehetosegek - LehetsegesLepesek.Count))
                    {
                        if (lepj == hova)
                            return true;
                        if (lepj == blokk)
                            return false;
                    }
                }
            }
            return true;
        }
    }
}


