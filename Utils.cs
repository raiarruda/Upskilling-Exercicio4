using Console_App_Exercicio.Entities;
using System;
using System.ComponentModel;
using System.Text.Json;



namespace Console_App_Exercicio
{
    public class Utils
    {
        static readonly string rootDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        static readonly string dataDirectory = Path.Combine(rootDirectory, "data");

        public static Client RegisterCostumer()
        {
            Client client = new();
            Console.WriteLine("Digite o nome do cliente: ");
            client.Name = Console.ReadLine();
            Console.WriteLine("Digite o CPF do cliente: ");
            client.Cpf = Console.ReadLine();

            if(client.Name == null || client.Cpf == null || client.Name == "" ||client.Cpf == "")
            {
                Console.WriteLine("Não é possivel cadastrar cliente com dados faltando, tente novamente: ");
                client.Name = "";
                client.Cpf = "";
                return client;
            }

            Console.WriteLine($"Cliente {client.Name} cadastrado com sucesso!");
            Thread.Sleep(1000);

            return client;
        }
        public static void ListCostumer() 
        {
            Console.WriteLine("Clientes cadastrados:");
            List<Client> clients = ReadFile<Client>("clients.json");
            clients.ForEach(client => {
                Console.WriteLine($"ID: {client.Id} Nome: {client.Name} CPF: {client.Cpf}");
            });
            Console.WriteLine("\r\nPressione alguma tecla para voltar ao menu incial");
            Console.ReadKey();
        }
        public static Vehicle RegisterVehicle()
        {
            Vehicle vehicle = new();
            Console.WriteLine("Digite o Id do proprietario do veiculo: ");
            vehicle.ClientId = Console.ReadLine();
            Console.WriteLine("Digite a placa do veiculo: ");
            vehicle.LicensePlate = Console.ReadLine();
            Console.WriteLine("Digite a marca do veiculo: ");
            vehicle.Brand = Console.ReadLine();
            Console.WriteLine("Digite o modelo do veiculo: ");
            vehicle.Model = Console.ReadLine();

            if (vehicle.LicensePlate == null || vehicle.LicensePlate == null || vehicle.LicensePlate == "" || vehicle.LicensePlate == "")
            {
                Console.WriteLine("Não é possivel cadastrar veiculo sem numero de placa, tente novamente: ");
                vehicle.LicensePlate = "";
                vehicle.Brand = "";
                vehicle.Model = "";
                return vehicle;
            }

            Console.WriteLine($"Veiculo {vehicle.LicensePlate} cadastrado com sucesso!");

            Console.WriteLine("\r\nPressione alguma tecla para voltar ao menu incial");
            Console.ReadKey();

            return vehicle;

        }
        public static CheckInOut CheckinVehicle() 
        {
            CheckInOut vehicleCheck = new();
            Console.WriteLine("Insira a placa do veiculo: ");
            vehicleCheck.VehicleLicense = Console.ReadLine();
            vehicleCheck.Entrance = DateTime.Now;

            Console.WriteLine($"Veiculo {vehicleCheck.VehicleLicense} teve entrada registrada às {vehicleCheck.Entrance}");

            Console.WriteLine("\r\nPressione alguma tecla para voltar ao menu incial");
            Console.ReadKey();

            return vehicleCheck;

        }

