create PROCEDURE [dbo].[GetPurchaseDetailReturns]

@id int

as

begin
	
	select * from Returns
	where PurchaseDetailId = @id

end