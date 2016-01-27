# URLForwarding
Short URL generator and Manager. Using ASP.NET vNext.

This project will provide you a private short url service. To deploy this project, you need an ASP.NET 5 server and a sqlserver database.

Before using, you need configure your environment. To learn how to configure ASP.NET 5 environment, click [here][1] to get started.

## Restore package

After downloading source code, using cmd or terminal change directory to src/URLForwarding and then type following:

`
dnu restore
`

## appsetting.json
Now, we need provide database connection string and login configurations.

Open appsetting.json, find out:

`
Server=(localdb)\\MSSQLLocalDB;Database=_CHANGE_ME;Trusted_Connection=True;
`

Replace it with your own connections string. And then you need set password into Password attribute and set username into UserName, CookieTime attribute tell system cookie expiration time(unit is minute).

## Create Database
After all, you need generate database using sqlscript provided [here][2] or using 

`
dnx ef migrations add initialization
`
`
dnx ef database update
`

Finally, execute

`
dnx web
`

Till here, your short url service is configured. The url format is `http://{yourdomain}/?go={id}`.


[1]:http://prpr.pro/?go=97ad8
[2]:http://prpr.pro/?go=01f43
