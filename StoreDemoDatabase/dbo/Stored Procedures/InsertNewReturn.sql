

--exec [dbo].[InsertNewReturn]  7,1,2,2
CREATE PROCEDURE [dbo].[InsertNewReturn]
	@purchaseDetailId int,
	@quantity int,
	@Employee int,
	@CreditMethod int
AS
begin
	declare @totalAmount decimal(7,2),
	@ItemPrice float,
	@itemId int,
	@returnId int,
	@purchaseId int,
	@TotalPayment decimal(7,2),
	@returnedAmount decimal(7,2),
	@purchaseUpdateStatus int,
	@purchaseDetailUpdateStatus int,
	@inventoryQuantity int
	
	set @itemId = (select item from PurchaseDetails where id=@purchaseDetailId)

	set @ItemPrice = (select price from Items where id = @itemId)
	set @totalAmount = (@ItemPrice * cast(@quantity as float))

	set @purchaseId = (select purchaseId from PurchaseDetails where id = @purchaseDetailId)

	insert into Returns values(
	@totalAmount,
	@CreditMethod,
	@Employee,
	GETDATE(),
	@purchaseDetailId,
	@quantity,
	@purchaseId
	)

	set @returnId  = SCOPE_IDENTITY()

	
	set @TotalPayment = (select PaidAmount from Purchase where id = @purchaseId)
	--set @returnedAmount = (select sum(TotalAmount) from Returns r left join PurchaseDetails p on r.PurchaseDetailId = p.Id  where p.PurchaseId = @PurchaseId) 
	set @purchaseUpdateStatus = (case when (@TotalPayment - @returnedAmount)<=0 then 4 else 5 end)
	set @purchaseDetailUpdateStatus = (case when ((select quantity from PurchaseDetails where id = @purchaseDetailId) - @returnedAmount)<=0 then 4 else 5 end)

	update Purchase
	set Status = @purchaseUpdateStatus,
	PaidAmount = (@TotalPayment - @totalAmount)
	where id = @purchaseId

	update PurchaseDetails
	set Status = @purchaseDetailUpdateStatus
	where id = @purchaseDetailId

	set @inventoryQuantity = (select quantity from Inventory where Item = @itemId) + @quantity
	
	update Inventory
	set Quantity = @inventoryQuantity, LastUpdated = GETDATE(), Employee = null
	where Item = @itemId

	select @returnId as returnId

end
