dotnet ef --startup-project ../Server/ migrations add V1.0.0 --context ScDbContext

dotnet ef --startup-project ../Server/ database update --context ScDbContext

$server = "localhost"
$username = "postgres"
$port = 5432
$db = "NacidSc"

SET PGCLIENTENCODING=utf-8
chcp 65001

psql -h $server -U $username -d $db -p $port -f "../Database/InsertScripts/1.District.pg.sql"
psql -h $server -U $username -d $db -p $port -f "../Database/InsertScripts/2.Municipality.pg.sql"
psql -h $server -U $username -d $db -p $port -f "../Database/InsertScripts/3.Settlement.pg.sql"
psql -h $server -U $username -d $db -p $port -f "../Database/InsertScripts/4.LawForm.pg.sql"
psql -h $server -U $username -d $db -p $port -f "../Database/InsertScripts/5.SmartSpecialization.pg.sql"
psql -h $server -U $username -d $db -p $port -f "../Database/InsertScripts/6.SupplierOfferingCounter.pg.sql"

$migrationFullPath = ('C:\projects\nacidsc\Migration\bin\Debug\net7.0\Migration.exe')

& $migrationFullPath
