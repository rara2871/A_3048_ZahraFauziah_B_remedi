internal class Program
{
    private static void Main(string[] args)
    {
        Agen Agen1 = new Agen { idAgen = "agn1", namaAgen = "jaya", stokAgen = 40 };
        Agen Agen2 = new Agen { idAgen = "agn2", namaAgen = "joyo", stokAgen = 30 };

        Agen1.TampilInfo();
        Agen2.TampilInfo();
      

        ProdukGalon produk1 = new GalonAkua();
        produk1.IdProduk = "P001";
        produk1.NamaProduk = "Galon Aqua Resmi";
        produk1.StokGudang = 50;

        ProdukGalon produk2 = new GalonBerkeran();
        produk2.IdProduk = "P002";
        produk2.NamaProduk = "Galon Berkeran Praktis";
        produk2.StokGudang = 40;

        produk1.HitungBiaya(20);
        produk2.HitungBiaya(10);

        DistribusiGalon distribusi1 = new DistribusiGalon(Agen1, produk1, 30);
        distribusi1.KirimPaket();

        DistribusiGalon distribusi2 = new DistribusiGalon(Agen2, produk1, 50);
        distribusi2.KirimPaket();

        SindikatGalon sindikat = new SindikatGalon();
        sindikat.TambahAgen(Agen1);
        sindikat.TambahAgen(Agen2);
        sindikat.TambahProduk(produk1);
        sindikat.TambahProduk(produk2);

        sindikat.TampilkanSindikat();
    }
}

public class Agen
{
    public string idAgen;
    public string namaAgen;
    public int stokAgen;

    public void TampilInfo()
    {
        Console.WriteLine($" Agen {namaAgen} memiliki informasi, id : {idAgen}, Nama : {namaAgen}, stok : {stokAgen}");
    }


}

public abstract class ProdukGalon
{
    private string idProduk;
    private string namaProduk;
    private int stokGudang;

    public string IdProduk
    {
        get { return idProduk; }
        set { idProduk = value; }
    }

    public string NamaProduk
    {
        get { return namaProduk; }
        set { namaProduk = value; }
    }
    public int StokGudang
    {
        get { return stokGudang; }
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Stok gudang tidak boleh bernilai negatif!");
            }
            stokGudang = value;
        }
    }

    public void KurangiStok(int jumlah)
    {
        if (jumlah > stokGudang)
        {
            Console.WriteLine("Jumlah melebihi stok gudang yang tersedia.");
            return;
        }

        StokGudang -= jumlah;
        Console.WriteLine($"Stok gudang berhasil dikurangi sebanyak {jumlah}. Sisa stok: {stokGudang}");
    }

    public abstract void HitungBiaya(int jumlah);
}

public class GalonAkua : ProdukGalon
{
    public override void HitungBiaya(int jumlah)
    {
        int hargaPerGalon = 20000;
        int totalBiaya = jumlah * hargaPerGalon;
        Console.WriteLine($"Galon Akua - {jumlah} galon x Rp {hargaPerGalon:N0} = Rp {totalBiaya}");
    }

}

public class GalonBerkeran : ProdukGalon
{
    public override void HitungBiaya(int jumlah)
    {
        int hargaPerGalon = 12000;
        int totalBiaya = jumlah * hargaPerGalon;
        Console.WriteLine($"Galon Berkeran - {jumlah} galon x Rp {hargaPerGalon:N0} = Rp {totalBiaya}");
    }
}

public interface DapatDidistribusikan
{
    void KirimPaket();
}

public class DistribusiGalon : DapatDidistribusikan
{
    public Agen AgenTujuan { get; set; }
    public ProdukGalon ProdukKirim { get; set; }
    public int Jumlah { get; set; }

    public DistribusiGalon(Agen agenTujuan, ProdukGalon produkKirim, int jumlah)
    {
        AgenTujuan = agenTujuan;
        ProdukKirim = produkKirim;
        Jumlah = jumlah;
    }
    public void KirimPaket()
    {
        if (Jumlah <= 0)
        {
            Console.WriteLine($"Jumlah kiriman ({Jumlah}) harus lebih dari 0.");
            return;
        }

        if (Jumlah > ProdukKirim.StokGudang)
        {
            Console.WriteLine($"Stok gudang tidak mencukupi! (Stok: {ProdukKirim.StokGudang}, Diminta: {Jumlah}).");
            return;
        }

        ProdukKirim.KurangiStok(Jumlah);
        AgenTujuan.stokAgen += Jumlah;

        Console.WriteLine($"Paket berhasil dikirimkan ke agen {AgenTujuan.namaAgen}. Sisa stok gudang: {ProdukKirim.StokGudang}, Stok agen sekarang: {AgenTujuan.stokAgen}.");
    }
}

//opsional
// Hubungan: SindikatGalon dan Agen memiliki hubungan AGGREGATION.
// SindikatGalon "memiliki" banyak Agen (relasi one-to-many),
// namun objek Agen dibuat di luar SindikatGalon dan tetap dapat
// hidup secara independen tanpa SindikatGalon. Agen hanya
// "didaftarkan" ke sindikat melalui method TambahAgen(),
// sehingga ini bukan Composition (objek Agen tidak hancur
// jika SindikatGalon dihapus).

public class SindikatGalon
{
    public List<Agen> agens = new List<Agen>();
    public List<ProdukGalon> produkGalons = new List<ProdukGalon>();

    public void TambahAgen(Agen agen)
    {
        agens.Add(agen);
    }

    public void TambahProduk(ProdukGalon produk)
    {
        produkGalons.Add(produk);
    }

    public void TampilkanSindikat()
    {
        if (agens.Count == 0)
        {
            Console.WriteLine("Belum ada agen yang bergabung.");
        }
        else
        {
            foreach (var agen in agens)
            {
                Console.WriteLine($"- ID Agen: {agen.idAgen} | Nama: {agen.namaAgen} | Stok Agen: {agen.stokAgen}");
            }
        }

        if (produkGalons.Count == 0)
        {
            Console.WriteLine("Belum ada produk galon di gudang.");
        }
        else
        {
            foreach (var produk in produkGalons)
            {
                Console.WriteLine($"- ID Produk: {produk.IdProduk} | Nama: {produk.NamaProduk} | Stok Gudang: {produk.StokGudang}");
            }
        }
    }
}