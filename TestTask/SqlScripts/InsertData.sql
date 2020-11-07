-- 
-- Вывод данных для таблицы [User]
--
PRINT (N'Вывод данных для таблицы [User]')
SET IDENTITY_INSERT dbo.[User] ON
GO
INSERT dbo.[User](UserId, Name, LastName) VALUES (2, 'Илья Игоревич', 'Баренцев')
INSERT dbo.[User](UserId, Name, LastName) VALUES (4, 'Владислав Владимирович', 'Гербицкий')
INSERT dbo.[User](UserId, Name, LastName) VALUES (5, 'Сергей Александрович', 'Петренко')
INSERT dbo.[User](UserId, Name, LastName) VALUES (6, 'Валерий Сергеевич', 'Горбатов')
INSERT dbo.[User](UserId, Name, LastName) VALUES (8, 'Елизавета Владимировна', 'Пшеничкина')
GO
SET IDENTITY_INSERT dbo.[User] OFF
GO
-- 
-- Вывод данных для таблицы PassTime
--
PRINT (N'Вывод данных для таблицы PassTime')
SET IDENTITY_INSERT dbo.PassTime ON
GO
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (1, 2, '2020-11-07 07:55:28.187', '2020-11-07 17:23:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (2, 2, '2020-11-10 09:56:28.187', '2020-11-10 19:22:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (3, 2, '2020-11-02 07:57:28.187', '2020-11-02 14:08:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (4, 2, '2020-11-03 08:58:28.187', '2020-11-03 15:20:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (5, 2, '2020-11-04 07:59:28.187', '2020-11-04 17:19:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (6, 2, '2020-11-05 09:00:28.187', '2020-11-05 15:18:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (7, 2, '2020-11-08 10:01:28.187', '2020-11-08 16:17:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (8, 2, '2020-11-09 11:02:28.187', '2020-11-09 17:16:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (9, 2, '2020-11-01 08:24:28.187', '2020-11-01 15:15:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (10, 2, '2020-11-06 10:04:28.187', '2020-11-06 16:14:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (11, 4, '2020-11-07 08:05:28.187', '2020-11-07 17:13:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (12, 4, '2020-11-08 10:06:28.187', '2020-11-08 16:12:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (13, 4, '2020-11-01 07:07:28.187', '2020-11-01 16:43:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (14, 4, '2020-11-06 10:08:28.187', '2020-11-06 16:10:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (15, 4, '2020-11-09 11:09:28.187', '2020-11-09 17:09:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (16, 4, '2020-11-02 08:10:28.187', '2020-11-02 14:08:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (17, 4, '2020-11-05 09:11:28.187', '2020-11-05 15:07:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (18, 4, '2020-11-10 09:12:28.187', '2020-11-10 18:06:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (19, 4, '2020-11-03 09:13:28.187', '2020-11-03 15:05:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (20, 4, '2020-11-04 08:14:28.187', '2020-11-04 17:04:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (21, 5, '2020-11-07 08:15:28.187', '2020-11-07 17:03:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (22, 5, '2020-11-08 10:16:28.187', '2020-11-08 16:02:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (23, 5, '2020-11-01 07:17:28.187', '2020-11-01 19:39:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (24, 5, '2020-11-06 10:18:28.187', '2020-11-06 16:00:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (25, 5, '2020-11-09 11:19:28.187', '2020-11-09 16:59:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (26, 5, '2020-11-02 08:20:28.187', '2020-11-02 13:58:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (27, 5, '2020-11-05 09:21:28.187', '2020-11-05 14:57:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (28, 5, '2020-11-10 09:22:28.187', '2020-11-10 17:56:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (29, 5, '2020-11-03 09:23:28.187', '2020-11-03 14:55:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (30, 5, '2020-11-04 08:24:28.187', '2020-11-04 16:54:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (31, 6, '2020-11-07 08:25:28.187', '2020-11-07 16:53:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (32, 6, '2020-11-08 10:26:28.187', '2020-11-08 15:52:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (33, 6, '2020-11-01 07:27:28.187', '2020-11-01 15:51:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (34, 6, '2020-11-06 10:28:28.187', '2020-11-06 15:50:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (35, 6, '2020-11-09 11:29:28.187', '2020-11-09 16:49:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (36, 6, '2020-11-02 08:30:28.187', '2020-11-02 13:48:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (37, 6, '2020-11-05 09:31:28.187', '2020-11-05 14:47:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (38, 6, '2020-11-10 09:32:28.187', '2020-11-10 17:46:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (39, 6, '2020-11-03 09:33:28.187', '2020-11-03 14:45:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (40, 6, '2020-11-04 08:34:28.187', '2020-11-04 16:44:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (41, 8, '2020-11-07 08:35:28.187', '2020-11-07 16:43:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (42, 8, '2020-11-08 10:36:28.187', '2020-11-08 15:42:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (43, 8, '2020-11-01 07:37:28.187', '2020-11-01 15:41:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (44, 8, '2020-11-06 10:38:28.187', '2020-11-06 15:40:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (45, 8, '2020-11-09 11:39:28.187', '2020-11-09 16:39:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (46, 8, '2020-11-02 08:40:28.187', '2020-11-02 13:38:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (47, 8, '2020-11-05 09:41:28.187', '2020-11-05 14:37:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (48, 8, '2020-11-10 09:42:28.187', '2020-11-10 17:36:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (49, 8, '2020-11-03 09:43:28.187', '2020-11-03 14:35:28.187')
INSERT dbo.PassTime(PassTimeId, UserId, EnterTime, ExitTime) VALUES (50, 8, '2020-11-04 08:44:28.187', '2020-11-04 16:34:28.187')
GO
SET IDENTITY_INSERT dbo.PassTime OFF
GO