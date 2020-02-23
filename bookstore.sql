-- phpMyAdmin SQL Dump
-- version 4.5.1
-- http://www.phpmyadmin.net
--
-- Host: 127.0.0.1
-- Generation Time: Feb 23, 2020 at 07:06 PM
-- Server version: 10.1.9-MariaDB
-- PHP Version: 5.6.15

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `bookstore`
--
CREATE DATABASE IF NOT EXISTS `bookstore` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `bookstore`;

-- --------------------------------------------------------

--
-- Table structure for table `book`
--

CREATE TABLE `book` (
  `item_id` int(11) NOT NULL,
  `author` varchar(100) NOT NULL,
  `genre` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `book`
--

INSERT INTO `book` (`item_id`, `author`, `genre`) VALUES
(5, 'J.K.Rowling', 'Fiction'),
(6, 'Peppa Pic', 'Kids'),
(9, 'National Geographic', 'Encyclopedia');

-- --------------------------------------------------------

--
-- Table structure for table `customer`
--

CREATE TABLE `customer` (
  `user_id` int(11) NOT NULL,
  `email` varchar(50) NOT NULL,
  `phone` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `customer`
--

INSERT INTO `customer` (`user_id`, `email`, `phone`) VALUES
(4, 'ggalk@gmail.com', '1234567890'),
(5, 'au@hotmail.com', '9856537829'),
(8, 'winston@gmail.com', '6637488830'),
(9, 'op@hotmail.com', '9999999999'),
(11, 'pat@email.com', '0876590000'),
(12, 'johns@gmail.com', '0897635748'),
(14, 'jb@gmail.ie', '1234567891');

-- --------------------------------------------------------

--
-- Table structure for table `employee`
--

CREATE TABLE `employee` (
  `user_id` int(11) NOT NULL,
  `isManager` tinyint(1) NOT NULL,
  `password` varchar(20) NOT NULL,
  `employed` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `employee`
--

INSERT INTO `employee` (`user_id`, `isManager`, `password`, `employed`) VALUES
(1, 0, '66666', 1),
(3, 1, 'letmein', 1),
(13, 0, 'llllll', 1);

-- --------------------------------------------------------

--
-- Table structure for table `item`
--

CREATE TABLE `item` (
  `item_id` int(11) NOT NULL,
  `item_name` varchar(100) NOT NULL,
  `description` varchar(500) NOT NULL,
  `stock` int(11) NOT NULL,
  `itemtype` varchar(50) NOT NULL,
  `picture` varchar(100) NOT NULL,
  `price` float NOT NULL,
  `inInventory` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `item`
--

INSERT INTO `item` (`item_id`, `item_name`, `description`, `stock`, `itemtype`, `picture`, `price`, `inInventory`) VALUES
(4, 'Faber Castle Pen', 'An Artist Pen', 20, 'stationery', '/Images/faberCastle.jpg', 5.5, 1),
(5, 'Harry Potter', 'Harry Potter and the Deathly Hallows', 6, 'book', '/Images/harryPotter6.jpg', 25, 1),
(6, 'Peppa Pig', 'Peppa Pig - The Tooth Fairy', 7, 'book', '/Images/peppaPig.jpg', 10, 0),
(7, 'The Face', 'A type of magazine', 7, 'magazine', '/Images/theFace.jpg', 10, 1),
(8, 'Scissors', 'A type of scissors', 6, 'stationery', '/Images/scissors.jpg', 3, 1),
(9, 'Animal Encyclopedia', 'National Geographic Animal Encyclopedia', 11, 'book', '/Images/animalEncyclopedia.jpg', 10, 1),
(10, 'Luna Colour Pencils', 'Luna Colour Pencils', 6, 'stationery', '/Images/luna.jpg', 20, 1),
(11, 'Marie Claire', 'Marie Claire Magazine', 20, 'magazine', '/Images/marieClaire.jpg', 15, 1),
(12, 'Pencil', 'A type of pencil', 10, 'stationery', '/Images/red-pencil.jpg', 2, 1);

-- --------------------------------------------------------

--
-- Table structure for table `magazine`
--

CREATE TABLE `magazine` (
  `item_id` int(11) NOT NULL,
  `publisher` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `magazine`
--

INSERT INTO `magazine` (`item_id`, `publisher`) VALUES
(7, 'Business of Fashion'),
(11, 'The UK Abortion');

-- --------------------------------------------------------

--
-- Table structure for table `order`
--

CREATE TABLE `order` (
  `orderID` int(11) NOT NULL,
  `code` varchar(10) NOT NULL,
  `date` varchar(15) NOT NULL,
  `user_id` int(11) NOT NULL,
  `complete` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `order`
--

INSERT INTO `order` (`orderID`, `code`, `date`, `user_id`, `complete`) VALUES
(1, 'hello1', '4-12-2019', 4, 1),
(2, 'hello2', '4-12-2019', 5, 0),
(5, 'doop12', '4-12-2019', 8, 1),
(6, 'llllll', '4-12-2019', 9, 0),
(7, 'meyouhim', '15-12-2019', 11, 0),
(8, 'mememe', '15-12-2019', 12, 1),
(9, '123456', '19-12-2019', 14, 1);

-- --------------------------------------------------------

--
-- Table structure for table `orderitem`
--

CREATE TABLE `orderitem` (
  `order_id` int(11) NOT NULL,
  `item_id` int(11) NOT NULL,
  `quantity` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `orderitem`
--

INSERT INTO `orderitem` (`order_id`, `item_id`, `quantity`) VALUES
(6, 7, 1),
(6, 6, 2),
(5, 5, 1),
(5, 6, 2),
(1, 4, 6),
(2, 7, 2),
(2, 5, 4),
(2, 6, 1),
(7, 6, 2),
(7, 9, 9),
(7, 9, 4),
(8, 7, 2),
(8, 9, 1),
(8, 8, 1),
(9, 5, 1),
(9, 10, 1),
(9, 7, 1);

-- --------------------------------------------------------

--
-- Table structure for table `sale`
--

CREATE TABLE `sale` (
  `sale_id` int(11) NOT NULL,
  `order_id` int(11) NOT NULL,
  `date` varchar(100) NOT NULL,
  `total` float NOT NULL,
  `paid` float NOT NULL,
  `saleby` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `sale`
--

INSERT INTO `sale` (`sale_id`, `order_id`, `date`, `total`, `paid`, `saleby`) VALUES
(1, 5, '8-12-2019', 30, 40, 1),
(2, 1, '9-12-2019', 1.8, 1.8, 1),
(3, 8, '15-12-2019', 33, 33, 3),
(4, 9, '19-12-2019', 55, 55, 3);

-- --------------------------------------------------------

--
-- Table structure for table `stationery`
--

CREATE TABLE `stationery` (
  `item_id` int(11) NOT NULL,
  `colour` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `stationery`
--

INSERT INTO `stationery` (`item_id`, `colour`) VALUES
(4, 'Black'),
(8, 'Red'),
(10, 'Colourful'),
(12, 'red');

-- --------------------------------------------------------

--
-- Table structure for table `user`
--

CREATE TABLE `user` (
  `user_id` int(11) NOT NULL,
  `firstname` varchar(100) NOT NULL,
  `lastname` varchar(100) NOT NULL,
  `user_type` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `user`
--

INSERT INTO `user` (`user_id`, `firstname`, `lastname`, `user_type`) VALUES
(1, 'Jon', 'Snow', 'employee'),
(3, 'Mary', 'Anne', 'employee'),
(4, 'Gregory', 'Galk', 'customer'),
(5, 'Amy', 'User', 'customer'),
(8, 'Winston', 'Joy', 'customer'),
(9, 'Ollie', 'Pickles', 'customer'),
(11, 'Patrick', 'Wright', 'customer'),
(12, 'John', 'Smith', 'customer'),
(13, 'Jenny', 'Bee', 'employee'),
(14, 'Joe', 'Bloggs', 'customer');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `book`
--
ALTER TABLE `book`
  ADD UNIQUE KEY `item_id_2` (`item_id`),
  ADD KEY `item_id` (`item_id`);

--
-- Indexes for table `customer`
--
ALTER TABLE `customer`
  ADD KEY `user_id` (`user_id`);

--
-- Indexes for table `employee`
--
ALTER TABLE `employee`
  ADD KEY `user_id` (`user_id`),
  ADD KEY `user_id_2` (`user_id`);

--
-- Indexes for table `item`
--
ALTER TABLE `item`
  ADD PRIMARY KEY (`item_id`);

--
-- Indexes for table `magazine`
--
ALTER TABLE `magazine`
  ADD UNIQUE KEY `item_id_2` (`item_id`),
  ADD KEY `item_id` (`item_id`);

--
-- Indexes for table `order`
--
ALTER TABLE `order`
  ADD PRIMARY KEY (`orderID`),
  ADD KEY `cust_id` (`user_id`);

--
-- Indexes for table `orderitem`
--
ALTER TABLE `orderitem`
  ADD KEY `order_id` (`order_id`),
  ADD KEY `item_id` (`item_id`);

--
-- Indexes for table `sale`
--
ALTER TABLE `sale`
  ADD PRIMARY KEY (`sale_id`),
  ADD KEY `order_id` (`order_id`),
  ADD KEY `saleby` (`saleby`);

--
-- Indexes for table `stationery`
--
ALTER TABLE `stationery`
  ADD UNIQUE KEY `item_id_2` (`item_id`),
  ADD KEY `item_id` (`item_id`);

--
-- Indexes for table `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`user_id`);

--
-- Constraints for dumped tables
--

--
-- Constraints for table `book`
--
ALTER TABLE `book`
  ADD CONSTRAINT `book_ibfk_1` FOREIGN KEY (`item_id`) REFERENCES `item` (`item_id`);

--
-- Constraints for table `customer`
--
ALTER TABLE `customer`
  ADD CONSTRAINT `customer_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`);

--
-- Constraints for table `employee`
--
ALTER TABLE `employee`
  ADD CONSTRAINT `employee_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`);

--
-- Constraints for table `magazine`
--
ALTER TABLE `magazine`
  ADD CONSTRAINT `magazine_ibfk_1` FOREIGN KEY (`item_id`) REFERENCES `item` (`item_id`);

--
-- Constraints for table `order`
--
ALTER TABLE `order`
  ADD CONSTRAINT `order_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `customer` (`user_id`);

--
-- Constraints for table `orderitem`
--
ALTER TABLE `orderitem`
  ADD CONSTRAINT `orderitem_ibfk_1` FOREIGN KEY (`item_id`) REFERENCES `item` (`item_id`),
  ADD CONSTRAINT `orderitem_ibfk_2` FOREIGN KEY (`order_id`) REFERENCES `order` (`orderID`);

--
-- Constraints for table `sale`
--
ALTER TABLE `sale`
  ADD CONSTRAINT `sale_ibfk_1` FOREIGN KEY (`order_id`) REFERENCES `order` (`orderID`),
  ADD CONSTRAINT `sale_ibfk_2` FOREIGN KEY (`saleby`) REFERENCES `employee` (`user_id`);

--
-- Constraints for table `stationery`
--
ALTER TABLE `stationery`
  ADD CONSTRAINT `stationery_ibfk_1` FOREIGN KEY (`item_id`) REFERENCES `item` (`item_id`);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
