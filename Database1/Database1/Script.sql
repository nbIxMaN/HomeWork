
 --1.	Выбрать всю информацию обо всех самолетах.
 Select * from [Planes]

 --2.	Выбрать самолеты типа Boeing.
 Select * from [Planes] p
	where p.Type = 'Boeing'

 --3.	Выбрать номера самолетов, у которых число мест >=100 и упорядочить их по количеству мест.
 Select p.Number from [Planes] p
	where p.PlacesNumber >= 100
	order by PlacesNumber

 --4.	Выбрать номера и экипаж всех быстрых (быстрее скорости звука) самолетов, у которых в номере встречается число 19. Список упорядочить по номеру самолета.
 Select p.Number, p.Pilot from [Planes] p
	where p.Speed >= 331
	and (Position("19", p.Number) != 0)
	order by p.Number

-- 5.	Выдать номера самолетов, имя заказчика и время отлета, улетевших с 11:00 до 23:00 1 июля 2014 года.
 Select p.Number, c.Name, f.DateTime from [Planes] p, [Clients] c, [Flights] f
	where f.DateTime between '2014/06/01 11:00' and '2014/06/01 23:00'

-- 1.	Посчитать общую сумму оплаты по всем выполненным заказам
 Select sum(o.Price) from [Orders] o

-- 2.	Получить список экипажей, отсортированный по количеству выполненных заказов
Select p.Pilot from [Planes] p, [Flights] f
    where f.Plane = p.Id
	group by f.Plane
	order by count(f.Plane)

-- 3.	Выбрать постоянных клиентов (не менее 5 выполненных заказов)
select * from (select c.Id, count(o.Id) as Orders from [Clients] c, [Orders] o
		where c.Id = o.CustomerId
		group by c.Id) client 
	where client.Orders >= 5;

-- 1.	Удалить клиента Попова.
delete from [Clients]
    where Name = N'Попов'

-- 2.	Удалить клиента Гейтса и все его заказы.
delete from [Orders] 
	where [ClientID] = (select [Id] from [Clients] where [Name] = N'Гейтс');
delete from [Clients] where [SecondName] = N'Гейтс';

-- 3.	Заменить номер самолета ‘121212’ на ‘521212’.
update [Flights] set [Number] = N'521212' where [Number] = N'121212';
