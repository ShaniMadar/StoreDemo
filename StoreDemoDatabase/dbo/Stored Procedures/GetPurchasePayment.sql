create PROCEDURE [dbo].[GetPurchasePayment]

@id int

as

begin
	
	select * from PurchasePayment
	where PurchaseId = @id

end