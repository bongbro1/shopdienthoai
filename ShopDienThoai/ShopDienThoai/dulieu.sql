USE [ShopDienThoai]
GO
/****** Object:  Table [dbo].[Cart]    Script Date: 9/4/2024 9:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cart](
	[CartId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[ProductId] [int] NULL,
	[Quantity] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CartId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Category]    Script Date: 9/4/2024 9:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Order]    Script Date: 9/4/2024 9:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[OrderId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[OrderDate] [datetime] NOT NULL,
	[TotalAmount] [decimal](18, 2) NOT NULL,
	[ShippingAddress] [nvarchar](255) NULL,
	[OrderStatus] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrderDetail]    Script Date: 9/4/2024 9:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetail](
	[OrderDetailId] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NULL,
	[ProductId] [int] NULL,
	[Quantity] [int] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Payment]    Script Date: 9/4/2024 9:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payment](
	[PaymentId] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NULL,
	[PaymentMethod] [nvarchar](50) NULL,
	[PaymentDate] [datetime] NULL,
	[PaymentStatus] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[PaymentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Product]    Script Date: 9/4/2024 9:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](100) NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CategoryId] [int] NULL,
	[Brand] [nvarchar](50) NULL,
	[StockQuantity] [int] NULL,
	[ImageURL] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Review]    Script Date: 9/4/2024 9:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Review](
	[ReviewId] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NULL,
	[UserId] [int] NULL,
	[Rating] [int] NULL,
	[Comment] [nvarchar](max) NULL,
	[ReviewDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ReviewId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 9/4/2024 9:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[Email] [nvarchar](100) NULL,
	[PhoneNumber] [nvarchar](20) NULL,
	[Address] [nvarchar](255) NULL,
	[Role] [nvarchar](20) NULL DEFAULT ('Customer'),
	[ResetPasswordCode] [nvarchar](100) NULL,
	[VerifyAccount] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[wishlist]    Script Date: 9/4/2024 9:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[wishlist](
	[WishlistId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[AddedDate] [datetime] NOT NULL DEFAULT (getdate()),
PRIMARY KEY CLUSTERED 
(
	[WishlistId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Category] ON 

GO
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (1, N'Điện thoại')
GO
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (2, N'Laptop')
GO
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (3, N'Máy tính bảng')
GO
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (4, N'Apple')
GO
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (5, N'Samsung')
GO
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (6, N'Tablet')
GO
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (7, N'Pc - Máy văn phòng')
GO
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (8, N'Đồng hồ')
GO
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (9, N'Phụ kiện')
GO
SET IDENTITY_INSERT [dbo].[Category] OFF
GO
SET IDENTITY_INSERT [dbo].[Product] ON 

GO
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [Description], [CategoryId], [Brand], [StockQuantity], [ImageURL]) VALUES (1, N'Laptop', CAST(3333.00 AS Decimal(18, 2)), N'Không', 1, N'Hotsale giá tốt', 111, N'dt3.jpg;dt5.png')
GO
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [Description], [CategoryId], [Brand], [StockQuantity], [ImageURL]) VALUES (3, N'Smartphone XYZ', CAST(499.99 AS Decimal(18, 2)), N'<p>Latest smartphone with advanced features</p>', 1, N'Hotsale giá tốt', 100, N'dh1.png;dh2.png;dh3.png')
GO
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [Description], [CategoryId], [Brand], [StockQuantity], [ImageURL]) VALUES (4, N'Laptop ABC', CAST(999.99 AS Decimal(18, 2)), N'<p>High performance laptop</p>', 2, N'Hotsale giá tốt', 50, N'lt1.jpg;lt2.jpg;lt3.jpg')
GO
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [Description], [CategoryId], [Brand], [StockQuantity], [ImageURL]) VALUES (5, N'Tablet DEF', CAST(299.99 AS Decimal(18, 2)), N'<p>Compact and powerful tablet</p>', 3, N'Hotsale giá tốt', 150, N'mt1.png;mt2.jpg;mt3.png')
GO
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [Description], [CategoryId], [Brand], [StockQuantity], [ImageURL]) VALUES (6, N'Smartwatch GHI', CAST(199.99 AS Decimal(18, 2)), N'Smartwatch with health monitoring', 4, N'Sản phẩm mới', 200, N'dh4.png;dh5.jpg;dh6.jpeg')
GO
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [Description], [CategoryId], [Brand], [StockQuantity], [ImageURL]) VALUES (7, N'Phone Accessories', CAST(19.99 AS Decimal(18, 2)), N'Variety of phone accessories', 5, N'Sản phẩm mới', 500, N'pk1.jpg;pk2.jpg;pk3.jpg')
GO
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [Description], [CategoryId], [Brand], [StockQuantity], [ImageURL]) VALUES (8, N'Laptop Bag', CAST(39.99 AS Decimal(18, 2)), N'Durable laptop bag', 2, N'Sản phẩm mới', 300, N'lt4.jpg;lt5.jpg')
GO
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [Description], [CategoryId], [Brand], [StockQuantity], [ImageURL]) VALUES (9, N'Wireless Mouse', CAST(25.99 AS Decimal(18, 2)), N'Wireless mouse with ergonomic design', 6, N'Sản phẩm mới', 400, N'sp1.jpg;sp2.jpg')
GO
SET IDENTITY_INSERT [dbo].[Product] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 

GO
INSERT [dbo].[User] ([UserId], [Name], [Password], [Email], [PhoneNumber], [Address], [Role], [ResetPasswordCode], [VerifyAccount]) VALUES (2, N'Nguyễn Văn Bông', N'1', N'admin@shopdienthoai.com', N'0973564344', N'Thái Nguyên', N'QTV', NULL, N'')
GO
INSERT [dbo].[User] ([UserId], [Name], [Password], [Email], [PhoneNumber], [Address], [Role], [ResetPasswordCode], [VerifyAccount]) VALUES (5, N'Nguyễn Văn Bông', N'1', N'dtc21h4802010193@ictu.edu.vn', N'122222266', N'112322', N'KH', NULL, N'')
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO
SET IDENTITY_INSERT [dbo].[wishlist] ON 

GO
INSERT [dbo].[wishlist] ([WishlistId], [UserId], [ProductId], [AddedDate]) VALUES (21, 5, 1, CAST(N'2024-09-04 17:29:16.523' AS DateTime))
GO
INSERT [dbo].[wishlist] ([WishlistId], [UserId], [ProductId], [AddedDate]) VALUES (24, 5, 3, CAST(N'2024-09-04 18:27:48.433' AS DateTime))
GO
INSERT [dbo].[wishlist] ([WishlistId], [UserId], [ProductId], [AddedDate]) VALUES (25, 5, 4, CAST(N'2024-09-04 18:27:52.060' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[wishlist] OFF
GO
ALTER TABLE [dbo].[Order] ADD  DEFAULT ('Pending') FOR [OrderStatus]
GO
ALTER TABLE [dbo].[Payment] ADD  DEFAULT (getdate()) FOR [PaymentDate]
GO
ALTER TABLE [dbo].[Payment] ADD  DEFAULT ('Pending') FOR [PaymentStatus]
GO
ALTER TABLE [dbo].[Review] ADD  DEFAULT (getdate()) FOR [ReviewDate]
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ProductId])
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([OrderId])
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ProductId])
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([OrderId])
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([CategoryId])
GO
ALTER TABLE [dbo].[Review]  WITH CHECK ADD FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ProductId])
GO
ALTER TABLE [dbo].[Review]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[wishlist]  WITH CHECK ADD  CONSTRAINT [FK_Wishlist_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ProductId])
GO
ALTER TABLE [dbo].[wishlist] CHECK CONSTRAINT [FK_Wishlist_Product]
GO
ALTER TABLE [dbo].[wishlist]  WITH CHECK ADD  CONSTRAINT [FK_Wishlist_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[wishlist] CHECK CONSTRAINT [FK_Wishlist_User]
GO
ALTER TABLE [dbo].[Review]  WITH CHECK ADD CHECK  (([Rating]>=(1) AND [Rating]<=(5)))
GO
