using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_PRESCRIBING_SYSTEM.Data.Migrations
{
    public partial class createTableScript : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewPrescriptions_PharmacyMedication_PharmacyMedicationId",
                table: "NewPrescriptions");

            migrationBuilder.DropIndex(
                name: "IX_NewPrescriptions_PharmacyMedicationId",
                table: "NewPrescriptions");

            migrationBuilder.DropColumn(
                name: "PharmacyMedicationId",
                table: "NewPrescriptions");

            migrationBuilder.CreateTable(
                name: "NewPrescriptionPharmacyMedication",
                columns: table => new
                {
                    NewPrescriptionsPrescriptionId = table.Column<int>(type: "int", nullable: false),
                    PharmacyMedicationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewPrescriptionPharmacyMedication", x => new { x.NewPrescriptionsPrescriptionId, x.PharmacyMedicationId });
                    table.ForeignKey(
                        name: "FK_NewPrescriptionPharmacyMedication_NewPrescriptions_NewPrescriptionsPrescriptionId",
                        column: x => x.NewPrescriptionsPrescriptionId,
                        principalTable: "NewPrescriptions",
                        principalColumn: "PrescriptionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NewPrescriptionPharmacyMedication_PharmacyMedication_PharmacyMedicationId",
                        column: x => x.PharmacyMedicationId,
                        principalTable: "PharmacyMedication",
                        principalColumn: "PharmacyMedicationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewPrescriptionPharmacyMedication_PharmacyMedicationId",
                table: "NewPrescriptionPharmacyMedication",
                column: "PharmacyMedicationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewPrescriptionPharmacyMedication");

            migrationBuilder.AddColumn<int>(
                name: "PharmacyMedicationId",
                table: "NewPrescriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_NewPrescriptions_PharmacyMedicationId",
                table: "NewPrescriptions",
                column: "PharmacyMedicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_NewPrescriptions_PharmacyMedication_PharmacyMedicationId",
                table: "NewPrescriptions",
                column: "PharmacyMedicationId",
                principalTable: "PharmacyMedication",
                principalColumn: "PharmacyMedicationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
