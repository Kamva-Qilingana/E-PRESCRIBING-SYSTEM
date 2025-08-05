using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_PRESCRIBING_SYSTEM.Data.Migrations
{
    public partial class CreateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.CreateTable(
                name: "conditions",
                columns: table => new
                {
                    ConditionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Diagnosis = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conditions", x => x.ConditionID);
                });

            migrationBuilder.CreateTable(
                name: "Dosages",
                columns: table => new
                {
                    DosageFormID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DosageFormName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dosages", x => x.DosageFormID);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    ProvinceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProvinceName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.ProvinceID);
                });

            migrationBuilder.CreateTable(
                name: "threatres",
                columns: table => new
                {
                    TheatreID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TheatreName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_threatres", x => x.TheatreID);
                });

            migrationBuilder.CreateTable(
                name: "TreatmentCodes",
                columns: table => new
                {
                    TreatmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentCodes", x => x.TreatmentID);
                });

            migrationBuilder.CreateTable(
                name: "ChricMedication",
                columns: table => new
                {
                    ChronicMedicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChronicMedicationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Schedule = table.Column<int>(type: "int", nullable: false),
                    DosageFormID = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "GeneralMedication",
                columns: table => new
                {
                    GeneralMedicationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DosageFormID = table.Column<int>(type: "int", nullable: false),
                    Schedules = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReorderLevels = table.Column<int>(type: "int", nullable: false),
                    StockOnHand = table.Column<int>(type: "int", nullable: false),
                    strengths = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralMedication", x => x.GeneralMedicationID);
                    table.ForeignKey(
                        name: "FK_GeneralMedication_Dosages_DosageFormID",
                        column: x => x.DosageFormID,
                        principalTable: "Dosages",
                        principalColumn: "DosageFormID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PharmacyMedication",
                columns: table => new
                {
                    PharmacyMedicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Schedule = table.Column<int>(type: "int", nullable: false),
                    ReorderLevel = table.Column<int>(type: "int", nullable: false),
                    StockOnHand = table.Column<int>(type: "int", nullable: false),
                    DosageFormID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmacyMedication", x => x.PharmacyMedicationId);
                    table.ForeignKey(
                        name: "FK_PharmacyMedication_Dosages_DosageFormID",
                        column: x => x.DosageFormID,
                        principalTable: "Dosages",
                        principalColumn: "DosageFormID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Citys",
                columns: table => new
                {
                    CityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProvinceID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citys", x => x.CityID);
                    table.ForeignKey(
                        name: "FK_Citys_Provinces_ProvinceID",
                        column: x => x.ProvinceID,
                        principalTable: "Provinces",
                        principalColumn: "ProvinceID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActiveIngredients",
                columns: table => new
                {
                    ActiveIngredientsID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActiveIngredientsName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PharmacyMedicationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveIngredients", x => x.ActiveIngredientsID);
                    table.ForeignKey(
                        name: "FK_ActiveIngredients_PharmacyMedication_PharmacyMedicationId",
                        column: x => x.PharmacyMedicationId,
                        principalTable: "PharmacyMedication",
                        principalColumn: "PharmacyMedicationId");
                });

            migrationBuilder.CreateTable(
                name: "OrderStock",
                columns: table => new
                {
                    StockID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderQuantity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PharmacyMedicationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStock", x => x.StockID);
                    table.ForeignKey(
                        name: "FK_OrderStock_PharmacyMedication_PharmacyMedicationId",
                        column: x => x.PharmacyMedicationId,
                        principalTable: "PharmacyMedication",
                        principalColumn: "PharmacyMedicationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockOrders",
                columns: table => new
                {
                    StockOrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderQuantity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PharmacyMedicationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockOrders", x => x.StockOrderID);
                    table.ForeignKey(
                        name: "FK_StockOrders_PharmacyMedication_PharmacyMedicationId",
                        column: x => x.PharmacyMedicationId,
                        principalTable: "PharmacyMedication",
                        principalColumn: "PharmacyMedicationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Suburbs",
                columns: table => new
                {
                    SuburbID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SuburbName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suburbs", x => x.SuburbID);
                    table.ForeignKey(
                        name: "FK_Suburbs_Citys_CityID",
                        column: x => x.CityID,
                        principalTable: "Citys",
                        principalColumn: "CityID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActiveMedicationIngredients",
                columns: table => new
                {
                    ActiveMedicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PharmacyMedicationId = table.Column<int>(type: "int", nullable: false),
                    ActiveIngredientsID = table.Column<int>(type: "int", nullable: false),
                    ChronicMedicationId = table.Column<int>(type: "int", nullable: false),
                    Strength = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveMedicationIngredients", x => x.ActiveMedicationId);
                    table.ForeignKey(
                        name: "FK_ActiveMedicationIngredients_ActiveIngredients_ActiveIngredientsID",
                        column: x => x.ActiveIngredientsID,
                        principalTable: "ActiveIngredients",
                        principalColumn: "ActiveIngredientsID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActiveMedicationIngredients_ChricMedication_ChronicMedicationId",
                        column: x => x.ChronicMedicationId,
                        principalTable: "ChricMedication",
                        principalColumn: "ChronicMedicationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActiveMedicationIngredients_PharmacyMedication_PharmacyMedicationId",
                        column: x => x.PharmacyMedicationId,
                        principalTable: "PharmacyMedication",
                        principalColumn: "PharmacyMedicationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContraIndications",
                columns: table => new
                {
                    ContraIndicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActiveIngredientsID = table.Column<int>(type: "int", nullable: false),
                    ConditionID = table.Column<int>(type: "int", nullable: false),
                    ChronicMedicationId = table.Column<int>(type: "int", nullable: true),
                    PharmacyMedicationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContraIndications", x => x.ContraIndicationId);
                    table.ForeignKey(
                        name: "FK_ContraIndications_ActiveIngredients_ActiveIngredientsID",
                        column: x => x.ActiveIngredientsID,
                        principalTable: "ActiveIngredients",
                        principalColumn: "ActiveIngredientsID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContraIndications_ChricMedication_ChronicMedicationId",
                        column: x => x.ChronicMedicationId,
                        principalTable: "ChricMedication",
                        principalColumn: "ChronicMedicationId");
                    table.ForeignKey(
                        name: "FK_ContraIndications_conditions_ConditionID",
                        column: x => x.ConditionID,
                        principalTable: "conditions",
                        principalColumn: "ConditionID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContraIndications_PharmacyMedication_PharmacyMedicationId",
                        column: x => x.PharmacyMedicationId,
                        principalTable: "PharmacyMedication",
                        principalColumn: "PharmacyMedicationId");
                });

            migrationBuilder.CreateTable(
                name: "Medications",
                columns: table => new
                {
                    MedicationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActiveIngredientsID = table.Column<int>(type: "int", nullable: false),
                    DosageFormID = table.Column<int>(type: "int", nullable: false),
                    IngredientStrength = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReorderLevel = table.Column<int>(type: "int", nullable: false),
                    StockOnHand = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medications", x => x.MedicationID);
                    table.ForeignKey(
                        name: "FK_Medications_ActiveIngredients_ActiveIngredientsID",
                        column: x => x.ActiveIngredientsID,
                        principalTable: "ActiveIngredients",
                        principalColumn: "ActiveIngredientsID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Medications_Dosages_DosageFormID",
                        column: x => x.DosageFormID,
                        principalTable: "Dosages",
                        principalColumn: "DosageFormID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockReceived",
                columns: table => new
                {
                    StockReceivedID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceivedQuantity = table.Column<int>(type: "int", nullable: false),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StockOrderID = table.Column<int>(type: "int", nullable: false),
                    PharmacyMedicationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockReceived", x => x.StockReceivedID);
                    table.ForeignKey(
                        name: "FK_StockReceived_PharmacyMedication_PharmacyMedicationId",
                        column: x => x.PharmacyMedicationId,
                        principalTable: "PharmacyMedication",
                        principalColumn: "PharmacyMedicationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockReceived_StockOrders_StockOrderID",
                        column: x => x.StockOrderID,
                        principalTable: "StockOrders",
                        principalColumn: "StockOrderID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Admissions",
                columns: table => new
                {
                    AdmissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WardID = table.Column<int>(type: "int", nullable: false),
                    PatientProfileId = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    height = table.Column<double>(type: "float", nullable: false),
                    weight = table.Column<double>(type: "float", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admissions", x => x.AdmissionId);
                });

            migrationBuilder.CreateTable(
                name: "PatientProfiles",
                columns: table => new
                {
                    PatientProfileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientIDno = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    PatientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PSurname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PContactNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdmissionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientProfiles", x => x.PatientProfileId);
                    table.ForeignKey(
                        name: "FK_PatientProfiles_Admissions_AdmissionId",
                        column: x => x.AdmissionId,
                        principalTable: "Admissions",
                        principalColumn: "AdmissionId");
                });

            migrationBuilder.CreateTable(
                name: "Wards",
                columns: table => new
                {
                    WardID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WardName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bed = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdmissionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wards", x => x.WardID);
                    table.ForeignKey(
                        name: "FK_Wards_Admissions_AdmissionId",
                        column: x => x.AdmissionId,
                        principalTable: "Admissions",
                        principalColumn: "AdmissionId");
                });

            migrationBuilder.CreateTable(
                name: "NewPrescriptions",
                columns: table => new
                {
                    PrescriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientProfileId = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAgent = table.Column<bool>(type: "bit", nullable: false),
                    Instruction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RejectionNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChronicMedicationId = table.Column<int>(type: "int", nullable: true),
                    PharmacyMedicationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewPrescriptions", x => x.PrescriptionId);
                    table.ForeignKey(
                        name: "FK_NewPrescriptions_ChricMedication_ChronicMedicationId",
                        column: x => x.ChronicMedicationId,
                        principalTable: "ChricMedication",
                        principalColumn: "ChronicMedicationId");
                    table.ForeignKey(
                        name: "FK_NewPrescriptions_PatientProfiles_PatientProfileId",
                        column: x => x.PatientProfileId,
                        principalTable: "PatientProfiles",
                        principalColumn: "PatientProfileId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NewPrescriptions_PharmacyMedication_PharmacyMedicationId",
                        column: x => x.PharmacyMedicationId,
                        principalTable: "PharmacyMedication",
                        principalColumn: "PharmacyMedicationId");
                });

            migrationBuilder.CreateTable(
                name: "patientAllergies",
                columns: table => new
                {
                    PatientAllergyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientProfileId = table.Column<int>(type: "int", nullable: false),
                    ActiveIngredientsID = table.Column<int>(type: "int", nullable: false),
                    AllergyDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patientAllergies", x => x.PatientAllergyId);
                    table.ForeignKey(
                        name: "FK_patientAllergies_ActiveIngredients_ActiveIngredientsID",
                        column: x => x.ActiveIngredientsID,
                        principalTable: "ActiveIngredients",
                        principalColumn: "ActiveIngredientsID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_patientAllergies_PatientProfiles_PatientProfileId",
                        column: x => x.PatientProfileId,
                        principalTable: "PatientProfiles",
                        principalColumn: "PatientProfileId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientConditions",
                columns: table => new
                {
                    PatientConditionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientProfileId = table.Column<int>(type: "int", nullable: false),
                    ConditionID = table.Column<int>(type: "int", nullable: false),
                    DiagnosisDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientConditions", x => x.PatientConditionId);
                    table.ForeignKey(
                        name: "FK_PatientConditions_conditions_ConditionID",
                        column: x => x.ConditionID,
                        principalTable: "conditions",
                        principalColumn: "ConditionID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientConditions_PatientProfiles_PatientProfileId",
                        column: x => x.PatientProfileId,
                        principalTable: "PatientProfiles",
                        principalColumn: "PatientProfileId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientMedications",
                columns: table => new
                {
                    PatientMedicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientProfileId = table.Column<int>(type: "int", nullable: false),
                    PharmacyMedicationId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChronicMedicationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientMedications", x => x.PatientMedicationId);
                    table.ForeignKey(
                        name: "FK_PatientMedications_ChricMedication_ChronicMedicationId",
                        column: x => x.ChronicMedicationId,
                        principalTable: "ChricMedication",
                        principalColumn: "ChronicMedicationId");
                    table.ForeignKey(
                        name: "FK_PatientMedications_PatientProfiles_PatientProfileId",
                        column: x => x.PatientProfileId,
                        principalTable: "PatientProfiles",
                        principalColumn: "PatientProfileId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientMedications_PharmacyMedication_PharmacyMedicationId",
                        column: x => x.PharmacyMedicationId,
                        principalTable: "PharmacyMedication",
                        principalColumn: "PharmacyMedicationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Prescriptions",
                columns: table => new
                {
                    PrescriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientProfileId = table.Column<int>(type: "int", nullable: false),
                    PharmacyMedicationId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    PrescriptionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChronicMedicationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescriptions", x => x.PrescriptionId);
                    table.ForeignKey(
                        name: "FK_Prescriptions_ChricMedication_ChronicMedicationId",
                        column: x => x.ChronicMedicationId,
                        principalTable: "ChricMedication",
                        principalColumn: "ChronicMedicationId");
                    table.ForeignKey(
                        name: "FK_Prescriptions_PatientProfiles_PatientProfileId",
                        column: x => x.PatientProfileId,
                        principalTable: "PatientProfiles",
                        principalColumn: "PatientProfileId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prescriptions_PharmacyMedication_PharmacyMedicationId",
                        column: x => x.PharmacyMedicationId,
                        principalTable: "PharmacyMedication",
                        principalColumn: "PharmacyMedicationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Surgery",
                columns: table => new
                {
                    SurgeryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientProfileId = table.Column<int>(type: "int", nullable: false),
                    TheatreID = table.Column<int>(type: "int", nullable: false),
                    SurgeryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeSlot = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surgery", x => x.SurgeryID);
                    table.ForeignKey(
                        name: "FK_Surgery_PatientProfiles_PatientProfileId",
                        column: x => x.PatientProfileId,
                        principalTable: "PatientProfiles",
                        principalColumn: "PatientProfileId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Surgery_threatres_TheatreID",
                        column: x => x.TheatreID,
                        principalTable: "threatres",
                        principalColumn: "TheatreID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VitalSigns",
                columns: table => new
                {
                    VitalSignId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientProfileId = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BodyTemperature = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HeartRate = table.Column<int>(type: "int", nullable: false),
                    BloodOxygenSaturation = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VitalSigns", x => x.VitalSignId);
                    table.ForeignKey(
                        name: "FK_VitalSigns_PatientProfiles_PatientProfileId",
                        column: x => x.PatientProfileId,
                        principalTable: "PatientProfiles",
                        principalColumn: "PatientProfileId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PrescriptionMedications",
                columns: table => new
                {
                    PrescriptionMedicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrescriptionId = table.Column<int>(type: "int", nullable: false),
                    PharmacyMedicationId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Instruction = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionMedications", x => x.PrescriptionMedicationId);
                    table.ForeignKey(
                        name: "FK_PrescriptionMedications_NewPrescriptions_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "NewPrescriptions",
                        principalColumn: "PrescriptionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrescriptionMedications_PharmacyMedication_PharmacyMedicationId",
                        column: x => x.PharmacyMedicationId,
                        principalTable: "PharmacyMedication",
                        principalColumn: "PharmacyMedicationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SurgeryTreatments",
                columns: table => new
                {
                    SurgeryTreatmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SurgeryID = table.Column<int>(type: "int", nullable: false),
                    TreatmentID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurgeryTreatments", x => x.SurgeryTreatmentId);
                    table.ForeignKey(
                        name: "FK_SurgeryTreatments_Surgery_SurgeryID",
                        column: x => x.SurgeryID,
                        principalTable: "Surgery",
                        principalColumn: "SurgeryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SurgeryTreatments_TreatmentCodes_TreatmentID",
                        column: x => x.TreatmentID,
                        principalTable: "TreatmentCodes",
                        principalColumn: "TreatmentID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActiveIngredients_PharmacyMedicationId",
                table: "ActiveIngredients",
                column: "PharmacyMedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveMedicationIngredients_ActiveIngredientsID",
                table: "ActiveMedicationIngredients",
                column: "ActiveIngredientsID");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveMedicationIngredients_ChronicMedicationId",
                table: "ActiveMedicationIngredients",
                column: "ChronicMedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveMedicationIngredients_PharmacyMedicationId",
                table: "ActiveMedicationIngredients",
                column: "PharmacyMedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_PatientProfileId",
                table: "Admissions",
                column: "PatientProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_WardID",
                table: "Admissions",
                column: "WardID");

            migrationBuilder.CreateIndex(
                name: "IX_ChricMedication_DosageFormID",
                table: "ChricMedication",
                column: "DosageFormID");

            migrationBuilder.CreateIndex(
                name: "IX_Citys_ProvinceID",
                table: "Citys",
                column: "ProvinceID");

            migrationBuilder.CreateIndex(
                name: "IX_ContraIndications_ActiveIngredientsID",
                table: "ContraIndications",
                column: "ActiveIngredientsID");

            migrationBuilder.CreateIndex(
                name: "IX_ContraIndications_ChronicMedicationId",
                table: "ContraIndications",
                column: "ChronicMedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ContraIndications_ConditionID",
                table: "ContraIndications",
                column: "ConditionID");

            migrationBuilder.CreateIndex(
                name: "IX_ContraIndications_PharmacyMedicationId",
                table: "ContraIndications",
                column: "PharmacyMedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralMedication_DosageFormID",
                table: "GeneralMedication",
                column: "DosageFormID");

            migrationBuilder.CreateIndex(
                name: "IX_Medications_ActiveIngredientsID",
                table: "Medications",
                column: "ActiveIngredientsID");

            migrationBuilder.CreateIndex(
                name: "IX_Medications_DosageFormID",
                table: "Medications",
                column: "DosageFormID");

            migrationBuilder.CreateIndex(
                name: "IX_NewPrescriptions_ChronicMedicationId",
                table: "NewPrescriptions",
                column: "ChronicMedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_NewPrescriptions_PatientProfileId",
                table: "NewPrescriptions",
                column: "PatientProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_NewPrescriptions_PharmacyMedicationId",
                table: "NewPrescriptions",
                column: "PharmacyMedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStock_PharmacyMedicationId",
                table: "OrderStock",
                column: "PharmacyMedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_patientAllergies_ActiveIngredientsID",
                table: "patientAllergies",
                column: "ActiveIngredientsID");

            migrationBuilder.CreateIndex(
                name: "IX_patientAllergies_PatientProfileId",
                table: "patientAllergies",
                column: "PatientProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientConditions_ConditionID",
                table: "PatientConditions",
                column: "ConditionID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientConditions_PatientProfileId",
                table: "PatientConditions",
                column: "PatientProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientMedications_ChronicMedicationId",
                table: "PatientMedications",
                column: "ChronicMedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientMedications_PatientProfileId",
                table: "PatientMedications",
                column: "PatientProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientMedications_PharmacyMedicationId",
                table: "PatientMedications",
                column: "PharmacyMedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientProfiles_AdmissionId",
                table: "PatientProfiles",
                column: "AdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyMedication_DosageFormID",
                table: "PharmacyMedication",
                column: "DosageFormID");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionMedications_PharmacyMedicationId",
                table: "PrescriptionMedications",
                column: "PharmacyMedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionMedications_PrescriptionId",
                table: "PrescriptionMedications",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_ChronicMedicationId",
                table: "Prescriptions",
                column: "ChronicMedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_PatientProfileId",
                table: "Prescriptions",
                column: "PatientProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_PharmacyMedicationId",
                table: "Prescriptions",
                column: "PharmacyMedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_StockOrders_PharmacyMedicationId",
                table: "StockOrders",
                column: "PharmacyMedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_StockReceived_PharmacyMedicationId",
                table: "StockReceived",
                column: "PharmacyMedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_StockReceived_StockOrderID",
                table: "StockReceived",
                column: "StockOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_Suburbs_CityID",
                table: "Suburbs",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_Surgery_PatientProfileId",
                table: "Surgery",
                column: "PatientProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Surgery_TheatreID",
                table: "Surgery",
                column: "TheatreID");

            migrationBuilder.CreateIndex(
                name: "IX_SurgeryTreatments_SurgeryID",
                table: "SurgeryTreatments",
                column: "SurgeryID");

            migrationBuilder.CreateIndex(
                name: "IX_SurgeryTreatments_TreatmentID",
                table: "SurgeryTreatments",
                column: "TreatmentID");

            migrationBuilder.CreateIndex(
                name: "IX_VitalSigns_PatientProfileId",
                table: "VitalSigns",
                column: "PatientProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Wards_AdmissionId",
                table: "Wards",
                column: "AdmissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admissions_PatientProfiles_PatientProfileId",
                table: "Admissions",
                column: "PatientProfileId",
                principalTable: "PatientProfiles",
                principalColumn: "PatientProfileId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Admissions_Wards_WardID",
                table: "Admissions",
                column: "WardID",
                principalTable: "Wards",
                principalColumn: "WardID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admissions_PatientProfiles_PatientProfileId",
                table: "Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Admissions_Wards_WardID",
                table: "Admissions");

            migrationBuilder.DropTable(
                name: "ActiveMedicationIngredients");

            migrationBuilder.DropTable(
                name: "ContraIndications");

            migrationBuilder.DropTable(
                name: "GeneralMedication");

            migrationBuilder.DropTable(
                name: "Medications");

            migrationBuilder.DropTable(
                name: "OrderStock");

            migrationBuilder.DropTable(
                name: "patientAllergies");

            migrationBuilder.DropTable(
                name: "PatientConditions");

            migrationBuilder.DropTable(
                name: "PatientMedications");

            migrationBuilder.DropTable(
                name: "PrescriptionMedications");

            migrationBuilder.DropTable(
                name: "Prescriptions");

            migrationBuilder.DropTable(
                name: "StockReceived");

            migrationBuilder.DropTable(
                name: "Suburbs");

            migrationBuilder.DropTable(
                name: "SurgeryTreatments");

            migrationBuilder.DropTable(
                name: "VitalSigns");

            migrationBuilder.DropTable(
                name: "ActiveIngredients");

            migrationBuilder.DropTable(
                name: "conditions");

            migrationBuilder.DropTable(
                name: "NewPrescriptions");

            migrationBuilder.DropTable(
                name: "StockOrders");

            migrationBuilder.DropTable(
                name: "Citys");

            migrationBuilder.DropTable(
                name: "Surgery");

            migrationBuilder.DropTable(
                name: "TreatmentCodes");

            migrationBuilder.DropTable(
                name: "ChricMedication");

            migrationBuilder.DropTable(
                name: "PharmacyMedication");

            migrationBuilder.DropTable(
                name: "Provinces");

            migrationBuilder.DropTable(
                name: "threatres");

            migrationBuilder.DropTable(
                name: "Dosages");

            migrationBuilder.DropTable(
                name: "PatientProfiles");

            migrationBuilder.DropTable(
                name: "Wards");

            migrationBuilder.DropTable(
                name: "Admissions");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
