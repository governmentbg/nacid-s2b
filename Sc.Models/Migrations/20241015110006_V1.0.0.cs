using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Sc.Models.Migrations
{
    /// <inheritdoc />
    public partial class V100 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "district",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    region = table.Column<int>(type: "integer", nullable: false),
                    code2 = table.Column<string>(type: "text", nullable: true),
                    secondlevelregioncode = table.Column<string>(type: "text", nullable: true),
                    mainsettlementcode = table.Column<string>(type: "text", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false),
                    alias = table.Column<string>(type: "text", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    namealt = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    descriptionalt = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_district", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "lawform",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false),
                    alias = table.Column<string>(type: "text", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    namealt = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    descriptionalt = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lawform", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "schemaversions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Version = table.Column<string>(type: "text", nullable: true),
                    Updatedon = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Systemname = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schemaversions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "smartspecialization",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codenumber = table.Column<string>(type: "text", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false),
                    alias = table.Column<string>(type: "text", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    namealt = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    descriptionalt = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    rootid = table.Column<int>(type: "integer", nullable: true),
                    parentid = table.Column<int>(type: "integer", nullable: true),
                    level = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_smartspecialization", x => x.id);
                    table.ForeignKey(
                        name: "FK_smartspecialization_smartspecialization_parentid",
                        column: x => x.parentid,
                        principalTable: "smartspecialization",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_smartspecialization_smartspecialization_rootid",
                        column: x => x.rootid,
                        principalTable: "smartspecialization",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "supplierofferingcounter",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    counter = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supplierofferingcounter", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "municipality",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    districtid = table.Column<int>(type: "integer", nullable: false),
                    code2 = table.Column<string>(type: "text", nullable: true),
                    mainsettlementcode = table.Column<string>(type: "text", nullable: true),
                    category = table.Column<string>(type: "text", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false),
                    alias = table.Column<string>(type: "text", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    namealt = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    descriptionalt = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_municipality", x => x.id);
                    table.ForeignKey(
                        name: "FK_municipality_district_districtid",
                        column: x => x.districtid,
                        principalTable: "district",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "settlement",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    municipalityid = table.Column<int>(type: "integer", nullable: false),
                    districtid = table.Column<int>(type: "integer", nullable: false),
                    municipalitycode = table.Column<string>(type: "text", nullable: true),
                    districtcode = table.Column<string>(type: "text", nullable: true),
                    municipalitycode2 = table.Column<string>(type: "text", nullable: true),
                    districtcode2 = table.Column<string>(type: "text", nullable: true),
                    typename = table.Column<string>(type: "text", nullable: true),
                    settlementname = table.Column<string>(type: "text", nullable: true),
                    typecode = table.Column<string>(type: "text", nullable: true),
                    mayoraltycode = table.Column<string>(type: "text", nullable: true),
                    category = table.Column<string>(type: "text", nullable: true),
                    altitude = table.Column<string>(type: "text", nullable: true),
                    isdistrict = table.Column<bool>(type: "boolean", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false),
                    alias = table.Column<string>(type: "text", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    namealt = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    descriptionalt = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_settlement", x => x.id);
                    table.ForeignKey(
                        name: "FK_settlement_district_districtid",
                        column: x => x.districtid,
                        principalTable: "district",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_settlement_municipality_municipalityid",
                        column: x => x.municipalityid,
                        principalTable: "municipality",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "company",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uic = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: true),
                    type = table.Column<int>(type: "integer", nullable: false),
                    lawformid = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    namealt = table.Column<string>(type: "text", nullable: true),
                    shortname = table.Column<string>(type: "text", nullable: true),
                    shortnamealt = table.Column<string>(type: "text", nullable: true),
                    settlementid = table.Column<int>(type: "integer", nullable: false),
                    municipalityid = table.Column<int>(type: "integer", nullable: false),
                    districtid = table.Column<int>(type: "integer", nullable: false),
                    address = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    addressalt = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    phonenumber = table.Column<string>(type: "character varying(18)", maxLength: 18, nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    isregistryagency = table.Column<bool>(type: "boolean", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company", x => x.id);
                    table.ForeignKey(
                        name: "FK_company_district_districtid",
                        column: x => x.districtid,
                        principalTable: "district",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_company_lawform_lawformid",
                        column: x => x.lawformid,
                        principalTable: "lawform",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_company_municipality_municipalityid",
                        column: x => x.municipalityid,
                        principalTable: "municipality",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_company_settlement_settlementid",
                        column: x => x.settlementid,
                        principalTable: "settlement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "complex",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lotnumber = table.Column<int>(type: "integer", nullable: false),
                    shortname = table.Column<string>(type: "text", nullable: true),
                    shortnamealt = table.Column<string>(type: "text", nullable: true),
                    coordinatorposition = table.Column<string>(type: "text", nullable: true),
                    coordinatorpositionalt = table.Column<string>(type: "text", nullable: true),
                    category = table.Column<string>(type: "text", nullable: true),
                    categoryalt = table.Column<string>(type: "text", nullable: true),
                    benefits = table.Column<string>(type: "text", nullable: true),
                    benefitsalt = table.Column<string>(type: "text", nullable: true),
                    scientificteam = table.Column<string>(type: "text", nullable: true),
                    scientificteamalt = table.Column<string>(type: "text", nullable: true),
                    areaofactivity = table.Column<int>(type: "integer", nullable: true),
                    datefrom = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    dateto = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    europeaninfrastructure = table.Column<string>(type: "text", nullable: true),
                    isforeign = table.Column<bool>(type: "boolean", nullable: false),
                    settlementid = table.Column<int>(type: "integer", nullable: true),
                    municipalityid = table.Column<int>(type: "integer", nullable: true),
                    districtid = table.Column<int>(type: "integer", nullable: true),
                    foreignsettlement = table.Column<string>(type: "text", nullable: true),
                    foreignsettlementalt = table.Column<string>(type: "text", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    addressalt = table.Column<string>(type: "text", nullable: true),
                    webpageurl = table.Column<string>(type: "text", nullable: true),
                    postcode = table.Column<string>(type: "text", nullable: true),
                    phone = table.Column<string>(type: "text", nullable: true),
                    fax = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false),
                    alias = table.Column<string>(type: "text", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    namealt = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    descriptionalt = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_complex", x => x.id);
                    table.ForeignKey(
                        name: "FK_complex_district_districtid",
                        column: x => x.districtid,
                        principalTable: "district",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_complex_municipality_municipalityid",
                        column: x => x.municipalityid,
                        principalTable: "municipality",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_complex_settlement_settlementid",
                        column: x => x.settlementid,
                        principalTable: "settlement",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "institution",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lotnumber = table.Column<int>(type: "integer", nullable: false),
                    uic = table.Column<string>(type: "text", nullable: true),
                    shortname = table.Column<string>(type: "text", nullable: true),
                    shortnamealt = table.Column<string>(type: "text", nullable: true),
                    organizationtype = table.Column<int>(type: "integer", nullable: true),
                    ownershiptype = table.Column<int>(type: "integer", nullable: true),
                    settlementid = table.Column<int>(type: "integer", nullable: true),
                    municipalityid = table.Column<int>(type: "integer", nullable: true),
                    districtid = table.Column<int>(type: "integer", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    addressalt = table.Column<string>(type: "text", nullable: true),
                    webpageurl = table.Column<string>(type: "text", nullable: true),
                    isresearchuniversity = table.Column<bool>(type: "boolean", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false),
                    alias = table.Column<string>(type: "text", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    namealt = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    descriptionalt = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    rootid = table.Column<int>(type: "integer", nullable: true),
                    parentid = table.Column<int>(type: "integer", nullable: true),
                    level = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_institution", x => x.id);
                    table.ForeignKey(
                        name: "FK_institution_district_districtid",
                        column: x => x.districtid,
                        principalTable: "district",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_institution_institution_parentid",
                        column: x => x.parentid,
                        principalTable: "institution",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_institution_institution_rootid",
                        column: x => x.rootid,
                        principalTable: "institution",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_institution_municipality_municipalityid",
                        column: x => x.municipalityid,
                        principalTable: "municipality",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_institution_settlement_settlementid",
                        column: x => x.settlementid,
                        principalTable: "settlement",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "companyadditional",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    staffcount = table.Column<long>(type: "bigint", nullable: false),
                    annualturnover = table.Column<decimal>(type: "numeric", nullable: false),
                    webpage = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companyadditional", x => x.id);
                    table.ForeignKey(
                        name: "FK_companyadditional_company_id",
                        column: x => x.id,
                        principalTable: "company",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "companyrepresentative",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    username = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    namealt = table.Column<string>(type: "text", nullable: true),
                    phonenumber = table.Column<string>(type: "text", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companyrepresentative", x => x.id);
                    table.ForeignKey(
                        name: "FK_companyrepresentative_company_id",
                        column: x => x.id,
                        principalTable: "company",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "complexorganization",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    complexid = table.Column<int>(type: "integer", nullable: false),
                    organizationtypeenum = table.Column<int>(type: "integer", nullable: true),
                    financingorganizationlotid = table.Column<int>(type: "integer", nullable: true),
                    financingorganizationname = table.Column<string>(type: "text", nullable: true),
                    financingorganizationnamealt = table.Column<string>(type: "text", nullable: true),
                    financingorganizationshortname = table.Column<string>(type: "text", nullable: true),
                    financingorganizationshortnamealt = table.Column<string>(type: "text", nullable: true),
                    organizationlotid = table.Column<int>(type: "integer", nullable: true),
                    organizationname = table.Column<string>(type: "text", nullable: true),
                    organizationnamealt = table.Column<string>(type: "text", nullable: true),
                    organizationshortname = table.Column<string>(type: "text", nullable: true),
                    organizationshortnamealt = table.Column<string>(type: "text", nullable: true),
                    namernd = table.Column<string>(type: "text", nullable: true),
                    namerndalt = table.Column<string>(type: "text", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_complexorganization", x => x.id);
                    table.ForeignKey(
                        name: "FK_complexorganization_complex_complexid",
                        column: x => x.complexid,
                        principalTable: "complex",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "supplier",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<int>(type: "integer", nullable: false),
                    institutionid = table.Column<int>(type: "integer", nullable: true),
                    complexid = table.Column<int>(type: "integer", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supplier", x => x.id);
                    table.ForeignKey(
                        name: "FK_supplier_complex_complexid",
                        column: x => x.complexid,
                        principalTable: "complex",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_supplier_institution_institutionid",
                        column: x => x.institutionid,
                        principalTable: "institution",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "approveregistration",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    finishdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    administrateduserid = table.Column<int>(type: "integer", nullable: true),
                    administratedusername = table.Column<string>(type: "text", nullable: true),
                    jsonsignupdto = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<int>(type: "integer", nullable: false),
                    declinednote = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    supplierid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_approveregistration", x => x.id);
                    table.ForeignKey(
                        name: "FK_approveregistration_supplier_supplierid",
                        column: x => x.supplierid,
                        principalTable: "supplier",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "supplierequipment",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    supplierid = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supplierequipment", x => x.id);
                    table.ForeignKey(
                        name: "FK_supplierequipment_supplier_supplierid",
                        column: x => x.supplierid,
                        principalTable: "supplier",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "supplieroffering",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    supplierid = table.Column<int>(type: "integer", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    offeringtype = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    shortdescription = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    settlementid = table.Column<int>(type: "integer", nullable: false),
                    municipalityid = table.Column<int>(type: "integer", nullable: false),
                    districtid = table.Column<int>(type: "integer", nullable: false),
                    address = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    webpageurl = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supplieroffering", x => x.id);
                    table.ForeignKey(
                        name: "FK_supplieroffering_district_districtid",
                        column: x => x.districtid,
                        principalTable: "district",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_supplieroffering_municipality_municipalityid",
                        column: x => x.municipalityid,
                        principalTable: "municipality",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_supplieroffering_settlement_settlementid",
                        column: x => x.settlementid,
                        principalTable: "settlement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_supplieroffering_supplier_supplierid",
                        column: x => x.supplierid,
                        principalTable: "supplier",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "supplierrepresentative",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    username = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    namealt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    phonenumber = table.Column<string>(type: "character varying(18)", maxLength: 18, nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supplierrepresentative", x => x.id);
                    table.ForeignKey(
                        name: "FK_supplierrepresentative_supplier_id",
                        column: x => x.id,
                        principalTable: "supplier",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "supplierteam",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    supplierid = table.Column<int>(type: "integer", nullable: false),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    username = table.Column<string>(type: "text", nullable: true),
                    academicrankdegree = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    position = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    firstname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    middlename = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    lastname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    phonenumber = table.Column<string>(type: "character varying(18)", maxLength: 18, nullable: true),
                    raslotid = table.Column<int>(type: "integer", nullable: true),
                    raslotnumber = table.Column<int>(type: "integer", nullable: true),
                    rasportalurl = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supplierteam", x => x.id);
                    table.ForeignKey(
                        name: "FK_supplierteam_supplier_supplierid",
                        column: x => x.supplierid,
                        principalTable: "supplier",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "approveregistrationfile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_approveregistrationfile", x => x.id);
                    table.ForeignKey(
                        name: "FK_approveregistrationfile_approveregistration_id",
                        column: x => x.id,
                        principalTable: "approveregistration",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "approveregistrationhistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    approveregistrationid = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    finishdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    administrateduserid = table.Column<int>(type: "integer", nullable: true),
                    administratedusername = table.Column<string>(type: "text", nullable: true),
                    jsonsignupdto = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<int>(type: "integer", nullable: false),
                    declinednote = table.Column<string>(type: "text", nullable: true),
                    supplierid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_approveregistrationhistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_approveregistrationhistory_approveregistration_approveregis~",
                        column: x => x.approveregistrationid,
                        principalTable: "approveregistration",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_approveregistrationhistory_supplier_supplierid",
                        column: x => x.supplierid,
                        principalTable: "supplier",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "supplierequipmentfile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supplierequipmentfile", x => x.id);
                    table.ForeignKey(
                        name: "FK_supplierequipmentfile_supplierequipment_id",
                        column: x => x.id,
                        principalTable: "supplierequipment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "receivedvoucher",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false),
                    contractdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    contractnumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    companyuserid = table.Column<int>(type: "integer", nullable: false),
                    companyid = table.Column<int>(type: "integer", nullable: false),
                    supplierid = table.Column<int>(type: "integer", nullable: true),
                    offeringid = table.Column<int>(type: "integer", nullable: true),
                    offeringadditionalpayment = table.Column<bool>(type: "boolean", nullable: false),
                    offeringclarifications = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    secondsupplierid = table.Column<int>(type: "integer", nullable: true),
                    secondofferingid = table.Column<int>(type: "integer", nullable: true),
                    secondofferingadditionalpayment = table.Column<bool>(type: "boolean", nullable: false),
                    secondofferingclarifications = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receivedvoucher", x => x.id);
                    table.ForeignKey(
                        name: "FK_receivedvoucher_company_companyid",
                        column: x => x.companyid,
                        principalTable: "company",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_receivedvoucher_supplier_secondsupplierid",
                        column: x => x.secondsupplierid,
                        principalTable: "supplier",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_receivedvoucher_supplier_supplierid",
                        column: x => x.supplierid,
                        principalTable: "supplier",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_receivedvoucher_supplieroffering_offeringid",
                        column: x => x.offeringid,
                        principalTable: "supplieroffering",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_receivedvoucher_supplieroffering_secondofferingid",
                        column: x => x.secondofferingid,
                        principalTable: "supplieroffering",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "supplierofferingequipment",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    supplierofferingid = table.Column<int>(type: "integer", nullable: false),
                    supplierequipmentid = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supplierofferingequipment", x => x.id);
                    table.ForeignKey(
                        name: "FK_supplierofferingequipment_supplierequipment_supplierequipme~",
                        column: x => x.supplierequipmentid,
                        principalTable: "supplierequipment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_supplierofferingequipment_supplieroffering_supplieroffering~",
                        column: x => x.supplierofferingid,
                        principalTable: "supplieroffering",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "supplierofferingfile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    supplierofferingid = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supplierofferingfile", x => x.id);
                    table.ForeignKey(
                        name: "FK_supplierofferingfile_supplieroffering_supplierofferingid",
                        column: x => x.supplierofferingid,
                        principalTable: "supplieroffering",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "supplierofferingsmartspecialization",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    supplierofferingid = table.Column<int>(type: "integer", nullable: false),
                    smartspecializationid = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supplierofferingsmartspecialization", x => x.id);
                    table.ForeignKey(
                        name: "FK_supplierofferingsmartspecialization_smartspecialization_sma~",
                        column: x => x.smartspecializationid,
                        principalTable: "smartspecialization",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_supplierofferingsmartspecialization_supplieroffering_suppli~",
                        column: x => x.supplierofferingid,
                        principalTable: "supplieroffering",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "voucherrequest",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying", fixedLength: true, maxLength: 5, nullable: true),
                    state = table.Column<int>(type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    requestuserid = table.Column<int>(type: "integer", nullable: false),
                    requestcompanyid = table.Column<int>(type: "integer", nullable: false),
                    supplierofferingid = table.Column<int>(type: "integer", nullable: false),
                    declinereason = table.Column<string>(type: "text", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_voucherrequest", x => x.id);
                    table.ForeignKey(
                        name: "FK_voucherrequest_company_requestcompanyid",
                        column: x => x.requestcompanyid,
                        principalTable: "company",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_voucherrequest_supplieroffering_supplierofferingid",
                        column: x => x.supplierofferingid,
                        principalTable: "supplieroffering",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "supplierofferingteam",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    supplierofferingid = table.Column<int>(type: "integer", nullable: false),
                    supplierteamid = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supplierofferingteam", x => x.id);
                    table.ForeignKey(
                        name: "FK_supplierofferingteam_supplieroffering_supplierofferingid",
                        column: x => x.supplierofferingid,
                        principalTable: "supplieroffering",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_supplierofferingteam_supplierteam_supplierteamid",
                        column: x => x.supplierteamid,
                        principalTable: "supplierteam",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "approveregistrationhistoryfile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_approveregistrationhistoryfile", x => x.id);
                    table.ForeignKey(
                        name: "FK_approveregistrationhistoryfile_approveregistrationhistory_id",
                        column: x => x.id,
                        principalTable: "approveregistrationhistory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "receivedvouchercertificate",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    receivedvoucherid = table.Column<int>(type: "integer", nullable: false),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    userfullname = table.Column<string>(type: "text", nullable: false),
                    supplierid = table.Column<int>(type: "integer", nullable: false),
                    offeringid = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receivedvouchercertificate", x => x.id);
                    table.ForeignKey(
                        name: "FK_receivedvouchercertificate_receivedvoucher_receivedvoucherid",
                        column: x => x.receivedvoucherid,
                        principalTable: "receivedvoucher",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_receivedvouchercertificate_supplier_supplierid",
                        column: x => x.supplierid,
                        principalTable: "supplier",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_receivedvouchercertificate_supplieroffering_offeringid",
                        column: x => x.offeringid,
                        principalTable: "supplieroffering",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "receivedvouchercommunication",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false),
                    entityid = table.Column<int>(type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    fromuserid = table.Column<int>(type: "integer", nullable: false),
                    fromusername = table.Column<string>(type: "text", nullable: true),
                    fromfullname = table.Column<string>(type: "text", nullable: true),
                    text = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receivedvouchercommunication", x => x.id);
                    table.ForeignKey(
                        name: "FK_receivedvouchercommunication_receivedvoucher_entityid",
                        column: x => x.entityid,
                        principalTable: "receivedvoucher",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "receivedvoucherfile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receivedvoucherfile", x => x.id);
                    table.ForeignKey(
                        name: "FK_receivedvoucherfile_receivedvoucher_id",
                        column: x => x.id,
                        principalTable: "receivedvoucher",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "receivedvoucherhistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    receivedvoucherid = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false),
                    contractdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    contractnumber = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<int>(type: "integer", nullable: false),
                    companyuserid = table.Column<int>(type: "integer", nullable: false),
                    companyid = table.Column<int>(type: "integer", nullable: false),
                    supplierid = table.Column<int>(type: "integer", nullable: true),
                    offeringid = table.Column<int>(type: "integer", nullable: true),
                    offeringadditionalpayment = table.Column<bool>(type: "boolean", nullable: false),
                    offeringclarifications = table.Column<string>(type: "text", nullable: true),
                    secondsupplierid = table.Column<int>(type: "integer", nullable: true),
                    secondofferingid = table.Column<int>(type: "integer", nullable: true),
                    secondofferingadditionalpayment = table.Column<bool>(type: "boolean", nullable: false),
                    secondofferingclarifications = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receivedvoucherhistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_receivedvoucherhistory_company_companyid",
                        column: x => x.companyid,
                        principalTable: "company",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_receivedvoucherhistory_receivedvoucher_receivedvoucherid",
                        column: x => x.receivedvoucherid,
                        principalTable: "receivedvoucher",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_receivedvoucherhistory_supplier_secondsupplierid",
                        column: x => x.secondsupplierid,
                        principalTable: "supplier",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_receivedvoucherhistory_supplier_supplierid",
                        column: x => x.supplierid,
                        principalTable: "supplier",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_receivedvoucherhistory_supplieroffering_offeringid",
                        column: x => x.offeringid,
                        principalTable: "supplieroffering",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_receivedvoucherhistory_supplieroffering_secondofferingid",
                        column: x => x.secondofferingid,
                        principalTable: "supplieroffering",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "receivedvouchernotification",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    changedtostate = table.Column<int>(type: "integer", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false),
                    entityid = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    fromuserid = table.Column<int>(type: "integer", nullable: false),
                    fromusername = table.Column<string>(type: "text", nullable: true),
                    fromfullname = table.Column<string>(type: "text", nullable: true),
                    fromuserorganization = table.Column<string>(type: "text", nullable: true),
                    touserid = table.Column<int>(type: "integer", nullable: false),
                    text = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receivedvouchernotification", x => x.id);
                    table.ForeignKey(
                        name: "FK_receivedvouchernotification_receivedvoucher_entityid",
                        column: x => x.entityid,
                        principalTable: "receivedvoucher",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "voucherrequestcommunication",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false),
                    entityid = table.Column<int>(type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    fromuserid = table.Column<int>(type: "integer", nullable: false),
                    fromusername = table.Column<string>(type: "text", nullable: true),
                    fromfullname = table.Column<string>(type: "text", nullable: true),
                    text = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_voucherrequestcommunication", x => x.id);
                    table.ForeignKey(
                        name: "FK_voucherrequestcommunication_voucherrequest_entityid",
                        column: x => x.entityid,
                        principalTable: "voucherrequest",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "voucherrequestnotification",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    changedtostate = table.Column<int>(type: "integer", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false),
                    entityid = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    fromuserid = table.Column<int>(type: "integer", nullable: false),
                    fromusername = table.Column<string>(type: "text", nullable: true),
                    fromfullname = table.Column<string>(type: "text", nullable: true),
                    fromuserorganization = table.Column<string>(type: "text", nullable: true),
                    touserid = table.Column<int>(type: "integer", nullable: false),
                    text = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_voucherrequestnotification", x => x.id);
                    table.ForeignKey(
                        name: "FK_voucherrequestnotification_voucherrequest_entityid",
                        column: x => x.entityid,
                        principalTable: "voucherrequest",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "receivedvouchercertificatefile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receivedvouchercertificatefile", x => x.id);
                    table.ForeignKey(
                        name: "FK_receivedvouchercertificatefile_receivedvouchercertificate_id",
                        column: x => x.id,
                        principalTable: "receivedvouchercertificate",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "receivedvoucherhistoryfile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receivedvoucherhistoryfile", x => x.id);
                    table.ForeignKey(
                        name: "FK_receivedvoucherhistoryfile_receivedvoucherhistory_id",
                        column: x => x.id,
                        principalTable: "receivedvoucherhistory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_approveregistration_supplierid",
                table: "approveregistration",
                column: "supplierid");

            migrationBuilder.CreateIndex(
                name: "IX_approveregistrationhistory_approveregistrationid",
                table: "approveregistrationhistory",
                column: "approveregistrationid");

            migrationBuilder.CreateIndex(
                name: "IX_approveregistrationhistory_supplierid",
                table: "approveregistrationhistory",
                column: "supplierid");

            migrationBuilder.CreateIndex(
                name: "IX_company_districtid",
                table: "company",
                column: "districtid");

            migrationBuilder.CreateIndex(
                name: "IX_company_lawformid",
                table: "company",
                column: "lawformid");

            migrationBuilder.CreateIndex(
                name: "IX_company_municipalityid",
                table: "company",
                column: "municipalityid");

            migrationBuilder.CreateIndex(
                name: "IX_company_settlementid",
                table: "company",
                column: "settlementid");

            migrationBuilder.CreateIndex(
                name: "IX_company_uic",
                table: "company",
                column: "uic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_complex_districtid",
                table: "complex",
                column: "districtid");

            migrationBuilder.CreateIndex(
                name: "IX_complex_municipalityid",
                table: "complex",
                column: "municipalityid");

            migrationBuilder.CreateIndex(
                name: "IX_complex_settlementid",
                table: "complex",
                column: "settlementid");

            migrationBuilder.CreateIndex(
                name: "IX_complexorganization_complexid",
                table: "complexorganization",
                column: "complexid");

            migrationBuilder.CreateIndex(
                name: "IX_institution_districtid",
                table: "institution",
                column: "districtid");

            migrationBuilder.CreateIndex(
                name: "IX_institution_municipalityid",
                table: "institution",
                column: "municipalityid");

            migrationBuilder.CreateIndex(
                name: "IX_institution_parentid",
                table: "institution",
                column: "parentid");

            migrationBuilder.CreateIndex(
                name: "IX_institution_rootid",
                table: "institution",
                column: "rootid");

            migrationBuilder.CreateIndex(
                name: "IX_institution_settlementid",
                table: "institution",
                column: "settlementid");

            migrationBuilder.CreateIndex(
                name: "IX_municipality_districtid",
                table: "municipality",
                column: "districtid");

            migrationBuilder.CreateIndex(
                name: "IX_receivedvoucher_companyid",
                table: "receivedvoucher",
                column: "companyid");

            migrationBuilder.CreateIndex(
                name: "IX_receivedvoucher_contractnumber",
                table: "receivedvoucher",
                column: "contractnumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_receivedvoucher_offeringid",
                table: "receivedvoucher",
                column: "offeringid");

            migrationBuilder.CreateIndex(
                name: "IX_receivedvoucher_secondofferingid",
                table: "receivedvoucher",
                column: "secondofferingid");

            migrationBuilder.CreateIndex(
                name: "IX_receivedvoucher_secondsupplierid",
                table: "receivedvoucher",
                column: "secondsupplierid");

            migrationBuilder.CreateIndex(
                name: "IX_receivedvoucher_supplierid",
                table: "receivedvoucher",
                column: "supplierid");

            migrationBuilder.CreateIndex(
                name: "IX_receivedvouchercertificate_offeringid",
                table: "receivedvouchercertificate",
                column: "offeringid");

            migrationBuilder.CreateIndex(
                name: "IX_receivedvouchercertificate_receivedvoucherid",
                table: "receivedvouchercertificate",
                column: "receivedvoucherid");

            migrationBuilder.CreateIndex(
                name: "IX_receivedvouchercertificate_supplierid",
                table: "receivedvouchercertificate",
                column: "supplierid");

            migrationBuilder.CreateIndex(
                name: "IX_receivedvouchercommunication_entityid",
                table: "receivedvouchercommunication",
                column: "entityid");

            migrationBuilder.CreateIndex(
                name: "IX_receivedvoucherhistory_companyid",
                table: "receivedvoucherhistory",
                column: "companyid");

            migrationBuilder.CreateIndex(
                name: "IX_receivedvoucherhistory_offeringid",
                table: "receivedvoucherhistory",
                column: "offeringid");

            migrationBuilder.CreateIndex(
                name: "IX_receivedvoucherhistory_receivedvoucherid",
                table: "receivedvoucherhistory",
                column: "receivedvoucherid");

            migrationBuilder.CreateIndex(
                name: "IX_receivedvoucherhistory_secondofferingid",
                table: "receivedvoucherhistory",
                column: "secondofferingid");

            migrationBuilder.CreateIndex(
                name: "IX_receivedvoucherhistory_secondsupplierid",
                table: "receivedvoucherhistory",
                column: "secondsupplierid");

            migrationBuilder.CreateIndex(
                name: "IX_receivedvoucherhistory_supplierid",
                table: "receivedvoucherhistory",
                column: "supplierid");

            migrationBuilder.CreateIndex(
                name: "IX_receivedvouchernotification_entityid",
                table: "receivedvouchernotification",
                column: "entityid");

            migrationBuilder.CreateIndex(
                name: "IX_settlement_districtid",
                table: "settlement",
                column: "districtid");

            migrationBuilder.CreateIndex(
                name: "IX_settlement_municipalityid",
                table: "settlement",
                column: "municipalityid");

            migrationBuilder.CreateIndex(
                name: "IX_smartspecialization_codenumber",
                table: "smartspecialization",
                column: "codenumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_smartspecialization_parentid",
                table: "smartspecialization",
                column: "parentid");

            migrationBuilder.CreateIndex(
                name: "IX_smartspecialization_rootid",
                table: "smartspecialization",
                column: "rootid");

            migrationBuilder.CreateIndex(
                name: "IX_supplier_complexid",
                table: "supplier",
                column: "complexid");

            migrationBuilder.CreateIndex(
                name: "IX_supplier_institutionid",
                table: "supplier",
                column: "institutionid");

            migrationBuilder.CreateIndex(
                name: "IX_supplierequipment_supplierid",
                table: "supplierequipment",
                column: "supplierid");

            migrationBuilder.CreateIndex(
                name: "IX_supplieroffering_code",
                table: "supplieroffering",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_supplieroffering_districtid",
                table: "supplieroffering",
                column: "districtid");

            migrationBuilder.CreateIndex(
                name: "IX_supplieroffering_municipalityid",
                table: "supplieroffering",
                column: "municipalityid");

            migrationBuilder.CreateIndex(
                name: "IX_supplieroffering_settlementid",
                table: "supplieroffering",
                column: "settlementid");

            migrationBuilder.CreateIndex(
                name: "IX_supplieroffering_supplierid",
                table: "supplieroffering",
                column: "supplierid");

            migrationBuilder.CreateIndex(
                name: "IX_supplierofferingequipment_supplierequipmentid",
                table: "supplierofferingequipment",
                column: "supplierequipmentid");

            migrationBuilder.CreateIndex(
                name: "IX_supplierofferingequipment_supplierofferingid_supplierequipm~",
                table: "supplierofferingequipment",
                columns: new[] { "supplierofferingid", "supplierequipmentid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_supplierofferingfile_supplierofferingid",
                table: "supplierofferingfile",
                column: "supplierofferingid");

            migrationBuilder.CreateIndex(
                name: "IX_supplierofferingsmartspecialization_smartspecializationid",
                table: "supplierofferingsmartspecialization",
                column: "smartspecializationid");

            migrationBuilder.CreateIndex(
                name: "IX_supplierofferingsmartspecialization_supplierofferingid_smar~",
                table: "supplierofferingsmartspecialization",
                columns: new[] { "supplierofferingid", "smartspecializationid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_supplierofferingteam_supplierofferingid_supplierteamid",
                table: "supplierofferingteam",
                columns: new[] { "supplierofferingid", "supplierteamid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_supplierofferingteam_supplierteamid",
                table: "supplierofferingteam",
                column: "supplierteamid");

            migrationBuilder.CreateIndex(
                name: "IX_supplierteam_supplierid",
                table: "supplierteam",
                column: "supplierid");

            migrationBuilder.CreateIndex(
                name: "IX_supplierteam_userid_supplierid",
                table: "supplierteam",
                columns: new[] { "userid", "supplierid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_voucherrequest_code",
                table: "voucherrequest",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_voucherrequest_requestcompanyid",
                table: "voucherrequest",
                column: "requestcompanyid");

            migrationBuilder.CreateIndex(
                name: "IX_voucherrequest_supplierofferingid_requestcompanyid",
                table: "voucherrequest",
                columns: new[] { "supplierofferingid", "requestcompanyid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_voucherrequestcommunication_entityid",
                table: "voucherrequestcommunication",
                column: "entityid");

            migrationBuilder.CreateIndex(
                name: "IX_voucherrequestnotification_entityid",
                table: "voucherrequestnotification",
                column: "entityid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "approveregistrationfile");

            migrationBuilder.DropTable(
                name: "approveregistrationhistoryfile");

            migrationBuilder.DropTable(
                name: "companyadditional");

            migrationBuilder.DropTable(
                name: "companyrepresentative");

            migrationBuilder.DropTable(
                name: "complexorganization");

            migrationBuilder.DropTable(
                name: "receivedvouchercertificatefile");

            migrationBuilder.DropTable(
                name: "receivedvouchercommunication");

            migrationBuilder.DropTable(
                name: "receivedvoucherfile");

            migrationBuilder.DropTable(
                name: "receivedvoucherhistoryfile");

            migrationBuilder.DropTable(
                name: "receivedvouchernotification");

            migrationBuilder.DropTable(
                name: "schemaversions");

            migrationBuilder.DropTable(
                name: "supplierequipmentfile");

            migrationBuilder.DropTable(
                name: "supplierofferingcounter");

            migrationBuilder.DropTable(
                name: "supplierofferingequipment");

            migrationBuilder.DropTable(
                name: "supplierofferingfile");

            migrationBuilder.DropTable(
                name: "supplierofferingsmartspecialization");

            migrationBuilder.DropTable(
                name: "supplierofferingteam");

            migrationBuilder.DropTable(
                name: "supplierrepresentative");

            migrationBuilder.DropTable(
                name: "voucherrequestcommunication");

            migrationBuilder.DropTable(
                name: "voucherrequestnotification");

            migrationBuilder.DropTable(
                name: "approveregistrationhistory");

            migrationBuilder.DropTable(
                name: "receivedvouchercertificate");

            migrationBuilder.DropTable(
                name: "receivedvoucherhistory");

            migrationBuilder.DropTable(
                name: "supplierequipment");

            migrationBuilder.DropTable(
                name: "smartspecialization");

            migrationBuilder.DropTable(
                name: "supplierteam");

            migrationBuilder.DropTable(
                name: "voucherrequest");

            migrationBuilder.DropTable(
                name: "approveregistration");

            migrationBuilder.DropTable(
                name: "receivedvoucher");

            migrationBuilder.DropTable(
                name: "company");

            migrationBuilder.DropTable(
                name: "supplieroffering");

            migrationBuilder.DropTable(
                name: "lawform");

            migrationBuilder.DropTable(
                name: "supplier");

            migrationBuilder.DropTable(
                name: "complex");

            migrationBuilder.DropTable(
                name: "institution");

            migrationBuilder.DropTable(
                name: "settlement");

            migrationBuilder.DropTable(
                name: "municipality");

            migrationBuilder.DropTable(
                name: "district");
        }
    }
}
