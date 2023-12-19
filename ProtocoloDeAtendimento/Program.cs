using System.Windows.Input;

public class NumberGenerator
{
    private static int numeroDeProtocolo = 0;
    private string dataInicial = string.Empty;
    private string dataAtual = DateTime.Now.ToString("yyyyMMdd");

    public string GerarProtocolo()
    {
        string idEmpresa = GerarIdEmpresa();

        string sequencialDiario = GerarSequencialDiario();

        string numeroDeProtocoloGerado = $"{idEmpresa}{dataAtual}{sequencialDiario}";

        return numeroDeProtocoloGerado;
    }

    private string GerarIdEmpresa()
    {
        Random random = new Random();
        string stringDoArquivo = string.Empty;

        if (File.Exists("IdEmpresa.txt"))
        {
            using (StreamReader readtext = new StreamReader("IdEmpresa.txt"))
            {
                stringDoArquivo = readtext.ReadLine();
            }

            return stringDoArquivo != null ? stringDoArquivo : random.Next(100000, 999999).ToString();
        }
        else
        {
            string aleatorio = random.Next(100000, 999999).ToString();

            using (StreamWriter writetext = new StreamWriter("IdEmpresa.txt"))
            {
                writetext.WriteLine(aleatorio);
            }

            return aleatorio;
        }
    }

    private string GerarSequencialDiario()
    {
        if (File.Exists("DataInicial.txt"))
        {
            using (StreamReader readtext = new StreamReader("DataInicial.txt"))
            {
                dataInicial = readtext.ReadLine();
            }

            if (dataInicial != dataAtual)
            {
                using (StreamWriter writetext = new StreamWriter("DataInicial.txt"))
                {
                    writetext.WriteLine(dataAtual);
                }

                numeroDeProtocolo = 0;

                lock (this)
                {
                    numeroDeProtocolo = (numeroDeProtocolo + 1) % 1000000;
                }

                using (StreamWriter writetext = new StreamWriter("Protocolo.txt"))
                {
                    writetext.WriteLine(numeroDeProtocolo.ToString("D6"));
                }

                return numeroDeProtocolo.ToString("D6");

            }
            else
            {
                if (File.Exists("Protocolo.txt"))
                {
                    using (StreamReader readtext = new StreamReader("Protocolo.txt"))
                    {
                        numeroDeProtocolo = Int32.Parse(readtext.ReadLine());
                    }
                }

                lock (this)
                {
                    numeroDeProtocolo = (numeroDeProtocolo + 1) % 1000000;
                }

                using (StreamWriter writetext = new StreamWriter("Protocolo.txt"))
                {
                    writetext.WriteLine(numeroDeProtocolo.ToString("D6"));
                }

                return numeroDeProtocolo.ToString("D6");
            }

        }
        else
        {
            using (StreamWriter writetext = new StreamWriter("DataInicial.txt"))
            {
                writetext.WriteLine(dataAtual);
            }

            numeroDeProtocolo = 0;

            lock (this)
            {
                numeroDeProtocolo = (numeroDeProtocolo + 1) % 1000000;
            }

            using (StreamWriter writetext = new StreamWriter("Protocolo.txt"))
            {
                writetext.WriteLine(numeroDeProtocolo.ToString("D6"));
            }

            return numeroDeProtocolo.ToString("D6");
        }
    }

    
}

class Program
{
    static void Main()
    {
        NumberGenerator generator = new NumberGenerator();
        string generatedNumber = string.Empty;

        while (true)
        {
            Console.Clear();
            generatedNumber = generator.GerarProtocolo();
            Console.WriteLine(generatedNumber);
            Console.Write("Aperte enter para gerar número de protocolo.");

            ConsoleKeyInfo c = Console.ReadKey();
            
            if(c.Key == ConsoleKey.Enter)
            {
                Console.Clear();
                generatedNumber = generator.GerarProtocolo();
                Console.WriteLine(generatedNumber);
                Console.Write("Aperte enter para gerar número de protocolo.");
            }
            
            do
            {
                c = Console.ReadKey();
            }
            while (c.Key != ConsoleKey.Enter);

            Console.Clear();
            generatedNumber = generator.GerarProtocolo();
            Console.WriteLine(generatedNumber);
            Console.Write("Aperte enter para gerar número de protocolo.");
        }
    }
}
