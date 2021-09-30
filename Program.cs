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
      Band bandByName = null;



      bool keepGoing = true;
     
      while (keepGoing)
      {
        var input = PromptInput("(C)reate, (V)iew, (U)pdate a band or (Q)uit? ").ToUpper();
        var input = PromptInput('\n' + "(C)reate, (V)iew, (U)pdate a band, or (Q)uit? ").ToUpper();
        
        switch (input)
        {
          case "C":
            input = PromptInput("Create a (B)and, (A)lbum, or (S)ong? ").ToUpper();
            input = PromptInput('\n' + "Create a (B)and, (A)lbum, or (S)ong? ").ToUpper();
           
            switch (input)
            {
              case "B":

                context.Band.Add(CreateBand());
                context.SaveChanges();
                 break;

                 case "A":
                 Album newAlbum = CreateAlbum();

                 input = PromptInput("What is the name of the band which produced the album?");
                 bandByName = context.Band.FirstOrDefault(b => b.Name.ToUpper().Contains(input.ToUpper()));

                 if (bandByName != null)
                 {
                   Album newAlbum = CreateAlbum();
                   newAlbum.BandId = bandByName.Id;
                   context.Add(newAlbum);
                   context.SaveChanges(); 
                 }
                 else
                 {
                   Console.WriteLine("That band does not exist..");
                 }

                 break;

          case "S":
                  input = PromptInput("What is the name of the album which contants the song?");
                  Album albumByName = context.Album.FirstOrDefault(a => a.Title.ToUpper().Contains(input.ToUpper()));
                  
                  if (albumByName != null)
                  {
                    Song newSong = CreateSong();
                    newSong.AlbumId = albumByName.Id;
                    context.Add(newSong);
                    context.SaveChanges();
                  }
                  else
                  {
                    Console.WriteLine("That album does not exist....");
                  }
                  break;
            }



          break;


          case "V":

          input = PromptInput('\n' + "Select a number: \n" +

          "1. View all Bands\n" +
          "2. View all albums for a band\n" +
          "3. View all bands that are signed\n" +
          "4. View all bands that are not signed\n" +
          "5. View all albums by ordered by Release Date" + '\n');

          List<Band> bandsList = context.Band.Include(Band => Band.Albums).ThenInclude(Album => Album.Songs).ToList();

          switch (input)
          {
            case "1":

            Console.WriteLine(String.Format(("{0,-20} | {1,-20} | {2,-8} | {3,-20}"), "Band Name", "Country of Origin", "Numbers", "Website"));
            Console.WriteLine("____________________________________________________________________");

            //view all bands
            foreach (Band b in bandsList)
            {
              Console.WriteLine(String.Format(("{0,-20} | {1,-20} | {2,-8} | {3,-20}"), b.Name, b.CountryOfOrigin, b.NumberOfMembers, b.Website));
            }
            break;


            case "2":

            input = PromptInput("What is the name of the band?");

            bandByName = context.Band.FirstOrDefault(b => b.Name.ToUpper().Contains(input.ToUpper()));

            if (bandByName != null)
          
              {
                Console.WriteLine($"\nAlbums by {bandByName.Name}");
                Console.WriteLine(String.Format(("{0,-20} | {1,-20} | {2,-20}"), "Album Title", "Explicit?", "Release Date"));
                Console.WriteLine("____________________________________________________________");
                foreach (Album a in bandByName.Albums)
                {
                  Console.WriteLine(String.Format(("{0,-20} | {1,-20} | {2,-20}"), a.Title, a.IsExplicit, a.ReleaseDate));
                }
              }
                else
                {
                  Console.WriteLine("Could not find band...");
                }

            
            break;

            case "3":
            bandsList = bandsList.Where(b => b.Signed).ToList();

            Console.WriteLine("Bands that are signed: ");

            foreach(Band b in bandsList)
            {
              Console.WriteLine(b.Name);

            }
              Console.WriteLine();

            break;


            case "4":
            bandsList = bandsList.Where(b => !b.Signed).ToList();  

            Console.WriteLine("Bands that are not signed: ");

            foreach (Band b in bandsList)
            {
              Console.WriteLine(b.Name);
            }
              Console.WriteLine();

            break;


            case "5":
              List<Album> albumsList = context.Album.OrderBy(album => album.ReleaseDate).ToList();

            foreach (Album a in albumsList)
            {
              Console.WriteLine($"{a.Title} was released on {a.ReleaseDate}");
            }
              Console.WriteLine();

            break;

            

          }


          break;

          case "U":
          //find a band and update is signed
          input = PromptInput("What is the name of the band you want to sign/release?");

          bandByName = context.Band.FirstOrDefault(b => b.Name.ToUpper().Contains(input.ToUpper()));

          if (bandByName != null)
          {
            var isSignedOrNah = (bandByName.Signed ? "signed" : "not signed" );
            Console.WriteLine($"{bandByName} is currently {isSignedOrNah}");
            input = PromptInput($"Would you like ot (S)ign or (R)elease {bandByName.Name}");
            bandByName.Signed = (input.ToUpper() == "S" ? true : false);
            context.Update(bandByName);
            context.SaveChanges();
            isSignedOrNah = (bandByName.Signed ? "signed" : "not signed");
            Console.WriteLine($"{bandByName.Name} is now {isSignedOrNah}");

          }
          else
          {
            Console.WriteLine("Could not find band by that name....");

          }
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
    }

     public static Song CreateSong()
    {
      Song newSong = new Song
      {
        TrackNumber = int.Parse(PromptInput("what is the track number? ")),
        Title = PromptInput("New song name:  "),
        Duration = PromptInput("What is the duration of the song?")
      };
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

