using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eticaret.Migrations
{
    /// <inheritdoc />
    public partial class MakeUserIdNullableInCartItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // UserId sütunu zaten varsa, nullable olup olmadığını kontrol ediyoruz
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CartItems",
                type: "nvarchar(450)",  // IdentityUser.Id bir string GUID formatında
                nullable: true,  // Şimdilik nullable olarak güncelliyoruz
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Geri almak isterseniz nullable durumunu eski haline getiriyoruz
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CartItems",
                type: "nvarchar(450)",  // IdentityUser.Id string GUID
                nullable: false,  // nullable değil yapıyoruz
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }


}
