using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portal.Database.Context.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar(64)", nullable: false),
                    type = table.Column<string>(type: "varchar(64)", nullable: false),
                    price = table.Column<double>(type: "double precision", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar(64)", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<double>(type: "double precision", nullable: false),
                    rental_time = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    last_name = table.Column<string>(type: "varchar(64)", nullable: false),
                    first_name = table.Column<string>(type: "varchar(64)", nullable: false),
                    middle_name = table.Column<string>(type: "varchar(64)", nullable: false),
                    birthday = table.Column<string>(type: "varchar(64)", nullable: false),
                    gender = table.Column<string>(type: "varchar(64)", nullable: false),
                    email = table.Column<string>(type: "varchar(64)", nullable: false),
                    phone = table.Column<string>(type: "varchar(64)", nullable: false),
                    password = table.Column<string>(type: "varchar(128)", nullable: true),
                    role = table.Column<string>(type: "varchar(64)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Zones",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar(64)", nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    size = table.Column<double>(type: "double precision", nullable: false),
                    limit = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<double>(type: "double precision", nullable: false),
                    rating = table.Column<double>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zones", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DishDbModelPackageDbModel",
                columns: table => new
                {
                    DishesId = table.Column<Guid>(type: "uuid", nullable: false),
                    PackagesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishDbModelPackageDbModel", x => new { x.DishesId, x.PackagesId });
                    table.ForeignKey(
                        name: "FK_DishDbModelPackageDbModel_Menu_DishesId",
                        column: x => x.DishesId,
                        principalTable: "Menu",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DishDbModelPackageDbModel_Packages_PackagesId",
                        column: x => x.PackagesId,
                        principalTable: "Packages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    zone_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    package_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount_of_people = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    start_time = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    end_time = table.Column<TimeOnly>(type: "time without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.id);
                    table.ForeignKey(
                        name: "FK_Bookings_Packages_package_id",
                        column: x => x.package_id,
                        principalTable: "Packages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Zones_zone_id",
                        column: x => x.zone_id,
                        principalTable: "Zones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    zone_id = table.Column<Guid>(type: "uuid", nullable: false),
                    date = table.Column<string>(type: "varchar(64)", nullable: false),
                    mark = table.Column<double>(type: "double precision", nullable: false),
                    message = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.id);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Zones_zone_id",
                        column: x => x.zone_id,
                        principalTable: "Zones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    zone_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar(64)", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    year_of_production = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.id);
                    table.ForeignKey(
                        name: "FK_Inventories_Zones_zone_id",
                        column: x => x.zone_id,
                        principalTable: "Zones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PackageDbModelZoneDbModel",
                columns: table => new
                {
                    PackagesId = table.Column<Guid>(type: "uuid", nullable: false),
                    ZonesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageDbModelZoneDbModel", x => new { x.PackagesId, x.ZonesId });
                    table.ForeignKey(
                        name: "FK_PackageDbModelZoneDbModel_Packages_PackagesId",
                        column: x => x.PackagesId,
                        principalTable: "Packages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackageDbModelZoneDbModel_Zones_ZonesId",
                        column: x => x.ZonesId,
                        principalTable: "Zones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_package_id",
                table: "Bookings",
                column: "package_id");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_user_id",
                table: "Bookings",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_zone_id",
                table: "Bookings",
                column: "zone_id");

            migrationBuilder.CreateIndex(
                name: "IX_DishDbModelPackageDbModel_PackagesId",
                table: "DishDbModelPackageDbModel",
                column: "PackagesId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_user_id",
                table: "Feedbacks",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_zone_id",
                table: "Feedbacks",
                column: "zone_id");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_zone_id",
                table: "Inventories",
                column: "zone_id");

            migrationBuilder.CreateIndex(
                name: "IX_PackageDbModelZoneDbModel_ZonesId",
                table: "PackageDbModelZoneDbModel",
                column: "ZonesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "DishDbModelPackageDbModel");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "PackageDbModelZoneDbModel");

            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.DropTable(
                name: "Zones");
        }
    }
}
