using System;
using System.Collections.Generic;
using System.Text;

namespace Persistencia.Entidades
{
    public class Actor
    {
        public int ActorId { get; set; }
        
        public string Name { get; set; }
        
        public DateTime DateBirth { get; set; }

        public virtual ICollection<ActorMovie> Characters { get; set; }
    }
}
