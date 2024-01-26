USE [master]
GO
/****** Object:  Database [AnajPos]    Script Date: 23/01/2024 1:29:06 am ******/
CREATE DATABASE [AnajPos]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'AnajPos', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\AnajPos.mdf' , SIZE = 5312KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'AnajPos_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\AnajPos_log.ldf' , SIZE = 4288KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [AnajPos] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AnajPos].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [AnajPos] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [AnajPos] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [AnajPos] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [AnajPos] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [AnajPos] SET ARITHABORT OFF 
GO
ALTER DATABASE [AnajPos] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [AnajPos] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [AnajPos] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [AnajPos] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [AnajPos] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [AnajPos] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [AnajPos] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [AnajPos] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [AnajPos] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [AnajPos] SET  DISABLE_BROKER 
GO
ALTER DATABASE [AnajPos] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [AnajPos] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [AnajPos] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [AnajPos] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [AnajPos] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [AnajPos] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [AnajPos] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [AnajPos] SET RECOVERY FULL 
GO
ALTER DATABASE [AnajPos] SET  MULTI_USER 
GO
ALTER DATABASE [AnajPos] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [AnajPos] SET DB_CHAINING OFF 
GO
ALTER DATABASE [AnajPos] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [AnajPos] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [AnajPos] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [AnajPos] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [AnajPos] SET QUERY_STORE = OFF
GO
USE [AnajPos]
GO
/****** Object:  UserDefinedFunction [dbo].[GetFirstParentAccountId]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetFirstParentAccountId]  
(  
    @AccountId INT  
)  
RETURNS INT  
AS  
BEGIN  
 DECLARE @ParentId INT;

    WITH RecursiveCTE AS (
        SELECT AccountId, ParentId
        FROM tblChartOfAccount
        WHERE AccountId = @AccountId

        UNION ALL

        SELECT c.AccountId, c.ParentId
        FROM tblChartOfAccount c
        INNER JOIN RecursiveCTE r ON c.AccountId = r.ParentId
    )
    SELECT TOP 1 @ParentId = AccountId
    FROM RecursiveCTE
    WHERE ParentId IS NULL
    ORDER BY AccountId;

    RETURN @ParentId;

END;  



GO
/****** Object:  Table [dbo].[tblAddress]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblAddress](
	[AddressID] [int] IDENTITY(100,1) NOT NULL,
	[AddressName] [varchar](200) NULL,
	[AddressNameUrdu] [nvarchar](200) NULL,
	[ZoneID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[AddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblAdjustment]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblAdjustment](
	[AdjustmentId] [int] NOT NULL,
	[TransactionDate] [date] NULL,
	[ZoneId] [int] NULL,
	[AdderssId] [int] NULL,
	[CustomerName] [int] NULL,
	[Dr] [int] NULL,
	[Cr] [int] NULL,
	[Notes] [nvarchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[AdjustmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblBank]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblBank](
	[BankId] [int] IDENTITY(1,1) NOT NULL,
	[BankTitle] [varchar](50) NULL,
	[BankName] [nvarchar](200) NULL,
	[BankNameUrdu] [nvarchar](200) NULL,
	[AccountNo] [varchar](50) NULL,
	[Address] [nvarchar](200) NULL,
	[OpeningDate] [datetime] NULL,
	[OpeningBalance] [money] NULL,
	[IsActive] [bit] NULL,
	[AccountId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[BankId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblCategory]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblCategory](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblChartOfAccount]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblChartOfAccount](
	[AccountId] [int] IDENTITY(1,1) NOT NULL,
	[AccountName] [nvarchar](200) NULL,
	[ParentId] [int] NULL,
	[Description] [varchar](200) NULL,
	[Dr] [money] NULL,
	[Cr] [money] NULL,
	[IsActive] [bit] NULL,
	[IsEditable] [bit] NULL,
	[IsChildable] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblClosing]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblClosing](
	[ClosingDate] [datetime] NULL,
	[CustomerID] [int] NULL,
	[Remark] [nvarchar](200) NULL,
	[Amount] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblCompany]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblCompany](
	[CompanyId] [int] IDENTITY(1,1) NOT NULL,
	[CompanyName] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblCounterSaleOrPurchase]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblCounterSaleOrPurchase](
	[CounterId] [int] IDENTITY(1,1) NOT NULL,
	[InvoiceId] [int] NULL,
	[InvTime] [datetime] NULL,
	[InvDate] [datetime] NULL,
	[ProductId] [int] NULL,
	[Price] [money] NULL,
	[Qty] [decimal](8, 2) NULL,
	[Remark] [varchar](255) NULL,
	[Weight] [decimal](18, 2) NULL,
	[Discount] [decimal](18, 2) NULL,
	[WarehouseId] [int] NULL,
	[IsDeleted] [bit] NULL,
	[Kg] [decimal](18, 0) NULL,
	[PaymentType] [int] NULL,
	[CounterNo] [int] NULL,
	[Isledger] [bit] NULL,
	[Rent] [int] NULL,
	[Remarks] [nvarchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[CounterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblCounterSaleReturn]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblCounterSaleReturn](
	[ReturnId] [int] IDENTITY(1,1) NOT NULL,
	[ReturnInvoiceId] [int] NULL,
	[CounterInvoiceId] [int] NULL,
	[ReturnDate] [datetime] NULL,
	[ProductId] [int] NULL,
	[Price] [money] NULL,
	[Qty] [int] NULL,
	[Weight] [decimal](18, 0) NULL,
	[Remark] [nvarchar](20) NULL,
	[Discount] [int] NULL,
	[Rent] [int] NULL,
	[Warehouse] [int] NULL,
	[IsDeleted] [bit] NULL,
	[Kg] [int] NULL,
	[PaymentType] [int] NULL,
	[CounterNo] [int] NULL,
	[IsLedger] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ReturnId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblCustomer]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblCustomer](
	[CustomerID] [int] NOT NULL,
	[CustomerName] [varchar](200) NULL,
	[CustomerNameUrdu] [nvarchar](200) NULL,
	[ZoneID] [int] NULL,
	[AddressId] [int] NULL,
	[ProperAddress] [varchar](200) NULL,
	[Picture] [image] NULL,
	[Phone1] [varchar](20) NULL,
	[Phone2] [varchar](20) NULL,
	[Telephone] [varchar](200) NULL,
	[Remark] [varchar](200) NULL,
	[ClosingDate] [date] NULL,
	[OpeningAmount] [int] NULL,
	[IsActice] [bit] NULL,
	[AccountId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblExpense]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblExpense](
	[ExpId] [int] IDENTITY(1,1) NOT NULL,
	[ExpDate] [datetime] NULL,
	[ExpTypeChartOfAccountId] [int] NULL,
	[PaymentModeChartOfAccountId] [int] NULL,
	[Amount] [money] NULL,
	[Notes] [nvarchar](200) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifyDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ExpId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblHeadOffAcc]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblHeadOffAcc](
	[HeadId] [int] NOT NULL,
	[HeadOfAcc] [varchar](200) NULL,
	[HeadOfAccName] [nvarchar](200) NULL,
	[Remark] [varchar](200) NULL,
	[OpeningDate] [datetime] NULL,
	[OpeningAmount] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[HeadId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblInventoryTransfer]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblInventoryTransfer](
	[InventoryTransferId] [int] IDENTITY(1,1) NOT NULL,
	[TransferDate] [datetime] NULL,
	[ProductId] [int] NULL,
	[FromLocationId] [int] NULL,
	[ToLocationId] [int] NULL,
	[transferQty] [decimal](18, 2) NULL,
	[Notes] [varchar](250) NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[InventoryTransferId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblInvoice]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblInvoice](
	[Sid] [int] NOT NULL,
	[SDate] [date] NULL,
	[ZoneId] [int] NULL,
	[AreaId] [int] NULL,
	[CustomerId] [int] NULL,
	[PageNo] [varchar](200) NULL,
	[Bilty] [varchar](200) NULL,
	[Transport] [nvarchar](200) NULL,
	[Remark] [nvarchar](200) NULL,
	[OrderDate] [datetime] NULL,
	[GrandTotal] [int] NULL,
	[PostingDate] [datetime] NULL,
	[PreviousAmount] [int] NULL,
	[NewAmount] [int] NULL,
	[PostingType] [varchar](200) NULL,
	[Rent] [int] NULL,
	[PerBagCharge] [int] NULL,
	[LabourCharges] [int] NULL,
	[TotalAmount] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Sid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblLedger]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblLedger](
	[LedgerId] [int] IDENTITY(1,1) NOT NULL,
	[TransactionDate] [datetime] NULL,
	[TransactionType] [nvarchar](50) NULL,
	[TransactionID] [int] NULL,
	[ChartOfAccountId] [int] NULL,
	[Remark] [nvarchar](max) NULL,
	[Dr] [money] NULL,
	[Cr] [money] NULL,
	[Description] [nvarchar](max) NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[LedgerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblPayment]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblPayment](
	[PayId] [int] NOT NULL,
	[PayDate] [date] NULL,
	[VendorID] [int] NULL,
	[HeadID] [int] NULL,
	[Remark] [nvarchar](200) NULL,
	[Amount] [int] NULL,
	[PreviousBalance] [int] NULL,
	[EntryDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[PayId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblProduct]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblProduct](
	[ProductID] [int] NOT NULL,
	[ProductName] [varchar](200) NULL,
	[ProductNameUrdu] [nvarchar](200) NULL,
	[Company] [varchar](200) NULL,
	[ProductCetegory] [varchar](200) NULL,
	[Pakaging] [int] NULL,
	[PurchaseRate] [int] NULL,
	[SaleRate] [int] NULL,
	[OpeningAmount] [int] NULL,
	[Remark] [nvarchar](200) NULL,
	[WarehouseId] [int] NULL,
	[PurchaseUnit] [int] NULL,
	[SaleUnit] [int] NULL,
	[MeasurementProduct] [int] NULL,
	[OpeningStock] [float] NULL,
	[ChartOfAccountId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblProductTransaction]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblProductTransaction](
	[InvoiceId] [int] NULL,
	[InvoiceDate] [datetime] NULL,
	[CustomerId] [int] NULL,
	[ProductId] [int] NULL,
	[Qty] [int] NULL,
	[ProductName] [varchar](200) NULL,
	[Kg] [float] NULL,
	[Weight] [float] NULL,
	[Price] [float] NULL,
	[Amount] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblProductTransactionReturn]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblProductTransactionReturn](
	[TransactionId] [int] IDENTITY(1,1) NOT NULL,
	[ReturnId] [int] NULL,
	[ReturnDate] [datetime] NULL,
	[CustomerVendorAccountId] [int] NULL,
	[ProductId] [int] NULL,
	[Qty] [int] NULL,
	[ProductName] [varchar](200) NULL,
	[Kg] [float] NULL,
	[Weight] [float] NULL,
	[Price] [money] NULL,
	[Amount] [money] NULL,
	[TransactionType] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblProductTransactionVendor]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblProductTransactionVendor](
	[PurchaseId] [int] NULL,
	[PurchaseDate] [datetime] NULL,
	[VendorId] [int] NULL,
	[ProductId] [int] NULL,
	[Qty] [int] NULL,
	[ProductName] [varchar](200) NULL,
	[Kg] [float] NULL,
	[Weight] [float] NULL,
	[Price] [float] NULL,
	[Amount] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblPurchase]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblPurchase](
	[PId] [int] NULL,
	[PDate] [date] NULL,
	[ZoneId] [int] NULL,
	[AreaId] [int] NULL,
	[VendorId] [int] NULL,
	[PageNo] [int] NULL,
	[Bilty] [varchar](200) NULL,
	[Transport] [varchar](200) NULL,
	[Remark] [nvarchar](200) NULL,
	[OrderDate] [datetime] NULL,
	[GrandTotal] [int] NULL,
	[PostingDate] [datetime] NULL,
	[PreviousAmount] [int] NULL,
	[NewAmount] [int] NULL,
	[PostingType] [varchar](200) NULL,
	[Rent] [int] NULL,
	[PerBag] [int] NULL,
	[LabourCharges] [int] NULL,
	[TotalAmount] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblPurchaseR]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblPurchaseR](
	[ReturnId] [int] NOT NULL,
	[VendorId] [int] NULL,
	[ReturnDate] [date] NULL,
	[Remark] [nvarchar](200) NULL,
	[Amount] [int] NULL,
	[IsLedger] [bit] NULL,
	[ZoneId] [int] NULL,
	[Area] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ReturnId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblReceipt]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblReceipt](
	[RecId] [int] NOT NULL,
	[RecDate] [date] NULL,
	[Zone] [int] NULL,
	[Area] [int] NULL,
	[CustomerID] [int] NULL,
	[HeadOfAcc] [int] NULL,
	[AmountInEnglish] [varchar](200) NULL,
	[Remark] [nvarchar](200) NULL,
	[Amount] [int] NULL,
	[PreviousAmount] [int] NULL,
	[LastBill] [varchar](200) NULL,
	[LastReceipt] [varchar](200) NULL,
	[CurrentBalance] [int] NULL,
	[EntryDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[RecId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblReceiptToPaid]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblReceiptToPaid](
	[Id] [int] NOT NULL,
	[CZone] [int] NULL,
	[CArea] [int] NULL,
	[CustomerId] [int] NULL,
	[CRemark] [varchar](200) NULL,
	[VZone] [int] NULL,
	[VArea] [int] NULL,
	[VendorId] [int] NULL,
	[VRemark] [varchar](200) NULL,
	[Amount] [int] NULL,
	[Transdate] [datetime] NULL,
	[CustomerPrevBalance] [int] NULL,
	[VendorPrevBalance] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSaleR]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSaleR](
	[ReturnId] [int] NULL,
	[Zoneid] [int] NULL,
	[Addressid] [int] NULL,
	[CustomerId] [int] NULL,
	[ReturnDate] [datetime] NULL,
	[Remark] [nvarchar](200) NULL,
	[Amount] [int] NULL,
	[IsLedger] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblStock]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblStock](
	[StockID] [int] IDENTITY(1,1) NOT NULL,
	[ProductTransDate] [datetime] NULL,
	[ProductTransId] [int] NULL,
	[ProductID] [int] NULL,
	[UnitID] [int] NULL,
	[TransactionType] [nvarchar](50) NULL,
	[WarehouseID] [int] NULL,
	[StockIn] [decimal](9, 2) NULL,
	[StockOut] [decimal](9, 2) NULL,
	[Amount] [money] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[StockID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblStockAdjustments]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblStockAdjustments](
	[StockAdjID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NULL,
	[WarehouseID] [int] NULL,
	[StockIn] [decimal](9, 2) NULL,
	[StockOut] [decimal](9, 2) NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[StockAdjID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblTransaction]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblTransaction](
	[TransactionDate] [date] NULL,
	[TranType] [varchar](200) NULL,
	[TranID] [int] NULL,
	[CustomerName] [int] NULL,
	[Remark] [nvarchar](200) NULL,
	[Dr] [int] NULL,
	[Cr] [int] NULL,
	[PostingDate] [datetime] NULL,
	[TranTypeUrdu] [nvarchar](200) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblTransactionVendor]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblTransactionVendor](
	[TransactionDate] [date] NULL,
	[LedgerType] [nvarchar](200) NULL,
	[TranId] [int] NULL,
	[VendorId] [int] NULL,
	[Remark] [varchar](200) NULL,
	[Dr] [int] NULL,
	[Cr] [int] NULL,
	[PostingDate] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblUnit]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblUnit](
	[UnitId] [int] IDENTITY(1,1) NOT NULL,
	[UnitName] [varchar](50) NULL,
	[UnitNameUrdu] [nvarchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[UnitId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblVendor]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblVendor](
	[VendorID] [int] NULL,
	[VendorName] [varchar](200) NULL,
	[VendorAddress] [int] NULL,
	[ProperAddress] [nvarchar](200) NULL,
	[Picture] [image] NULL,
	[Phone1] [varchar](20) NULL,
	[Phone2] [varchar](20) NULL,
	[Telephone] [varchar](20) NULL,
	[Remark] [varchar](20) NULL,
	[ClosingDate] [date] NULL,
	[OpeningAmount] [int] NULL,
	[IsActive] [bit] NULL,
	[AccountId] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblVendorAdjustment]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblVendorAdjustment](
	[AdjustmentId] [int] NOT NULL,
	[TransactionDate] [date] NULL,
	[VendorID] [int] NULL,
	[Dr] [int] NULL,
	[Cr] [int] NULL,
	[Notes] [nvarchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[AdjustmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblWarehouse]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblWarehouse](
	[WarehouseId] [int] IDENTITY(1,1) NOT NULL,
	[WarehouseName] [nvarchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[WarehouseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblZone]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblZone](
	[ZoneId] [int] IDENTITY(1,1) NOT NULL,
	[ZoneName] [varchar](200) NULL,
	[ZoneNameUrdu] [nvarchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[ZoneId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tblAddress]  WITH CHECK ADD FOREIGN KEY([ZoneID])
REFERENCES [dbo].[tblZone] ([ZoneId])
GO
ALTER TABLE [dbo].[tblAddress]  WITH CHECK ADD FOREIGN KEY([ZoneID])
REFERENCES [dbo].[tblZone] ([ZoneId])
GO
ALTER TABLE [dbo].[tblCustomer]  WITH CHECK ADD FOREIGN KEY([AddressId])
REFERENCES [dbo].[tblAddress] ([AddressID])
GO
ALTER TABLE [dbo].[tblCustomer]  WITH CHECK ADD FOREIGN KEY([AddressId])
REFERENCES [dbo].[tblAddress] ([AddressID])
GO
ALTER TABLE [dbo].[tblCustomer]  WITH CHECK ADD FOREIGN KEY([ZoneID])
REFERENCES [dbo].[tblZone] ([ZoneId])
GO
ALTER TABLE [dbo].[tblCustomer]  WITH CHECK ADD FOREIGN KEY([ZoneID])
REFERENCES [dbo].[tblZone] ([ZoneId])
GO
/****** Object:  StoredProcedure [dbo].[CRM_GetLedger_sp]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CRM_GetLedger_sp]
(
    @AccountID INT,
    @StartDate DATETIME,
    @EndDate DATETIME
)
AS
BEGIN
    DECLARE @ParentId AS INT
    SET @ParentId = dbo.GetFirstParentAccountId(@AccountID);

    WITH OpeningBalance AS (
        SELECT 
            0 AS LedgerTransID,
            0 AS VoucherID,
            CAST(@StartDate AS Date) AS TransDate,
            'Opening Balance' AS TransactionType,
            'Opening Balance' AS Description,
            ISNULL(SUM(t.Dr), 0) AS TotalDr,
            ISNULL(SUM(t.Cr), 0) AS TotalCr
        FROM 
            [tblLedger] t
        WHERE
            t.ChartOfAccountId = @AccountID 
            AND t.TransactionDate BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)
       
    ),
    LedgerData AS (
        SELECT 
            0 AS LedgerTransID,
            0 AS VoucherID,
            @StartDate AS TranDate,
            'Opening Balance' AS TransactionType,
            'Opening Balance' AS Description,
            0 AS DR,
            0 AS CR 
        WHERE 
            NOT EXISTS (SELECT 1 FROM OpeningBalance)

        UNION ALL

        SELECT 
            LedgerId,
            ISNULL(TransactionID, 0) AS VoucherID,
            CAST(TransactionDate AS DATE) AS TranDate,
            ISNULL(TransactionType, '') AS TransactionType,
            ISNULL('Description will be set', '') AS Description,
            ISNULL(t.Dr, 0) AS DR,
            ISNULL(t.Cr, 0) AS CR
        FROM 
            [tblLedger] t
        INNER JOIN 
            dbo.[tblChartOfAccount] c ON c.AccountId = t.ChartOfAccountId
        WHERE 
            t.ChartOfAccountId = @AccountID 
            AND t.TransactionDate BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)
    ),
    CalculateLedgerData AS (
        SELECT
            ROW_NUMBER() OVER (ORDER BY LedgerTransID) AS SNO,
            *,
            SUM(
                CASE 
                    WHEN @ParentId IN (1, 2) THEN ISNULL(DR, 0) - ISNULL(CR, 0)
                    ELSE ISNULL(CR, 0) - ISNULL(DR, 0)
                END
            ) OVER (ORDER BY LedgerTransID, TranDate) AS Balance
        FROM
            LedgerData
    )
    SELECT
        SNO,
        TranDate AS TranDate,
        VoucherID,
        TransactionType,
        Description,
        CASE 
            WHEN TransactionType = 'Opening Balance' THEN 0 
            ELSE DR 
        END AS DR,
        CASE 
            WHEN TransactionType = 'Opening Balance' THEN 0 
            ELSE CR 
        END AS CR,
        Balance
    FROM
        CalculateLedgerData
END;
GO
/****** Object:  StoredProcedure [dbo].[InsertAdjustment]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[InsertAdjustment]
(

	@id int,
	@TransactionDate datetime,
	@ZoneId int,
	@AdderssId int,
	@CustomerName int,
	@Dr int,
	@Cr int,
	@Notes nvarchar(200)	
)
as
	begin
		insert into tblAdjustment values (@id, @TransactionDate,@ZoneId,@AdderssId,@CustomerName,@Dr,@Cr,@Notes) 
	end
GO
/****** Object:  StoredProcedure [dbo].[InsertUpdateExpense]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
    
CREATE proc [dbo].[InsertUpdateExpense]      
 @ExpenseId int,      
 @ExpDate datetime,    
 @ExpenseType int,      
 @PaymentMode int,      
 @Amount money,      
 @Notes nvarchar(200)      
as      
begin      
 if @ExpenseId > 0      
 begin      
  update tblExpense set     
   ExpDate = @ExpDate,    
   ExpTypeChartOfAccountId = @ExpenseType,      
         PaymentModeChartOfAccountId= @PaymentMode,      
         Amount = @Amount,      
         Notes = @Notes ,    
   ModifyDate = GETDATE()    
  where ExpId = @ExpenseId    
    
  update tblLedger set TransactionDate = @ExpDate,Remark = @Notes,Dr = 0 , Cr = @Amount 
  where TransactionID = @ExpenseId and TransactionType = 'Create Expense' and ChartOfAccountId = @PaymentMode  
  
  update tblLedger set TransactionDate = @ExpDate,Remark = @Notes, Dr=@Amount,Cr = 0  
  where TransactionID = @ExpenseId and TransactionType = 'Create Expense' and ChartOfAccountId = @ExpenseType  
  
 end      
 else      
 begin      
  insert into tblExpense (ExpDate,ExpTypeChartOfAccountId,PaymentModeChartOfAccountId,Amount,Notes,CreatedDate)      
  values      
  (@ExpDate,@ExpenseType,@PaymentMode,@Amount,@Notes,GETDATE())      
  
 declare @InsertedId as int   
 set @InsertedId = @@identity  
   
 insert into tblLedger (TransactionDate,TransactionType,TransactionID,ChartOfAccountId,Remark,Dr,Cr)  
 values  
 (@ExpDate,'Create Expense',@InsertedId,@PaymentMode,@Notes,0,@Amount)  
  
    insert into tblLedger (TransactionDate,TransactionType,TransactionID,ChartOfAccountId,Remark,Dr,Cr)  
 values  
 (@ExpDate,'Create Expense',@InsertedId,@ExpenseType,@Notes,@Amount,0)  
  
  
 end      
end   
  
  
GO
/****** Object:  StoredProcedure [dbo].[InsertUpdateInventoryTransfer]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[InsertUpdateInventoryTransfer]        
 @InventoryTransferId int,        
 @TransferDate datetime,      
 @ProductId int,        
 @fromLocationID int,        
 @ToLocationID int,    
 @TransferQty decimal,        
 @Notes nvarchar(200)  
as        
begin        
 if @InventoryTransferId > 0        
 begin        
  update tblInventoryTransfer set       
    TransferDate = @TransferDate , ProductId=@ProductId, fromLocationID =  @fromLocationID , ToLocationID=@ToLocationID ,TransferQty =@TransferQty , Notes=@Notes    
 where  InventoryTransferId = @InventoryTransferId 
 
 update tblStock set ProductTransDate = @TransferDate,ProductID = @ProductId,UnitID= 1,WarehouseID = @fromLocationID , StockIn =0, StockOut = 0,
 Amount = 0,IsDeleted = 0
 where ProductTransId = 1 and TransactionType = ''

  update tblStock set ProductTransDate = @TransferDate,ProductID = @ProductId,UnitID= 1,WarehouseID = @fromLocationID ,
 StockOut = @TransferQty,
 Amount = 0,IsDeleted = 0
 where ProductTransId = @InventoryTransferId  and TransactionType = 'Inventory Transfer' and StockIn = 0

 update tblStock set ProductTransDate = @TransferDate,ProductID = @ProductId,UnitID= 1,WarehouseID = @ToLocationID ,
 StockIn = @TransferQty,
 Amount = 0,IsDeleted = 0
 where ProductTransId = @InventoryTransferId  and TransactionType = 'Inventory Transfer' and StockOut = 0

 end        
 else        
 begin        
  insert into tblInventoryTransfer (TransferDate,ProductId,FromLocationID,ToLocationID,TransferQty,Notes,CreatedDate,IsDeleted)        
  values        
  (@TransferDate,@ProductId,@FromLocationID,@ToLocationID,@TransferQty,@Notes,GETDATE(),0)        
   
 declare @InsertedId int  
 set @InsertedId = @@IDENTITY  
   insert into tblStock values (@TransferDate,@InsertedId,@ProductID,1,'Inventory Transfer',@fromLocationID,0,@TransferQty,0,0)    
   
   insert into tblStock values (@TransferDate,@InsertedId,@ProductID,1,'Inventory Transfer',@ToLocationID,@TransferQty,0,0,0)    
  
 end  
end


GO
/****** Object:  StoredProcedure [dbo].[UpdaterLedger]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[UpdaterLedger]
(
	@TransactionDate DATETIME,  
    @TransactionType NVARCHAR(50),  
    @TransactionID INT,  
    @ChartOfAccountId INT,  
    @Remark NVARCHAR(MAX),  
    @Dr MONEY,  
    @Cr MONEY  

)
as
begin
	update tblLedger set TransactionDate = @TransactionDate,
	Remark = @Remark , Dr = @Dr, Cr = @Cr
	where TransactionID = @TransactionID and ChartOfAccountId = @ChartOfAccountId and TransactionType = @TransactionType
end
GO
/****** Object:  StoredProcedure [dbo].[usp_AdjustmentbyDate]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_AdjustmentbyDate]
(@date date)
as
begin
select tblVendorAdjustment.AdjustmentId,tblVendorAdjustment.TransactionDate,tblVendor.VendorName,tblVendorAdjustment.Notes,tblVendorAdjustment.Dr,tblVendorAdjustment.Cr from tblVendorAdjustment
inner join tblVendor on tblVendor.VendorID=tblVendorAdjustment.VendorID
where TransactionDate=@date
end
GO
/****** Object:  StoredProcedure [dbo].[usp_AdjustmentbyId]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_AdjustmentbyId]
(@id int)
as
begin
select tblVendorAdjustment.AdjustmentId,tblVendorAdjustment.TransactionDate,tblZone.ZoneId,tblAddress.AddressID,tblVendorAdjustment.VendorID,tblVendorAdjustment.Dr,tblVendorAdjustment.Cr,tblVendorAdjustment.Notes from tblVendorAdjustment
inner join tblVendor on tblVendorAdjustment.VendorID=tblVendor.VendorID
inner join tblAddress on tblVendor.VendorAddress=tblAddress.AddressID
inner join tblZone on tblAddress.ZoneID=tblZone.ZoneID
where AdjustmentId=@id
end
GO
/****** Object:  StoredProcedure [dbo].[usp_AllVendorByID]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_AllVendorByID]
(@id int)
as
begin
select VendorID,VendorName,tblZone.ZoneId,VendorAddress,ProperAddress,Picture,Phone1,Phone2,Telephone,Remark,ClosingDate,OpeningAmount,IsActive from tblVendor inner join 
tblAddress on tblAddress.AddressID=tblVendor.VendorAddress
inner join
tblZone on tblZone.ZoneId=tblAddress.ZoneID
where tblVendor.VendorID=@id
end
GO
/****** Object:  StoredProcedure [dbo].[usp_BankAccountId]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 create proc [dbo].[usp_BankAccountId]    
 (    
 @id int    
)    
 as    
  begin    
   select AccountId from tblBank  
where BankId =@id    
  end
GO
/****** Object:  StoredProcedure [dbo].[Usp_ChartOfAccountDropDown_sp]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[Usp_ChartOfAccountDropDown_sp]  
As  
BEGIN  
--WITH RecursiveCTE AS (  
--    SELECT AccountId, AccountName, ParentId  
--    FROM tblChartOfAccount  
--    WHERE ParentId IS NULL  
  
--    UNION ALL  
  
--    SELECT c.AccountId, c.AccountName, c.ParentId  
--    FROM tblChartOfAccount c  
--    INNER JOIN RecursiveCTE r ON c.ParentId = r.AccountId  
--)  
--SELECT *  
--FROM RecursiveCTE  
--WHERE AccountId NOT IN (SELECT ParentId FROM tblChartOfAccount WHERE ParentId IS NOT NULL)  
--ORDER BY AccountId;
select AccountId,AccountName,ParentId from tblChartOfAccount
END  
  
  
  
GO
/****** Object:  StoredProcedure [dbo].[usp_ClosingLedgerEnglish]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE proc [dbo].[usp_ClosingLedgerEnglish] --1,'10-dec-2023'    
(     
 @CustomerId int,    
 @ClosingDate date    
)    
as     
 begin   
   SELECT  
        CustomerName = tblCustomer.CustomerName,  
        TranDate = NULL,  
        TranType = 'Closing',  
        TranId = NULL,  
        Remark = 'Closing amount',  
        Dr = 0,  
        Cr = 0,  
        Balance = SUM(Cr) - SUM(Dr)    
    FROM  
        tblTransaction  
    LEFT JOIN tblCustomer ON tblCustomer.CustomerID = tblTransaction.CustomerName  
    WHERE  
        tblTransaction.CustomerName = @CustomerId  
        AND tblTransaction.TransactionDate <= @ClosingDate  
    GROUP BY  
        tblCustomer.CustomerName  -- Add this line to handle aggregation  
  
-- select CustomerName = null,TranDate =null, TranType = 'Closing',TranId = null,Remark = 'Closing amount' , Dr =0,Cr=0,   sum(tblTransaction.Cr)-sum(tblTransaction.Dr) as 'Balance' from tblTransaction where tblTransaction.CustomerName =@CustomerId and tblTransaction.TransactionDate <= @ClosingDate    
union    
select tblCustomer.CustomerName, TransactionDate,TranType,TranID,tblTransaction.Remark,Dr,Cr,Cr-Dr as 'Balance' from tblTransaction inner join tblCustomer on tblCustomer.CustomerID=tblTransaction.CustomerName where  tblTransaction.CustomerName =@CustomerId and tblTransaction.TransactionDate > @ClosingDate     
 end
GO
/****** Object:  StoredProcedure [dbo].[usp_CustomeBalance]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 create proc [dbo].[usp_CustomeBalance]
 (
	@id int
)
	as
		begin
			select SUM(tblTransaction.Cr)-SUM(tblTransaction.Dr) from tblTransaction
where CustomerName =@id
		end
GO
/****** Object:  StoredProcedure [dbo].[usp_CustomerAccountId]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 create proc [dbo].[usp_CustomerAccountId]  
 (  
 @id int  
)  
 as  
  begin  
   select AccountId from tblCustomer
where CustomerID =@id  
  end
GO
/****** Object:  StoredProcedure [dbo].[usp_CustomerNewId]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_CustomerNewId]
as
	begin
	select MAX(CustomerId) from tblCustomer
	end
GO
/****** Object:  StoredProcedure [dbo].[usp_DeleteLedger]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_DeleteLedger]
(
	@TransactionType varchar(200),
	@TransactionId int,
	@ChartOfAccountId int
)
as
begin
	update tblLedger set IsDeleted = 1 
	where TransactionType = @TransactionType and TransactionID = @TransactionId --and ChartOfAccountId = @ChartOfAccountId 
end
GO
/****** Object:  StoredProcedure [dbo].[usp_DeleteStock]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_DeleteStock]    
(    
   
 @ProductTransId int,    
 @ProductID int,
 @TransactionType varchar(200)
)    
as    
begin    
 update tblStock set   
  IsDeleted = 1  
 where ProductTransId = @ProductTransId and ProductID = @ProductID  and TransactionType = @TransactionType
end    
GO
/****** Object:  StoredProcedure [dbo].[usp_DisplayCustomer]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_DisplayCustomer]
as 
begin
select tblCustomer.CustomerID as 'ID',tblCustomer.CustomerName as 'Client',
tblZone.ZoneName as 'Area',tblAddress.AddressName as 'Address',COALESCE (Phone1,Phone2,Telephone) as 'Contact',IsActice as 'Active' from 
tblCustomer inner join tblZone on tblCustomer.ZoneID=tblZone.ZoneId
inner join tblAddress on tblCustomer.AddressId=tblAddress.AddressID
end

GO
/****** Object:  StoredProcedure [dbo].[usp_DisplayVendor]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_DisplayVendor]
as
	begin
select VendorID AS 'ID',VendorName as 'Vendor',tblZone.ZoneName as 'Area',tblAddress.AddressName as 'Address',coalesce(tblVendor.Phone1,tblVendor.Phone2,tblVendor.Telephone) as 'Contact',tblVendor.IsActive from tblVendor
inner join tblAddress on tblAddress.AddressID=tblVendor.VendorAddress
inner join tblZone on tblZone.ZoneId=tblAddress.ZoneId
	end
GO
/****** Object:  StoredProcedure [dbo].[usp_GatePasProduct]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_GatePasProduct]
(@id int)
as
	begin
select Qty,tblProduct.ProductNameUrdu,Kg,Price,[Weight],Amount from tblProductTransaction inner join tblProduct on tblProduct.ProductID=tblProductTransaction.ProductId where InvoiceId = @id
	end
GO
/****** Object:  StoredProcedure [dbo].[usp_GetBankAccountid]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_GetBankAccountid]
(
	@BankId int
)
as
begin
	select Isnull(AccountId,0) from tblBank where BankId = @BankId
end
GO
/****** Object:  StoredProcedure [dbo].[usp_GetCategory]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_GetCategory] 
as  
begin  
 select * from tblCategory  
end  

GO
/****** Object:  StoredProcedure [dbo].[usp_GetChartOfAccount]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_GetChartOfAccount]
(
	@ParentId int
)
as  
begin  
 select AccountId,AccountName,Dr,Cr from tblChartOfAccount where ParentId = @ParentId  
end  
GO
/****** Object:  StoredProcedure [dbo].[usp_GetChartOfAccountExpense]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_GetChartOfAccountExpense]
(
	@ParentId int
)
as  
begin  
 select AccountId,AccountName,Dr,Cr from tblChartOfAccount where ParentId = @ParentId  
end 
GO
/****** Object:  StoredProcedure [dbo].[usp_GetCompany]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_GetCompany] 
as  
begin  
 select * from tblCompany  
end  

GO
/****** Object:  StoredProcedure [dbo].[usp_GetLedger_sp]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetLedger_sp]  --4041,'07-jan-2024','07-jan-2024'             
    @AccountID INT,              
    @StartDate DATETIME,              
    @EndDate DATETIME              
AS              
BEGIN              
    DECLARE @ParentId AS INT              
    SET @ParentId = dbo.GetFirstParentAccountId(@AccountID);              
              
    WITH OpeningBalance AS              
    (              
          SELECT              
            c.AccountId AS LedgerId,              
            ISNULL(0, 0) AS VoucherID,              
            CAST(@StartDate AS DATE) AS TranDate,              
            ISNULL('Opening Balance', '') AS TransactionType,              
            ISNULL('Opening Balance', '') AS Description, 
			''  as 'Remark',             
            CASE              
                WHEN @ParentId IN (1, 2) THEN SUM(ISNULL(t.Dr, 0)) - SUM(ISNULL(t.Cr, 0))              
                ELSE 0              
            END AS DR,              
   CASE              
                WHEN @ParentId IN (4,5,3) THEN SUM(ISNULL(t.Cr, 0)) - SUM(ISNULL(t.Dr, 0))              
                ELSE 0              
            END AS CR 
			           
              
        FROM              
            [tblLedger] t              
        INNER JOIN              
            dbo.[tblChartOfAccount] c ON c.AccountId = t.ChartOfAccountId              
        WHERE              
            t.ChartOfAccountId = @AccountID              
            AND  t.TransactionDate < @StartDate and ISNULL(t.IsDeleted,0) = 0            
        GROUP BY              
            c.AccountId               
    ),              
    LedgerData AS              
    (          
  select * from OpeningBalance          
  union all          
        SELECT              
            c.AccountId AS LedgerId,              
            ISNULL(TransactionID, 0) AS VoucherID,              
            CAST(TransactionDate AS DATE) AS TranDate,              
            ISNULL(TransactionType, '') AS TransactionType,              
            ISNULL(t.Description, '') AS Description,
			t.Remark  as 'Remark',              
            ISNULL(t.Dr, 0) AS DR,              
            ISNULL(t.Cr, 0) AS CR              
        FROM              
            [tblLedger] t              
        INNER JOIN              
            dbo.[tblChartOfAccount] c ON c.AccountId = t.ChartOfAccountId              
        WHERE              
            t.ChartOfAccountId = @AccountID              
            AND t.TransactionDate >= DATEADD(DAY,1, CAST(@StartDate AS DATE)) AND t.TransactionDate <= DATEADD(DAY, 1, CAST(@EndDate AS DATE))              
			and ISNULL(t.IsDeleted,0) = 0
    ),              
    CalculateLedgerData AS              
    (              
        SELECT              
            ROW_NUMBER() OVER (ORDER BY TranDate) AS SNO,              
            *,              
            SUM(              
                CASE              
                    WHEN @ParentId IN (1, 2) THEN ISNULL(DR, 0) - ISNULL(CR, 0)              
                    ELSE ISNULL(CR, 0) - ISNULL(DR, 0)              
                END              
            ) OVER (ORDER BY TranDate ROWS UNBOUNDED PRECEDING) AS Balance              
        FROM              
            LedgerData              
    )              
              
    SELECT              
        SNO,              
        FORMAT(TranDate,'dd-MMM-yyyy') AS TranDate,              
        --CASE              
        --    WHEN TransactionType = 'Create Customer Account' THEN 'Account No ' + CAST(VoucherID AS NVARCHAR(255))              
        --    WHEN TransactionType = 'Create Sale Invoice' THEN 'S-Inv-00' + CAST(VoucherID AS NVARCHAR(255))              
        --    WHEN TransactionType = 'Customer Receiving Voucher' THEN 'CRV-00' + CAST(VoucherID AS NVARCHAR(255))        
        --    WHEN TransactionType = 'Chart of Account Opening Balance' THEN 'Account No ' + CAST(VoucherID AS NVARCHAR(255))              
        --    WHEN TransactionType = 'Create Vendor Account' THEN 'Account No ' + CAST(VoucherID AS NVARCHAR(255))              
        --    WHEN TransactionType = 'Create Purchase Invoice' THEN 'P-Inv-00' + CAST(VoucherID AS NVARCHAR(255))              
        --    ELSE TransactionType + CAST(VoucherID AS NVARCHAR(255))              
        --END AS 'Voucher No',              
        ----VoucherID,
		case when TransactionType = 'Opening Balance' then ''
		when TransactionType = 'Counter Sale'then 'S-Rcpt-' + cast(VoucherID as varchar(30))
		when TransactionType = 'Create Expense' then 'Exp-' + cast(VoucherID as varchar(30))
		when TransactionType = 'Create Purchase Invoice' then 'P-Inv-'  + cast(VoucherID as varchar(30))
		when TransactionType = 'Create Vendor Account' then '' 
		when TransactionType = 'Vendor Payment Voucher' then 'PV-'  + cast(VoucherID as varchar(30))
		when TransactionType = 'Create Customer Account' then '' 
		when TransactionType = 'Create Sale Invoice' then 'S-Inv-'  + cast(VoucherID as varchar(30))
		when TransactionType = 'Customer Reveiving Voucher' then 'RV-'  + cast(VoucherID as varchar(30))
		when TransactionType = 'Create Purchase Return' then 'PR-'  + cast(VoucherID as varchar(30))		
	    else TransactionType + ' ' + CAST(VoucherID as nvarchar(50)) end as 'ID',  
        STUFF(              
            (              
      SELECT ', ' + CASE              
                        WHEN cast(CalculateLedgerData.DR as int) > 0 THEN c.AccountName + ' has been credit'            
                        
						WHEN cast(CalculateLedgerData.CR as int) > 0 THEN  +  c.AccountName  + ' has been debit'                       
                   END             
                FROM tblLedger l              
                    INNER JOIN tblChartOfAccount c ON l.ChartOfAccountId = c.AccountId              
                WHERE l.TransactionID = CalculateLedgerData.VoucherID              
                      AND l.TransactionType = CalculateLedgerData.TransactionType and l.ChartOfAccountId = CalculateLedgerData.LedgerId            
                FOR XML PATH('')              
            ), 1, 2, ''              
        ) AS Description,
		 CalculateLedgerData.Remark,            
        CASE              
            WHEN TransactionType = 'Opening Balance' THEN 0              
            ELSE CAST(DR AS INT)              
        END AS DR,              
        CASE              
            WHEN TransactionType = 'Opening Balance' THEN 0              
            ELSE CAST(CR AS INT)              
        END AS CR,              
        CAST(Balance AS INT) AS Balance              
    FROM              
        CalculateLedgerData;              
 --select * from CalculateLedgerData              
END; 
GO
/****** Object:  StoredProcedure [dbo].[usp_GetPaymentMethod]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_GetPaymentMethod]
as
begin
	select * from tblChartOfAccount where ParentId in (22,13)
	order by 3 
end
GO
/****** Object:  StoredProcedure [dbo].[usp_GetPendingCounterSale]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_GetPendingCounterSale]
as
begin
WITH RankedRemarks AS (
  SELECT
    InvoiceId,
    InvDate,
    Remark,
    (SUM(Price) * SUM(Qty)) AS 'Amount',
    ROW_NUMBER() OVER (PARTITION BY InvoiceId, InvDate ORDER BY Remark) AS RowNum
  FROM
    tblCounterSaleOrPurchase
  WHERE
    Isledger = 0 and IsDeleted = 0
  GROUP BY
    InvoiceId, InvDate, Remark
)

SELECT
  InvoiceId,
  InvDate,
  Remark,
  Cast(Amount as int) as 'Amount'
FROM
  RankedRemarks
WHERE
  RowNum = 1;
end

GO
/****** Object:  StoredProcedure [dbo].[usp_GetStock]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetStock]    
    @WarehouseId INT    
AS    
BEGIN    
    SELECT DISTINCT    
        s.ProductID,    
        p.ProductName,    
        p.Pakaging,    
        w.WarehouseName,    
        SUM(CASE WHEN s.WarehouseID = @WarehouseId OR @WarehouseId = 0 and IsDeleted = 0 THEN StockIn - StockOut ELSE 0 END) AS 'Stock'    
    FROM    
        tblStock s    
    INNER JOIN    
        tblProduct p ON p.ProductID = s.ProductID    
    INNER JOIN    
        tblWarehouse w ON w.WarehouseId = s.WarehouseID    
    GROUP BY    
        s.ProductID, p.ProductName, p.Pakaging, w.WarehouseName;    
END; 
GO
/****** Object:  StoredProcedure [dbo].[usp_getStockById]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_getStockById]
@StockId int
as  
 begin  
  SELECT DISTINCT  
		s.StockID,
        s.ProductID,  
        p.ProductName,  
		w.WarehouseId,
        w.WarehouseName,
		s.StockIn,
		s.StockOut
    FROM  
        tblStock s  
    INNER JOIN  
        tblProduct p ON p.ProductID = s.ProductID  
    INNER JOIN  
        tblWarehouse w ON w.WarehouseId = s.WarehouseID  
		where s.StockID = @StockId and s.IsDeleted = 0
 end  


GO
/****** Object:  StoredProcedure [dbo].[usp_GetStockByProductAndLocationId]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE proc [dbo].[usp_GetStockByProductAndLocationId]
 (
	@ProductId int,
	@LocationId int
 )
 as
 begin
	select Isnull((sum(StockIn) - sum(StockOut)),0) as stock from tblStock
	where  isnull(IsDeleted,0) = 0 
	group by ProductID ,WarehouseId 
	having ProductID = @ProductId and WarehouseID = @LocationId 
 end
GO
/****** Object:  StoredProcedure [dbo].[usp_GetStockByWarehouseId]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetStockByWarehouseId]    
    @WarehouseId INT    
AS    
BEGIN    
       
SELECT    
    s.ProductID,    
    p.ProductName,    
    p.Pakaging,    
    w.WarehouseName,    
    SUM(s.StockIn - s.StockOut) AS 'Stock'    
FROM    
    tblStock s    
INNER JOIN    
    tblProduct p ON p.ProductID = s.ProductID    
INNER JOIN    
    tblWarehouse w ON w.WarehouseId = s.WarehouseID    
WHERE    
    s.WarehouseID = @WarehouseId and IsDeleted = 0   
GROUP BY    
    s.ProductID, p.ProductName, p.Pakaging, w.WarehouseName;    
    
END;    
 
GO
/****** Object:  StoredProcedure [dbo].[usp_GetUnit]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_GetUnit] 
as  
begin  
 select * from tblUnit  
end  

GO
/****** Object:  StoredProcedure [dbo].[usp_GetWarehouse]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_GetWarehouse]
as
begin
	select * from tblWarehouse
end
GO
/****** Object:  StoredProcedure [dbo].[usp_HeadOfAcc]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_HeadOfAcc]
as
	begin
		select HeadId,HeadOfAcc,CONVERT(varchar(200),OpeningDate,106) as 'Opening Date',OpeningAmount,HeadOfAccName from tblHeadOffAcc order by HeadOfAcc
	end
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertCategory]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_InsertCategory]
(  
 @CategoryName nvarchar(200)  
)  
as  
begin  
 insert into tblCategory values  
 (@CategoryName)  
end  

GO
/****** Object:  StoredProcedure [dbo].[usp_InsertChartOfAccountExpeseHead]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_InsertChartOfAccountExpeseHead]        
(        
 @AccountName varchar(200),        
 @ParentId int,      
 @Dr money = null,  
 @Cr money = null,
 @InsertedId INT OUTPUT
)        
as        
begin        
 insert into tblChartOfAccount (AccountName,ParentId,Dr,Cr) values        
 (@AccountName,@ParentId,@Dr,@Cr)        
     SET @InsertedId = SCOPE_IDENTITY();    
end
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertChartOfAccountLedgers]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_InsertChartOfAccountLedgers]
(
	@TransactionDate DATETIME,
    @TransactionType NVARCHAR(50),
    @TransactionID INT,
    @ChartOfAccountId INT,
    @Remark NVARCHAR(MAX),
    @Dr MONEY,
    @Cr MONEY
)
as
begin
	insert into tblLedger (TransactionDate,TransactionType,TransactionID,ChartOfAccountId,Remark,Dr,Cr)
	values
	(@TransactionDate,@TransactionType,@TransactionID,@ChartOfAccountId,@Remark,@Dr,@Cr)
end
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertClosing]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_InsertClosing]
(
	@ClosingDate datetime,
	@CustomerID int,
	@Remark nvarchar(200),
	@Amount int
)
as
	begin
	insert into tblClosing values (@ClosingDate,@CustomerID,@Remark,@Amount)
	end
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertCompany]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_InsertCompany]
(  
 @CompanyName nvarchar(200)  
)  
as  
begin  
 insert into tblCompany values  
 (@CompanyName)  
end  

GO
/****** Object:  StoredProcedure [dbo].[usp_InsertCustomer]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_InsertCustomer]    
(    
 @CustomerID int,    
 @CustomerName varchar(200),    
 @CustomerNameUrdu nvarchar(200),    
 @ZoneID int ,     
 @AddressId int,    
 @ProperAddress varchar(200),    
 @Picture image,    
 @Phone1 varchar(20),    
 @Phone2 varchar(20),    
 @Telephone varchar(200),    
 @Remark varchar(200),    
 @ClosingDate date,    
 @OpeningAmount int,    
 @IsActice bit    
)    
 as    
 begin    
  insert into tblChartOfAccount(AccountName,ParentId,Dr,IsActive,IsEditable,IsChildable) values     
          (@CustomerName,10,@OpeningAmount,1,1,1)    
      
 declare @AccountId int  
 set @AccountId = @@IDENTITY   
   
  
  insert into tblCustomer values(@CustomerID,@CustomerName,@CustomerNameUrdu,@ZoneID,@AddressId,@ProperAddress ,@Picture,@Phone1,@Phone2,@Telephone,@Remark,@ClosingDate,@OpeningAmount,@IsActice,@AccountId)    
 end    
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertHeadOfAcc]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_InsertHeadOfAcc]
(
	@HeadId int ,
	@HeadOfAcc varchar(200),
	@HeadOfAccName nvarchar(200),
	@Remark varchar(200),
	@OpeningDate datetime,
	@OpeningAmount int
)
as
	begin
		insert into tblHeadOffAcc values (@HeadId,@HeadOfAcc,@HeadOfAccName,@Remark,@OpeningDate,@OpeningAmount)
	end
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertLedger]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_InsertLedger]
(
	@TransactionDate DATETIME,
    @TransactionType NVARCHAR(50),
    @TransactionID INT,
    @ChartOfAccountId INT,
    @Remark NVARCHAR(MAX),
    @Dr MONEY,
    @Cr MONEY
)
as
begin
	insert into tblLedger (TransactionDate,TransactionType,TransactionID,ChartOfAccountId,Remark,Dr,Cr)
	values
	(@TransactionDate,@TransactionType,@TransactionID,@ChartOfAccountId,@Remark,@Dr,@Cr)
end
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertPayment]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_InsertPayment]
(@PayId int,@PayDate date,@VendorId int,@HeadId int,@Remark nvarchar(200),@Amount int,@PreviousAmount int,@EntryDate datetime)
as
	begin
insert into tblPayment values
(
	@Payid,@PayDate,@VendorId,@HeadId,@Remark,@Amount,@PreviousAmount,@EntryDate
)
	end
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertProduct]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_InsertProduct]    
(    
 @ProductID int,    
 @ProductName varchar(200),    
 @ProductNameUrdu nvarchar(200),    
 @Company varchar(200),    
 @ProductCetegory varchar(200),    
 @Pakaging int,    
 @PurchaseRate int,    
 @SaleRate int,    
 @OpeningAmount int,    
 @Remark nvarchar(200),  
 @WarehouseId int,  
 @PurchaseUnitId int,  
 @SaleUnitId int,  
 @MeasurementProduct int,  
 @OpeningStock float  
)    
as    
 begin    

  declare @ChartOfAccountId as int
  
  insert into tblChartOfAccount (AccountName,ParentId)
  values
  (@ProductName,29)

  set @ChartOfAccountId = @@IDENTITY
  insert into tblProduct values(@ProductID,@ProductName,@ProductNameUrdu,@Company,@ProductCetegory,@Pakaging,@PurchaseRate,@SaleRate,@OpeningAmount,@Remark,@WarehouseId,@PurchaseUnitId,@SaleUnitId,@MeasurementProduct,@OpeningStock,@ChartOfAccountId)    

  insert into tblStock values (GETDATE(),0,@ProductID,@PurchaseUnitId,'Opening Inventory',@WarehouseId,@OpeningStock,0,@OpeningAmount,0)
 end 
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertPurchaseR]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_InsertPurchaseR]  
(  
 @rid int,@vid int,@date date,@remark nvarchar(200),@amount int ,@isLedger int,@ZoneId int,@AreaId int 
)  
as  
begin  
insert into tblPurchaseR values(@rid,@vid,@date,@remark,@amount,@isLedger,@ZoneId,@AreaId)  
end
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertReceipt]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_InsertReceipt]
(
	@RecId int ,
	@RecDate date,
	@Zone int,
	@Area int,	
	@CustomerID int,
	@HeadOfAcc int,
	@AmountInEnglish varchar(200),
	@Remark nvarchar(200),
	@Amount int,
	@PreviousAmount int,
	@LastBill varchar(200),
	@LastReceipt varchar(200),
	@CurrentBalance int
)
as
	begin
		insert into tblReceipt values (@RecId,@RecDate,@Zone,@Area,@CustomerID,@HeadOfAcc,@AmountInEnglish,@Remark,@Amount,@PreviousAmount,@LastBill,@LastReceipt,@CurrentBalance,GETDATE())
	end
	
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertReturn]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_InsertReturn]  
(  
 @ReturnId int,  
 @Zoneid int,  
 @Addressid int,  
 @CustomerId int,  
 @ReturnDate datetime,  
 @Remark nvarchar(200),  
 @Amount int  
  
)  
as   
begin  
 insert into tblSaleR (Returnid,Zoneid,Addressid,CustomerId,ReturnDate,Remark,Amount) values ( @ReturnId,@Zoneid,@Addressid,@CustomerId,@ReturnDate,@Remark,@Amount)  
end
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertReturnProduct]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_InsertReturnProduct]
(
	@ReturnId int,
	@ReturnDate datetime,
	@CustomerVendorAccountId int,
	@ProductId int ,
	@Qty int,
	@ProductName varchar(200),
	@Kg float,
	@Weight float,
	@Price money,
	@Amount money,
	@Type varchar(50)
)
as
begin
	declare @InsertedId int
	
	insert into tblProductTransactionReturn (ReturnId,ReturnDate,CustomerVendorAccountId,ProductId,Qty,ProductName,Kg,Weight,Price,Amount,TransactionType)
	values 
	(@ReturnId,@ReturnDate,@CustomerVendorAccountId,@ProductId,@Qty,@ProductName,@Kg,@Weight,@Price,@Amount,@Type)

	set @InsertedId = @@IDENTITY

	if @Type = 'SaleReturn'
	begin
		insert into tblStock (ProductTransDate,ProductTransId,ProductID,UnitID,TransactionType,WarehouseID,StockIn,StockOut,Amount,IsDeleted)
		values
		(@ReturnDate,@InsertedId,@ProductId,2,@Type,1,@Qty,0,@Amount,0)
	end
	else 
	begin
		insert into tblStock (ProductTransDate,ProductTransId,ProductID,UnitID,TransactionType,WarehouseID,StockIn,StockOut,Amount,IsDeleted)
		values
		(@ReturnDate,@InsertedId,@ProductId,2,@Type,1,0,@Qty,@Amount,0)
	end
end
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertStcokAdjustment]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create proc [dbo].[usp_InsertStcokAdjustment]  
(  
 @ProductTransDate datetime null,
 @ProductTransId int,  
 @ProductID int,  
 @UnitID int,  
 @TransactionType nvarchar(50),  
 @WarehouseID int,  
 @StockIn decimal(9,2),  
 @StockOut decimal(9,2),  
 @Amount money,  
 @IsDeleted bit  
)  
as  
begin  
	insert into tblStockAdjustments
	(
		ProductID,
		WarehouseID,
		StockIn,
		StockOut,
		IsDeleted
	)  
	values  
	(
		@ProductID,
		@WarehouseID,
		@StockIn,
		@StockOut,
		@IsDeleted
	)  

	DECLARE @lStockAdjustmentId INT;
	SET @lStockAdjustmentId = @@IDENTITY;

	insert into tblStock (
		ProductTransDate,
		ProductTransId,
		ProductID,
		UnitID,
		TransactionType,
		WarehouseID,
		StockIn,
		StockOut,
		Amount,
		IsDeleted)  
	values  
	(
		@ProductTransDate,
		@lStockAdjustmentId, -- yha inserted id ayegi adjustment ki   ok
		@ProductID,
		@UnitID,
		'Stock adjustment',
		@WarehouseID,
		@StockIn,
		@StockOut,
		@Amount,
		@IsDeleted)  
end  
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertStock]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_InsertStock]
(
	@ProductTransDate datetime, 
	@ProductTransId int,
	@ProductID int,
	@UnitID int,
	@TransactionType nvarchar(50),
	@WarehouseID int,
	@StockIn decimal(9,2),
	@StockOut decimal(9,2),
	@Amount money,
	@IsDeleted bit
)
as
begin
	
	insert into tblStockAdjustments (ProductID,WarehouseID,StockIn,StockOut,IsDeleted) values (@ProductID,@WarehouseID,@StockIn,@StockOut,0)

	insert into tblStock (ProductTransDate,ProductTransId,ProductID,UnitID,TransactionType,WarehouseID,StockIn,StockOut,Amount,IsDeleted)
	values
	(@ProductTransDate,@ProductTransId,@ProductID,@UnitID,@TransactionType,@WarehouseID,@StockIn,@StockOut,@Amount,@IsDeleted)
end
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertTransaction]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_InsertTransaction]
(
	@TransactionDate date,
	@TranType varchar(200),
	@TranID int,
	@CustomerName int,
	@Remark nvarchar(200),
	@Dr int,
	@Cr int,
	@PostingDate datetime,
	@TranTypeUrdu nvarchar(200)
)
	as
	begin
		insert into tblTransaction values ( @TransactionDate,@TranType,@TranID,@CustomerName,@Remark,@Dr,@Cr,@PostingDate,@TranTypeUrdu)
	end
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertUnit]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_InsertUnit]
(  
 @UnitName nvarchar(200) ,
 @UnitNameInUrdu nvarchar(200) = null 
  
)  
as  
begin  
 insert into tblUnit(UnitName,UnitNameUrdu)values  
 (@UnitName,@UnitNameInUrdu)  
end  


GO
/****** Object:  StoredProcedure [dbo].[usp_InsertVendor]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[usp_InsertVendor]    
(    
 @VendorId int,
 @VendorName varchar(200),
 @AddressId int,
 @ProperAddress varchar(200) ,
 @Picture image,
 @Phone1 varchar(20),
 @Phone2 varchar(200),
 @Telephone varchar(20),
 @Remark varchar(200),
 @Date date,
 @OpeningAmount int,
 @IsActive bit    
)    
as    
begin    
insert into tblChartOfAccount(AccountName,ParentId,Dr,IsActive,IsEditable,IsChildable) values     
          (@VendorName,11,@OpeningAmount,1,1,1)    
      
 declare @AccountId int  
 set @AccountId = @@IDENTITY   
 insert into tblVendor values(@VendorId,@VendorName,@AddressId,@ProperAddress ,@Picture,@Phone1,@Phone2,@Remark,@Telephone,@Date,@OpeningAmount,@IsActive,@AccountId) 
 
 insert into tblTransactionVendor values (@Date,'Opening Balance',0,@VendorId,@Remark,@OpeningAmount,0,getdate())     
end
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertVendorAdjustment]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_InsertVendorAdjustment]
(
	@id int,@date date,@vendorId int,@dr int,@cr int,@notes nvarchar(200)
)
as
begin 
	insert into tblVendorAdjustment values
	(@id,@date,@vendorId,@dr,@cr,@notes)
end
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertWarehouse]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_InsertWarehouse]
(
	@WarehouseName nvarchar(200)
)
as
begin
	insert into tblWarehouse values
	(@WarehouseName)
end
GO
/****** Object:  StoredProcedure [dbo].[usp_Invoice]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_Invoice]
(@id int)
	as
	begin
select tblInvoice.[Sid],tblInvoice.SDate,tblAddress.AddressNameUrdu,tblCustomer.CustomerNameUrdu,tblInvoice.Bilty,tblInvoice.Transport,tblInvoice.Remark,tblInvoice.TotalAmount,tblInvoice.Rent,tblInvoice.LabourCharges,tblInvoice.GrandTotal,tblInvoice.PreviousAmount,tblInvoice.NewAmount,tblInvoice.PostingType from tblInvoice inner join tblAddress on tblAddress.AddressID=tblInvoice.AreaId inner join tblCustomer on tblCustomer.CustomerID=tblInvoice.CustomerId where [Sid] =@id
	end
GO
/****** Object:  StoredProcedure [dbo].[usp_OutstandingReport]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_OutstandingReport]
as
	begin
select distinct(tblCustomer.CustomerID), tblCustomer.CustomerName,tblAddress.AddressName,(select sum(Cr)-sum(Dr) from tblTransaction where CustomerName=tblCustomer.CustomerID) as'Balance' from tblCustomer
inner join tblTransaction on tblCustomer.CustomerID=tblTransaction.CustomerName
inner join tblAddress on tblAddress.AddressID=tblCustomer.AddressId
where tblCustomer.IsActice =1 order by CustomerName
	end
GO
/****** Object:  StoredProcedure [dbo].[usp_OutstandingReportVendor]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_OutstandingReportVendor]
as
	begin
select distinct(tblVendor.VendorID), tblVendor.VendorName,tblAddress.AddressName,
(select sum(Dr)-sum(Cr) from tblTransactionVendor where VendorId=tblVendor.VendorID) as'Balance' from tblVendor
inner join tblTransactionVendor on tblVendor.vendorid=tblTransactionVendor.VendorId
inner join tblAddress on tblAddress.AddressID=tblVendor.VendorAddress
where tblVendor.IsActive =1 order by VendorName
	end
GO
/****** Object:  StoredProcedure [dbo].[usp_PrintPaymentVoucher]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_PrintPaymentVoucher]
(
	@PaymentId int
)
as
begin
select p.PayId,p.PayDate,v.VendorName + ' ' + a.AddressName as 'VendorName',c.AccountName,p.Remark,p.PreviousBalance,p.Amount
from tblPayment p 
inner join tblChartOfAccount c on  p.HeadID = c.AccountId
inner join tblVendor v on v.VendorID = p.VendorID
inner join tblAddress a on a.AddressID = v.VendorAddress
where p.PayId = @PaymentId
end
GO
/****** Object:  StoredProcedure [dbo].[usp_PrintReceipt]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_PrintReceipt]  
(@id int)  
as   
 begin  
select tblReceipt.RecId,RecDate,tblCustomer.CustomerName,tblAddress.AddressName,tblChartOfAccount.AccountName,tblReceipt.AmountInEnglish,tblReceipt.Remark,tblReceipt.Amount,PreviousAmount,tblReceipt.CurrentBalance from tblReceipt   
inner join tblCustomer on tblReceipt.CustomerID=tblCustomer.CustomerID  
inner join tblAddress on tblAddress.AddressID=tblReceipt.Area  
inner join tblChartOfAccount on tblChartOfAccount.AccountId=tblReceipt.HeadOfAcc  
where RecId=@id  
 end  
 
GO
/****** Object:  StoredProcedure [dbo].[usp_PrintReceiptUrdu]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_PrintReceiptUrdu]
(@id int)
as
	begin
select tblReceipt.RecId,RecDate,tblCustomer.CustomerNameUrdu,tblAddress.AddressNameUrdu,tblHeadOffAcc.HeadOfAccName,tblReceipt.Remark,tblReceipt.Amount,tblReceipt.PreviousAmount,tblReceipt.CurrentBalance from tblReceipt
inner join tblCustomer on tblCustomer.CustomerID= tblReceipt.CustomerID
inner join tblAddress on tblAddress.AddressID=tblReceipt.Area
inner join tblHeadOffAcc on tblHeadOffAcc.HeadId=tblReceipt.HeadOfAcc
where RecId=@id
	end
GO
/****** Object:  StoredProcedure [dbo].[usp_ProductInvoice]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_ProductInvoice]
(@id int)
as
begin
select tblProductTransaction.Qty,tblProduct.ProductNameUrdu,tblProductTransaction.Kg,tblProductTransaction.Weight,tblProductTransaction.Price,tblProductTransaction.Amount from tblProductTransaction inner join tblProduct on tblProduct.ProductID=tblProductTransaction.ProductId where tblProductTransaction.InvoiceId=@id
end
GO
/****** Object:  StoredProcedure [dbo].[usp_ProductPurchaseInvoice]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create proc [dbo].[usp_ProductPurchaseInvoice]  
(@id int)  
as  
begin  
select 
tblProductTransactionVendor.Qty,
tblProduct.ProductNameUrdu,
tblProductTransactionVendor.Kg,
tblProductTransactionVendor.Weight,
tblProductTransactionVendor.Price,
tblProductTransactionVendor.Amount 
from 
tblProductTransactionVendor 
inner join tblProduct on tblProduct.ProductID=tblProductTransactionVendor.ProductId 
where tblProductTransactionVendor.PurchaseId=@id
end
GO
/****** Object:  StoredProcedure [dbo].[usp_PurchaseInvoice]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_PurchaseInvoice]  
(@id int)  
 as  
 begin  
	select 
	tblPurchase.[PId],
	tblPurchase.PDate,tblAddress.AddressName,
	tblVendor.VendorName,
	tblPurchase.Bilty,tblPurchase.Transport,
	tblPurchase.Remark,tblPurchase.TotalAmount,
	tblPurchase.Rent,
	tblPurchase.LabourCharges,
	tblPurchase.GrandTotal,
	tblPurchase.PreviousAmount,
	tblPurchase.NewAmount,
	tblPurchase.PostingType 
	from tblPurchase 
	inner join tblAddress on tblAddress.AddressID=tblPurchase.AreaId
	inner join tblVendor on tblVendor.VendorID = tblPurchase.VendorId
	inner join tblproducttransactionvendor  on tblProductTransactionVendor.PurchaseId = tblPurchase.PId
	where [PId] =@id
 end  

GO
/****** Object:  StoredProcedure [dbo].[usp_ReturnDetail]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_ReturnDetail]
(
	@ReturnId int,
	@TransactionType varchar(50)
)
as
begin
select Qty,p.ProductName,Kg,Price,Weight,Amount,ReturnId,TransactionId,t.ProductId
from tblProductTransactionReturn t 
inner join tblProduct p on t.ProductId  = p.ProductID 
where ReturnId = @ReturnId and TransactionType= @TransactionType
end
GO
/****** Object:  StoredProcedure [dbo].[usp_RollbackCounter]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_RollbackCounter]
(  @InvoiceId int ) 
as 
begin  
update tblCounterSaleOrPurchase set  Isledger =0 where InvoiceId = @InvoiceId 
update  
tblLedger set IsDeleted = 1 where TransactionID = @InvoiceId and TransactionType = 'Counter Sale' 
end 
GO
/****** Object:  StoredProcedure [dbo].[usp_rptDailyExpenseReport]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_rptDailyExpenseReport] --'10-dec-2023'
(
	@ExpenseDate datetime
)
as
begin
	select e.ExpId,e.ExpDate,c.AccountName as 'ExpenseHead',cc.AccountName as 'PaymentHead',e.Notes,e.Amount from tblExpense e
	inner join tblChartOfAccount c on c.AccountId= e.ExpTypeChartOfAccountId
	inner join tblChartOfAccount cc on  cc.AccountId = e.PaymentModeChartOfAccountId 
	where cast(e.ExpDate as date) = cast(@ExpenseDate as date)
end
GO
/****** Object:  StoredProcedure [dbo].[usp_rtpCounterReceipt]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_rtpCounterReceipt]
(
	@id int
)
as
begin
	select c.InvoiceId,FORMAT(c.InvTime, 'HH:mm')  as 'InvTime',format(c.InvDate,'dd-MMM-yyyy') as 'InvDate',
	p.ProductName + ' ' + cast(c.Kg as varchar(50)) + ' Kg' as ProductName
	,c.Price,cast(c.Qty as int) as 'Qty',c.Weight,Cast((c.Weight * c.Price) as int) as 'TotalPrice',
	c.Kg,coac.AccountName as 'PaymentType',coa.AccountName as 'CounerNo',Isnull(c.Rent,0) as 'Rent',Isnull(c.Discount,0) as 'Discount',
	c.Remark
	from tblCounterSaleOrPurchase c inner join tblProduct p on c.ProductId = p.ProductID
	inner join tblChartOfAccount coa on coa.AccountId = c.PaymentType
	inner join tblChartOfAccount coac on coac.AccountId = c.CounterNo
	where c.InvoiceId = @id and ISNULL(IsDeleted,0) = 0
end
GO
/****** Object:  StoredProcedure [dbo].[usp_StockLedger]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_StockLedger]
(
	@ProductId int,
	@WarehouseId int
)
as
begin
	select ProductTransDate,TransactionType,StockIn,StockOut, 
	sum(StockIn - StockOut) over (order by StockId) as 'Total' from tblStock s
	where (ProductID = @ProductId  or 0 = @ProductId)  
	-- warehouse ki condition pending hai
end
GO
/****** Object:  StoredProcedure [dbo].[usp_tblAddressInsert]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_tblAddressInsert]
(
	@AddressName varchar(200),
	@AddressNameUrdu nvarchar(200),
	@zoneId int
)
as	
	begin
		insert into tblAddress values (@AddressName,@AddressNameUrdu,@zoneId)
	end
GO
/****** Object:  StoredProcedure [dbo].[usp_tblAddressUpdate]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_tblAddressUpdate]
(
	@AddressId int,
	@AddressName varchar(200),
	@AddressNameUrdu nvarchar(200),
	@ZoneId int
)
	as
	begin	
		update tblAddress set AddressName=@AddressName,AddressNameUrdu=@AddressNameUrdu,ZoneID=@ZoneId where AddressID =@AddressId
	end
GO
/****** Object:  StoredProcedure [dbo].[usp_tblAddressView]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_tblAddressView]
as
	begin
		select tblAddress.AddressID,tblAddress.AddressName,tblAddress.AddressNameUrdu,tblZone.ZoneName ,tblZone.ZoneId from tblAddress inner join tblZone on tblAddress.ZoneID= tblZone.ZoneId
	end

GO
/****** Object:  StoredProcedure [dbo].[usp_tblBankInsert]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_tblBankInsert]      
(      
 @BankId int,      
 @BankTitle varchar(50),      
 @BankName nvarchar(200),      
 @BankNameUrdu nvarchar(200),      
 @AccountNo varchar(50),      
 @Address nvarchar(200),      
 @OpeningDate datetime,      
 @OpeningBalance money,      
 @IsActive bit,      
 @AccountId int      
)      
as       
begin      
   insert into tblChartOfAccount(AccountName,ParentId,Dr,IsActive,IsEditable,IsChildable) values       
          (@BankName,22,@OpeningBalance,1,1,1)      
        
  set @AccountId = @@IDENTITY      
  insert into tblBank values (@BankTitle,@BankName,@BankNameUrdu,@AccountNo,@Address,@OpeningDate,@OpeningBalance,@IsActive,@AccountId)   
     
end
GO
/****** Object:  StoredProcedure [dbo].[usp_tblBankView]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_tblBankView]        
as        
 begin        
  select * from tblBank
end  
GO
/****** Object:  StoredProcedure [dbo].[usp_tblBankViewByID]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create proc [dbo].[usp_tblBankViewByID]
(
	@id int
)
as        
 begin        
  select * from tblBank where BankId = @id
end 
GO
/****** Object:  StoredProcedure [dbo].[usp_tblStockView]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_tblStockView]
as  
 begin  
  SELECT  
		s.StockAdjID,
        p.ProductName,  
        w.WarehouseName,
		s.StockIn,
		s.StockOut
    FROM  
        tblStockAdjustments s  
    INNER JOIN  
        tblProduct p ON p.ProductID = s.ProductID  
    INNER JOIN  
        tblWarehouse w ON w.WarehouseId = s.WarehouseID   
		where IsDeleted = 0
 end  


 

GO
/****** Object:  StoredProcedure [dbo].[usp_tblZoneInsert]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_tblZoneInsert]
(
	@zone varchar(200),
	@zoneUrdu nvarchar(200)
)
as	
	begin
		insert into tblZone values (@zone,@zoneUrdu)
	end
GO
/****** Object:  StoredProcedure [dbo].[usp_tblZoneUpdate]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_tblZoneUpdate]
(
	@id int,
	@zoneName varchar(200),
	@zoneNameUrdu nvarchar(200)
)
	as
	begin
		update tblZone set ZoneName=@zoneName,ZoneNameUrdu=@zoneNameUrdu where ZoneId=@id
	end

GO
/****** Object:  StoredProcedure [dbo].[usp_tblZoneView]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_tblZoneView]
as
begin
	select ZoneId as 'ID' ,ZoneName as 'Name of Zone',ZoneNameUrdu as 'زون کا نام'  from tblZone	
end
GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateAdjustment]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_UpdateAdjustment]
(
	@id int,
	@TransactionDate datetime,
	@ZoneId int,
	@AdderssId int,
	@CustomerName int,
	@Dr int,
	@Cr int,
	@Notes nvarchar(200)	

)
as
	begin
		update tblAdjustment
			set TransactionDate=@TransactionDate,
			ZoneId=@ZoneId,
			AdderssId=@AdderssId,
			CustomerName = @CustomerName,
			Dr=@Dr,
			Cr=@Cr,
			Notes=@Notes
				where AdjustmentId =@id 
	end
GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateBank]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_UpdateBank]  
(  
 @BankId int,    
 @BankTitle varchar(50),    
 @BankName nvarchar(200),    
 @BankNameUrdu nvarchar(200),    
 @AccountNo varchar(50),    
 @Address nvarchar(200),    
 @OpeningDate datetime,    
 @OpeningBalance money,    
 @IsActive bit,    
 @AccountId int  
)  
as  
begin  
 update tblBank set BankTitle = @BankTitle,  
        BankName = @BankName,  
        BankNameUrdu = @BankNameUrdu,  
        AccountNo = @AccountNo,  
        Address = @Address,  
        OpeningDate = @OpeningDate,  
        OpeningBalance = @OpeningBalance,  
        IsActive = @IsActive  
 where BankId = @BankId  
  
  set @AccountId = (select ISNULL(AccountId,0) from tblBank where BankId = @BankId)
 update tblChartOfAccount set AccountName = @BankName,   
        Description = @BankTitle,  
        dr = @OpeningBalance  
 where AccountId = @AccountId  
end
GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateCategory]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_UpdateCategory]    
(    
 @CategoryId int,    
 @CategoryName nvarchar(200)    
)    
as    
begin    
 update tblCategory set CategoryName= @CategoryName where CategoryId= @CategoryId  
end    
  
GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateChartOfAccountExpenseHead]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_UpdateChartOfAccountExpenseHead]
(
	@AccountId int,
	@AccountName varchar(200)
)
as
begin
	update tblChartOfAccount set AccountName = @AccountName 
	where AccountId = @AccountId
end
GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateCompany]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_UpdateCompany]  
(  
 @CompanyId int,  
 @CompanyName nvarchar(200)  
)  
as  
begin  
 update tblCompany set CompanyName= @CompanyName where CompanyId= @CompanyId
end  

--------------------------------------------------------------------------------\Create Table tblUnit
create table tblUnit
(
   UnitId  int Primary key identity(1,1),
   UnitName varchar(250),
   UnitNameInUrdu nvarchar(200)
);

GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateCustomer]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
	CREATE proc [dbo].[usp_UpdateCustomer]
	(
		@CustomerID int,
		@CustomerName varchar(200),
		@CustomerNameUrdu nvarchar(200),
		@ZoneID int , 
		@AddressId int,
		@ProperAddress varchar(200),
		@Picture image,
		@Phone1 varchar(20),
		@Phone2 varchar(20),
		@Telephone varchar(200),
		@Remark varchar(200),
		@ClosingDate date,
		@OpeningAmount int,
		@IsActice bit
	)
		as
			begin
				update tblCustomer set
					CustomerName = @CustomerName,
					CustomerNameUrdu=@CustomerNameUrdu,
					ZoneID = @ZoneID,
					AddressId = @AddressId,
					ProperAddress = @ProperAddress,
					Picture=@Picture,
					Phone1=@Phone1,
					Phone2=@Phone2,
					Telephone=@Telephone,
					Remark=@Remark,
					ClosingDate=@ClosingDate,
					OpeningAmount = @OpeningAmount,
					IsActice=@IsActice
						where CustomerID =@CustomerID				
			end
GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateHead]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_UpdateHead]
(
	@HeadId int ,
	@HeadOfAcc varchar(200),
	@HeadOfAccName nvarchar(200),
	@Remark varchar(200),
	@OpeningDate datetime,
	@OpeningAmount int
)
as
begin
	update tblHeadOffAcc 
		set HeadOfAcc = @HeadOfAcc,
		HeadOfAccName=@HeadOfAccName,
		Remark=@Remark,
		OpeningDate=@OpeningDate,
		OpeningAmount=@OpeningAmount
			where 
				HeadId=@HeadId
end
GO
/****** Object:  StoredProcedure [dbo].[usp_UpdatePayment]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_UpdatePayment]
(@PayId int,@PayDate date,@VendorId int,@HeadId int,@Remark nvarchar(200),@Amount int,@PreviousAmount int)
as
begin
	update tblPayment 
		set PayDate=@PayDate,VendorID=@VendorId,HeadID=@HeadId,Remark=@Remark,Amount=@Amount,PreviousBalance=@PreviousAmount
		where PayId=@PayId
end
GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateProduct]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_UpdateProduct]  
(  
 @ProductID int,  
 @ProductName varchar(200),  
 @ProductNameUrdu nvarchar(200),  
 @Company varchar(200),  
 @ProductCetegory varchar(200),  
 @Pakaging int,  
 @PurchaseRate int,  
 @SaleRate int,  
 @OpeningAmount int,  
 @Remark nvarchar(200),
 @WarehouseId int,    
 @PurchaseUnitId int,    
 @SaleUnitId int,    
 @MeasurementProduct int,    
 @OpeningStock float    
)  
as  
 begin  
  update tblProduct  
   set   
   ProductName=@ProductName,  
   ProductNameUrdu=@ProductNameUrdu,  
   Company=@Company,  
   ProductCetegory=@ProductCetegory,  
   Pakaging=@Pakaging,  
   PurchaseRate=@PurchaseRate,  
   SaleRate=@SaleRate,  
   OpeningAmount=@OpeningAmount,  
   Remark=@Remark  ,
   PurchaseUnit = @PurchaseUnitId,
   SaleUnit = @SaleUnitId,
   MeasurementProduct = @MeasurementProduct,
   OpeningStock = @OpeningStock
    where ProductID=@ProductID  

	declare @ChartOfAccountID  int = (select ISNULL(ChartOfAccountId,0) from tblProduct where ProductID = @ProductID)
	
	update  tblChartOfAccount
		set AccountName = @ProductName
	where AccountId = @ChartOfAccountID

	update tblStock set WarehouseID = @WarehouseId,StockIn = @OpeningStock
	where TransactionType = 'Opening Inventory' and ProductID = @ProductID
 end  
GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateReceipt]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_UpdateReceipt]
(
	@RecId int ,
	@RecDate datetime,
	@Zone int,
	@Area int,	
	@CustomerID int,
	@HeadOfAcc int,
	@AmountInEnglish varchar(200),
	@Remark nvarchar(200),
	@Amount int,
	@PreviousAmount int,
	@LastBill varchar(200),
	@LastReceipt varchar(200),
	@CurrentBalance int
)
	as
		begin
			update tblReceipt 
				set RecDate=@RecDate,Zone=@Zone,Area=@Area,CustomerID=@CustomerID,HeadOfAcc=@HeadOfAcc,
				AmountInEnglish=@AmountInEnglish,Remark=@Remark,Amount=@Amount,PreviousAmount=@PreviousAmount,
				CurrentBalance=@CurrentBalance,EntryDate= getdate()
				where 
					RecId=@RecId
		end

GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateReceiptToPaid]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_UpdateReceiptToPaid]
(
	@Id int,
    @EntryDate datetime,
    @CZone int,
	@CAddress int,
    @CustomerId int,
    @CRemark nvarchar(200),
    @CLastBalance decimal,
	@VZone int,
    @VAddress int,
    @VendorId int,
    @VRemark nvarchar(200),
    @VLastBalance decimal,
	@Amount decimal
)
as
begin
	update tblReceiptToPaid set CZone = @CZone,CArea = @CAddress,CustomerId = @CustomerId,CRemark = @CRemark,
	VZone = @VZone,VArea = @VAddress,VendorId = @VendorId,VRemark = @VRemark,Amount = @Amount,
	CustomerPrevBalance = @CLastBalance,
	VendorPrevBalance = @VLastBalance 
	where Id = @Id
end
GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateReceiptTransaction]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_UpdateReceiptTransaction]
(
	@TranDate datetime,
	@TranId int,
	@CustomerName int,
	@Remark nvarchar(200),
	@Dr	int
)
as
	begin
		update tblTransaction 
			set TransactionDate = @TranDate,
				CustomerName = @CustomerName,
				Remark = @Remark,
				Dr = @Dr
					where
				TranID=@TranId
				and 
				TranType = 'Receipt'
	end
GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateTransaction]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_UpdateTransaction]
(
	@TransactionDate datetime,
	@TranType varchar(200),
	@TranID int,
	@CustomerName int,
	@Remark nvarchar(200),
	@Dr int,
	@Cr int,
	@TranTypeUrdu nvarchar(200)
)
	as
	begin
		update tblTransaction set 
			TransactionDate=@TransactionDate,
			TranType=@TranType,
			TranID=@TranID,
			Remark=@Remark,
			Dr=@Dr,
			Cr=@Cr,
			TranTypeUrdu = @TranTypeUrdu
				where CustomerName=@CustomerName and TranID=0 and TranType ='Ledger'
	end	

GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateTransactionAdjustment]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_UpdateTransactionAdjustment]
(
	@TransactionDate datetime,
	@TranType varchar(200),
	@TranID int,
	@CustomerName int,
	@Remark nvarchar(200),
	@Dr int,
	@Cr int,
	@TranTypeUrdu nvarchar(200)
)
	as
	begin
		update tblTransaction set 
			TransactionDate=@TransactionDate,
			TranType=@TranType,
			Remark=@Remark,
			Dr=@Dr,
			Cr=@Cr,
			TranTypeUrdu = @TranTypeUrdu
				where CustomerName=@CustomerName and TranID=@TranID and TranType ='Adjustment'
	end	
GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateUnit]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_UpdateUnit]
(  
 @UnitId int,  
 @UnitName nvarchar(200),
 @UnitNameInUrdu nvarchar(200) = null  
)  
as  
begin  
 update tblUnit set UnitName= @UnitName , UnitNameUrdu = @UnitNameInUrdu  where UnitId= @UnitId

end  







GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateVendor]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_UpdateVendor]
(@VendorId int,@VendorName varchar(200),@AddressId int,@ProperAddress varchar(200) ,@Picture image,@Phone1 varchar(20),@Phone2 varchar(200),@Telephone varchar(20),@Remark varchar(200),@Date date,@OpeningAmount int,@IsActive bit)
as
begin

	declare @ChartOfAccountId int
	set @ChartOfAccountId = (select isnull(AccountId,0) from tblVendor where VendorID = @VendorId)

	update tblChartOfAccount set AccountName = @VendorName where AccountId = @ChartOfAccountId

	update tblVendor set
	VendorName=@VendorName,VendorAddress=@AddressId,ProperAddress=@ProperAddress,Picture=@Picture,Phone1=@Phone1,Phone2=@Phone2,Telephone=@Telephone,Remark=@Remark,ClosingDate=@Date,OpeningAmount=@OpeningAmount,IsActive=@IsActive
	where
	VendorID=@VendorId

	update tblTransactionVendor set TransactionDate = @Date,Remark = @Remark,Dr = @OpeningAmount 
	where VendorId = @VendorId and LedgerType = 'Opening Balance'
end
GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateVendorAdjustment]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_UpdateVendorAdjustment]
(@id int,@date date,@vendorId int,@dr int,@cr int,@notes nvarchar(200))
as
begin 
	update tblVendorAdjustment 
		set
		TransactionDate=@date,
		VendorID=@vendorId,
		Dr=@dr,
		Cr=@cr,
		Notes=@notes
			where AdjustmentId=@id
end
GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateWarehouse]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_UpdateWarehouse]
(
	@WarehouseId int,
	@WarehouseName nvarchar(200)
)
as
begin
	update tblWarehouse set WarehouseName = @WarehouseName where WarehouseId = @WarehouseId
end
GO
/****** Object:  StoredProcedure [dbo].[usp_VendorAccountId]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_VendorAccountId]    
 (    
 @id int    
)    
 as    
  begin    
   select Isnull(AccountId,0) from tblVendor  
where VendorID =@id    
  end
GO
/****** Object:  StoredProcedure [dbo].[usp_VendorAdjustmentAll]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_VendorAdjustmentAll]
as
begin
select tblVendorAdjustment.AdjustmentId,tblVendorAdjustment.TransactionDate,tblVendor.VendorName,tblVendorAdjustment.Notes,tblVendorAdjustment.Dr,tblVendorAdjustment.Cr from tblVendorAdjustment
inner join tblVendor on tblVendor.VendorID=tblVendorAdjustment.VendorID
end
GO
/****** Object:  StoredProcedure [dbo].[usp_VendorBalance]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE proc [dbo].[usp_VendorBalance]
  (
	@id int
)
	as
		begin
			select SUM(tblTransactionVendor.Dr)-SUM(tblTransactionVendor.Cr) from tblTransactionVendor
where VendorId =@id
		end
GO
/****** Object:  StoredProcedure [dbo].[usp_ViewAdjustment]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_ViewAdjustment]
( @date datetime)
as
	begin
select tblAdjustment.AdjustmentId,tblAdjustment.TransactionDate,tblCustomer.CustomerName,tblAdjustment.Notes,tblAdjustment.Dr,tblAdjustment.Cr from tblAdjustment inner join tblZone on tblAdjustment.ZoneId=tblZone.ZoneId inner join tblAddress on tblAddress.AddressID=tblAdjustment.AdderssId inner join tblCustomer on tblCustomer.CustomerID=tblAdjustment.CustomerName where tblAdjustment.TransactionDate=@date
	end
GO
/****** Object:  StoredProcedure [dbo].[usp_ViewAllAdjustment]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_ViewAllAdjustment]
as
	begin
select tblAdjustment.AdjustmentId,tblAdjustment.TransactionDate,tblCustomer.CustomerName,tblAdjustment.Notes,tblAdjustment.Dr,tblAdjustment.Cr from tblAdjustment inner join tblCustomer on tblCustomer.CustomerID=tblAdjustment.CustomerName
	end
GO
/****** Object:  StoredProcedure [dbo].[usp_ViewCustomerAll]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_ViewCustomerAll]
as
	begin
		select * from tblCustomer	
	end
GO
/****** Object:  StoredProcedure [dbo].[usp_ViewCustomerAllbyAddress]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_ViewCustomerAllbyAddress]
( @id int)
as
	begin
		select * from tblCustomer where AddressId= @id and IsActice=1 order by CustomerName asc
	end
GO
/****** Object:  StoredProcedure [dbo].[usp_ViewCustomerAllbyId]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_ViewCustomerAllbyId]
( @id int)
as
	begin
		select * from tblCustomer where CustomerID= @id	
	end
GO
/****** Object:  StoredProcedure [dbo].[usp_ViewCustomerUser]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_ViewCustomerUser]
as
	begin
		select * from tblCustomer	
	end

GO
/****** Object:  StoredProcedure [dbo].[usp_ViewExpenseByDate]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_ViewExpenseByDate]
( @ExpDate datetime )
as
begin
	select ExpId as 'ID',
	cast(ExpDate as date) as 'Expense Date',
	ch.AccountName as 'Expense Type',
	c.AccountName as 'Payment Mode',
	Notes,Cast(Amount as int) as 'Amount',
	e.PaymentModeChartOfAccountId,
	e.ExpTypeChartOfAccountId 
	from tblExpense e
	inner join tblChartOfAccount c on c.AccountId = e.PaymentModeChartOfAccountId 
	inner join tblChartOfAccount ch on ch.AccountId = e.ExpTypeChartOfAccountId
	where CAST(ExpDate as date) = CAST(@ExpDate as date)
end
GO
/****** Object:  StoredProcedure [dbo].[usp_ViewGatepasOrder]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_ViewGatepasOrder]
(@id int)
	as
		begin
select tblInvoice.[Sid],tblInvoice.SDate,tblAddress.AddressNameUrdu,tblCustomer.CustomerNameUrdu,tblInvoice.PageNo,tblInvoice.Transport,tblInvoice.Remark,tblInvoice.PostingType from tblInvoice inner join tblAddress on tblAddress.AddressID=tblInvoice.AreaId inner join tblCustomer on tblCustomer.CustomerID= tblInvoice.CustomerId where [Sid] = @id
		end
GO
/****** Object:  StoredProcedure [dbo].[usp_ViewInventoryTransferByDate]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_ViewInventoryTransferByDate]    
( @TransferDate datetime )    
as    
begin    
 select it.InventoryTransferId as 'ID',    
 cast(it.transferDate as date) as 'Transfer Date',    
 p.ProductName as 'Product Name',    
 it.Notes as 'Notes',  
 it.transferQty as 'transferQty' ,
 it.ProductId,
 it.FromLocationId,
 it.ToLocationId,
 it.Notes
 from tblInventoryTransfer it    
 inner join tblProduct  p on it.ProductId = p.ProductID  
 inner join tblWarehouse w on it.FromLocationID = w.WarehouseId  
 inner join tblWarehouse wh on it.TolocationId = wh.WarehouseId  
where CAST(it.TransferDate as date) = CAST(@TransferDate as date)    
end   
GO
/****** Object:  StoredProcedure [dbo].[usp_viewInvertorytransfer]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[usp_viewInvertorytransfer]
as 
begin
select * from tblInventoryTransfer 
end
GO
/****** Object:  StoredProcedure [dbo].[usp_ViewPayment]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_ViewPayment]
(@date datetime)
as
begin 
select tblPayment.PayId,tblPayment.PayDate,tblZone.ZoneName,tblAddress.AddressName,tblVendor.VendorName,tblPayment.Remark,tblPayment.Amount from tblPayment
inner join tblVendor on tblPayment.VendorID=tblVendor.VendorID
inner join tblAddress on tblVendor.VendorAddress=tblAddress.AddressID
inner join tblZone on tblAddress.ZoneID=tblZone.ZoneID
where PayDate=@date
end
GO
/****** Object:  StoredProcedure [dbo].[usp_ViewPaymentByID]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_ViewPaymentByID]
(@id int)
as
begin
select tblPayment.PayId,tblPayment.PayDate,tblZone.ZoneId,tblAddress.AddressID,tblPayment.VendorID,tblPayment.HeadID,tblPayment.Amount,tblPayment.Remark,tblPayment.PreviousBalance,tblPayment.PreviousBalance-tblPayment.Amount as 'Balance' from tblPayment
inner join tblVendor on tblPayment.VendorID=tblVendor.VendorID
inner join tblAddress on tblVendor.VendorAddress=tblAddress.AddressID
inner join tblZone on tblAddress.ZoneID=tblZone.ZoneID
where PayId=@id
end
GO
/****** Object:  StoredProcedure [dbo].[usp_ViewReceiving]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_ViewReceiving]
(@date datetime)
as
	begin
select tblReceipt.RecId,convert(varchar(200),tblReceipt.RecDate,106) as 'Date',
tblCustomer.CustomerName,tblAddress.AddressName,tblHeadOffAcc.HeadOfAcc,tblReceipt.Remark,
tblReceipt.Amount
from tblReceipt inner join tblCustomer on tblCustomer.CustomerID=tblReceipt.CustomerID
inner join tblAddress on tblAddress.AddressID=tblReceipt.Area
inner join tblHeadOffAcc on tblHeadOffAcc.HeadId=tblReceipt.HeadOfAcc
where tblReceipt.RecDate=@date
	end
GO
/****** Object:  StoredProcedure [dbo].[usp_ViewSingleAdjustment]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_ViewSingleAdjustment]
(@id int )
as
begin
select * from tblAdjustment where AdjustmentId =@id
end
GO
/****** Object:  StoredProcedure [dbo].[usp_ViewVendorByAddress]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create proc [dbo].[usp_ViewVendorByAddress]
( @id int)
as
	begin
		select * from tblVendor where VendorAddress = @id and IsActive=1 order by VendorName asc
	end
GO
/****** Object:  StoredProcedure [dbo].[uspDefaultLedgerUrdu]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
	CREATE proc [dbo].[uspDefaultLedgerUrdu]
	(
		@cid int	
	)
	as
		begin
	select tblTransaction.Cr as'رقم','' as 'جمع','' as 'بل',N'سابقہ' as 'تفصیل',tblCustomer.CustomerID  as 'آئی ڈی',tblTransaction.TranTypeUrdu as 'لین دین',tblTransaction.TransactionDate,tblCustomer.CustomerNameUrdu as 'کسٹمر کا نام' from tblTransaction inner join tblCustomer on tblTransaction.CustomerName=tblCustomer.CustomerID where tblTransaction.CustomerName=@cid and tblTransaction.TranType='Ledger'
	union all
	select tblTransaction.Cr-tblTransaction.Dr as 'رقم',tblTransaction.Dr as 'جمع',tblTransaction.Cr as 'بل',tblTransaction.Remark as 'تفصیل',tblTransaction.TranID as 'آئی ڈی',tblTransaction.TranTypeUrdu as 'لین دین',tblTransaction.TransactionDate,tblCustomer.CustomerNameUrdu as 'کسٹمر کا نام' from tblTransaction inner join tblCustomer on tblTransaction.CustomerName=tblCustomer.CustomerID where tblTransaction.CustomerName=@cid and tblTransaction.TranType<>'Ledger'
	order by tblTransaction.TransactionDate
		end

GO
/****** Object:  StoredProcedure [dbo].[ViewLedgerEnglishDefault]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[ViewLedgerEnglishDefault]
(
	@cid int
)
as
	begin
select tblCustomer.CustomerName as 'Customer Name',tblTransaction.TransactionDate,'Ledger' as 'Type of',tblCustomer.CustomerID  as 'ID','Opening Balance' as Notes,'' as 'Dr','' as 'Cr',tblTransaction.Cr as'Balance' from tblTransaction inner join tblCustomer on tblTransaction.CustomerName=tblCustomer.CustomerID where tblTransaction.CustomerName=@cid and tblTransaction.TranType='Ledger'
union all
select tblCustomer.CustomerName as 'Customer Name',tblTransaction.TransactionDate,tblTransaction.TranType as'Type of',tblTransaction.TranID as 'ID',tblTransaction.Remark as 'Notes',Dr,Cr,cr-Dr as 'Balance' from tblTransaction inner join tblCustomer on tblTransaction.CustomerName=tblCustomer.CustomerID where tblTransaction.CustomerName=@cid and TranType<> 'Ledger'
	order by tblTransaction.TransactionDate
	end
GO
/****** Object:  StoredProcedure [dbo].[ViewLedgerUrduDefault]    Script Date: 23/01/2024 1:29:06 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[ViewLedgerUrduDefault]
(
	@cid int
)
as
	begin
select tblCustomer.CustomerNameUrdu as 'کسٹمر کا نام', tblTransaction.TransactionDate,N'کھاتا' as 'انٹیری',tblCustomer.CustomerID  as 'آئی ڈی',N'سابقہ' as 'تفصیل','' as 'جمع','' as 'بل',tblTransaction.Cr as'رقم' from tblTransaction inner join tblCustomer on tblTransaction.CustomerName=tblCustomer.CustomerID where tblTransaction.CustomerName=1 and tblTransaction.TranType='Ledger'
union all
select tblCustomer.CustomerNameUrdu as 'کسٹمر کا نام',tblTransaction.TransactionDate,tblTransaction.TranType as'انٹیری',tblTransaction.TranID as 'آئی ڈی',tblTransaction.Remark as 'تفصیل',Dr as 'جمع',Cr as 'بل', cr-Dr as 'رقم' from tblTransaction inner join tblCustomer on tblTransaction.CustomerName=tblCustomer.CustomerID where tblTransaction.CustomerName=1 and TranType<> 'Ledger'
order by tblTransaction.TransactionDate
	end

GO
USE [master]
GO
ALTER DATABASE [AnajPos] SET  READ_WRITE 
GO
