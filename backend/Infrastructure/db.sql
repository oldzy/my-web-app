-- Create the database if it does not exist
CREATE DATABASE IF NOT EXISTS store;
USE store;

-- Create Users table if it does not exist
CREATE TABLE IF NOT EXISTS Users (
    Id CHAR(36) PRIMARY KEY DEFAULT (UUID()),
    Username VARCHAR(256) NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL,
    IsAdmin BOOLEAN DEFAULT FALSE,
    CreatedAt DATETIME(6) DEFAULT (UTC_TIMESTAMP()),
    UpdatedAt DATETIME(6) DEFAULT (UTC_TIMESTAMP()) ON UPDATE CURRENT_TIMESTAMP(6)
);

CREATE INDEX IF NOT EXISTS IX_Users_Username ON Users (Username);

INSERT IGNORE INTO Users (Username, PasswordHash, IsAdmin) VALUES
('kamal@test.com', '$2a$12$1HzgyUoO6S9B5Bdqx/QJd.OW8ht7Z80wZBwDfiNOYXXvIymnj.XY2', TRUE),
('kamal2@test.com', '$2a$12$1HzgyUoO6S9B5Bdqx/QJd.OW8ht7Z80wZBwDfiNOYXXvIymnj.XY2', FALSE);

-- Create Product table if it does not exist
CREATE TABLE IF NOT EXISTS Product (
    Id CHAR(36) PRIMARY KEY DEFAULT (UUID()),
    Name VARCHAR(255) NOT NULL UNIQUE,
    Description TEXT,
    Price DECIMAL(10, 2) NOT NULL,
    Stock INT UNSIGNED DEFAULT 0,
    ImageUrl VARCHAR(2048),
    CreatedAt DATETIME(6) DEFAULT (UTC_TIMESTAMP()),
    UpdatedAt DATETIME(6) DEFAULT (UTC_TIMESTAMP()) ON UPDATE CURRENT_TIMESTAMP(6)
);

-- Insert sample products, ignore if product name already exists
INSERT IGNORE INTO Product (Name, Description, Price, Stock, ImageUrl) VALUES
('Laptop Pro 15', 'High-performance laptop for professionals.', 1200.00, 50, 'https://images.pexels.com/photos/812264/pexels-photo-812264.jpeg'),
('Wireless Mouse Ergonomic', 'Comfortable ergonomic wireless mouse.', 25.50, 200, 'https://images.pexels.com/photos/2115256/pexels-photo-2115256.jpeg'),
('Mechanical Keyboard RGB', 'RGB backlit mechanical keyboard for gaming.', 75.00, 100, 'https://images.pexels.com/photos/2115257/pexels-photo-2115257.jpeg'),
('4K Monitor 27 inch', 'Ultra HD 27-inch monitor for crisp visuals.', 350.00, 75, 'https://images.pexels.com/photos/546819/pexels-photo-546819.jpeg'),
('USB-C Hub 7-in-1', 'Versatile USB-C hub with multiple ports.', 39.99, 150, 'https://images.pexels.com/photos/4195399/pexels-photo-4195399.jpeg');

-- Create Cart table if it does not exist
CREATE TABLE IF NOT EXISTS Cart (
    Id CHAR(36) PRIMARY KEY DEFAULT (UUID()),
    UserId CHAR(36) NOT NULL,
    CreatedAt DATETIME(6) DEFAULT (UTC_TIMESTAMP()),
    UpdatedAt DATETIME(6) DEFAULT (UTC_TIMESTAMP()) ON UPDATE CURRENT_TIMESTAMP(6),
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS IX_Cart_UserId ON Cart (UserId);

-- Create CartItem table if it does not exist
CREATE TABLE IF NOT EXISTS CartItem (
    Id CHAR(36) PRIMARY KEY DEFAULT (UUID()),
    CartId CHAR(36) NOT NULL,
    ProductId CHAR(36) NOT NULL,
    Quantity INT UNSIGNED NOT NULL DEFAULT 1,
    Price DECIMAL(10, 2) NOT NULL,
    CreatedAt DATETIME(6) DEFAULT (UTC_TIMESTAMP()),
    UpdatedAt DATETIME(6) DEFAULT (UTC_TIMESTAMP()) ON UPDATE CURRENT_TIMESTAMP(6),
    FOREIGN KEY (CartId) REFERENCES Cart(Id) ON DELETE CASCADE,
    FOREIGN KEY (ProductId) REFERENCES Product(Id) ON DELETE CASCADE,
    UNIQUE (CartId, ProductId)
);

CREATE INDEX IF NOT EXISTS IX_CartItem_CartId ON CartItem (CartId);
CREATE INDEX IF NOT EXISTS IX_CartItem_ProductId ON CartItem (ProductId);