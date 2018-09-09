--exec [dbo].[GetPurchaseAndReturnReport] '09/07/2018'
CREATE procedure [dbo].[GetPurchaseAndReturnReport]

 @date date

as
begin

select 
isnull(a.Date,b.Date) [Date],
isnull(a.[Total Purchases],0)[Total Purchases],
isnull(a.[Purchases Total Sum],0)[Purchases Total Sum],
isnull(a.[Employees who completed purchases],0)[Employees who completed purchases],
isnull(b.[Total Returns],0)[Total Returns],
isnull(b.[Returns total sum],0)[Returns total sum],
isnull(b.[Employees who completed a return],0)[Employees who completed a return]
into #basic
from(
Select 
CONVERT(varchar,Date,101) as [Date],
count(distinct id) [Total Purchases],
sum(PaidAmount) [Purchases Total Sum],
count(distinct employee) [Employees who completed purchases]
from purchase 
where FORMAT(Date, 'yyyyMM') = FORMAT(@date, 'yyyyMM')
group by CONVERT(varchar,Date,101)) as a

full outer join

(select 
CONVERT(varchar,Date,101) as [Date],
count(distinct r.id) [Total Returns],
sum(r.TotalAmount) [Returns total sum],
count(distinct r.employee)[Employees who completed a return]
from
Returns r
where FORMAT(r.Date, 'yyyyMM') = FORMAT(@date, 'yyyyMM')
group by CONVERT(varchar,r.Date,101)) as b
on a.Date = b.Date

select * from #basic
union
select 
'Total' as [Date],
isnull(sum([Total Purchases]),0),
isnull(sum([Purchases Total Sum]),0),
(select count(distinct employee) from Purchase where FORMAT(Date, 'yyyyMM') = FORMAT(@date, 'yyyyMM')) as [Employees who completed purchases],
isnull(sum([Total Returns]),0),
isnull(sum([Returns total sum]),0),
(select count(distinct employee) from Returns where FORMAT(Date, 'yyyyMM') = FORMAT(@date, 'yyyyMM')) as [Employees who completed a return]
from #basic

end