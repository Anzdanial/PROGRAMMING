
create database BookManagmentSystem;
use BookManagmentSystem;

CREATE TABLE books (

                       book_id INT PRIMARY KEY,
                       title VARCHAR(255) NOT NULL,
                       author VARCHAR(255) NOT NULL,
                       price DECIMAL(10,2) NOT NULL,
                       stock INT NOT NULL
);

CREATE TABLE customers (
                           customer_id INT PRIMARY KEY,
                           name VARCHAR(255) NOT NULL,
                           email VARCHAR(255) NOT NULL,
                           address VARCHAR(255) NOT NULL
);

CREATE TABLE orders (
                        order_id INT PRIMARY KEY,
                        customer_id INT NOT NULL,
                        order_date DATE NOT NULL,
                        total DECIMAL(10,2) NOT NULL,
                        FOREIGN KEY (customer_id) REFERENCES customers(customer_id)
);

CREATE TABLE order_details (
                               order_id INT NOT NULL,
                               book_id INT NOT NULL,
                               quantity INT NOT NULL,
                               price DECIMAL(10,2) NOT NULL,
                               PRIMARY KEY (order_id, book_id),
                               FOREIGN KEY (order_id) REFERENCES orders(order_id),
                               FOREIGN KEY (book_id) REFERENCES books(book_id)
);
-- Insert some books into the books table
INSERT INTO books (title, author, price, stock) VALUES
                                                    ('The Catcher in the Rye', 'J.D. Salinger', 9.99, 10),
                                                    ('The Lord of the Rings', 'J.R.R. Tolkien', 19.99, 15),
                                                    ('Harry Potter and the Philosopher''s Stone', 'J.K. Rowling', 14.99, 20),
                                                    ('To Kill a Mockingbird', 'Harper Lee', 12.99, 12),
                                                    ('Nineteen Eighty-Four', 'George Orwell', 11.99, 8);

-- Insert some customers into the customers table
INSERT INTO customers (name, email, address) VALUES
                                                 ('Alice Smith', 'alice@gmail.com', '123 Main Street'),
                                                 ('Bob Jones', 'bob@yahoo.com', '456 Park Avenue'),
                                                 ('Charlie Brown', 'charlie@hotmail.com', '789 Elm Street'),
                                                 ('David Lee', 'david@gmail.com', '101 Maple Road'),
                                                 ('Emma Watson', 'emma@yahoo.com', '102 Oak Lane');

-- Insert some orders into the orders table
INSERT INTO orders (customer_id, order_date, total) VALUES
                                                        (1, '2023-10-27', 29.98),
                                                        (2, '2023-10-26', 39.98),
                                                        (3, '2023-10-25', 49.97),
                                                        (4, '2023-10-24', 59.96),
                                                        (5, '2023-10-23', 69.95);

-- Insert some order details into the order_details table
INSERT INTO order_details (order_id, book_id, quantity, price) VALUES
                                                                   (2001, 1, 1, 9.99),
                                                                   (2001, 2, 1, 19.99),
                                                                   (2002, 2, 1, 19.99),
                                                                   (2002, 3, 1, 19.99);

