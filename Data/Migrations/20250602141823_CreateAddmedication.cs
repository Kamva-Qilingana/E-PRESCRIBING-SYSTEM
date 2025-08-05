using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_PRESCRIBING_SYSTEM.Data.Migrations
{
    public partial class CreateAddmedication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveMedicationIngredients_ChricMedication_ChronicMedicationId",
                table: "ActiveMedicationIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_ContraIndications_ChricMedication_ChronicMedicationId",
                table: "ContraIndications");

            migrationBuilder.DropForeignKey(
                name: "FK_NewPrescriptions_ChricMedication_ChronicMedicationId",
                table: "NewPrescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientMedications_ChricMedication_ChronicMedicationId",
                table: "PatientMedications");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_ChricMedication_ChronicMedicationId",
                table: "Prescriptions");

            migrationBuilder.DropTable(
                name: "ChricMedication");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_ChronicMedicationId",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_PatientMedications_ChronicMedicationId",
                table: "PatientMedications");

            migrationBuilder.DropIndex(
                name: "IX_NewPrescriptions_ChronicMedicationId",
                table: "NewPrescriptions");

            migrationBuilder.DropIndex(
                name: "IX_ContraIndications_ChronicMedicationId",
                table: "ContraIndications");

            migrationBuilder.DropIndex(
                name: "IX_ActiveMedicationIngredients_ChronicMedicationId",
                table: "ActiveMedicationIngredients");

            migrationBuilder.DropColumn(
                name: "ChronicMedicationId",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "ChronicMedicationId",
                table: "PatientMedications");

            migrationBuilder.DropColumn(
                name: "ChronicMedicationId",
                table: "NewPrescriptions");

            migrationBuilder.DropColumn(
                name: "ChronicMedicationId",
                table: "ContraIndications");

            migrationBuilder.DropColumn(
                name: "ChronicMedicationId",
                table: "ActiveMedicationIngredients");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChronicMedicationId",
                table: "Prescriptions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChronicMedicationId",
                table: "PatientMedications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChronicMedicationId",
                table: "NewPrescriptions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChronicMedicationId",
                table: "ContraIndications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChronicMedicationId",
                table: "ActiveMedicationIngredients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ChricMedication",
                columns: table => new
                {
                    ChronicMedicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DosageFormID = table.Column<int>(type: "int", nullable: false),
                    ChronicMedicationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Schedule = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChricMedication", x => x.ChronicMedicationId);
                    table.ForeignKey(
                        name: "FK_ChricMedication_Dosages_DosageFormID",
                        column: x => x.DosageFormID,
                        principalTable: "Dosages",
                        principalColumn: "DosageFormID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_ChronicMedicationId",
                table: "Prescriptions",
                column: "ChronicMedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientMedications_ChronicMedicationId",
                table: "PatientMedications",
                column: "ChronicMedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_NewPrescriptions_ChronicMedicationId",
                table: "NewPrescriptions",
                column: "ChronicMedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ContraIndications_ChronicMedicationId",
                table: "ContraIndications",
                column: "ChronicMedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveMedicationIngredients_ChronicMedicationId",
                table: "ActiveMedicationIngredients",
                column: "ChronicMedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChricMedication_DosageFormID",
                table: "ChricMedication",
                column: "DosageFormID");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveMedicationIngredients_ChricMedication_ChronicMedicationId",
                table: "ActiveMedicationIngredients",
                column: "ChronicMedicationId",
                principalTable: "ChricMedication",
                principalColumn: "ChronicMedicationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContraIndications_ChricMedication_ChronicMedicationId",
                table: "ContraIndications",
                column: "ChronicMedicationId",
                principalTable: "ChricMedication",
                principalColumn: "ChronicMedicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_NewPrescriptions_ChricMedication_ChronicMedicationId",
                table: "NewPrescriptions",
                column: "ChronicMedicationId",
                principalTable: "ChricMedication",
                principalColumn: "ChronicMedicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientMedications_ChricMedication_ChronicMedicationId",
                table: "PatientMedications",
                column: "ChronicMedicationId",
                principalTable: "ChricMedication",
                principalColumn: "ChronicMedicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_ChricMedication_ChronicMedicationId",
                table: "Prescriptions",
                column: "ChronicMedicationId",
                principalTable: "ChricMedication",
                principalColumn: "ChronicMedicationId");
        }
    }
}
