using System.Collections.Concurrent;
using System.Collections.Generic;

public class Program
{
    private static int threadSayisi;
    private static ConcurrentQueue<string> kuyrukList;
    private static void Main(string[] args)
    {
        if (args.Count() > 1)     // 2 parametre var mı kontrol eder. Ornek deneme.txt 5
        {
            kuyrukList = new ConcurrentQueue<string>();

            Thread th = new Thread(() => DosyaOku(args[0]));
            th.Start();
            Thread.Sleep(3000); // Dosya okuma yapılırken main thread 3 sn bekletildi.
            threadSayisi = Convert.ToInt32(args[1]); // Thread Sayisi args parametresinden alınır.

            // Thread Sayisi  0 dan büyükse threadler oluşturulur.
            if (threadSayisi > 0)
            {
                for (int i = 0; i < threadSayisi; i++)
                {
                    Thread thread = new Thread(() => EkranaYaz());
                    thread.Name = (i + 1).ToString();
                    thread.Start();
                }
            }
        }
        else
        {
            Console.WriteLine("Parametre eksik. 'Dosyaadi.txt ThreadSayisi' şeklinde parametreleri eksiksiz giriniz.");
        }
    }

    private static void EkranaYaz()
    {
        string kelime;
        while (kuyrukList.Count > 0)  // List 0 olana kadar Threadler ekrana yazdıracak.
        {
            kuyrukList.TryDequeue(out kelime);
            Thread.Sleep(3000); // Ekrana yazmaya geçmeden 3sn bekletildi. 

            // Kelime:Kelime.Lenght şeklinde çıktı yerine aşağıdaki formatı tercih edilmiştir.
            Console.WriteLine("Thread: {0,-2} Kelime: {1,-14} Uzunluk: {2,-4}", Thread.CurrentThread.Name, kelime, kelime.Length);
        }
    }
    private static void DosyaOku(string DosyaAdi)
    {
        string dosyaDizini = AppDomain.CurrentDomain.BaseDirectory.ToString() + DosyaAdi;
     
        if (File.Exists(dosyaDizini))   // dizindeki dosya var mı ?
        {
            // Dosyadan okunan tüm veriler satirlar dizisine aktarıldı.
            string[] satirlar = File.ReadAllLines(dosyaDizini);

            // satirlar dizisi dolaşılarak kuyruğa tek tek eklendi.
            foreach (var satir in satirlar)
            {
                kuyrukList.Enqueue(satir);
            }
        }
        else
        {
            Console.WriteLine("Dosya okunamadi!");
        }
    }
}


