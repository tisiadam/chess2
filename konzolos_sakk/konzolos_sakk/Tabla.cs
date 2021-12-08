using konzolos_sakk.Babbuk;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace konzolos_sakk
{

    public class Tabla
    {
        public class Cella
        {
 
            public Tabla Szulo
            {
                private set;
                get;
            }

            public int X;
            public int Y;
            public Babu Babu;

            public List<Babu> UtesCellara;

            public Cella(Tabla szulo, int x, int y)
            {
                Szulo = szulo;
                UtesCellara = new List<Babu>();
                X = x;
                Y = y;
            }

            public IEnumerable<Cella> OpenLineOfSight(int irX, int irY, int Lehetosegek = 1)
            {
                for (int i = 1; i <= Lehetosegek; i++)
                {
                    Cella cella = Szulo.CelllaBe(X + irX * i, Y + irY * i);
                    if (cella == null) yield break;

                    yield return cella;

                    if (cella.Babu != null)
                        yield break;
                }
            }
            public Cella Megnyit(int x, int y)
            {
                Cella cella = Szulo.CellaBe(X + x, Y + y);
                return cella ?? null;
            }
        }
        private Cella[,] cellak;

        public Cella KeresztLepes
        {
            private set;
            get;
        }

        public Cella KeresztUtes

        {
            private set;
            get;
        }

        private List<Babu> babuk = new List<Babu>();

        private Babu feketeKiraly;
        private Babu feherKiraly;

        private bool sakkban;

        public Tabla()
        {
            Reset();
        }

        #region Getters

        public Cella Cellabe(int x, int y)
        {
            if (x < 0 || cellak.GetLength(0) <= x || y < 0 || cellak.GetLength(1) <= y) return null;

            return cellak[x, y];
        }

        #endregion

        #region HelperMethods

        private void babuAdas(Cella cella, Babu babu)
        {
            cella.Babu = babu;
            babuk.Add(babu);
            babu.Vegenel(cella);
        }

        #endregion

        #region InterfaceMethods

        public void Reset()
        {
            cellak = new Cella[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    cellak[i, j] = new Cella(this, i, j);
                }
            }

            babuk.Clear();

            KeresztLepes = null;
            KeresztUtes = null;

            babuAdas(cellak[0, 0], new Bastya(JatekosSzin.Feher));
            babuAdas(cellak[1, 0], new Lo(JatekosSzin.Feher));
            babuAdas(cellak[2, 0], new Futo(JatekosSzin.Feher));
            babuAdas(cellak[3, 0], new Kiralyno(JatekosSzin.Feher));
            babuAdas(cellak[4, 0], (feherkiraly = new Kiraly(JatekosSzin.Feher)));
            babuAdas(cellak[5, 0], new Futo(JatekosSzin.Feher));
            babuAdas(cellak[6, 0], new Lo(JatekosSzin.Feher));
            babuAdas(cellak[7, 0], new Bastya(JatekosSzin.Feher));

            babuAdas(cellak[0, 1], new Paraszt(JatekosSzin.Feher));
            babuAdas(cellak[1, 1], new Paraszt(JatekosSzin.Feher));
            babuAdas(cellak[2, 1], new Paraszt(JatekosSzin.Feher));
            babuAdas(cellak[3, 1], new Paraszt(JatekosSzin.Feher));
            babuAdas(cellak[4, 1], new Paraszt(JatekosSzin.Feher));
            babuAdas(cellak[5, 1], new Paraszt(JatekosSzin.Feher));
            babuAdas(cellak[6, 1], new Paraszt(JatekosSzin.Feher));
            babuAdas(cellak[7, 1], new Paraszt(JatekosSzin.Feher));

            babuAdas(cellak[0, 6], new Paraszt(JatekosSzin.Fekete));
            babuAdas(cellak[1, 6], new Paraszt(JatekosSzin.Fekete));
            babuAdas(cellak[2, 6], new Paraszt(JatekosSzin.Fekete));
            babuAdas(cellak[3, 6], new Paraszt(JatekosSzin.Fekete));
            babuAdas(cellak[4, 6], new Paraszt(JatekosSzin.Fekete));
            babuAdas(cellak[5, 6], new Paraszt(JatekosSzin.Fekete));
            babuAdas(cellak[6, 6], new Paraszt(JatekosSzin.Fekete));
            babuAdas(cellak[7, 6], new Paraszt(JatekosSzin.Fekete));

            babuAdas(cellak[0, 7], new Bastya(JatekosSzin.Fekete));
            babuAdas(cellak[1, 7], new Lo(JatekosSzin.Fekete));
            babuAdas(cellak[2, 7], new Futo(JatekosSzin.Fekete));
            babuAdas(cellak[3, 7], new Kiralyno(JatekosSzin.Fekete));
            babuAdas(cellak[4, 7], (feketeKiraly = new Kiraly(JatekosSzin.Fekete)));
            babuAdas(cellak[5, 7], new Futo(JatekosSzin.Fekete));
            babuAdas(cellak[6, 7], new Lo(JatekosSzin.Fekete));
            babuAdas(cellak[7, 7], new Bastya(JatekosSzin.Fekete));

            foreach (Babu babu in babuk)
            {
                babu.Ujraszamol();
            }
        }

        public bool Ujkor(JatekosSzin aktualisJatekos)
        {
            sakkban = Sakkbane(aktualisJatekos, false);
            bool barmiLepes = false;

            foreach (Cella cella in cellak)
            {
                cella.UtesCellara.Clear();
            }

            foreach (Babu babu in babuk)
            {
                babu.Ujraszamol();
            }

            foreach (Babu babu in babuk)
            {
                babu.Lepesek.Clear();
                foreach (Cella lepj in babu.Lehetosegek)
                {
                    if (babu.Szin == aktualisJatekos && lephetoE(babu, lepj))
                    {
                        babu.Lepesek.Add(lepj);
                        barmiLepes = true;
                    }
                }
            }

            return barmiLepes;
        }


        private bool lephetoE(Babu babu, Cella lepj)
        {
            Babu aktualisKiraly = babu.Szin == JatekosSzin.Feher ? feherKiraly : feketeKiraly;

            if (babu is Kiraly)
            {
                foreach (Babu tamado in lepj.UtesCellara)
                {
                    if (tamado.Szulo != lepj && tamado.Szin != babu.Szin)
                        return false;
                }

                if (Math.Abs(lepj.X - babu.Szulo.X) == 2)
                {
                    if (sakkban)
                        return false;

                    foreach (Babu tamado in Cellabe(lepj.X > babu.Szulo.X ? lepj.X - 1 : lepj.X + 1, lepj.Y).UtesCellara)
                    {
                        if (tamado.Szin != babu.Szin)
                            return false;
                    }
                }
            }
            else
            {
                if (sakkban)
                {
                    foreach (Babu tamado in aktualisKiraly.Szulo.UtesCellara)
                    {
                        if (tamado.Szin == aktualisKiraly.Szin) continue;
                        if (tamado.Szulo == lepj) continue;
                        if (tamado.Blokkolte(babu.Szulo, lepj, aktualisKiraly.Szulo)) continue;

                        return false;
                    }
                }

                foreach (Babu tamado in babu.Szulo.UtesCellara)
                {
                    if (tamado.Szin == aktualisKiraly.Szin) continue;
                    if (tamado.Szulo == lepj) continue;

                    if (!tamado.Blokkolte(babu.Szulo, lepj, aktualisKiraly.Szulo))
                        return false;
                }
            }


            return true;
        }

        public bool Sakkbane(JatekosSzin jatekos, bool useCache = true)
        {
            if (useCache)
                return sakkban;

            if (jatekos == JatekosSzin.Feher)
                return feherKiraly.Szulo.UtesCellara.Any(tamado => tamado.Szin != jatekos);
            else
                return feketeKiraly.Szulo.UtesCellara.Any(tamado => tamado.Szin != jatekos);
        }
        public void Lepj(Cella honnan, Cella hova, Opciok Opcio)
            {
            if (hova.Babu != null)
                babuk.Remove(hova.Babu);

            hova.Babu = honnan.Babu;
            honnan.Babu = null;

            if (hova == KeresztLepes && hova.Babu is Paraszt)
            {
                babuk.Remove(KeresztUtes.Babu);
                KeresztUtes.Babu = null;
            }

            if (hova.Babu is Kiraly && hova.X - honnan.X == 2)
            {
                Lepj(Cellabe(7, hova.Y), Cellabe(hova.X - 1, hova.Y), Opcio);
            }

            if (hova.Babu is Kiraly && hova.X - honnan.X == -2)
            {
                Lepj(Cellabe(0, hova.Y), Cellabe(hova.X + 1, hova.Y), Opcio);
            }

            if (hova.Babu is Paraszt && hova.Y == (hova.Babu.Szin == JatekosSzin.Feher ? 7 : 0))
            {
                Babu lepett = null;
                switch (Opcio)
                {
                    case Opciok.Kiralyno:
                        lepett = new Kiralyno(hova.Babu);
                        break;
                    case Opciok.Bastya:
                        lepett = new Bastya(hova.Babu);
                        break;
                    case Opciok.Futo:
                        lepett = new Futo(hova.Babu);
                        break;
                    case Opciok.Lo:
                        lepett = new Lo(hova.Babu);
                        break;
                }

                babuk.Remove(hova.Babu);
                hova.Babu = lepett;
                lepett.Vegenel(hova);
                babuk.Add(lepett);
            }

            hova.Babu.Mozgas(hova);
            hova.Babu.Ujraszamol();

            KeresztLepes = null;
            KeresztUtes = null;

            if (hova.Babu is Paraszt && Math.Abs(hova.Y - honnan.Y) == 2)
            {
                KeresztLepes = Cellabe(hova.X, (honnan.Y > hova.Y) ? honnan.Y - 1 : hova.Y + 1);
                KeresztUtes = hova;
            }
        }

        public bool Elmozdithato(Cella honnan, Cella hova)
        {
            return honnan.Babu is Paraszt && hova.Y == (honnan.Babu.Szin == JatekosSzin.Feher ? 7 : 0);
        }

        #endregion
    }
}
