USE [AuthECDB]
GO
/****** Object:  UserDefinedTableType [dbo].[ProizvodIdTableType]    Script Date: 10/31/2024 5:04:35 PM ******/
CREATE TYPE [dbo].[ProizvodIdTableType] AS TABLE(
	[ProizvodID] [int] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[ProizvodType]    Script Date: 10/31/2024 5:04:35 PM ******/
CREATE TYPE [dbo].[ProizvodType] AS TABLE(
	[ProizvodID] [int] NULL,
	[NazivProizvoda] [nvarchar](100) NULL,
	[Cena] [decimal](10, 2) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[RacunIdTableType]    Script Date: 10/31/2024 5:04:35 PM ******/
CREATE TYPE [dbo].[RacunIdTableType] AS TABLE(
	[RacunId] [int] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[RacunType]    Script Date: 10/31/2024 5:04:35 PM ******/
CREATE TYPE [dbo].[RacunType] AS TABLE(
	[RacunId] [int] NULL,
	[StatusRacuna] [nvarchar](50) NULL,
	[Datum] [datetime] NULL,
	[Vreme] [time](7) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[StavkeRacunaIdTableType]    Script Date: 10/31/2024 5:04:35 PM ******/
CREATE TYPE [dbo].[StavkeRacunaIdTableType] AS TABLE(
	[StavkeRacunaID] [int] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[StavkeRacunaType]    Script Date: 10/31/2024 5:04:35 PM ******/
CREATE TYPE [dbo].[StavkeRacunaType] AS TABLE(
	[StavkeRacunaID] [int] NULL,
	[RacunId] [int] NULL,
	[Kolicina] [int] NULL,
	[Popust] [decimal](18, 0) NULL
)
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 10/31/2024 5:04:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 10/31/2024 5:04:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 10/31/2024 5:04:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 10/31/2024 5:04:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 10/31/2024 5:04:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 10/31/2024 5:04:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 10/31/2024 5:04:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[Discriminator] [nvarchar](13) NOT NULL,
	[FullName] [nvarchar](15) NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 10/31/2024 5:04:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Proizvod]    Script Date: 10/31/2024 5:04:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Proizvod](
	[ProizvodID] [int] IDENTITY(1,1) NOT NULL,
	[NazivProizvoda] [nvarchar](50) NULL,
	[Cena] [decimal](10, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProizvodID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Racun]    Script Date: 10/31/2024 5:04:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Racun](
	[RacunId] [int] IDENTITY(1,1) NOT NULL,
	[StatusRacuna] [nvarchar](50) NULL,
	[Datum] [date] NULL,
	[Vreme] [time](0) NULL,
PRIMARY KEY CLUSTERED 
(
	[RacunId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StavkeRacuna]    Script Date: 10/31/2024 5:04:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StavkeRacuna](
	[StavkeRacunaID] [int] IDENTITY(1,1) NOT NULL,
	[RacunId] [int] NULL,
	[ProizvodID] [int] NULL,
	[Kolicina] [int] NULL,
	[Popust] [decimal](5, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[StavkeRacunaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT (N'') FOR [Discriminator]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[StavkeRacuna]  WITH NOCHECK ADD  CONSTRAINT [FK_Proizvod] FOREIGN KEY([ProizvodID])
REFERENCES [dbo].[Proizvod] ([ProizvodID])
GO
ALTER TABLE [dbo].[StavkeRacuna] CHECK CONSTRAINT [FK_Proizvod]
GO
ALTER TABLE [dbo].[StavkeRacuna]  WITH NOCHECK ADD  CONSTRAINT [FK_Racun] FOREIGN KEY([RacunId])
REFERENCES [dbo].[Racun] ([RacunId])
GO
ALTER TABLE [dbo].[StavkeRacuna] CHECK CONSTRAINT [FK_Racun]
GO
/****** Object:  StoredProcedure [dbo].[BulkDel_Proizvod]    Script Date: 10/31/2024 5:04:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BulkDel_Proizvod]
    @ProizvodIds ProizvodIdTableType READONLY -- Use the defined table type
AS
BEGIN
    DELETE FROM [dbo].[Proizvod]
    WHERE ProizvodID IN (SELECT ProizvodID FROM @ProizvodIds);
END
GO
/****** Object:  StoredProcedure [dbo].[BulkDel_Racun]    Script Date: 10/31/2024 5:04:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[BulkDel_Racun]
    @RacunIds RacunIdTableType READONLY -- Use the defined table type
AS
BEGIN
    DELETE FROM [dbo].[Racun]
    WHERE RacunId IN (SELECT RacunId FROM @RacunIds);
END
GO
/****** Object:  StoredProcedure [dbo].[BulkDel_StavkeRacuna]    Script Date: 10/31/2024 5:04:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[BulkDel_StavkeRacuna]
    @StavkeRacunaIds StavkeRacunaIdTableType READONLY -- Use the defined table type
AS
BEGIN
    DELETE FROM [dbo].[StavkeRacuna]
    WHERE StavkeRacunaID IN (SELECT StavkeRacunaID FROM @StavkeRacunaIds);
END
GO
/****** Object:  StoredProcedure [dbo].[BulkIns_Proizvod]    Script Date: 10/31/2024 5:04:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BulkIns_Proizvod]
    @Proizvodi ProizvodType READONLY
AS
BEGIN
    INSERT INTO [dbo].[Proizvod] (NazivProizvoda, Cena)
    SELECT NazivProizvoda, Cena FROM @Proizvodi;
END
GO
/****** Object:  StoredProcedure [dbo].[BulkIns_Racun]    Script Date: 10/31/2024 5:04:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[BulkIns_Racun]
    @Racuni RacunType READONLY
AS
BEGIN
    -- Declare the output table variable to store inserted RacunId
    DECLARE @OutputTable TABLE (RacunId INT);

    INSERT INTO [dbo].[Racun] (StatusRacuna, Datum, Vreme)
    OUTPUT INSERTED.RacunId INTO @OutputTable -- Capture the inserted IDs
    SELECT StatusRacuna, Datum, Vreme FROM @Racuni;

    -- Optionally, you can return the inserted RacunId values
    SELECT * FROM @OutputTable; -- Return the results if needed
END
GO
/****** Object:  StoredProcedure [dbo].[BulkIns_StavkeRacuna]    Script Date: 10/31/2024 5:04:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[BulkIns_StavkeRacuna]
    @Stavke StavkeRacunaType READONLY
AS
BEGIN
    INSERT INTO [dbo].[StavkeRacuna] (RacunId, Kolicina, Popust)
    SELECT RacunId, Kolicina, Popust FROM @Stavke;
END
GO
/****** Object:  StoredProcedure [dbo].[BulkUpd_Proizvod]    Script Date: 10/31/2024 5:04:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BulkUpd_Proizvod]
    @Proizvodi ProizvodType READONLY
AS
BEGIN
    UPDATE p
    SET p.NazivProizvoda = pv.NazivProizvoda, p.Cena = pv.Cena
    FROM [dbo].[Proizvod] p
    INNER JOIN @Proizvodi pv ON p.ProizvodID = pv.ProizvodID;
END
GO
/****** Object:  StoredProcedure [dbo].[BulkUpd_Racun]    Script Date: 10/31/2024 5:04:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BulkUpd_Racun]
    @Racuni RacunType READONLY
AS
BEGIN
    UPDATE r
    SET r.StatusRacuna = rv.StatusRacuna,
        r.Datum = rv.Datum,
        r.Vreme = rv.Vreme
    FROM [dbo].[Racun] r
    INNER JOIN @Racuni rv ON r.RacunId = rv.RacunId;
END
GO
/****** Object:  StoredProcedure [dbo].[BulkUpd_StavkeRacuna]    Script Date: 10/31/2024 5:04:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BulkUpd_StavkeRacuna]
    @Stavke StavkeRacunaType READONLY
AS
BEGIN
    UPDATE sr
    SET sr.RacunId = sv.RacunId, sr.Kolicina = sv.Kolicina, sr.Popust = sv.Popust
    FROM [dbo].[StavkeRacuna] sr
    INNER JOIN @Stavke sv ON sr.StavkeRacunaID = sv.StavkeRacunaID;
END
GO
