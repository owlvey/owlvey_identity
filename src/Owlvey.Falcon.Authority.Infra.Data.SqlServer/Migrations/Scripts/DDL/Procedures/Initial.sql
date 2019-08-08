CREATE PROCEDURE [dbo].[usp_UserCustomer_GetByQuery]   
(  
	@pUserId NVARCHAR(MAX) = null,
	@pCustomerName NVARCHAR(MAX) = null
)  
AS  
Begin  
	
	Create Table #Team
	(
		TeamId UniqueIdentifier
	)

	Insert Into #Customer
	Select TU.CustomerId 
	From CustomerMember TU With (Nolock)
	Where TU.UserId = @pUserId and TU.[Status] = 1

	Select Top 5 T.CustomerId, T.[Name], T.AdminName, T.AdminEmail
	From Customer T With (Nolock)
	Inner Join #Customer tmpT on T.CustomerId = tmpT.CustomerId 
	Where T.[Name] like '%' + IsNull(@pCustomerName, '') + '%'
	Order by T.[Name]

End
