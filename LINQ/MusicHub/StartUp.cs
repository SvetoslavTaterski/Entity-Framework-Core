using System.Globalization;
using System.Text;

namespace MusicHub
{
    using System;

    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here

            var result = ExportSongsAboveDuration(context, 4);
            Console.WriteLine(result);
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            StringBuilder sb = new StringBuilder();

            var albums = context.Albums
                .Where(a => a.ProducerId.HasValue && a.ProducerId == producerId)
                .ToArray()
                .OrderByDescending(a => a.Price)
                .Select(a => new
                {
                    Name = a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    ProducerName = a.Producer!.Name,
                    Songs = a.Songs
                        .Select(s => new
                        {
                            SongName = s.Name,
                            Price = s.Price.ToString("f2"),
                            SongWriterName = s.Writer.Name
                        })
                        .OrderByDescending(s => s.SongName)
                        .ThenBy(w => w.SongWriterName)
                        .ToArray(),
                    TotalAlbumPrice = a.Price.ToString("f2")
                });

            foreach (var album in albums)
            {
                sb.AppendLine($"-AlbumName: {album.Name}");
                sb.AppendLine($"-ReleaseDate: {album.ReleaseDate}");
                sb.AppendLine($"-ProducerName: {album.ProducerName}");
                sb.AppendLine($"-Songs:");

                int songNumber = 1;

                foreach (var song in album.Songs)
                {
                    sb.AppendLine($"---#{songNumber}");
                    sb.AppendLine($"---SongName: {song.SongName}");
                    sb.AppendLine($"---Price: {song.Price}");
                    sb.AppendLine($"---Writer: {song.SongWriterName}");

                    songNumber++;
                }

                sb.AppendLine($"-AlbumPrice: {album.TotalAlbumPrice}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            StringBuilder sb = new StringBuilder();

            var songs = context.Songs
                .AsEnumerable()
                .Where(s => s.Duration.TotalSeconds > duration)
                .Select(s => new
                {
                    Name = s.Name,
                    PerformerFullName = s.SongPerformers
                        .Select(sp => $"{sp.Performer.FirstName} {sp.Performer.LastName}")
                        .OrderBy(sp => sp)
                        .ToArray(),
                    WriterName = s.Writer.Name,
                    AlbumProducer = s.Album!.Producer!.Name,
                    Duration = s.Duration.ToString("c")
                })
                .OrderBy(s => s.Name)
                .ThenBy(s => s.WriterName)
                .ToArray();

            int songCounter = 1;

            foreach (var song in songs)
            {
                sb.AppendLine($"-Song #{songCounter}");
                sb.AppendLine($"---SongName: {song.Name}");
                sb.AppendLine($"---Writer: {song.WriterName}");

                foreach (var performer in song.PerformerFullName)
                {
                    sb.AppendLine($"---Performer: {performer}");
                }

                sb.AppendLine($"---AlbumProducer: {song.AlbumProducer}");
                sb.AppendLine($"---Duration: {song.Duration}");

                songCounter++;
            }

            return sb.ToString().TrimEnd();
        }
    }
}
