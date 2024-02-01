using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazingTech.InternSystem.Migrations
{
    public partial class Test1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CauHoi",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CauHoi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dashboard",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReceivedCV = table.Column<int>(type: "int", nullable: false),
                    Interviewed = table.Column<int>(type: "int", nullable: false),
                    Passed = table.Column<int>(type: "int", nullable: false),
                    Interning = table.Column<int>(type: "int", nullable: false),
                    Interned = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dashboard", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NhomZalo",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenNhom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkNhom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhomZalo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TruongHoc",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoTuanThucTap = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TruongHoc", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KiThucTap",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdTruong = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KiThucTap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KiThucTap_TruongHoc_IdTruong",
                        column: x => x.IdTruong,
                        principalSchema: "dbo",
                        principalTable: "TruongHoc",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                schema: "dbo",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                schema: "dbo",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HoVaTen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResetToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetTokenExpires = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VerificationToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerificationTokenExpires = table.Column<DateTime>(type: "datetime2", nullable: true),
                    isConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    InternInfoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TrangThaiThucTap = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                schema: "dbo",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DuAn",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeaderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ThoiGianBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuAn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DuAn_AspNetUsers_LeaderId",
                        column: x => x.LeaderId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InternInfo",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GioiTinh = table.Column<bool>(type: "bit", nullable: false),
                    MSSV = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailTruong = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailCaNhan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sdt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SdtNguoiThan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GPA = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TrinhDoTiengAnh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkFacebook = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkCV = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NganhHoc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Round = table.Column<int>(type: "int", nullable: true),
                    ViTriMongMuon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdTruong = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    KiThucTapId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InternInfo_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InternInfo_KiThucTap_KiThucTapId",
                        column: x => x.KiThucTapId,
                        principalSchema: "dbo",
                        principalTable: "KiThucTap",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InternInfo_TruongHoc_IdTruong",
                        column: x => x.IdTruong,
                        principalSchema: "dbo",
                        principalTable: "TruongHoc",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LichPhongVan",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdNguoiPhongVan = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdNguoiDuocPhongVan = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ThoiGianPhongVan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DiaDiemPhongVan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DaXacNhanMail = table.Column<bool>(type: "bit", nullable: true),
                    InterviewForm = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    TimeDuration = table.Column<TimeSpan>(type: "time", nullable: false),
                    KetQua = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichPhongVan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LichPhongVan_AspNetUsers_IdNguoiDuocPhongVan",
                        column: x => x.IdNguoiDuocPhongVan,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LichPhongVan_AspNetUsers_IdNguoiPhongVan",
                        column: x => x.IdNguoiPhongVan,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ThongBao",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdNguoiNhan = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdNguoiGui = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TieuDe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TinhTrang = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongBao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThongBao_AspNetUsers_IdNguoiGui",
                        column: x => x.IdNguoiGui,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ThongBao_AspNetUsers_IdNguoiNhan",
                        column: x => x.IdNguoiNhan,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserNhomZalo",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsMentor = table.Column<bool>(type: "bit", nullable: false),
                    IdNhomZalo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    JoinedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LeftTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNhomZalo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserNhomZalo_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserNhomZalo_NhomZalo_IdNhomZalo",
                        column: x => x.IdNhomZalo,
                        principalSchema: "dbo",
                        principalTable: "NhomZalo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserDuAn",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdDuAn = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ViTri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDuAn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDuAn_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserDuAn_DuAn_IdDuAn",
                        column: x => x.IdDuAn,
                        principalSchema: "dbo",
                        principalTable: "DuAn",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ViTri",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkNhomZalo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DuAnId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViTri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ViTri_DuAn_DuAnId",
                        column: x => x.DuAnId,
                        principalSchema: "dbo",
                        principalTable: "DuAn",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdNguoiDuocComment = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdNguoiComment = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comment_AspNetUsers_IdNguoiComment",
                        column: x => x.IdNguoiComment,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comment_InternInfo_IdNguoiDuocComment",
                        column: x => x.IdNguoiDuocComment,
                        principalSchema: "dbo",
                        principalTable: "InternInfo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CongNghe",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdViTri = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CongNghe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CongNghe_ViTri_IdViTri",
                        column: x => x.IdViTri,
                        principalSchema: "dbo",
                        principalTable: "ViTri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserViTri",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ViTrisId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserViTri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserViTri_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserViTri_ViTri_ViTrisId",
                        column: x => x.ViTrisId,
                        principalSchema: "dbo",
                        principalTable: "ViTri",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CauHoiCongNghe",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdCongNghe = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdCauhoi = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CauHoiCongNghe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CauHoiCongNghe_CauHoi_IdCauhoi",
                        column: x => x.IdCauhoi,
                        principalSchema: "dbo",
                        principalTable: "CauHoi",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CauHoiCongNghe_CongNghe_IdCongNghe",
                        column: x => x.IdCongNghe,
                        principalSchema: "dbo",
                        principalTable: "CongNghe",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CongNgheDuAn",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdCongNghe = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdDuAn = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CongNgheDuAn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CongNgheDuAn_CongNghe_IdCongNghe",
                        column: x => x.IdCongNghe,
                        principalSchema: "dbo",
                        principalTable: "CongNghe",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CongNgheDuAn_DuAn_IdDuAn",
                        column: x => x.IdDuAn,
                        principalSchema: "dbo",
                        principalTable: "DuAn",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PhongVan",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CauTraLoi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    NguoiCham = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RankDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdCauHoiCongNghe = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdLichPhongVan = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhongVan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhongVan_CauHoiCongNghe_IdCauHoiCongNghe",
                        column: x => x.IdCauHoiCongNghe,
                        principalSchema: "dbo",
                        principalTable: "CauHoiCongNghe",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PhongVan_LichPhongVan_IdLichPhongVan",
                        column: x => x.IdLichPhongVan,
                        principalSchema: "dbo",
                        principalTable: "LichPhongVan",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                schema: "dbo",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "dbo",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                schema: "dbo",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                schema: "dbo",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                schema: "dbo",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "dbo",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_InternInfoId",
                schema: "dbo",
                table: "AspNetUsers",
                column: "InternInfoId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "dbo",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CauHoiCongNghe_IdCauhoi",
                schema: "dbo",
                table: "CauHoiCongNghe",
                column: "IdCauhoi");

            migrationBuilder.CreateIndex(
                name: "IX_CauHoiCongNghe_IdCongNghe",
                schema: "dbo",
                table: "CauHoiCongNghe",
                column: "IdCongNghe");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_IdNguoiComment",
                schema: "dbo",
                table: "Comment",
                column: "IdNguoiComment");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_IdNguoiDuocComment",
                schema: "dbo",
                table: "Comment",
                column: "IdNguoiDuocComment");

            migrationBuilder.CreateIndex(
                name: "IX_CongNghe_IdViTri",
                schema: "dbo",
                table: "CongNghe",
                column: "IdViTri");

            migrationBuilder.CreateIndex(
                name: "IX_CongNgheDuAn_IdCongNghe",
                schema: "dbo",
                table: "CongNgheDuAn",
                column: "IdCongNghe");

            migrationBuilder.CreateIndex(
                name: "IX_CongNgheDuAn_IdDuAn",
                schema: "dbo",
                table: "CongNgheDuAn",
                column: "IdDuAn");

            migrationBuilder.CreateIndex(
                name: "IX_DuAn_LeaderId",
                schema: "dbo",
                table: "DuAn",
                column: "LeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_InternInfo_IdTruong",
                schema: "dbo",
                table: "InternInfo",
                column: "IdTruong");

            migrationBuilder.CreateIndex(
                name: "IX_InternInfo_KiThucTapId",
                schema: "dbo",
                table: "InternInfo",
                column: "KiThucTapId");

            migrationBuilder.CreateIndex(
                name: "IX_InternInfo_UserId",
                schema: "dbo",
                table: "InternInfo",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_KiThucTap_IdTruong",
                schema: "dbo",
                table: "KiThucTap",
                column: "IdTruong");

            migrationBuilder.CreateIndex(
                name: "IX_LichPhongVan_IdNguoiDuocPhongVan",
                schema: "dbo",
                table: "LichPhongVan",
                column: "IdNguoiDuocPhongVan");

            migrationBuilder.CreateIndex(
                name: "IX_LichPhongVan_IdNguoiPhongVan",
                schema: "dbo",
                table: "LichPhongVan",
                column: "IdNguoiPhongVan");

            migrationBuilder.CreateIndex(
                name: "IX_PhongVan_IdCauHoiCongNghe",
                schema: "dbo",
                table: "PhongVan",
                column: "IdCauHoiCongNghe");

            migrationBuilder.CreateIndex(
                name: "IX_PhongVan_IdLichPhongVan",
                schema: "dbo",
                table: "PhongVan",
                column: "IdLichPhongVan");

            migrationBuilder.CreateIndex(
                name: "IX_ThongBao_IdNguoiGui",
                schema: "dbo",
                table: "ThongBao",
                column: "IdNguoiGui");

            migrationBuilder.CreateIndex(
                name: "IX_ThongBao_IdNguoiNhan",
                schema: "dbo",
                table: "ThongBao",
                column: "IdNguoiNhan");

            migrationBuilder.CreateIndex(
                name: "IX_UserDuAn_IdDuAn",
                schema: "dbo",
                table: "UserDuAn",
                column: "IdDuAn");

            migrationBuilder.CreateIndex(
                name: "IX_UserDuAn_UserId",
                schema: "dbo",
                table: "UserDuAn",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNhomZalo_IdNhomZalo",
                schema: "dbo",
                table: "UserNhomZalo",
                column: "IdNhomZalo");

            migrationBuilder.CreateIndex(
                name: "IX_UserNhomZalo_UserId",
                schema: "dbo",
                table: "UserNhomZalo",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserViTri_UsersId",
                schema: "dbo",
                table: "UserViTri",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_UserViTri_ViTrisId",
                schema: "dbo",
                table: "UserViTri",
                column: "ViTrisId");

            migrationBuilder.CreateIndex(
                name: "IX_ViTri_DuAnId",
                schema: "dbo",
                table: "ViTri",
                column: "DuAnId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                schema: "dbo",
                table: "AspNetUserClaims",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                schema: "dbo",
                table: "AspNetUserLogins",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                schema: "dbo",
                table: "AspNetUserRoles",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_InternInfo_InternInfoId",
                schema: "dbo",
                table: "AspNetUsers",
                column: "InternInfoId",
                principalSchema: "dbo",
                principalTable: "InternInfo",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternInfo_AspNetUsers_UserId",
                schema: "dbo",
                table: "InternInfo");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Comment",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CongNgheDuAn",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Dashboard",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PhongVan",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ThongBao",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserDuAn",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserNhomZalo",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserViTri",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetRoles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CauHoiCongNghe",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "LichPhongVan",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "NhomZalo",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CauHoi",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CongNghe",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ViTri",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "DuAn",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUsers",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "InternInfo",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "KiThucTap",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TruongHoc",
                schema: "dbo");
        }
    }
}
