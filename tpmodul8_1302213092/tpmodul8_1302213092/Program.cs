// See https://aka.ms/new-console-template for more information
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text.Json;

public class Program
{
    private static void Main(String[] args)
    {
        AppConfig config = new AppConfig();
        try
        {
            config.ReadJSON();
        }
        catch 
        {
            config.setDefault();
            config.writeConfig();
            config.ReadJSON();
        }

        Console.WriteLine($"Berapa suhu badan anda saat ini? Dalam nilai {config.covid.satuan_suhu}");
        double inputSuhu = Convert.ToDouble( Console.ReadLine() );
        Console.WriteLine($"Berapa hari yang lalu (perkiraan) anda terakhir memiliki gejala demam?");
        int inputLamaDemam = Convert.ToInt32( Console.ReadLine() );
        if ((inputSuhu >= 36.5 && inputSuhu < 37.5 && config.covid.satuan_suhu == "Celcius") || (inputSuhu >= 97.5 && inputSuhu < 99.5))
        {
            Console.WriteLine(config.covid.pesan_diterima);
            return;
        }
        else if(inputLamaDemam > config.covid.batas_hari_demam)
        {
            Console.WriteLine(config.covid.pesan_diterima);
            return;
        }
        else {
            Console.WriteLine(config.covid.pesan_ditolak);
        }
    }
}

public class AppConfig
{
    public covid covid;
    public AppConfig() { }

    public AppConfig(covid covid) { 
        this.covid = covid;
    }

    public void ReadJSON()
    {
        String txt = File.ReadAllText(@"./covid_config.json");
        covid = JsonSerializer.Deserialize<covid>(txt);
    }

    public void setDefault()
    {
        covid = new covid("Celcius", 14, "Anda tidak diperbolehkan masuk ruangan", "Anda dipersilahkan masuk ruangan");
    }

    public void writeConfig()
    {
        var option = new JsonSerializerOptions { WriteIndented = true };
        string jsonstr = JsonSerializer.Serialize(covid, option);
        File.WriteAllText(@"./covid_config.json", jsonstr);
    }

    public void ubahSatuanSuhu()
    {
        if(covid.satuan_suhu == "Celscius")
        {
            covid.satuan_suhu = "Farenheit";
            writeConfig();
            return;
        }
        covid.satuan_suhu = "Celcius";
        writeConfig();
    }
}

public class covid
{
    public string satuan_suhu { get; set; }
    public int batas_hari_demam { get; set; }
    public string pesan_ditolak { get; set; }
    public string pesan_diterima { get; set; }

    public covid() {}

    public covid(string satuan_suhu, int batas_hari_demam, string pesan_ditolak, string pesan_diterima)
    {
        this.satuan_suhu = satuan_suhu;
        this.batas_hari_demam = batas_hari_demam;
        this.pesan_ditolak = pesan_ditolak;
        this.pesan_diterima = pesan_diterima;
    }


}
