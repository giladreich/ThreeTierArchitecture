﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TeamManager.Database;

namespace TeamManager.Tests
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("mLab MongoDB Connection via MongoDB.Driver 2.4.4 for ConceptType.Second:\n");
            Console.WriteLine(
                "- TeamManager temporarily set to Output type: Console Application. \n- Code in Program.cs Main temporarily commented out.\n");
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();

            TestMongoDB();
        }

        private static void TestMongoDB()
        {
            Console.Clear();
            Console.WriteLine("Connecting to MongoDB.");
            DBLayerMongo mongo = new DBLayerMongo();
            Console.WriteLine("DB connected.");

            Console.WriteLine("Showing all Teams.");
            

            //Console.WriteLine("Dropping old collections.");
            //DBLayerMongo.Database.DropCollection("team");
            //DBLayerMongo.Database.DropCollection("player");

            //Console.WriteLine("Creating Team.");
            //DBLayerMongo.Team.InsertOne(new Team()
            //{
            //    Name = "Team One"
            //});
            //DBLayerMongo.Team.InsertOne(new Team()
            //{
            //    Name = "Team Two"
            //});
            //DBLayerMongo.Team.InsertOne(new Team()
            //{
            //    Name = "Team Three"
            //});

            //Console.WriteLine("Getting first Team Id.");
            //string teamId = DBLayerMongo.Team.Find(t => t.Name == "Team One").First().Id;

            //Console.WriteLine("Creating first Team Player.");
            //DBLayerMongo.Player.InsertOne(new Player()
            //{
            //    Name = "Player One",
            //    Team = teamId
            //});
            //DBLayerMongo.Player.InsertOne(new Player()
            //{
            //    Name = "Player Two",
            //    Team = teamId
            //});
            //DBLayerMongo.Player.InsertOne(new Player()
            //{
            //    Name = "Player Three",
            //    Team = teamId
            //});

            //Console.WriteLine("Getting second Team Id.");
            //string secondTeamId = DBLayerMongo.Team.Find(t => t.Name == "Team Two").First().Id;

            //Console.WriteLine("Creating second Team Player.");
            //DBLayerMongo.Player.InsertOne(new Player()
            //{
            //    Name = "Player Four",
            //    Team = secondTeamId
            //});
            //DBLayerMongo.Player.InsertOne(new Player()
            //{
            //    Name = "Player Five",
            //    Team = secondTeamId
            //});

            //Console.WriteLine("Getting third Team Id.");
            //string thirdTeamId = DBLayerMongo.Team.Find(t => t.Name == "Team Three").First().Id;

            //Console.WriteLine("Creating third Team Player.");
            //DBLayerMongo.Player.InsertOne(new Player()
            //{
            //    Name = "Player Six",
            //    Team = thirdTeamId
            //});
            //DBLayerMongo.Player.InsertOne(new Player()
            //{
            //    Name = "Player Seven",
            //    Team = thirdTeamId
            //});
            //DBLayerMongo.Player.InsertOne(new Player()
            //{
            //    Name = "Player Eight",
            //    Team = thirdTeamId
            //});
            //DBLayerMongo.Player.InsertOne(new Player()
            //{
            //    Name = "Player Nine",
            //    Team = thirdTeamId
            //});

            Console.WriteLine("Finished creating Team and Player.");
            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();

            Console.Clear();
            Console.WriteLine("Querying for Team One.");
            //List<Team> teams = DBLayerMongo.Team.Find(t => t.Name == "Team One").ToList();
            //Console.WriteLine("Team found: {0}", teams.Count());
            //string teamId = "";
            //foreach (Team team in teams)
            //{
            //    Console.WriteLine(team.Name);
            //    teamId = team.Id;

            //    Console.WriteLine("Querying for Player of {0}.", team.Name);
            //    List<Player> players = DBLayerMongo.Player.Find(p => p.Team == teamId).ToList();
            //    Console.WriteLine("Player found: {0}", players.Count());
            //    foreach (Player player in players)
            //    {
            //        Console.WriteLine(player.Name);
            //    }
            //}

            Console.WriteLine("Query finished.");
            Console.WriteLine("\nPress any key to quit.");
            Console.ReadKey();
        }

    }
}