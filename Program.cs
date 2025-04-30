// See https://aka.ms/new-console-template for more information


using DinamicaXP.Models;
using DinamicaXP.Repository;
using System.Globalization;

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
        Console.WriteLine("5 - Sincronizar com Banco de Dados");
        Console.WriteLine("6 - Fechar o programa");
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
                Console.WriteLine("Sincronizando com o Banco De Dados...");
                sincronizarBD();
                break;
            case "6":
                Console.WriteLine("Encerrando o programa...");
                executando = false;
                break;

            default:
                Console.WriteLine("Opção inválida. Tente novamente.");
                break;
        }
    }
}

void sincronizarBD()
{
    if (!File.Exists(clientesPath) || !File.Exists(pagamentosPath))
    {
        Console.WriteLine("Arquivo(s) não encontrado(s).");
        return;
    }
    buildRelatorio();
}
void buildRelatorio()
{
    using AppDbContext _dbContext = new AppDbContext();

    clientes = _dbContext.Clientes.ToList();
    pagamentos = _dbContext.Pagamentos.ToList();
    contas = _dbContext.Contas.ToList();
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