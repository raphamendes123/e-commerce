sleep 20s


#aguardando provisionamento e start do bvanco 
  
/opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P "MeuDB@123" -i create_db_authentication.sql

/opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P "MeuDB@123" -i create_db_cart.sql

/opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P "MeuDB@123" -i create_db_catalog.sql

/opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P "MeuDB@123" -i create_db_customer.sql

/opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P "MeuDB@123" -i create_db_order.sql

/opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P "MeuDB@123" -i create_db_payment.sql
 