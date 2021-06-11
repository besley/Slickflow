-- MySQL dump 10.13  Distrib 8.0.23, for Win64 (x86_64)
--
-- Host: localhost    Database: wfdbcore2021
-- ------------------------------------------------------
-- Server version	8.0.23

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `bizappflow`
--

DROP TABLE IF EXISTS `bizappflow`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `bizappflow` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `AppName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `AppInstanceID` varchar(50) NOT NULL,
  `AppInstanceCode` varchar(50) DEFAULT NULL,
  `Status` varchar(10) DEFAULT NULL,
  `ActivityName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Remark` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ChangedTime` datetime(6) NOT NULL,
  `ChangedUserID` varchar(50) NOT NULL,
  `ChangedUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bizappflow`
--

LOCK TABLES `bizappflow` WRITE;
/*!40000 ALTER TABLE `bizappflow` DISABLE KEYS */;
/*!40000 ALTER TABLE `bizappflow` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hrsleave`
--

DROP TABLE IF EXISTS `hrsleave`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hrsleave` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `LeaveType` smallint NOT NULL DEFAULT '0',
  `Days` decimal(18,1) NOT NULL,
  `FromDate` date NOT NULL,
  `ToDate` date NOT NULL,
  `CurrentActivityText` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Status` int DEFAULT NULL,
  `CreatedUserID` int NOT NULL,
  `CreatedUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreatedDate` date NOT NULL,
  `DepManagerRemark` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `DirectorRemark` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `DeputyGeneralRemark` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `GeneralManagerRemark` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hrsleave`
--

LOCK TABLES `hrsleave` WRITE;
/*!40000 ALTER TABLE `hrsleave` DISABLE KEYS */;
/*!40000 ALTER TABLE `hrsleave` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hrsleaveopinion`
--

DROP TABLE IF EXISTS `hrsleaveopinion`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hrsleaveopinion` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `AppInstanceID` varchar(50) NOT NULL,
  `ActivityID` varchar(50) DEFAULT NULL,
  `ActivityName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Remark` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ChangedTime` datetime(6) NOT NULL,
  `ChangedUserID` int NOT NULL,
  `ChangedUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hrsleaveopinion`
--

LOCK TABLES `hrsleaveopinion` WRITE;
/*!40000 ALTER TABLE `hrsleaveopinion` DISABLE KEYS */;
/*!40000 ALTER TABLE `hrsleaveopinion` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `manproductorder`
--

DROP TABLE IF EXISTS `manproductorder`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `manproductorder` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `OrderCode` varchar(30) DEFAULT NULL,
  `Status` smallint DEFAULT NULL,
  `ProductName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Quantity` int DEFAULT NULL,
  `UnitPrice` decimal(18,2) DEFAULT NULL,
  `TotalPrice` decimal(18,2) DEFAULT NULL,
  `CreatedTime` datetime(6) DEFAULT NULL,
  `CustomerName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Address` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Mobile` varchar(30) DEFAULT NULL,
  `Remark` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `LastUpdatedTime` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `manproductorder`
--

LOCK TABLES `manproductorder` WRITE;
/*!40000 ALTER TABLE `manproductorder` DISABLE KEYS */;
/*!40000 ALTER TABLE `manproductorder` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sysdepartment`
--

DROP TABLE IF EXISTS `sysdepartment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sysdepartment` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `DeptCode` varchar(50) NOT NULL,
  `DeptName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ParentID` int NOT NULL,
  `Description` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sysdepartment`
--

LOCK TABLES `sysdepartment` WRITE;
/*!40000 ALTER TABLE `sysdepartment` DISABLE KEYS */;
/*!40000 ALTER TABLE `sysdepartment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sysemployee`
--

DROP TABLE IF EXISTS `sysemployee`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sysemployee` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `DeptID` int NOT NULL,
  `EmpCode` varchar(50) NOT NULL,
  `EmpName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `UserID` int DEFAULT NULL,
  `Mobile` varchar(20) DEFAULT NULL,
  `EMail` varchar(100) DEFAULT NULL,
  `Remark` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sysemployee`
--

LOCK TABLES `sysemployee` WRITE;
/*!40000 ALTER TABLE `sysemployee` DISABLE KEYS */;
/*!40000 ALTER TABLE `sysemployee` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sysemployeemanager`
--

DROP TABLE IF EXISTS `sysemployeemanager`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sysemployeemanager` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `EmployeeID` int NOT NULL,
  `EmpUserID` int NOT NULL,
  `ManagerID` int NOT NULL,
  `MgrUserID` int NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sysemployeemanager`
--

LOCK TABLES `sysemployeemanager` WRITE;
/*!40000 ALTER TABLE `sysemployeemanager` DISABLE KEYS */;
/*!40000 ALTER TABLE `sysemployeemanager` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sysresource`
--

DROP TABLE IF EXISTS `sysresource`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sysresource` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `ResourceType` smallint NOT NULL,
  `ParentResourceID` int NOT NULL,
  `ResourceName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ResourceCode` varchar(100) NOT NULL,
  `OrderNo` smallint DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sysresource`
--

LOCK TABLES `sysresource` WRITE;
/*!40000 ALTER TABLE `sysresource` DISABLE KEYS */;
/*!40000 ALTER TABLE `sysresource` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sysrole`
--

DROP TABLE IF EXISTS `sysrole`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sysrole` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `RoleCode` varchar(50) NOT NULL,
  `RoleName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sysrole`
--

LOCK TABLES `sysrole` WRITE;
/*!40000 ALTER TABLE `sysrole` DISABLE KEYS */;
INSERT INTO `sysrole` VALUES (1,'employees','普通员工'),(2,'depmanager','部门经理'),(3,'hrmanager','人事经理'),(4,'director','主管总监'),(5,'deputygeneralmanager','副总经理'),(6,'generalmanager','总经理'),(7,'salesmate','业务员(Sales)'),(8,'techmate','打样员(Tech)'),(9,'merchandiser','跟单员(Made)'),(10,'qcmate','质检员(QC)'),(11,'expressmate','包装员(Express)'),(12,'finacemanager','财务经理'),(13,'testrole','testrole');
/*!40000 ALTER TABLE `sysrole` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sysrolegroupresource`
--

DROP TABLE IF EXISTS `sysrolegroupresource`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sysrolegroupresource` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `RgType` smallint NOT NULL,
  `RgID` int NOT NULL,
  `ResourceID` int NOT NULL,
  `PermissionType` smallint NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sysrolegroupresource`
--

LOCK TABLES `sysrolegroupresource` WRITE;
/*!40000 ALTER TABLE `sysrolegroupresource` DISABLE KEYS */;
/*!40000 ALTER TABLE `sysrolegroupresource` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sysroleuser`
--

DROP TABLE IF EXISTS `sysroleuser`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sysroleuser` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `RoleID` int NOT NULL,
  `UserID` int NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sysroleuser`
--

LOCK TABLES `sysroleuser` WRITE;
/*!40000 ALTER TABLE `sysroleuser` DISABLE KEYS */;
/*!40000 ALTER TABLE `sysroleuser` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sysuser`
--

DROP TABLE IF EXISTS `sysuser`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sysuser` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `UserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `EMail` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sysuser`
--

LOCK TABLES `sysuser` WRITE;
/*!40000 ALTER TABLE `sysuser` DISABLE KEYS */;
/*!40000 ALTER TABLE `sysuser` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sysuserresource`
--

DROP TABLE IF EXISTS `sysuserresource`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sysuserresource` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `UserID` int NOT NULL,
  `ResourceID` int NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sysuserresource`
--

LOCK TABLES `sysuserresource` WRITE;
/*!40000 ALTER TABLE `sysuserresource` DISABLE KEYS */;
/*!40000 ALTER TABLE `sysuserresource` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tmptest`
--

DROP TABLE IF EXISTS `tmptest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tmptest` (
  `ID` int NOT NULL,
  `VALUE` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tmptest`
--

LOCK TABLES `tmptest` WRITE;
/*!40000 ALTER TABLE `tmptest` DISABLE KEYS */;
/*!40000 ALTER TABLE `tmptest` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `vwwfactivityinstancetasks`
--

DROP TABLE IF EXISTS `vwwfactivityinstancetasks`;
/*!50001 DROP VIEW IF EXISTS `vwwfactivityinstancetasks`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `vwwfactivityinstancetasks` AS SELECT 
 1 AS `TaskID`,
 1 AS `AppName`,
 1 AS `AppInstanceID`,
 1 AS `ProcessGUID`,
 1 AS `Version`,
 1 AS `ProcessInstanceID`,
 1 AS `ActivityGUID`,
 1 AS `ActivityInstanceID`,
 1 AS `ActivityName`,
 1 AS `ActivityCode`,
 1 AS `ActivityType`,
 1 AS `WorkItemType`,
 1 AS `BackSrcActivityInstanceID`,
 1 AS `PreviousUserID`,
 1 AS `PreviousUserName`,
 1 AS `PreviousDateTime`,
 1 AS `TaskType`,
 1 AS `EntrustedTaskID`,
 1 AS `AssignedToUserID`,
 1 AS `AssignedToUserName`,
 1 AS `IsEMailSent`,
 1 AS `CreatedDateTime`,
 1 AS `LastUpdatedDateTime`,
 1 AS `EndedDateTime`,
 1 AS `EndedByUserID`,
 1 AS `EndedByUserName`,
 1 AS `TaskState`,
 1 AS `ActivityState`,
 1 AS `RecordStatusInvalid`,
 1 AS `ProcessState`,
 1 AS `ComplexType`,
 1 AS `MIHostActivityInstanceID`,
 1 AS `ApprovalStatus`,
 1 AS `CompleteOrder`,
 1 AS `AppInstanceCode`,
 1 AS `ProcessName`,
 1 AS `CreatedByUserName`,
 1 AS `PCreatedDateTime`,
 1 AS `MiHostState`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `wfactivityinstance`
--

DROP TABLE IF EXISTS `wfactivityinstance`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `wfactivityinstance` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `ProcessInstanceID` int NOT NULL,
  `AppName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `AppInstanceID` varchar(50) NOT NULL,
  `AppInstanceCode` varchar(50) DEFAULT NULL,
  `ProcessGUID` varchar(100) NOT NULL,
  `ActivityGUID` varchar(100) NOT NULL,
  `ActivityName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ActivityCode` varchar(50) DEFAULT NULL,
  `ActivityType` smallint NOT NULL,
  `ActivityState` smallint NOT NULL DEFAULT '0',
  `WorkItemType` smallint NOT NULL DEFAULT '0',
  `AssignedToUserIDs` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `AssignedToUserNames` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `BackwardType` smallint DEFAULT NULL,
  `BackSrcActivityInstanceID` int DEFAULT NULL,
  `BackOrgActivityInstanceID` int DEFAULT NULL,
  `GatewayDirectionTypeID` smallint DEFAULT NULL,
  `CanNotRenewInstance` tinyint unsigned NOT NULL DEFAULT '0',
  `ApprovalStatus` smallint DEFAULT NULL,
  `TokensRequired` int NOT NULL DEFAULT '1',
  `TokensHad` int NOT NULL,
  `ComplexType` smallint DEFAULT NULL,
  `MergeType` smallint DEFAULT NULL,
  `MIHostActivityInstanceID` int DEFAULT NULL,
  `CompareType` smallint DEFAULT NULL,
  `CompleteOrder` double DEFAULT NULL,
  `SignForwardType` smallint DEFAULT NULL,
  `NextStepPerformers` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `CreatedByUserID` varchar(50) NOT NULL,
  `CreatedByUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreatedDateTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `OverdueDateTime` datetime(6) DEFAULT NULL,
  `OverdueTreatedDateTime` datetime(6) DEFAULT NULL,
  `LastUpdatedByUserID` varchar(50) DEFAULT NULL,
  `LastUpdatedByUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `LastUpdatedDateTime` datetime(6) DEFAULT NULL,
  `EndedDateTime` datetime(6) DEFAULT NULL,
  `EndedByUserID` varchar(50) DEFAULT NULL,
  `EndedByUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `RecordStatusInvalid` tinyint unsigned NOT NULL DEFAULT '0',
  `RowVersionID` varbinary(8) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_WfActivityInstance_ProcessInstanceID` (`ProcessInstanceID`),
  CONSTRAINT `FK_WfActivityInstance_ProcessInstanceID` FOREIGN KEY (`ProcessInstanceID`) REFERENCES `wfprocessinstance` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=54 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `wfactivityinstance`
--

LOCK TABLES `wfactivityinstance` WRITE;
/*!40000 ALTER TABLE `wfactivityinstance` DISABLE KEYS */;
INSERT INTO `wfactivityinstance` VALUES (1,1,'Order-Books','123','123-code','73bae357-0e06-42b4-83bc-3b725f6344ff','f39860c8-cfc1-499c-9b4f-888f3b499bc9','Start','Start',1,4,0,NULL,NULL,0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'01','Zero','2021-03-16 09:07:49',NULL,NULL,NULL,NULL,NULL,'2021-03-16 17:07:48.625230','01','Zero',0,NULL),(2,1,'Order-Books','123','123-code','73bae357-0e06-42b4-83bc-3b725f6344ff','571c67db-466b-4d6b-8da7-de1e7ddbc9d9','Task-001','task001',4,1,1,'01','Zero',0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'01','Zero','2021-03-16 09:07:49',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,NULL),(3,2,'Order-Books','123','123-code','cff73a19-5c3a-4975-88d1-549a8ca81d26','690e9add-4e3e-4148-be48-2e04432b6792','Start','Start',1,4,0,NULL,NULL,0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'01','Zero','2021-03-16 13:03:00',NULL,NULL,NULL,NULL,NULL,'2021-03-16 21:02:59.559916','01','Zero',0,NULL),(4,2,'Order-Books','123','123-code','cff73a19-5c3a-4975-88d1-549a8ca81d26','16f63016-3746-4c49-85b3-da4621dde8f4','Task-001','task001',4,1,1,'01','Zero',0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'01','Zero','2021-03-16 13:03:00',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,NULL),(5,3,'Order-Books','123','123-code','b46f37e6-840e-4b22-bb10-96af7bcdb82a','7024ebee-daa2-4f22-b75d-efe869670645','Start','Start',1,4,0,NULL,NULL,0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'01','Zero','2021-03-16 13:06:56',NULL,NULL,NULL,NULL,NULL,'2021-03-16 21:06:56.073398','01','Zero',0,NULL),(6,3,'Order-Books','123','123-code','b46f37e6-840e-4b22-bb10-96af7bcdb82a','7554a59e-37d9-49e7-b028-2fea6147e677','Task-001','task001',4,4,1,'01','Zero',0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'01','Zero','2021-03-16 13:06:56',NULL,NULL,NULL,NULL,NULL,'2021-03-16 21:07:01.980155','01','Zero',0,NULL),(7,3,'Order-Books','123','123-code','b46f37e6-840e-4b22-bb10-96af7bcdb82a','186e3635-5f50-4e91-a93a-dde97a2b3d45','Task-002','task002',4,4,1,'151,200,71','Oliven(模拟),Ted(模拟),Glant(模拟)',0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'01','Zero','2021-03-16 13:07:02',NULL,NULL,NULL,NULL,NULL,'2021-03-16 21:07:07.493617','200','Ted(模拟)',0,NULL),(8,3,'Order-Books','123','123-code','b46f37e6-840e-4b22-bb10-96af7bcdb82a','8a7566b4-5636-4dce-a5d8-78625375e29c','Task-003','task003',4,4,1,'261,230,111','Zetophy(模拟),White(模拟),King(模拟)',0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'200','Ted(模拟)','2021-03-16 13:07:07',NULL,NULL,NULL,NULL,NULL,'2021-03-16 21:07:12.326703','111','King(模拟)',0,NULL),(9,3,'Order-Books','123','123-code','b46f37e6-840e-4b22-bb10-96af7bcdb82a','5a1340c9-db08-4cbe-8772-12228f261802','End','End',2,4,0,NULL,NULL,0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'111','King(模拟)','2021-03-16 13:07:12',NULL,NULL,NULL,NULL,NULL,'2021-03-16 21:07:12.331567','111','King(模拟)',0,NULL),(10,4,'SamplePrice','SEQ-P-1099',NULL,'072af8c3-482a-4b1c-890b-685ce2fcc75d','9b78486d-5b8f-4be4-948e-522356e84e79','Start','AJBNOX',1,4,0,NULL,NULL,0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:08',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:08.091294','10','Long',0,NULL),(11,4,'SamplePrice','SEQ-P-1099',NULL,'072af8c3-482a-4b1c-890b-685ce2fcc75d','3c438212-4863-4ff8-efc9-a9096c4a8230','Sales Submit','5Q1Q82',4,4,1,'10','Long',0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:08','2021-03-17 09:39:08.204591',NULL,NULL,NULL,NULL,'2021-03-17 09:34:08.356758','10','Long',0,NULL),(12,4,'SamplePrice','SEQ-P-1099',NULL,'072af8c3-482a-4b1c-890b-685ce2fcc75d','eb833577-abb5-4239-875a-5f2e2fcb6d57','Manager Signature','HNGPSC',4,4,1,'10','Long',0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:08',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:08.389924','10','Long',0,NULL),(13,4,'SamplePrice','SEQ-P-1099',NULL,'072af8c3-482a-4b1c-890b-685ce2fcc75d','cab57060-f433-422a-a66f-4a5ecfafd54e','Sales Confirm','9S66UP',4,4,1,'10','Long',0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:08',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:08.410991','10','Long',0,NULL),(14,4,'SamplePrice','SEQ-P-1099',NULL,'072af8c3-482a-4b1c-890b-685ce2fcc75d','b53eb9ab-3af6-41ad-d722-bed946d19792','End','9IQ4FV',2,4,0,NULL,NULL,0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:08',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:08.415126','10','Long',0,NULL),(15,5,'SamplePrice','SEQ-C-1099',NULL,'072af8c3-482a-4b1c-890b-685ce2fcc75d','9b78486d-5b8f-4be4-948e-522356e84e79','Start','AJBNOX',1,4,0,NULL,NULL,0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:08',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:08.447687','10','Long',0,NULL),(16,5,'SamplePrice','SEQ-C-1099',NULL,'072af8c3-482a-4b1c-890b-685ce2fcc75d','3c438212-4863-4ff8-efc9-a9096c4a8230','Sales Submit','5Q1Q82',4,4,1,'10','Long',0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:08','2021-03-17 09:39:08.449395',NULL,NULL,NULL,NULL,'2021-03-17 09:34:08.469898','10','Long',0,NULL),(17,5,'SamplePrice','SEQ-C-1099',NULL,'072af8c3-482a-4b1c-890b-685ce2fcc75d','eb833577-abb5-4239-875a-5f2e2fcb6d57','Manager Signature','HNGPSC',4,7,1,'10','Long',0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:08',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:08.516241','10','Long',0,NULL),(18,5,'SamplePrice','SEQ-C-1099',NULL,'072af8c3-482a-4b1c-890b-685ce2fcc75d','3c438212-4863-4ff8-efc9-a9096c4a8230','Sales Submit','5Q1Q82',4,4,1,'10','Long',2,17,16,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:08.533848','10','Long',0,NULL),(19,5,'SamplePrice','SEQ-C-1099',NULL,'072af8c3-482a-4b1c-890b-685ce2fcc75d','eb833577-abb5-4239-875a-5f2e2fcb6d57','Manager Signature','HNGPSC',4,4,1,'10','Long',0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:08.560108','10','Long',0,NULL),(20,5,'SamplePrice','SEQ-C-1099',NULL,'072af8c3-482a-4b1c-890b-685ce2fcc75d','cab57060-f433-422a-a66f-4a5ecfafd54e','Sales Confirm','9S66UP',4,4,1,'10','Long',0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:08.596009','10','Long',0,NULL),(21,5,'SamplePrice','SEQ-C-1099',NULL,'072af8c3-482a-4b1c-890b-685ce2fcc75d','b53eb9ab-3af6-41ad-d722-bed946d19792','End','9IQ4FV',2,4,0,NULL,NULL,0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:08.599453','10','Long',0,NULL),(22,5,'SamplePrice','SEQ-C-1099',NULL,'072af8c3-482a-4b1c-890b-685ce2fcc75d','cab57060-f433-422a-a66f-4a5ecfafd54e','Sales Confirm','9S66UP',4,4,1,'10','Long',3,21,20,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:08.649454','10','Long',0,NULL),(23,5,'SamplePrice','SEQ-C-1099',NULL,'072af8c3-482a-4b1c-890b-685ce2fcc75d','b53eb9ab-3af6-41ad-d722-bed946d19792','End','9IQ4FV',2,4,0,NULL,NULL,0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:08.653757','10','Long',0,NULL),(24,6,'OfficeIn','10998',NULL,'3a8ce214-fd18-4fac-95c0-e7958bc1b2f8','e52d0836-9f98-4b70-d485-6b01b8cc277e','开始','',1,4,0,NULL,NULL,0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:08.677868','10','Long',0,NULL),(25,6,'OfficeIn','10998',NULL,'3a8ce214-fd18-4fac-95c0-e7958bc1b2f8','4db4a153-c8fc-45df-b067-9d188ae19a41','仓库签字','',4,4,1,'10','Long',0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:08.696894','10','Long',0,NULL),(26,6,'OfficeIn','10998',NULL,'3a8ce214-fd18-4fac-95c0-e7958bc1b2f8','eb492ba8-075a-46e4-b95f-ac071dd3a43d','Gateway','',8,4,0,NULL,NULL,0,NULL,NULL,1,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:08.713806','10','Long',0,NULL),(27,6,'OfficeIn','10998',NULL,'3a8ce214-fd18-4fac-95c0-e7958bc1b2f8','c3cbb3cc-fa60-42ad-9a10-4ec2638aff49','行政部签字','',4,4,1,'10','Long',0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:08.734541','10','Long',0,NULL),(28,6,'OfficeIn','10998',NULL,'3a8ce214-fd18-4fac-95c0-e7958bc1b2f8','932f7fa0-2d4c-4257-c158-b8b181af2d0a','财务主管','',4,4,1,'10','Long',0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:08.759009','10','Long',0,NULL),(29,6,'OfficeIn','10998',NULL,'3a8ce214-fd18-4fac-95c0-e7958bc1b2f8','30929bbb-c76e-4604-c956-f26feb4aa22e','结束','',2,4,0,NULL,NULL,0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:08.763704','10','Long',0,NULL),(30,7,'SignTogeterPrice','MI-TOGETHER-1099',NULL,'9fb4bca4-5674-4181-a010-f0e730e166dd','1f303f19-71aa-4879-c501-f4d0f448f0a2','开始','WVYUM1',1,4,0,NULL,NULL,0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:08.791538','10','Long',0,NULL),(31,7,'SignTogeterPrice','MI-TOGETHER-1099',NULL,'9fb4bca4-5674-4181-a010-f0e730e166dd','791d9d3a-882d-4796-cffc-84d9fca76afd','业务员提交','EIXGXT',4,4,1,'10','Long',0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:08.811503','10','Long',0,NULL),(32,7,'SignTogeterPrice','MI-TOGETHER-1099',NULL,'9fb4bca4-5674-4181-a010-f0e730e166dd','36cf2479-e8ec-4936-8bcd-b38101e4664a','板房会签','0O88XX',6,4,1,'40,30,20','Susan,Jack,Meilinda',0,NULL,NULL,0,0,0,1,1,1,1,NULL,1,3,NULL,'{\"36cf2479-e8ec-4936-8bcd-b38101e4664a\":[{\"UserID\":\"20\",\"UserName\":\"Meilinda\"}]}','10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,NULL),(33,7,'SignTogeterPrice','MI-TOGETHER-1099',NULL,'9fb4bca4-5674-4181-a010-f0e730e166dd','36cf2479-e8ec-4936-8bcd-b38101e4664a','板房会签','0O88XX',6,4,1,'40','Susan',0,NULL,NULL,0,0,1,1,1,NULL,NULL,32,NULL,1,NULL,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:08.844644','40','Susan',0,NULL),(34,7,'SignTogeterPrice','MI-TOGETHER-1099',NULL,'9fb4bca4-5674-4181-a010-f0e730e166dd','36cf2479-e8ec-4936-8bcd-b38101e4664a','板房会签','0O88XX',6,4,1,'30','Jack',0,NULL,NULL,0,0,1,1,1,NULL,NULL,32,NULL,2,NULL,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:08.922181','30','Jack',0,NULL),(35,7,'SignTogeterPrice','MI-TOGETHER-1099',NULL,'9fb4bca4-5674-4181-a010-f0e730e166dd','36cf2479-e8ec-4936-8bcd-b38101e4664a','板房会签','0O88XX',6,4,1,'20','Meilinda',0,NULL,NULL,0,0,1,1,1,NULL,NULL,32,NULL,3,NULL,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:08.946888','20','Meilinda',0,NULL),(36,7,'SignTogeterPrice','MI-TOGETHER-1099',NULL,'9fb4bca4-5674-4181-a010-f0e730e166dd','23017d0c-08ca-4a59-9649-c6912b819001','业务员确认','AYZRCN',4,4,1,'10','Long',0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'20','Meilinda','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:08.990336','10','Long',0,NULL),(37,7,'SignTogeterPrice','MI-TOGETHER-1099',NULL,'9fb4bca4-5674-4181-a010-f0e730e166dd','7462aae9-da1c-43f0-d741-a4586879de77','结束','KESZ09',2,4,0,NULL,NULL,0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:08.994176','10','Long',0,NULL),(38,8,'SignForwardPrice','MI-FORWARD-1099',NULL,'1bc22da3-47e3-4a0a-be81-6d7297ad3aca','1f303f19-71aa-4879-c501-f4d0f448f0a2','开始','',1,4,0,NULL,NULL,0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:09.021676','10','Long',0,NULL),(39,8,'SignForwardPrice','MI-FORWARD-1099',NULL,'1bc22da3-47e3-4a0a-be81-6d7297ad3aca','791d9d3a-882d-4796-cffc-84d9fca76afd','业务员提交','',4,4,1,'10','Long',0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:09.043130','10','Long',0,NULL),(40,8,'SignForwardPrice','MI-FORWARD-1099',NULL,'1bc22da3-47e3-4a0a-be81-6d7297ad3aca','36cf2479-e8ec-4936-8bcd-b38101e4664a','板房加签','',6,4,1,'10,20','Long,Meilinda',0,NULL,NULL,0,0,0,1,1,2,NULL,NULL,1,2,NULL,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,NULL),(41,8,'SignForwardPrice','MI-FORWARD-1099',NULL,'1bc22da3-47e3-4a0a-be81-6d7297ad3aca','36cf2479-e8ec-4936-8bcd-b38101e4664a','板房加签','',6,8,1,'10','Long',0,NULL,NULL,0,0,0,1,1,2,NULL,40,NULL,3,1,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,'40','FangFang','2021-03-17 09:34:09.124742',NULL,NULL,NULL,0,NULL),(42,8,'SignForwardPrice','MI-FORWARD-1099',NULL,'1bc22da3-47e3-4a0a-be81-6d7297ad3aca','36cf2479-e8ec-4936-8bcd-b38101e4664a','板房加签','',6,4,1,'30','Alice',0,NULL,NULL,0,0,1,1,1,2,NULL,40,NULL,1,1,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:09.098158','30','Alice',0,NULL),(43,8,'SignForwardPrice','MI-FORWARD-1099',NULL,'1bc22da3-47e3-4a0a-be81-6d7297ad3aca','36cf2479-e8ec-4936-8bcd-b38101e4664a','板房加签','',6,4,1,'40','FangFang',0,NULL,NULL,0,0,1,1,1,2,NULL,40,NULL,2,1,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:09.121417','40','FangFang',0,NULL),(44,8,'SignForwardPrice','MI-FORWARD-1099',NULL,'1bc22da3-47e3-4a0a-be81-6d7297ad3aca','23017d0c-08ca-4a59-9649-c6912b819001','业务员确认','',4,4,1,'10','Long',0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'40','FangFang','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:09.147728','10','Long',0,NULL),(45,8,'SignForwardPrice','MI-FORWARD-1099',NULL,'1bc22da3-47e3-4a0a-be81-6d7297ad3aca','7462aae9-da1c-43f0-d741-a4586879de77','结束','',2,4,0,NULL,NULL,0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:09.151658','10','Long',0,NULL),(46,9,'SamplePrice','SEQ-P-1099',NULL,'072af8c3-482a-4b1c-890b-685ce2fcc75d','9b78486d-5b8f-4be4-948e-522356e84e79','Start','AJBNOX',1,4,0,NULL,NULL,0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:09.182355','10','Long',0,NULL),(47,9,'SamplePrice','SEQ-P-1099',NULL,'072af8c3-482a-4b1c-890b-685ce2fcc75d','3c438212-4863-4ff8-efc9-a9096c4a8230','Sales Submit','5Q1Q82',4,4,1,'10','Long',0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:09','2021-03-17 09:39:09.184474',NULL,NULL,NULL,NULL,'2021-03-17 09:34:09.206627','10','Long',0,NULL),(48,9,'SamplePrice','SEQ-P-1099',NULL,'072af8c3-482a-4b1c-890b-685ce2fcc75d','eb833577-abb5-4239-875a-5f2e2fcb6d57','Manager Signature','HNGPSC',4,4,1,'10','Long',0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:09.232979','10','Long',0,NULL),(49,9,'SamplePrice','SEQ-P-1099',NULL,'072af8c3-482a-4b1c-890b-685ce2fcc75d','cab57060-f433-422a-a66f-4a5ecfafd54e','Sales Confirm','9S66UP',4,4,1,'10','Long',0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:09.258401','10','Long',0,NULL),(50,9,'SamplePrice','SEQ-P-1099',NULL,'072af8c3-482a-4b1c-890b-685ce2fcc75d','b53eb9ab-3af6-41ad-d722-bed946d19792','End','9IQ4FV',2,4,0,NULL,NULL,0,NULL,NULL,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:09.262782','10','Long',0,NULL),(51,9,'SamplePrice','SEQ-P-1099',NULL,'072af8c3-482a-4b1c-890b-685ce2fcc75d','cab57060-f433-422a-a66f-4a5ecfafd54e','Sales Confirm','9S66UP',4,7,1,'10','Long',3,50,49,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:09.325648','10','Long',0,NULL),(52,9,'SamplePrice','SEQ-P-1099',NULL,'072af8c3-482a-4b1c-890b-685ce2fcc75d','3c438212-4863-4ff8-efc9-a9096c4a8230','Sales Submit','5Q1Q82',4,7,1,'10','Long',2,51,47,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,'2021-03-17 09:34:09.350400','10','Long',0,NULL),(53,9,'SamplePrice','SEQ-P-1099',NULL,'072af8c3-482a-4b1c-890b-685ce2fcc75d','b53eb9ab-3af6-41ad-d722-bed946d19792','End','9IQ4FV',2,1,0,'10','Long',2,52,50,0,0,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,NULL);
/*!40000 ALTER TABLE `wfactivityinstance` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `wflog`
--

DROP TABLE IF EXISTS `wflog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `wflog` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `EventTypeID` int NOT NULL,
  `Priority` int NOT NULL,
  `Severity` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Title` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Message` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `StackTrace` varchar(4000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `InnerStackTrace` varchar(4000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `RequestData` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Timestamp` datetime(6) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `wflog`
--

LOCK TABLES `wflog` WRITE;
/*!40000 ALTER TABLE `wflog` DISABLE KEYS */;
INSERT INTO `wflog` VALUES (1,2,1,'HIGH','PROCESS STARTUP ERROR','未检测到流程配置节点信息，请先设计流程后在进行办理!','   at Slickflow.Engine.Xpdl.ProcessModel.GetStartActivity() in D:\\Cloud365\\GitHomeCore\\SlickflowCore\\Source\\Slickflow.Engine\\Xpdl\\ProcessModel.cs:line 158\r\n   at Slickflow.Engine.Core.Runtime.WfRuntimeManagerFactory.CreateRuntimeInstanceStartup(WfAppRunner runner, WfExecutedResult& result) in D:\\Cloud365\\GitHomeCore\\SlickflowCore\\Source\\Slickflow.Engine\\Core\\Runtime\\WfRuntimeManagerFactory.cs:line 82\r\n   at Slickflow.Engine.Service.WorkflowService.Start(IDbConnection conn, IDbTransaction trans) in D:\\Cloud365\\GitHomeCore\\SlickflowCore\\Source\\Slickflow.Engine\\Service\\WorkflowService.cs:line 656',NULL,'{\"AppName\":\"Order-Books\",\"AppInstanceID\":\"123\",\"AppInstanceCode\":\"123-code\",\"ProcessGUID\":\"73bae357-0e06-42b4-83bc-3b725f6344ff\",\"Version\":\"1\",\"UserID\":\"01\",\"UserName\":\"Zero\",\"Conditions\":{},\"NextPerformerType\":\"Specific\"}','2021-03-16 17:00:45.639981'),(2,2,1,'HIGH','PROCESS STARTUP ERROR','未检测到流程配置节点信息，请先设计流程后在进行办理!','   at Slickflow.Engine.Xpdl.ProcessModel.GetStartActivity() in D:\\Cloud365\\GitHomeCore\\SlickflowCore\\Source\\Slickflow.Engine\\Xpdl\\ProcessModel.cs:line 158\r\n   at Slickflow.Engine.Core.Runtime.WfRuntimeManagerFactory.CreateRuntimeInstanceStartup(WfAppRunner runner, WfExecutedResult& result) in D:\\Cloud365\\GitHomeCore\\SlickflowCore\\Source\\Slickflow.Engine\\Core\\Runtime\\WfRuntimeManagerFactory.cs:line 82\r\n   at Slickflow.Engine.Service.WorkflowService.Start(IDbConnection conn, IDbTransaction trans) in D:\\Cloud365\\GitHomeCore\\SlickflowCore\\Source\\Slickflow.Engine\\Service\\WorkflowService.cs:line 656',NULL,'{\"AppName\":\"Order-Books\",\"AppInstanceID\":\"123\",\"AppInstanceCode\":\"123-code\",\"ProcessGUID\":\"cff73a19-5c3a-4975-88d1-549a8ca81d26\",\"Version\":\"1\",\"UserID\":\"01\",\"UserName\":\"Zero\",\"Conditions\":{},\"NextPerformerType\":\"Specific\"}','2021-03-16 21:01:31.764116'),(3,2,1,'HIGH','PROCESS STARTUP ERROR','未检测到流程配置节点信息，请先设计流程后在进行办理!','   at Slickflow.Engine.Xpdl.ProcessModel.GetStartActivity() in D:\\Cloud365\\GitHomeCore\\SlickflowCore\\Source\\Slickflow.Engine\\Xpdl\\ProcessModel.cs:line 158\r\n   at Slickflow.Engine.Core.Runtime.WfRuntimeManagerFactory.CreateRuntimeInstanceStartup(WfAppRunner runner, WfExecutedResult& result) in D:\\Cloud365\\GitHomeCore\\SlickflowCore\\Source\\Slickflow.Engine\\Core\\Runtime\\WfRuntimeManagerFactory.cs:line 82\r\n   at Slickflow.Engine.Service.WorkflowService.Start(IDbConnection conn, IDbTransaction trans) in D:\\Cloud365\\GitHomeCore\\SlickflowCore\\Source\\Slickflow.Engine\\Service\\WorkflowService.cs:line 656',NULL,'{\"AppName\":\"Order-Books\",\"AppInstanceID\":\"123\",\"AppInstanceCode\":\"123-code\",\"ProcessGUID\":\"cff73a19-5c3a-4975-88d1-549a8ca81d26\",\"Version\":\"1\",\"UserID\":\"01\",\"UserName\":\"Zero\",\"Conditions\":{},\"NextPerformerType\":\"Specific\"}','2021-03-16 21:02:05.097605'),(4,2,1,'HIGH','PROCESS STARTUP ERROR','流程实体为空!请检查数据库中的流程记录是否有效!','   at Slickflow.Engine.Xpdl.ProcessModelFactory.Create(IDbConnection conn, String processGUID, String version, IDbTransaction trans) in D:\\Cloud365\\GitHomeCore\\SlickflowCore\\Source\\Slickflow.Engine\\Xpdl\\ProcessModelFactory.cs:line 60\r\n   at Slickflow.Engine.Xpdl.ProcessModelFactory.Create(String processGUID, String version) in D:\\Cloud365\\GitHomeCore\\SlickflowCore\\Source\\Slickflow.Engine\\Xpdl\\ProcessModelFactory.cs:line 29\r\n   at Slickflow.Engine.Core.Runtime.WfRuntimeManagerFactory.CreateRuntimeInstanceStartup(WfAppRunner runner, WfExecutedResult& result) in D:\\Cloud365\\GitHomeCore\\SlickflowCore\\Source\\Slickflow.Engine\\Core\\Runtime\\WfRuntimeManagerFactory.cs:line 81\r\n   at Slickflow.Engine.Service.WorkflowService.Start(IDbConnection conn, IDbTransaction trans) in D:\\Cloud365\\GitHomeCore\\SlickflowCore\\Source\\Slickflow.Engine\\Service\\WorkflowService.cs:line 656',NULL,'{\"AppName\":\"SamplePrice\",\"AppInstanceID\":\"SEQ-P-1099\",\"ProcessGUID\":\"072af8c3-482a-4b1c-890b-685ce2fcc75d\",\"Version\":\"1\",\"UserID\":\"10\",\"UserName\":\"Long\",\"Conditions\":{},\"NextPerformerType\":\"Specific\"}','2021-03-16 21:10:01.243406'),(5,2,1,'HIGH','PROCESS STARTUP ERROR','流程实体为空!请检查数据库中的流程记录是否有效!','   at Slickflow.Engine.Xpdl.ProcessModelFactory.Create(IDbConnection conn, String processGUID, String version, IDbTransaction trans) in D:\\Cloud365\\GitHomeCore\\SlickflowCore\\Source\\Slickflow.Engine\\Xpdl\\ProcessModelFactory.cs:line 60\r\n   at Slickflow.Engine.Xpdl.ProcessModelFactory.Create(String processGUID, String version) in D:\\Cloud365\\GitHomeCore\\SlickflowCore\\Source\\Slickflow.Engine\\Xpdl\\ProcessModelFactory.cs:line 29\r\n   at Slickflow.Engine.Core.Runtime.WfRuntimeManagerFactory.CreateRuntimeInstanceStartup(WfAppRunner runner, WfExecutedResult& result) in D:\\Cloud365\\GitHomeCore\\SlickflowCore\\Source\\Slickflow.Engine\\Core\\Runtime\\WfRuntimeManagerFactory.cs:line 81\r\n   at Slickflow.Engine.Service.WorkflowService.Start(IDbConnection conn, IDbTransaction trans) in D:\\Cloud365\\GitHomeCore\\SlickflowCore\\Source\\Slickflow.Engine\\Service\\WorkflowService.cs:line 656',NULL,'{\"AppName\":\"SamplePrice\",\"AppInstanceID\":\"SEQ-P-1099\",\"ProcessGUID\":\"072af8c3-482a-4b1c-890b-685ce2fcc75d\",\"Version\":\"1\",\"UserID\":\"10\",\"UserName\":\"Long\",\"Conditions\":{},\"NextPerformerType\":\"Specific\"}','2021-03-16 21:10:41.755257'),(6,2,1,'HIGH','PROCESS STARTUP ERROR','流程实体为空!请检查数据库中的流程记录是否有效!','   at Slickflow.Engine.Xpdl.ProcessModelFactory.Create(IDbConnection conn, String processGUID, String version, IDbTransaction trans) in D:\\Cloud365\\GitHomeCore\\SlickflowCore\\Source\\Slickflow.Engine\\Xpdl\\ProcessModelFactory.cs:line 60\r\n   at Slickflow.Engine.Xpdl.ProcessModelFactory.Create(String processGUID, String version) in D:\\Cloud365\\GitHomeCore\\SlickflowCore\\Source\\Slickflow.Engine\\Xpdl\\ProcessModelFactory.cs:line 29\r\n   at Slickflow.Engine.Core.Runtime.WfRuntimeManagerFactory.CreateRuntimeInstanceStartup(WfAppRunner runner, WfExecutedResult& result) in D:\\Cloud365\\GitHomeCore\\SlickflowCore\\Source\\Slickflow.Engine\\Core\\Runtime\\WfRuntimeManagerFactory.cs:line 81\r\n   at Slickflow.Engine.Service.WorkflowService.Start(IDbConnection conn, IDbTransaction trans) in D:\\Cloud365\\GitHomeCore\\SlickflowCore\\Source\\Slickflow.Engine\\Service\\WorkflowService.cs:line 656',NULL,'{\"AppName\":\"SamplePrice\",\"AppInstanceID\":\"SEQ-P-1099\",\"ProcessGUID\":\"072af8c3-482a-4b1c-890b-685ce2fcc75d\",\"Version\":\"1\",\"UserID\":\"10\",\"UserName\":\"Long\",\"Conditions\":{},\"NextPerformerType\":\"Specific\"}','2021-03-16 21:11:20.468099'),(7,2,1,'HIGH','PROCESS STARTUP ERROR','流程实体为空!请检查数据库中的流程记录是否有效!','   at Slickflow.Engine.Xpdl.ProcessModelFactory.Create(IDbConnection conn, String processGUID, String version, IDbTransaction trans) in D:\\Cloud365\\GitHomeCore\\SlickflowCore\\Source\\Slickflow.Engine\\Xpdl\\ProcessModelFactory.cs:line 60\r\n   at Slickflow.Engine.Xpdl.ProcessModelFactory.Create(String processGUID, String version) in D:\\Cloud365\\GitHomeCore\\SlickflowCore\\Source\\Slickflow.Engine\\Xpdl\\ProcessModelFactory.cs:line 29\r\n   at Slickflow.Engine.Core.Runtime.WfRuntimeManagerFactory.CreateRuntimeInstanceStartup(WfAppRunner runner, WfExecutedResult& result) in D:\\Cloud365\\GitHomeCore\\SlickflowCore\\Source\\Slickflow.Engine\\Core\\Runtime\\WfRuntimeManagerFactory.cs:line 81\r\n   at Slickflow.Engine.Service.WorkflowService.Start(IDbConnection conn, IDbTransaction trans) in D:\\Cloud365\\GitHomeCore\\SlickflowCore\\Source\\Slickflow.Engine\\Service\\WorkflowService.cs:line 656',NULL,'{\"AppName\":\"SamplePrice\",\"AppInstanceID\":\"SEQ-P-1099\",\"ProcessGUID\":\"072af8c3-482a-4b1c-890b-685ce2fcc75d\",\"Version\":\"1\",\"UserID\":\"10\",\"UserName\":\"Long\",\"Conditions\":{},\"NextPerformerType\":\"Specific\"}','2021-03-16 21:12:20.643675');
/*!40000 ALTER TABLE `wflog` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `wfprocess`
--

DROP TABLE IF EXISTS `wfprocess`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `wfprocess` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `ProcessGUID` varchar(100) DEFAULT NULL,
  `Version` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT '1',
  `ProcessName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProcessCode` varchar(50) NOT NULL,
  `IsUsing` tinyint unsigned NOT NULL DEFAULT '0',
  `AppType` varchar(20) DEFAULT NULL,
  `PackageType` tinyint unsigned DEFAULT NULL,
  `PackageProcessID` int DEFAULT NULL,
  `PageUrl` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `XmlFileName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `XmlFilePath` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `XmlContent` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `StartType` tinyint unsigned NOT NULL DEFAULT '0',
  `StartExpression` varchar(100) DEFAULT NULL,
  `Description` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `EndType` tinyint unsigned NOT NULL DEFAULT '0',
  `EndExpression` varchar(100) DEFAULT NULL,
  `CreatedDateTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpdatedDateTime` datetime(6) DEFAULT NULL,
  `RowVersionID` varbinary(8) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `wfprocess`
--

LOCK TABLES `wfprocess` WRITE;
/*!40000 ALTER TABLE `wfprocess` DISABLE KEYS */;
INSERT INTO `wfprocess` VALUES (4,'b46f37e6-840e-4b22-bb10-96af7bcdb82a','1','simpletest','simpletestcode',1,NULL,NULL,NULL,NULL,NULL,NULL,'<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<Package>\n	<Participants/>\n	<Layout>\n		<Groups/>\n		<Messages/>\n	</Layout>\n	<WorkflowProcesses>\n		<Process id=\"b46f37e6-840e-4b22-bb10-96af7bcdb82a\" name=\"simpletest\" code=\"simpletestcode\" package=\"null\">\n			<Description></Description>\n			<Activities>\n				<Activity id=\"7024ebee-daa2-4f22-b75d-efe869670645\" name=\"Start\" code=\"Start\" url=\"\">\n					<Description></Description>\n					<ActivityType type=\"StartNode\" trigger=\"null\" expression=\"null\" messageDirection=\"null\"/>\n					<Geography parent=\"77f7f349-88e6-4f6d-8999-f56f124b31ec\" style=\"symbol;image=Scripts/mxGraph/src/editor/images/symbols/event.png\">\n						<Widget left=\"50\" top=\"160\" width=\"32\" height=\"32\"/>\n					</Geography>\n				</Activity>\n				<Activity id=\"7554a59e-37d9-49e7-b028-2fea6147e677\" name=\"Task-001\" code=\"task001\" url=\"http://www.slickflow.com\">\n					<Description></Description>\n					<ActivityType type=\"TaskNode\"/>\n					<Geography parent=\"77f7f349-88e6-4f6d-8999-f56f124b31ec\" style=\"\">\n						<Widget left=\"210\" top=\"160\" width=\"72\" height=\"32\"/>\n					</Geography>\n				</Activity>\n				<Activity id=\"186e3635-5f50-4e91-a93a-dde97a2b3d45\" name=\"Task-002\" code=\"task002\" url=\"\">\n					<Description></Description>\n					<ActivityType type=\"TaskNode\"/>\n					<Geography parent=\"77f7f349-88e6-4f6d-8999-f56f124b31ec\" style=\"\">\n						<Widget left=\"370\" top=\"160\" width=\"72\" height=\"32\"/>\n					</Geography>\n				</Activity>\n				<Activity id=\"8a7566b4-5636-4dce-a5d8-78625375e29c\" name=\"Task-003\" code=\"task003\" url=\"\">\n					<Description></Description>\n					<ActivityType type=\"TaskNode\"/>\n					<Geography parent=\"77f7f349-88e6-4f6d-8999-f56f124b31ec\" style=\"\">\n						<Widget left=\"530\" top=\"160\" width=\"72\" height=\"32\"/>\n					</Geography>\n				</Activity>\n				<Activity id=\"5a1340c9-db08-4cbe-8772-12228f261802\" name=\"End\" code=\"End\" url=\"\">\n					<Description></Description>\n					<ActivityType type=\"EndNode\" trigger=\"null\" expression=\"null\" messageDirection=\"null\"/>\n					<Geography parent=\"77f7f349-88e6-4f6d-8999-f56f124b31ec\" style=\"symbol;image=Scripts/mxGraph/src/editor/images/symbols/event_end.png\">\n						<Widget left=\"740\" top=\"160\" width=\"32\" height=\"32\"/>\n					</Geography>\n				</Activity>\n			</Activities>\n			<Transitions>\n				<Transition id=\"d6d2447f-8b63-4fef-b548-13af71847dd6\" from=\"7024ebee-daa2-4f22-b75d-efe869670645\" to=\"7554a59e-37d9-49e7-b028-2fea6147e677\">\n					<Description></Description>\n					<Geography parent=\"77f7f349-88e6-4f6d-8999-f56f124b31ec\" style=\"null\"/>\n				</Transition>\n				<Transition id=\"29435200-8f97-4aa6-8053-416a33867b68\" from=\"7554a59e-37d9-49e7-b028-2fea6147e677\" to=\"186e3635-5f50-4e91-a93a-dde97a2b3d45\">\n					<Description>t-001</Description>\n					<Geography parent=\"77f7f349-88e6-4f6d-8999-f56f124b31ec\" style=\"null\"/>\n				</Transition>\n				<Transition id=\"b2821570-5a34-497d-9a46-4768ee682bae\" from=\"186e3635-5f50-4e91-a93a-dde97a2b3d45\" to=\"8a7566b4-5636-4dce-a5d8-78625375e29c\">\n					<Description></Description>\n					<Geography parent=\"77f7f349-88e6-4f6d-8999-f56f124b31ec\" style=\"null\"/>\n				</Transition>\n				<Transition id=\"7435fdca-8cb3-49ad-9d3d-acac634c3ee1\" from=\"8a7566b4-5636-4dce-a5d8-78625375e29c\" to=\"5a1340c9-db08-4cbe-8772-12228f261802\">\n					<Description></Description>\n					<Geography parent=\"77f7f349-88e6-4f6d-8999-f56f124b31ec\" style=\"null\"/>\n				</Transition>\n			</Transitions>\n		</Process>\n	</WorkflowProcesses>\n</Package>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              ',0,NULL,'',0,NULL,'2021-03-16 13:06:43','2021-03-16 21:06:45.451926',NULL),(5,'072af8c3-482a-4b1c-890b-685ce2fcc75d','1','PriceProcess(SequenceTest)','PriceProcessCode',1,NULL,NULL,NULL,NULL,NULL,'\\','<?xml version=\"1.0\" encoding=\"UTF-8\"?> <Package>  <Participants>   <Participant type=\"Role\" id=\"60c8a694-632a-4ded-9155-f666e461b078\" name=\"业务员(Sales)\" code=\"salesmate\" outerId=\"9\"/>   <Participant type=\"Role\" id=\"7f9be0fb-7ffa-4b57-8c88-26734fbe3cf6\" name=\"打样员(Tech)\" code=\"techmate\" outerId=\"10\"/>  </Participants>  <Layout>   <Groups/>   <Messages/>  </Layout>  <WorkflowProcesses>   <Process id=\"072af8c3-482a-4b1c-890b-685ce2fcc75d\" name=\"PriceProcess(SequenceTest)\" code=\"PriceProcessCode\" package=\"null\">    <Description>null</Description>    <Activities>     <Activity id=\"9b78486d-5b8f-4be4-948e-522356e84e79\" name=\"Start\" code=\"AJBNOX\" url=\"null\">      <Description></Description>      <ActivityType type=\"StartNode\" trigger=\"null\" expression=\"null\" messageDirection=\"null\"/>      <Geography parent=\"382eebd0-9250-46ef-b235-a97b64ebaa57\" style=\"symbol;image=scripts/mxGraph/src/editor/images/symbols/event.png\">       <Widget left=\"140\" top=\"117\" width=\"38\" height=\"38\"/>      </Geography>     </Activity>     <Activity id=\"b53eb9ab-3af6-41ad-d722-bed946d19792\" name=\"End\" code=\"9IQ4FV\" url=\"null\">      <Description></Description>      <ActivityType type=\"EndNode\" trigger=\"null\" expression=\"null\" messageDirection=\"null\"/>      <Geography parent=\"382eebd0-9250-46ef-b235-a97b64ebaa57\" style=\"symbol;image=scripts/mxGraph/src/editor/images/symbols/event_end.png\">       <Widget left=\"820\" top=\"117\" width=\"38\" height=\"38\"/>      </Geography>     </Activity>     <Activity id=\"3c438212-4863-4ff8-efc9-a9096c4a8230\" name=\"Sales Submit\" code=\"5Q1Q82\" url=\"null\">      <Description></Description>      <ActivityType type=\"TaskNode\"/>      <Performers>       <Performer id=\"60c8a694-632a-4ded-9155-f666e461b078\"/>      </Performers>      <Boundaries>       <Boundary event=\"Timer\" expression=\"PT5M\"/>      </Boundaries>      <Sections>       <Section name=\"myProperties\">        <![CDATA[{\"Fruit\": \"apple\"}]]>       </Section>      </Sections>      <Geography parent=\"382eebd0-9250-46ef-b235-a97b64ebaa57\" style=\"undefined\">       <Widget left=\"280\" top=\"123\" width=\"100\" height=\"27\"/>      </Geography>     </Activity>     <Activity id=\"eb833577-abb5-4239-875a-5f2e2fcb6d57\" name=\"Manager Signature\" code=\"HNGPSC\" url=\"null\">      <Description></Description>      <ActivityType type=\"TaskNode\"/>      <Performers>       <Performer id=\"7f9be0fb-7ffa-4b57-8c88-26734fbe3cf6\"/>      </Performers>      <Boundaries>       <Boundary event=\"Timer\" expression=\"\"/>      </Boundaries>      <Sections>       <Section name=\"myProperties\">        <![CDATA[{ \"Fruit\": \"orange\", \"Book\": \"story\" }]]>       </Section>      </Sections>      <Geography parent=\"382eebd0-9250-46ef-b235-a97b64ebaa57\" style=\"undefined\">       <Widget left=\"450\" top=\"120\" width=\"120\" height=\"32\"/>      </Geography>     </Activity>     <Activity id=\"cab57060-f433-422a-a66f-4a5ecfafd54e\" name=\"Sales Confirm\" code=\"9S66UP\" url=\"null\">      <Description></Description>      <ActivityType type=\"TaskNode\"/>      <Performers>       <Performer id=\"60c8a694-632a-4ded-9155-f666e461b078\"/>      </Performers>      <Geography parent=\"382eebd0-9250-46ef-b235-a97b64ebaa57\" style=\"undefined\">       <Widget left=\"640\" top=\"123\" width=\"90\" height=\"27\"/>      </Geography>     </Activity>    </Activities>    <Transitions>     <Transition id=\"5432de95-cbcd-4349-9cf0-7e67904c52aa\" from=\"3c438212-4863-4ff8-efc9-a9096c4a8230\" to=\"eb833577-abb5-4239-875a-5f2e2fcb6d57\">      <Description></Description>      <Condition type=\"Expression\">       <ConditionText/>      </Condition>      <Receiver/>      <Geography parent=\"382eebd0-9250-46ef-b235-a97b64ebaa57\" style=\"undefined\"/>     </Transition>     <Transition id=\"ac609b39-b6eb-4506-c36f-670c5ed53f5c\" from=\"eb833577-abb5-4239-875a-5f2e2fcb6d57\" to=\"cab57060-f433-422a-a66f-4a5ecfafd54e\">      <Description></Description>      <Condition type=\"Expression\">       <ConditionText/>      </Condition>      <Receiver/>      <Geography parent=\"382eebd0-9250-46ef-b235-a97b64ebaa57\" style=\"undefined\"/>     </Transition>     <Transition id=\"2d5c0e7b-1303-48cb-c22b-3cd2b45701e3\" from=\"cab57060-f433-422a-a66f-4a5ecfafd54e\" to=\"b53eb9ab-3af6-41ad-d722-bed946d19792\">      <Description></Description>      <Condition type=\"Expression\">       <ConditionText/>      </Condition>      <Receiver/>      <Geography parent=\"382eebd0-9250-46ef-b235-a97b64ebaa57\" style=\"undefined\"/>     </Transition>     <Transition id=\"9cf01621-2dd5-474a-8889-cdbe53a0b72e\" from=\"9b78486d-5b8f-4be4-948e-522356e84e79\" to=\"3c438212-4863-4ff8-efc9-a9096c4a8230\">      <Description></Description>      <Condition type=\"Expression\">       <ConditionText/>      </Condition>      <Receiver/>      <Geography parent=\"382eebd0-9250-46ef-b235-a97b64ebaa57\" style=\"undefined\"/>     </Transition>    </Transitions>   </Process>  </WorkflowProcesses> </Package>',0,NULL,NULL,0,NULL,'2021-03-17 01:26:41','2021-03-17 09:26:41.061087',NULL),(6,'3a8ce214-fd18-4fac-95c0-e7958bc1b2f8','1','办公用品(SplitJoinTest)','vgn306IP',1,NULL,NULL,NULL,NULL,NULL,'\\','<?xml version=\"1.0\" encoding=\"UTF-8\"?> <Package>  <Participants>   <Participant type=\"Role\" id=\"114e7e8d-574c-42c2-eb1c-3d7160516ba3\" name=\"普通员工\" code=\"employees\" outerId=\"1\"/>   <Participant type=\"Role\" id=\"595410fc-2f24-4708-bacd-0eb38b17e7fc\" name=\"人事经理\" code=\"hrmanager\" outerId=\"3\"/>   <Participant type=\"Role\" id=\"c9694802-fcb1-4cad-ad9e-aae9894305a6\" name=\"总经理\" code=\"generalmanager\" outerId=\"8\"/>   <Participant type=\"Role\" id=\"db7031ac-c0b4-4691-d6e0-195e66be6fe1\" name=\"财务经理\" code=\"finacemanager\" outerId=\"14\"/>  </Participants>  <WorkflowProcesses>   <Process name=\"办公用品(SplitJoinTest)\" id=\"3a8ce214-fd18-4fac-95c0-e7958bc1b2f8\">    <Description>null</Description>    <Activities>     <Activity id=\"e52d0836-9f98-4b70-d485-6b01b8cc277e\" name=\"开始\" code=\"\" url=\"null\">      <Description></Description>      <ActivityType type=\"StartNode\" trigger=\"null\"/>      <Geography parent=\"6a788335-b5c8-4c94-f48f-f605aec62c65\" style=\"symbol;image=scripts/mxGraph/src/editor/images/symbols/event.png\">       <Widget left=\"160\" top=\"141\" width=\"38\" height=\"38\"/>      </Geography>     </Activity>     <Activity id=\"30929bbb-c76e-4604-c956-f26feb4aa22e\" name=\"结束\" code=\"\" url=\"null\">      <Description></Description>      <ActivityType type=\"EndNode\" trigger=\"null\"/>      <Geography parent=\"6a788335-b5c8-4c94-f48f-f605aec62c65\" style=\"symbol;image=scripts/mxGraph/src/editor/images/symbols/event_end.png\">       <Widget left=\"876\" top=\"141\" width=\"38\" height=\"38\"/>      </Geography>     </Activity>     <Activity id=\"4db4a153-c8fc-45df-b067-9d188ae19a41\" name=\"仓库签字\" code=\"\" url=\"null\">      <Description></Description>      <ActivityType type=\"TaskNode\"/>      <Performers>       <Performer id=\"114e7e8d-574c-42c2-eb1c-3d7160516ba3\"/>      </Performers>      <Boundaries>       <Boundary event=\"Timer\" expression=\"\"/>      </Boundaries>      <Sections>       <Section name=\"myProperties\">        <![CDATA[]]>       </Section>      </Sections>      <Geography parent=\"6a788335-b5c8-4c94-f48f-f605aec62c65\" style=\"undefined\">       <Widget left=\"280\" top=\"146\" width=\"67\" height=\"27\"/>      </Geography>     </Activity>     <Activity id=\"eb492ba8-075a-46e4-b95f-ac071dd3a43d\" name=\"Gateway\" code=\"\" url=\"null\">      <Description></Description>      <ActivityType type=\"GatewayNode\" gatewaySplitJoinType=\"Split\" gatewayDirection=\"OrSplit\" gatewayJoinPass=\"null\"/>      <Geography parent=\"6a788335-b5c8-4c94-f48f-f605aec62c65\" style=\"symbol;image=scripts/mxGraph/src/editor/images/symbols/fork.png\">       <Widget left=\"414\" top=\"141\" width=\"38\" height=\"38\"/>      </Geography>     </Activity>     <Activity id=\"c3cbb3cc-fa60-42ad-9a10-4ec2638aff49\" name=\"行政部签字\" code=\"\" url=\"null\">      <Description></Description>      <ActivityType type=\"TaskNode\"/>      <Performers>       <Performer id=\"595410fc-2f24-4708-bacd-0eb38b17e7fc\"/>      </Performers>      <Geography parent=\"6a788335-b5c8-4c94-f48f-f605aec62c65\" style=\"undefined\">       <Widget left=\"553\" top=\"60\" width=\"67\" height=\"27\"/>      </Geography>     </Activity>     <Activity id=\"12c6c0d2-1d23-4ed1-8d58-ddc4268f3149\" name=\"总经理签字\" code=\"\" url=\"null\">      <Description></Description>      <ActivityType type=\"TaskNode\"/>      <Performers>       <Performer id=\"c9694802-fcb1-4cad-ad9e-aae9894305a6\"/>      </Performers>      <Geography parent=\"6a788335-b5c8-4c94-f48f-f605aec62c65\" style=\"undefined\">       <Widget left=\"555\" top=\"220\" width=\"67\" height=\"27\"/>      </Geography>     </Activity>     <Activity id=\"9414c43c-0c8c-4c0b-b65d-16203288c7ca\" name=\"财务签字\" code=\"\" url=\"null\">      <Description></Description>      <ActivityType type=\"TaskNode\"/>      <Performers>       <Performer id=\"db7031ac-c0b4-4691-d6e0-195e66be6fe1\"/>      </Performers>      <Geography parent=\"6a788335-b5c8-4c94-f48f-f605aec62c65\" style=\"undefined\">       <Widget left=\"555\" top=\"147\" width=\"67\" height=\"27\"/>      </Geography>     </Activity>     <Activity id=\"932f7fa0-2d4c-4257-c158-b8b181af2d0a\" name=\"财务主管\" code=\"\" url=\"null\">      <Description></Description>      <ActivityType type=\"TaskNode\"/>      <Performers>       <Performer id=\"db7031ac-c0b4-4691-d6e0-195e66be6fe1\"/>      </Performers>      <Actions>       <Action type=\"ExternalMethod\"/>      </Actions>      <Boundaries>       <Boundary event=\"Timer\" expression=\"\"/>      </Boundaries>      <Geography parent=\"6a788335-b5c8-4c94-f48f-f605aec62c65\" style=\"undefined\">       <Widget left=\"734\" top=\"144\" width=\"72\" height=\"32\"/>      </Geography>     </Activity>    </Activities>    <Transitions>     <Transition id=\"81fdf756-ecd5-43c0-e2b3-25770aab5dee\" from=\"e52d0836-9f98-4b70-d485-6b01b8cc277e\" to=\"4db4a153-c8fc-45df-b067-9d188ae19a41\">      <Description></Description>      <Condition type=\"Expression\">       <ConditionText/>      </Condition>      <Receiver/>      <Geography parent=\"6a788335-b5c8-4c94-f48f-f605aec62c65\" style=\"undefined\"/>     </Transition>     <Transition id=\"69c1ba54-acb0-4b4e-ff03-3f6cf572e98a\" from=\"4db4a153-c8fc-45df-b067-9d188ae19a41\" to=\"eb492ba8-075a-46e4-b95f-ac071dd3a43d\">      <Description></Description>      <Condition type=\"Expression\">       <ConditionText/>      </Condition>      <Receiver/>      <Geography parent=\"6a788335-b5c8-4c94-f48f-f605aec62c65\" style=\"undefined\"/>     </Transition>     <Transition id=\"8d776249-f3c6-4397-817f-44880b34a451\" from=\"eb492ba8-075a-46e4-b95f-ac071dd3a43d\" to=\"c3cbb3cc-fa60-42ad-9a10-4ec2638aff49\">      <Description>正常</Description>      <Condition type=\"Expression\">       <ConditionText>        <![CDATA[surplus = \"normal\"]]>       </ConditionText>      </Condition>      <GroupBehaviours/>      <Receiver/>      <Geography parent=\"6a788335-b5c8-4c94-f48f-f605aec62c65\" style=\"undefined\"/>     </Transition>     <Transition id=\"e40270aa-834a-455d-ffd6-b3f72feeeadc\" from=\"eb492ba8-075a-46e4-b95f-ac071dd3a43d\" to=\"12c6c0d2-1d23-4ed1-8d58-ddc4268f3149\">      <Description>超量</Description>      <Condition type=\"Expression\">       <ConditionText>        <![CDATA[surplus = \"overamount\"]]>       </ConditionText>      </Condition>      <Receiver/>      <Geography parent=\"6a788335-b5c8-4c94-f48f-f605aec62c65\" style=\"undefined\"/>     </Transition>     <Transition id=\"952b3594-fe40-427f-a27a-f2650226aeca\" from=\"c3cbb3cc-fa60-42ad-9a10-4ec2638aff49\" to=\"932f7fa0-2d4c-4257-c158-b8b181af2d0a\">      <Description></Description>      <Condition type=\"Expression\">       <ConditionText/>      </Condition>      <Receiver/>      <Geography parent=\"6a788335-b5c8-4c94-f48f-f605aec62c65\" style=\"undefined\"/>     </Transition>     <Transition id=\"fd39de26-d9e9-425e-c952-dd8c37d329d6\" from=\"12c6c0d2-1d23-4ed1-8d58-ddc4268f3149\" to=\"932f7fa0-2d4c-4257-c158-b8b181af2d0a\">      <Description></Description>      <Condition type=\"Expression\">       <ConditionText/>      </Condition>      <Receiver/>      <Geography parent=\"6a788335-b5c8-4c94-f48f-f605aec62c65\" style=\"undefined\"/>     </Transition>     <Transition id=\"6af8936c-a467-470a-f389-d0a3dcc3739b\" from=\"eb492ba8-075a-46e4-b95f-ac071dd3a43d\" to=\"9414c43c-0c8c-4c0b-b65d-16203288c7ca\">      <Description>正常</Description>      <Condition type=\"Expression\">       <ConditionText>        <![CDATA[surplus = \"normal\"]]>       </ConditionText>      </Condition>      <Receiver/>      <Geography parent=\"6a788335-b5c8-4c94-f48f-f605aec62c65\" style=\"undefined\"/>     </Transition>     <Transition id=\"ec4b9497-c187-40a0-af21-1bc3401eb2cf\" from=\"9414c43c-0c8c-4c0b-b65d-16203288c7ca\" to=\"932f7fa0-2d4c-4257-c158-b8b181af2d0a\">      <Description></Description>      <Condition type=\"Expression\">       <ConditionText/>      </Condition>      <Receiver/>      <Geography parent=\"6a788335-b5c8-4c94-f48f-f605aec62c65\" style=\"undefined\"/>     </Transition>     <Transition id=\"4b8a68d6-ef32-420a-93e7-33c7e4b80360\" from=\"932f7fa0-2d4c-4257-c158-b8b181af2d0a\" to=\"30929bbb-c76e-4604-c956-f26feb4aa22e\">      <Description></Description>      <Condition type=\"null\">       <ConditionText/>      </Condition>      <Receiver type=\"Default\"/>      <Geography parent=\"6a788335-b5c8-4c94-f48f-f605aec62c65\" style=\"undefined\"/>     </Transition>    </Transitions>   </Process>  </WorkflowProcesses>  <Layout>   <Swimlanes/>   <Groups/>  </Layout> </Package>',0,NULL,NULL,0,NULL,'2021-03-17 01:28:15','2021-03-17 09:28:14.608746',NULL),(7,'805a2af4-5196-4461-8b94-ec57714dfd9d','1','子流程Main(SubProcessMain)','tpk170CX',1,NULL,NULL,NULL,NULL,NULL,'\\','<?xml version=\"1.0\" encoding=\"UTF-8\"?> <Package>  <Participants>   <Participant type=\"Role\" id=\"dbb4dcfd-a288-4bc6-a2ba-0288dcd51ea3\" name=\"普通员工\" code=\"employees\" outerId=\"1\"/>   <Participant type=\"Role\" id=\"f137400d-0659-4a92-e433-9868d0411279\" name=\"testrole\" code=\"testrole\" outerId=\"21\"/>   <Participant type=\"Role\" id=\"89e87b2b-6c39-43f3-c647-2a968f1899c1\" name=\"人事经理\" code=\"hrmanager\" outerId=\"3\"/>  </Participants>  <WorkflowProcesses>   <Process name=\"子流程Main(SubProcessMain)\" id=\"805a2af4-5196-4461-8b94-ec57714dfd9d\">    <Description>null</Description>    <Activities>     <Activity id=\"39778075-73b1-43ed-d49f-da9c2e26d58c\" name=\"开始\" code=\"\" url=\"null\">      <Description></Description>      <ActivityType type=\"StartNode\" trigger=\"None\"/>      <Geography parent=\"dd12eb8a-4bbe-45b3-9263-69d3c574724b\" style=\"symbol;image=scripts/mxGraph/src/editor/images/symbols/event.png\">       <Widget left=\"100\" top=\"195\" width=\"32\" height=\"32\"/>      </Geography>     </Activity>     <Activity id=\"f8de1810-2db4-4f9d-fea1-2b6d33d02c24\" name=\"结束\" code=\"\" url=\"null\">      <Description></Description>      <ActivityType type=\"EndNode\" trigger=\"null\"/>      <Geography parent=\"dd12eb8a-4bbe-45b3-9263-69d3c574724b\" style=\"symbol;image=scripts/mxGraph/src/editor/images/symbols/event_end.png\">       <Widget left=\"712\" top=\"192\" width=\"38\" height=\"38\"/>      </Geography>     </Activity>     <Activity id=\"1122ea0a-6c06-42f7-9b2f-72c1ddea38a5\" name=\"申请\" code=\"\" url=\"null\">      <Description></Description>      <ActivityType type=\"TaskNode\"/>      <Performers>       <Performer id=\"dbb4dcfd-a288-4bc6-a2ba-0288dcd51ea3\"/>      </Performers>      <Actions>       <Action type=\"ExternalMethod\"/>      </Actions>      <Geography parent=\"dd12eb8a-4bbe-45b3-9263-69d3c574724b\" style=\"undefined\">       <Widget left=\"230\" top=\"195\" width=\"72\" height=\"32\"/>      </Geography>     </Activity>     <Activity id=\"1689ba04-cebc-4ae3-d7af-2075b81f99c4\" name=\"财务内部审批子流程\" code=\"\" url=\"null\">      <Description></Description>      <ActivityType type=\"SubProcessNode\" subId=\"f0782fc8-43f1-4520-bed9-f37fcbdede99\"/>      <Performers>       <Performer id=\"f137400d-0659-4a92-e433-9868d0411279\"/>      </Performers>      <Actions>       <Action type=\"null\"/>      </Actions>      <Geography parent=\"dd12eb8a-4bbe-45b3-9263-69d3c574724b\" style=\"rounded\">       <Widget left=\"400\" top=\"195\" width=\"72\" height=\"32\"/>      </Geography>     </Activity>     <Activity id=\"a89e17ef-e213-4d2d-d4d1-20dcec40d6c2\" name=\"归档\" code=\"\" url=\"null\">      <Description></Description>      <ActivityType type=\"TaskNode\"/>      <Performers>       <Performer id=\"89e87b2b-6c39-43f3-c647-2a968f1899c1\"/>      </Performers>      <Actions>       <Action type=\"ExternalMethod\"/>      </Actions>      <Geography parent=\"dd12eb8a-4bbe-45b3-9263-69d3c574724b\" style=\"undefined\">       <Widget left=\"560\" top=\"195\" width=\"72\" height=\"32\"/>      </Geography>     </Activity>    </Activities>    <Transitions>     <Transition id=\"dc8f24ea-c33c-4bce-c8eb-d2f66edfa64d\" from=\"39778075-73b1-43ed-d49f-da9c2e26d58c\" to=\"1122ea0a-6c06-42f7-9b2f-72c1ddea38a5\">      <Description></Description>      <Condition type=\"null\">       <ConditionText/>      </Condition>      <Receiver type=\"Default\"/>      <Geography parent=\"dd12eb8a-4bbe-45b3-9263-69d3c574724b\" style=\"undefined\"/>     </Transition>     <Transition id=\"cee2428c-7fd1-4864-db3d-585df2bb39a4\" from=\"1122ea0a-6c06-42f7-9b2f-72c1ddea38a5\" to=\"1689ba04-cebc-4ae3-d7af-2075b81f99c4\">      <Description></Description>      <Condition type=\"null\">       <ConditionText/>      </Condition>      <Receiver type=\"Default\"/>      <Geography parent=\"dd12eb8a-4bbe-45b3-9263-69d3c574724b\" style=\"undefined\"/>     </Transition>     <Transition id=\"32e5ddd3-cf50-4843-d49c-e46a68737361\" from=\"1689ba04-cebc-4ae3-d7af-2075b81f99c4\" to=\"a89e17ef-e213-4d2d-d4d1-20dcec40d6c2\">      <Description></Description>      <Condition type=\"null\">       <ConditionText/>      </Condition>      <Receiver type=\"Default\"/>      <Geography parent=\"dd12eb8a-4bbe-45b3-9263-69d3c574724b\" style=\"undefined\"/>     </Transition>     <Transition id=\"9c371cfd-d68d-44bb-8258-6b6cb729fe42\" from=\"a89e17ef-e213-4d2d-d4d1-20dcec40d6c2\" to=\"f8de1810-2db4-4f9d-fea1-2b6d33d02c24\">      <Description></Description>      <Condition type=\"null\">       <ConditionText/>      </Condition>      <Receiver type=\"Default\"/>      <Geography parent=\"dd12eb8a-4bbe-45b3-9263-69d3c574724b\" style=\"undefined\"/>     </Transition>    </Transitions>   </Process>  </WorkflowProcesses>  <Layout>   <Swimlanes/>   <Groups/>  </Layout> </Package>',0,NULL,NULL,0,NULL,'2021-03-17 01:29:15','2021-03-17 09:29:15.423618',NULL),(8,'9fb4bca4-5674-4181-a010-f0e730e166dd','1','报价会签(SignTogetherTest)','rna512IV',1,NULL,NULL,NULL,NULL,NULL,'\\','<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<Package>\n	<Participants/>\n	<Layout>\n		<Groups/>\n		<Messages/>\n	</Layout>\n	<WorkflowProcesses>\n		<Process id=\"9fb4bca4-5674-4181-a010-f0e730e166dd\" name=\"报价会签(SignTogetherTest)\" code=\"rna512IV\" package=\"null\">\n			<Description>null</Description>\n			<Activities>\n				<Activity id=\"1f303f19-71aa-4879-c501-f4d0f448f0a2\" name=\"开始\" code=\"WVYUM1\" url=\"null\">\n					<Description></Description>\n					<ActivityType type=\"StartNode\" trigger=\"null\" expression=\"null\" messageDirection=\"null\"/>\n					<Geography parent=\"ec22fe46-8959-4445-9177-96e80b7e35a9\" style=\"symbol;image=scripts/mxGraph/src/editor/images/symbols/event.png\">\n						<Widget left=\"165\" top=\"116\" width=\"38\" height=\"38\"/>\n					</Geography>\n				</Activity>\n				<Activity id=\"7462aae9-da1c-43f0-d741-a4586879de77\" name=\"结束\" code=\"KESZ09\" url=\"null\">\n					<Description></Description>\n					<ActivityType type=\"EndNode\" trigger=\"null\" expression=\"null\" messageDirection=\"null\"/>\n					<Geography parent=\"ec22fe46-8959-4445-9177-96e80b7e35a9\" style=\"symbol;image=scripts/mxGraph/src/editor/images/symbols/event_end.png\">\n						<Widget left=\"770\" top=\"116\" width=\"38\" height=\"38\"/>\n					</Geography>\n				</Activity>\n				<Activity id=\"791d9d3a-882d-4796-cffc-84d9fca76afd\" name=\"业务员提交\" code=\"EIXGXT\" url=\"null\">\n					<Description></Description>\n					<ActivityType type=\"TaskNode\"/>\n					<Boundaries>\n						<Boundary event=\"Timer\" expression=\"\"/>\n					</Boundaries>\n					<Sections>\n						<Section name=\"myProperties\">\n							<![CDATA[]]>\n						</Section>\n					</Sections>\n					<Geography parent=\"ec22fe46-8959-4445-9177-96e80b7e35a9\" style=\"undefined\">\n						<Widget left=\"303\" top=\"121\" width=\"67\" height=\"27\"/>\n					</Geography>\n				</Activity>\n				<Activity id=\"23017d0c-08ca-4a59-9649-c6912b819001\" name=\"业务员确认\" code=\"AYZRCN\" url=\"\">\n					<Description></Description>\n					<ActivityType type=\"TaskNode\"/>\n					<Boundaries>\n						<Boundary event=\"Timer\" expression=\"\"/>\n					</Boundaries>\n					<Sections>\n						<Section name=\"myProperties\">\n							<![CDATA[]]>\n						</Section>\n					</Sections>\n					<Geography parent=\"ec22fe46-8959-4445-9177-96e80b7e35a9\" style=\"undefined\">\n						<Widget left=\"621\" top=\"121\" width=\"67\" height=\"27\"/>\n					</Geography>\n				</Activity>\n				<Activity id=\"36cf2479-e8ec-4936-8bcd-b38101e4664a\" name=\"板房会签\" code=\"0O88XX\" url=\"\">\n					<Description></Description>\n					<ActivityType type=\"MultipleInstanceNode\" complexType=\"SignTogether\" mergeType=\"Sequence\" compareType=\"Count\" completeOrder=\"3\"/>\n					<Boundaries>\n						<Boundary event=\"Timer\" expression=\"\"/>\n					</Boundaries>\n					<Sections>\n						<Section name=\"myProperties\">\n							<![CDATA[]]>\n						</Section>\n					</Sections>\n					<Geography parent=\"ec22fe46-8959-4445-9177-96e80b7e35a9\" style=\"symbol;image=scripts/mxGraph/src/editor/images/symbols/samll_multiple_instance_task.png\">\n						<Widget left=\"472\" top=\"121\" width=\"67\" height=\"27\"/>\n					</Geography>\n				</Activity>\n			</Activities>\n			<Transitions>\n				<Transition id=\"50f7acb2-99d0-4877-e116-5bf19433bb89\" from=\"1f303f19-71aa-4879-c501-f4d0f448f0a2\" to=\"791d9d3a-882d-4796-cffc-84d9fca76afd\">\n					<Description></Description>\n					<Condition type=\"Expression\">\n						<ConditionText/>\n					</Condition>\n					<Receiver/>\n					<Geography parent=\"ec22fe46-8959-4445-9177-96e80b7e35a9\" style=\"undefined\"/>\n				</Transition>\n				<Transition id=\"87651a0d-81e5-4d6f-9ef3-ed0be0011c8f\" from=\"791d9d3a-882d-4796-cffc-84d9fca76afd\" to=\"36cf2479-e8ec-4936-8bcd-b38101e4664a\">\n					<Description></Description>\n					<Condition type=\"Expression\">\n						<ConditionText/>\n					</Condition>\n					<Receiver/>\n					<Geography parent=\"ec22fe46-8959-4445-9177-96e80b7e35a9\" style=\"undefined\"/>\n				</Transition>\n				<Transition id=\"63031ecf-2116-47a3-a0d8-f920dc5bee11\" from=\"36cf2479-e8ec-4936-8bcd-b38101e4664a\" to=\"23017d0c-08ca-4a59-9649-c6912b819001\">\n					<Description></Description>\n					<Condition type=\"Expression\">\n						<ConditionText/>\n					</Condition>\n					<Receiver/>\n					<Geography parent=\"ec22fe46-8959-4445-9177-96e80b7e35a9\" style=\"undefined\"/>\n				</Transition>\n				<Transition id=\"3d06aebb-2fb3-4995-e0c7-99d488f8312d\" from=\"23017d0c-08ca-4a59-9649-c6912b819001\" to=\"7462aae9-da1c-43f0-d741-a4586879de77\">\n					<Description></Description>\n					<Condition type=\"Expression\">\n						<ConditionText/>\n					</Condition>\n					<Receiver/>\n					<Geography parent=\"ec22fe46-8959-4445-9177-96e80b7e35a9\" style=\"undefined\"/>\n				</Transition>\n			</Transitions>\n		</Process>\n	</WorkflowProcesses>\n</Package>',0,NULL,NULL,0,NULL,'2021-03-17 01:30:28','2021-03-17 09:30:58.875287',NULL),(9,'1bc22da3-47e3-4a0a-be81-6d7297ad3aca','1','报价加签(SignForwardTest)','xda858SW',1,NULL,NULL,NULL,NULL,NULL,'\\','<?xml version=\"1.0\" encoding=\"UTF-8\"?> <Package>  <Participants>   <Participant type=\"Role\" id=\"28e71769-f197-4fe0-fd9f-63474956dc60\" name=\"业务员(Sales)\" code=\"salesmate\" outerId=\"9\"/>   <Participant type=\"Role\" id=\"24b1a282-d4d4-4461-febb-2f28eb31f48f\" name=\"打样员(Tech)\" code=\"techmate\" outerId=\"10\"/>  </Participants>  <WorkflowProcesses>   <Process name=\"报价加签(SignForwardTest)\" id=\"1bc22da3-47e3-4a0a-be81-6d7297ad3aca\">    <Description>null</Description>    <Activities>     <Activity id=\"1f303f19-71aa-4879-c501-f4d0f448f0a2\" name=\"开始\" code=\"\">      <Description>undefined</Description>      <ActivityType type=\"StartNode\" trigger=\"null\"/>      <Geography parent=\"96536a6e-8652-46a0-ee5e-0b887e0e0258\" style=\"symbol;image=scripts/mxGraph/src/editor/images/symbols/event.png\">       <Widget left=\"165\" top=\"120\" width=\"38\" height=\"38\"/>      </Geography>     </Activity>     <Activity id=\"7462aae9-da1c-43f0-d741-a4586879de77\" name=\"结束\" code=\"\">      <Description>undefined</Description>      <ActivityType type=\"EndNode\"/>      <Geography parent=\"96536a6e-8652-46a0-ee5e-0b887e0e0258\" style=\"symbol;image=scripts/mxGraph/src/editor/images/symbols/event_end.png\">       <Widget left=\"768\" top=\"124\" width=\"38\" height=\"38\"/>      </Geography>     </Activity>     <Activity id=\"791d9d3a-882d-4796-cffc-84d9fca76afd\" name=\"业务员提交\" code=\"\">      <Description>undefined</Description>      <ActivityType type=\"TaskNode\"/>      <Performers>       <Performer id=\"28e71769-f197-4fe0-fd9f-63474956dc60\"/>      </Performers>      <Geography parent=\"96536a6e-8652-46a0-ee5e-0b887e0e0258\" style=\"undefined\">       <Widget left=\"303\" top=\"121\" width=\"67\" height=\"27\"/>      </Geography>     </Activity>     <Activity id=\"23017d0c-08ca-4a59-9649-c6912b819001\" name=\"业务员确认\" code=\"\">      <Description>undefined</Description>      <ActivityType type=\"TaskNode\"/>      <Performers>       <Performer id=\"28e71769-f197-4fe0-fd9f-63474956dc60\"/>      </Performers>      <Geography parent=\"96536a6e-8652-46a0-ee5e-0b887e0e0258\" style=\"undefined\">       <Widget left=\"621\" top=\"123\" width=\"67\" height=\"27\"/>      </Geography>     </Activity>     <Activity id=\"36cf2479-e8ec-4936-8bcd-b38101e4664a\" name=\"板房加签\" code=\"\">      <Description>undefined</Description>      <ActivityType type=\"MultipleInstanceNode\" complexType=\"SignForward\" mergeType=\"Parallel\" compareType=\"Percentage\" completeOrder=\"60\"/>      <Performers>       <Performer id=\"24b1a282-d4d4-4461-febb-2f28eb31f48f\"/>      </Performers>      <Geography parent=\"96536a6e-8652-46a0-ee5e-0b887e0e0258\" style=\"symbol;image=scripts/mxGraph/src/editor/images/symbols/samll_multiple_instance_task.png\">       <Widget left=\"472\" top=\"119\" width=\"67\" height=\"27\"/>      </Geography>     </Activity>    </Activities>    <Transitions>     <Transition id=\"50f7acb2-99d0-4877-e116-5bf19433bb89\" from=\"1f303f19-71aa-4879-c501-f4d0f448f0a2\" to=\"791d9d3a-882d-4796-cffc-84d9fca76afd\">      <Description></Description>      <Receiver/>      <Condition type=\"Expression\">       <ConditionText/>      </Condition>      <Geography parent=\"96536a6e-8652-46a0-ee5e-0b887e0e0258\" style=\"undefined\"/>     </Transition>     <Transition id=\"87651a0d-81e5-4d6f-9ef3-ed0be0011c8f\" from=\"791d9d3a-882d-4796-cffc-84d9fca76afd\" to=\"36cf2479-e8ec-4936-8bcd-b38101e4664a\">      <Description></Description>      <Receiver/>      <Condition type=\"Expression\">       <ConditionText/>      </Condition>      <Geography parent=\"96536a6e-8652-46a0-ee5e-0b887e0e0258\" style=\"undefined\"/>     </Transition>     <Transition id=\"63031ecf-2116-47a3-a0d8-f920dc5bee11\" from=\"36cf2479-e8ec-4936-8bcd-b38101e4664a\" to=\"23017d0c-08ca-4a59-9649-c6912b819001\">      <Description></Description>      <Receiver/>      <Condition type=\"Expression\">       <ConditionText/>      </Condition>      <Geography parent=\"96536a6e-8652-46a0-ee5e-0b887e0e0258\" style=\"undefined\"/>     </Transition>     <Transition id=\"3d06aebb-2fb3-4995-e0c7-99d488f8312d\" from=\"23017d0c-08ca-4a59-9649-c6912b819001\" to=\"7462aae9-da1c-43f0-d741-a4586879de77\">      <Description></Description>      <Receiver/>      <Condition type=\"Expression\">       <ConditionText/>      </Condition>      <Geography parent=\"96536a6e-8652-46a0-ee5e-0b887e0e0258\" style=\"undefined\"/>     </Transition>    </Transitions>   </Process>  </WorkflowProcesses>  <Layout>   <Swimlanes/>  </Layout> </Package>',0,NULL,NULL,0,NULL,'2021-03-17 01:32:17','2021-03-17 09:32:16.872927',NULL);
/*!40000 ALTER TABLE `wfprocess` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `wfprocessinstance`
--

DROP TABLE IF EXISTS `wfprocessinstance`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `wfprocessinstance` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `ProcessGUID` varchar(100) NOT NULL,
  `ProcessName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Version` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT '1',
  `AppInstanceID` varchar(50) NOT NULL,
  `AppName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `AppInstanceCode` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ProcessState` smallint NOT NULL DEFAULT '0',
  `ParentProcessInstanceID` int DEFAULT '0',
  `ParentProcessGUID` varchar(100) DEFAULT NULL,
  `InvokedActivityInstanceID` int DEFAULT '0',
  `InvokedActivityGUID` varchar(100) DEFAULT NULL,
  `CreatedDateTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `CreatedByUserID` varchar(50) NOT NULL,
  `CreatedByUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `OverdueDateTime` datetime(6) DEFAULT NULL,
  `OverdueTreatedDateTime` datetime(6) DEFAULT NULL,
  `LastUpdatedDateTime` datetime(6) DEFAULT NULL,
  `LastUpdatedByUserID` varchar(50) DEFAULT NULL,
  `LastUpdatedByUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `EndedDateTime` datetime(6) DEFAULT NULL,
  `EndedByUserID` varchar(50) DEFAULT NULL,
  `EndedByUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `RecordStatusInvalid` tinyint unsigned NOT NULL DEFAULT '0',
  `RowVersionID` varbinary(8) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `wfprocessinstance`
--

LOCK TABLES `wfprocessinstance` WRITE;
/*!40000 ALTER TABLE `wfprocessinstance` DISABLE KEYS */;
INSERT INTO `wfprocessinstance` VALUES (1,'73bae357-0e06-42b4-83bc-3b725f6344ff','proxcessname','1','123','Order-Books','123-code',2,NULL,NULL,0,NULL,'2021-03-16 09:07:49','01','Zero',NULL,NULL,'2021-03-16 17:07:48.568791','01','Zero',NULL,NULL,NULL,0,NULL),(2,'cff73a19-5c3a-4975-88d1-549a8ca81d26','simpletest','1','123','Order-Books','123-code',2,NULL,NULL,0,NULL,'2021-03-16 13:02:59','01','Zero',NULL,NULL,'2021-03-16 21:02:59.454889','01','Zero',NULL,NULL,NULL,0,NULL),(3,'b46f37e6-840e-4b22-bb10-96af7bcdb82a','simpletest','1','123','Order-Books','123-code',4,NULL,NULL,0,NULL,'2021-03-16 13:06:56','01','Zero',NULL,NULL,'2021-03-16 21:06:56.020573','01','Zero','2021-03-16 21:07:12.335183','111','King(模拟)',0,NULL),(4,'072af8c3-482a-4b1c-890b-685ce2fcc75d','PriceProcess(SequenceTest)','1','SEQ-P-1099','SamplePrice',NULL,4,NULL,NULL,0,NULL,'2021-03-17 01:34:08','10','Long',NULL,NULL,'2021-03-17 09:34:07.944485','10','Long','2021-03-17 09:34:08.420127','10','Long',0,NULL),(5,'072af8c3-482a-4b1c-890b-685ce2fcc75d','PriceProcess(SequenceTest)','1','SEQ-C-1099','SamplePrice',NULL,4,NULL,NULL,0,NULL,'2021-03-17 01:34:08','10','Long',NULL,NULL,'2021-03-17 09:34:08.630728','10','Long','2021-03-17 09:34:08.655889','10','Long',0,NULL),(6,'3a8ce214-fd18-4fac-95c0-e7958bc1b2f8','办公用品(SplitJoinTest)','1','10998','OfficeIn',NULL,4,NULL,NULL,0,NULL,'2021-03-17 01:34:09','10','Long',NULL,NULL,'2021-03-17 09:34:08.675330','10','Long','2021-03-17 09:34:08.766583','10','Long',0,NULL),(7,'9fb4bca4-5674-4181-a010-f0e730e166dd','报价会签(SignTogetherTest)','1','MI-TOGETHER-1099','SignTogeterPrice',NULL,4,NULL,NULL,0,NULL,'2021-03-17 01:34:09','10','Long',NULL,NULL,'2021-03-17 09:34:08.789044','10','Long','2021-03-17 09:34:08.996940','10','Long',0,NULL),(8,'1bc22da3-47e3-4a0a-be81-6d7297ad3aca','报价加签(SignForwardTest)','1','MI-FORWARD-1099','SignForwardPrice',NULL,4,NULL,NULL,0,NULL,'2021-03-17 01:34:09','10','Long',NULL,NULL,'2021-03-17 09:34:09.018364','10','Long','2021-03-17 09:34:09.155048','10','Long',0,NULL),(9,'072af8c3-482a-4b1c-890b-685ce2fcc75d','PriceProcess(SequenceTest)','1','SEQ-P-1099','SamplePrice',NULL,2,NULL,NULL,0,NULL,'2021-03-17 01:34:09','10','Long',NULL,NULL,'2021-03-17 09:34:09.282494','10','Long','2021-03-17 09:34:09.265215','10','Long',0,NULL);
/*!40000 ALTER TABLE `wfprocessinstance` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `wfprocessvariable`
--

DROP TABLE IF EXISTS `wfprocessvariable`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `wfprocessvariable` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `VariableType` varchar(50) NOT NULL,
  `AppInstanceID` varchar(100) NOT NULL,
  `ProcessGUID` varchar(100) NOT NULL,
  `ProcessInstanceID` int NOT NULL,
  `ActivityGUID` varchar(100) DEFAULT NULL,
  `ActivityName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Name` varchar(50) NOT NULL,
  `Value` varchar(1024) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `LastUpdatedDateTime` datetime(6) NOT NULL,
  `RowVersionID` binary(8) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `wfprocessvariable`
--

LOCK TABLES `wfprocessvariable` WRITE;
/*!40000 ALTER TABLE `wfprocessvariable` DISABLE KEYS */;
/*!40000 ALTER TABLE `wfprocessvariable` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `wftasks`
--

DROP TABLE IF EXISTS `wftasks`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `wftasks` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `ActivityInstanceID` int NOT NULL,
  `ProcessInstanceID` int NOT NULL,
  `AppName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `AppInstanceID` varchar(50) NOT NULL,
  `ProcessGUID` varchar(100) NOT NULL,
  `ActivityGUID` varchar(100) NOT NULL,
  `ActivityName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `TaskType` smallint NOT NULL,
  `TaskState` smallint NOT NULL DEFAULT '0',
  `EntrustedTaskID` int DEFAULT NULL,
  `AssignedToUserID` varchar(50) NOT NULL,
  `AssignedToUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `IsEMailSent` tinyint unsigned NOT NULL DEFAULT '0',
  `CreatedByUserID` varchar(50) NOT NULL,
  `CreatedByUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreatedDateTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpdatedDateTime` datetime(6) DEFAULT NULL,
  `LastUpdatedByUserID` varchar(50) DEFAULT NULL,
  `LastUpdatedByUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `EndedByUserID` varchar(50) DEFAULT NULL,
  `EndedByUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `EndedDateTime` datetime(6) DEFAULT NULL,
  `RecordStatusInvalid` tinyint unsigned NOT NULL DEFAULT '0',
  `RowVersionID` varbinary(8) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_WfTasks_ActivityInstanceID` (`ActivityInstanceID`),
  CONSTRAINT `FK_WfTasks_ActivityInstanceID` FOREIGN KEY (`ActivityInstanceID`) REFERENCES `wfactivityinstance` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=40 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `wftasks`
--

LOCK TABLES `wftasks` WRITE;
/*!40000 ALTER TABLE `wftasks` DISABLE KEYS */;
INSERT INTO `wftasks` VALUES (1,2,1,'Order-Books','123','73bae357-0e06-42b4-83bc-3b725f6344ff','571c67db-466b-4d6b-8da7-de1e7ddbc9d9','Task-001',1,1,NULL,'01','Zero',0,'01','Zero','2021-03-16 09:07:49',NULL,NULL,NULL,NULL,NULL,NULL,0,NULL),(2,4,2,'Order-Books','123','cff73a19-5c3a-4975-88d1-549a8ca81d26','16f63016-3746-4c49-85b3-da4621dde8f4','Task-001',1,1,NULL,'01','Zero',0,'01','Zero','2021-03-16 13:03:00',NULL,NULL,NULL,NULL,NULL,NULL,0,NULL),(3,6,3,'Order-Books','123','b46f37e6-840e-4b22-bb10-96af7bcdb82a','7554a59e-37d9-49e7-b028-2fea6147e677','Task-001',1,4,NULL,'01','Zero',0,'01','Zero','2021-03-16 13:06:56',NULL,NULL,NULL,'01','Zero','2021-03-16 21:07:01.976984',0,NULL),(4,7,3,'Order-Books','123','b46f37e6-840e-4b22-bb10-96af7bcdb82a','186e3635-5f50-4e91-a93a-dde97a2b3d45','Task-002',1,1,NULL,'151','Oliven(模拟)',0,'01','Zero','2021-03-16 13:07:02',NULL,NULL,NULL,NULL,NULL,NULL,0,NULL),(5,7,3,'Order-Books','123','b46f37e6-840e-4b22-bb10-96af7bcdb82a','186e3635-5f50-4e91-a93a-dde97a2b3d45','Task-002',1,4,NULL,'200','Ted(模拟)',0,'01','Zero','2021-03-16 13:07:02',NULL,NULL,NULL,'200','Ted(模拟)','2021-03-16 21:07:07.491171',0,NULL),(6,7,3,'Order-Books','123','b46f37e6-840e-4b22-bb10-96af7bcdb82a','186e3635-5f50-4e91-a93a-dde97a2b3d45','Task-002',1,1,NULL,'71','Glant(模拟)',0,'01','Zero','2021-03-16 13:07:02',NULL,NULL,NULL,NULL,NULL,NULL,0,NULL),(7,8,3,'Order-Books','123','b46f37e6-840e-4b22-bb10-96af7bcdb82a','8a7566b4-5636-4dce-a5d8-78625375e29c','Task-003',1,1,NULL,'261','Zetophy(模拟)',0,'200','Ted(模拟)','2021-03-16 13:07:07',NULL,NULL,NULL,NULL,NULL,NULL,0,NULL),(8,8,3,'Order-Books','123','b46f37e6-840e-4b22-bb10-96af7bcdb82a','8a7566b4-5636-4dce-a5d8-78625375e29c','Task-003',1,1,NULL,'230','White(模拟)',0,'200','Ted(模拟)','2021-03-16 13:07:07',NULL,NULL,NULL,NULL,NULL,NULL,0,NULL),(9,8,3,'Order-Books','123','b46f37e6-840e-4b22-bb10-96af7bcdb82a','8a7566b4-5636-4dce-a5d8-78625375e29c','Task-003',1,4,NULL,'111','King(模拟)',0,'200','Ted(模拟)','2021-03-16 13:07:07',NULL,NULL,NULL,'111','King(模拟)','2021-03-16 21:07:12.323795',0,NULL),(10,11,4,'SamplePrice','SEQ-P-1099','072af8c3-482a-4b1c-890b-685ce2fcc75d','3c438212-4863-4ff8-efc9-a9096c4a8230','Sales Submit',1,4,NULL,'10','Long',0,'10','Long','2021-03-17 01:34:08',NULL,NULL,NULL,'10','Long','2021-03-17 09:34:08.352505',0,NULL),(11,12,4,'SamplePrice','SEQ-P-1099','072af8c3-482a-4b1c-890b-685ce2fcc75d','eb833577-abb5-4239-875a-5f2e2fcb6d57','Manager Signature',1,4,NULL,'10','Long',0,'10','Long','2021-03-17 01:34:08',NULL,NULL,NULL,'10','Long','2021-03-17 09:34:08.388333',0,NULL),(12,13,4,'SamplePrice','SEQ-P-1099','072af8c3-482a-4b1c-890b-685ce2fcc75d','cab57060-f433-422a-a66f-4a5ecfafd54e','Sales Confirm',1,4,NULL,'10','Long',0,'10','Long','2021-03-17 01:34:08',NULL,NULL,NULL,'10','Long','2021-03-17 09:34:08.409399',0,NULL),(13,16,5,'SamplePrice','SEQ-C-1099','072af8c3-482a-4b1c-890b-685ce2fcc75d','3c438212-4863-4ff8-efc9-a9096c4a8230','Sales Submit',1,4,NULL,'10','Long',0,'10','Long','2021-03-17 01:34:08',NULL,NULL,NULL,'10','Long','2021-03-17 09:34:08.467249',0,NULL),(14,17,5,'SamplePrice','SEQ-C-1099','072af8c3-482a-4b1c-890b-685ce2fcc75d','eb833577-abb5-4239-875a-5f2e2fcb6d57','Manager Signature',1,9,NULL,'10','Long',0,'10','Long','2021-03-17 01:34:08',NULL,NULL,NULL,'10','Long','2021-03-17 09:34:08.514492',0,NULL),(15,18,5,'SamplePrice','SEQ-C-1099','072af8c3-482a-4b1c-890b-685ce2fcc75d','3c438212-4863-4ff8-efc9-a9096c4a8230','Sales Submit',1,4,NULL,'10','Long',0,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,'10','Long','2021-03-17 09:34:08.532188',0,NULL),(16,19,5,'SamplePrice','SEQ-C-1099','072af8c3-482a-4b1c-890b-685ce2fcc75d','eb833577-abb5-4239-875a-5f2e2fcb6d57','Manager Signature',1,4,NULL,'10','Long',0,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,'10','Long','2021-03-17 09:34:08.558177',0,NULL),(17,20,5,'SamplePrice','SEQ-C-1099','072af8c3-482a-4b1c-890b-685ce2fcc75d','cab57060-f433-422a-a66f-4a5ecfafd54e','Sales Confirm',1,4,NULL,'10','Long',0,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,'10','Long','2021-03-17 09:34:08.594405',0,NULL),(18,22,5,'SamplePrice','SEQ-C-1099','072af8c3-482a-4b1c-890b-685ce2fcc75d','cab57060-f433-422a-a66f-4a5ecfafd54e','Sales Confirm',1,4,NULL,'10','Long',0,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,'10','Long','2021-03-17 09:34:08.647987',0,NULL),(19,25,6,'OfficeIn','10998','3a8ce214-fd18-4fac-95c0-e7958bc1b2f8','4db4a153-c8fc-45df-b067-9d188ae19a41','仓库签字',1,4,NULL,'10','Long',0,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,'10','Long','2021-03-17 09:34:08.695200',0,NULL),(20,27,6,'OfficeIn','10998','3a8ce214-fd18-4fac-95c0-e7958bc1b2f8','c3cbb3cc-fa60-42ad-9a10-4ec2638aff49','行政部签字',1,4,NULL,'10','Long',0,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,'10','Long','2021-03-17 09:34:08.732979',0,NULL),(21,28,6,'OfficeIn','10998','3a8ce214-fd18-4fac-95c0-e7958bc1b2f8','932f7fa0-2d4c-4257-c158-b8b181af2d0a','财务主管',1,4,NULL,'10','Long',0,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,'10','Long','2021-03-17 09:34:08.757244',0,NULL),(22,31,7,'SignTogeterPrice','MI-TOGETHER-1099','9fb4bca4-5674-4181-a010-f0e730e166dd','791d9d3a-882d-4796-cffc-84d9fca76afd','业务员提交',1,4,NULL,'10','Long',0,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,'10','Long','2021-03-17 09:34:08.809770',0,NULL),(23,33,7,'SignTogeterPrice','MI-TOGETHER-1099','9fb4bca4-5674-4181-a010-f0e730e166dd','36cf2479-e8ec-4936-8bcd-b38101e4664a','板房会签',1,4,NULL,'40','Susan',0,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,'40','Susan','2021-03-17 09:34:08.841356',0,NULL),(24,34,7,'SignTogeterPrice','MI-TOGETHER-1099','9fb4bca4-5674-4181-a010-f0e730e166dd','36cf2479-e8ec-4936-8bcd-b38101e4664a','板房会签',1,4,NULL,'30','Jack',0,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,'30','Jack','2021-03-17 09:34:08.918031',0,NULL),(25,35,7,'SignTogeterPrice','MI-TOGETHER-1099','9fb4bca4-5674-4181-a010-f0e730e166dd','36cf2479-e8ec-4936-8bcd-b38101e4664a','板房会签',1,4,NULL,'20','Meilinda',0,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,'20','Meilinda','2021-03-17 09:34:08.942636',0,NULL),(26,36,7,'SignTogeterPrice','MI-TOGETHER-1099','9fb4bca4-5674-4181-a010-f0e730e166dd','23017d0c-08ca-4a59-9649-c6912b819001','业务员确认',1,4,NULL,'10','Long',0,'20','Meilinda','2021-03-17 01:34:09',NULL,NULL,NULL,'10','Long','2021-03-17 09:34:08.988577',0,NULL),(27,39,8,'SignForwardPrice','MI-FORWARD-1099','1bc22da3-47e3-4a0a-be81-6d7297ad3aca','791d9d3a-882d-4796-cffc-84d9fca76afd','业务员提交',1,4,NULL,'10','Long',0,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,'10','Long','2021-03-17 09:34:09.041083',0,NULL),(28,40,8,'SignForwardPrice','MI-FORWARD-1099','1bc22da3-47e3-4a0a-be81-6d7297ad3aca','36cf2479-e8ec-4936-8bcd-b38101e4664a','板房加签',1,1,NULL,'10','Long',0,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,NULL,0,NULL),(29,40,8,'SignForwardPrice','MI-FORWARD-1099','1bc22da3-47e3-4a0a-be81-6d7297ad3aca','36cf2479-e8ec-4936-8bcd-b38101e4664a','板房加签',1,1,NULL,'20','Meilinda',0,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,NULL,0,NULL),(30,41,8,'SignForwardPrice','MI-FORWARD-1099','1bc22da3-47e3-4a0a-be81-6d7297ad3aca','36cf2479-e8ec-4936-8bcd-b38101e4664a','板房加签',1,1,NULL,'10','Long',0,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,NULL,0,NULL),(31,42,8,'SignForwardPrice','MI-FORWARD-1099','1bc22da3-47e3-4a0a-be81-6d7297ad3aca','36cf2479-e8ec-4936-8bcd-b38101e4664a','板房加签',1,4,NULL,'30','Alice',0,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,'30','Alice','2021-03-17 09:34:09.094936',0,NULL),(32,43,8,'SignForwardPrice','MI-FORWARD-1099','1bc22da3-47e3-4a0a-be81-6d7297ad3aca','36cf2479-e8ec-4936-8bcd-b38101e4664a','板房加签',1,4,NULL,'40','FangFang',0,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,'40','FangFang','2021-03-17 09:34:09.117223',0,NULL),(33,44,8,'SignForwardPrice','MI-FORWARD-1099','1bc22da3-47e3-4a0a-be81-6d7297ad3aca','23017d0c-08ca-4a59-9649-c6912b819001','业务员确认',1,4,NULL,'10','Long',0,'40','FangFang','2021-03-17 01:34:09',NULL,NULL,NULL,'10','Long','2021-03-17 09:34:09.146012',0,NULL),(34,47,9,'SamplePrice','SEQ-P-1099','072af8c3-482a-4b1c-890b-685ce2fcc75d','3c438212-4863-4ff8-efc9-a9096c4a8230','Sales Submit',1,4,NULL,'10','Long',0,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,'10','Long','2021-03-17 09:34:09.204349',0,NULL),(35,48,9,'SamplePrice','SEQ-P-1099','072af8c3-482a-4b1c-890b-685ce2fcc75d','eb833577-abb5-4239-875a-5f2e2fcb6d57','Manager Signature',1,4,NULL,'10','Long',0,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,'10','Long','2021-03-17 09:34:09.230819',0,NULL),(36,49,9,'SamplePrice','SEQ-P-1099','072af8c3-482a-4b1c-890b-685ce2fcc75d','cab57060-f433-422a-a66f-4a5ecfafd54e','Sales Confirm',1,4,NULL,'10','Long',0,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,'10','Long','2021-03-17 09:34:09.256669',0,NULL),(37,51,9,'SamplePrice','SEQ-P-1099','072af8c3-482a-4b1c-890b-685ce2fcc75d','cab57060-f433-422a-a66f-4a5ecfafd54e','Sales Confirm',1,1,NULL,'10','Long',0,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,NULL,0,NULL),(38,52,9,'SamplePrice','SEQ-P-1099','072af8c3-482a-4b1c-890b-685ce2fcc75d','3c438212-4863-4ff8-efc9-a9096c4a8230','Sales Submit',1,1,NULL,'10','Long',0,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,NULL,0,NULL),(39,53,9,'SamplePrice','SEQ-P-1099','072af8c3-482a-4b1c-890b-685ce2fcc75d','b53eb9ab-3af6-41ad-d722-bed946d19792','End',1,1,NULL,'10','Long',0,'10','Long','2021-03-17 01:34:09',NULL,NULL,NULL,NULL,NULL,NULL,0,NULL);
/*!40000 ALTER TABLE `wftasks` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `wftransitioninstance`
--

DROP TABLE IF EXISTS `wftransitioninstance`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `wftransitioninstance` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `TransitionGUID` varchar(100) NOT NULL,
  `AppName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `AppInstanceID` varchar(50) NOT NULL,
  `ProcessInstanceID` int NOT NULL,
  `ProcessGUID` varchar(100) NOT NULL,
  `TransitionType` tinyint unsigned NOT NULL,
  `FlyingType` tinyint unsigned NOT NULL DEFAULT '0',
  `FromActivityInstanceID` int NOT NULL,
  `FromActivityGUID` varchar(100) NOT NULL,
  `FromActivityType` smallint NOT NULL,
  `FromActivityName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ToActivityInstanceID` int NOT NULL,
  `ToActivityGUID` varchar(100) NOT NULL,
  `ToActivityType` smallint NOT NULL,
  `ToActivityName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ConditionParseResult` tinyint unsigned NOT NULL DEFAULT '0',
  `CreatedByUserID` varchar(50) NOT NULL,
  `CreatedByUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreatedDateTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `RecordStatusInvalid` tinyint unsigned NOT NULL DEFAULT '0',
  `RowVersionID` varbinary(8) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=39 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `wftransitioninstance`
--

LOCK TABLES `wftransitioninstance` WRITE;
/*!40000 ALTER TABLE `wftransitioninstance` DISABLE KEYS */;
INSERT INTO `wftransitioninstance` VALUES (1,'03d0382e-a4cc-4081-b479-511dbb9fb766','Order-Books','123',1,'73bae357-0e06-42b4-83bc-3b725f6344ff',1,0,1,'f39860c8-cfc1-499c-9b4f-888f3b499bc9',1,'Start',2,'571c67db-466b-4d6b-8da7-de1e7ddbc9d9',4,'Task-001',1,'01','Zero','2021-03-16 09:07:49',0,NULL),(2,'98ffea96-9448-45af-94d0-3bf579a9032d','Order-Books','123',2,'cff73a19-5c3a-4975-88d1-549a8ca81d26',1,0,3,'690e9add-4e3e-4148-be48-2e04432b6792',1,'Start',4,'16f63016-3746-4c49-85b3-da4621dde8f4',4,'Task-001',1,'01','Zero','2021-03-16 13:03:00',0,NULL),(3,'d6d2447f-8b63-4fef-b548-13af71847dd6','Order-Books','123',3,'b46f37e6-840e-4b22-bb10-96af7bcdb82a',1,0,5,'7024ebee-daa2-4f22-b75d-efe869670645',1,'Start',6,'7554a59e-37d9-49e7-b028-2fea6147e677',4,'Task-001',1,'01','Zero','2021-03-16 13:06:56',0,NULL),(4,'29435200-8f97-4aa6-8053-416a33867b68','Order-Books','123',3,'b46f37e6-840e-4b22-bb10-96af7bcdb82a',1,0,6,'7554a59e-37d9-49e7-b028-2fea6147e677',4,'Task-001',7,'186e3635-5f50-4e91-a93a-dde97a2b3d45',4,'Task-002',1,'01','Zero','2021-03-16 13:07:02',0,NULL),(5,'b2821570-5a34-497d-9a46-4768ee682bae','Order-Books','123',3,'b46f37e6-840e-4b22-bb10-96af7bcdb82a',1,0,7,'186e3635-5f50-4e91-a93a-dde97a2b3d45',4,'Task-002',8,'8a7566b4-5636-4dce-a5d8-78625375e29c',4,'Task-003',1,'200','Ted(模拟)','2021-03-16 13:07:07',0,NULL),(6,'7435fdca-8cb3-49ad-9d3d-acac634c3ee1','Order-Books','123',3,'b46f37e6-840e-4b22-bb10-96af7bcdb82a',1,0,8,'8a7566b4-5636-4dce-a5d8-78625375e29c',4,'Task-003',9,'5a1340c9-db08-4cbe-8772-12228f261802',2,'End',1,'111','King(模拟)','2021-03-16 13:07:12',0,NULL),(7,'9cf01621-2dd5-474a-8889-cdbe53a0b72e','SamplePrice','SEQ-P-1099',4,'072af8c3-482a-4b1c-890b-685ce2fcc75d',1,0,10,'9b78486d-5b8f-4be4-948e-522356e84e79',1,'Start',11,'3c438212-4863-4ff8-efc9-a9096c4a8230',4,'Sales Submit',1,'10','Long','2021-03-17 01:34:08',0,NULL),(8,'5432de95-cbcd-4349-9cf0-7e67904c52aa','SamplePrice','SEQ-P-1099',4,'072af8c3-482a-4b1c-890b-685ce2fcc75d',1,0,11,'3c438212-4863-4ff8-efc9-a9096c4a8230',4,'Sales Submit',12,'eb833577-abb5-4239-875a-5f2e2fcb6d57',4,'Manager Signature',1,'10','Long','2021-03-17 01:34:08',0,NULL),(9,'ac609b39-b6eb-4506-c36f-670c5ed53f5c','SamplePrice','SEQ-P-1099',4,'072af8c3-482a-4b1c-890b-685ce2fcc75d',1,0,12,'eb833577-abb5-4239-875a-5f2e2fcb6d57',4,'Manager Signature',13,'cab57060-f433-422a-a66f-4a5ecfafd54e',4,'Sales Confirm',1,'10','Long','2021-03-17 01:34:08',0,NULL),(10,'2d5c0e7b-1303-48cb-c22b-3cd2b45701e3','SamplePrice','SEQ-P-1099',4,'072af8c3-482a-4b1c-890b-685ce2fcc75d',1,0,13,'cab57060-f433-422a-a66f-4a5ecfafd54e',4,'Sales Confirm',14,'b53eb9ab-3af6-41ad-d722-bed946d19792',2,'End',1,'10','Long','2021-03-17 01:34:08',0,NULL),(11,'9cf01621-2dd5-474a-8889-cdbe53a0b72e','SamplePrice','SEQ-C-1099',5,'072af8c3-482a-4b1c-890b-685ce2fcc75d',1,0,15,'9b78486d-5b8f-4be4-948e-522356e84e79',1,'Start',16,'3c438212-4863-4ff8-efc9-a9096c4a8230',4,'Sales Submit',1,'10','Long','2021-03-17 01:34:08',0,NULL),(12,'5432de95-cbcd-4349-9cf0-7e67904c52aa','SamplePrice','SEQ-C-1099',5,'072af8c3-482a-4b1c-890b-685ce2fcc75d',1,0,16,'3c438212-4863-4ff8-efc9-a9096c4a8230',4,'Sales Submit',17,'eb833577-abb5-4239-875a-5f2e2fcb6d57',4,'Manager Signature',1,'10','Long','2021-03-17 01:34:08',0,NULL),(13,'5432de95-cbcd-4349-9cf0-7e67904c52aa','SamplePrice','SEQ-C-1099',5,'072af8c3-482a-4b1c-890b-685ce2fcc75d',14,0,17,'eb833577-abb5-4239-875a-5f2e2fcb6d57',4,'Manager Signature',18,'3c438212-4863-4ff8-efc9-a9096c4a8230',4,'Sales Submit',1,'10','Long','2021-03-17 01:34:09',0,NULL),(14,'5432de95-cbcd-4349-9cf0-7e67904c52aa','SamplePrice','SEQ-C-1099',5,'072af8c3-482a-4b1c-890b-685ce2fcc75d',1,0,18,'3c438212-4863-4ff8-efc9-a9096c4a8230',4,'Sales Submit',19,'eb833577-abb5-4239-875a-5f2e2fcb6d57',4,'Manager Signature',1,'10','Long','2021-03-17 01:34:09',0,NULL),(15,'ac609b39-b6eb-4506-c36f-670c5ed53f5c','SamplePrice','SEQ-C-1099',5,'072af8c3-482a-4b1c-890b-685ce2fcc75d',1,0,19,'eb833577-abb5-4239-875a-5f2e2fcb6d57',4,'Manager Signature',20,'cab57060-f433-422a-a66f-4a5ecfafd54e',4,'Sales Confirm',1,'10','Long','2021-03-17 01:34:09',0,NULL),(16,'2d5c0e7b-1303-48cb-c22b-3cd2b45701e3','SamplePrice','SEQ-C-1099',5,'072af8c3-482a-4b1c-890b-685ce2fcc75d',1,0,20,'cab57060-f433-422a-a66f-4a5ecfafd54e',4,'Sales Confirm',21,'b53eb9ab-3af6-41ad-d722-bed946d19792',2,'End',1,'10','Long','2021-03-17 01:34:09',0,NULL),(17,'2d5c0e7b-1303-48cb-c22b-3cd2b45701e3','SamplePrice','SEQ-C-1099',5,'072af8c3-482a-4b1c-890b-685ce2fcc75d',14,0,21,'b53eb9ab-3af6-41ad-d722-bed946d19792',2,'End',22,'cab57060-f433-422a-a66f-4a5ecfafd54e',4,'Sales Confirm',1,'10','Long','2021-03-17 01:34:09',0,NULL),(18,'2d5c0e7b-1303-48cb-c22b-3cd2b45701e3','SamplePrice','SEQ-C-1099',5,'072af8c3-482a-4b1c-890b-685ce2fcc75d',1,0,22,'cab57060-f433-422a-a66f-4a5ecfafd54e',4,'Sales Confirm',23,'b53eb9ab-3af6-41ad-d722-bed946d19792',2,'End',1,'10','Long','2021-03-17 01:34:09',0,NULL),(19,'81fdf756-ecd5-43c0-e2b3-25770aab5dee','OfficeIn','10998',6,'3a8ce214-fd18-4fac-95c0-e7958bc1b2f8',1,0,24,'e52d0836-9f98-4b70-d485-6b01b8cc277e',1,'开始',25,'4db4a153-c8fc-45df-b067-9d188ae19a41',4,'仓库签字',1,'10','Long','2021-03-17 01:34:09',0,NULL),(20,'69c1ba54-acb0-4b4e-ff03-3f6cf572e98a','OfficeIn','10998',6,'3a8ce214-fd18-4fac-95c0-e7958bc1b2f8',1,0,25,'4db4a153-c8fc-45df-b067-9d188ae19a41',4,'仓库签字',26,'eb492ba8-075a-46e4-b95f-ac071dd3a43d',8,'Gateway',1,'10','Long','2021-03-17 01:34:09',0,NULL),(21,'8d776249-f3c6-4397-817f-44880b34a451','OfficeIn','10998',6,'3a8ce214-fd18-4fac-95c0-e7958bc1b2f8',1,0,26,'eb492ba8-075a-46e4-b95f-ac071dd3a43d',8,'Gateway',27,'c3cbb3cc-fa60-42ad-9a10-4ec2638aff49',4,'行政部签字',1,'10','Long','2021-03-17 01:34:09',0,NULL),(22,'952b3594-fe40-427f-a27a-f2650226aeca','OfficeIn','10998',6,'3a8ce214-fd18-4fac-95c0-e7958bc1b2f8',1,0,27,'c3cbb3cc-fa60-42ad-9a10-4ec2638aff49',4,'行政部签字',28,'932f7fa0-2d4c-4257-c158-b8b181af2d0a',4,'财务主管',1,'10','Long','2021-03-17 01:34:09',0,NULL),(23,'4b8a68d6-ef32-420a-93e7-33c7e4b80360','OfficeIn','10998',6,'3a8ce214-fd18-4fac-95c0-e7958bc1b2f8',1,0,28,'932f7fa0-2d4c-4257-c158-b8b181af2d0a',4,'财务主管',29,'30929bbb-c76e-4604-c956-f26feb4aa22e',2,'结束',1,'10','Long','2021-03-17 01:34:09',0,NULL),(24,'50f7acb2-99d0-4877-e116-5bf19433bb89','SignTogeterPrice','MI-TOGETHER-1099',7,'9fb4bca4-5674-4181-a010-f0e730e166dd',1,0,30,'1f303f19-71aa-4879-c501-f4d0f448f0a2',1,'开始',31,'791d9d3a-882d-4796-cffc-84d9fca76afd',4,'业务员提交',1,'10','Long','2021-03-17 01:34:09',0,NULL),(25,'87651a0d-81e5-4d6f-9ef3-ed0be0011c8f','SignTogeterPrice','MI-TOGETHER-1099',7,'9fb4bca4-5674-4181-a010-f0e730e166dd',1,0,31,'791d9d3a-882d-4796-cffc-84d9fca76afd',4,'业务员提交',32,'36cf2479-e8ec-4936-8bcd-b38101e4664a',6,'板房会签',1,'10','Long','2021-03-17 01:34:09',0,NULL),(26,'63031ecf-2116-47a3-a0d8-f920dc5bee11','SignTogeterPrice','MI-TOGETHER-1099',7,'9fb4bca4-5674-4181-a010-f0e730e166dd',1,0,35,'36cf2479-e8ec-4936-8bcd-b38101e4664a',6,'板房会签',36,'23017d0c-08ca-4a59-9649-c6912b819001',4,'业务员确认',1,'20','Meilinda','2021-03-17 01:34:09',0,NULL),(27,'3d06aebb-2fb3-4995-e0c7-99d488f8312d','SignTogeterPrice','MI-TOGETHER-1099',7,'9fb4bca4-5674-4181-a010-f0e730e166dd',1,0,36,'23017d0c-08ca-4a59-9649-c6912b819001',4,'业务员确认',37,'7462aae9-da1c-43f0-d741-a4586879de77',2,'结束',1,'10','Long','2021-03-17 01:34:09',0,NULL),(28,'50f7acb2-99d0-4877-e116-5bf19433bb89','SignForwardPrice','MI-FORWARD-1099',8,'1bc22da3-47e3-4a0a-be81-6d7297ad3aca',1,0,38,'1f303f19-71aa-4879-c501-f4d0f448f0a2',1,'开始',39,'791d9d3a-882d-4796-cffc-84d9fca76afd',4,'业务员提交',1,'10','Long','2021-03-17 01:34:09',0,NULL),(29,'87651a0d-81e5-4d6f-9ef3-ed0be0011c8f','SignForwardPrice','MI-FORWARD-1099',8,'1bc22da3-47e3-4a0a-be81-6d7297ad3aca',1,0,39,'791d9d3a-882d-4796-cffc-84d9fca76afd',4,'业务员提交',40,'36cf2479-e8ec-4936-8bcd-b38101e4664a',6,'板房加签',1,'10','Long','2021-03-17 01:34:09',0,NULL),(30,'63031ecf-2116-47a3-a0d8-f920dc5bee11','SignForwardPrice','MI-FORWARD-1099',8,'1bc22da3-47e3-4a0a-be81-6d7297ad3aca',1,0,43,'36cf2479-e8ec-4936-8bcd-b38101e4664a',6,'板房加签',44,'23017d0c-08ca-4a59-9649-c6912b819001',4,'业务员确认',1,'40','FangFang','2021-03-17 01:34:09',0,NULL),(31,'3d06aebb-2fb3-4995-e0c7-99d488f8312d','SignForwardPrice','MI-FORWARD-1099',8,'1bc22da3-47e3-4a0a-be81-6d7297ad3aca',1,0,44,'23017d0c-08ca-4a59-9649-c6912b819001',4,'业务员确认',45,'7462aae9-da1c-43f0-d741-a4586879de77',2,'结束',1,'10','Long','2021-03-17 01:34:09',0,NULL),(32,'9cf01621-2dd5-474a-8889-cdbe53a0b72e','SamplePrice','SEQ-P-1099',9,'072af8c3-482a-4b1c-890b-685ce2fcc75d',1,0,46,'9b78486d-5b8f-4be4-948e-522356e84e79',1,'Start',47,'3c438212-4863-4ff8-efc9-a9096c4a8230',4,'Sales Submit',1,'10','Long','2021-03-17 01:34:09',0,NULL),(33,'5432de95-cbcd-4349-9cf0-7e67904c52aa','SamplePrice','SEQ-P-1099',9,'072af8c3-482a-4b1c-890b-685ce2fcc75d',1,0,47,'3c438212-4863-4ff8-efc9-a9096c4a8230',4,'Sales Submit',48,'eb833577-abb5-4239-875a-5f2e2fcb6d57',4,'Manager Signature',1,'10','Long','2021-03-17 01:34:09',0,NULL),(34,'ac609b39-b6eb-4506-c36f-670c5ed53f5c','SamplePrice','SEQ-P-1099',9,'072af8c3-482a-4b1c-890b-685ce2fcc75d',1,0,48,'eb833577-abb5-4239-875a-5f2e2fcb6d57',4,'Manager Signature',49,'cab57060-f433-422a-a66f-4a5ecfafd54e',4,'Sales Confirm',1,'10','Long','2021-03-17 01:34:09',0,NULL),(35,'2d5c0e7b-1303-48cb-c22b-3cd2b45701e3','SamplePrice','SEQ-P-1099',9,'072af8c3-482a-4b1c-890b-685ce2fcc75d',1,0,49,'cab57060-f433-422a-a66f-4a5ecfafd54e',4,'Sales Confirm',50,'b53eb9ab-3af6-41ad-d722-bed946d19792',2,'End',1,'10','Long','2021-03-17 01:34:09',0,NULL),(36,'2d5c0e7b-1303-48cb-c22b-3cd2b45701e3','SamplePrice','SEQ-P-1099',9,'072af8c3-482a-4b1c-890b-685ce2fcc75d',14,0,50,'b53eb9ab-3af6-41ad-d722-bed946d19792',2,'End',51,'cab57060-f433-422a-a66f-4a5ecfafd54e',4,'Sales Confirm',1,'10','Long','2021-03-17 01:34:09',0,NULL),(37,'GATEWAY-BYPASS-GUID','SamplePrice','SEQ-P-1099',9,'072af8c3-482a-4b1c-890b-685ce2fcc75d',4,0,51,'cab57060-f433-422a-a66f-4a5ecfafd54e',4,'Sales Confirm',52,'3c438212-4863-4ff8-efc9-a9096c4a8230',4,'Sales Submit',1,'10','Long','2021-03-17 01:34:09',0,NULL),(38,'9cf01621-2dd5-474a-8889-cdbe53a0b72e','SamplePrice','SEQ-P-1099',9,'072af8c3-482a-4b1c-890b-685ce2fcc75d',4,0,52,'3c438212-4863-4ff8-efc9-a9096c4a8230',4,'Sales Submit',53,'b53eb9ab-3af6-41ad-d722-bed946d19792',2,'End',1,'10','Long','2021-03-17 01:34:09',0,NULL);
/*!40000 ALTER TABLE `wftransitioninstance` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `whjoblog`
--

DROP TABLE IF EXISTS `whjoblog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `whjoblog` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `JobType` varchar(50) NOT NULL,
  `JobName` varchar(200) NOT NULL,
  `JobKey` varchar(50) DEFAULT NULL,
  `RefClass` varchar(50) NOT NULL COMMENT 'PROCESS-INSTANCE\r\n   ACTIVITY-INSTANCE',
  `RefIDs` varchar(4000) NOT NULL,
  `Status` smallint NOT NULL,
  `Message` varchar(4000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `StackTrace` varchar(0) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `CreatedDateTime` datetime(6) NOT NULL,
  `CreatedByUserID` varchar(50) NOT NULL,
  `CreatedByUserName` varchar(50) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `whjoblog`
--

LOCK TABLES `whjoblog` WRITE;
/*!40000 ALTER TABLE `whjoblog` DISABLE KEYS */;
/*!40000 ALTER TABLE `whjoblog` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `whjobschedule`
--

DROP TABLE IF EXISTS `whjobschedule`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `whjobschedule` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `ScheduleType` tinyint unsigned NOT NULL,
  `ScheduleGUID` varchar(100) DEFAULT NULL,
  `ScheduleName` varchar(100) NOT NULL,
  `Title` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Status` smallint NOT NULL DEFAULT '0',
  `CronExpression` varchar(100) DEFAULT NULL,
  `LastUpdatedDateTime` datetime(6) DEFAULT NULL,
  `LastUpdatedByUserID` varchar(50) DEFAULT NULL,
  `LastUpdatedByUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `whjobschedule`
--

LOCK TABLES `whjobschedule` WRITE;
/*!40000 ALTER TABLE `whjobschedule` DISABLE KEYS */;
/*!40000 ALTER TABLE `whjobschedule` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Final view structure for view `vwwfactivityinstancetasks`
--

/*!50001 DROP VIEW IF EXISTS `vwwfactivityinstancetasks`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `vwwfactivityinstancetasks` AS select `wftasks`.`ID` AS `TaskID`,`wfactivityinstance`.`AppName` AS `AppName`,`wfactivityinstance`.`AppInstanceID` AS `AppInstanceID`,`wfactivityinstance`.`ProcessGUID` AS `ProcessGUID`,`wfprocessinstance`.`Version` AS `Version`,`wftasks`.`ProcessInstanceID` AS `ProcessInstanceID`,`wfactivityinstance`.`ActivityGUID` AS `ActivityGUID`,`wftasks`.`ActivityInstanceID` AS `ActivityInstanceID`,`wfactivityinstance`.`ActivityName` AS `ActivityName`,`wfactivityinstance`.`ActivityCode` AS `ActivityCode`,`wfactivityinstance`.`ActivityType` AS `ActivityType`,`wfactivityinstance`.`WorkItemType` AS `WorkItemType`,`wfactivityinstance`.`BackSrcActivityInstanceID` AS `BackSrcActivityInstanceID`,`wfactivityinstance`.`CreatedByUserID` AS `PreviousUserID`,`wfactivityinstance`.`CreatedByUserName` AS `PreviousUserName`,`wfactivityinstance`.`CreatedDateTime` AS `PreviousDateTime`,`wftasks`.`TaskType` AS `TaskType`,`wftasks`.`EntrustedTaskID` AS `EntrustedTaskID`,`wftasks`.`AssignedToUserID` AS `AssignedToUserID`,`wftasks`.`AssignedToUserName` AS `AssignedToUserName`,`wftasks`.`IsEMailSent` AS `IsEMailSent`,`wftasks`.`CreatedDateTime` AS `CreatedDateTime`,`wftasks`.`LastUpdatedDateTime` AS `LastUpdatedDateTime`,`wftasks`.`EndedDateTime` AS `EndedDateTime`,`wftasks`.`EndedByUserID` AS `EndedByUserID`,`wftasks`.`EndedByUserName` AS `EndedByUserName`,`wftasks`.`TaskState` AS `TaskState`,`wfactivityinstance`.`ActivityState` AS `ActivityState`,`wftasks`.`RecordStatusInvalid` AS `RecordStatusInvalid`,`wfprocessinstance`.`ProcessState` AS `ProcessState`,`wfactivityinstance`.`ComplexType` AS `ComplexType`,`wfactivityinstance`.`MIHostActivityInstanceID` AS `MIHostActivityInstanceID`,`wfactivityinstance`.`ApprovalStatus` AS `ApprovalStatus`,`wfactivityinstance`.`CompleteOrder` AS `CompleteOrder`,`wfprocessinstance`.`AppInstanceCode` AS `AppInstanceCode`,`wfprocessinstance`.`ProcessName` AS `ProcessName`,`wfprocessinstance`.`CreatedByUserName` AS `CreatedByUserName`,`wfprocessinstance`.`CreatedDateTime` AS `PCreatedDateTime`,(case when (`wfactivityinstance`.`MIHostActivityInstanceID` is null) then `wfactivityinstance`.`ActivityState` else (select `a`.`ActivityState` from `wfactivityinstance` `a` where (`a`.`ID` = `wfactivityinstance`.`MIHostActivityInstanceID`)) end) AS `MiHostState` from ((`wfactivityinstance` join `wftasks` on((`wfactivityinstance`.`ID` = `wftasks`.`ActivityInstanceID`))) join `wfprocessinstance` on((`wfactivityinstance`.`ProcessInstanceID` = `wfprocessinstance`.`ID`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-03-17  9:55:24
