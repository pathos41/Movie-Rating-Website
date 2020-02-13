using System.Collections.Generic;
using MovieSite.Models;
using MovieSite.Services;
using Microsoft.AspNetCore.Mvc;

namespace MovieSite.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class MoviesController : ControllerBase
  {
    //Constructor
    public MoviesController(JsonFileMovieService movieService)
    {
      MovieService = movieService;
    }

    public JsonFileMovieService MovieService { get; }

    [HttpGet]
    //Get all movies
    public IEnumerable<Movie> Get()
    {
      return MovieService.GetMovies();
    }

    [HttpPatch]
    //Update movie ratings
    public ActionResult Patch([FromBody] RatingRequest request)
    {
      MovieService.AddRating(request.MovieId, request.Rating);
            
      return Ok();
    }

    public class RatingRequest
    {
      public string MovieId { get; set; }
      public int Rating { get; set; }
    }
  }
}