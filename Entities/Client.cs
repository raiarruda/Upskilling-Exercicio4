namespace Console_App_Exercicio.Entities
{
    public class Client
    {
        public Guid Id = Guid.NewGuid();
        public string Name { get; set; }
        public string Cpf { get; set; }
    }
}