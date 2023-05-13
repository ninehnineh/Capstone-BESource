using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.FindingSlotManagement.Infrastructure.Migrations
{
    public partial class AddedThreeEntityAndModifyPackagePRiceToTimeLine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parking",
                columns: table => new
                {
                    ParkingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(10,6)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(10,6)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    MotoSpot = table.Column<int>(type: "int", nullable: true),
                    CarSpot = table.Column<int>(type: "int", nullable: true),
                    IsFull = table.Column<bool>(type: "bit", nullable: true),
                    IsPrepayment = table.Column<bool>(type: "bit", nullable: true),
                    IsOvernight = table.Column<bool>(type: "bit", nullable: true),
                    Stars = table.Column<int>(type: "int", nullable: true),
                    StarsCount = table.Column<int>(type: "int", nullable: true),
                    ManagerID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parking", x => x.ParkingId);
                });

            migrationBuilder.CreateTable(
                name: "ParkingPrice",
                columns: table => new
                {
                    ParkingPriceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParkingPriceName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingPrice", x => x.ParkingPriceId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Traffic",
                columns: table => new
                {
                    TrafficId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Traffic", x => x.TrafficId);
                });

            migrationBuilder.CreateTable(
                name: "Floors",
                columns: table => new
                {
                    FloorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FloorName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    ParkingID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Floors", x => x.FloorId);
                    table.ForeignKey(
                        name: "FK__Floors__ParkingI__47DBAE45",
                        column: x => x.ParkingID,
                        principalTable: "Parking",
                        principalColumn: "ParkingId");
                });

            migrationBuilder.CreateTable(
                name: "ParkingSpotImage",
                columns: table => new
                {
                    ParkingSpotImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImgPath = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    ParkingID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingSpotImage", x => x.ParkingSpotImageId);
                    table.ForeignKey(
                        name: "FK__ParkingSp__Parki__3C69FB99",
                        column: x => x.ParkingID,
                        principalTable: "Parking",
                        principalColumn: "ParkingId");
                });

            migrationBuilder.CreateTable(
                name: "ParkingHasPrice",
                columns: table => new
                {
                    ParkingHasPriceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParkingId = table.Column<int>(type: "int", nullable: true),
                    ParkingPriceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingHasPrice", x => x.ParkingHasPriceId);
                    table.ForeignKey(
                        name: "FK_ParkingHasPrice_Parking",
                        column: x => x.ParkingId,
                        principalTable: "Parking",
                        principalColumn: "ParkingId");
                    table.ForeignKey(
                        name: "FK_ParkingHasPrice_ParkingPrice",
                        column: x => x.ParkingPriceId,
                        principalTable: "ParkingPrice",
                        principalColumn: "ParkingPriceId");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(255)", maxLength: 255, nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(255)", maxLength: 255, nullable: true),
                    Phone = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    Avatar = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: true),
                    Gender = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    Devicetoken = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    IsCensorship = table.Column<bool>(type: "bit", nullable: true),
                    ManagerID = table.Column<int>(type: "int", nullable: true),
                    RoleID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK__Users__ManagerID__267ABA7A",
                        column: x => x.ManagerID,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK__Users__RoleID__276EDEB3",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "RoleId");
                });

            migrationBuilder.CreateTable(
                name: "TimeLine",
                columns: table => new
                {
                    TimeLineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Price = table.Column<decimal>(type: "money", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    StartingTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsExtrafee = table.Column<bool>(type: "bit", nullable: true),
                    ExtraFee = table.Column<decimal>(type: "money", nullable: true),
                    ExtraTimeStep = table.Column<float>(type: "real", nullable: true),
                    HasPenaltyPrice = table.Column<bool>(type: "bit", nullable: true),
                    PenaltyPrice = table.Column<decimal>(type: "money", nullable: true),
                    PenaltyPriceStepTime = table.Column<float>(type: "real", nullable: true),
                    TrafficId = table.Column<int>(type: "int", nullable: true),
                    ParkingPriceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeLine", x => x.TimeLineId);
                    table.ForeignKey(
                        name: "FK_Timeline_ParkingPrice",
                        column: x => x.ParkingPriceId,
                        principalTable: "ParkingPrice",
                        principalColumn: "ParkingPriceId");
                    table.ForeignKey(
                        name: "FK_Timeline_Traffic",
                        column: x => x.TrafficId,
                        principalTable: "Traffic",
                        principalColumn: "TrafficId");
                });

            migrationBuilder.CreateTable(
                name: "BusinessProfiles",
                columns: table => new
                {
                    BusinessProfileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FrontIdentification = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    BackIdentification = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    BusinessLicense = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessProfiles", x => x.BusinessProfileId);
                    table.ForeignKey(
                        name: "fk_IsManager",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "FavoriteAddress",
                columns: table => new
                {
                    FavoriteAddressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteAddress", x => x.FavoriteAddressId);
                    table.ForeignKey(
                        name: "FK__FavoriteA__UserI__33D4B598",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "OTPs",
                columns: table => new
                {
                    OTPID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "char(6)", unicode: false, fixedLength: true, maxLength: 6, nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OTPs", x => x.OTPID);
                    table.ForeignKey(
                        name: "FK__OTP__UserID",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PayPal",
                columns: table => new
                {
                    PayPalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    SecretKey = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    ManagerID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayPal", x => x.PayPalId);
                    table.ForeignKey(
                        name: "FK__PayPal__ManagerI__2D27B809",
                        column: x => x.ManagerID,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "StaffParking",
                columns: table => new
                {
                    StaffParkingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    ParkingID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffParking", x => x.StaffParkingId);
                    table.ForeignKey(
                        name: "FK__StaffPark__Parki__398D8EEE",
                        column: x => x.ParkingID,
                        principalTable: "Parking",
                        principalColumn: "ParkingId");
                    table.ForeignKey(
                        name: "FK__StaffPark__UserI__38996AB5",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "VehicleInfor",
                columns: table => new
                {
                    VehicleInforId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LicensePlate = table.Column<string>(type: "nvarchar(225)", maxLength: 225, nullable: true),
                    VehicleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Color = table.Column<string>(type: "nvarchar(225)", maxLength: 225, nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    TrafficID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleInfor", x => x.VehicleInforId);
                    table.ForeignKey(
                        name: "FK__VehicleIn__Traff__4BAC3F29",
                        column: x => x.TrafficID,
                        principalTable: "Traffic",
                        principalColumn: "TrafficId");
                    table.ForeignKey(
                        name: "FK__VehicleIn__UserI__4AB81AF0",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "VnPay",
                columns: table => new
                {
                    VnPayId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TmnCode = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    HashSecret = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    ManagerID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VnPay", x => x.VnPayId);
                    table.ForeignKey(
                        name: "FK__VnPay__ManagerID__2A4B4B5E",
                        column: x => x.ManagerID,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "FieldWorkImg",
                columns: table => new
                {
                    FieldWorkImgId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImgUrl = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    BusinessProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldWorkImg", x => x.FieldWorkImgId);
                    table.ForeignKey(
                        name: "FK_FieldWorkImg_BusinessProfiles",
                        column: x => x.BusinessProfileId,
                        principalTable: "BusinessProfiles",
                        principalColumn: "BusinessProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    DateBook = table.Column<DateTime>(type: "date", nullable: false),
                    ParkingSlotID = table.Column<int>(type: "int", nullable: false),
                    BookingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    CheckinTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    CheckoutTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    ActualPrice = table.Column<decimal>(type: "money", nullable: true),
                    QRCodeText = table.Column<string>(type: "varchar(225)", unicode: false, maxLength: 225, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    GuestName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GuestPhone = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    TotalPrice = table.Column<decimal>(type: "money", nullable: true),
                    PaymentMethod = table.Column<string>(type: "varchar(225)", unicode: false, maxLength: 225, nullable: true),
                    TmnCodeVnPay = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    VehicleInforID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Booking__1BDD09E6ABAB9F2E", x => new { x.ParkingSlotID, x.StartTime, x.DateBook });
                    table.UniqueConstraint("AK_Booking_BookingID", x => x.BookingID);
                    table.ForeignKey(
                        name: "FK__Booking__UserID__5070F446",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK__Booking__Vehicle__4F7CD00D",
                        column: x => x.VehicleInforID,
                        principalTable: "VehicleInfor",
                        principalColumn: "VehicleInforId");
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tiltle = table.Column<string>(type: "nvarchar(225)", maxLength: 225, nullable: true),
                    Body = table.Column<string>(type: "nvarchar(225)", maxLength: 225, nullable: true),
                    SentTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    BookingID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK__Notificat__Booki__59063A47",
                        column: x => x.BookingID,
                        principalTable: "Booking",
                        principalColumn: "BookingID");
                });

            migrationBuilder.CreateTable(
                name: "ParkingSlots",
                columns: table => new
                {
                    ParkingSlotId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: true),
                    RowIndex = table.Column<int>(type: "int", nullable: true),
                    ColumnIndex = table.Column<int>(type: "int", nullable: true),
                    TrafficID = table.Column<int>(type: "int", nullable: true),
                    FloorID = table.Column<int>(type: "int", nullable: true),
                    ParkingID = table.Column<int>(type: "int", nullable: true),
                    BookingID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingSlots", x => x.ParkingSlotId);
                    table.ForeignKey(
                        name: "FK__ParkingSl__Booki__5629CD9C",
                        column: x => x.BookingID,
                        principalTable: "Booking",
                        principalColumn: "BookingID");
                    table.ForeignKey(
                        name: "FK__ParkingSl__Floor__5441852A",
                        column: x => x.FloorID,
                        principalTable: "Floors",
                        principalColumn: "FloorId");
                    table.ForeignKey(
                        name: "FK__ParkingSl__Parki__5535A963",
                        column: x => x.ParkingID,
                        principalTable: "Parking",
                        principalColumn: "ParkingId");
                    table.ForeignKey(
                        name: "FK__ParkingSl__Traff__534D60F1",
                        column: x => x.TrafficID,
                        principalTable: "Traffic",
                        principalColumn: "TrafficId");
                });

            migrationBuilder.CreateIndex(
                name: "AK_Booking_BookingIDsas",
                table: "Booking",
                column: "BookingID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_UserID",
                table: "Booking",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_VehicleInforID",
                table: "Booking",
                column: "VehicleInforID");

            migrationBuilder.CreateIndex(
                name: "UQ__Booking__3214EC2628BBAE14",
                table: "Booking",
                column: "BookingID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Business__1788CCAD877AB68C",
                table: "BusinessProfiles",
                column: "UserID",
                unique: true,
                filter: "([UserID] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteAddress_UserID",
                table: "FavoriteAddress",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_FieldWorkImg_BusinessProfileId",
                table: "FieldWorkImg",
                column: "BusinessProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Floors_ParkingID",
                table: "Floors",
                column: "ParkingID");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_BookingID",
                table: "Notifications",
                column: "BookingID");

            migrationBuilder.CreateIndex(
                name: "IX_OTPs_UserId",
                table: "OTPs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingHasPrice_ParkingId",
                table: "ParkingHasPrice",
                column: "ParkingId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingHasPrice_ParkingPriceId",
                table: "ParkingHasPrice",
                column: "ParkingPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSlots_BookingID",
                table: "ParkingSlots",
                column: "BookingID");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSlots_FloorID",
                table: "ParkingSlots",
                column: "FloorID");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSlots_ParkingID",
                table: "ParkingSlots",
                column: "ParkingID");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSlots_TrafficID",
                table: "ParkingSlots",
                column: "TrafficID");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSpotImage_ParkingID",
                table: "ParkingSpotImage",
                column: "ParkingID");

            migrationBuilder.CreateIndex(
                name: "IX_PayPal_ManagerID",
                table: "PayPal",
                column: "ManagerID");

            migrationBuilder.CreateIndex(
                name: "IX_StaffParking_ParkingID",
                table: "StaffParking",
                column: "ParkingID");

            migrationBuilder.CreateIndex(
                name: "IX_StaffParking_UserID",
                table: "StaffParking",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_TimeLine_ParkingPriceId",
                table: "TimeLine",
                column: "ParkingPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeLine_TrafficId",
                table: "TimeLine",
                column: "TrafficId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ManagerID",
                table: "Users",
                column: "ManagerID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInfor_TrafficID",
                table: "VehicleInfor",
                column: "TrafficID");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInfor_UserID",
                table: "VehicleInfor",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_VnPay_ManagerID",
                table: "VnPay",
                column: "ManagerID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteAddress");

            migrationBuilder.DropTable(
                name: "FieldWorkImg");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "OTPs");

            migrationBuilder.DropTable(
                name: "ParkingHasPrice");

            migrationBuilder.DropTable(
                name: "ParkingSlots");

            migrationBuilder.DropTable(
                name: "ParkingSpotImage");

            migrationBuilder.DropTable(
                name: "PayPal");

            migrationBuilder.DropTable(
                name: "StaffParking");

            migrationBuilder.DropTable(
                name: "TimeLine");

            migrationBuilder.DropTable(
                name: "VnPay");

            migrationBuilder.DropTable(
                name: "BusinessProfiles");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "Floors");

            migrationBuilder.DropTable(
                name: "ParkingPrice");

            migrationBuilder.DropTable(
                name: "VehicleInfor");

            migrationBuilder.DropTable(
                name: "Parking");

            migrationBuilder.DropTable(
                name: "Traffic");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