        public static CheckInOut FindCheckIn(List<CheckInOut> checkInOutsList, string vehiclePlate)
        {
            List<CheckInOut> checks = ReadFile<CheckInOut>("checksInOut.json");
            CheckInOut vehicleCheck = new();
            foreach (CheckInOut checkInOut in checks)
            {
                if (checkInOut.VehicleLicense == vehiclePlate)
                {
                    return checkInOut;
                }
            }
            return vehicleCheck;
        }
        public static void CheckoutVehicle(List<CheckInOut> checkInOutsList, PriceTable priceTable) 
        {
            Console.WriteLine("Digite a placa do veiculo que esta saindo: ");
            string vehiclePlate = Console.ReadLine();
            CheckInOut checkIn = FindCheckIn(checkInOutsList, vehiclePlate);

            if(vehiclePlate == "" || vehiclePlate == null || checkIn.VehicleLicense == null || checkIn.VehicleLicense == "")
            {
                Console.WriteLine("Placa incorreta!");
                Thread.Sleep(1000);
                CheckoutVehicle(checkInOutsList, priceTable);
                return;
            }

            checkIn.Exit = DateTime.Now;

            TimeSpan diff = checkIn.Exit - checkIn.Entrance;

            int minutes = (int)diff.TotalMinutes;

            checkIn.Value = minutes * priceTable.PriceMinute;

            /** UPDATE JSON INFO **/
            List<CheckInOut> checks = ReadFile<CheckInOut>("checksInOut.json");

            // Filter the JSON object to identify the specific lines that need to be updated
            var filteredChecks = checks.Where(p => p.VehicleLicense == vehiclePlate);

            // Update the appropriate fields or values in the JSON object
            foreach (CheckInOut check in filteredChecks)
            {
                check.Exit = checkIn.Exit;
                check.Value = checkIn.Value;
            }

            WriteFile("checksInOut.json", checks);

            /*********************/

            Console.WriteLine($"O veículo {checkIn.VehicleLicense} teve a entrada registrada as {checkIn.Entrance} e a saída as {checkIn.Exit}");
            Console.WriteLine($"O valor a ser pago é de: {checkIn.Value}");

            Console.WriteLine("\r\nPressione alguma tecla para voltar ao menu incial");
            Console.ReadKey();
        }
        public static void CheckInOutList()
        {
            Console.WriteLine("Entradas cadastradas:");
            List<CheckInOut> checkInOutsList = ReadFile<CheckInOut>("checksInOut.json");
            checkInOutsList.ForEach(check => {
                Console.WriteLine($"Placa: {check.VehicleLicense} Entrada: {check.Entrance} Saída: {check.Exit} Valor: {check.Value}");
            });
            Console.WriteLine("\r\nPressione alguma tecla para voltar ao menu incial");
            Console.ReadKey();
        }

        public static int loadPrice()
        {
            //Valor Padrão
            int price = 2;

            String strPrice = ReadTxtFile("price.txt");
            strPrice = (strPrice != "" ? strPrice : "0");

            int filePrice = int.Parse(strPrice);
            if (filePrice > 0) price = filePrice;
            return price;
        }

        public static PriceTable UpdateHourPrice() 
        {
            PriceTable priceTable = new();
            Console.WriteLine("Digite o valor da Hora, utilize ponto para casa decimal: ");
            var hourPrice = float.TryParse(Console.ReadLine(), out var value);
            if (hourPrice)
            {
                priceTable.PriceMinute = value;

                WriteTxtFile("price.txt", value.ToString());

                Console.WriteLine("\r\nPressione alguma tecla para voltar ao menu incial");
                Console.ReadKey();
                return priceTable;
            }
            Console.WriteLine("Valor inserido não é aceito. Tente novamente");

            Console.WriteLine("\r\nPressione alguma tecla para voltar ao menu incial");
            Console.ReadKey();
            return priceTable;
        }

        public static void WriteFile<T>(string fileName, List<T> data)
        {
           
            string dataDirectory = Path.Combine(rootDirectory, "data");
            Directory.CreateDirectory(dataDirectory);
            string filePath = dataDirectory + "/" + fileName;

            string jsonString = JsonSerializer.Serialize(data);
            File.WriteAllText(filePath, jsonString);
        }

        public static List<T> ReadFile<T>(string fileName)
        {
            
            string filePath = dataDirectory + "/" + fileName;
            
            List<T> data = new List<T>();

            if(File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                data = JsonSerializer.Deserialize<List<T>>(jsonData);
            }
            
            return data;
        }

        public static void WriteTxtFile(string fileName, String text)
        {

            string dataDirectory = Path.Combine(rootDirectory, "data");
            Directory.CreateDirectory(dataDirectory);
            string filePath = dataDirectory + "/" + fileName;

            File.WriteAllText(filePath, text);
        }

        public static String ReadTxtFile(string fileName)
        {

            string filePath = dataDirectory + "/" + fileName;
            String txt = "";

            if (File.Exists(filePath))
            {
                txt = File.ReadAllText(filePath);
            }

            return txt;
        }
    }
}
