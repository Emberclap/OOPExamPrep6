using NUnit.Framework;
using System;
using System.IO;
using System.Numerics;

namespace FootballTeam.Tests
{
    [TestFixture]
    public class Tests
    {
        private string name;
        private FootballTeam team;
        private int capacity;
        [SetUp]
        public void Setup()
        {
            this.name = "ManUn";
            this.capacity = 15;
            this.team = new FootballTeam(name, capacity);
        }

        [Test]
        public void ConstructorShould_initializeProperly()
        {
            FootballTeam team = new FootballTeam(name, capacity);
            Assert.IsNotNull(team);
            Assert.AreEqual(name, team.Name);
            Assert.AreEqual(capacity, team.Capacity);
            Assert.That(team.Players.Count == 0);

        }
        [Test]
        public void ConstructorShould_ReturnExceptionIfNameIsNullOrEmpty()
        {

            ArgumentException ex = Assert.Throws<ArgumentException>(()
                => team = new FootballTeam("", capacity));

            Assert.That(ex.Message == "Name cannot be null or empty!");
        }
        [Test]
        public void ConstructorShould_ReturnExceptionIfCapacityIslessThan15()
        {

            ArgumentException ex = Assert.Throws<ArgumentException>(()
                => team = new FootballTeam(this.name, 14));

            Assert.That(ex.Message == "Capacity min value = 15");
        }
        [Test]
        public void AddingNewPlayerToTeam()
        {
            FootballPlayer player = new FootballPlayer("Pesho", 1, "Goalkeeper");
            FootballPlayer player2 = new FootballPlayer("Pesho2", 2, "Midfielder");

            string expectedMessage = this.team.AddNewPlayer(player);
            Assert.AreEqual($"Added player {player.Name} in position {player.Position} with number {player.PlayerNumber}"
                , expectedMessage);
            expectedMessage = this.team.AddNewPlayer(player2);
            Assert.AreEqual($"Added player {player2.Name} in position {player2.Position} with number {player2.PlayerNumber}"
                , expectedMessage);
        }
        [Test]
        public void AddPlayerShould_ReturnMessageIf_ThereAreNoMoreSpotsForPlayers()
        {
            FootballPlayer player = new FootballPlayer("Pesho", 1, "Goalkeeper");
            FootballPlayer player2 = new FootballPlayer("Pesho2", 2, "Midfielder");
            FootballPlayer player3 = new FootballPlayer("Pesho3", 3, "Forward");
            FootballPlayer player4 = new FootballPlayer("Pesho4", 4, "Goalkeeper");
            FootballPlayer player5 = new FootballPlayer("Pesho5", 5, "Midfielder");
            FootballPlayer player6 = new FootballPlayer("Pesho6", 6, "Midfielder");
            FootballPlayer player7 = new FootballPlayer("Pesho7", 7, "Goalkeeper");
            FootballPlayer player8 = new FootballPlayer("Pesho8", 8, "Midfielder");
            FootballPlayer player9 = new FootballPlayer("Pesho9", 9, "Midfielder");
            FootballPlayer player10 = new FootballPlayer("Pesho10", 10, "Forward");
            FootballPlayer player11 = new FootballPlayer("Pesho11", 11, "Forward");
            FootballPlayer player12 = new FootballPlayer("Pesho12", 12, "Forward");
            FootballPlayer player13 = new FootballPlayer("Pesho13", 13, "Forward");
            FootballPlayer player14 = new FootballPlayer("Pesho14", 14, "Forward");
            FootballPlayer player15 = new FootballPlayer("Pesho15", 15, "Midfielder");
            FootballPlayer player16 = new FootballPlayer("Pesho16", 16, "Goalkeeper");
            this.team.AddNewPlayer(player);
            this.team.AddNewPlayer(player2);
            this.team.AddNewPlayer(player3);
            this.team.AddNewPlayer(player4);
            this.team.AddNewPlayer(player5);
            this.team.AddNewPlayer(player6);
            this.team.AddNewPlayer(player7);
            this.team.AddNewPlayer(player8);
            this.team.AddNewPlayer(player9);
            this.team.AddNewPlayer(player10);
            this.team.AddNewPlayer(player11);
            this.team.AddNewPlayer(player12);
            this.team.AddNewPlayer(player13);
            this.team.AddNewPlayer(player14);
            this.team.AddNewPlayer(player15);

            string expectedMessage = this.team.AddNewPlayer(player16);
            Assert.AreEqual("No more positions available!", expectedMessage);
        }
        [Test]
        public void PickPlayerShould_WorkProperly()
        {
            FootballPlayer player = new FootballPlayer("Pesho", 1, "Goalkeeper");
            FootballPlayer player2 = new FootballPlayer("Pesho2", 2, "Midfielder");
            FootballPlayer player3 = new FootballPlayer("Pesho3", 3, "Forward");
            FootballPlayer player4 = new FootballPlayer("Pesho4", 4, "Goalkeeper");
            FootballPlayer player5 = new FootballPlayer("Pesho5", 5, "Midfielder");
            this.team.AddNewPlayer(player);
            this.team.AddNewPlayer(player2);
            this.team.AddNewPlayer(player3);
            this.team.AddNewPlayer(player4);
            this.team.AddNewPlayer(player5);
            Assert.That(player2 == team.PickPlayer("Pesho2"));
            Assert.That(player5 == team.PickPlayer("Pesho5"));

        }
        [Test]
        public void PlayerScoreShould_IncreasePlayerScore()
        {
            FootballPlayer player = new FootballPlayer("Pesho", 1, "Goalkeeper");
            FootballPlayer player2 = new FootballPlayer("Pesho2", 2, "Midfielder");
            FootballPlayer player3 = new FootballPlayer("Pesho3", 3, "Forward");
            FootballPlayer player4 = new FootballPlayer("Pesho4", 4, "Goalkeeper");
            FootballPlayer player5 = new FootballPlayer("Pesho5", 5, "Midfielder");
            this.team.AddNewPlayer(player);
            this.team.AddNewPlayer(player2);
            this.team.AddNewPlayer(player3);
            this.team.AddNewPlayer(player4);
            this.team.AddNewPlayer(player5);
            team.PlayerScore(3);
            team.PlayerScore(3);
            int expectedGoals = player3.ScoredGoals;
            team.PlayerScore(3);
            int expectedGoalsAfter = expectedGoals + 1;
            Assert.That(expectedGoalsAfter, Is.EqualTo(player3.ScoredGoals));
        }
        [Test]
        public void PlayerScoreShould_PlayerScore()
        {
            FootballPlayer player = new FootballPlayer("Pesho", 1, "Goalkeeper");
            FootballPlayer player2 = new FootballPlayer("Pesho2", 2, "Midfielder");
            FootballPlayer player3 = new FootballPlayer("Pesho3", 3, "Forward");
            FootballPlayer player4 = new FootballPlayer("Pesho4", 4, "Goalkeeper");
            FootballPlayer player5 = new FootballPlayer("Pesho5", 5, "Midfielder");
            this.team.AddNewPlayer(player);
            this.team.AddNewPlayer(player2);
            this.team.AddNewPlayer(player3);
            this.team.AddNewPlayer(player4);
            this.team.AddNewPlayer(player5);
            team.PlayerScore(3);
            team.PlayerScore(3);
            team.PlayerScore(3);
            int expectedGoals = 4;
            Assert.That($"{player3.Name} scored and now has {4} for this season!",
                Is.EqualTo(team.PlayerScore(3)));
        }
    }
}