using Microsoft.EntityFrameworkCore.Migrations;

namespace FriendOrganizer.DataAccess.Migrations
{
    public partial class AddedFriendPhoneNumbers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FriendPhoneNumbers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(nullable: false),
                    FriendId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendPhoneNumbers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FriendPhoneNumbers_Friends_FriendId",
                        column: x => x.FriendId,
                        principalTable: "Friends",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FriendPhoneNumbers_FriendId",
                table: "FriendPhoneNumbers",
                column: "FriendId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendPhoneNumbers");
        }
    }
}
