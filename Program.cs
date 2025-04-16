    // See https://aka.ms/new-console-template for more information


using System;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;

var clientesPath = "";
var pagamentosPath = "";
var resultPath = "";

var clientesPathMsg = "Informar caminho da pasta de CLIENTES:\n";
var pagamentosPathMsg = "Informar caminho da pasta de PAGAMENTOS:\n";
var resultPathMsg = "Informar caminho da pasta para guardar resultados:\n";

var msgInicial = "Bem-vindo ao sistema de recuperação pagamentos!";

List<Cliente> clientes = new();
List<Pagamento> pagamentos = new();
List<Conta> contas = new();

main();

void main()
{
    Console.WriteLine(msgInicial);

    bool executando = true;

    while (executando)
    {
        Console.WriteLine("\nEscolha uma opção:");
        Console.WriteLine("1 - Digitar caminho da TABELA CLIENTES");
        Console.WriteLine("2 - Digitar caminho da TABELA PAGAMENTOS");
        Console.WriteLine("3 - Digitar caminho do arquivo resposta");
        Console.WriteLine("4 - Exportar arquivo");
        Console.WriteLine("5 - Fechar o programa");
        Console.Write("Opção: ");
        string opcao = Console.ReadLine();

        switch (opcao)
        {
            case "1":
                Console.Write(clientesPathMsg);
                clientesPath = Console.ReadLine();
                LerArquivo(clientesPath);
                break;

            case "2":
                Console.Write(pagamentosPathMsg);
                pagamentosPath = Console.ReadLine();
                LerArquivo(pagamentosPath);
                break;

            case "3":
                Console.WriteLine(resultPathMsg);
                resultPath = Console.ReadLine() + "\\result.txt";
                break;

            case "4":
                Console.WriteLine("Aguarde... Exportando arquivo para caminho: " + resultPath);
                buildRelatorio();
                result();
                break;

            case "5":
                Console.WriteLine("Encerrando o programa...");
                executando = false;
                break;

            default:
                Console.WriteLine("Opção inválida. Tente novamente.");
                break;
        }
    }
}

void buildRelatorio()
{
    foreach (var linha in File.ReadLines(clientesPath))
    {
        var campos = linha.Split(';');

        clientes.Add(new Cliente
        {
            Id = int.Parse(campos[0]),
            Name = campos[4]
        });
    }

    foreach (var linha in File.ReadLines(pagamentosPath))
    {
        var campos = linha.Split(';');

        pagamentos.Add(new Pagamento
        {
            Value = decimal.Parse(campos[3]),
            Pago = campos[4] == "f" ? false : true,
            Data = DateTime.ParseExact((campos[1].Length < 8 ? $"0{campos[1]}" : campos[1]), "ddMMyyyy", CultureInfo.InvariantCulture),
            Cliente = clientes.FirstOrDefault(x => x.Id == int.Parse(campos[0]))
        });
    }

    foreach (var cliente in clientes)
    {
        contas.Add(new Conta
        {
            Id = cliente.Id,
            SaldoDevido = pagamentos.Where(x => x.Cliente != null && x.Cliente.Equals(cliente) && x.Pago == false).ToList().Sum(a => a.Value),
            ValorPago = pagamentos.Where(x => x.Cliente != null && x.Cliente.Equals(cliente) && x.Pago == true).ToList().Sum(a => a.Value),
            Cliente = cliente
        });
    }
}

void result()
{
    using (StreamWriter writer = new(resultPath))
    {
        writer.WriteLine("DEBITO DE CLIENTES ENCONTRADOS\n");

        foreach (var conta in contas)
        {
            if (conta.Cliente is null) continue;
            string linha = $"ID: {conta.Cliente.Id}; {conta.Cliente.Name}; Valor Devido: {conta.SaldoDevido};";
            writer.WriteLine(linha);
        }

        writer.WriteLine("\nDEBITO DE CLIENTES NÃO ENCONTRADOS\n");

        foreach (var pagamento in pagamentos)
        {
            if (pagamento.Cliente != null) continue;
            string linha = $"Valor: {pagamento.Value}; Pago: {(pagamento.Pago ? "Sim" : "Não")}";
            writer.WriteLine(linha);
        }

        writer.WriteLine("\nCONTABILIDADE GERAL ORDENADA\n");

        var agrupamento = pagamentos.OrderBy(x => x.Data).GroupBy(x => x.Data).ToList();
        foreach(var dia in agrupamento)
        {
            var valorParaReceber = dia.Sum(x => x.Value);
            var valorRecebido = dia.Where(x => x.Pago == true).ToList().Sum(a => a.Value);

            string linha = $"DIA: {dia.First().Data.Date}\nValor à Receber: R${valorParaReceber}\nValor recebido: R${valorRecebido}\n";
            writer.WriteLine(linha);
        }

        writer.WriteLine("PAGAMENTOS ENCONRADOS\n");
        foreach (var conta in contas)
        {
            if (conta.Cliente is null) continue;
            string linha = $"ID: {conta.Cliente.Id}; {conta.Cliente.Name}; Valor Pago: {conta.ValorPago}";
            writer.WriteLine(linha);
        }

    }

    Console.WriteLine("Arquivo exportado com sucesso.");

}

static void LerArquivo(string caminhoArquivo)
{
    if (!File.Exists(caminhoArquivo))
    {
        Console.WriteLine("Arquivo não encontrado.");
        return;
    }

    string[] linhas = File.ReadAllLines(caminhoArquivo);

    for (int numLinha = 0; numLinha < linhas.Length; numLinha++)
    {
        string linha = linhas[numLinha].Trim();
        string[] campos = linha.Split(';');

        Console.WriteLine($"\nLinha {numLinha}:");

        for (int i = 0; i < campos.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(campos[i]))
            {
                Console.WriteLine($"  Campo {i}: [vazio]");
            }
            else
            {
                Console.WriteLine($"  Campo {i}: {campos[i]}");
            }
        }
    }
}
class Cliente
{
    public int Id { get; set; }
    public string Name { get; set; }
}

class Pagamento
{
    public decimal Value { get; set; }
    public bool Pago { get; set; }
    public DateTime Data { get; set; }
    public Cliente Cliente { get; set; }

}

class Conta
{
    public int Id { get; set; }
    public decimal ValorPago { get; set; } = 0M;
    public decimal SaldoDevido { get; set; } = 0M;
    public Cliente Cliente { get; set; }
}