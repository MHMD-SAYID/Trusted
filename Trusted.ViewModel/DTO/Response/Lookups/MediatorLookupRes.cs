using System;
using System.Collections.Generic;
using System.Text;

namespace Trusted.ViewModel.DTO.Response.Lookups
{
    public class MediatorLookupRes
    {
        public ICollection<Mediator> Mediators { get; set; }= new List<Mediator>();
    }
    public class Mediator
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
