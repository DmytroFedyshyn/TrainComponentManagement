using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainComponentManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Components",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UniqueNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CanAssignQuantity = table.Column<bool>(type: "bit", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.Id);
                    table.CheckConstraint("CK_Component_Quantity_NonNegative", "Quantity IS NULL OR Quantity >= 0");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Components_CanAssignQuantity_Quantity",
                table: "Components",
                columns: new[] { "CanAssignQuantity", "Quantity" });

            migrationBuilder.CreateIndex(
                name: "IX_Components_Name",
                table: "Components",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Components_UniqueNumber",
                table: "Components",
                column: "UniqueNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Components");
        }
    }
}
