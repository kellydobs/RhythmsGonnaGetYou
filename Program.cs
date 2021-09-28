using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;



namespace RhythmsGonnaGetYou
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Welcome to Kel and El Records");

      var context = new KelAndElRecords();
      

      List<Band> bandsList = context.Band.Include(Band => Band.Albums).ThenInclude(Album => Album.Songs).ToList();

      bool keepGoing = true;
     
      while (keepGoing)
      {
        var input = PromptInput("(C)reate, (V)iew, (U)pdate a band or (Q)uit? ").ToUpper();
        
        switch (input)
        {
          case "C":
            input = PromptInput("Create a (B)and, (A)lbum, or (S)ong? ").ToUpper();

            switch (input)
            {
              case "B":

                context.Band.Add(CreateBand());
                context.SaveChanges();
                 break;

                 case "A":
                 Album newAlbum = CreateAlbum();

                 break;

          case "S":



          break;


          case "V":


          break;

          case "U":

          break;

          default:
          //Quit the program
          keepGoing = false;
          break;


        }

      }
    

      // foreach (Band b in bandsList) 
      // {
      //   Console.WriteLine($"Band name is {b.Name}");
      //   foreach (Album a in b.Albums) 
      //   { 
      //     Console.WriteLine($"Album name is {a.Title}");
      //     foreach (Song s in a.Songs) 
      //     {
      //       Console.WriteLine($"The Song {s.Title} is an album {a.Title} by the band {b.Name}");
      //     }
        }
      }
    


    public static Band CreateBand()
    {
      var newBand = new Band
      {
       Name = PromptInput("New band name: "),
       CountryOfOrigin = PromptInput("New band country of origin: "),
       NumberOfMembers = int.Parse(PromptInput("New band number of members: ")),
       Website = PromptInput ("New band website:  "),
       Style = PromptInput ("New band style: "),
       Signed = (PromptInput ("Is the new band signed?").ToUpper() == "YES" ? true : false),
       ContactName = PromptInput ("New band contact name: "),
       ContactPhoneNumber = PromptInput ("New band contact number: ")
      };
      return newBand;
    
    }

     public static Album CreateAlbum()
    {
      var newAlbum = new Album
      {
        Title = PromptInput("New Album name: "),
        IsExplicit = (PromptInput("Is the new album explicit?").ToUpper() == "YES" ? true : false),
        ReleaseDate = DateTime.Parse(PromptInput("What date was it released? (YYYY-MM-DD)"))
        };

      return newAlbum;

     public static Song CreateSong()
    {
      Song newSong = new Song();
      return newSong;
    }

    public static string PromptInput(string prompt)
    {
      Console.WriteLine(prompt);
      string response = Console.ReadLine();
      return response;
    }
  }
  class KelAndElRecords : DbContext
  {
    // Define a movies property that is a DbSet.
    public DbSet<Band> Band { get; set; }
    public DbSet<Album> Album { get; set; }
    public DbSet<Song> Song { get; set; }


    // Define a method required by EF that will configure our connection
    // to the database.
    //
    // DbContextOptionsBuilder is provided to us. We then tell that object
    // we want to connect to a postgres database named suncoast_movies on
    // our local machine.
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseNpgsql("server=localhost;database=KelAndElRecords");
    }
  }


  class Band
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string CountryOfOrigin { get; set; }
    public int NumberOfMembers { get; set; }
    public string Website{ get; set; }
    public string Style { get; set; }
    public bool Signed { get; set; }
    public string ContactName { get; set; }
    public string ContactPhoneNumber { get; set; }

    public List<Album> Albums { get; set; }
  }

  class Album
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsExplicit { get; set; }
    public DateTime ReleaseDate { get; set; }
   
    public int BandId { get; set; }
    public Band Band { get; set; }

    public List<Song> Songs { get; set;}
    
  }

 class Song
  {
    public int Id { get; set; }
    public int TrackNumber { get; set; }
    public string Title { get; set; }
    public string Duration { get; set; }
    public int BandId { get; set; }
    public int AlbumId { get; set; }

    public Album Album { get; set; }
  }


}
