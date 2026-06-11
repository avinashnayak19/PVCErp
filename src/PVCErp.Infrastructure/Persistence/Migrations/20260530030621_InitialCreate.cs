using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PVCErp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BatchFormulas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormulaCode = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchFormulas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GstNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Machines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MachineNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Machines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RawMaterials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReorderLevel = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RawMaterials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScrapRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Stage = table.Column<int>(type: "int", nullable: false),
                    ScrapType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeneratedKg = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    ReusedKg = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    SourceReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecordDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScrapRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScrewBarrels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BarrelNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetKgPerHour = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScrewBarrels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    GstNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductionBatches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchNumber = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    BatchFormulaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Shift = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OperatorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupervisorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupervisorApproved = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionBatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionBatches_BatchFormulas_BatchFormulaId",
                        column: x => x.BatchFormulaId,
                        principalTable: "BatchFormulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DispatchChallans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChallanNumber = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DispatchDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    VehicleNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispatchChallans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DispatchChallans_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BatchFormulaItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchFormulaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RawMaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StandardQuantityKg = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchFormulaItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BatchFormulaItems_BatchFormulas_BatchFormulaId",
                        column: x => x.BatchFormulaId,
                        principalTable: "BatchFormulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BatchFormulaItems_RawMaterials_RawMaterialId",
                        column: x => x.RawMaterialId,
                        principalTable: "RawMaterials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoodsReceiptNotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GrnNumber = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiptDate = table.Column<DateOnly>(type: "date", nullable: false),
                    QualityStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsReceiptNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoodsReceiptNotes_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryBatches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RawMaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BatchNumber = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    ReceivedQuantityKg = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    AvailableQuantityKg = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    QualityStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryBatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryBatches_RawMaterials_RawMaterialId",
                        column: x => x.RawMaterialId,
                        principalTable: "RawMaterials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryBatches_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductionConsumptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductionBatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RawMaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StandardQuantityKg = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    ActualQuantityKg = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionConsumptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionConsumptions_ProductionBatches_ProductionBatchId",
                        column: x => x.ProductionBatchId,
                        principalTable: "ProductionBatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductionConsumptions_RawMaterials_RawMaterialId",
                        column: x => x.RawMaterialId,
                        principalTable: "RawMaterials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductionOutputs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductionBatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MachineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScrewBarrelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Stage = table.Column<int>(type: "int", nullable: false),
                    PipeDimension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovedQuantityKg = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    RejectedQuantityKg = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    HoursRun = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionOutputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionOutputs_Machines_MachineId",
                        column: x => x.MachineId,
                        principalTable: "Machines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductionOutputs_ProductionBatches_ProductionBatchId",
                        column: x => x.ProductionBatchId,
                        principalTable: "ProductionBatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductionOutputs_ScrewBarrels_ScrewBarrelId",
                        column: x => x.ScrewBarrelId,
                        principalTable: "ScrewBarrels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DispatchItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DispatchChallanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PipeDimension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuantityKg = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    QuantityPieces = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispatchItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DispatchItems_DispatchChallans_DispatchChallanId",
                        column: x => x.DispatchChallanId,
                        principalTable: "DispatchChallans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    DispatchChallanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceDate = table.Column<DateOnly>(type: "date", nullable: false),
                    TaxableAmount = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    GstAmount = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_DispatchChallans_DispatchChallanId",
                        column: x => x.DispatchChallanId,
                        principalTable: "DispatchChallans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoodsReceiptItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GoodsReceiptNoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RawMaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuantityKg = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsReceiptItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoodsReceiptItems_GoodsReceiptNotes_GoodsReceiptNoteId",
                        column: x => x.GoodsReceiptNoteId,
                        principalTable: "GoodsReceiptNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoodsReceiptItems_RawMaterials_RawMaterialId",
                        column: x => x.RawMaterialId,
                        principalTable: "RawMaterials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BatchFormulaItems_BatchFormulaId",
                table: "BatchFormulaItems",
                column: "BatchFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchFormulaItems_RawMaterialId",
                table: "BatchFormulaItems",
                column: "RawMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchFormulas_FormulaCode",
                table: "BatchFormulas",
                column: "FormulaCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DispatchChallans_CustomerId",
                table: "DispatchChallans",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_DispatchItems_DispatchChallanId",
                table: "DispatchItems",
                column: "DispatchChallanId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptItems_GoodsReceiptNoteId",
                table: "GoodsReceiptItems",
                column: "GoodsReceiptNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptItems_RawMaterialId",
                table: "GoodsReceiptItems",
                column: "RawMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptNotes_GrnNumber",
                table: "GoodsReceiptNotes",
                column: "GrnNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptNotes_SupplierId",
                table: "GoodsReceiptNotes",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryBatches_RawMaterialId",
                table: "InventoryBatches",
                column: "RawMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryBatches_SupplierId",
                table: "InventoryBatches",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_DispatchChallanId",
                table: "Invoices",
                column: "DispatchChallanId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_InvoiceNumber",
                table: "Invoices",
                column: "InvoiceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionBatches_BatchFormulaId",
                table: "ProductionBatches",
                column: "BatchFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionConsumptions_ProductionBatchId",
                table: "ProductionConsumptions",
                column: "ProductionBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionConsumptions_RawMaterialId",
                table: "ProductionConsumptions",
                column: "RawMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOutputs_MachineId",
                table: "ProductionOutputs",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOutputs_ProductionBatchId",
                table: "ProductionOutputs",
                column: "ProductionBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOutputs_ScrewBarrelId",
                table: "ProductionOutputs",
                column: "ScrewBarrelId");

            migrationBuilder.CreateIndex(
                name: "IX_RawMaterials_Name",
                table: "RawMaterials",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BatchFormulaItems");

            migrationBuilder.DropTable(
                name: "DispatchItems");

            migrationBuilder.DropTable(
                name: "GoodsReceiptItems");

            migrationBuilder.DropTable(
                name: "InventoryBatches");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "ProductionConsumptions");

            migrationBuilder.DropTable(
                name: "ProductionOutputs");

            migrationBuilder.DropTable(
                name: "ScrapRecords");

            migrationBuilder.DropTable(
                name: "GoodsReceiptNotes");

            migrationBuilder.DropTable(
                name: "DispatchChallans");

            migrationBuilder.DropTable(
                name: "RawMaterials");

            migrationBuilder.DropTable(
                name: "Machines");

            migrationBuilder.DropTable(
                name: "ProductionBatches");

            migrationBuilder.DropTable(
                name: "ScrewBarrels");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "BatchFormulas");
        }
    }
}
