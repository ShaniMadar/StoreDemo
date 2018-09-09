
--exec [dbo].[GetReportPurchaseAndReturnByEmployee] '07/09/2018'

CREATE procedure [dbo].[GetReportPurchaseAndReturnByEmployee]

 @date date
As
Begin

select 
isnull(a.Employee,b.Employee) [Employee],
isnull(a.FirstName, b.Name) as [Name],
isnull(a.[Total Purchases],0)[Total Purchases],
isnull(a.[Purchases Total Sum],0)[Purchases Total Sum],
isnull(b.[Total Returns],0)[Total Returns],
isnull(b.[Returns total sum],0)[Returns total sum]
into #basic 
from(
Select 
p.Employee,
e.FirstName,
count(distinct p.id) [Total Purchases],
sum(p.PaidAmount) [Purchases Total Sum]
from purchase p inner join Employees e
on Employee = e.Id
where FORMAT(Date, 'yyyyMM') = FORMAT(@date, 'yyyyMM')
group by Employee, e.FirstName) as a

full outer join

(select 
Employee as [Employee],
e.firstName as [Name],
count(distinct r.id) [Total Returns],
sum(r.TotalAmount) [Returns total sum]
from
Returns r
inner join Employees e
on Employee = e.Id
where FORMAT(r.Date, 'yyyyMM') = FORMAT(@date, 'yyyyMM')
group by Employee, e.FirstName) as b
on a.Employee = b.Employee

select * from #basic

union
select 
9999 [Employee],
'Total' [Name],
isnull(sum([Total Purchases]),0),
isnull(sum([Purchases Total Sum]),0),
isnull(sum([Total Returns]),0),
isnull(sum([Returns total sum]),0)
from #basic

End