CREATE PROCEDURE [dbo].[GetPurchase]

@id int

as

begin
	
	select
	p.Id [PurchaseId],
	p.Date [Date],
	p.Employee [Employee],
	p.Status [Status],
	p.TotalSum [TotalSum],
	p.PaidAmount [PaidAmount],
	pd.Id [PdId],
	pd.Item [PdItem],
	pd.Quantity [PdQuantity],
	pd.Status [PdStatus]
	from Purchase p
	left join 
	PurchaseDetails pd
	on pd.PurchaseId = p.Id
	where pd.PurchaseId = @id

end