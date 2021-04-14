
using Persistencia.Entidades;
using Persistencia.Repositorio;
using System.Linq;

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Persistencia
{
    class Program
    {
        static void Main(string[] args)
        {
            MovieContext _context = new MovieContext();
           
            #region # LINQ - minhas consulta


            //1.Listar o nome de todos personagens desempenhados por um determinado ator, incluindo a informação de qual o filme


            var charactersByActor = _context.Characters
                                    .Where(c => c.Actor.Name == "Judi Dench")
                                    .Select(c => new
                                    {
                                        c.Actor.Name,
                                        c.Movie.Title
                                    });
            Console.WriteLine("1 - Personagens do ator Judi Dench");

            int countCharacters = 0;
            foreach (var elem in charactersByActor)
            {
                countCharacters++;
                Console.WriteLine("\t{0} - {1}", countCharacters, elem.Title);
            }


            //2.Mostrar o nome de todos atores que desempenharam um determinado personagem(por exemplo, quais os atores que já atuaram como "007" ?)
            var actorsCharacters = _context.Characters
                                    .Where(c => c.Character == "James Bond")
                                    .Select(c => new
                                    {
                                        c.Character,
                                        c.Actor.Name
                                    });
            Console.WriteLine("2 - Atores do personagem Darth Vader");

            int countActors = 0;
            foreach (var elem in actorsCharacters.Distinct())
            {
                countActors++;
                Console.WriteLine("\t{0} - {1}", countActors, elem.Name);
            }

            //3.Informar qual o ator desempenhou mais vezes um determinado personagem(por exemplo: qual o ator que realizou mais filmes como o “agente 007”) 
            var actorsByMovie = _context.Characters
                                   .Where(c => c.Movie.Title == "Star Wars")
                                   .Select(c => new
                                   {
                                       c.Actor.Name
                                   });

            var mostFrequentActorByMovie = actorsByMovie
                .GroupBy(a => a.Name)
                .OrderByDescending(a => a.Count())
                .Take(1)
                .Select(a => a.Key)
                .ToList()
                .FirstOrDefault();

            Console.WriteLine("3 - Ator que mais fez Star Wars");
            Console.WriteLine("\t{0}", mostFrequentActorByMovie);


            //4.Mostrar o nome e a data de nascimento do ator mais idoso e o mais novo 
            Actor theNewestActor = _context.Actors
                    .OrderBy(a => a.DateBirth).LastOrDefault();
            Actor theEldestActor = _context.Actors
                    .OrderBy(a => a.DateBirth).FirstOrDefault();

            Console.WriteLine("4 - Ator mais novo - {0}", theNewestActor.Name);
            Console.WriteLine("4 - Ator mais velho - {0}", theEldestActor.Name);


            //5.Mostrar o nome e a data de nascimento do ator mais idoso e o mais novo de um determinado gênero
            var theNewestActorByGender = _context.Characters
                    .Where(a => a.Movie.Genre.Name == "Action")
                    .Select(a => new
                    {
                        a.Actor.Name,
                        a.Actor.DateBirth
                    }
                     )
                    .OrderBy(a => a.DateBirth)
                    .ToList()
                    .LastOrDefault();

            var theEldestActorByGender = _context.Characters
                    .Where(a => a.Movie.Genre.Name == "Action")
                    .Select(a => new
                    {
                        a.Actor.Name,
                        a.Actor.DateBirth
                    }
                     )
                    .OrderBy(a => a.DateBirth)
                    .ToList()
                    .FirstOrDefault();


            Console.WriteLine("5 - Ator mais novo  do Genêro Action - {0}", theNewestActorByGender.Name);
            Console.WriteLine("5 - Ator mais velho do Genêro Action - {0}", theEldestActorByGender.Name);

            //6.Mostrar o valor médio das avaliações dos filmes que um determinado ator participou
            var sumRateByActor = _context.Characters
                                  .Where(c => c.Actor.Name == "Daniel Craig")
                                  .Select(c =>
                                      (decimal)c.Movie.Rating
                                  ).Sum();
            var lengthRateByActor = _context.Characters
                                  .Where(c => c.Actor.Name == "Daniel Craig")
                                  .Select(c =>
                                      c.Movie
                                  ).Distinct()
                                  .Count();
            Console.WriteLine("6 - Valor médio das avaliações dos filmes do ator Daniel Craig {0}", sumRateByActor / lengthRateByActor);

            //7.Qual o elenco do filme PIOR avaliado ?

            var worstRatingMovie = _context.Movies
                .OrderByDescending(m => m.Rating)
                .Last();

            var castworstRatingMovie = _context.Characters
                .Where(character => character.Movie.MovieId == worstRatingMovie.MovieId)
                .Select(character => character.Actor.Name);



            Console.WriteLine("7 - Elenco do filme pior avaliado: ({0})", worstRatingMovie.Title);
            int countActorsWorstMovie = 0;
            foreach (var actor in castworstRatingMovie)
            {
                countActorsWorstMovie++;
                Console.WriteLine("\t{0} - {1}", countActorsWorstMovie, actor);
            }

            //8.Qual o elenco do filme com o pior faturamento?
            var lowestGrossMovie = _context.Movies
               .OrderBy(movie => movie.Gross).First();

            var castLowestGrossMovie = _context.Characters
                .Where(character => character.Movie.MovieId == lowestGrossMovie.MovieId)
                .Select(character => character.Actor.Name);


            Console.WriteLine("8 - Elenco do filme com o pior faturamento:({0})", lowestGrossMovie.Title);

            int countActorsWorstGrossMovie = 0;
            foreach (var actor in castLowestGrossMovie)
            {
                countActorsWorstGrossMovie++;
                Console.WriteLine("\t{0} - {1}", countActorsWorstGrossMovie, actor);
            }

            //9. Quantidade de personagens por um filme
            var movie = _context.Characters
               .Where(c => c.Movie.Title == "Jurassic Park")
               .Select(c => new
               {
                   Title = c.Movie.Title,
                   Count = c.Movie.Title.Count()
               }
               ).FirstOrDefault();

             Console.WriteLine("9 - O filme {0} tem {1} personagens", movie.Title, movie.Count);

            //10. Filme Mais antigo
            var oldestMovie = _context.Movies
                .OrderBy(c => c.ReleaseDate)
                .First();
            Console.WriteLine("9 - O filme mais antigo é o {0}, lançado na data {1}", oldestMovie.Title, oldestMovie.ReleaseDate.Date);


            #endregion

        }
    }
    
}

