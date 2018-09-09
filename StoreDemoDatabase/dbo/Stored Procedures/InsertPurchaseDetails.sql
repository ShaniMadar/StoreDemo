
CREATE PROCEDURE [dbo].[InsertPurchaseDetails]
		
	@PurchaseId int,
	@item int,
	@quantity int

AS

begin

	declare @inventoryQuantity int

	insert into PurchaseDetails values (
	@PurchaseId,
	@item,
	@quantity,
	2
	)

	set @inventoryQuantity = (select quantity from Inventory where Item = @item)

	update Inventory 
	set Quantity = (@inventoryQuantity - @quantity),
	LastUpdated = GETDATE(),
	Employee = null
	where Item = @item

	select SCOPE_IDENTITY() as id

end
