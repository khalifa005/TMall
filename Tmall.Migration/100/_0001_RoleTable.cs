using FluentMigrator;

namespace Tmall.Migration._100
{
    [Migration(1)]
    public class _0001_RoleTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table(Tables.Role)
                .IdSmall()
                .MultiText("Name")
                .Bool("IsTest", true)
                .IsActive();
        }
    }
}
