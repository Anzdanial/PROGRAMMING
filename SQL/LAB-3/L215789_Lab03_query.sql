CREATE DATABASE L215789_Lab03;

USE L215789_Lab03;
CREATE TABLE salesman(salesman_id int PRIMARY KEY NOT NULL, name varchar(255), city varchar(255), commission float);
CREATE TABLE customers(customer_id int PRIMARY KEY NOT NULL, cust_name varchar(255), city varchar(255), grade int, salesman_id int references salesman(salesman_id));
CREATE TABLE orders(ord_no int PRIMARY KEY NOT NULL, purch_amt float, ord_date date, customer_id int references customers(customer_id), salesman_id int references salesman(salesman_id));

INSERT INTO salesman values(5001, 'James Hoog', 'New York', 0.15), (5002, 'Nail Knite', 'Paris', 0.13), (5005, 'Pit Alex', 'London', 0.11), (5006, 'Mc Lyon', 'Paris', 0.14), (5007, 'Paul Adam', 'San Jose', 0.13), (5003, 'Lauson Hen', 'San Jose', 0.12);

INSERT INTO customers values(3002, 'Nick Rimando', 'New York', 100, 5001),
(3007, 'John Brad Davis', 'New York', 200, 5001),
(3005, 'Graham Zusi', 'California', 200, 5002),
(3008, 'Julian Green', 'London', 300, 5002),
(3004, 'Fabian Johnson', 'Paris', 300, 5006),
(3009, 'Geoff Cameron', 'Berlin', 100, 5003),
(3003, 'Jozy Altidor', 'Moscow', 200, 5007),
(3001, 'John Brad Guzan', 'London', Null, 5005);

INSERT INTO orders values(70001, 150.5, '2012-10-05', 3005, 5002), (70009, 270.65, '2011-09-10', 3001, 5005), (70002, 65.26, '2014-10-05', 3002, 5001), (70004, 110.5, '2011-08-17', 3009, 5003), 
(70007, 948.5, '2012-09-10', 3005, 5002),
(70005, 2400.6, '2010-07-27', 3007, 5001),
(70008, 5760, '2013-09-10', 3002, 5001),
(70010, 1983.43, '2010-10-10', 3004, 5006),
(70003, 2480.4, '2013-10-10', 3009, 5003),
(70012, 250.45, '2010-06-27', 3008, 5002),
(70011, 75.29, '2014-08-17', 3003, 5007),
(70013, 3045.6, '2010-04-25', 3002, 5001);

--Q1
SELECT * FROM customers where city='New York' ORDER BY cust_name ASC;

--Q2
SELECT * FROM customers where cust_name LIKE '%John%' AND city in ('London', 'Paris', 'New York');

--Q3
Select * FROM customers where city='London' OR city='New York';

--Q4
SELECT * FROM orders where purch_amt > 500;

--Q5
SELECT * FROM salesman where name LIKE '_a%';

--Q6
SELECT * FROM salesman;
UPDATE salesman SET commission = commission + 0.5 where city='San Jose';
SELECT * FROM salesman;

--Q7
SELECT * from orders ORDER BY ord_date ASC;

--Q8
ALTER TABLE salesman ADD first_name varchar(255);
UPDATE salesman SET first_name=LEFT(name, charindex(' ', name) - 1);
ALTER TABLE salesman DROP COLUMN name;
Select * from salesman;

--Q9
SELECT * from orders where MONTH(ord_date) = 1;

--Q10
SELECT DATEPART(YEAR, ord_date) as year,
DATEPART(WEEK, ord_date) as Week,
DATEPART(dayofyear, ord_date) as DayOfYear,
DATEPART(MONTH, ord_date) as Month,
DATEPART(DAY, ord_date) as Day,
DATEPART(WEEKDAY, ord_date) + 1 as DayOfWeek
from orders where MONTH(ord_date) = 10;

--Q11
select * from orders;
UPDATE orders SET purch_amt = purch_amt*3 where MONTH(ord_date)=10;
select * from orders;

--Q12
select customers.customer_id, customers.cust_name, customers.city, customers.grade, customers.salesman_id FROM customers INNER JOIN orders on orders.customer_id=customers.customer_id where YEAR(ord_date) = 2011 OR YEAR(ord_date)=2013;

--Q13
SELECT TOP 1 * from salesman ORDER BY commission DESC;

SELECT * FROM customers;
SELECT * FROM orders;
SELECT * FROM salesman;
