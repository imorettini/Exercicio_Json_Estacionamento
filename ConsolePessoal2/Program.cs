#region
using Newtonsoft.Json;

var clientes = new List<dynamic>();
var movimentacoesEntrada = new List<dynamic>();
var movimentacoesSaida = new List<dynamic>();
var precoMinuto = 1;
while (true)
{
    Console.Clear();
    Console.WriteLine("Digite uma das opções abaixo:");
    Console.WriteLine("1 - Cadastrar Cliente");
    Console.WriteLine("2 - Listar Cliente");
    Console.WriteLine("3 - Movimentação - Entrada");
    Console.WriteLine("4 - Movimentação - Saída");
    Console.WriteLine("5 - Sair");

    var opcao = Console.ReadLine();
    var sair = false;

    switch (opcao)
    {
        case "1":
            Console.Clear();
            Console.WriteLine("Digite o nome do cliente");
            var nome = Console.ReadLine();
            Console.WriteLine("Digite o telefone do cliente");
            var telefone = Console.ReadLine();

            dynamic cliente = new
            {
                Id = Guid.NewGuid(),
                Nome = nome,
                Telefone = telefone
            };

            clientes.Add(cliente);

            Console.WriteLine("Cliente cadastrado com sucesso ...");
            Thread.Sleep(1000);
            break;
        case "2":
            if (clientes.Count > 0)
            {
                Console.WriteLine("=== Lista de clientes =====");
                foreach (var cli in clientes)
                {
                    Console.WriteLine($"Nome: {cli.Nome}");
                    Console.WriteLine($"Telefone: {cli.Telefone}");
                    Console.WriteLine(" --------- ");
                }
                Console.WriteLine(" --------------------------------- ");
                Console.WriteLine("Pressione Enter para continuar ...");
                Console.ReadKey();
                break;
            }
            else
            {
                Console.WriteLine("Não possui nenhum cliente cadastrado");
                Console.WriteLine(" --------------------------------- ");
                Console.WriteLine("Pressione Enter para continuar ...");
                Console.ReadKey();
                break;
            }

        case "3":
            Console.WriteLine("=== Movimentação Entrada =====");
            Console.WriteLine("Escolha o número correspondente ao cliente: ");
            foreach (var cli in clientes)
            {
                int indice = clientes.IndexOf(cli);
                Console.WriteLine(indice + " - " + cli.Nome);
            }
            var indiceClienteEscolhido = Console.ReadLine();
            var idClienteEscolhido = clientes[Convert.ToInt32(indiceClienteEscolhido)].Id;
            var horaEntrada = DateTime.Now;
            Console.WriteLine("Digite a placa do carro");
            var placa = Console.ReadLine();
            dynamic movimentacaoEntrada = new
            {
                Id = Guid.NewGuid(),
                Placa = placa,
                IdCliente = idClienteEscolhido,
                HoraEntrada = horaEntrada,
            };
            movimentacoesEntrada.Add(movimentacaoEntrada);
            Console.WriteLine("Movimentação de Entrada Cadastrada com sucesso: ");
            Console.WriteLine(" --------------------------------- ");
            Console.WriteLine("IdMovimentação: " + movimentacaoEntrada.Id);
            Console.WriteLine("Cliente: " + movimentacaoEntrada.IdCliente);
            Console.WriteLine("Placa do Carro: " + movimentacaoEntrada.Placa);
            Console.WriteLine("Hora de Entrada: " + movimentacaoEntrada.HoraEntrada);
            Console.WriteLine(" --------------------------------- ");
            Console.WriteLine("Pressione Enter para continuar ...");
            Console.ReadKey();
            break;
        case "4":
            Console.WriteLine("=== Movimentação Saída =====");
            Console.WriteLine("=== Olá! Escolha o número correspondente à movimentação que deseja encerrar =====");
            foreach (var mov in movimentacoesEntrada)
            {
                int indice = movimentacoesEntrada.IndexOf(mov);
                Console.WriteLine(indice + " - " + mov.Id + " " + mov.Placa);
            }
            var indiceMovimentacaoEscolhido = Console.ReadLine();
            var idMovimentacaoEntrada = movimentacoesEntrada[Convert.ToInt32(indiceMovimentacaoEscolhido)].Id;
            var jaFaturado = movimentacoesSaida.Where(p => p.IdMovimentacaoEntrada == idMovimentacaoEntrada).FirstOrDefault();
            if (jaFaturado == null)
            {

                DateTime horaSaida;
                bool horaSaidaValida = false;
                Console.WriteLine("Digite a hora de saída (dd/MM/yyyy HH:mm): ");
                horaSaidaValida = DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out horaSaida);
                var horaTotalEstacionado = (horaSaida - movimentacoesEntrada[Convert.ToInt32(indiceMovimentacaoEscolhido)].HoraEntrada).TotalMinutes;
                Console.WriteLine(horaTotalEstacionado);
                var precoTotal = horaTotalEstacionado * precoMinuto;
                Console.WriteLine($"Preço total: R${precoTotal:F2}");
                dynamic movimentacaoSaida = new
                {
                    Id = Guid.NewGuid(),
                    IdMovimentacaoEntrada = idMovimentacaoEntrada,
                    PrecoTotal = precoTotal
                };
                movimentacoesSaida.Add(movimentacaoSaida);
            }
            else
            {
                Console.WriteLine("Movimentação já foi encerrada");
            }

            Console.WriteLine(" --------------------------------- ");
            Console.WriteLine("Pressione Enter para continuar ...");
            Console.ReadKey();
            break;
        default:
            sair = true;
            break;
    }

    // Serializa as variáveis em JSON
    var clientesJson = JsonConvert.SerializeObject(clientes, Formatting.Indented);
    var movimentacoesEntradaJson = JsonConvert.SerializeObject(movimentacoesEntrada, Formatting.Indented);
    var movimentacoesSaidaJson = JsonConvert.SerializeObject(movimentacoesSaida, Formatting.Indented);

    // Salva o JSON em um arquivo
    File.WriteAllText("clientes.json", clientesJson);
    File.WriteAllText("movimentacoesEntrada.json", movimentacoesEntradaJson);
    File.WriteAllText("movimentacoesSaida.json", movimentacoesSaidaJson);


    if (sair) break;
}
#endregion