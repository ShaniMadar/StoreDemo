

CREATE PROCEDURE [dbo].[InsertNewPurchase]
	@date datetime =null,
	@employee int,
	@totalSum decimal(7,2),


	@paidAmount decimal(7,2),	
	@paymentMethod int

AS
	declare @status int
	declare @purchaseid int
	set @date = GETDATE()

	set @status = (case when @paidAmount = @totalSum then 2 else (case when @paidAmount = 0 then 1 else (case when @paidAmount < @totalSum then 6 end)end)end)

	insert into Purchase values (
	@date,
	@employee,
	@status,
	@totalSum,
	@paidAmount
	)

	set @purchaseid = SCOPE_IDENTITY()

	insert into PurchasePayment values (
		@purchaseid,
		@paymentMethod,
		@paidAmount
	)


select @purchaseid as purchaseId
