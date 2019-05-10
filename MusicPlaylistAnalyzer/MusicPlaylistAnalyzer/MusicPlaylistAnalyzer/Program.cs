using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAnalyzer
{
    class Song
    {
        public string Name;
        public string Artist;
        public string Album;
        public string Genre;
        public int Size;
        public int Time;
        public int Year;
        public int Plays;

        public Song(string name, string artist, string album, string genre, int size, int time, int year, int plays)
        {
            Name = name;
            Artist = artist;
            Album = album;
            Genre = genre;
            Size = size;
            Time = time;
            Year = year;
            Plays = plays;
        }


        override public string ToString()
        {
            return String.Format("Name: {0}, Artist: {1}, Album: {2}, Genre: {3}, Size: {4}, Time: {5}, Year: {6}, Plays: {7}", Name, Artist, Album, Genre, Size, Time, Year, Plays);
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            string report = null;

            int i;

            List<Song> RowCol = new List<Song>();

            try
            {
                if (File.Exists($"SampleMusicPlaylist.txt") == false)
                {
                    Console.WriteLine("Sorry, unable to locate the file.");
                }
                else
                {
                    StreamReader sr = new StreamReader($"SampleMusicPlaylist.txt");

                    i = 0;

                    string line = sr.ReadLine();
                    while ((line = sr.ReadLine()) != null)
                    {
                        i = i + 1;
                        try
                        {
                            string[] strings = line.Split('\t');

                            if (strings.Length < 8)
                            {
                                Console.Write("Record file contains the incorrect number of elements.");
                                Console.WriteLine($"Row {i} contains {strings.Length}  values. It should contain 8.");
                                break;
                            }
                            else
                            {
                                Song dataTemp = new Song((strings[0]), (strings[1]), (strings[2]), (strings[3]), Int32.Parse(strings[4]), Int32.Parse(strings[5]), Int32.Parse(strings[6]), Int32.Parse(strings[7]));
                                RowCol.Add(dataTemp);
                            }
                        }
                        catch
                        {
                            Console.Write("Unable to read playlist data file...");
                            break;
                        }
                    }
                    sr.Close();
                }
            }


            catch (Exception e)
            {
                Console.WriteLine("Unable to locate the Playlist file...");
            }


            try
            {
                Song[] songs = RowCol.ToArray();

                using (StreamWriter write = new StreamWriter("PlaylistReport.txt"))
                {
                    write.WriteLine("Music Playlist Report\n");



                    var songStart = from song in songs where song.Plays >= 200 select song;

                    report += "\nSongs that received 200 or more plays:\n \n";

                    foreach (Song song in songStart)
                    {
                        report += song + "\n";
                    }


                    var alternativeSongs = from song in songs where song.Genre == "Alternative" select song;
                    i = 0;

                    foreach (Song song in alternativeSongs)
                    {
                        i++;
                    }

                    report += $"\nNumber of Alternative songs: {i}\n \n";



                    var hipHopSongs = from song in songs where song.Genre == "Hip-Hop/Rap" select song;
                    i = 0;

                    foreach (Song song in hipHopSongs)
                    {
                        i++;
                    }
                    report += $"Number of Hip-Hop/Rap songs: {i}\n \n";


                    var fishbowlSongs = from song in songs where song.Album == "Welcome to the Fishbowl" select song;
                    report += "Songs from the album \"Welcome to the Fishbowl\":\n \n";
                    foreach (Song song in fishbowlSongs)
                    {
                        report += song + "\n";
                    }


                    var songs1970s = from song in songs where song.Year < 1970 select song;
                    report += "\nSongs from before 1970:\n \n";
                    foreach (Song song in songs1970s)
                    {
                        report += song + "\n";
                    }


                    var ultra85songs = from song in songs where song.Name.Length > 85 select song.Name;
                    report += "\nSong names longer than 85 characters:\n \n";
                    foreach (string name in ultra85songs)
                    {
                        report += name + "\n";
                    }


                    var LongestSong = from song in songs orderby song.Time descending select song;
                    report += "\nLongest song:\n \n";
                    report += LongestSong.First();

                    write.Write(report);
                    write.Close();
                }
                Console.WriteLine("Music Playlist Analyzer");
                Console.WriteLine("\n[Report file PlaylistReport.txt has been saved successfully]");
                Console.WriteLine("\n\nPress Enter to exit...");
            }


            catch (Exception e)
            {
                Console.WriteLine("Report file canʼt be opened or written to.");
            }

            Console.ReadLine();
        }
    }
}