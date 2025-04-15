// See https://aka.ms/new-console-template for more information


using System;

var clientesPath = "C:\\Users\\knabb\\Documents\\ENG SOFTWARE\\Disciplinas\\Laboratorio Software\\arquivos\\1428624292050_clientes.txt";
var pagamentosPath = "C:\\Users\\knabb\\Documents\\ENG SOFTWARE\\Disciplinas\\Laboratorio Software\\arquivos\\1428624292736_pagamentos.txt";
var resultPath = "C:\\Users\\knabb\\Documents\\ENG SOFTWARE\\Disciplinas\\Laboratorio Software\\arquivos\\result.txt";

var clientesPathMsg = "Informar caminho da pasta de CLIENTES:\n";
var pagamentosPathMsg = "Informar caminho da pasta de PAGAMENTOS:\n";
var resultPathMsg = "Informar caminho da pasta para guardar resultados:\n";

var msgInicial = "Bem-vindo ao sistema de recuperação pagamentos!";

main();

void main()
{
    Console.WriteLine(msgInicial);

    //string antes = caminho[..index]; // tudo antes do último "\"
    //string depois = caminho[(index + 1)..]; // tudo depois do último "\"

    //if (!Directory.Exists(clientesPath) || !Directory.Exists(pagamentosPath))
    //{
    //    Console.WriteLine("Caminho não encontrado.");
    //    return;
    //}

    List<Cliente> clientes = new();

    foreach (var linha in File.ReadLines(clientesPath))
    {
        var campos = linha.Split(';');

        clientes.Add(new Cliente
        {
            Id = int.Parse(campos[0]),
            Name = campos[4]
        });
    }

    List<Pagamento> pagamentos = new();

    foreach (var linha in File.ReadLines(pagamentosPath))
    {
        var campos = linha.Split(';');

        pagamentos.Add(new Pagamento
        {
            Value = decimal.Parse(campos[3]),
            Pago = campos[4] == "f" ? false : true,
            Cliente = clientes.FirstOrDefault(x => x.Id == int.Parse(campos[0]))
        });
    }

    List<Conta> contas = new();

    foreach(var cliente in clientes)
    {
        contas.Add(new Conta
        {
            Id = cliente.Id,
            Saldo = pagamentos.Where(x => x.Cliente != null && x.Cliente.Equals(cliente) && x.Pago == true).ToList().Sum(a => a.Value),
            Cliente = cliente
        });
    }

    result();

    Console.WriteLine("Arquivo exportado com sucesso.");

    void result()
    {
        using (StreamWriter writer = new(resultPath))
        {
            writer.WriteLine("DEBITO DE CLIENTES ENCONTRADOS\n");

            foreach (var conta in contas)
            {
                if(conta.Cliente is null) continue;
                string linha = $"ID: {conta.Cliente.Id}; {conta.Cliente.Name}; Valor Devido: {conta.Saldo};";
                writer.WriteLine(linha);
            }

            writer.WriteLine("\nDEBITO DE CLIENTES NÃO ENCONTRADOS\n");

            foreach(var pagamento in pagamentos)
            {
                if (pagamento.Cliente != null) continue;
                string linha = $"Valor: {pagamento.Value}; Pago: {(pagamento.Pago ? "Sim" : "Não")}";
                writer.WriteLine(linha);
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
    public Cliente Cliente { get; set; }

}

class Conta
{
    public int Id { get; set; }
    public decimal Saldo { get; set; } = 0M;
    public Cliente Cliente { get; set; }

}