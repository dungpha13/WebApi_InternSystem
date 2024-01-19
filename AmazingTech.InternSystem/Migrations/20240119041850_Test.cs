using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazingTech.InternSystem.Migrations
{
    public partial class Test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
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
                name: "Dashboard",
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
                name: "TruongHoc",
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
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
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
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
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
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
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
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
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
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DuAn",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeaderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ThoiGianBatDau = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ThoiGianKetThuc = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LichPhongVan",
                columns: table => new
                {
                    IdNguoiPhongVan = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdNguoiDuocPhongVan = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ThoiGianPhongVan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DiaDiemPhongVan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DaXacNhanMail = table.Column<bool>(type: "bit", nullable: true),
                    InterviewForm = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    TimeDuration = table.Column<TimeSpan>(type: "time", nullable: false),
                    KetQua = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichPhongVan", x => new { x.IdNguoiDuocPhongVan, x.IdNguoiPhongVan });
                    table.ForeignKey(
                        name: "FK_LichPhongVan_AspNetUsers_IdNguoiDuocPhongVan",
                        column: x => x.IdNguoiDuocPhongVan,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LichPhongVan_AspNetUsers_IdNguoiPhongVan",
                        column: x => x.IdNguoiPhongVan,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NhomZalo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenNhom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkNhom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdMentor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MentorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                    table.ForeignKey(
                        name: "FK_NhomZalo_AspNetUsers_MentorId",
                        column: x => x.MentorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoleUser",
                columns: table => new
                {
                    RolesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.RolesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_RoleUser_AspNetRoles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUser_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThongBao",
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
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ThongBao_AspNetUsers_IdNguoiNhan",
                        column: x => x.IdNguoiNhan,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "KiThucTap",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdTruong = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                        principalTable: "TruongHoc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDuAn",
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
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserDuAn_DuAn_IdDuAn",
                        column: x => x.IdDuAn,
                        principalTable: "DuAn",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ViTri",
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
                        principalTable: "DuAn",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserNhomZalo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserNhomZalo_NhomZalo_IdNhomZalo",
                        column: x => x.IdNhomZalo,
                        principalTable: "NhomZalo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InternInfo",
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
                    ChungChi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkFacebook = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkCV = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NganhHoc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Round = table.Column<int>(type: "int", nullable: true),
                    KiThucTapId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InternInfo_KiThucTap_KiThucTapId",
                        column: x => x.KiThucTapId,
                        principalTable: "KiThucTap",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CongNghe",
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
                        principalTable: "ViTri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserViTri",
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
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserViTri_ViTri_ViTrisId",
                        column: x => x.ViTrisId,
                        principalTable: "ViTri",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Comment",
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
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comment_InternInfo_IdNguoiDuocComment",
                        column: x => x.IdNguoiDuocComment,
                        principalTable: "InternInfo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CongNgheDuAn",
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
                        principalTable: "CongNghe",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CongNgheDuAn_DuAn_IdDuAn",
                        column: x => x.IdDuAn,
                        principalTable: "DuAn",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_IdNguoiComment",
                table: "Comment",
                column: "IdNguoiComment");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_IdNguoiDuocComment",
                table: "Comment",
                column: "IdNguoiDuocComment");

            migrationBuilder.CreateIndex(
                name: "IX_CongNghe_IdViTri",
                table: "CongNghe",
                column: "IdViTri");

            migrationBuilder.CreateIndex(
                name: "IX_CongNgheDuAn_IdCongNghe",
                table: "CongNgheDuAn",
                column: "IdCongNghe");

            migrationBuilder.CreateIndex(
                name: "IX_CongNgheDuAn_IdDuAn",
                table: "CongNgheDuAn",
                column: "IdDuAn");

            migrationBuilder.CreateIndex(
                name: "IX_DuAn_LeaderId",
                table: "DuAn",
                column: "LeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_InternInfo_KiThucTapId",
                table: "InternInfo",
                column: "KiThucTapId");

            migrationBuilder.CreateIndex(
                name: "IX_InternInfo_UserId",
                table: "InternInfo",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_KiThucTap_IdTruong",
                table: "KiThucTap",
                column: "IdTruong");

            migrationBuilder.CreateIndex(
                name: "IX_LichPhongVan_IdNguoiPhongVan",
                table: "LichPhongVan",
                column: "IdNguoiPhongVan");

            migrationBuilder.CreateIndex(
                name: "IX_NhomZalo_MentorId",
                table: "NhomZalo",
                column: "MentorId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_UsersId",
                table: "RoleUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_ThongBao_IdNguoiGui",
                table: "ThongBao",
                column: "IdNguoiGui");

            migrationBuilder.CreateIndex(
                name: "IX_ThongBao_IdNguoiNhan",
                table: "ThongBao",
                column: "IdNguoiNhan");

            migrationBuilder.CreateIndex(
                name: "IX_UserDuAn_IdDuAn",
                table: "UserDuAn",
                column: "IdDuAn");

            migrationBuilder.CreateIndex(
                name: "IX_UserDuAn_UserId",
                table: "UserDuAn",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNhomZalo_IdNhomZalo",
                table: "UserNhomZalo",
                column: "IdNhomZalo");

            migrationBuilder.CreateIndex(
                name: "IX_UserNhomZalo_UserId",
                table: "UserNhomZalo",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserViTri_UsersId",
                table: "UserViTri",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_UserViTri_ViTrisId",
                table: "UserViTri",
                column: "ViTrisId");

            migrationBuilder.CreateIndex(
                name: "IX_ViTri_DuAnId",
                table: "ViTri",
                column: "DuAnId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "CongNgheDuAn");

            migrationBuilder.DropTable(
                name: "Dashboard");

            migrationBuilder.DropTable(
                name: "LichPhongVan");

            migrationBuilder.DropTable(
                name: "RoleUser");

            migrationBuilder.DropTable(
                name: "ThongBao");

            migrationBuilder.DropTable(
                name: "UserDuAn");

            migrationBuilder.DropTable(
                name: "UserNhomZalo");

            migrationBuilder.DropTable(
                name: "UserViTri");

            migrationBuilder.DropTable(
                name: "InternInfo");

            migrationBuilder.DropTable(
                name: "CongNghe");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "NhomZalo");

            migrationBuilder.DropTable(
                name: "KiThucTap");

            migrationBuilder.DropTable(
                name: "ViTri");

            migrationBuilder.DropTable(
                name: "TruongHoc");

            migrationBuilder.DropTable(
                name: "DuAn");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
