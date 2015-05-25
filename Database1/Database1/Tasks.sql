--1 Выбрать всю информацию обо всех а/м.
Select * from [Car]

--2 Выбрать легковые а/м.
Select * from [Car]	where [Type] = 'Легковой'

--3 Выбрать номера а/м, в которых число мест >= 5 и упорядочить их по количеству мест.
Select c.[Number] from [Car] c
	where c.[Seats] > 5
	order by c.[Seats]

--4 Выбрать номера и фамилии водителей всех белых автомобилей  BMV, у которых в номере встречается цифра 2. Список упорядочить по фамилии водителя.
Select c.[Number], c.[Driver] from [Car] c
	where c.[Color] = 'Белый' and
	c.[Brand] = 'BMW' and
	(Position('2', c.[Number]) != 0)
	order by c.[Driver]

--5 Выдать номера автомобилей, время заказа и фамилии водителей, принимавших заказы с 12:00 до 19:00 1 сентября 2014 года.
Select c.[Number], o.[DataTime] ,c.[Driver] from [Car] c, [Order] o
	where o.[DataTime] between '2014/09/01 12:00' and '2014/09/01 19:00'

--1 Посчитать общую сумму оплаты по всем выполненным заказам
Select sum([Price]) from [Order] where [Order].[Status] = 'closed'

-- 2 Получить список водителей, отсортированный по количеству выполненных заказов
Select c.[Driver] from [Car] c, [Order] o
	where o.[Driver] = c.[Driver]
	group by o.[Status]
	order by count(o.[Status])

-- 3 Выбрать постоянных клиентов (не менее 5 выполненных заказов)
Select client.[Name] from (Select c.[Id], c.[Name], count(o.[Status]) as Orders from [Client] c, [Order] o
	where c.[Id] = o.[ClientID]
	group by c.[Id], c.[Name]) client
	where client.[Orders] >= 5

--1 Удалить клиента Сидорова.
Delete from [Client] where [Client].[Name] = 'Сидоров'

--2 Удалить клиента Сидорова и все его заказы.
Delete from [Order] where
	[Order].[ClientID] = (Select c.[Id] from [Client] c
	where c.[Name] = 'Сидоров')
Delete from [Client] where [Client].[Name] = 'Сидоров'

--3 Заменить номер автомобиля ‘C404HM78’ на ‘C405HM78’.
 Update [Car] set [Car].[Number] = 'C405HM78'
	where [Car].[Number] = 'C404HM78'