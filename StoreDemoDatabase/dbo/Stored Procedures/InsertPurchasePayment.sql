



CREATE PROCEDURE [dbo].[InsertPurchasePayment]
	@purchaseId int,
	@paymentMethod int,
	@sum decimal(7,2)

AS
	declare @status int
	declare @paid decimal(7,2)
	declare @totalSum decimal(7,2)
	declare @paidAmount decimal(7,2)

	set @paid = (select p.PaidAmount from Purchase p where p.id = @purchaseId)
	set @totalSum = (select p.TotalSum from Purchase p where p.Id = @purchaseId)
	set @paidAmount = @paid+@sum

	insert into PurchasePayment values (
		@purchaseid,
		@paymentMethod,
		@sum
	)

	set @status = (case when @paidAmount >= @totalSum then 2 else (case when @paidAmount = 0 then 1 else (case when @paidAmount < @totalSum then 6 end)end)end)

	update Purchase 
	set Purchase.Status = @status, Purchase.PaidAmount = @paidAmount
	where Purchase.Id = @purchaseId


select @status as status
