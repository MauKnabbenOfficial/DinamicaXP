using System;
using System.IO;

class Program
{
    static void Main()
    {
        string clientesPath = "";
        string pagamentosPath = "";

        string msgInicial = "Bem-vindo ao sistema de recuperação de pagamentos!";
        Console.WriteLine(msgInicial);

        bool executando = true;

        while (executando)
        {
            Console.WriteLine("\nEscolha uma opção:");
            Console.WriteLine("1 - Digitar caminho da TABELA CLIENTES");
            Console.WriteLine("2 - Digitar caminho da TABELA PAGAMENTOS");
            Console.WriteLine("3 - Fechar o programa");
            Console.Write("Opção: ");
            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    Console.Write("Informe o caminho do arquivo de CLIENTES: ");
                    clientesPath = Console.ReadLine();
                    LerArquivo(clientesPath);
                    break;

                case "2":
                    Console.Write("Informe o caminho do arquivo de PAGAMENTOS: ");
                    pagamentosPath = Console.ReadLine();
                    LerArquivo(pagamentosPath);
                    break;

                case "3":
                    Console.WriteLine("Encerrando o programa...");
                    executando = false;
                    break;

                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }
        }
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
}
