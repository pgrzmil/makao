namespace Makao.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArchiveGames",
                c => new
                    {
                        ArchiveGameID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Points = c.Int(nullable: false),
                        Player_PlayerID = c.Int(),
                        WinnerID_PlayerID = c.Int(),
                    })
                .PrimaryKey(t => t.ArchiveGameID)
                .ForeignKey("dbo.Players", t => t.Player_PlayerID)
                .ForeignKey("dbo.Players", t => t.WinnerID_PlayerID)
                .Index(t => t.Player_PlayerID)
                .Index(t => t.WinnerID_PlayerID);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        PlayerID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SessionId = c.String(),
                        ConnectionId = c.String(),
                        IsReady = c.Boolean(nullable: false),
                        IsTurn = c.Boolean(nullable: false),
                        ArchiveGame_ArchiveGameID = c.Int(),
                        GameRoom_GameRoomId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PlayerID)
                .ForeignKey("dbo.ArchiveGames", t => t.ArchiveGame_ArchiveGameID)
                .ForeignKey("dbo.GameRooms", t => t.GameRoom_GameRoomId)
                .Index(t => t.ArchiveGame_ArchiveGameID)
                .Index(t => t.GameRoom_GameRoomId);
            
            CreateTable(
                "dbo.Cards",
                c => new
                    {
                        CardId = c.String(nullable: false, maxLength: 128),
                        Suit = c.Int(nullable: false),
                        Rank = c.Int(nullable: false),
                        Player_PlayerID = c.Int(),
                        GameRoom_GameRoomId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CardId)
                .ForeignKey("dbo.Players", t => t.Player_PlayerID)
                .ForeignKey("dbo.GameRooms", t => t.GameRoom_GameRoomId)
                .Index(t => t.Player_PlayerID)
                .Index(t => t.GameRoom_GameRoomId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false),
                        Username = c.String(),
                        Password = c.String(),
                        Email = c.String(),
                        RankingPoints = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.Players", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.GameRooms",
                c => new
                    {
                        GameRoomId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        NumberOfPlayers = c.Int(nullable: false),
                        MoveTime = c.Int(nullable: false),
                        CurrentPlayerIndex = c.Int(nullable: false),
                        IsRunning = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.GameRoomId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cards", "GameRoom_GameRoomId", "dbo.GameRooms");
            DropForeignKey("dbo.Players", "GameRoom_GameRoomId", "dbo.GameRooms");
            DropForeignKey("dbo.ArchiveGames", "WinnerID_PlayerID", "dbo.Players");
            DropForeignKey("dbo.Players", "ArchiveGame_ArchiveGameID", "dbo.ArchiveGames");
            DropForeignKey("dbo.Users", "UserID", "dbo.Players");
            DropForeignKey("dbo.Cards", "Player_PlayerID", "dbo.Players");
            DropForeignKey("dbo.ArchiveGames", "Player_PlayerID", "dbo.Players");
            DropIndex("dbo.Users", new[] { "UserID" });
            DropIndex("dbo.Cards", new[] { "GameRoom_GameRoomId" });
            DropIndex("dbo.Cards", new[] { "Player_PlayerID" });
            DropIndex("dbo.Players", new[] { "GameRoom_GameRoomId" });
            DropIndex("dbo.Players", new[] { "ArchiveGame_ArchiveGameID" });
            DropIndex("dbo.ArchiveGames", new[] { "WinnerID_PlayerID" });
            DropIndex("dbo.ArchiveGames", new[] { "Player_PlayerID" });
            DropTable("dbo.GameRooms");
            DropTable("dbo.Users");
            DropTable("dbo.Cards");
            DropTable("dbo.Players");
            DropTable("dbo.ArchiveGames");
        }
    }
}
