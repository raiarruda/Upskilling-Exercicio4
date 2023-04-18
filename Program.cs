using Console_App_Exercicio;
using Console_App_Exercicio.Entities;

namespace Estacionamento
{
    class Program
    {
        static void Main(string[] args)
        {
            var clients = new List<Client>();
            var vehicles = new List<Vehicle>();
            var checkInOutsList = new List<CheckInOut>();
            var priceTable = new PriceTable();
            //Preço padrão
            priceTable.PriceMinute = Utils.loadPrice();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Digite uma das opções abaixo:");
                Console.WriteLine("1 - Cadastrar Cliente");
                Console.WriteLine("2 - Listar Clientes");
                Console.WriteLine("3 - Cadastrar Veículo");
                Console.WriteLine("4 - Registrar Entrada de Veículo");
                Console.WriteLine("5 - Registrar Saída de Veículo");
                Console.WriteLine("6 - Listar Movimentações");
                Console.WriteLine("7 - Atualizar Preço (R$ " + priceTable.PriceMinute.ToString() + ")");
                Console.WriteLine("8 - Sair");
                var opcao = Console.ReadLine();
                var sair = false;
                switch (opcao)
                {
                    case "1":
                        Console.Clear();
                        Client client = Utils.RegisterCostumer();
                        if(client.Name != null && client.Name != "")
                        {
                            clients.Add(client); 
                            Utils.WriteFile("clients.json", clients);
                        }                        
                        break;
                    case "2":
                        Console.Clear();
                        Utils.ListCostumer();
                        break;
                    case "3":
                        Console.Clear();
                        Vehicle vehicle = Utils.RegisterVehicle();
                        //TODO: verificar 
                        vehicles.Add(vehicle);
                        Utils.WriteFile("vehicles.json", vehicles);
                        
                        break;
                    case "4":
                        Console.Clear();
                        checkInOutsList.Add(Utils.CheckinVehicle());

                        Utils.WriteFile("checksInOut.json",checkInOutsList);
                        break;
                    case "5":
                        Console.Clear();
                        Utils.CheckoutVehicle(checkInOutsList, priceTable);
                        Console.WriteLine(checkInOutsList);
                        break;
                    case "6":
                        Console.Clear();
                        Utils.CheckInOutList();
                        break;
                    case "7":
                        Console.Clear();
                        priceTable.PriceMinute = Utils.UpdateHourPrice().PriceMinute;
                        break;
                    case "8":
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}