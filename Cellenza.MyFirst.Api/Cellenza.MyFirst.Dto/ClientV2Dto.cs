namespace Cellenza.MyFirst.Dto
{
    public class ClientV2Dto
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public string Url { get; set; }

        public override int GetHashCode()
        {
            return 0;
        }

        public override bool Equals(object obj)
        {
            var client = obj as ClientV2Dto;

            if (client == null)
            {
                return false;
            }

            return this.DisplayName == client.DisplayName;
        }
    }
}