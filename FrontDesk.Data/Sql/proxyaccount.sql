use master
go

xp_sqlagent_proxy_account N'SET'
			, N''
                        , N'mmaxim'
                        , N'sheepi25'
go

-- retrieve the proxy account to check that it's correct.
xp_sqlagent_proxy_account N'GET'
go

-- grant database access in master
sp_grantdbaccess 'ipbased'
go

grant exec on xp_cmdshell to ipbased
go
