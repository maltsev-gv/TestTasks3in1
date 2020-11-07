--
-- Создать процедуру [dbo].[GetWorkTimes]
--
GO
PRINT (N'Создать процедуру [dbo].[GetWorkTimes]')
GO
CREATE PROCEDURE dbo.GetWorkTimes @StartTime DATETIME, @EndTime DATETIME
AS 

--создаём временную таблицу для накапливания времени сотрудников
IF OBJECT_ID('tempdb.dbo.#UserTimes') IS NOT NULL
  DROP TABLE #UserTimes; 

CREATE TABLE #UserTimes (UserId INT, WorkTime DATETIME)

-- объявляем курсор, который возьмёт все записи из PassTime, хотя бы частично попадающие в заданный диапазон
DECLARE my_cur CURSOR FOR 
  SELECT PassTimeId, UserId, EnterTime, ExitTime FROM PassTime
    WHERE @StartTime < ExitTime AND @EndTime > EnterTime
    ORDER BY ExitTime

DECLARE @passTimeId INT
DECLARE @userId INT
DECLARE @enterTime DATETIME
DECLARE @exitTime DATETIME

DECLARE @curEnterTime DATETIME
DECLARE @curExitTime DATETIME

--открываем курсор 
OPEN my_cur
--считываем данные первой строки в наши переменные 
FETCH NEXT FROM my_cur INTO @passTimeId, @userId , @enterTime, @exitTime
--если данные в курсоре есть, то заходим в цикл 
--и крутимся там до тех пор, пока не закончатся строки в курсоре 
WHILE @@FETCH_STATUS = 0
  BEGIN
    IF (SELECT COUNT(1) FROM #UserTimes ut WHERE ut.UserId = @userId) = 0
      INSERT INTO #UserTimes (UserId, WorkTime) VALUES (@userId, '1.1.1900')
    
    --определяем начало и конец периода для данной строки 
      SET @curEnterTime = CASE WHEN @StartTime > @enterTime THEN @StartTime ELSE @enterTime END
      SET @curExitTime = CASE WHEN @EndTime > @exitTime THEN @exitTime ELSE @EndTime END
    
    --суммируем (накапливаем) время данного юзера в #UserTimes 
      UPDATE #UserTimes SET WorkTime = WorkTime + (@curExitTime - @curEnterTime) WHERE UserId = @userId
  --считываем следующую строку курсора 
  FETCH NEXT FROM my_cur INTO @passTimeId, @userId, @enterTime, @exitTime
  END

--закрываем курсор 
CLOSE my_cur
DEALLOCATE my_cur

-- выводим результат, извлекая из дат вроде '05.01.1900 5:08:34.233' конкретное число месяцев, суток, часов и минут
SELECT u.UserId, u.LastName, u.Name, DATEPART(MONTH, ut.WorkTime) - 1 Months, DATEPART(DAY, ut.WorkTime) - 1 Days, DATEPART(hour, ut.WorkTime) Hours, DATEPART(MINUTE, ut.WorkTime) Minutes 
  FROM #UserTimes ut 
  INNER JOIN [User] u ON ut.UserId = u.UserId
GO