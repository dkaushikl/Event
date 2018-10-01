USE [master]
GO
/****** Object:  Database [Event]    Script Date: 10/1/2018 5:34:02 PM ******/
CREATE DATABASE [Event]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Event', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\Event.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Event_log', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\Event_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Event] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Event].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Event] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Event] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Event] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Event] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Event] SET ARITHABORT OFF 
GO
ALTER DATABASE [Event] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Event] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Event] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Event] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Event] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Event] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Event] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Event] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Event] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Event] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Event] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Event] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Event] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Event] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Event] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Event] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Event] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Event] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Event] SET  MULTI_USER 
GO
ALTER DATABASE [Event] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Event] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Event] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Event] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [Event] SET DELAYED_DURABILITY = DISABLED 
GO
USE [Event]
GO
/****** Object:  Table [dbo].[Company]    Script Date: 10/1/2018 5:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Company](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Email] [nvarchar](200) NOT NULL,
	[MobileNo] [nvarchar](200) NOT NULL,
	[Address] [nvarchar](500) NOT NULL,
	[City] [nvarchar](200) NOT NULL,
	[State] [nvarchar](200) NOT NULL,
	[Country] [nvarchar](200) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CompanyMember]    Script Date: 10/1/2018 5:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompanyMember](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyId] [bigint] NOT NULL,
	[UserId] [bigint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_CompanyMember] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Event]    Script Date: 10/1/2018 5:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Event](
	[Id] [bigint] NOT NULL,
	[CompanyId] [bigint] NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Vanue] [nvarchar](max) NOT NULL,
	[StartDate] [date] NOT NULL,
	[StartTime] [time](7) NOT NULL,
	[EndDate] [date] NOT NULL,
	[EndTime] [time](7) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EventMember]    Script Date: 10/1/2018 5:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventMember](
	[Id] [bigint] NOT NULL,
	[EventId] [bigint] NOT NULL,
	[UserId] [bigint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_EventMember] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 10/1/2018 5:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Firstname] [nvarchar](250) NULL CONSTRAINT [DF_User_Firstname]  DEFAULT (''),
	[Lastname] [nvarchar](250) NULL CONSTRAINT [DF_User_Lastname]  DEFAULT (''),
	[Username] [nvarchar](250) NULL CONSTRAINT [DF_User_Username]  DEFAULT (''),
	[Email] [nvarchar](250) NOT NULL,
	[Password] [nvarchar](250) NOT NULL,
	[Mobile] [nvarchar](50) NULL CONSTRAINT [DF_User_Mobile]  DEFAULT (''),
	[Gender] [nvarchar](3) NULL CONSTRAINT [DF_User_Gender]  DEFAULT (''),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_User_CreateDate]  DEFAULT (getdate()),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_User_IsActive]  DEFAULT ((1)),
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Company] ADD  CONSTRAINT [DF_Company_Name]  DEFAULT ('') FOR [Name]
GO
ALTER TABLE [dbo].[Company] ADD  CONSTRAINT [DF_Company_Email]  DEFAULT ('') FOR [Email]
GO
ALTER TABLE [dbo].[Company] ADD  CONSTRAINT [DF_Company_MobileNo]  DEFAULT ('') FOR [MobileNo]
GO
ALTER TABLE [dbo].[Company] ADD  CONSTRAINT [DF_Company_Address]  DEFAULT ('') FOR [Address]
GO
ALTER TABLE [dbo].[Company] ADD  CONSTRAINT [DF_Company_City]  DEFAULT ('') FOR [City]
GO
ALTER TABLE [dbo].[Company] ADD  CONSTRAINT [DF_Company_State]  DEFAULT ('') FOR [State]
GO
ALTER TABLE [dbo].[Company] ADD  CONSTRAINT [DF_Company_Country]  DEFAULT ('') FOR [Country]
GO
ALTER TABLE [dbo].[Company] ADD  CONSTRAINT [DF_Company_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Company] ADD  CONSTRAINT [DF_Company_CreatedBy]  DEFAULT ((0)) FOR [CreatedBy]
GO
ALTER TABLE [dbo].[Company] ADD  CONSTRAINT [DF_Company_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[CompanyMember] ADD  CONSTRAINT [DF_CompanyMember_CompanyId]  DEFAULT ((0)) FOR [CompanyId]
GO
ALTER TABLE [dbo].[CompanyMember] ADD  CONSTRAINT [DF_CompanyMember_UserId]  DEFAULT ((0)) FOR [UserId]
GO
ALTER TABLE [dbo].[CompanyMember] ADD  CONSTRAINT [DF_CompanyMember_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[CompanyMember] ADD  CONSTRAINT [DF_CompanyMember_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Event] ADD  CONSTRAINT [DF_Event_CompanyId]  DEFAULT ((0)) FOR [CompanyId]
GO
ALTER TABLE [dbo].[Event] ADD  CONSTRAINT [DF_Event_Name]  DEFAULT ('') FOR [Name]
GO
ALTER TABLE [dbo].[Event] ADD  CONSTRAINT [DF_Event_Description]  DEFAULT ('') FOR [Description]
GO
ALTER TABLE [dbo].[Event] ADD  CONSTRAINT [DF_Event_Vanue]  DEFAULT ('') FOR [Vanue]
GO
ALTER TABLE [dbo].[Event] ADD  CONSTRAINT [DF_Event_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Event] ADD  CONSTRAINT [DF_Event_CreatedBy]  DEFAULT ((0)) FOR [CreatedBy]
GO
ALTER TABLE [dbo].[Event] ADD  CONSTRAINT [DF_Event_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[EventMember] ADD  CONSTRAINT [DF_EventMember_EventId]  DEFAULT ((0)) FOR [EventId]
GO
ALTER TABLE [dbo].[EventMember] ADD  CONSTRAINT [DF_EventMember_UserId]  DEFAULT ((0)) FOR [UserId]
GO
ALTER TABLE [dbo].[EventMember] ADD  CONSTRAINT [DF_EventMember_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[EventMember] ADD  CONSTRAINT [DF_EventMember_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
USE [master]
GO
ALTER DATABASE [Event] SET  READ_WRITE 
GO
