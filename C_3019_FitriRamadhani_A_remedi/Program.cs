using System;
using System.Collections.Generic;

class Sekolah
{
    public string idSekolah;
    public string namaSekolah;
    public int jumlahPaketDiterima;

    public Sekolah(string id, string nama)
    {
        idSekolah = id;
        namaSekolah = nama;
        jumlahPaketDiterima = 0;
    }

    public void TampilInfo()
    {
        Console.WriteLine($"ID Sekolah : {idSekolah}");
        Console.WriteLine($"Nama Sekolah : {namaSekolah}");
        Console.WriteLine($"Jumlah Paket Diterima : {jumlahPaketDiterima}");
    }
}
abstract class PaketMBG
{
    private string idPaket;
    private string namaPaket;
    private int stokDapur;
    public string IdPaket
    {
        get { return idPaket; }
        set { idPaket = value; }
    }
    public string NamaPaket
    {
        get { return namaPaket; }
        set { namaPaket = value; }
    }
    public int StokDapur
    {
        get { return stokDapur; }
        private set
        {
            if (value >= 0)
                stokDapur = value;
        }
    }
    public PaketMBG(string id, string nama, int stok)
    {
        idPaket = id;
        namaPaket = nama;
        stokDapur = Math.Max(0, stok);
    }
    public bool KurangiStok(int jumlah)
    {
        if (jumlah <= 0)
        {
            Console.WriteLine("Gagal: Jumlah pengurangan harus lebih dari 0.");
            return false;
        }

        if (jumlah > stokDapur)
        {
            Console.WriteLine("Gagal: Stok dapur tidak mencukupi.");
            return false;
        }

        stokDapur -= jumlah;
        return true;
    }
    public void TambahStok(int jumlah)
    {
        if (jumlah > 0)
            stokDapur += jumlah;
    }
    public abstract decimal HitungBiaya(int jumlah);
    public virtual void TampilInfo()
    {
        Console.WriteLine($"ID Paket : {idPaket}");
        Console.WriteLine($"Nama Paket : {namaPaket}");
        Console.WriteLine($"Stok Dapur : {stokDapur}");
    }
}
class PaketReguler : PaketMBG
{
    public PaketReguler(string id, string nama, int stok) : base(id, nama, stok)
    {
    }
    public override decimal HitungBiaya(int jumlah)
    {
        return jumlah * 15000;
    }

    public override void TampilInfo()
    {
        base.TampilInfo();
        Console.WriteLine("Jenis Paket : Reguler");
        Console.WriteLine("Isi Paket : Nasi, Ayam, Sayur, Buah");
        Console.WriteLine("Harga : Rp15.000");
    }
}
class PaketKhusus : PaketMBG
{
    public string JenisDiet { get; set; }
    public PaketKhusus(
        string id,
        string nama,
        int stok,
        string jenisDiet
    ) : base(id, nama, stok)
    {
        JenisDiet = jenisDiet;
    }
    public override decimal HitungBiaya(int jumlah)
    {
        return jumlah * 20000;
    }
    public override void TampilInfo()
    {
        base.TampilInfo();
        Console.WriteLine("Jenis Paket : Khusus");
        Console.WriteLine($"Jenis Diet : {JenisDiet}");
        Console.WriteLine("Harga : Rp20.000");
    }
}
interface DapatDidistribusikan
{
    void KirimPaket();
}
class DistribusiMBG : DapatDidistribusikan
{
    private Sekolah sekolahTujuan;
    private PaketMBG paket;
    private int jumlah;

    public DistribusiMBG(
        Sekolah sekolah,
        PaketMBG paket,
        int jumlah
    )
    {
        sekolahTujuan = sekolah;
        this.paket = paket;
        this.jumlah = jumlah;
    }

    public void KirimPaket()
    {
        Console.WriteLine("\n=== PROSES DISTRIBUSI ===");

        if (jumlah <= 0)
        {
            Console.WriteLine(
                "Distribusi gagal: Jumlah paket harus lebih dari 0."
            );
            return;
        }

        if (!paket.KurangiStok(jumlah))
        {
            Console.WriteLine("Distribusi MBG gagal!");
            return;
        }

        // Jika berhasil, jumlah paket sekolah bertambah
        sekolahTujuan.jumlahPaketDiterima += jumlah;

        Console.WriteLine(
            $"Distribusi MBG berhasil! Sebanyak {jumlah} paket makanan " +
            $"telah dikirim ke sekolah {sekolahTujuan.namaSekolah}."
        );
    }
}
class PengelolaMBG
{
    private List<Sekolah> daftarSekolah = new List<Sekolah>();
    private List<PaketMBG> daftarPaket = new List<PaketMBG>();

    public void TambahSekolah(Sekolah sekolah)
    {
        daftarSekolah.Add(sekolah);
    }
    public void TambahPaket(PaketMBG paket)
    {
        daftarPaket.Add(paket);
    }
    public void TampilkanDataMBG()
    {
        Console.WriteLine("=== DATA SEKOLAH MBG ===");

        foreach (Sekolah sekolah in daftarSekolah)
        {
            sekolah.TampilInfo();
            Console.WriteLine();
        }
        Console.WriteLine("=== DATA PAKET MBG ===");

        foreach (PaketMBG paket in daftarPaket)
        {
            paket.TampilInfo();
            Console.WriteLine();
        }
    }
}
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== SISTEM MANAJEMEN DISTRIBUSI MBG ===");

        Sekolah sekolah1 = new Sekolah("64668", "SMA Negeri 1 Lamongan");
        Sekolah sekolah2 = new Sekolah("64669", "SMA Negeri 2 Lamongan");
        Console.WriteLine("\n=== DATA SEKOLAH ===");
        sekolah1.TampilInfo();

        Console.WriteLine();
        sekolah2.TampilInfo();

        PaketReguler reguler = new PaketReguler(
            "P001", "Paket Reguler", 100
        );

        PaketKhusus khusus = new PaketKhusus(
            "P002", "Paket Khusus", 50, "Rendah Gula"
        );

        //4. Polymorphism
        Console.WriteLine("\n=== PERHITUNGAN BIAYA ===");

        PaketMBG paket1 = reguler;
        PaketMBG paket2 = khusus;

        Console.WriteLine(
            $"Biaya 10 Paket Reguler : Rp{paket1.HitungBiaya(10):N0}"
        );

        Console.WriteLine(
            $"Biaya 10 Paket Khusus  : Rp{paket2.HitungBiaya(10):N0}"
        );

        Console.WriteLine("\n=== DATA PAKET ===");
        reguler.TampilInfo();

        Console.WriteLine();
        khusus.TampilInfo();

        DistribusiMBG distribusi1 = new DistribusiMBG(
            sekolah1, reguler, 30
        );

        DistribusiMBG distribusi2 = new DistribusiMBG(
            sekolah2, khusus, 20
        );

        distribusi1.KirimPaket();
        distribusi2.KirimPaket();

        DistribusiMBG distribusi3 = new DistribusiMBG(
            sekolah1, reguler, 1000
        );

        distribusi3.KirimPaket();

        PengelolaMBG pengelola = new PengelolaMBG();

        pengelola.TambahSekolah(sekolah1);
        pengelola.TambahSekolah(sekolah2);

        pengelola.TambahPaket(reguler);
        pengelola.TambahPaket(khusus);

        pengelola.TampilkanDataMBG();
    }
}

