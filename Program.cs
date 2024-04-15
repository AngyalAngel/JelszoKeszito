using System;
using System.Linq;
using System.Text;

namespace JelszoGenerator
{
    class Program
    {
        
       
        static Random rnd = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine("A program egy általad megadott szöveget alakít át jelszóvá úgy, hogy amennyire lehet, megtartja a szöveg eredeti értelmét.");
            Console.WriteLine("Kérem, add meg a jelszótól elvárt formai tulajdonságokat! ");
            
           
            Console.Write("\t A minimális hossza: ");
            int minimalisHossz = int.Parse(Console.ReadLine());

            Console.Write("\tA kisbetűk minimális száma: ");
            int kisbetukMinSzama = int.Parse(Console.ReadLine());              //1-4 feladat - Veronika 
                                                                               //5-7 feladat - Tamás
            Console.Write("\tA nagybetűk minimális száma: ");                  //8-10 feladat - Bence
            int nagybetukMinSzama = int.Parse(Console.ReadLine());

            Console.Write("\tA számjegyek minimális száma: ");
            int szamjegyekMinSzama = int.Parse(Console.ReadLine());

            Console.Write("\tA speciális karakterek száma: ");
            int specialisKarakterekSzama = int.Parse(Console.ReadLine());


            Console.WriteLine("Kérlek, írd be az átalakítandó szöveget:");
            string eredetiSzoveg = Console.ReadLine();
            List<char> szovegLista = new List<char>(eredetiSzoveg.ToCharArray());
            
            bool modositva = false; 
            
            if (modositva) {
                Console.WriteLine("A megfelelő hosszúságú jelszó alapja: " + string.Join("", szovegLista));
            } else {
                Console.WriteLine("A megfelelő hosszúságú jelszó alapja: " + eredetiSzoveg);
            }

            // szoveg hossz, ha kell hozzaadas
            Random rnd = new Random();
            while (szovegLista.Count < minimalisHossz) {
                char veletlenKarakter = (char)rnd.Next(33, 127);
                szovegLista.Add(veletlenKarakter);
                modositva = true; 
            }
            string specialisKarakterek = ".,;:-_?!+%()[]{}<>#&@*";
            string szamjegyHelyettesitok = "OIZEASGTBP";
        

            List<char> maszk = new List<char>(szovegLista.Count);
            int kisbetuSzamlalo = 0;
            int nagybetuSzamlalo = 0;
            int szamjegySzamlalo = 0;
            int specialisKarakterSzamlalo = 0;

            foreach (var karakter in szovegLista) {
                if (char.IsLower(karakter)) {
                    maszk.Add('k');
                    kisbetuSzamlalo++;
                } else if (char.IsUpper(karakter)) {
                    maszk.Add('n');
                    nagybetuSzamlalo++;
                } else if (char.IsDigit(karakter)) {
                    maszk.Add('o');
                    szamjegySzamlalo++;
                } else if (specialisKarakterek.Contains(karakter)) {
                    maszk.Add('s');
                    specialisKarakterSzamlalo++;
                }
            }
            
            // szam betu csere
            for (int i = 0; i < szovegLista.Count; i++) {
                int helyettesitoIndex = szamjegyHelyettesitok.IndexOf(char.ToUpper(szovegLista[i]));
                if (helyettesitoIndex != -1 && szamjegySzamlalo < szamjegyekMinSzama) {
                    szovegLista[i] = helyettesitoIndex.ToString()[0];
                    maszk[i] = 'O'; 
                    szamjegySzamlalo++;
                }
            }
            
            // szamok hozzaadas ha kell
            while (szamjegySzamlalo < szamjegyekMinSzama) {
                szovegLista.Add(rnd.Next(0, 10).ToString()[0]);
                maszk.Add('O');  
                szamjegySzamlalo++;
            }
            
            Console.WriteLine($"A számjegyeket tartalmazó jelszó alapja: {string.Join("", szovegLista)}");
            
            // spec karakterek
            for (int i = 0; i < szovegLista.Count; i++) {
                if (!char.IsLetterOrDigit(szovegLista[i]) && !specialisKarakterek.Contains(szovegLista[i])) {
                    szovegLista[i] = specialisKarakterek[rnd.Next(specialisKarakterek.Length)];
                    maszk[i] = 'S';  
                }
            }

            // spec hozzaadas
            while (specialisKarakterSzamlalo < specialisKarakterekSzama) {
                char specKarakter = specialisKarakterek[rnd.Next(specialisKarakterek.Length)];
                szovegLista.Add(specKarakter);
                maszk.Add('S');  
                specialisKarakterSzamlalo++;
            }


            // kis es nagybetuk ell, hozzaad.
            while (nagybetuSzamlalo < nagybetukMinSzama) {
                char nagybetu = (char)rnd.Next('A', 'Z' + 1);
                szovegLista.Add(nagybetu);
                maszk.Add('N');
                nagybetuSzamlalo++;
            }
            
            while (kisbetuSzamlalo < kisbetukMinSzama) {
                char kisbetu = (char)rnd.Next('a', 'z' + 1);
                szovegLista.Add(kisbetu);
                maszk.Add('K');
                kisbetuSzamlalo++;
            }
            
            for (int i = 0; i < szovegLista.Count && nagybetuSzamlalo < nagybetukMinSzama; i++) {
                if (maszk[i] == 'k') {
                    szovegLista[i] = char.ToUpper(szovegLista[i]);
                    maszk[i] = 'N';
                    nagybetuSzamlalo++;
                    kisbetuSzamlalo--;
                }
            }

            // nagybetu->kisbetu ha kell
            for (int i = szovegLista.Count - 1; i >= 0 && kisbetuSzamlalo < kisbetukMinSzama; i--) {
                if (maszk[i] == 'n') {
                    szovegLista[i] = char.ToLower(szovegLista[i]);
                    maszk[i] = 'K';
                    kisbetuSzamlalo++;
                    nagybetuSzamlalo--;
                }
            }
            
            // spec szoveg kiiras
            Console.WriteLine($"A specialis karaktereket is tartalmazo jelszo alap: {string.Join("", szovegLista)}");
            
            // kesz jelszo
            Console.WriteLine();
            Console.WriteLine($"A minden szempontnak megfelelő jelszó: {string.Join("", szovegLista)}");

        }
    }
}
