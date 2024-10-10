using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eticaret.Migrations
{
    /// <inheritdoc />
    public partial class MakeUserIdNonNullableWithForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // UserId sütununu nullable olmaktan çıkarıyoruz
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CartItems",
                type: "nvarchar(450)",  // GUID olarak saklanır
                nullable: false,  // nullable değil
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            // Foreign Key ekliyoruz, CartItems ile AspNetUsers arasında
            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_AspNetUsers_UserId",
                table: "CartItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);  // Kullanıcı silindiğinde, sepet silinir
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Foreign key'i geri alıyoruz ve nullable olarak geri değiştiriyoruz
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_AspNetUsers_UserId",
                table: "CartItems");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CartItems",
                type: "nvarchar(450)",  // GUID formatında tutuluyor
                nullable: true,  // Geri nullable yapıyoruz
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: false);
        }
    }

}
