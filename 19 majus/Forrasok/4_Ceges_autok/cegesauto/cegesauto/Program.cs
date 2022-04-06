using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace cegesauto
{
    class C {

        
        public int nap, ora, perc, szemelyI, km,kibe,ment;
        public string rendszam,  oraperc;
        public bool beki;
        public C(string sor)
        {
            var s = sor.Split(' ');

            var ido = s[1].Split(':');
            ora = int.Parse(ido[0]);
            perc = int.Parse(ido[1]);

            nap = int.Parse(s[0]);    
            oraperc = s[1];                   
            rendszam = s[2];
            szemelyI = int.Parse(s[3]);
            km = int.Parse(s[4]);
            kibe =int.Parse( s[5]);
            beki = kibe == 1 ? true :false;
            
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var lista = new List<C>();
            var sr = new StreamReader("autok.txt");
            while (!sr.EndOfStream)
            {
                lista.Add(new C(sr.ReadLine()));
            }
            sr.Close();
            var last = (from sor in lista where sor.kibe==0 select sor).Last();
            Console.WriteLine("2. feladat");
            Console.WriteLine($"{last.nap}. nap renszám: {last.rendszam}");
            Console.WriteLine("3. feladat");
            Console.Write("Nap: ");
            int nap = int.Parse(Console.ReadLine());
            var aznap = (from sor in lista where sor.nap == nap select sor);
            foreach (var item in aznap)
            {
                Console.WriteLine($"{item.oraperc} {item.rendszam} {item.szemelyI} {item.beki}");
            }
            /*var x = (from sor in lista select sor).Reverse().Take(10);
            foreach (var item in x)
            {
                if (item.beki=="ki")
                {
                    Console.WriteLine(item.rendszam);
                }
                
            }*/
            var y = (from sor in lista group sor.kibe by sor.rendszam);
            int q = 0;
            foreach (var item in y)
            {
                if (item.Count()%2==1)
                {
                    q++;
                }
            }
            Console.WriteLine($"4. feladat {q}");

            int[] aautokki = new int[10];
            int[] aautokbe = new int[10];
            int[] szumkmauto = new int[10];
            for (int i = 0; i < 10; i++) aautokki[i] = 0;
            foreach (var item in lista)
            {
                int auto = Convert.ToInt32(item.rendszam.Substring(3, 3)) % 10;
                if (!item.beki && aautokki[auto] == 0)
                    aautokki[auto] = item.km;
                if (item.beki)
                    aautokbe[auto] = item.km;
            }
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"CEG3{i.ToString("00")}  {szumkmauto[i] = aautokbe[i] - aautokki[i]}");
            }
            var test = lista.OrderBy(o => o.rendszam).ToList();

            int maxi = 0;
            int nev = 0;
            for (int i = 1; i < test.Count(); i++)
            {
                if (test[i].rendszam==test[i-1].rendszam && test[i].beki)
                {
                    if (maxi<test[i].km-test[i-1].km && test[i].beki)
                    {
                        maxi = test[i].km - test[i - 1].km;
                        nev = test[i].szemelyI;
                    }
                }
                
            }
            Console.WriteLine($"{nev}   {maxi}");

            Console.Write("adjon be egy rendszámot ");
            string rsz = Console.ReadLine();
            var keres = (from sor in lista where sor.rendszam == rsz select sor);
            var sw = new StreamWriter(rsz + "_menetlevel.txt");
            
            foreach (var item in keres)
            {
                if (!item.beki)
                {
                    sw.Write($"{item.szemelyI}{(char)9}{item.nap}. {item.oraperc}{(char)9}{item.km} km{(char)9}  ");
                }
                else
                {
                    sw.Write($"{item.nap}. {item.oraperc}{(char)9}{item.km} km\n");
                }                    
            }
            sw.Close();
            Console.ReadKey();
            

        }
    }
}
