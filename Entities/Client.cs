namespace Console_App_Exercicio.Entities
{
    public class Client
    {
        public Client()
        {
            this.id = Guid.NewGuid().ToString();
        }

        public Client(string _id)
        {
            this.id = _id;
        }

        private string id;
        public string Id
        {
            get
            {
                return this.id;
            }
        }
        public string Name { get; set; }
        public string Cpf { get; set; }
    }
}