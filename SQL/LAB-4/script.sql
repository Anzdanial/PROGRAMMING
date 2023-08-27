create database L215789Lab04;

create table hotel(
hotelno varchar(10) primary key,
hotelname varchar(20),
city varchar(20),
)

insert into hotel values('fb01', 'Grosvenor', 'London');
insert into hotel values('fb02', 'Watergate', 'Paris');
insert into hotel values('ch01', 'Omni Shoreham', 'London');
insert into hotel values('ch02', 'Phoenix Park', 'London');
insert into hotel values('dc01', 'Latham', 'Berlin');

 -- create a table named hotel

 create table room(

roomno numeric(5),
hotelno varchar(10),
type varchar(10),
price decimal(5,2),
primary key (roomno, hotelno),
foreign key (hotelno) REFERENCES hotel(hotelno)
)

insert into room values(501, 'fb01', 'single', 19);
insert into room values(601, 'fb01', 'double', 29);
insert into room values(701, 'fb01', 'family', 39);
insert into room values(1001, 'fb02', 'single', 58);
insert into room values(1101, 'fb02', 'double', 86);
insert into room values(1001, 'ch01', 'single', 29.99);
insert into room values(1101, 'ch01', 'family', 59.99);
insert into room values(701, 'ch02', 'single', 10);
insert into room values(801, 'ch02', 'double', 15);
insert into room values(901, 'dc01', 'single', 18);
insert into room values(1001, 'dc01', 'double', 30);
insert into room values(1101, 'dc01', 'family', 35);

create table guest(
guestno numeric(5),
guestname varchar(20),
guestaddress varchar(50),
primary key (guestno)
)

insert into guest values(10001, 'John Kay', '56 High St, London');
insert into guest values(10002, 'Mike Ritchie', '18 Tain St, London');
insert into guest values(10003, 'Mary Tregear', '5 Tarbot Rd, Aberdeen');
insert into guest values(10004, 'Joe Keogh', '2 Fergus Dr, Aberdeen');
insert into guest values(10005, 'Carol Farrel', '6 Achray St, Glasgow');
insert into guest values(10006, 'Tina Murphy', '63 Well St, Glasgow');
insert into guest values(10007, 'Tony Shaw', '12 Park Pl, Glasgow');

create table booking(
hotelno varchar(10),
guestno numeric(5),
datefrom date,
dateto date,
roomno numeric(5),
primary key (hotelno, guestno, datefrom),
foreign key (roomno, hotelno) REFERENCES room(roomno, hotelno),
foreign key (guestno) REFERENCES guest(guestno)
)

 

insert into booking values('fb01', 10001, '04-04-01', '04-04-08', 501);
insert into booking values('fb01', 10004, '04-04-15', '04-05-15', 601);
insert into booking values('fb01', 10005, '04-05-02', '04-05-07', 501);
insert into booking values('fb01', 10001, '04-05-01', null, 701);
insert into booking values('fb02', 10003, '04-04-05', '10-04-04', 1001);
insert into booking values('ch01', 10006, '04-04-21', null, 1101);
insert into booking values('ch02', 10002, '04-04-25', '04-05-06', 801);
insert into booking values('dc01', 10007, '04-05-13', '04-05-15', 1001);
insert into booking values('dc01', 10003, '04-05-20', null, 1001);

--Q1
Select * from hotel where city='London' ORDER BY hotelname DESC;

--Q2
select * from hotel where hotelname like '__t%';

--Q3
select * from Booking where dateto is NULL;

--Q4
select guestname, guestaddress from guest where guestaddress like '%Glasgow%' AND (guestname LIKE '%Tony %' OR guestname LIKE '% Farrel%');

--Q5
select R.roomno, R.hotelno from Room as R inner join Booking as B on R.hotelno = B.hotelno AND R.roomno = B.roomno where Year(B.datefrom) <= 2010 AND Year(B.datefrom) >= 2005 ;

--Q6
select roomno from room where hotelno='ch02' AND type='single' AND price > 20 AND price < 40;

--Q7
select top 1 roomno, hotelno from room ORDER BY price DESC;

--Q8
select ('The hotel whose id is ' + hotelno + ' is in ' + city + ' and its name is' + hotelname + '.') AS operatorMethod from hotel;
select CONCAT('The hotel whose id is ' , hotelno , ' is in ' , city , ' and its name is' , hotelname , '.') AS functionMethod from hotel;

--Q9
select h.hotelname, h.hotelno from hotel AS h inner join room as r on r.hotelno = h.hotelno where r.type='double'
intersect
select h.hotelname, h.hotelno from hotel AS h inner join room as r on r.hotelno = h.hotelno where r.type='family';

--Q10
(select h.hotelname, h.hotelno from hotel AS h inner join room as r on r.hotelno = h.hotelno where r.type='double'
intersect
select h.hotelname, h.hotelno from hotel AS h inner join room as r on r.hotelno = h.hotelno where r.type='single')
except
select h.hotelname, h.hotelno from hotel AS h inner join room as r on r.hotelno = h.hotelno where r.type='family';

--Q11
(select r.roomno from room as r inner join booking as b on r.roomno = b.roomno where b.guestno = 10003
intersect
select r.roomno from room as r inner join booking as b on r.roomno = b.roomno where b.guestno = 10007)
except
select r.roomno from room as r inner join booking as b on r.roomno = b.roomno where b.guestno = 10001;
