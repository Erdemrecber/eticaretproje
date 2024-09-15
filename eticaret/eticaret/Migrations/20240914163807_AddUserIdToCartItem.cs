using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eticaret.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToCartItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // CartItem tablosuna UserId sütunu ekle
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CartItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Eğer geri almak istersen UserId sütununu kaldır
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CartItems");
        }
    }

}
