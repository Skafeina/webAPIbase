﻿CREATE PROC TGS_SP_IncTodoItem
    @NAME		VARCHAR (100),
    @ISCOMPLETED     INT
AS
BEGIN
	INSERT INTO TODOITEMS (NAME, ISCOMPLETED) 
	VALUES (@NAME, @ISCOMPLETED)

	SELECT SCOPE_IDENTITY()
END