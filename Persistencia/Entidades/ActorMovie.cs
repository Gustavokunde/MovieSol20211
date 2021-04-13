using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Persistencia.Entidades
{
    public class ActorMovie
    {
        public int ActorMovieId { get; set; }
       
        public string Character { get; set; }

        [ForeignKey("MovieId")]
        public int MovieId { get; set; }

        public Movie Movie { get; set; }

        [ForeignKey("ActorId")]
        public int ActorId { get; set; }

        public Actor Actor { get; set; }
    }
}
