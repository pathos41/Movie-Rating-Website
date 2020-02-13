using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using MovieSite.Models;
using Microsoft.AspNetCore.Hosting;

namespace MovieSite.Services
{
  public class JsonFileMovieService
  {
    //Constructor
    public JsonFileMovieService(IWebHostEnvironment webHostEnvironment)
    {
      WebHostEnvironment = webHostEnvironment;
    }

    public IWebHostEnvironment WebHostEnvironment { get; }

    //Get the file path
    private string JsonFileName
    {
      get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "movies.json"); }
    }

    public IEnumerable<Movie> GetMovies()
    {
      //Open the file from the path
      using(var jsonFileReader = File.OpenText(JsonFileName))
      {
        //Deserialize the JSON file
        return JsonSerializer.Deserialize<Movie[]>(jsonFileReader.ReadToEnd(),
          new JsonSerializerOptions
          {
            PropertyNameCaseInsensitive = true
          });
      }
    }

    public void AddRating(string movieId, int rating)
    {
      //Get all movies
      var movies = GetMovies();

      //Find movie by its id
      if(movies.First(x => x.Id == movieId).Ratings == null)
      {
        //Set the rating array if it doesn't exist yet
        movies.First(x => x.Id == movieId).Ratings = new int[] { rating };
      }
      else
      {
        //Add the new rating to the rating array
        var ratings = movies.First(x => x.Id == movieId).Ratings.ToList();
        ratings.Add(rating);
        movies.First(x => x.Id == movieId).Ratings = ratings.ToArray();
      }

      using(var outputStream = File.OpenWrite(JsonFileName))
      {
        JsonSerializer.Serialize<IEnumerable<Movie>>(
          new Utf8JsonWriter(outputStream, new JsonWriterOptions
          {
            SkipValidation = true,
            Indented = true
          }),
          movies
        );
      }
    }
  }
}