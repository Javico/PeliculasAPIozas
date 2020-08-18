using Microsoft.EntityFrameworkCore.Migrations;

namespace PeliculasAPI.Migrations
{
    public partial class AdminDataUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
                SET IDENTITY_INSERT [AspNetRoles] ON;
                INSERT INTO [AspNetRoles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName])
                VALUES (N'1a40931e-15fa-4a30-9a2d-307525673e58', N'7d8b136c-ebd8-4817-b22a-47395ba3eab6', N'Admin', N'Admin');
                IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
                SET IDENTITY_INSERT [AspNetRoles] OFF;

                GO

                IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'ConcurrencyStamp', N'Email', N'EmailConfirmed', N'LockoutEnabled', N'LockoutEnd', N'NormalizedEmail', N'NormalizedUserName', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'SecurityStamp', N'TwoFactorEnabled', N'UserName') AND [object_id] = OBJECT_ID(N'[AspNetUsers]'))
                SET IDENTITY_INSERT [AspNetUsers] ON;
                INSERT INTO [AspNetUsers] ([Id], [AccessFailedCount], [ConcurrencyStamp], [Email], [EmailConfirmed], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [SecurityStamp], [TwoFactorEnabled], [UserName])
                VALUES (N'72a677a2-2384-4f39-86cb-3253bb9f65ad', 0, N'fb01eae1-4c1a-49d5-818f-7c4b723dedf0', N'javiconlinea@gmail.com', CAST(0 AS bit), CAST(0 AS bit), NULL, N'javiconlinea@gmail.com', N'javiconlinea@gmail.com', N'AQAAAAEAACcQAAAAELVSAK8nmKbUWhFKcOhQFt7sikJLwgfDMseOdIXkWsgLeQ6k328KDZPgGdeYyE6msg==', NULL, CAST(0 AS bit), N'343a0e88-b62d-4b8f-ae28-388f4b35c858', CAST(0 AS bit), N'javiconlinea@gmail.com');
                IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'ConcurrencyStamp', N'Email', N'EmailConfirmed', N'LockoutEnabled', N'LockoutEnd', N'NormalizedEmail', N'NormalizedUserName', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'SecurityStamp', N'TwoFactorEnabled', N'UserName') AND [object_id] = OBJECT_ID(N'[AspNetUsers]'))
                SET IDENTITY_INSERT [AspNetUsers] OFF;

                GO

                IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ClaimType', N'ClaimValue', N'UserId') AND [object_id] = OBJECT_ID(N'[AspNetUserClaims]'))
                SET IDENTITY_INSERT [AspNetUserClaims] ON;
                INSERT INTO [AspNetUserClaims] ([Id], [ClaimType], [ClaimValue], [UserId])
                VALUES (1, N'http://schemas.microsoft.com/ws/2008/06/identity/claims/role', N'Admin', N'72a677a2-2384-4f39-86cb-3253bb9f65ad');
                IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ClaimType', N'ClaimValue', N'UserId') AND [object_id] = OBJECT_ID(N'[AspNetUserClaims]'))
                SET IDENTITY_INSERT [AspNetUserClaims] OFF;
                GO
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
            table: "AspNetRoles",
            keyColumn: "Id",
            keyValue: "1a40931e-15fa-4a30-9a2d-307525673e58");

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "72a677a2-2384-4f39-86cb-3253bb9f65ad");
        }
    }
}
