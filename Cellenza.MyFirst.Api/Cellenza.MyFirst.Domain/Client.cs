using System.ComponentModel.DataAnnotations;

namespace Cellenza.MyFirst.Domain
{
    public class Client
    {
        public int Id { get; set; }

        [MaxLength(60)]
        public string Name { get; set; }
    }
}