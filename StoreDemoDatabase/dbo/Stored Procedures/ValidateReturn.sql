
--exec [dbo].[ValidateReturn] 1039,3,77
CREATE procedure [dbo].[ValidateReturn]
	@purchaseId int,
	@returnQuantity int,
	@purchaseDetailId int
as
begin

	declare 
			@item int,
			@ValidReturn int,
			@returnPeriod int,
			@daysSincePurchase int,
			@quantity int,
			@paidAmount decimal(7,2),
			@returnAmount decimal(7,2)

	set @item = (select item from PurchaseDetails where id = @purchaseDetailId)

	set @quantity = (
		(select quantity from PurchaseDetails where
		Id = @purchaseDetailId) - 
		(select isnull(sum(Quantity),0) from Returns where
		PurchaseDetailId = @purchaseDetailId)
	)

	set @returnPeriod = (
		select returnPeriod from ItemType
		where id = (select itemType from Items where id = @item)
	)

	set @daysSincePurchase = (
	 datediff(day,(select Date from Purchase where id = @purchaseId),getdate())
	)

	set @paidAmount = (
		(select PaidAmount from Purchase where Id = @purchaseId) -
		(select isnull(sum(TotalAmount),0) from Returns where 
		PurchaseId = @purchaseId)
	)

	set @returnAmount = ((select price from Items where id = @item)*cast(@returnQuantity as decimal(7,2)))

	set @ValidReturn =  
	(case when @returnPeriod is null or @returnPeriod = 0 then 0 else (
	case when @quantity < @returnQuantity then 2 else(
	case when @paidAmount <= 0  then 3 else(
	case when @returnPeriod >= @daysSincePurchase then 4 else(
	case when @daysSincePurchase <= 30 then 5 else 1
	end)end)end)end)end)

	select @ValidReturn as ReturnMethod

end
