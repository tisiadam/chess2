using System.Collections.Generic;

namespace konzolos_sakk
{
    public abstract class Babu
    {
        public JatekosSzin Szin
        {
            private set;
            get;
        }
        public bool Lepett
        {
            protected set;
            get;
        }

        public abstract IEnumerable<Tabla.Cella> Lehetosegek
        {
            get;
        }
        
        public List<Tabla.Cella> Lepheto
        {
            private set;
            get;
        }
        public Tabla.Cella Szulo
        {
            private set;
            get;
        }

        public Babu(JatekosSzin szin)
        {
            Szin = szin;
            Lepett = false;
            Lepheto = new List<Tabla.Cella>();
        }

        public void Vegenel(Tabla.Cella cella)
        {
            Szulo = cella;
        }

        public void Mozgas(Tabla.Cella cella)
        {
            Szulo = cella;
            Lepett = true;
        }

        public abstract void Ujraszamol();

        public abstract bool Blokkolte(Tabla.Cella honnan, Tabla.Cella hova, Tabla.Cella blokk);

        public abstract char Char { get; }

        protected virtual bool Uthetoe(Tabla.Cella cella)
        {
            return cella != null && cella.Babu != null && cella.Babu.Szin != Szin;
        }
    }

}
